# Microsoft.Extensions.Hosting.Console - Boilerplace for dependency injection.

[![NuGet version (NetCoreHostingConsole)](https://img.shields.io/nuget/v/NetCoreHostingConsole.svg?style=flat-square)](https://www.nuget.org/packages/NetCoreHostingConsole/)

- [License](LICENSE)
- [Stack Overflow](https://stackoverflow.com/questions/tagged/netcorehostingconsole)

Extensions for generic host based applications. Makes the [generic-host work for console applications](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host).

# Getting Started
Create a new console application project, add the [NuGet Package](https://www.nuget.org/packages/NetCoreHostingConsole/), and edit the *Program.cs* file.

Include the following namespaces
```
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Console;
```

Change the *Main* method, and extend as follows:

```
public static void Main(string[] args)
{
    CreateHostBuilder(args).Build().RunConsole();
}

// main host builder 
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Microsoft.Extensions.Hosting.Console.Host.CreateDefaultBuilder(args)
        .UseStartup<Startup>();
```

Add a new file *startup.cs*

```
public class Startup
{

// constructor
public Startup(IConfiguration configuration)
{
}

// 
public void ConfigureServices(IServiceCollection services)
{
    // add all DI .Add() services...
}

public void Configure(IHostBuilder app, IHostEnvironment env, ILoggerFactory loggerFactory)
{
    // add all DI .Use() ....
}
}
```