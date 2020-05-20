using System.Linq;
using System.Text.RegularExpressions;
using Docx.DataModel;

namespace Docx.Processors.Searching
{
    internal static class EngineConfigExtensions
    {
        private const string anyText = ".*?";

        public static Token CreateOpeningToken(this EngineConfig engineConfig, Group match, int paragraphIndex)
        {
            ModelDescription modelDescription;
            if (engineConfig.IsArrayToken(match.Value))
            {
                modelDescription = match.Value.ToCollectionModelDescription(engineConfig);
                return Token.CollectionBegin(modelDescription, match.Index, paragraphIndex);
            }

            if (engineConfig.IsConditionToken(match.Value))
            {
                modelDescription = match.Value.ToConditionModelDescription(engineConfig);
                return Token.ConditionBegin(modelDescription, match.Index, paragraphIndex);
            }

            modelDescription = match.Value.ToSingleValueModelDescription(engineConfig);
            return Token.SingleValue(modelDescription, match.Index, paragraphIndex);
        }

        public static Token CreateClosingToken(this EngineConfig engineConfig, Group match, int paragraphIndex)
        {
            if (engineConfig.IsArrayToken(match.Value))
            {
                var description = match.Value.ToCollectionModelDescription(engineConfig);
                return Token.CollectionEnd(description, match.Index, paragraphIndex);
            }

            if (engineConfig.IsConditionToken(match.Value))
            {
                var description = match.Value.ToConditionModelDescription(engineConfig);
                return Token.ConditionEnd(description, match.Index, paragraphIndex);
            }

            throw new System.Exception("The match is not any of closing tokens");
        }

        public static string OpeningTokenRegexPattern(this EngineConfig engineConfig)
        {
            return $"^{anyText}"
                + engineConfig.SimpleValueRegexPattern().ToRegexGroup()
                //+ "|"
                //+ engineConfig.ArrayOpenRegexPattern()
                //+ "|"
                //+ engineConfig.ConditionOpenRegexPattern()
                + $"{anyText}$";
        }

        public static bool IsTemplateToken(this EngineConfig engineConfig, string token)
        {
            return Regex.IsMatch(token, engineConfig.ArrayOpenRegexPattern())
                || Regex.IsMatch(token, engineConfig.ConditionOpenRegexPattern());
        }

        public static bool IsArrayToken(this EngineConfig engineConfig, string match)
        {
            return Regex.IsMatch(match, engineConfig.ArrayOpenRegexPattern());
        }

        public static bool IsConditionToken(this EngineConfig engineConfig, string token)
        {
            return Regex.IsMatch(token, engineConfig.ConditionOpenRegexPattern());
        }

        public static (string name, string parameters) SplitToken(this EngineConfig engineConfig, string token)
        {
            if (engineConfig.IsArrayToken(token))
            {
                return (token.Cut(engineConfig.Array.Open.Length, engineConfig.Array.Close.Length), string.Empty);
            }

            if (engineConfig.IsConditionToken(token))
            {
                return (token.Cut(engineConfig.Condition.Begin.Length, engineConfig.Condition.End.Length), string.Empty);
            }

            return token
                .Cut(engineConfig.Placeholder.Start.Length, engineConfig.Placeholder.End.Length)
                .SplitBy(engineConfig.Placeholder.ParametersDelimiter);
        }

        private static string SimpleValueRegexPattern(this EngineConfig engineConfig)
        {
            return $"{engineConfig.Placeholder.Start.Escape()}{anyText}{engineConfig.Placeholder.End.Escape()}";
        }

        private static string ArrayOpenRegexPattern(this EngineConfig engineConfig)
        {
            return $"{engineConfig.Placeholder.Start.Escape()}{anyText}{engineConfig.Array.Open.Escape()}{engineConfig.Placeholder.End.Escape()}"
                .ToRegexGroup();
        }

        private static string ArrayCloseRegexPattern(this EngineConfig engineConfig)
        {
            return $"{engineConfig.Placeholder.Start.Escape()}{engineConfig.Array.Close.Escape()}{anyText}{engineConfig.Placeholder.End.Escape()}"
                .ToRegexGroup();
        }

        private static string ConditionOpenRegexPattern(this EngineConfig engineConfig)
        {
            return $"{engineConfig.Placeholder.Start.Escape()}{anyText}{engineConfig.Condition.Begin.Escape()}{engineConfig.Placeholder.End.Escape()}"
                .ToRegexGroup();
        }

        private static string ConditionCloseRegexPattern(this EngineConfig engineConfig)
        {
            return $"{engineConfig.Placeholder.Start.Escape()}{engineConfig.Condition.End.Escape()}.{engineConfig.Placeholder.End.Escape()}"
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

        private static ModelDescription ToCollectionModelDescription(this string token, EngineConfig engineConfig)
        {
            var segments = token
                .Cut(engineConfig.Array.Open.Length, engineConfig.Array.Close.Length)
                .Split(engineConfig.Placeholder.NamesDelimiter);

            return new ModelDescription(segments, token);
        }

        private static ModelDescription ToConditionModelDescription(this string token, EngineConfig engineConfig)
        {
            var segments = token
                .Cut(engineConfig.Condition.Begin.Length, engineConfig.Condition.End.Length)
                .Split(engineConfig.Placeholder.NamesDelimiter);

            return new ModelDescription(segments, token);
        }

        private static ModelDescription ToSingleValueModelDescription(this string token, EngineConfig engineConfig)
        {
            var (name, parameters) = token
                .Cut(engineConfig.Placeholder.Start.Length, engineConfig.Placeholder.End.Length)
                .SplitBy(engineConfig.Placeholder.ParametersDelimiter);

            var segments = name
                .Split(engineConfig.Placeholder.NamesDelimiter);

            return new ModelDescription(segments, parameters, token);
        }
    }
}
