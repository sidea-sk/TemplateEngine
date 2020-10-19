using System;

namespace Docx.DataModel
{
    public class ConditionModel : Model
    {
        private readonly Func<bool> _conditionFunc;

        public ConditionModel(string name, bool value) : this(name, () => value)
        {
        }

        public ConditionModel(string name, Func<bool> conditionFunc): base(name)
        {
            _conditionFunc = conditionFunc;
        }

        public bool IsTrue() => _conditionFunc();

        public bool IsFullfilled(string parameter)
        {
            return string.IsNullOrWhiteSpace(parameter) || parameter.ToLower() != "false"
                ? this.IsTrue()
                : !this.IsTrue();
        }

        public override string FormattedValue()
        {
            return _conditionFunc().ToString().ToLower();
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
