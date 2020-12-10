using Autofac;
using Erx.Database;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Erx.Seeder
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Task.Delay(60);

            var scripts = (await File.ReadAllTextAsync(@"seed.sql"))
                .Split(Environment.NewLine)
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Select(e => e.Trim())
                .ToArray();

            if (!scripts.Any()) return;

            var container = InitializeContainer();
            await using var scope = container.BeginLifetimeScope();
            var dbService = scope.Resolve<IDbService>();
            var isCreated = await dbService.CreateDatabaseIfNotExistAsync();
            if (isCreated)
            {
                Console.WriteLine("Database is already exists");
                return;
            }
            await dbService.ExecuteScriptsAsync(await File.ReadAllTextAsync(@"seed.sql"));
            Console.WriteLine("Seed completed");
        }

        public static IContainer InitializeContainer()
        {
            var builder = new ContainerBuilder();
            builder.Register(x =>
                    new ConfigurationBuilder()
                        .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
                        .AddJsonFile("appsettings.json", false)
                        .AddEnvironmentVariables()
                        .Build())
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterModule<AutofacModule>();
            return builder.Build();
        }
    }
}
