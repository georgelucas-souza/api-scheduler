using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupAPIScheduler
{
    public static class LogManager
    {
        
        private static string GetCurrentFilepath(string apiName = null)
        {
            string dateRef = DateTime.Now.ToString("yyyy-MM-dd");
            string folderPath = string.Empty;
            string filepath = string.Empty;

            if (apiName != null)
            {
                string moduleFolder = AppConfig.GetModuleFolderPath(apiName);
                folderPath = Path.Combine(moduleFolder, dateRef);
                filepath = Path.Combine(moduleFolder, dateRef, "log.txt");
            }
            else
            {
                folderPath = Path.Combine(AppConfig.AppLogFolderPath, dateRef);
                filepath = Path.Combine(AppConfig.AppLogFolderPath, dateRef, "log.txt");
            }           
            

            IOManager.IDirectory.CreateIfNotExist(folderPath);
            IOManager.IFile.CreateIfNotExist(filepath);

            return filepath;
        }

        public static void Write(bool success, string message, string apiName = null)
        {
            var logFilePath = GetCurrentFilepath(apiName);

            List<string> logLineList = File.ReadAllLines(logFilePath).Take(999).ToList();

            string lineConstructor = $@"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}][{(success ? "SUCCESS" :"ERROR")}]: {message}";

            logLineList.Insert(0, lineConstructor);

            File.WriteAllLines(logFilePath, logLineList.ToArray());

            if(apiName != null)
            {
                Write(success, $"Module [{apiName}] Error.", null);
            }

        }
    }
}
