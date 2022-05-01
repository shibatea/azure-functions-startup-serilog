# azure-functions-startup-serilog

Serilog を実装した Azure Functions プロジェクトのサンプルです

## テンプレートから実装した機能

- パッケージの追加
  - Serilog.AspNetCore
  - Serilog.Exceptions

- 以下は必要に応じて
  - Serilog.Exceptions.EntityFrameworkCore

- DI 機能の追加
  - Microsoft.Azure.Functions.Extensions
  - Microsoft.Extensions.DependencyInjection

- Startup クラスの追加

```csharp
[assembly: FunctionsStartup(typeof(Startup))]

namespace FunctionApp4;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
    }
}
```

- Serilog の構成

```csharp
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
```

- Serilog のカスタムログプロバイダーを追加

```csharp
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSerilog(logger, true);
});
```

- ILogger<T> をコンストラクターに注入

```csharp
private readonly ILogger<Function1> _logger;

public Function1(ILogger<Function1> logger)
{
    _logger = logger;
}
```
