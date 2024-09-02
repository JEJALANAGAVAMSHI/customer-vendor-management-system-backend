namespace AuthenticationAPI.Models
{
    public class RegisterCustomer
    {
        public int customer_id {  get; set; }
        public string customer_name { get; set;}
        public string customer_email { get; set;}
        public string password { get; set;}
        public int mobile_number { get; set;}
        public string address { get; set;}
    }
}
