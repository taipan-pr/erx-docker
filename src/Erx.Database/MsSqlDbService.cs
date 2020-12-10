using Dapper;
using Erx.Database.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Erx.Database
{
    internal class MsSqlDbService : IDbService
    {
        private readonly IDbConnection _connection;

        public MsSqlDbService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<bool> CreateDatabaseIfNotExistAsync()
        {
            var originalConnectionConnectionString = _connection.ConnectionString;
            var database = _connection.Database;
            var masterConnectionString = _connection.ConnectionString.Replace(database, "master");
            _connection.ConnectionString = masterConnectionString;
            _connection.Open();
            var isExist = await _connection.ExecuteScalarAsync<bool>($"If(db_id(N'{database}') IS NOT NULL) select 'true';else select 'false';");
            if (isExist) return true;
            await _connection.ExecuteAsync($"CREATE DATABASE [{database}]");
            _connection.Close();
            _connection.ConnectionString = originalConnectionConnectionString;
            return false;
        }

        public async Task ExecuteScriptsAsync(params string[] scripts)
        {
            foreach (var script in scripts)
            {
                await _connection.ExecuteAsync(script, commandType: CommandType.Text);
            }
        }

        public async Task<City> GetCityByNameAsync(string cityName)
        {
            const string sql = "SELECT * FROM Cities WHERE Name = @Name;";
            return await _connection.QueryFirstOrDefaultAsync<City>(sql, new { Name = cityName });
        }

        public async Task CreateMessageAsync(string message)
        {
            const string sql = "INSERT INTO [Messages] VALUES (@Guid, @Message, @MappingId);";
            var city = await GetCityByNameAsync(message);
            await _connection.ExecuteAsync(sql, new
            {
                Guid = Guid.NewGuid(),
                Message = message,
                MappingId = city?.Id
            });
        }
    }
}
