using System.Diagnostics;
using Docx.DataModel;

namespace Docx.Processors.Searching
{
    [DebuggerDisplay("{ModelDescription}({ParagraphIndex}|{TextIndex})")]
    internal class Token
    {
        public static readonly Token None = new Token(TokenType.None, ModelDescription.Empty, -1, -1, -1, -1);

        private Token(
            TokenType tokenType,
            ModelDescription modelDescription,
            int textIndex,
            int paragraphIndex,
            int cellIndex,
            int rowIndex)
        {
            this.TokenType = tokenType;
            this.ModelDescription = modelDescription;
            this.TextIndex = textIndex;
            this.ParagraphIndex = paragraphIndex;
            this.CellIndex = cellIndex;
            this.RowIndex = rowIndex;
        }

        public TokenType TokenType { get; }
        public ModelDescription ModelDescription { get; }

        public int TextIndex { get; }
        public int ParagraphIndex { get; }

        public int RowIndex { get; }
        public int CellIndex { get; }

        public static Token SingleValue(ModelDescription modelDescription, int textIndex, int paragraphIndex, int cellIndex, int rowIndex)
        {
            return new Token(TokenType.SingleValue, modelDescription, textIndex, paragraphIndex, cellIndex, rowIndex);
        }

        //public static Token CollectionBegin(ModelDescription modelDescription, int textIndex, int paragraphIndex)
        //{
        //    return new Token(TokenType.CollectionBegin, modelDescription, textIndex, paragraphIndex, -1, -1);
        //}

        public static Token CollectionBegin(ModelDescription modelDescription, int textIndex, int paragraphIndex, int cellIndex, int rowIndex)
        {
            return new Token(TokenType.CollectionBegin, modelDescription, textIndex, paragraphIndex, cellIndex, rowIndex);
        }

        //public static Token CollectionEnd(ModelDescription modelDescription, int textIndex, int paragraphIndex)
        //{
        //    return new Token(TokenType.CollectionEnd, modelDescription, textIndex, paragraphIndex, -1, -1);
        //}

        public static Token CollectionEnd(ModelDescription modelDescription, int textIndex, int paragraphIndex, int cellIndex, int rowIndex)
        {
            return new Token(TokenType.CollectionEnd, modelDescription, textIndex, paragraphIndex, cellIndex, rowIndex);
        }

        public static Token ConditionBegin(ModelDescription modelDescription, int textIndex, int paragraphIndex)
        {
            return new Token(TokenType.ConditionBegin, modelDescription, textIndex, paragraphIndex, -1, -1);
        }

        public static Token ConditionEnd(ModelDescription modelDescription, int textIndex, int paragraphIndex)
        {
            return new Token(TokenType.ConditionEnd, modelDescription, textIndex, paragraphIndex, -1, -1);
        }
    }
}
