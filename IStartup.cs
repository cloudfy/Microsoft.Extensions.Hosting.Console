using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Hosting.Console
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        IServiceProvider ConfigureServices(IServiceCollection services);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        void Configure(IHostBuilder app);
    }

    public interface IStartupFilter
    {
        Action<IHostBuilder> Configure(Action<IHostBuilder> next);
    }

    internal interface ISupportsStartup
    {
        IHostBuilder Configure(Action<HostBuilderContext, IHostBuilder> configure);
        IHostBuilder UseStartup(Type startupType);
    }

    public interface IStartupConfigureContainerFilter<TContainerBuilder>
    {
        Action<TContainerBuilder> ConfigureContainer(Action<TContainerBuilder> container);
    }

    public interface IStartupConfigureServicesFilter
    {
        Action<IServiceCollection> ConfigureServices(Action<IServiceCollection> next);
    }
}
