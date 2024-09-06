using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace AuthenticationAPI.Contracts
{
    public interface ICustomerRepository
    {
        public Task<IEnumerable> GetCustomers();
    }
}
