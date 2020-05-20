namespace Docx.Processors.Searching
{
    internal abstract class Template
    {
        public static readonly Template Empty = new EmptyTemplate();

        public abstract bool IsComplete { get; }

        protected Template()
        {
        }

        private class EmptyTemplate : Template
        {
            public override bool IsComplete => true;
        }
    }
}
