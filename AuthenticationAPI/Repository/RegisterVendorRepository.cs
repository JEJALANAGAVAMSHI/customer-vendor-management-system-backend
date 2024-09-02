using AuthenticationAPI.Contracts;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;

namespace AuthenticationAPI.Repository
{
    public class RegisterVendorRepository : IRegisterVendor
    {
        private readonly UserManager<IdentityUser> _userManager;
        public RegisterVendorRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Response> RegisterVendor(RegisterBusiness model)
        {
            var userExists = await _userManager.FindByNameAsync(model.vendor_email);
            if (userExists != null)
            {
                return new Response { Status = "Error", Message = "User Already Exists!!" };
            }
            IdentityUser user = new()
            {
                
                Email = model.vendor_email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.vendor_name,
                PhoneNumber = model.vendor_phone,
            };
            var result = await _userManager.CreateAsync(user, model.password);
            await _userManager.AddToRoleAsync(user, UserRoles.Vendor);
            if (!result.Succeeded)
            {
                return new Response { Status = "Error", Message = "User creation failed! Please check user details and try again!" };
            }
            return new Response { Status = "Success", Message = "User created successfully" };

        
    }
    }
}
