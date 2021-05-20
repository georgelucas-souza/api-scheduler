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
        public static string AppModuleFolderPath { get; } = Path.Combine(Environment.CurrentDirectory, "Modules");
        public static string AppLogFolderPath { get; } = Path.Combine(Environment.CurrentDirectory, "Logs");
        public static string ConfigFilePath { get; } = Path.Combine(Environment.CurrentDirectory, "config.json");

        public static string GetModuleFolderPath(string apiName)
        {
            string moduleFolder = Path.Combine(AppModuleFolderPath, apiName.Trim().ToUpper());
            IOManager.IDirectory.CreateIfNotExist(moduleFolder);
            return moduleFolder;
        }
    }
}
