using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace das.Extensions.Adapter
{
    public class TableRecordCollection<T>: RecordCollection<T> where T : Record, new()
    {
        private readonly DataTable _table;
        private readonly bool _isFirstRowHeader;

        public TableRecordCollection(DataTable table, bool isFirstRowHeader = false)
        {
            _table = table;
            _isFirstRowHeader = isFirstRowHeader;
        }

        public override IEnumerator<T> GetEnumerator()
        {
            if (_isFirstRowHeader)                    
                return _table.Rows?.Cast<DataRow>().Skip(1).Select((r, i) => new T { Index = i + 2, Source = r.ItemArray }).GetEnumerator();
            return _table.Rows?.Cast<DataRow>().Select((r, i) => new T {Index = ++i, Source = r.ItemArray}).GetEnumerator();
        }
    }
}
