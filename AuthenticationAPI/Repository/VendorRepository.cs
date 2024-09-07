using AuthenticationAPI.Contracts;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationAPI.Repository
{
    public class VendorRepository : IVendorRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public VendorRepository(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> DeleteVendorAsync(string vendorId)
        {
            // Find the user by their ID
            var vendor = await _userManager.FindByIdAsync(vendorId);

            // Check if the user exists
            if (vendor == null)
            {
                // Vendor not found
                return false;
            }

            // Delete the vendor
            var result = await _userManager.DeleteAsync(vendor);

            // Return true if the deletion was successful, otherwise false
            return result.Succeeded;
        }

        public async Task<IEnumerable<VendorDto>> GetVendors()
        {
            var vendorRole = await _roleManager.FindByNameAsync("Vendor");
            if (vendorRole == null)
            {
                return null;
            }

            var vendors = await _userManager.GetUsersInRoleAsync(vendorRole.Name);

            var vendorDtos = vendors.Select(v => new VendorDto
            {
                Id = v.Id,
                UserName = v.UserName,
                Email = v.Email,
                PhoneNumber = v.PhoneNumber,
                Address = v.Address,
                State = v.State,
                PostalCode = v.PostalCode,

            }).ToList();

            return vendorDtos;
        }




        // Other methods, if any, would go here...
    }
}

