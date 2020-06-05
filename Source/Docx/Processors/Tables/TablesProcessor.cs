using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Docx.DataModel;
using Docx.Processors.Searching;
using Microsoft.Extensions.Logging;

namespace Docx.Processors
{
    internal class TablesProcessor
    {
        private readonly EngineConfig _engineConfig;
        private readonly IImageProcessor _imageProcessor;
        private readonly ILogger _logger;

        public TablesProcessor(EngineConfig engineConfig, IImageProcessor imageProcessor, ILogger logger)
        {
            _engineConfig = engineConfig;
            _imageProcessor = imageProcessor;
            _logger = logger;
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
            Template template;
            var lastProcessedTableRow = -1;
            do
            {
                template = table.FindNextTemplate(_engineConfig);
                switch (template)
                {
                    case ArrayTemplate at:
                        this.ProcessRowsBetweenIndeces(table, lastProcessedTableRow, at.Start.Position.RowIndex - 1, context);
                        lastProcessedTableRow = this.ProcessTemplate(at, table, context);
                        break;
                    case ConditionTemplate ct:
                        break;
                }
            }
            while (template != Template.Empty);

            this.ProcessRowsBetweenIndeces(
                table,
                lastProcessedTableRow,
                table.Rows().Count() - 1,
                context);
        }

        private int ProcessTemplate(ArrayTemplate template, Table table, Model context)
        {
            var collection = (CollectionModel)context.Find(template.Start.ModelDescription.Expression);
            var resultRows = new List<TableRow>();
            foreach(var item in collection.Items)
            {
                foreach(var row in template.OpenXml.Elements.Select(e => e.CloneNode(true)).Cast<TableRow>())
                {
                    this.ProcessCellsOfRow(row, item);
                    resultRows.Add(row);
                }
            }

            var originalRows = table.Rows()
                .GetTemplateRows(template)
                .ToArray();

            for (var i = 0; i < resultRows.Count; i++)
            {
                originalRows.First().InsertBeforeSelf(resultRows[i]);
            }

            originalRows.RemoveSelfFromParent();

            return template.Start.Position.RowIndex + resultRows.Count;
        }

        private void ProcessRowsBetweenIndeces(Table table, int firstIndex, int lastIndex, Model context)
        {
            foreach(var row in table.Rows().Skip(firstIndex).Take(lastIndex - firstIndex))
            {
                this.ProcessCellsOfRow(row, context);
            }
        }

        private void ProcessCellsOfRow(TableRow row, Model context)
        {
            var compositeElementProcessor = new CompositeElementProcessor(_engineConfig, _imageProcessor, _logger);

            foreach (var cell in row.Cells())
            {
                compositeElementProcessor.Process(cell, context);
            }
        }
    }
}
