using Quartz;
using InventoryAlertApi.Jobs;
using InventoryAlertApi.Data;
using InventoryAlertApi.Services;
using Quartz.Simpl;
using Microsoft.EntityFrameworkCore;
using InventoryAlertApi.Models;
using InventoryAlertApi.RealHub;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<InventoryAlertJob>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddQuartz(q =>
{
    q.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();
    var jobKey = new JobKey("InventoryAlertJob");
    q.AddJob<InventoryAlertJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts.ForJob(jobKey).WithIdentity("InventoryAlertTrigger").WithSimpleSchedule(x =>
                                            x.WithIntervalInMinutes(5).RepeatForever()));
});
builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});
builder.Services.AddSignalR();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAnyOrigin");
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.MapControllers();
app.MapHub<RealTimeHub>("/realtimehub");
app.Run();