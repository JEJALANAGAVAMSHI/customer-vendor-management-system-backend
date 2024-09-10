using System.Text.Json.Serialization;

namespace CustomerVendorApi.Models
{
    public class Offer
    {
        public int OfferId { get; set; }
        public int BusinessId { get; set; }
        public string OfferName { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        
        [JsonIgnore]
        public Business Business { get; set; }
    }
}
