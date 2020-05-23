namespace Docx
{
    public sealed class ArrayConfig : ITemplateConfig
    {
        public static readonly ArrayConfig Default = new ArrayConfig("[", "]", "$i");

        public ArrayConfig(
            string open,
            string close,
            string item)
        {
            this.Open = open;
            this.Close = close;
            this.Item = item;
        }

        public string Open { get; }
        public string Close { get; }
        public string Item { get; }

        string ITemplateConfig.OpenSuffix => this.Open;

        string ITemplateConfig.ClosePrefix => this.Close;
    }
}
