using System;
using System.Diagnostics;

namespace Docx.DataModel
{
    [DebuggerDisplay("{Name}: {FormattedValue()}")]
    public sealed class SimpleModel : Model
    {
        private readonly Func<string> _formattedValueFunc;

        public SimpleModel(string name, string formattedValue) : this(name, () => formattedValue)
        {
        }

        public SimpleModel(string name, Func<string> formattedValueFunc) : base(name)
        {
            _formattedValueFunc = formattedValueFunc;
        }

        public override string FormattedValue()
        {
            return _formattedValueFunc();
        }

        internal override Model Find(ModelDescription description)
        {
            if(description.Name == this.Name)
            {
                return this;
            }

            return Model.Empty;
        }
    }
}
