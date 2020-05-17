namespace Docx.DataModel
{
    public class EmptyModel : Model
    {
        public static readonly EmptyModel Instance = new EmptyModel();

        private EmptyModel() : base(string.Empty)
        {
        }

        public override string FormattedValue()
        {
            return string.Empty;
        }
    }
}
