using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace StartupAPIScheduler
{
    public partial class APIScheduler : ServiceBase
    {
        private static object _intervalSync = new object();

        private Timer appTimer = new Timer();

        ConfigModel config = new ConfigModel();
        INetworkManager networkManager = null;

        public APIScheduler()
        {
            InitializeComponent();

            LogManager.Write(true, "Starting StartupAPIScheduler service...");

            IOManager.IDirectory.CreateIfNotExist(AppConfig.AppLogFolderPath);
            IOManager.IDirectory.CreateIfNotExist(AppConfig.AppModuleFolderPath);
            IOManager.IFile.CreateIfNotExist(AppConfig.ConfigFilePath, config.ToJson());

            if (!config.Initialize())
            {
                Environment.Exit(1);
            }

        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            networkManager = new NetworkManager();

            appTimer.Elapsed += new ElapsedEventHandler(OnTimerTick);
            appTimer.Interval = config.TimerInterval;
            appTimer.Enabled = true;

            LogManager.Write(true, "StartupAPIScheduler was successfully initialized.");
        }

        protected override void OnStop()
        {
            LogManager.Write(true, "StartupAPIScheduler was stopped");
        }

        private async Task FireModule(APIListItem item)
        {

            LogManager.Write(true, $"Fire Module [{item.ApiName}]...");

            var nextRun = item.LastRun.AddMilliseconds(item.TimeInterval);

            if (nextRun <= DateTime.Now)
            {
                var itemResponse = await networkManager.RequestAsync(item.EndPoint, item.Resource, item.Method, item.TimeOut, item.Paramters, item.ApiName);

                var checkResponse = itemResponse.CheckResponse();

                if (checkResponse.IsValid)
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add("ApiName", item.ApiName);
                    dict.Add("LastRun", nextRun);
                    var apiUpdateResponse = networkManager.Request(config.ApiListEndPoint, config.ApiUpdateResource, RestSharp.Method.POST, config.DefaultConnectionTimeout, dict, null).CheckResponse();
                    if (apiUpdateResponse.IsValid)
                    {
                        LogManager.Write(true, $"Module [{item.ApiName}] SUCCESS.");
                    }
                }
                else
                {
                    LogManager.Write(true, $"Module [{item.ApiName}] ERROR.");
                }
            }
        }


        private void ExecuteMainJob()
        {
            var apiListResponse = networkManager.Request(config.ApiListEndPoint, config.ApiListResource, RestSharp.Method.GET, config.DefaultConnectionTimeout, null, null).CheckResponse();
            LogManager.Write(true, "Retreiving API List...");

            List<Task> taskList = new List<Task>();

            if (apiListResponse.IsValid)
            {
                LogManager.Write(true, "Retreiving API List... Success!");

                var apiScheduleList = APIListItem.FromJson(apiListResponse.Content);

                if (apiScheduleList != null)
                {
                    LogManager.Write(true, $"{apiScheduleList.Count.ToString("00")} API's found.");

                    var tasks = apiScheduleList.Select(s => Task.Factory.StartNew(async () => await FireModule(s)).Unwrap()).ToList().ToArray();

                    Task.WaitAll(tasks);

                }
            }
        }

        private void OnTimerTick(object source, ElapsedEventArgs e)
        {
            appTimer.Stop();
            appTimer.Enabled = false;

            if (!config.Initialize())
            {
                Environment.Exit(1);
            }

            appTimer.Interval = config.TimerInterval;

            DateTime startTime = DateTime.Now;

            try
            {
                ExecuteMainJob();
            }
            catch (Exception ex) { LogManager.Write(false, ex.Message); }
            finally
            {
                DateTime endTime = DateTime.Now;

                var secondsPassed = Math.Round(endTime.Subtract(startTime).TotalSeconds);
                var ts = TimeSpan.FromSeconds(secondsPassed).ToString();


                LogManager.Write(true, $"Proccess finished in {{{ts}}}");

                appTimer.Start();
                appTimer.Enabled = true;
            }


        }
    }
}
