using CustomerVendorApi.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerVendorApi.Features.Admin.Commands.DeleteVendorCommand
{
    public class DeleteVendorCommand : IRequest<bool>
    {
        public string VendorId { get; set; }
        public string AuthorizationHeader { get; set; }
    }

    public class DeleteVendorCommandHandler : IRequestHandler<DeleteVendorCommand, bool>
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProductsServicesDbContext _dbContext;

        public DeleteVendorCommandHandler(IHttpClientFactory httpClientFactory, 
            IHttpContextAccessor httpContextAccessor,
            ProductsServicesDbContext dbContext)
        {
            _httpClient = httpClientFactory.CreateClient("AuthService");
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.AuthorizationHeader))
            {
                throw new HttpRequestException("Authorization header is missing.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AuthorizationHeader.Replace("Bearer ", ""));

            var response = await _httpClient.DeleteAsync($"api/auth/delete-vendor/{request.VendorId}", cancellationToken);
            if(!response.IsSuccessStatusCode)
            {
                return false;
            }

            // Delete from the Vendors table
            var vendor = await _dbContext.Vendors.FindAsync(request.VendorId);
            if (vendor == null)
            {
                throw new KeyNotFoundException($"Vendor with ID {request.VendorId} not found.");
            }

            // Delete associated businesses
            var businesses = await _dbContext.Businesses
                .Where(b => b.VendorId == request.VendorId)
                .ToListAsync();

            _dbContext.Businesses.RemoveRange(businesses);

            // Finally, delete the vendor
            _dbContext.Vendors.Remove(vendor);

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
