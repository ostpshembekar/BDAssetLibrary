using BDAssetLibrary.Config;
using BDAssetLibrary.Data;
using BDAssetLibrary.DomainModels;
using BDAssetLibrary.Services;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDAssetLibrary.Implementations
{
    public class PartnerRepository : IPartnerRepository
    {
        private readonly ApplicationDbContext _context;

        public PartnerRepository(IOptions<DbSettings> settings)
        {
            _context = new ApplicationDbContext(settings);
        }
        public async Task<IList<Partner>> GetPartners()
        {
            var filter = Builders<Partner>.Filter.Empty;
            var projection = Builders<Partner>.Projection.Include("_id")
                                                    .Include("name")
                                                    .Include("contact")
                                                    .Include("footprint")
                                                    .Include("potentialares")
                                                    .Include("contracts");
            var result = await _context.Partners.Find(filter).Project<Partner>(projection).ToListAsync();
            return result;
        }
    }
}
