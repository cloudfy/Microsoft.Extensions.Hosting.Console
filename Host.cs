using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Microsoft.Extensions.Hosting.Console
{
    /// <summary>
    /// 
    /// </summary>
    public static class Host
    {
        //
        // Summary:
        //     Initializes a new instance of the Microsoft.Extensions.Hosting.HostBuilder
        //     class with pre-configured defaults.
        //
        // Returns:
        //     The initialized Microsoft.Extensions.Hosting.IHostBuilder.
        //
        // Remarks:
        //     The following defaults are applied to the returned Microsoft.Extensions.Hosting.HostBuilder:
        //     load Microsoft.Extensions.Configuration.IConfiguration from 'appsettings.json' and 
        //     'appsettings.[Microsoft.Extensions.Hosting.IHostEnvironment.EnvironmentName].json', 
        //     load Microsoft.Extensions.Configuration.IConfiguration
        //     from environment variables, configure the Microsoft.Extensions.Logging.ILoggerFactory
        //     to log to the console and debug output.
        public static IHostBuilder CreateDefaultBuilder()
            => CreateDefaultBuilder(args: null);

        //
        // Summary:
        //     Initializes a new instance of the Microsoft.AspNetCore.Hosting.WebHostBuilder
        //     class with pre-configured defaults.
        //
        // Parameters:
        //   args:
        //     The command line args.
        //
        // Returns:
        //     The initialized Microsoft.AspNetCore.Hosting.IWebHostBuilder.
        //
        // Remarks:
        //     The following defaults are applied to the returned Microsoft.AspNetCore.Hosting.WebHostBuilder:
        //     use Kestrel as the web server and configure it using the application's configuration
        //     providers, set the Microsoft.AspNetCore.Hosting.IHostEnvironment.ContentRootPath
        //     to the result of System.IO.Directory.GetCurrentDirectory, load Microsoft.Extensions.Configuration.IConfiguration
        //     from 'appsettings.json' and 'appsettings.[Microsoft.AspNetCore.Hosting.IHostEnvironment.EnvironmentName].json',
        //     load Microsoft.Extensions.Configuration.IConfiguration from User Secrets when
        //     Microsoft.AspNetCore.Hosting.IHostEnvironment.EnvironmentName is 'Development'
        //     using the entry assembly, load Microsoft.Extensions.Configuration.IConfiguration
        //     from environment variables, load Microsoft.Extensions.Configuration.IConfiguration
        //     from supplied command line args, configure the Microsoft.Extensions.Logging.ILoggerFactory
        //     to log to the console and debug output, and enable IIS integration.
        public static IHostBuilder CreateDefaultBuilder(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            string applicationName = Environment.GetEnvironmentVariable("APPLICATION_NAME");

            var host = new HostBuilder()
                .ConfigureHostConfiguration((builder) =>
                {
                    // host leve
                    builder
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureAppConfiguration((ctx, builder) =>
                {
                    // initialize the IHostEnvironment
                    // mirrors useconfiguration
                    builder
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json"
                                , optional: true, reloadOnChange: true);

                    // if not environment, use production //.NET Core default behaviour
                    if (string.IsNullOrEmpty(environment))
                        builder.AddJsonFile("appsettings.production.json"
                            , optional: true, reloadOnChange: true);
                    builder.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    hostContext.HostingEnvironment.ApplicationName = applicationName ?? "ConsoleHost";
                })
                .UseEnvironment(environment ?? "Production");

            ConfigureHostDefaults(host);

            return host;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        internal static void ConfigureHostDefaults(IHostBuilder builder)
        {
            // istens for shutdown signals
            builder.UseConsoleLifetime();
        }

        ////
        //// Summary:
        ////     Initializes a new instance of the Microsoft.AspNetCore.Hosting.WebHostBuilder
        ////     class with pre-configured defaults using typed Startup.
        ////
        //// Parameters:
        ////   args:
        ////     The command line args.
        ////
        //// Type parameters:
        ////   TStartup:
        ////     The type containing the startup methods for the application.
        ////
        //// Returns:
        ////     The initialized Microsoft.AspNetCore.Hosting.IWebHostBuilder.
        ////
        //// Remarks:
        ////     The following defaults are applied to the returned Microsoft.AspNetCore.Hosting.WebHostBuilder:
        ////     use Kestrel as the web server and configure it using the application's configuration
        ////     providers, set the Microsoft.AspNetCore.Hosting.IHostEnvironment.ContentRootPath
        ////     to the result of System.IO.Directory.GetCurrentDirectory, load Microsoft.Extensions.Configuration.IConfiguration
        ////     from 'appsettings.json' and 'appsettings.[Microsoft.AspNetCore.Hosting.IHostEnvironment.EnvironmentName].json',
        ////     load Microsoft.Extensions.Configuration.IConfiguration from User Secrets when
        ////     Microsoft.AspNetCore.Hosting.IHostEnvironment.EnvironmentName is 'Development'
        ////     using the entry assembly, load Microsoft.Extensions.Configuration.IConfiguration
        ////     from environment variables, load Microsoft.Extensions.Configuration.IConfiguration
        ////     from supplied command line args, configure the Microsoft.Extensions.Logging.ILoggerFactory
        ////     to log to the console and debug output, enable IIS integration.
        public static IHostBuilder CreateDefaultBuilder<TStartup>(string[] args) where TStartup : class, IStartup
        {
            return CreateDefaultBuilder(args)
                .UseStartup<TStartup>();
        }
    }
}
