namespace AuthenticationAPI.Models
{
    public class RegisterBusiness
    {
        public int vendor_id { get; set; }
        public string vendor_name { get; set; }
        public string vendor_email {  get; set; }
        public string vendor_phone { get; set;}
        public string vendor_address { get; set;}
        public string password { get; set;}
    }
}
