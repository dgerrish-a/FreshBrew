using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Riverbed.Test.FreshBrew.Models;
using Riverbed.Test.FreshBrewApi.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LoadScript
{
    public interface IConfigParserService
    {
        bool Parse(string configfile);
        void Execute();        
    }
    public class ConfigParserService : IConfigParserService
    {
        private readonly ILogger<ConfigParserService> logger;
        private LoadGeneratorSetting loadSetting;
        private bool isValidConfig = false;
        public ConfigParserService(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ConfigParserService>();
        }

        public void Execute()
        {
            int noOfThreads = loadSetting.Threads;
            if (noOfThreads == 0)
                noOfThreads = 1;

            Task[] taskList = new Task[noOfThreads];

            for(int i = 0; i < noOfThreads; i++)
            {
                taskList[i]  = Task.Run(
                                () => ExecuteSingleThreaded()
                                );
            }
            Console.WriteLine("Started all threads...Waiting for them to complete.");
            Task.WaitAll(taskList);
        }
        public void ExecuteSingleThreaded9999()
        {
            int loopCountMax = loadSetting.Loop;
            //if loop value from config is negative, it means 'run forever'
            if (loopCountMax < 0)
                loopCountMax = Int32.MaxValue;
            int loopWait = loadSetting.LoopWait;
            int iteration = 0;
            if (!isValidConfig)
            {
                logger.LogError("Invalid config. Exiting from load script.");
                return;
            }
            IOrderItem orderItemData = new InMemoryDb();
            while (true)
            {
                iteration++;

                foreach (OrderItem i in orderItemData.GetAll())
                {
                    Console.WriteLine($"id={i.Id}  Name={i.Name} IsReady={i.IsReady} Delay={i.DelayByInSeconds}");
                }
                Console.WriteLine("------------------------------");
                System.Threading.Thread.Sleep(loopWait);

            }

        }
        public void ExecuteSingleThreaded()
        {
            int loopCountMax = loadSetting.Loop;
            //if loop value from config is negative, it means 'run forever'
            if (loopCountMax < 0)
                loopCountMax = Int32.MaxValue;
            int loopWait = loadSetting.LoopWait;
            int iteration = 0;
            if (!isValidConfig)
            {
                logger.LogError("Invalid config. Exiting from load script.");
                return;
            }
            while (true)
            {
                iteration++;
                PrintMessage($"iteration={iteration}, loopCountMax={loopCountMax}");
                PrintMessage($"**** Loop count = {iteration} *******");
                if (Int32.MaxValue == iteration) //if integer max, reset iteration counter
                {
                    iteration = 0;
                }
                else if (iteration > loopCountMax)
                    break;
                foreach (UrlPath urlItem in loadSetting.urlPathList)
                {
                    var uri = new Uri(urlItem.ApplicationUrl);
                    if (urlItem.HttpVerb.Equals("get", StringComparison.CurrentCultureIgnoreCase))
                    {
                        PrintMessage("Get: " + uri.AbsolutePath);
                        for (int i = 0; i < 30; i++)
                        {
                            try
                            {
                                HttpCaller caller = GetHttpCaller(urlItem);
                                string data = caller.DoHttpGet(uri.PathAndQuery);
                                if (data != null)
                                {
                                    PrintMessage("(Result from get) " + data);
                                    PrintMessage("Successful");
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.ToString().ToLower().Contains("network is unreachable"))
                                {
                                    PrintMessage("Network is unreachable, Will try after some time.");
                                    System.Threading.Thread.Sleep(2000);
                                }
                            }
                        }

                    }
                }

                 PrintMessage($"**** Looping urls *******");
                foreach (UrlPath urlItem in loadSetting.urlPathList)
                {
                    var uri = new Uri(urlItem.ApplicationUrl);
                    PrintHeaderLine(urlItem.HttpVerb, urlItem.ApplicationUrl);
                    try
                    {
                        if (urlItem.HttpVerb.Equals("get", StringComparison.CurrentCultureIgnoreCase))
                        {
                            HttpCaller caller = GetHttpCaller(urlItem);
                            string data = caller.DoHttpGet(uri.PathAndQuery);
                            if (data != null)                                
                                    PrintMessage("(Resul from get) "+data);
                            else
                                PrintMessage("ERROR: Web application is not functional, please check its api and its response");
                        }
                        else if (urlItem.HttpVerb.Equals("post", StringComparison.CurrentCultureIgnoreCase))
                        {
                            HttpCaller caller = GetHttpCaller(urlItem);
                            string data = caller.DoHttpPost(uri.PathAndQuery, urlItem.Payload); // urlItem.DelayByInSeconds);
                            if (data != null)
                                PrintMessage("(Result from post) "+data);
                        }
                        else if (urlItem.HttpVerb.Equals("put", StringComparison.CurrentCultureIgnoreCase))
                        {
                            HttpCaller caller = GetHttpCaller(urlItem);
                            String statusCode = caller.DoHttpPut(uri.PathAndQuery, urlItem.Payload);// urlItem.DelayByInSeconds);
                            if (statusCode!=null)
                                PrintMessage("(Result from put) " + statusCode.ToString());
                            else
                                PrintMessage("(Result from put): NULL");
                        }
                        else if (urlItem.HttpVerb.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
                        {
                            HttpCaller caller = GetHttpCaller(urlItem);
                            HttpStatusCode statusCode = caller.DoHttpDelete(uri.PathAndQuery);
                            PrintMessage("(Result from delete) " + statusCode.ToString());
                        }
                    }
                    catch(Exception ex)
                    {
                        PrintMessage(ex.ToString());
                    }
                   
                    PrintFooterLine();
                    System.Threading.Thread.Sleep(loopWait);
                }
            }

        }
        private void PrintMessage(string msg)
        {
            Console.WriteLine($"ThreadId={Environment.CurrentManagedThreadId} \t {msg}");
        }
        private HttpCaller GetHttpCaller(UrlPath urlItem)
        {
            string urlValue = urlItem.ApplicationUrl;
            var uri = new Uri(urlValue);

            string baseAddress = $"http://{uri.Host}:{uri.Port}";
            HttpCaller caller = new HttpCaller(baseAddress);
            return caller;
        }
        private void PrintHeaderLine(string httpVerb, string url)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine($"ThreadId={Environment.CurrentManagedThreadId}\t Making " + httpVerb + " " + url);
        }
        private void PrintFooterLine()
        {
            Console.WriteLine("---------------------------------------------------------------");
        }
        public bool Parse(string configfile)
        {
            /*                 
                                           {
                               "loop":-1,
                               "loopwait":0,
                               "threads":4,
                               "hits":[
                                       { "httpverb":"get", "url": "http://10.46.45.8/FreshBrewFE/api/order/" },
                                       { "httpverb":"post", "url": "http://10.46.45.8/FreshBrewFE/api/order", "delaybyinseconds":1 },
                                       { "httpverb":"put", "url": "http://10.46.45.8/FreshBrewFE/api/order/1", "delaybyinseconds":1 },
                                       { "httpverb":"delete", "url": "http://10.46.45.8/FreshBrewFE/api/order/1"  }
                                   ]
                           }

           */

            if (!File.Exists(configfile)) {
                logger.LogError("Invalid: could not find config file:" + configfile);
                return false;
            }
            string content = File.ReadAllText(configfile);

            loadSetting = JsonConvert.DeserializeObject<LoadGeneratorSetting>(content);
           
            foreach (UrlPath urlItem in loadSetting.urlPathList)
            {
                string urlValue = urlItem.ApplicationUrl;
                var uri = new Uri(urlValue);
                if (uri.Scheme != Uri.UriSchemeHttp)
                {
                    logger.LogError("The url is NOT http. Exiting from load script:"+urlValue);
                    isValidConfig= false;
                    return false;
                }
            }
            isValidConfig = true;
            return true;
        }
    }
}
