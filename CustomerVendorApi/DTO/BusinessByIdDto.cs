﻿using CustomerVendorApi.Models;

namespace CustomerVendorApi.DTO
{
    public class BusinessByIdDto : BusinessDto
    {
        public ICollection<Product> Products { get; set; }
        public ICollection<Service> Services { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
