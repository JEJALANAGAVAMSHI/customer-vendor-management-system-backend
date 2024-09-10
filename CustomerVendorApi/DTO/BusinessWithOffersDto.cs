namespace CustomerVendorApi.DTO
{
    public class BusinessWithOffersDto
    {
        public int OfferId { get; set; }
        public string OfferName { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string BusinessName { get; set; }
    }
}
