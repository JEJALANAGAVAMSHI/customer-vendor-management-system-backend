using Carter;
using MediatR;

namespace CustomerVendorApi.Features.Vendor.Queries.GetAllOffersQuery
{
    public class GetAllOffersQueryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/businesses/{businessId}/offers", async (int businessId, IMediator mediator) =>
            {
                var query = new GetAllOffersQuery { BusinessId = businessId };
                var offers = await mediator.Send(query);

                if (offers == null || offers.Count == 0)
                {
                    return Results.NotFound("No Offers found for this business.");
                }

                return Results.Ok(offers);
            });
        }
    }
}
