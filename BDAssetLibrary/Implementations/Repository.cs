using BDAssetLibrary.Config;
using BDAssetLibrary.Data;
using BDAssetLibrary.DomainModels;
using BDAssetLibrary.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDAssetLibrary.Implementations
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;

        public Repository(IOptions<DbSettings> settings)
        {
            _context = new ApplicationDbContext(settings);
        }
        public async Task<IList<Partner>> GetPartners()
        {
            return await _context.Partners.Find(_ => true).ToListAsync();
        }
    }
}
