using QuartzJobScheduling.Jobs;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services)=>
    {
        services.AddJobs(builder.Configuration);
    })
    .Build();

host.Run();