using CustomerVendorApi.Data;
using CustomerVendorApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CustomerVendorApi.Features.Vendor.Commands.AddLocationCommand
{
    public class AddLocationCommand : IRequest<int>
    {
        public int businessId { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
    public class AddLocationCommandHandler : IRequestHandler<AddLocationCommand, int>
    {
        private readonly ProductsServicesDbContext _context;

        public AddLocationCommandHandler(ProductsServicesDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(AddLocationCommand request, CancellationToken cancellationToken)
        {
            var business = await _context.Businesses
            .Include(b => b.Location) // Include the location to check if it exists
            .FirstOrDefaultAsync(b => b.BusinessId == request.businessId);

            if (business == null)
            {
                throw new Exception("Business not found.");
            }

            // Check if the business already has a location
            if (business.Location != null)
            {
                // Update the existing location
                business.Location.Latitude = request.latitude;
                business.Location.Longitude = request.longitude;
            }
            else
            {
                // Create a new location if it doesn't exist
                var businessLocation = new Location
                {
                    Latitude = request.latitude,
                    Longitude = request.longitude,
                    BusinessId = request.businessId
                };

                _context.Location.Add(businessLocation);
            }
            await _context.SaveChangesAsync(cancellationToken);

            return business.Location?.LocationId ?? business.BusinessId;
        }
    }
}
