namespace Docx.DataModel
{
    public abstract class Model
    {
        public static readonly Model Empty = new EmptyModel();
        public static readonly Model Exception = new ExceptionModel();

        protected Model(string name)
        {
            this.Name = name;
        }

        protected Model Parent { get; private set; }

        public string Name { get; }

        public abstract string FormattedValue();

        internal abstract Model Find(ModelExpression expression);

        internal void SetParent(Model context)
        {
            this.Parent = context;
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

            internal override Model Find(ModelExpression expression)
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

            internal override Model Find(ModelExpression expression)
            {
                throw new System.Exception("Exception model");
            }
        }
    }
}
