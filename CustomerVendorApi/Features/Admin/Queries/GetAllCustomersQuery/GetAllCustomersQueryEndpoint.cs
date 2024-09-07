using Carter;
using MediatR;

namespace CustomerVendorApi.Features.Admin.Queries.GetAllCustomersQuery
{
    public class GetAllCustomersQueryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/customers", async (IMediator mediator) =>
            {
                var query = new GetAllCustomersQuery();
                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
            .WithName("GetAllCustomers")
            .WithTags("Customers");
        }
    }
}
