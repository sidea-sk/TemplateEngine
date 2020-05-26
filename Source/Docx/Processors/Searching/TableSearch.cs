using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Docx.Processors.Searching
{
    internal static class TableSearch
    {
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
                    var template = paragraphs.FindNextTemplate(0, engineConfig, rowIndex, cellIndex);
                    switch (template)
                    {
                        case SingleValueTemplate svt:
                            return svt;
                        case ArrayTemplate at when at.IsComplete:
                            return at;
                        case ArrayTemplate at:
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
            var rowIndex = start.RowIndex;
            foreach (var row in table.Rows().Skip(start.RowIndex))
            {
                var cellIndex = rowIndex == start.RowIndex
                    ? start.CellIndex
                    : 0;

                foreach(var cell in row.Cells().Skip(cellIndex))
                {
                    var end = cell.Paragraphs().ToArray().FindCloseToken(start, engineConfig, rowIndex, cellIndex);
                    if(end != Token.None)
                    {
                        var templateRows = table.Rows()
                            .Skip(start.RowIndex)
                            .Take(end.RowIndex - start.RowIndex + 1);

                        return new ArrayTemplate(start, end, new OpenXmlTemplate(templateRows));
                    }

                    cellIndex++;
                }

                rowIndex++;
            }

            return new ArrayTemplate(start, Token.None, null);
        }
    }
}
