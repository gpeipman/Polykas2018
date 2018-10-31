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

namespace MediaGallery
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateFileLogger();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        public static void CreateFileLogger()

        {

            Log.Logger = new LoggerConfiguration()

                           .WriteTo.File("Logs/Example.txt",

                                    LogEventLevel.Information, // Minimum Log level

                                    rollingInterval: RollingInterval.Day, // This will append time period to the filename like Example20180316.txt

                                    retainedFileCountLimit: null,

                                    fileSizeLimitBytes: null,

                                    outputTemplate: "{Timestamp:dd-MMM-yyyy HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",  // Set custom file format

                                    shared: true // Shared between multi-process shared log files

                                    )

                            .CreateLogger();

        }

    }
}
