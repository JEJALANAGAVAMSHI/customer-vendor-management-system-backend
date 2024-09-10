using CustomerVendorApi.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CustomerVendorApi.Features.Vendor.Commands.DeleteOfferCommand
{
    public class DeleteOfferCommand : IRequest<int>
    {
        public int OfferId { get; set; }
    }
    public class DeleteOfferCommandHandler : IRequestHandler<DeleteOfferCommand, int>
    {
        private readonly ProductsServicesDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteOfferCommandHandler(ProductsServicesDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
        {
            var vendorId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(vendorId))
            {
                throw new UnauthorizedAccessException("Vendor ID is missing from claims.");
            }


            var offerToAdd = await _dbContext.Offers
                .Include(p => p.Business)
                .FirstOrDefaultAsync(p => p.OfferId == request.OfferId && p.Business.VendorId == vendorId, cancellationToken);

            if (offerToAdd == null)
            {
                throw new KeyNotFoundException("Offer not found or does not belong to the vendor.");
            }


            _dbContext.Offers.Remove(offerToAdd);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return offerToAdd.OfferId;
        }
    }
}
