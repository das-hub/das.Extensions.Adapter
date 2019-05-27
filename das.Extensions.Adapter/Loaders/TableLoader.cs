using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using das.Extensions.Adapter.Common;

namespace das.Extensions.Adapter.Loaders
{
    public abstract class TableLoader
    {
        public abstract Dictionary<string, DataTable> Parse(string file);

        protected string Dublicate(string file)
        {
            string fileName = $"{Guid.NewGuid():N}{Path.GetExtension(file)}";
            string dublicate = Path.Combine(AppEnv.GetTemp(), fileName);
            File.Copy(file, dublicate, true);
            return dublicate;
        }

        protected void Deallocate(string file)
        {
            File.Delete(file);
        }
    }
}