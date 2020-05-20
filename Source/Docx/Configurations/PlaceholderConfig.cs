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
            this.NamesDelimiter = delimiter;
            this.ParametersDelimiter = formatDelimiter;
        }

        public string Start { get; }
        public string End { get; }
        public string NamesDelimiter { get; }
        public string ParametersDelimiter { get; }

        public string ToRegexPattern()
        {
            return Start + "." + End;
        }
    }
}
