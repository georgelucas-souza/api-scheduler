using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StartupAPIScheduler
{
    public class APIListItem
    {
        public string ApiName { get; set; }
        public string EndPoint { get; set; }
        public string Resource { get; set; }
        public int TimeInterval { get; set; }
        public int TimeOut { get; set; }
        public RestSharp.Method Method { get; set; }
        public DateTime LastRun { get; set; }
        public Dictionary<string, object> Paramters { get; set; }


        public static List<APIListItem> FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<APIListItem>>(json);
            }
            catch(Exception ex)
            {
                LogManager.Write(false, ex.Message);
                return null;
            }
        }
    }
}
