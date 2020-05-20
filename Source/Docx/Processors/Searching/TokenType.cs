namespace Docx.Processors.Searching
{
    internal enum TokenType
    {
        None,
        SingleValue,
        CollectionBegin,
        CollectionEnd,
        ConditionBegin,
        ConditionEnd,
        Unknown
    }
}
