using System;

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

        public ImageModel(
            string name,
            string imageName,
            string base64) : base(name)
        {
            this.ImageName = imageName;
            this.Data = ImageSourceToByteArray(base64);
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

        private static byte[] ImageSourceToByteArray(string imageData)
        {
            if (string.IsNullOrWhiteSpace(imageData))
            {
                return Array.Empty<byte>();
            }

            var i = imageData.IndexOf(",", StringComparison.InvariantCultureIgnoreCase);
            var base64 = imageData.Substring(i + 1);
            var imageByteData = Convert.FromBase64String(base64);
            return imageByteData;
        }
    }
}
