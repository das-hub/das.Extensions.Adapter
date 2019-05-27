using das.Extensions.Adapter.Loaders;

namespace das.Extensions.Adapter
{
    public abstract class FileAdapter
    {     
        public string Source { get; protected set; }
        public abstract RecordCollection<T> Records<T>(bool isFirstRowHeader = false) where T : Record, new();
        public abstract Table Table(bool isFirstRowHeader = false);

        public static ObjectAdapter Csv(string file, char delemiter = ';')
        {
            return new ObjectAdapter(file, new CsvLoader(delemiter));
        }

        public static ObjectAdapter Excel(string file)
        {
            return new ObjectAdapter(file, new OleLoader());
        }

        public static ObjectAdapter Load(string file, TableLoader loader)
        {            
            return new ObjectAdapter(file, loader);
        }
    }
}
