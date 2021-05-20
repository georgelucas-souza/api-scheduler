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
            await Task.Run(() =>
            {
                LogManager.Write(true, $"Fire Module [{item.ApiName}]...");

                var nextRun = item.LastRun.AddMilliseconds(item.TimeInterval);

                if (nextRun <= DateTime.Now)
                {
                    var itemResponse = networkManager.Request(item.EndPoint, item.Resource, item.Method, item.TimeOut, item.Paramters, item.ApiName).CheckResponse();
                    if (itemResponse.IsValid)
                    {
                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        dict.Add("ApiName", item.ApiName);
                        dict.Add("LastRun", nextRun);
                        var apiUpdateResponse = networkManager.Request(config.ApiListEndPoint, config.ApiUpdateResource, RestSharp.Method.POST, config.DefaultConnectionTimeout, dict, null).CheckResponse();
                        if (apiUpdateResponse.IsValid)
                        {
                            LogManager.Write(true, $"Module [{item.ApiName}] success.");
                        }
                    }
                }
            });
        }

        private async Task ExecuteMainJob()
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

                    foreach (var item in apiScheduleList)
                    {
                        taskList.Add(FireModule(item));
                    }

                    await Task.WhenAll(taskList.ToArray());
                }
            }
        }

        private void OnTimerTick(object source, ElapsedEventArgs e)
        {
            appTimer.Stop();
            appTimer.Enabled = false;

            DateTime startTime = DateTime.Now;

            try
            {
                ExecuteMainJob().GetAwaiter().GetResult();
            }
            catch(Exception ex) { LogManager.Write(false, ex.Message); }
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
