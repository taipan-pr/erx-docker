using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace Erx.Database
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MsSqlDbService>().AsImplementedInterfaces();

            builder.Register(x =>
            {
                var config = x.Resolve<IConfiguration>();
                var mssqlOptions = new MsSqlOptions();
                config.GetSection("MSSQL").Bind(mssqlOptions);

                // override values from environment variables (if available)
                var server = Environment.GetEnvironmentVariable("SERVER");
                if (!string.IsNullOrWhiteSpace(server)) mssqlOptions.Server = server;
                var password = Environment.GetEnvironmentVariable("SA_PASSWORD");
                if (!string.IsNullOrWhiteSpace(password)) mssqlOptions.Password = password;

                return mssqlOptions;
            }).AsSelf().SingleInstance();

            builder.Register(x =>
            {
                var config = x.Resolve<IConfiguration>();
                var mssqlOptions = x.Resolve<MsSqlOptions>();
                var connectionString = config.GetConnectionString("Default");
                connectionString = string.Format(connectionString, mssqlOptions.Server, mssqlOptions.Database, mssqlOptions.Password);
                return new SqlConnection(connectionString);
            }).AsImplementedInterfaces();
        }
    }
}
