using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupAPIScheduler
{
    public static class Extensions
    {
        public static string ToJson(this ConfigModel obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static void CopyPropertiesFrom<T>(this T objFrom, T objTo)
        {
            Type typeParameter = typeof(T);

            var properties = typeParameter.GetProperties();

            foreach (var property in properties)
            {
                var objFromPropertyValue = property.GetValue(objTo);
                property.SetValue(objFrom, objFromPropertyValue);
            }
        }

        public static NetworkResponse CheckResponse(this IRestResponse response, string apiName = null)
        {
            if (response != null)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new NetworkResponse(true, response.Content);
                }
                else
                {
                    string responseMessage = response.ErrorException != null ? response.ErrorException.Message : response.Content;
                    LogManager.Write(false, responseMessage, apiName);

                    return new NetworkResponse(false, responseMessage);
                }
            }
            else
            {
                return new NetworkResponse(false, "");
            }
        }

        public static object ToJsonObject(this Dictionary<string, object> dict)
        {
            var expandoObj = new ExpandoObject();
            var expandoObjCollection = (ICollection<KeyValuePair<string, object>>)expandoObj;

            foreach (var keyValuePair in dict)
            {
                expandoObjCollection.Add(keyValuePair);
            }

            dynamic eoDynamic = expandoObj;

            

            return JsonConvert.SerializeObject(eoDynamic, Formatting.Indented);
        }

        //public static T FirstOrDefault<T>(this ExpandoObject eo, string key)
        //{
        //    object r = eo.FirstOrDefault(x => x.Key == key).Value;

        //    return (T)Convert.ChangeType(r, typeof(T));
        //}
    }
}
