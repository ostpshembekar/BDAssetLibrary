using BDAssetLibrary.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDAssetLibrary.Services
{
    public interface IPartnerRepository
    {
        Task<IList<Partner>> GetPartners();
    }
}
