using CustomerVendorApi.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CustomerVendorApi.Features.Vendor.Commands.DeleteBusinessCommand
{
    public class DeleteBusinessCommand : IRequest<int>
    {
        public int BusinessId { get; set; }
    }

    public class DeleteBusinessCommandHandler : IRequestHandler<DeleteBusinessCommand, int>
    {
        private readonly ProductsServicesDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteBusinessCommandHandler(ProductsServicesDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> Handle(DeleteBusinessCommand request, CancellationToken cancellationToken)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var vendorIdClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (vendorIdClaim == null)
            {
                throw new UnauthorizedAccessException("Vendor ID is missing in the token.");
            }

            string vendorId = vendorIdClaim.Value;

            // Find the business by ID and ensure it belongs to the logged-in vendor
            var business = await _dbContext.Businesses
                .FirstOrDefaultAsync(b => b.BusinessId == request.BusinessId && b.VendorId == vendorId, cancellationToken);

            if (business == null)
            {
                throw new KeyNotFoundException("Business not found or you do not have permission to delete this business.");
            }

            // Remove the business
            _dbContext.Businesses.Remove(business);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return business.BusinessId;
        }
    }
}
