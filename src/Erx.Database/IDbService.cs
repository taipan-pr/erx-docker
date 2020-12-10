using Erx.Database.Models;
using System.Threading.Tasks;

namespace Erx.Database
{
    public interface IDbService
    {
        Task ExecuteScriptsAsync(params string[] scripts);
        Task<City> GetCityByNameAsync(string cityName);
        Task CreateMessageAsync(string message);
        Task<bool> CreateDatabaseIfNotExistAsync();
    }
}
