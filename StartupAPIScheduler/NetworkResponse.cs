using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartupAPIScheduler
{
    public class NetworkResponse
    {
        public bool IsValid { get; private set; }
        public string Content { get; private set; }

        public NetworkResponse(bool isValid, string content)
        {
            IsValid = isValid;
            Content = content;
        }
    }
}
