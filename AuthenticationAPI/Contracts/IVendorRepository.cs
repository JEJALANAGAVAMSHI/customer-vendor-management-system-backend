using AuthenticationAPI.Models;

namespace AuthenticationAPI.Contracts
{
    public interface IVendorRepository
    {
        public Task<IEnumerable<VendorDto>> GetVendors();
        public Task<bool> DeleteVendorAsync(string vendorId);
    }
}
