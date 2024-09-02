using AuthenticationAPI.Models;
using Microsoft.Win32;

namespace AuthenticationAPI.Contracts
{
    public interface IRegisterAdmin
    {
        public Task<Response> RegisterAdmin(RegisterAdmin model);
    }
}
