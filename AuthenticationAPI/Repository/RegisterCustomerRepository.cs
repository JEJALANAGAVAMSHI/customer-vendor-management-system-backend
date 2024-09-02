using AuthenticationAPI.Contracts;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;

namespace AuthenticationAPI.Repository
{
    public class RegisterCustomerRepository : IRegisterCustomer
    {
        private readonly UserManager<IdentityUser> _userManager;
        public RegisterCustomerRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Response> Register(RegisterCustomer model)
        {
            var userExists = await _userManager.FindByNameAsync(model.customer_name);
            if (userExists != null)
            {
                return new Response { Status = "Error", Message = "User Already Exists!!" };
            }
            IdentityUser user = new()
            {
                Email = model.customer_email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.customer_name,
                
            };
            var result = await _userManager.CreateAsync(user, model.password);
            await _userManager.AddToRoleAsync(user, UserRoles.Customer);
            if (!result.Succeeded)
            {
                return new Response { Status = "Error", Message = "User creation failed! Please check user details and try again!" };
            }
            return new Response { Status = "Success", Message = "User created successfully" };

        }
    }
}
