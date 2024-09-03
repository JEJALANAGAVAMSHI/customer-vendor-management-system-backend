using AuthenticationAPI.Models;
using Microsoft.Win32;

namespace AuthenticationAPI.Contracts
{
    public interface IRegisterVendor
    {
        public Task<Response> RegisterVendor(ApplicationUser model);
    }
}
