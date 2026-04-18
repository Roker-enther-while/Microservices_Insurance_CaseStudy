using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace PolicyService;

public static class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        var host = CreateWebHostBuilder(args).Build();
        
        InitializeDatabase(host);

        host.Run();
    }

    private static void InitializeDatabase(IWebHost host)
    {
        var config = host.Services.GetService(typeof(IConfiguration)) as IConfiguration;
        var connectionString = config.GetConnectionString("DefaultConnection");
        using (var conn = new Npgsql.NpgsqlConnection(connectionString))
        {
            conn.Open();
            using (var cmd = new Npgsql.NpgsqlCommand(@"
                CREATE TABLE IF NOT EXISTS outbox_messages (
                    id SERIAL PRIMARY KEY,
                    type VARCHAR(500),
                    json_payload TEXT
                );", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("hosting.json", true)
            .AddJsonFile("appsettings.json", true)
            .AddCommandLine(args)
            .Build();

        return WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(config)
            .UseStartup<Startup>()
            .UseSerilog();
    }
}