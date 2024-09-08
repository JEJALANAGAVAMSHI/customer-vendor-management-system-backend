namespace CustomerVendorApi.Models
{
    public class Vendor
    {
        public string VendorId { get; set; }
        public string UserName { get; set; }

        public ICollection<Business> Businesses { get; set; } = new List<Business>();
    }
}
