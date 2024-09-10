namespace CustomerVendorApi.Models
{
    public class Event
    {
        public int EventId { get; set; } 
        public int BusinessId { get; set; }
        public string EventName { get; set; } 
        public string Description { get; set; } 
        public DateTime Date { get; set; } 
        public TimeOnly TimeFrom { get; set; } 
        public TimeOnly TimeTo { get; set; } 

        public Business Business { get; set; } // Navigation property to Busines
    }
}
