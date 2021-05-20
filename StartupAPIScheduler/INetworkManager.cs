using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupAPIScheduler
{
    public interface INetworkManager
    {
        IRestResponse Request(string endpoint, string resource, Method method, int timeOut, Dictionary<string, object> parameters = null, string apiName = null);
    }
}
