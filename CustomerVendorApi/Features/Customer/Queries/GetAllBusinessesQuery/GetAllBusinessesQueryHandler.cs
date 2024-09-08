using CustomerVendorApi.Data;
using CustomerVendorApi.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CustomerVendorApi.Features.Customer.Queries.GetAllBusinessesQuery
{
    public class GetAllBusinessesQuery : IRequest<IEnumerable<BusinessDto>>
    {
    }
    public class GetAllBusinessesQueryHandler : IRequestHandler<GetAllBusinessesQuery, IEnumerable<BusinessDto>>
    {
        private readonly ProductsServicesDbContext _context;
        public GetAllBusinessesQueryHandler(ProductsServicesDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BusinessDto>> Handle(GetAllBusinessesQuery request, CancellationToken cancellationToken)
        {
            var businesses = await _context.Businesses
                .Select(b => new BusinessDto
                {
                    BusinessName = b.BusinessName,
                    Category = b.Category,
                    Address = b.Address,
                    State = b.State,
                    PostalCode = b.PostalCode,
                    Email = b.Email,
                    DayFrom = b.DayFrom,
                    DayTo = b.DayTo,
                    TimeFrom = b.TimeFrom,
                    TimeTo = b.TimeTo
                })
                .ToListAsync(cancellationToken);

            return businesses;
        }
    }

}
