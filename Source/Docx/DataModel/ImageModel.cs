namespace Docx.DataModel
{
    public class ImageModel : Model
    {
        public ImageModel(
            string name,
            string imageName,
            byte[] data) : base(name)
        {
            this.ImageName = imageName;
            this.Data = data;
        }

        public string ImageName { get; }

        public byte[] Data { get; }

        public override string FormattedValue()
        {
            return string.Empty;
        }

        internal override Model Find(ModelExpression expression)
        {
            if (expression.IsFinal && expression.Name == this.Name)
            {
                return this;
            }

            return this.Parent.Find(expression);
        }
    }
}
