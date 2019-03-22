using BDAssetLibrary.Services;
using BDAssetLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [BindProperty]
        public int RecordCount { get; set; }

        public async Task OnGetAsync()
        {
            var lstpartners = new List<PartnerViewModel>();
            var partners = await _repo.GetPartners();
            foreach (var partner in partners)
            {
                var partnerviewmodel = new PartnerViewModel
                {
                    ID = partner.PartnerID.ToString(),
                    PartnerName = partner.name,
                    ContactName = partner.GetContactName(),
                    ContactEmail = partner.GetContactEmail(),
                    ContactPhone = partner.GetContactPhoneNo()
                };
                lstpartners.Add(partnerviewmodel);
            }
            Partners = lstpartners;
            SetRecordCount();
        }

        private void SetRecordCount() => RecordCount = Partners?.Count ?? 0;
        
    }
}