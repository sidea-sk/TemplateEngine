using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Docx.DataModel;
using Docx.Processors.Searching;

namespace Docx.Processors
{
    internal class TablesProcessor
    {
        private readonly EngineConfig _engineConfig;

        public TablesProcessor(EngineConfig engineConfig)
        {
            _engineConfig = engineConfig;
        }

        public void Process(OpenXmlCompositeElement parent, Model context)
        {
            var tables = parent
                .Childs<Table>();

            foreach(var table in tables)
            {
                this.Process(table, context);
            }
        }

        private void Process(Table table, Model context)
        {
            var template = Template.Empty;
            do
            {
                template = table.FindNextTemplate(_engineConfig);
                switch (template)
                {
                    case SingleValueTemplate st:
                        this.ProcessTemplate(st, table, context);
                        break;
                    case ArrayTemplate at:
                        break;
                }
                // process template
            }
            while (template != Template.Empty);
        }

        private int ProcessTemplate(SingleValueTemplate template, Table table, Model context)
        {
            var p = table
                .Rows().ElementAt(template.Token.RowIndex)
                .Cells().ElementAt(template.Token.CellIndex)
                .Paragraphs().ElementAt(template.Token.ParagraphIndex);

            var model = context.Find(template.Token.ModelDescription.Expression);

            var textEndIndex = p.ReplaceToken(template.Token, model);
            return textEndIndex;
        }

        private void ProcessArrayTemplate(ArrayTemplate template, Table table, Model context)
        {

        }
    }
}
