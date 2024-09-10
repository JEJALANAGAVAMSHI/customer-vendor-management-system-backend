using CustomerVendorApi.Data;
using CustomerVendorApi.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CustomerVendorApi.Features.Vendor.Queries.GetAllBusinessessQuery
{
    public class GetAllBusinessessQuery : IRequest<List<Business>>
    {

    }
    public class GetAllBusinessessQueryHandler : IRequestHandler<GetAllBusinessessQuery, List<Business>>
    {
        private readonly ProductsServicesDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetAllBusinessessQueryHandler(ProductsServicesDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Business>> Handle(GetAllBusinessessQuery request, CancellationToken cancellationToken)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var vendorIdClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (vendorIdClaim == null)
            {
                throw new UnauthorizedAccessException("Vendor ID is missing in the token");
            }

            string vendorId = vendorIdClaim.Value;

            // Fetch businesses of the vendor
            var businesses = await _context.Businesses
                .Where(b => b.VendorId == vendorId)
                .Include(b => b.Services)  // Include services
                .Include(b => b.Products)
                .Include(b => b.Events)
                .ToListAsync(cancellationToken);

            return businesses;
        }
    }
}
