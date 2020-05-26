using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using Docx.DataModel;

namespace Docx.Processors.Searching
{
    internal static class TableSearch
    {
        // search only for Array and Condition Templates
        public static Template FindNextTemplate(
            this Table table,
            EngineConfig engineConfig)
        {
            var rowIndex = 0;
            foreach(var row in table.Childs<TableRow>())
            {
                var cellIndex = 0;
                foreach(var cell in row.Childs<TableCell>())
                {
                    var paragraphs = cell.Paragraphs().ToArray();
                    var template = paragraphs.FindNextTemplate(0, engineConfig, rowIndex, cellIndex, simpleValue: false);
                    switch (template)
                    {
                        case SingleValueTemplate svt:
                            return svt;
                        case ArrayTemplate at when at.IsComplete && !at.Start.Position.IsSameRowCell(at.End.Position):
                            return at;
                        case ArrayTemplate at when !at.IsComplete:
                            var t = at.Start.FindCompleteTemplate(table, engineConfig);
                            return t;
                    }
                    cellIndex++;
                }

                rowIndex++;
            }
            return Template.Empty;
        }

        private static ArrayTemplate FindCompleteTemplate(this Token start, Table table, EngineConfig engineConfig)
        {
            var rowIndex = -1;
            foreach (var row in table.Rows())
            {
                rowIndex++;
                if(rowIndex < start.Position.RowIndex)
                {
                    continue;
                }

                var cellIndex = rowIndex == start.Position.RowIndex
                    ? start.Position.CellIndex
                    : 0;

                foreach(var cell in row.Cells().Skip(cellIndex))
                {
                    var end = cell.Paragraphs().ToArray().FindCloseToken(start, engineConfig, continueAfterOpenToken: false, rowIndex, cellIndex);
                    if(end != Token.None)
                    {
                        var openXmlTemplate = PrepareRowsTemplate(start, end, table.Rows());
                        return new ArrayTemplate(start, end, openXmlTemplate);
                    }

                    cellIndex++;
                }
            }

            return new ArrayTemplate(start, Token.None, null);
        }

        private static OpenXmlTemplate PrepareRowsTemplate(Token start, Token end, IEnumerable<TableRow> tableRows)
        {
            var rows = tableRows
                .GetTemplateRows(start.Position, end.Position)
                .Select(r => r.CloneNode(true))
                .Cast<TableRow>()
                .ToArray();

            rows.First()
                .Cells().ElementAt(start.Position.CellIndex)
                .Paragraphs().ElementAt(start.Position.ParagraphIndex)
                .ReplaceToken(start, Model.Empty);

            rows.Last()
                .Cells().ElementAt(end.Position.CellIndex)
                .Paragraphs().ElementAt(end.Position.ParagraphIndex)
                .ReplaceToken(end, Model.Empty);

            return new OpenXmlTemplate(rows);
        }
    }
}
