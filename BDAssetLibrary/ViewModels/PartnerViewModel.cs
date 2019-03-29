using System.ComponentModel;

namespace BDAssetLibrary.ViewModels
{
    public class PartnerViewModel
    {
        public string ID { get; set; }
        [DisplayName("Partner Name")]
        public string PartnerName { get; set; }
        [DisplayName("Contact Name")]
        public string ContactName { get; set; }
        [DisplayName("Contact Email")]
        public string ContactEmail { get; set; }
        [DisplayName("Contact Phone")]
        public string ContactPhone { get; set; }
        [DisplayName("Has footprint in")]
        public string Footprints { get; set; }
        public int Relevancy { get; internal set; }
    }
}
