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
            EngineConfig config)
        {
            var pattern = config.OpeningTokenRegexPattern();

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

                var token = config.CreateOpeningToken(match.Groups[1], i, textIndexOffset);
                switch (token.TokenType)
                {
                    case TokenType.SingleValue:
                        return new SingleValueTemplate(token);
                    case TokenType.CollectionBegin:
                        {
                            var closeToken = paragraphs.FindCloseToken(token, config);
                            var elementTemplate = paragraphs.GetTemplate(token, closeToken);
                            return new ArrayTemplate(token, closeToken, elementTemplate);
                        }
                    case TokenType.ConditionBegin:
                        {
                            var closeToken = paragraphs.FindCloseToken(token, config);
                            return new ConditionTemplate(token, closeToken);
                        }
                    default:
                        throw new System.Exception("unexpected token");
                }
            }

            return Template.Empty;
        }

        private static Token FindCloseToken(
            this IReadOnlyCollection<Paragraph> paragraphs,
            Token openToken,
            EngineConfig config)
        {
            var pattern = config.ClosingTokenRegexPattern(openToken);
            for (var i = 0; i < paragraphs.Count; i++)
            {
                var text = paragraphs.ElementAt(i).InnerText;
                var match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    continue;
                }

                return config.CreateClosingToken(match.Groups[1], i);
            }

            return Token.None;
        }

        private static OpenXmlTemplate GetTemplate(this IReadOnlyCollection<Paragraph> paragraphs, Token startToken, Token endToken)
        {
            var startParagraph = paragraphs.ElementAt(startToken.ParagraphIndex);
            var endParagraph = paragraphs.ElementAt(endToken.ParagraphIndex);

            var e = startParagraph.NextSibling();
            var templateElements = new List<OpenXmlElement>();
            while(e != endParagraph)
            {
                templateElements.Add(e.CloneNode(true));
                e = e.NextSibling();
            }

            return new  OpenXmlTemplate(new Run[0], templateElements, new Run[0]);
        }
    }
}
