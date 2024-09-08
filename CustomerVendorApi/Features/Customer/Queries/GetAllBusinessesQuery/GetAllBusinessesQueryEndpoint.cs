using Carter;
using MediatR;

namespace CustomerVendorApi.Features.Customer.Queries.GetAllBusinessesQuery
{
    public class GetAllBusinessesQueryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/customer/get-businesses", async (IMediator mediator) =>
            {
                var query = new GetAllBusinessesQuery();
                var businesses = await mediator.Send(query);
                return Results.Ok(businesses);
            });
        }
    }
}
