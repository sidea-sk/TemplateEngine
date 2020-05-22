using System.Collections.Generic;
using System.Linq;

namespace Docx.DataModel
{
    public class ObjectModel : Model
    {
        private readonly Dictionary<string, Model> _childModels = new Dictionary<string, Model>();

        public ObjectModel(string name, params Model[] childModels) : this(name, (IEnumerable<Model>)childModels)
        {
        }

        public ObjectModel(string name, IEnumerable<Model> childModels) : base(name)
        {
            foreach(var child in childModels)
            {
                this.AddChild(child);
            }
        }

        public override string FormattedValue()
            => this.Name;

        protected void AddChild(Model child)
        {
            this.SetSelfAsParent(child);
            _childModels.Add(child.Name, child);
        }

        internal override Model Find(ModelExpression expression)
        {
            if(expression.Root == this.Name)
            {
                if (expression.IsFinal)
                {
                    return this;
                }

                var childExpression = expression.Child();
                if (!_childModels.ContainsKey(childExpression.Root))
                {
                    return Model.Empty;
                }

                return _childModels[childExpression.Root].Find(childExpression);
            }

            return this.Parent == null
                ? Model.Empty
                : this.Parent.Find(expression);
        }

        protected void SetSelfAsParent(params Model[] childModels)
        {
            foreach(var child in childModels)
            {
                child.SetParent(this);
            }
        }
    }
}
