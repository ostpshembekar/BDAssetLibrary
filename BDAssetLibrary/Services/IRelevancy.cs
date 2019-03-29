using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDAssetLibrary.Services
{
    public interface IRelevancy
    {
        Dictionary<string, int> FindRelevancy(IList<string> items, IList<string> queries);
    }
}
