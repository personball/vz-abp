using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace CompanyName.ProjectName.Web;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
#endif
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning)
#if DEBUG
                .MinimumLevel.Override("CompanyName.ProjectName", LogEventLevel.Debug)
#else
                .MinimumLevel.Override("CompanyName.ProjectName", LogEventLevel.Information)
#endif
            .Enrich.FromLogContext()
#if DEBUG
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
            .WriteTo.Async(c => c.Console())
#else
            .WriteTo.Async(c => c.Console(new JsonFormatter()))
#endif

            .CreateLogger();

        try
        {
            Log.Information("Starting web host.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson(reloadOnChange: false)
                .UseAutofac()
                 .UseSerilog((ctx, config) =>
                {
                    config.ReadFrom
                        .Configuration(ctx.Configuration.GetSection("Logging:ProjectName"))
                        .Enrich.FromLogContext()
#if DEBUG
                        .WriteTo.Async(c => c.Console());
#else
                        .WriteTo.Async(c => c.Console(new JsonFormatter()));
#endif
                });
            await builder.AddApplicationAsync<ProjectNameWebModule>(opt => opt.Configuration.ReloadOnChange = false);
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                throw;
            }

            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
