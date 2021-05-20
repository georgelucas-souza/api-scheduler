using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace StartupAPIScheduler
{
    public class IOManager
    {
        public class IDirectory
        {
            public static void CreateIfNotExist(string path)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        public class IFile
        {
            public static void CreateIfNotExist(string path)
            {
                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                }
            }

            public static void CreateIfNotExist(string path, string defaultContent)
            {
                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                    File.WriteAllText(path, defaultContent);
                }
            }
        }
    }
}
