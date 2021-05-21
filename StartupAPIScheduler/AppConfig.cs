using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupAPIScheduler
{
    public static class AppConfig
    {
        public static string AppModuleFolderPath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules");
        public static string AppLogFolderPath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        public static string ConfigFilePath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        public static string GetModuleFolderPath(string apiName)
        {
            string moduleFolder = Path.Combine(AppModuleFolderPath, apiName.Trim().ToUpper());
            IOManager.IDirectory.CreateIfNotExist(moduleFolder);
            return moduleFolder;
        }
    }
}
