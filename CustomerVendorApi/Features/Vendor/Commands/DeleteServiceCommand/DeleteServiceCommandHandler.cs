using CustomerVendorApi.Data;
using CustomerVendorApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CustomerVendorApi.Features.Vendor.Commands.DeleteServiceCommand
{
    public class DeleteServiceCommand : IRequest<int>
    {
        public int ServiceId { get; set; }
    }

    public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, int>
    {
        private readonly ProductsServicesDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteServiceCommandHandler(ProductsServicesDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            var vendorId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(vendorId))
            {
                throw new UnauthorizedAccessException("Vendor ID is missing from claims.");
            }

            // Find the service and ensure it belongs to the vendor's business
            var service = await _dbContext.Services
                .Include(s => s.Business)
                .FirstOrDefaultAsync(s => s.ServiceId == request.ServiceId && s.Business.VendorId == vendorId, cancellationToken);

            if (service == null)
            {
                throw new KeyNotFoundException("Service not found or does not belong to the vendor.");
            }

            // Remove the service
            _dbContext.Services.Remove(service);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return service.ServiceId;
        }
    }
}
