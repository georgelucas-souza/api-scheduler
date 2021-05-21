using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupAPIScheduler
{
    public class NetworkManager : INetworkManager
    {
         
        public async Task<IRestResponse> RequestAsync(string endpoint, string resource, Method method, int timeOut, Dictionary<string, object> parameters = null, string apiName = null)
        {
            IRestResponse response;

            try
            {
                RestClient client = new RestClient(endpoint);

                var request = new RestRequest(resource, method);

                if ((parameters != null) && parameters.Count > 0)
                {
                    if (method == Method.POST)
                    {
                        var jsonBody = parameters.ToJsonObject();

                        request.AddJsonBody(jsonBody);

                        request.RequestFormat = DataFormat.Json;
                    }
                    else if (method == Method.GET)
                    {
                        foreach (var parameter in parameters)
                        {
                            request.AddParameter(parameter.Key, parameter.Value);
                        }
                    }
                }

                request.Timeout = timeOut;

                response = await client.ExecuteTaskAsync(request);

                return response;
            }
            catch (Exception ex)
            {
                LogManager.Write(false, ex.Message, apiName);

                return null;
            }



        }

        IRestResponse INetworkManager.Request(string endpoint, string resource, Method method, int timeOut, Dictionary<string, object> parameters, string apiName)
        {
            IRestResponse response;

            try
            {
                RestClient client = new RestClient(endpoint);

                var request = new RestRequest(resource, method);

                if ((parameters != null) && parameters.Count > 0)
                {
                    if (method == Method.POST)
                    {
                        var jsonBody = parameters.ToJsonObject();

                        request.AddJsonBody(jsonBody);

                        request.RequestFormat = DataFormat.Json;
                    }
                    else if (method == Method.GET)
                    {
                        foreach (var parameter in parameters)
                        {
                            request.AddParameter(parameter.Key, parameter.Value);
                        }
                    }
                }

                request.Timeout = timeOut;

                response = client.Execute(request);

                return response;
            }
            catch (Exception ex)
            {
                LogManager.Write(false, ex.Message, apiName);

                return null;
            }
        }
    }
}
