using System;
using System.IO;
using System.Reflection;

namespace das.Extensions.Adapter.Common
{
    internal static class AppEnv
    {
        internal static string Name = Assembly.GetEntryAssembly().GetName().Name;
        internal static string Temp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Name}.TEMP");

        internal static string GetTemp()
        {
            return Directory.CreateDirectory(Temp).FullName;
        }
    }
}
