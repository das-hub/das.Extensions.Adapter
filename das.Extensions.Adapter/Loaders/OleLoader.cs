using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using das.Extensions.Adapter.Extensions;

namespace das.Extensions.Adapter.Loaders
{
    public class OleLoader : TableLoader
    {
        public override Dictionary<string, DataTable> Parse(string file)
        {
            Dictionary<string, DataTable> result = new Dictionary<string, DataTable>();

            string dublicate = Dublicate(file);

            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"{dublicate}\";Extended Properties=\"Excel 12.0;HDR=No;IMEX=1;\";";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                DataTable table = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new[] { null, null, null, "TABLE" });

                List<string> sheets = new List<string>();
                table.Rows.Cast<DataRow>().Do(r => sheets.Add(r["TABLE_NAME"].ToString()));

                foreach (string sheet in sheets)
                {
                    table = new DataTable(sheet.Replace("$", ""));
                    OleDbDataAdapter adapter;
                    using (adapter = new OleDbDataAdapter($"select * from [{sheet}]", connection)) { adapter.Fill(table); }

                    if (table.Rows.Count != 1 & table.Columns.Count != 1)
                        result.Add(table.TableName, table);
                }
            }

            Deallocate(dublicate);

            return result;
        }
    }
}