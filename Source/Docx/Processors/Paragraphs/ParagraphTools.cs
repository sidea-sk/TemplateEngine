using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using Docx.DataModel;
using Docx.Processors.Searching;

namespace Docx.Processors.Paragraphs
{
    internal static class ParagraphTools
    {
        public static int ReplaceToken(
            this Paragraph paragraph,
            Token token,
            Model model)
        {
            var runs = paragraph.Runs().ToArray();

            var (startRunIndex, endRunIndex) = runs.FindIndeces(token.TextIndex, token.ModelDescription.OriginalText.Length);

            var affectedRuns = runs
                .Skip(startRunIndex)
                .Take(endRunIndex - startRunIndex + 1)
                .ToArray();

            var startRun = affectedRuns.First();
            var endRun = affectedRuns.Last();

            var previousRunsTextLength = runs
                .Take(startRunIndex)
                .TextLength();

            var startFromIndex = token.TextIndex - previousRunsTextLength;
            var startToIndex = token.TextIndex + token.ModelDescription.OriginalText.Length - previousRunsTextLength;

            var replacement = model.FormattedValue();
            var toRemoveRunCount = affectedRuns.Length - 2;
            if (startRun != endRun)
            {
                var replacedTextEndIndex = token.TextIndex + token.ModelDescription.OriginalText.Length - 1
                    - previousRunsTextLength
                    - affectedRuns.Take(affectedRuns.Length - 1).TextLength();

                endRun.ReplaceText(0, replacedTextEndIndex, string.Empty);
                if (string.IsNullOrWhiteSpace(endRun.InnerText))
                {
                    toRemoveRunCount += 1;
                }
            }

            startRun.ReplaceText(startFromIndex, startToIndex, replacement);

            affectedRuns
                .Skip(1)
                .Take(toRemoveRunCount)
                .RemoveSelfFromParent();

            return token.TextIndex + replacement.Length;
        }

        private static (int startRun, int endRun) FindIndeces(this IEnumerable<Run> runs, int tokenStartTextIndex, int tokenLength)
        {
            var startIndex = -1;
            var endIndex = -1;

            var lastTextIndex = -1;
            var index = 0;
            foreach(var run in runs)
            {
                lastTextIndex += run.InnerText.Length;
                if (startIndex == -1 && lastTextIndex >= tokenStartTextIndex)
                {
                    startIndex = index;
                }

                if (endIndex == -1 && lastTextIndex >= tokenStartTextIndex + tokenLength - 1)
                {
                    endIndex = index;
                }

                if (startIndex > -1 && endIndex > -1)
                {
                    return (startIndex, endIndex);
                }

                index++;
            }

            throw new System.Exception("Seek run index out of range");
        }

        private static int TextLength(this IEnumerable<Run> runs)
        {
            var lastTextIndex = runs.Sum(r => r.InnerText.Length);
            return lastTextIndex;
        }

        private static void ReplaceText(this Run run, int fromIndex, int toIndex, string replacement)
        {
            var previousTextLastIndex = 0;

            foreach(var t in run.Childs<Text>().ToArray())
            {
                if(previousTextLastIndex + t.Text.Length < fromIndex)
                {
                    previousTextLastIndex += t.Text.Length;
                    continue;
                }

                var prefixText = t.Text.Substring(0, fromIndex - previousTextLastIndex);
                var tailText = toIndex < t.Text.Length - 1
                    ? t.Text.Substring(toIndex - previousTextLastIndex)
                    : string.Empty;

                t.Text = prefixText + replacement + tailText;
                break;
            }
        }
    }
}
