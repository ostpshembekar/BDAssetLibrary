using BDAssetLibrary.Config;
using BDAssetLibrary.DomainModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDAssetLibrary.Data
{
    public class ApplicationDbContext
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
