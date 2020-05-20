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

            var (startRunIndex, endRunIndex) = runs.FindIndeces(token.TextIndex, token.TextIndex + token.ModelDescription.OriginalText.Length);

            var affectedRuns = runs
                .Skip(startRunIndex)
                .Take(endRunIndex - startRunIndex + 1)
                .ToArray();

            var startRun = runs.First();
            var endRun = runs.Last();

            var previousRunsLastTextIndex = runs
                .Take(startRunIndex - 1)
                .LastTextIndex();

            var startFromIndex = token.TextIndex - previousRunsLastTextIndex;
            var startToIndex = token.TextIndex + token.ModelDescription.OriginalText.Length - previousRunsLastTextIndex;

            var replacement = model.FormattedValue();
            startRun.ReplaceText(startFromIndex, startToIndex, replacement);

            runs.Skip(startRunIndex)
                .Take(endRunIndex - startRunIndex - 1)
                .LastTextIndex();

            if (startRunIndex != endRunIndex)
            {
                // remove text from last run
            }

            return token.TextIndex + replacement.Length;
        }

        private static (int startRun, int endRun) FindIndeces(this IEnumerable<Run> runs, int tokenStartTextIndex, int tokenEndIndex)
        {
            var startIndex = -1;
            var endIndex = -1;

            var textIndex = 0;
            var index = 0;
            foreach(var run in runs)
            {
                if(textIndex + run.InnerText.Length >= tokenStartTextIndex)
                {
                    startIndex = index;
                }

                if(textIndex + run.InnerText.Length >= tokenEndIndex)
                {
                    endIndex = index;
                }

                textIndex += run.InnerText.Length;

                if(startIndex > -1 && endIndex > -1)
                {
                    return (startIndex, endIndex);
                }
            }

            throw new System.Exception("Seek run index out of range");
        }

        private static int LastTextIndex(this IEnumerable<Run> runs)
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
                var tailText = toIndex < t.Text.Length
                    ? t.Text.Substring(toIndex - previousTextLastIndex)
                    : string.Empty;

                t.Text = prefixText + replacement + tailText;
                break;
            }
        }
    }
}
