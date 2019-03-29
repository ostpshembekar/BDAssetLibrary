using BDAssetLibrary.Services;
using BDAssetLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDAssetLibrary.Pages.Partners
{
    public class IndexModel : PageModel
    {
        private readonly IPartnerRepository _repo;
        private readonly IRelevancy _relevancy;

        public IndexModel(IPartnerRepository repo, IRelevancy relevancy)
        {
            _repo = repo;
            _relevancy = relevancy;
        }

        [BindProperty]
        public IList<PartnerViewModel> Partners { get; set; }

        [BindProperty]
        public string Query { get; set; }

        [BindProperty]
        public int RecordCount { get; set; }

        public async Task OnGetAsync()
        {
            var partners = await _repo.GetPartners();
            ShowPartners(partners);
            SetRecordCount();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await SearchPartners();
            SetRecordCount();
            return Page();
        }

        private void ShowPartners(IList<DomainModels.Partner> partners)
        {
            var lstpartners = new List<PartnerViewModel>();
            foreach (var partner in partners)
            {
                var partnerviewmodel = new PartnerViewModel
                {
                    ID = partner.PartnerID.ToString(),
                    PartnerName = partner.name,
                    ContactName = partner.GetContactName(),
                    ContactEmail = partner.GetContactEmail(),
                    ContactPhone = partner.GetContactPhoneNo(),
                    Footprints = partner.GetFootPrintInfo()
                };
                lstpartners.Add(partnerviewmodel);
            }
            Partners = lstpartners;
        }

        private async Task SearchPartners()
        {
            var lstCustomers = await _repo.GetPartners();
            var query = GetListOfQueries();
            if (query == null || query.Count == 0)
            {
                ShowPartners(lstCustomers);
                return;
            }
            var lstFiltered = lstCustomers
                                .Where(c => query.Any(n => c.name.ToLower().Contains(n.ToLower()))
                                            || query.Any(gcn => c.GetContactName().ToLower().Contains(gcn.ToLower()))
                                            || query.Any(gce => c.GetContactEmail().ToLower().Contains(gce.ToLower()))
                                            || query.Any(gcp => c.GetContactPhoneNo().ToLower().Contains(gcp.ToLower()))
                                            || query.Any(gcf => c.GetFootPrintInfo().ToLower().Contains(gcf.ToLower())))
                                .ToList();
            var partners = CalculateRelevancy(lstFiltered, query);
            SetSearchResultsForView(partners);
        }

        private IList<DomainModels.Partner> CalculateRelevancy(IList<DomainModels.Partner> partners, List<string> queries)
        {
            Dictionary<string, int> namerelevancy = _relevancy.FindRelevancy(partners.Select(item => item.name).ToList(), queries);
            Dictionary<string, int> cnamerelevancy = _relevancy.FindRelevancy(partners.Select(item => item.GetContactName()).ToList(), queries);
            Dictionary<string, int> cemailrelevancy = _relevancy.FindRelevancy(partners.Select(item => item.GetContactEmail()).ToList(), queries);
            Dictionary<string, int> cphonerelevancy = _relevancy.FindRelevancy(partners.Select(item => item.GetContactPhoneNo()).ToList(), queries);
            Dictionary<string, int> footprintrelevancy = _relevancy.FindRelevancy(partners.Select(item => item.GetFootPrintInfo()).ToList(), queries);

            foreach (var partner in partners)
            {
                if (namerelevancy.ContainsKey(partner.name))
                    partner.Relevancy += namerelevancy[partner.name];
                if (cnamerelevancy.ContainsKey(partner.GetContactName()))
                    partner.Relevancy += cnamerelevancy[partner.GetContactName()];
                if (cemailrelevancy.ContainsKey(partner.GetContactEmail()))
                    partner.Relevancy += cemailrelevancy[partner.GetContactEmail()];
                if (cphonerelevancy.ContainsKey(partner.GetContactPhoneNo()))
                    partner.Relevancy += cphonerelevancy[partner.GetContactPhoneNo()];
                if (footprintrelevancy.ContainsKey(partner.GetFootPrintInfo()))
                    partner.Relevancy += footprintrelevancy[partner.GetFootPrintInfo()];
            }
            return partners;
        }

        private void SetSearchResultsForView(IList<DomainModels.Partner> partners)
        {
            if (partners == null) return;
            Partners = partners.GroupBy(u => u.PartnerID)
                                                    .Select(g => new PartnerViewModel
                                                    {
                                                        ID = g.Key.ToString(),
                                                        PartnerName = GetFormattedString(g.Select(item => item.name)),
                                                        ContactName = GetFormattedString(g.Select(item => item.GetContactName())),
                                                        ContactEmail = GetFormattedString(g.Select(item => item.GetContactEmail())),
                                                        ContactPhone = GetFormattedString(g.Select(item => item.GetContactPhoneNo())),
                                                        Footprints = GetFormattedString(g.Select(item => item.GetFootPrintInfo())),
                                                        Relevancy = g.Max(item => item.Relevancy)
                                                    })
                                                    .OrderByDescending(item => item.Relevancy)
                                                    .ToList();

            
        }

        /// <summary>
        /// Formats the list to be shown in the view
        /// 1. Creates a distinct list of items
        /// 2. Joins the list items as a comma delimited string
        /// </summary>
        /// <param name="list">list of items to be formatted</param>
        /// <returns></returns>
        private string GetFormattedString(IEnumerable<string> list)
        {
            var distinctItems = GetDistinctValues(list);
            return CreateCommaDelimitedString(distinctItems);
        }


        private string CreateCommaDelimitedString(IEnumerable<string> list)
        {
            if (list == null || list.Count().Equals(0))
                return string.Empty;
            return string.Join(", ", list);
        }

        private IEnumerable<string> GetDistinctValues(IEnumerable<string> list)
        {
            if (list == null || list.Count().Equals(0))
                return list;

            return list.Distinct();
        }

        private List<string> GetListOfQueries()
        {
            if (string.IsNullOrEmpty(Query)) return null;
            return Query.Split(" ").ToList();
        }

        private void SetRecordCount() => RecordCount = Partners?.Count ?? 0;
        
    }
}