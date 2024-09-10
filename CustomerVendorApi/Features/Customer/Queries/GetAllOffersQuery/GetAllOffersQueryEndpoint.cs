using Carter;
using MediatR;

namespace CustomerVendorApi.Features.Customer.Queries.GetAllOffersQuery
{
    public class GetAllOffersQueryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/customer/offers", async (IMediator mediator) =>
            {
                var query = new GetAllOffersQuery();
                var offers = await mediator.Send(query);
                return Results.Ok(offers);
            });
        }
    }
}
