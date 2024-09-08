using CustomerVendorApi.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CustomerVendorApi.Features.Vendor.Commands.DeleteProductCommand
{
    public class DeleteProductCommand : IRequest<int>
    {
        public int ProductId { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
    {
        private readonly ProductsServicesDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteProductCommandHandler(ProductsServicesDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var vendorId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(vendorId))
            {
                throw new UnauthorizedAccessException("Vendor ID is missing from claims.");
            }

            // Find the product and ensure it belongs to the vendor's business
            var product = await _dbContext.Products
                .Include(p => p.Business)
                .FirstOrDefaultAsync(p => p.ProductId == request.ProductId && p.Business.VendorId == vendorId, cancellationToken);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found or does not belong to the vendor.");
            }

            // Remove the product
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return product.ProductId;
        }
    }
}
