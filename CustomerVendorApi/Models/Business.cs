namespace CustomerVendorApi.Models
{
    public class Business
    {
        public int BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string Category {  get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public int PostalCode { get; set; }
        public string Email { get; set; }
        public string DayFrom { get; set; }
        public string DayTo { get; set; }
        public TimeOnly TimeFrom { get; set; }
        public TimeOnly TimeTo { get; set;}
        public string VendorId { get; set; }

        public Vendor Vendor { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Service> Services { get; set; } = new List<Service>();

    }
}
