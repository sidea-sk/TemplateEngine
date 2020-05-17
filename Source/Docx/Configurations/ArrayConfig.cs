namespace Docx
{
    public class ArrayConfig
    {
        public static readonly ArrayConfig Default = new ArrayConfig("[", "]", "$");

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
    }
}
