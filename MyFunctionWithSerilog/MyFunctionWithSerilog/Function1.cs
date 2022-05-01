﻿using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace MyFunctionWithSerilog;

public class Function1
{
    private readonly ILogger<Function1> _logger;

    public Function1(ILogger<Function1> logger) => _logger = logger;

    [FunctionName("Function1")]
    public void Run([QueueTrigger("myqueue-items-1", Connection = "")] string myQueueItem) =>
        _logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
}
