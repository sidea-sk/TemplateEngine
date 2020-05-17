namespace Docx
{
    public class PlaceholderConfig
    {
        public static readonly PlaceholderConfig Default = new PlaceholderConfig("{", "}", ".", ":");

        public PlaceholderConfig(
            string start,
            string end,
            string delimiter,
            string formatDelimiter)
        {
            this.Start = start;
            this.End = end;
            this.Delimiter = delimiter;
            this.FormatDelimiter = formatDelimiter;
        }

        public string Start { get; }
        public string End { get; }
        public string Delimiter { get; }
        public string FormatDelimiter { get; }
    }
}
