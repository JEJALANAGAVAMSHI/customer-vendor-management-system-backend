using CustomerVendorApi.Data;
using CustomerVendorApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CustomerVendorApi.Features.Vendor.Commands.AddEventCommand
{
    public class AddEventCommand : IRequest<bool>
    {
        public int BusinessId { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeOnly TimeFrom { get; set; }
        public TimeOnly TimeTo { get; set; }
    }
    public class AddEventCommandHandler : IRequestHandler<AddEventCommand, bool>
    {
        private readonly ProductsServicesDbContext _context;

        public AddEventCommandHandler(ProductsServicesDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(AddEventCommand request, CancellationToken cancellationToken)
        {
            var business = await _context.Businesses
                .Include(b => b.Events)
                .FirstOrDefaultAsync(b => b.BusinessId == request.BusinessId, cancellationToken);

            if (business == null)
            {
                return false; // Business not found
            }

            // Create a new event
            var newEvent = new Event
            {
                BusinessId = request.BusinessId,
                EventName = request.EventName,
                Description = request.Description,
                Date = request.Date,
                TimeFrom = request.TimeFrom,
                TimeTo = request.TimeTo
            };

            // Add the event to the Events table
            _context.Events.Add(newEvent);

            // Add the event to the business's event list
            business.Events.Add(newEvent);

            // Save changes to the database
            var result = await _context.SaveChangesAsync(cancellationToken);

            var updatedEvent = await _context.Businesses
                .Include(b => b.Events)
                .FirstOrDefaultAsync(b => b.BusinessId == request.BusinessId, cancellationToken);

            if (updatedEvent?.Events == null || !updatedEvent.Events.Any(s => s.EventName == request.EventName))
            {
                throw new InvalidOperationException("Failed to add the event to the business.");
            }

            return result > 0;
        }
    }
}
