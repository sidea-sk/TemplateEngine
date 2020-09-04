using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Docx.Processors.Searching
{
    internal static class ParagraphSearch
    {
        public static Template FindNextTemplate(
            this IReadOnlyCollection<Paragraph> paragraphs,
            int firstParagraphStartTextIndex,
            EngineConfig config,
            int tableRowIndex = -1,
            int tableCellIndex = -1,
            bool simpleValue = true,
            bool array = true,
            bool condition = true)
        {
            var pattern = config.OpeningTokenRegexPattern(simpleValue: simpleValue, array: array, condition: condition);

            for (var i = 0; i < paragraphs.Count; i++)
            {
                var textIndexOffset = i == 0
                    ? firstParagraphStartTextIndex
                    : 0;

                var text = paragraphs.ElementAt(i).InnerText.Substring(textIndexOffset);
                var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    continue;
                }

                var token = config.CreateOpeningToken(match.Groups[1], i, textIndexOffset, tableRowIndex, tableCellIndex);
                switch (token.TokenType)
                {
                    case TokenType.SingleValue:
                        return new SingleValueTemplate(token);
                    case TokenType.CollectionBegin:
                        {
                            var closeToken = paragraphs.FindCloseToken(token, config, continueAfterOpenToken: true, tableRowIndex, tableCellIndex);
                            var elementTemplate = paragraphs.GetTemplate(token, closeToken);
                            return new ArrayTemplate(token, closeToken, elementTemplate);
                        }
                    case TokenType.ConditionBegin:
                        {
                            var closeToken = paragraphs.FindCloseToken(token, config, continueAfterOpenToken: true);
                            return new ConditionTemplate(token, closeToken);
                        }
                    default:
                        throw new System.Exception("unexpected token");
                }
            }

            return Template.Empty;
        }

        public static Token FindCloseToken(
            this IReadOnlyCollection<Paragraph> paragraphs,
            Token openToken,
            EngineConfig config,
            bool continueAfterOpenToken,
            int tableRowIndex = -1,
            int tableCellIndex = -1)
        {
            // var openPattern = config.OpeningTokenRegexPattern(openToken);
            var closePattern = config.ClosingTokenRegexPattern(openToken);

            for (var i = 0; i < paragraphs.Count; i++)
            {
                if(i < openToken.Position.ParagraphIndex)
                {
                    continue;
                }

                string text;
                int sameParagraphTextIndex;
                if(continueAfterOpenToken && i == openToken.Position.ParagraphIndex)
                {
                    text = paragraphs.ElementAt(i).InnerText.Substring(openToken.Position.TextIndex + openToken.ModelDescription.OriginalText.Length);
                    sameParagraphTextIndex = openToken.Position.TextIndex;
                }
                else
                {
                    text = paragraphs.ElementAt(i).InnerText;
                    sameParagraphTextIndex = 0;
                }

                var closeMatch = Regex.Match(text, closePattern, RegexOptions.IgnoreCase);
                if (closeMatch.Success)
                {
                    return config.CreateClosingToken(
                        closeMatch.Groups[1],
                        sameParagraphTextIndex,
                        i,
                        tableRowIndex,
                        tableCellIndex);
                }
            }

            return Token.None;
        }

        private static OpenXmlTemplate GetTemplate(this IReadOnlyCollection<Paragraph> paragraphs, Token startToken, Token endToken)
        {
            if(endToken.Position.ParagraphIndex == -1)
            {
                return OpenXmlTemplate.Empty;
            }

            var startParagraph = paragraphs.ElementAt(startToken.Position.ParagraphIndex);
            var endParagraph = paragraphs.ElementAt(endToken.Position.ParagraphIndex);

            var e = startParagraph.NextSibling();
            var templateElements = new List<OpenXmlElement>();
            while(e != null && e != endParagraph)
            {
                templateElements.Add(e.CloneNode(true));
                e = e.NextSibling();
            }

            return new OpenXmlTemplate(new Run[0], templateElements, new Run[0]);
        }
    }
}
