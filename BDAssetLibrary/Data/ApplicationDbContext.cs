using BDAssetLibrary.Config;
using BDAssetLibrary.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BDAssetLibrary.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IMongoDatabase _database = null;

        public ApplicationDbContext(IOptions<DbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Partner> Partners => _database.GetCollection<Partner>("Partners");
    }
}
