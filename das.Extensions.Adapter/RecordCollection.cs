using System.Collections;
using System.Collections.Generic;

namespace das.Extensions.Adapter
{
    public interface IRecordCollection<out T>: IEnumerable<T> where T: Record, new()  { }

    public abstract class RecordCollection<T>: IRecordCollection<T> where T : Record, new()
    {
        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
