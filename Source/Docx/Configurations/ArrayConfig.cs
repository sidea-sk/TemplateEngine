namespace Docx
{
    public sealed class ArrayConfig : ITemplateConfig
    {
        public static readonly ArrayConfig Default = new ArrayConfig("[", "]");

        public ArrayConfig(
            string open,
            string close)
        {
            this.Open = open;
            this.Close = close;
        }

        public string Open { get; }
        public string Close { get; }

        string ITemplateConfig.OpenSuffix => this.Open;

        string ITemplateConfig.ClosePrefix => this.Close;
    }
}
