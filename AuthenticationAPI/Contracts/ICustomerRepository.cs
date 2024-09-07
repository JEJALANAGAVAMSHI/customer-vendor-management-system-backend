using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace AuthenticationAPI.Contracts
{
    public interface ICustomerRepository
    {
        public Task<IEnumerable<CustomerDto>> GetCustomers();
    }
}
