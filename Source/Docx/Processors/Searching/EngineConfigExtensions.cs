using System.Collections.Generic;
using System.Text.RegularExpressions;
using Docx.DataModel;

namespace Docx.Processors.Searching
{
    internal static class EngineConfigExtensions
    {
        private const string _anyText = ".*?";

        public static Token CreateOpeningToken(
            this EngineConfig engineConfig,
            Group match,
            int paragraphIndex,
            int textIndexOffset,
            int tableRowIndex,
            int tableCellIndex)
        {
            ModelDescription modelDescription;
            TokenPosition tokenPosition;

            if (engineConfig.IsArrayOpenToken(match.Value))
            {
                modelDescription = match.Value.ToOpenCollectionModelDescription(engineConfig);
                tokenPosition = new TokenPosition(paragraphIndex, match.Index + textIndexOffset, tableRowIndex, tableCellIndex);
                return Token.CollectionBegin(modelDescription, tokenPosition);
            }

            if (engineConfig.IsConditionBeginToken(match.Value))
            {
                modelDescription = match.Value.ToOpenConditionModelDescription(engineConfig);
                tokenPosition = new TokenPosition(paragraphIndex, match.Index + textIndexOffset, tableRowIndex, tableCellIndex);
                return Token.ConditionBegin(modelDescription, tokenPosition);
            }

            modelDescription = match.Value.ToSingleValueModelDescription(engineConfig);
            tokenPosition = new TokenPosition(paragraphIndex, match.Index + textIndexOffset);
            return Token.SingleValue(modelDescription, tokenPosition);
        }

        public static Token CreateClosingToken(
            this EngineConfig engineConfig,
            Group match,
            int textIndexOffset,
            int paragraphIndex,
            int tableRowIndex,
            int tableCellIndex)
        {
            var position = new TokenPosition(paragraphIndex, match.Index + textIndexOffset, tableRowIndex, tableCellIndex);

            if (engineConfig.IsArrayCloseToken(match.Value))
            {
                var description = match.Value.ToCloseCollectionModelDescription(engineConfig);
                return Token.CollectionEnd(description, position);
            }

            if (engineConfig.IsConditionEndToken(match.Value))
            {
                var description = match.Value.ToCloseConditionModelDescription(engineConfig);
                return Token.ConditionEnd(description, position);
            }

            throw new System.Exception("The match is not any of closing tokens");
        }

        public static string OpeningTokenRegexPattern(
            this EngineConfig engineConfig,
            bool simpleValue,
            bool array,
            bool condition)
        {
            var temp = new List<string>();
            if (simpleValue)
            {
                temp.Add(engineConfig.SimpleValueRegexPattern());
            }
            if (array)
            {
                temp.Add(engineConfig.ArrayOpenRegexPattern(_anyText));
            }

            if (condition)
            {
                temp.Add(engineConfig.ConditionOpenRegexPattern(_anyText));
            }

            return $"^{_anyText}"
                + string.Join("|", temp).ToRegexGroup()
                + $"{_anyText}$";
        }

        public static string OpeningTokenRegexPattern(this EngineConfig engineConfig, Token openingToken)
        {
            var expression = openingToken.ModelDescription.Expression
                .ToExpressionString(engineConfig.Placeholder.NamesDelimiter)
                .Escape();

            switch (openingToken.TokenType)
            {
                case TokenType.CollectionBegin:
                    return engineConfig.ArrayOpenRegexPattern(expression);
                case TokenType.ConditionBegin:
                    return engineConfig.ConditionCloseRegexPattern(expression);
                default:
                    throw new System.Exception("not supported");
            }
        }

        public static string ClosingTokenRegexPattern(this EngineConfig engineConfig, Token openingToken)
        {
            var expression = openingToken.ModelDescription.Expression
                .ToExpressionString(engineConfig.Placeholder.NamesDelimiter)
                .Escape();

            switch (openingToken.TokenType)
            {
                case TokenType.CollectionBegin:
                    return engineConfig.ArrayCloseRegexPattern(expression);
                case TokenType.ConditionBegin:
                    return engineConfig.ConditionCloseRegexPattern(expression);
                default:
                    throw new System.Exception("not supported");
            }
        }

        private static bool IsArrayOpenToken(this EngineConfig engineConfig, string match)
        {
            return Regex.IsMatch(match, engineConfig.ArrayOpenRegexPattern(_anyText));
        }

        private static bool IsArrayCloseToken(this EngineConfig engineConfig, string match)
        {
            return Regex.IsMatch(match, engineConfig.ArrayCloseRegexPattern(_anyText));
        }

        private static bool IsConditionBeginToken(this EngineConfig engineConfig, string token)
        {
            return Regex.IsMatch(token, engineConfig.ConditionOpenRegexPattern(_anyText));
        }

