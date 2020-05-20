namespace Docx.DataModel
{
    public interface IModel
    {
        string Name { get; }

        string FormattedValue();
    }
}
