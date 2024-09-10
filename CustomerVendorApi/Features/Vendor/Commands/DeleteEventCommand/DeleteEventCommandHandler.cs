using CustomerVendorApi.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CustomerVendorApi.Features.Vendor.Commands.DeleteEventCommand
{
    public class DeleteEventCommand : IRequest<int>
    {
        public int EventId { get; set; }
    }
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, int>
    {
        private readonly ProductsServicesDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteEventCommandHandler(ProductsServicesDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<int> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var vendorId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(vendorId))
            {
                throw new UnauthorizedAccessException("Vendor ID is missing from claims.");
            }


            var eventToAdd = await _dbContext.Events
                .Include(p => p.Business)
                .FirstOrDefaultAsync(p => p.EventId == request.EventId && p.Business.VendorId == vendorId, cancellationToken);

            if (eventToAdd == null)
            {
                throw new KeyNotFoundException("Event not found or does not belong to the vendor.");
            }

            
            _dbContext.Events.Remove(eventToAdd);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return eventToAdd.EventId;
        }
    
    }
}
