using Quartz;

namespace QuartzJobScheduling.Jobs;

public class MyJob : IJob
{
    private readonly ILogger<MyJob> _logger;

    public MyJob(ILogger<MyJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation($"InstanceId:{context.FireInstanceId} - {DateTime.Now}");

        return Task.CompletedTask;
    }
}