using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
                            var closeToken = paragraphs.FindCloseToken(token);
                            return new ArrayTemplate(token, closeToken);
                        }
                    case TokenType.ConditionBegin:
                        {
                            var closeToken = paragraphs.FindCloseToken(token);
                            return new ConditionTemplate(token, closeToken);
                        }
                    default:
                    //case TokenType.None:
                    //case TokenType.Unknown:
                    //    return Template.Empty;
                    //case TokenType.CollectionEnd:
                    //case TokenType.ConditionEnd:
                        throw new System.Exception("unexpected token");
                }
            }

            return Template.Empty;
        }

        private static Token FindCloseToken(
            this IReadOnlyCollection<Paragraph> paragraphs,
            Token openToken)
        {
            return Token.None;
        }
    }
}
