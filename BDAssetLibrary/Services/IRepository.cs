using BDAssetLibrary.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDAssetLibrary.Services
{
    public interface IRepository
    {
        Task<IList<Partner>> GetPartners();
    }
}
