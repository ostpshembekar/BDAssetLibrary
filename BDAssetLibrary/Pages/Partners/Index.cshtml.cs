using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDAssetLibrary.Services;
using BDAssetLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BDAssetLibrary.Pages.Partners
{
    public class IndexModel : PageModel
    {
        private readonly IPartnerRepository _repo;

        public IndexModel(IPartnerRepository repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public IList<PartnerViewModel> Partners { get; set; }

        public async Task OnGetAsync()
        {
            var lstpartners = new List<PartnerViewModel>();
            var partners = await _repo.GetPartners();
            foreach(var partner in partners)
            {
                var partnerviewmodel = new PartnerViewModel
                {
                    ID = partner.PartnerID.ToString(),
                    PartnerName = partner.name,
                    ContactName = partner.contact["name"].AsString,
                    ContactEmail = partner.contact["email"].AsString,
                    ContactPhone = partner.contact["phoneno"].AsString
                };
                lstpartners.Add(partnerviewmodel);
            }
            Partners = lstpartners;
        }
    }
}