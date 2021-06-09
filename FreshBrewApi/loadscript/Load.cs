
using LoadScript;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Riverbed.Test.FreshBrew.LoadScript
{
    class Load
    {

       
        public static void execute()
        {
            try
            {
                /*if (args.Length == 0)
                {
                    Console.WriteLine("Please provide config json file containing urls to hit, iteration and thread.");
                    return;
                }
                string configFile = args[0];
                */
                string configFile = System.IO.Path.Combine(Environment.CurrentDirectory, "config.json");


                var serviceProvider = new ServiceCollection()
                    .AddLogging()
                    .AddSingleton<IConfigParserService, ConfigParserService>()                   
                    .BuildServiceProvider();
               /*
                serviceProvider
                    .GetService<ILoggerFactory>()
                    .AddConsole(LogLevel.Debug);
                 */   
                // add the framework services
                
               
                var logger = serviceProvider.GetService<ILoggerFactory>()
                    .CreateLogger<Load>();
                logger.LogInformation("Starting application");
                               
                var parser = serviceProvider.GetService<IConfigParserService>();

                bool isValidConfig = parser.Parse(configFile);

                
                if (isValidConfig)
                {
                    Task[] taskList = new Task[1];

                    for (int i = 0; i < 1; i++)
                    {
                        taskList[i] = Task.Run(
                                        () => parser.Execute()
                                        );
                    }
                }                    
                else
                    logger.LogError("Cannot execute load script, its config file was not valid");

                logger.LogInformation("All done!");

            }
            catch (Exception ex)
            {
               Console.WriteLine("Host terminated unexpectedly:"+ex.ToString());

            }
            finally
            {               
                Console.WriteLine("<<<Exit>>>");
            }
            
        }
        

    }
}
