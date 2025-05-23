using Quartz;
using InventoryAlertApi.Jobs;
using Quartz.Simpl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddQuartz(q =>
{
    q.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();
    var jobKey = new JobKey("InventoryAlertJob");
    q.AddJob<InventoryAlertJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts.ForJob(jobKey).WithIdentity("InventoryAlertTrigger").WithSimpleSchedule(x => 
                                            x.WithIntervalInMinutes(1).RepeatForever()));
});
builder.Services.AddQuartzHostedService(options => {
    options.WaitForJobsToComplete = true;
});

var app=builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();