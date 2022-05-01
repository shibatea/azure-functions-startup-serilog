using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using MyFunctionWithSerilog;

using Serilog;
using Serilog.Events;

[assembly: FunctionsStartup(typeof(Startup))]

namespace MyFunctionWithSerilog;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Worker", LogEventLevel.Warning)
            .MinimumLevel.Override("Host", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Error)
            .MinimumLevel.Override("Function", LogEventLevel.Error)
            .MinimumLevel.Override("Azure.Storage.Blobs", LogEventLevel.Error)
            .MinimumLevel.Override("Azure.Core", LogEventLevel.Error)
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.File(
                $"logs\\{nameof(MyFunctionWithSerilog)}.txt",
                LogEventLevel.Information,
                rollingInterval: RollingInterval.Hour)
            .CreateLogger();

        // Case1
        // ドキュメントに書かれてるやり方
        // https://docs.microsoft.com/ja-jp/azure/azure-functions/functions-dotnet-dependency-injection
        // builder.Services.AddSingleton<ILoggerProvider>(provider => new SerilogLoggerProvider(logger, true));

        // Case2
        // 内部で Case1 の実装がされている
        builder.Services.AddLogging(loggingBuilder => { loggingBuilder.AddSerilog(logger, true); });
    }
}
