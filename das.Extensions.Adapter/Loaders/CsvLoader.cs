using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using das.Extensions.Adapter.Extensions;

namespace das.Extensions.Adapter.Loaders
{
    public class CsvLoader : TableLoader
    {
        private readonly char _delimiter;

        public CsvLoader(char delimiter = ';')
        {
            _delimiter = delimiter;
        }

        public override Dictionary<string, DataTable> Parse(string file)
        {
            Dictionary<string, DataTable> result = new Dictionary<string, DataTable>();

            Regex regex = new Regex($"{_delimiter}(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

            using (StreamReader reader = new StreamReader(file))
            {
                string[] parts = regex.Split(reader.ReadLine());

                DataTable dataTable = new DataTable(Path.GetFileName(file));

                parts.Do((_, i) => dataTable.Columns.Add($"Column#{++i}"));

                reader.BaseStream.Position = 0;
                reader.DiscardBufferedData();

                while (!reader.EndOfStream)
                {
                    parts = regex.Split(reader.ReadLine()); ;

                    DataRow row = dataTable.NewRow();

                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        row[i] = parts.Length < i ? "" : parts[i];
                    }

                    dataTable.Rows.Add(row);
                }

                result.Add(dataTable.TableName, dataTable);
            }

            return result;
        }
    }
}