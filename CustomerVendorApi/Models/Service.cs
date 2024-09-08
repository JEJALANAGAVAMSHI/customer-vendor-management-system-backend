using System.Text.Json.Serialization;

namespace CustomerVendorApi.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int BusinessId { get; set; }
        [JsonIgnore]
        public Business Business { get; set; }
    }
}
