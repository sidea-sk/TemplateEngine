namespace Docx.DataModel.Factories
{
    public static class ImageModelFactory
    {
        public static ImageModel ToImageModel(this byte[] data, string name, string imageName)
        {
            return new ImageModel(name, imageName, data);
        }
    }
}
