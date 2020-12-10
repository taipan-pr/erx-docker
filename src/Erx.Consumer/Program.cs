using Autofac;
using Erx.Database;
using Erx.Queue;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutofacModule = Erx.Queue.AutofacModule;

namespace Erx.Consumer
{
    public class Program
    {
        private static readonly AutoResetEvent WaitHandle = new AutoResetEvent(false);

        public static async Task Main(string[] args)
        {
            await Task.Delay(30);

            var container = InitializeContainer();
            await using var scope = container.BeginLifetimeScope();
            var queueService = scope.Resolve<IQueueService>();
            queueService.ConsumeQueue(async e => await PrintMessage(scope, e));
            Console.WriteLine("Start consuming queue");

            // Handle Control+C or Control+Break
            Console.CancelKeyPress += (o, e) =>
            {
                Console.WriteLine("Exit");
                // Allow the main thread to continue and exit...
                WaitHandle.Set();
            };
            // wait until Set method calls
            WaitHandle.WaitOne();
        }

        private static async Task PrintMessage(IComponentContext scope, string message)
        {
            var dbService = scope.Resolve<IDbService>();
            await dbService.CreateMessageAsync(message);
            Console.WriteLine(message);
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
            builder.RegisterModule<Database.AutofacModule>();
            return builder.Build();
        }
    }
}
