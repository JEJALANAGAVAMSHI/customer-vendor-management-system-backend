using CustomerVendorApi.Data;
using CustomerVendorApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CustomerVendorApi.Features.Vendor.Commands.AddOfferCommand
{
    public class AddOfferCommand : IRequest<bool>
    {
        public int BusinessId { get; set; }
        public string OfferName { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
    public class AddOfferCommandHandler : IRequestHandler<AddOfferCommand, bool>
    {
        private readonly ProductsServicesDbContext _context;

        public AddOfferCommandHandler(ProductsServicesDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(AddOfferCommand request, CancellationToken cancellationToken)
        {
            var business = await _context.Businesses
                .Include(b => b.Offers)
                .FirstOrDefaultAsync(b => b.BusinessId == request.BusinessId, cancellationToken);

            if (business == null)
            {
                return false; 
            }

            // Create a new event
            var newOffer = new Offer
            {
                BusinessId = request.BusinessId,
                OfferName = request.OfferName,
                Description = request.Description,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo
            };

            // Add the event to the Events table
            _context.Offers.Add(newOffer);

            // Add the event to the business's event list
            business.Offers.Add(newOffer);

            // Save changes to the database
            var result = await _context.SaveChangesAsync(cancellationToken);

            var updatedOffer = await _context.Businesses
                .Include(b => b.Offers)
                .FirstOrDefaultAsync(b => b.BusinessId == request.BusinessId, cancellationToken);

            if (updatedOffer?.Offers == null || !updatedOffer.Offers.Any(s => s.OfferName == request.OfferName))
            {
                throw new InvalidOperationException("Failed to add the offer to the business.");
            }

            return result > 0;
        }
    }
}
