using CustomerVendorApi.Data;
using CustomerVendorApi.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CustomerVendorApi.Features.Vendor.Commands.AddServiceCommand
{
    public class AddServiceCommand : IRequest<bool>
    {
        public int BusinessId { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
    public class AddServiceCommandHandler : IRequestHandler<AddServiceCommand, bool>
    {
        private readonly ProductsServicesDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddServiceCommandHandler(ProductsServicesDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> Handle(AddServiceCommand request, CancellationToken cancellationToken)
        {
            var vendorId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(vendorId))
            {
                throw new UnauthorizedAccessException("Vendor ID is missing from claims.");
            }

            // Check if the business belongs to the vendor
            var business = await _context.Businesses
                .Include(b => b.Services)
                .FirstOrDefaultAsync(b => b.BusinessId == request.BusinessId && b.VendorId == vendorId, cancellationToken);

            if (business == null)
            {
                throw new ArgumentException("Business does not exist or does not belong to the vendor.");
            }

            // Create a new service
            var service = new Service
            {
                ServiceName = request.ServiceName,
                Description = request.Description,
                Price = request.Price,
                BusinessId = request.BusinessId
            };

            // Add the service to the business's services collection
            business.Services.Add(service);

            // Add the service to the context
            _context.Services.Add(service);

            // Save changes to the database
            await _context.SaveChangesAsync(cancellationToken);

            // Verify that the service was added
            var updatedBusiness = await _context.Businesses
                .Include(b => b.Services)
                .FirstOrDefaultAsync(b => b.BusinessId == request.BusinessId, cancellationToken);

            if (updatedBusiness?.Services == null || !updatedBusiness.Services.Any(s => s.ServiceName == request.ServiceName))
            {
                throw new InvalidOperationException("Failed to add the service to the business.");
            }

            return true;
        }
    }
}
