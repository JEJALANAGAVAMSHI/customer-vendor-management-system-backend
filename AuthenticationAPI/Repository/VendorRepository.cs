using AuthenticationAPI.Contracts;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationAPI.Repository
{
    public class VendorRepository : IVendorRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public VendorRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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





        // Other methods, if any, would go here...
    }
}

