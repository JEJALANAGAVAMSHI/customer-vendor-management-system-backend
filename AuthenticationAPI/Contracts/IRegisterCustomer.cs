using AuthenticationAPI.Models;
using Microsoft.Win32;

namespace AuthenticationAPI.Contracts
{
    public interface IRegisterCustomer
    {
        public Task<Response> Register(ApplicationUser model);
    }
}
