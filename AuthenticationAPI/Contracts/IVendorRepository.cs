namespace AuthenticationAPI.Contracts
{
    public interface IVendorRepository
    {
        public Task<bool> DeleteVendorAsync(string vendorId);
    }
}
