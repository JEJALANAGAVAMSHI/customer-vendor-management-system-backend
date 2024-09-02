using AuthenticationAPI.Contracts;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;

namespace AuthenticationAPI.Repository
{
    public class RegisterAdminRepository : IRegisterAdmin
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RegisterAdminRepository(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<Response> RegisterAdmin(RegisterAdmin model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return new Response { Status = "Error", Message = "User already exists!" };
            }
            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." };

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.Customer))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Customer));
            if (!await _roleManager.RoleExistsAsync(UserRoles.Vendor))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Vendor));
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            
            return new Response { Status = "Success", Message = "User created successfully!" };
        }

        
    }
}
