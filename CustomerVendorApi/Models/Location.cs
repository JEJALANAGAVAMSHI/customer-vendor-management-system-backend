using System.Text.Json.Serialization;

namespace CustomerVendorApi.Models
{
    public class Location
    {
        public int LocationId { get; set; } 
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int BusinessId { get; set; }
        [JsonIgnore]
        public Business Business { get; set; }
    }
}
