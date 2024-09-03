using AuthenticationAPI.Contracts;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;

namespace AuthenticationAPI.Repository
{
    public class RegisterVendorRepository : IRegisterVendor
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public RegisterVendorRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Response> RegisterVendor(ApplicationUser model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Email);
            if (userExists != null)
            {
                return new Response { Status = "Error", Message = "User Already Exists!!" };
            }
            ApplicationUser user = new()
            {
                
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            };
            var result = await _userManager.CreateAsync(user, model.PasswordHash);
            await _userManager.AddToRoleAsync(user, UserRoles.Vendor);
            if (!result.Succeeded)
            {
                return new Response { Status = "Error", Message = "User creation failed! Please check user details and try again!" };
            }
            return new Response { Status = "Success", Message = "User created successfully" };

        
    }
    }
}
