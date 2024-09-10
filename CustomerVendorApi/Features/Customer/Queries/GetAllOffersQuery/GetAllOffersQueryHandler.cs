using CustomerVendorApi.Data;
using CustomerVendorApi.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CustomerVendorApi.Features.Customer.Queries.GetAllOffersQuery
{
    public class GetAllOffersQuery: IRequest<List<BusinessWithOffersDto>>
    {

    }
    public class GetAllOffersQueryHandler : IRequestHandler<GetAllOffersQuery, List<BusinessWithOffersDto>>
    {
        private readonly ProductsServicesDbContext _context;

        public GetAllOffersQueryHandler(ProductsServicesDbContext context)
        {
            _context = context;
        }
        public async Task<List<BusinessWithOffersDto>> Handle(GetAllOffersQuery request, CancellationToken cancellationToken)
        {
            var offers = await _context.Offers
                .Include(e => e.Business)
                .Select(e => new BusinessWithOffersDto
                {
                    OfferId = e.OfferId,
                    OfferName = e.OfferName,
                    Description = e.Description,
                    DateFrom = e.DateFrom,
                    DateTo = e.DateTo,
                    BusinessName = e.Business.BusinessName 
                })
                .ToListAsync(cancellationToken);

            return offers;
        }
    }
}
