using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Docx.Processors.Searching
{
    internal static class OpenXmlFilters
    {
        public static IEnumerable<TableRow> GetTemplateRows(this IEnumerable<TableRow> rows, ArrayTemplate arrayTemplate)
            => rows.GetTemplateRows(arrayTemplate.Start.Position, arrayTemplate.End.Position);

        public static IEnumerable<TableRow> GetTemplateRows(this IEnumerable<TableRow> rows, TokenPosition start, TokenPosition end)
        {
            return rows
                .Skip(start.RowIndex)
                .Take(end.RowIndex - start.RowIndex + 1);
        }
    }
}
