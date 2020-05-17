namespace Docx.DataModel
{
    public abstract class Model : IModel, IParentedModel
    {
        protected Model(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public abstract string FormattedValue();

        void IParentedModel.SetParent(IModel context)
        {
        }
    }
}
