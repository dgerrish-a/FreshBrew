/*
 * 
 * 
 * 
 * 2/4/2019     kthennamanallur     Instead of using machine IP from network (On Linux machines, access to localhost:<port> is NOT working from outside the machine), use 0.0.0.0. 
 * :wq
 * 3/20/2019    kthennamanallur     Instead of reading serilog configuration from appsettings.json (removing log file path modifications required by user in appsettings.json), configure in Program.cs.
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.RollingFileAlternate;
using System.Net;

namespace Riverbed.Test.FreshBrewApi
{
    public class Program
    {
        static Serilog.ILogger log = null;
        public static void Main(string[] args)
        {             
            try
            {                
                string logfile = Path.Combine($"{Environment.CurrentDirectory}", "log.txt");
                
                Log.Logger = new LoggerConfiguration()
                  .Enrich.FromLogContext()
                  .MinimumLevel.Information()
                  .WriteTo.File(logfile, fileSizeLimitBytes: 5000000, rollOnFileSizeLimit: true, shared: true, flushToDiskInterval: TimeSpan.FromSeconds(1), retainedFileCountLimit: 5,
                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                  .CreateLogger();

                Log.Information("Starting FreshBrewApi");
                BuildWebHost(args).Run();                
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Fatal(ex, "Host terminated unexpectedly");
               
            }
            finally
            {
                Log.CloseAndFlush();
            }
           
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            int port = 8080;
            if (args.Length != 0)
            {
                try
                {
                    // arg will be of form: /port:value=5002
                    Console.WriteLine("Argument passed:" + args[0] +" (Port should be passed as: /port:value=9008)");
                    string arg1 = args[0].ToLower();
                    if (arg1.StartsWith("/port"))
                    {
                        string[] tokens = arg1.Split(":");
                        string value = tokens[1];
                        if (tokens.Length == 2 && value.StartsWith("value"))
                        {
                            tokens = value.Split("=");
                            if (tokens.Length == 2)
                            {
                                port = Convert.ToInt32(tokens[1]); //index=1 will have port number
                                Console.WriteLine("Port number from command line:" + port);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Application uses default port of 9003. To override default port, use /port:value=<number>");
                    Console.WriteLine("Exception:" + ex.ToString());
                }
            }
            Riverbed.Test.FreshBrew.LoadScript.Load.execute();
            System.Net.IPAddress ipaddress = System.Net.IPAddress.Parse("0.0.0.0");
            return WebHost.CreateDefaultBuilder(args)
                 .UseStartup<Startup>()
                 .UseKestrel(options =>
                 {
                     Console.WriteLine("Application's port:" + port);
                     //instead of loopback, use 0.0.0.0
                     // options.Listen(IPAddress.Loopback, port);
                     options.Listen(ipaddress, port);
                 })
                 .UseSerilog(log)
                 .Build();
        }
        /*
       public static IWebHost BuildWebHost(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .UseSerilog(log)
               .UseKestrel(options =>
               {
                   options.Listen(IPAddress.Loopback, 5001);
               })
               .Build(); */
    }
}
