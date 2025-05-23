using Quartz;
using InventoryAlertApi.Jobs;
using InventoryAlertApi.Data;
using InventoryAlertApi.Services;
using Quartz.Simpl;
using Microsoft.EntityFrameworkCore;
using InventoryAlertApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<InventoryAlertJob>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddQuartz(q =>
{
    q.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();
    var jobKey = new JobKey("InventoryAlertJob");
    q.AddJob<InventoryAlertJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts.ForJob(jobKey).WithIdentity("InventoryAlertTrigger").WithSimpleSchedule(x => 
                                            x.WithIntervalInMinutes(10).RepeatForever()));
});
builder.Services.AddQuartzHostedService(options => {
    options.WaitForJobsToComplete = true;
});

var app=builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();