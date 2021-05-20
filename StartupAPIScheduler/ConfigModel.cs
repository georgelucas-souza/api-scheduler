using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Dynamic;
using Newtonsoft.Json.Converters;

namespace StartupAPIScheduler
{
    public class ConfigModel
    {

        [JsonProperty]
        public int TimerInterval { get; set; }

        [JsonProperty]
        public string ApiListEndPoint { get; set; }

        [JsonProperty]
        public string ApiListResource { get; set; }

        [JsonProperty]
        public string ApiUpdateResource { get; set; }

        [JsonProperty]
        public int DefaultConnectionTimeout { get; set; }

        public ConfigModel()
        {
            //Default Values
            TimerInterval = 1000;
            ApiListEndPoint = @"http://localhost:0000/";
            ApiListResource = @"api/ApiSchelule/GetList";
            ApiUpdateResource = @"api/ApiSchelule/UpdateApi";
            DefaultConnectionTimeout = 300000;
        }

        public bool Initialize()
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText(AppConfig.ConfigFilePath));

                this.CopyPropertiesFrom(obj);                

                File.WriteAllText(AppConfig.ConfigFilePath, JsonConvert.SerializeObject(this, Formatting.Indented));
                return true;
            }
            catch(Exception ex)
            {
                LogManager.Write(false, ex.Message);
                return false;
            }

        }
    }
}
