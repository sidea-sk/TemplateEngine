using System.Collections.Generic;
using System.Linq;

namespace Docx.DataModel
{
    public class ObjectModel : Model
    {
        private readonly Dictionary<string, Model> _childModels = new Dictionary<string, Model>();

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

        internal override Model Find(ModelDescription description)
        {
            return null;
        }

        protected void SetSelfAsParent(params Model[] childModels)
        {
            foreach(var child in childModels.Cast<IParentedModel>())
            {
                child.SetParent(this);
            }
        }
    }
}
