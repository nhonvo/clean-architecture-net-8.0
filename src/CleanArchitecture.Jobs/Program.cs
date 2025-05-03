using CleanArchitecture.Jobs;
using Hangfire;
using Hangfire.Redis.StackExchange;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration.Get<AppSettings>();

builder.Services.AddSingleton(configuration);

// Background services
builder.Services.AddHostedService<BackgroundJobs>();

// Add Hangfire services.

var redis = ConnectionMultiplexer.Connect(configuration.RedisConnection);

builder.Services.AddHangfire(config => config.UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseRedisStorage(redis));

builder.Services.AddHangfireServer();

builder.Services.AddScoped<EmailService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard("/jobs", new DashboardOptions
    {
        Authorization = new[] { new HangfireBasicAuthenticationFilter("admin", "123456") }
    });
}

app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization = new[] { new HangfireBasicAuthenticationFilterDevelopment() }
});


RecurringJob.AddOrUpdate("print-hello", () => Console.WriteLine("Hello from Hangfire!"), Cron.Minutely);

RecurringJob.AddOrUpdate<EmailService>("daily-email", x => x.SendEmailReport(), Cron.Daily);

app.Run();

