namespace Docx
{
    public class EngineConfig
    {
        public static readonly EngineConfig Default = new EngineConfig(PlaceholderConfig.Default, ArrayConfig.Default, ConditionConfig.Default);

        public EngineConfig(
            PlaceholderConfig placeholder,
            ArrayConfig arrayConfig,
            ConditionConfig condition)
        {
            this.Placeholder = placeholder;
            this.Array = arrayConfig;
            this.Condition = condition;
        }

        public PlaceholderConfig Placeholder { get; }
        public ArrayConfig Array { get; }
        public ConditionConfig Condition { get; }
    }
}
