using System.Diagnostics;
using Docx.DataModel;

namespace Docx.Processors.Searching
{
    [DebuggerDisplay("{ModelDescription}({Position})")]
    internal class Token
    {
        public static readonly Token None = new Token(TokenType.None, ModelDescription.Empty, TokenPosition.None);

        private Token(
            TokenType tokenType,
            ModelDescription modelDescription,
            TokenPosition position)
        {
            this.TokenType = tokenType;
            this.ModelDescription = modelDescription;
            this.Position = position;
        }

        public TokenType TokenType { get; }
        public ModelDescription ModelDescription { get; }
        public TokenPosition Position { get; }

        public static Token SingleValue(ModelDescription modelDescription, TokenPosition position)
        {
            return new Token(TokenType.SingleValue, modelDescription, position);
        }

        public static Token CollectionBegin(ModelDescription modelDescription, TokenPosition position)
        {
            return new Token(TokenType.CollectionBegin, modelDescription, position);
        }

        public static Token CollectionEnd(ModelDescription modelDescription, TokenPosition position)
        {
            return new Token(TokenType.CollectionEnd, modelDescription, position);
        }

        public static Token ConditionBegin(ModelDescription modelDescription, TokenPosition position)
        {
            return new Token(TokenType.ConditionBegin, modelDescription, position);
        }

        public static Token ConditionEnd(ModelDescription modelDescription, TokenPosition position)
        {
            return new Token(TokenType.ConditionEnd, modelDescription, position);
        }
    }
}