        private static bool IsConditionEndToken(this EngineConfig engineConfig, string token)
        {
            return Regex.IsMatch(token, engineConfig.ConditionCloseRegexPattern(_anyText));
        }

        private static string SimpleValueRegexPattern(this EngineConfig engineConfig)
        {
            return $"{engineConfig.Placeholder.Start.Escape()}{_anyText}{engineConfig.Placeholder.End.Escape()}";
        }

        private static string ArrayOpenRegexPattern(this EngineConfig engineConfig, string tokenRegex)
        {
            return $"{engineConfig.Placeholder.Start.Escape()}{tokenRegex}{engineConfig.Array.Open.Escape()}{engineConfig.Placeholder.End.Escape()}"
                .ToRegexGroup();
        }

        private static string ArrayCloseRegexPattern(this EngineConfig engineConfig, string tokenRegex)
        {
            return $"{engineConfig.Placeholder.Start.Escape()}{engineConfig.Array.Close.Escape()}{tokenRegex}{engineConfig.Placeholder.End.Escape()}"
                .ToRegexGroup();
        }

        private static string ConditionOpenRegexPattern(this EngineConfig engineConfig, string tokenRegex)
        {
            var pattern = $"{engineConfig.Placeholder.Start.Escape()}{tokenRegex}{engineConfig.Condition.Begin.Escape()}{engineConfig.Placeholder.End.Escape()}";
            return pattern
                .ToRegexGroup();
        }

        private static string ConditionCloseRegexPattern(this EngineConfig engineConfig, string tokenRegex)
        {
            return $"{engineConfig.Placeholder.Start.Escape()}{engineConfig.Condition.End.Escape()}{tokenRegex}{engineConfig.Placeholder.End.Escape()}"
                .ToRegexGroup();
        }

        private static string ToRegexGroup(this string pattern)
        {
            return $"({pattern})";
        }

        private static string Escape(this string str)
        {
            return Regex.Escape(str);
        }

        private static string Cut(this string value, int fromBegin, int fromEnd)
        {
            return value.Substring(fromBegin, value.Length - fromBegin - fromEnd);
        }

        private static (string, string) SplitBy(this string value, string delimiter)
        {
            var i = value.IndexOf(delimiter);
            if(i == -1)
            {
                return (value, string.Empty);
            }

            return (value.Substring(0, i), value.Substring(i + 1));
        }

        private static ModelDescription ToOpenCollectionModelDescription(this string token, EngineConfig engineConfig)
        {
            var (segments, _) = token.SplitToSegmentsAndParameters(engineConfig.Placeholder, engineConfig.Array, true);
            return new ModelDescription(segments, token);
        }

        private static ModelDescription ToCloseCollectionModelDescription(this string token, EngineConfig engineConfig)
        {
            var (segments, _) = token.SplitToSegmentsAndParameters(engineConfig.Placeholder, engineConfig.Array, false);
            return new ModelDescription(segments, token);
        }

        private static ModelDescription ToOpenConditionModelDescription(this string token, EngineConfig engineConfig)
        {
            var (segments, _) = token.SplitToSegmentsAndParameters(engineConfig.Placeholder, engineConfig.Condition, true);
            return new ModelDescription(segments, token);
        }

        private static ModelDescription ToCloseConditionModelDescription(this string token, EngineConfig engineConfig)
        {
            var (segments, _) = token.SplitToSegmentsAndParameters(engineConfig.Placeholder, engineConfig.Condition, false);
            return new ModelDescription(segments, token);
        }

        private static ModelDescription ToSingleValueModelDescription(this string token, EngineConfig engineConfig)
        {
            var (segments, parameters) = token.SplitToSegmentsAndParameters(engineConfig.Placeholder, engineConfig.Placeholder, true);
            return new ModelDescription(segments, parameters, token);
        }

        private static (string[] nameSemgents, string parameters) SplitToSegmentsAndParameters(
            this string token,
            PlaceholderConfig placeholderConfig,
            ITemplateConfig templateConfig,
            bool isOpen)
        {
            var prefixLength = placeholderConfig.Start.Length;
            if (!isOpen)
            {
                prefixLength += templateConfig.ClosePrefix.Length;
            }
            var suffixLength = placeholderConfig.End.Length;
            if (isOpen)
            {
                suffixLength += templateConfig.OpenSuffix.Length;
            }

            var (name, parameters) = token
                .Cut(prefixLength, suffixLength)
                .SplitBy(placeholderConfig.ParametersDelimiter);

            var segments = name.Split(placeholderConfig.NamesDelimiter);
            return (segments, parameters);
        }
    }
}
