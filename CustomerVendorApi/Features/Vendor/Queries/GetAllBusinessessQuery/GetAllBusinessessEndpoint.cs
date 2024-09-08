using Carter;
using MediatR;

namespace CustomerVendorApi.Features.Vendor.Queries.GetAllBusinessessQuery
{
    public class GetAllBusinessessEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/vendor/businesses", async (IMediator mediator) =>
            {
                var businesses = await mediator.Send(new GetAllBusinessessQuery());
                return businesses.Any() ? Results.Ok(businesses) : Results.NotFound("No businesses found for this vendor.");
            }).RequireAuthorization();
        }
    }
}
