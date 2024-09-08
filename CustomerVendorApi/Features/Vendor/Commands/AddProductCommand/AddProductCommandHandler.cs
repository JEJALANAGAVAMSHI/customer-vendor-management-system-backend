using CustomerVendorApi.Data;
using CustomerVendorApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CustomerVendorApi.Features.Vendor.Commands.AddProductCommand
{
    public class AddProductCommand : IRequest<bool>
    {
        public int BusinessId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, bool>
    {
        private readonly ProductsServicesDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddProductCommandHandler(ProductsServicesDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var vendorId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(vendorId))
            {
                throw new UnauthorizedAccessException("Vendor ID is missing from claims.");
            }

            // Check if the business belongs to the vendor
            var business = await _context.Businesses
                .Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.BusinessId == request.BusinessId && b.VendorId == vendorId, cancellationToken);

            if (business == null)
            {
                throw new ArgumentException("Business does not exist or does not belong to the vendor.");
            }

            // Create a new service
            var product = new Product
            {
                ProductName = request.ProductName,
                Description = request.Description,
                Price = request.Price,
                BusinessId = request.BusinessId
            };

            // Add the service to the business's services collection
            business.Products.Add(product);

            // Add the service to the context
            _context.Products.Add(product);

            // Save changes to the database
            await _context.SaveChangesAsync(cancellationToken);

            // Verify that the service was added
            var updatedBusiness = await _context.Businesses
                .Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.BusinessId == request.BusinessId, cancellationToken);

            if (updatedBusiness?.Products == null || !updatedBusiness.Products.Any(s => s.ProductName == request.ProductName))
            {
                throw new InvalidOperationException("Failed to add the product to the business.");
            }

            return true;
        }
    }
}
