using CustomerVendorApi.Data;
using CustomerVendorApi.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CustomerVendorApi.Features.Vendor.Queries.GetBusinessByIdQuery
{
    public class GetBusinessByIdQuery : IRequest<BusinessByIdDto>
    {
        public int BusinessId { get; set; }

    }
    public class GetBusinessByIdQueryHandler : IRequestHandler<GetBusinessByIdQuery, BusinessByIdDto>
    {
        private readonly ProductsServicesDbContext _context;
        public GetBusinessByIdQueryHandler(ProductsServicesDbContext context)
        {
            _context = context;
        }
        public async Task<BusinessByIdDto> Handle(GetBusinessByIdQuery request, CancellationToken cancellationToken)
        {
            var business = await _context.Businesses
                .Include(b => b.Products)
                .Include(b => b.Services)
                .FirstOrDefaultAsync(b => b.BusinessId == request.BusinessId, cancellationToken);

            if (business == null)
            {
                return null;
            }

            var businessDto = new BusinessByIdDto
            {
                BusinessId = business.BusinessId,
                BusinessName = business.BusinessName,
                Category = business.Category,
                Address = business.Address,
                State = business.State,
                PostalCode = business.PostalCode,
                Email = business.Email,
                DayFrom = business.DayFrom,
                DayTo = business.DayTo,
                TimeFrom = business.TimeFrom,
                TimeTo = business.TimeTo,
                Products = business.Products,
                Services = business.Services
            };

            return businessDto;
        }
    
    }
}
