using AuthenticationAPI.Models;
using System.IdentityModel.Tokens.Jwt;

namespace AuthenticationAPI.Contracts
{
    public interface ILoginRepository
    {
        public Task<IResult> Login(Login model);
    }
}
