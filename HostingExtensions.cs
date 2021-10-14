using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System;
using Microsoft.Console.Hosting;
using Microsoft.Console.Hosting.Internal;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting.Console
{
    /// <summary>
    /// 
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static IHostBuilder UseStartup<TStartup>(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseStartup(typeof(TStartup));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="startupType"></param>
        /// <returns></returns>
        public static IHostBuilder UseStartup(this IHostBuilder hostBuilder, Type startupType)
        {
            //var startupAssemblyName = startupType.GetTypeInfo().Assembly.GetName().Name;

            ////DO hostBuilder.UseSetting(WebHostDefaults.ApplicationKey, startupAssemblyName);

            //// Light up the GenericWebHostBuilder implementation
            //if (hostBuilder is ISupportsStartup supportsStartup)
            //    return supportsStartup.UseStartup(startupType);

            //return hostBuilder
            //       .ConfigureServices((hostContext, services) =>
            //       {
            //           if (typeof(IStartup).GetTypeInfo().IsAssignableFrom(startupType.GetTypeInfo()))
            //           {
            //               services.AddSingleton(typeof(IStartup), startupType);
            //           }
            //           else
            //           {
            //               services.AddSingleton(typeof(IStartup), sp =>
            //               {
            //                   return new ConventionBasedStartup(StartupLoader.LoadMethods(sp, startupType, hostContext.HostingEnvironment.EnvironmentName));
            //               });
            //           }
            //       });




            //object classInstance = Activator.CreateInstance(startupType, null);

            //builder.ConfigureServices(sc => sc.ConfigureServices<TStartup>(classInstance));

            //IHost host = builder.Build();

            //builder.Configure(classInstance, host.Services);


            return hostBuilder
                   .ConfigureServices((hostContext, services) => {

                       //services.AddSingleton<IStartup>(
                       //    pv => new ConventionBasedStartup(StartupLoader.LoadMethods(pv, startupType, hostContext.HostingEnvironment.EnvironmentName)));

                       var sp = services.BuildServiceProvider();

                       IStartup convertedStartup = new ConventionBasedStartup(StartupLoader.LoadMethods(sp, startupType, hostContext.HostingEnvironment.EnvironmentName));
                       services.AddSingleton<IStartup>(convertedStartup);

                       convertedStartup.ConfigureServices(services);
                       hostContext.Properties.Add("startup", convertedStartup);
                       // convertedStartup.Configure(hostBuilder);

                       //object instance = ActivatorUtilities.GetServiceOrCreateInstance(services.BuildServiceProvider(), startupType);
                       //services.AddSingleton<IStartup>(instance as IStartup);
                       //instance.ConfigureServices(services);
                       //instance.Configure(hostBuilder);
                   });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        public static void RunConsole(this IHost host)
        {
            try
            {
                var startup = host.Services.GetService<IStartup>();
                //startup.Configure(host.Services);
            }
            catch 
            { 
            }
            host.Run();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public async static Task RunConsoleAsync(this IHost host)
        {
            try
            {
                var startup = host.Services.GetService<IStartup>();
            }
            catch
            {
            }
            await host.RunAsync();
        }
    }
}
