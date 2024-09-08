namespace CustomerVendorApi.DTO
{
    public class BusinessDto
    {
        public string BusinessName { get; set; }
        public string Category { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public int PostalCode { get; set; }
        public string Email { get; set; }
        public string DayFrom { get; set; }
        public string DayTo { get; set; }
        public TimeOnly TimeFrom { get; set; }
        public TimeOnly TimeTo { get; set; }
    }
}
