namespace CustomerVendorApi.DTO
{
    public class RegisterCustomerDto
    {
        public string UserName { get; set; }      
        public string Email { get; set; }      
        public string PasswordHash { get; set; }      
        public string PhoneNumber { get; set; }   
        public string Address { get; set; }        
        public string State { get; set; }         
        public string PostalCode { get; set; }
    }
}
