using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // create builder object, create IHost instance, store in host
            var host = CreateHostBuilder(args).Build();

            // Create new IServiceScope that can be used to resolve scoped services.
            // Services come from ConfigureServices() in Startup.cs 
            // IServiceScope contains IServiceProvider to resolve dependencies from a newly created scope
            using var scope = host.Services.CreateScope();

            // Store scoped services in variable
            var services = scope.ServiceProvider;
            
            try 
            {
                // Get service of type DataContext that we defined in ConfigureServices() in Startup.cs
                // Migrate() applies any pending migrations for the context to the database,
                // and creates the database if it doesnt exist.
                var context = services.GetRequiredService<DataContext>();
                await context.Database.MigrateAsync();
                // Pass data context to SeedData.cs which adds premade data to database
                await Seed.SeedData(context);
            }
            catch (Exception ex)
            {
                // Create logger for error messaging
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error ocurred during migration");
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
