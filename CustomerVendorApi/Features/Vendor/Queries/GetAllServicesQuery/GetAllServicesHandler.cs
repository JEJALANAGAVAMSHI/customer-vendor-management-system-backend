using CustomerVendorApi.Data;
using CustomerVendorApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CustomerVendorApi.Features.Vendor.Queries.GetAllServicesQuery
{
    public class GetAllServicesQuery : IRequest<List<Service>>
    {
        public int BusinessId { get; set; }
    }
    public class GetAllServicesHandler : IRequestHandler<GetAllServicesQuery, List<Service>>
    {
        private readonly ProductsServicesDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetAllServicesHandler(ProductsServicesDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Service>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
        {
            var vendorId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(vendorId))
            {
                throw new UnauthorizedAccessException("Vendor ID is missing from claims.");
            }

            // Check if the business belongs to the vendor
            var business = await _context.Businesses
                .Include(b => b.Services) // Include services
                .FirstOrDefaultAsync(b => b.BusinessId == request.BusinessId && b.VendorId == vendorId, cancellationToken);

            if (business == null)
            {
                throw new ArgumentException("Business does not exist or does not belong to the vendor.");
            }

            // Return the list of services associated with the business
            return business.Services.ToList();
        }
    }
}
