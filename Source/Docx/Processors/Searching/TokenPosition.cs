using System.Diagnostics;

namespace Docx.Processors.Searching
{
    [DebuggerDisplay("{ParagraphIndex}|{TextIndex} / {RowIndex}|{CellIndex})")]
    internal class TokenPosition
    {
        public static readonly TokenPosition None = new TokenPosition(-1, -1, -1, -1);

        public TokenPosition(int paragraphIndex, int textIndex): this(paragraphIndex, textIndex, -1 ,-1)
        {
        }

        public TokenPosition(
            int paragraphIndex,
            int textIndex,
            int rowIndex,
            int cellIndex)
        {
            this.ParagraphIndex = paragraphIndex;
            this.TextIndex = textIndex;
            this.RowIndex = rowIndex;
            this.CellIndex = cellIndex;
        }

        public int ParagraphIndex { get; }
        public int TextIndex { get; }
        public int RowIndex { get; }
        public int CellIndex { get; }

        public bool IsSameRowCell(TokenPosition other)
        {
            return this.RowIndex == other.RowIndex
                && this.CellIndex == other.CellIndex;
        }
    }
}
