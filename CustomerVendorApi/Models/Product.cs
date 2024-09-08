using System.Text.Json.Serialization;

namespace CustomerVendorApi.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int BusinessId { get; set; }
        [JsonIgnore]
        public Business Business { get; set; }
    }
}
