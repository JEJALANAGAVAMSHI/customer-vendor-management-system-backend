using CustomerVendorApi.Data;
using CustomerVendorApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CustomerVendorApi.Features.Vendor.Queries.GetAllOffersQuery
{
    public class GetAllOffersQuery : IRequest<List<Offer>>
    {
        public int BusinessId { get; set; }
    }
    public class GetAllOffersQueryHandler : IRequestHandler<GetAllOffersQuery, List<Offer>>
    {
        private readonly ProductsServicesDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllOffersQueryHandler(ProductsServicesDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Offer>> Handle(GetAllOffersQuery request, CancellationToken cancellationToken)
        {
            var vendorId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(vendorId))
            {
                throw new UnauthorizedAccessException("Vendor ID is missing from claims.");
            }

            // Check if the business belongs to the vendor
            var business = await _context.Businesses
                .Include(b => b.Offers)
                .FirstOrDefaultAsync(b => b.BusinessId == request.BusinessId && b.VendorId == vendorId, cancellationToken);

            if (business == null)
            {
                throw new ArgumentException("Business does not exist or does not belong to the vendor.");
            }

            // Return the list of events associated with the business
            return business.Offers.ToList();
        }
    }
}
