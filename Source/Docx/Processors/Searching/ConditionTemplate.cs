namespace Docx.Processors.Searching
{
    internal class ConditionTemplate : Template
    {
        public ConditionTemplate(Token start, Token end)
        {
            this.Start = start;
            this.End = end;
        }

        public override bool IsComplete => this.Start != Token.None && this.End != Token.None;

        public Token Start { get; }
        public Token End { get; }
    }
}
