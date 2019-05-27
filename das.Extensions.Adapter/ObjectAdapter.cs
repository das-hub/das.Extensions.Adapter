using System.Collections.Generic;
using System.Data;
using System.Linq;
using das.Extensions.Adapter.Loaders;

namespace das.Extensions.Adapter
{
    public class ObjectAdapter : FileAdapter
    { 
        private readonly Dictionary<string, DataTable> _sheetsData;
        private DataTable _currentSheetData;

        public ObjectAdapter(string file, TableLoader loader)
        {
            Source = file;

            _sheetsData = loader.Parse(file);
      
            _currentSheetData = _sheetsData[_sheetsData.Keys.First()];
        }

        public IEnumerable<string> SheetNames => _sheetsData.Keys;

        public string CurrentSheet => _currentSheetData.TableName;

        public ObjectAdapter this[string sheetName]
        {
            get
            {
                if (_sheetsData.ContainsKey(sheetName))
                    _currentSheetData = _sheetsData[sheetName];
                return this;
            }
        }

        public override RecordCollection<T> Records<T>(bool isFirstRowHeader=false)
        {            
            return new TableRecordCollection<T>(_currentSheetData, isFirstRowHeader);
        }

        public override Table Table(bool isFirstRowHeader = false)
        {
            return new Table(_currentSheetData, isFirstRowHeader);
        }
    }
}
