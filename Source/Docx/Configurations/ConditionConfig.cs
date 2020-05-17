namespace Docx
{
    public class ConditionConfig
    {
        public static readonly ConditionConfig Default = new ConditionConfig("?", "?");

        public ConditionConfig(string beginCondition, string endCondition)
        {
            this.Begin = beginCondition;
            this.End = endCondition;
        }

        /// <summary>
        /// Engine will search for pattern (Placeholder.Start|variableName|BeginCondition|Placeholder.End)
        /// </summary>
        public string Begin { get; }

        /// <summary>
        /// Engine will search for pattern (Placeholder.Start|EndCondition|variableName|Placeholder.End)
        /// </summary>
        public string End { get; }
    }
}
