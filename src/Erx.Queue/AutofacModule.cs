using Autofac;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace Erx.Queue
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<QueueService>().AsImplementedInterfaces();

            builder.Register(x =>
            {
                var config = x.Resolve<IConfiguration>();
                var rabbitMqConfiguration = new RabbitMqOptions();
                config.GetSection("RabbitMQ").Bind(rabbitMqConfiguration);

                // override values from environment variables (if available)
                var host = Environment.GetEnvironmentVariable("HOST");
                if (!string.IsNullOrWhiteSpace(host)) rabbitMqConfiguration.HostName = host;
                var port = Environment.GetEnvironmentVariable("PORT");
                if (!string.IsNullOrWhiteSpace(port) && int.TryParse(port, out var p)) rabbitMqConfiguration.Port = p;
                var username = Environment.GetEnvironmentVariable("MQ_USERNAME");
                if (!string.IsNullOrWhiteSpace(username)) rabbitMqConfiguration.Username = username;
                var password = Environment.GetEnvironmentVariable("PASSWORD");
                if (!string.IsNullOrWhiteSpace(password)) rabbitMqConfiguration.Password = password;
                var queue = Environment.GetEnvironmentVariable("QUEUE");
                if (!string.IsNullOrWhiteSpace(queue)) rabbitMqConfiguration.QueueName = queue;

                return rabbitMqConfiguration;
            }).AsSelf().SingleInstance();

            builder.Register(x =>
            {
                var config = x.Resolve<RabbitMqOptions>();
                var uri = $"amqp://{config.Username}:{config.Password}@{config.HostName}:{config.Port}";
                var connectionFactory = new ConnectionFactory
                {
                    Uri = new Uri(uri)
                };
                return connectionFactory.CreateConnection().CreateModel();
            }).AsImplementedInterfaces();
        }
    }
}
