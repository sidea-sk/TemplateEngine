using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Docx.DataModel;
using Docx.Processors.Searching;

namespace Docx.Processors
{
    internal static class ParagraphTools
    {
        public static int ReplaceToken(
            this Paragraph paragraph,
            Token token,
            Model model,
            IImageProcessor imageProcessor)
        {
            var runs = paragraph.Runs().ToArray();

            var (startRunIndex, endRunIndex) = runs.FindIndeces(token.Position.TextIndex, token.ModelDescription.OriginalText.Length);

            var affectedRuns = runs
                .Skip(startRunIndex)
                .Take(endRunIndex - startRunIndex + 1)
                .ToArray();

            var startRun = affectedRuns.First();
            var endRun = affectedRuns.Last();

            var previousRunsTextLength = runs
                .Take(startRunIndex)
                .TextLength();

            if (startRun != endRun)
            {
                var replaceLength = token.ModelDescription.OriginalText.Length
                    - previousRunsTextLength
                    - affectedRuns.Take(affectedRuns.Length - 1).TextLength()
                    + token.Position.TextIndex
                    ;

                endRun.ReplaceText(0, replaceLength, string.Empty);
                var firstText = endRun.Childs<Text>().FirstOrDefault();
                if(firstText != null)
                {
                    firstText.Space = SpaceProcessingModeValues.Preserve;
                }
            }

            var replaceFromIndex = token.Position.TextIndex - previousRunsTextLength;
            string replacement;

            int replacementLength;
            switch (model)
            {
                case ImageModel im:
                    replacementLength = 0;
                    startRun.ReplaceText(replaceFromIndex, token.ModelDescription.OriginalText.Length, string.Empty);
                    startRun.SplitIntoTwoRuns(replaceFromIndex);

                    var imageRun = imageProcessor.AddImage(im, token.ModelDescription.Parameters);
                    startRun.InsertAfterSelf(imageRun);
                    break;
                default:
                    replacement = model.FormattedValue() ?? string.Empty;
                    replacementLength = replacement.Length;
                    startRun.ReplaceText(replaceFromIndex, token.ModelDescription.OriginalText.Length, model.FormattedValue());
                    break;
            }

            affectedRuns
                .Skip(1)
                .Take(affectedRuns.Length - 2)
                .RemoveSelfFromParent();

            return token.Position.TextIndex + replacementLength;
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

        private static void ReplaceText(this Run run, int fromIndex, int length, string replacement)
        {
            if (length <= 0)
            {
                return;
            }

            var aggregatedTextLength = 0;
            foreach(var t in run.Childs<Text>().ToArray())
            {
                if (aggregatedTextLength + t.Text.Length - 1 < fromIndex)
                {
                    aggregatedTextLength += t.Text.Length;
                    continue;
                }

                var prefixText = t.Text.Substring(0, fromIndex - aggregatedTextLength);
                var tailText = fromIndex + length < t.Text.Length
                        ? t.Text.Substring(fromIndex + length - aggregatedTextLength)
                        : string.Empty;

                t.Text = prefixText + replacement + tailText;
                break;
            }
        }

        private static void SplitIntoTwoRuns(this Run run, int atTextIndex)
        {
            if(run.InnerText.Length - 1 == atTextIndex)
            {
                return;
            }

            var aggregatedTextLength = 0;
            Text splittingText = null;
            int splitAtTextIndex = -1;

            foreach(var t in run.Childs<Text>().ToArray())
            {
                if(t.Text.Length + aggregatedTextLength - 1 < atTextIndex)
                {
                    aggregatedTextLength += t.Text.Length;
                    continue;
                }

                splittingText = t;
                splitAtTextIndex = atTextIndex - aggregatedTextLength;
                break;
            }

            if(splittingText == null)
            {
                return;
            }

            var tail = new Text(splittingText.Text.Substring(splitAtTextIndex));
            if(tail.Text.Length > 0 && char.IsWhiteSpace(tail.Text[0]))
            {
                tail.Space = SpaceProcessingModeValues.Preserve;
            }

            splittingText.Text = splittingText.Text.Substring(0, splitAtTextIndex);
            if(splittingText.Text.Length > 0 && char.IsWhiteSpace(splittingText.Text.Last()))
            {
                splittingText.Space = SpaceProcessingModeValues.Preserve;
            }

            var newRun = new Run();
            if(run.RunProperties != null)
            {
                newRun.RunProperties = (RunProperties)run.RunProperties.CloneNode(true);
            }

            var childsToReplace = run
                .ChildElements
                .Where(c => !(c is RunProperties))
                .ItemsAfter(splittingText)
                .ToArray();

            var clones = childsToReplace
                .Select(c => c.CloneNode(true))
                .Reverse()
                .ToArray();

            childsToReplace.RemoveSelfFromParent();

            newRun.InsertAt(tail, 0);
            clones.InsertSelfAfter(tail);

            run.InsertAfterSelf(newRun);
        }
    }
}
