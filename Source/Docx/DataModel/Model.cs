namespace Docx.DataModel
{
    public abstract class Model : IModel, IParentedModel
    {
        public static readonly Model Empty = new EmptyModel();
        public static readonly Model Exception = new ExceptionModel();

        protected Model(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public abstract string FormattedValue();

        internal abstract Model Find(ModelDescription description);

        void IParentedModel.SetParent(IModel context)
        {
        }

        private class EmptyModel : Model
        {
            public EmptyModel() : base(string.Empty)
            {
            }

            public override string FormattedValue()
            {
                return string.Empty;
            }

            internal override Model Find(ModelDescription description)
            {
                return this;
            }
        }

        private class ExceptionModel : Model
        {
            public ExceptionModel() : base(string.Empty)
            {
            }

            public override string FormattedValue()
            {
                throw new System.Exception("Exception model");
            }

            internal override Model Find(ModelDescription description)
            {
                throw new System.Exception("Exception model");
            }
        }
    }
}
