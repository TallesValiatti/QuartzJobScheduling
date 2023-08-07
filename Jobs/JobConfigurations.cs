using Quartz;
using Quartz.Impl.AdoJobStore;

namespace QuartzJobScheduling.Jobs;

public static class JobConfigurations
{
    public static IServiceCollection AddJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            
            q.SchedulerId = Guid.NewGuid().ToString();
            q.SchedulerName = "Scheduler";
            
            q.UsePersistentStore(opts => 
            {
                opts.UseClustering();

                opts.UseJsonSerializer();

                opts.UsePostgres(o => 
                {
                    o.UseDriverDelegate<PostgreSQLDelegate>();
                    o.ConnectionString = configuration!.GetConnectionString("DefaultConnection")!;
                    o.TablePrefix = "qrtz_";
                });
            });

            var jobKey = new JobKey("MyJob");
            
            q.AddJob<MyJob>(opts => opts.WithIdentity(jobKey));
            
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("MyJob-trigger")
                .WithCronSchedule("0/5 * * * * ?")); // run every 5 seconds            
        });
 
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }
}