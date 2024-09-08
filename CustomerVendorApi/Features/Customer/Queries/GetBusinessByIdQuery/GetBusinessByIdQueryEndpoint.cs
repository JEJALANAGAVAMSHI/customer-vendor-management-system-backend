using Carter;
using MediatR;

namespace CustomerVendorApi.Features.Customer.Queries.GetBusinessByIdQuery
{
    public class GetBusinessByIdQueryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/customer/get-businesses/{id}", async (int id, IMediator mediator) =>
            {
                var query = new GetBusinessByIdQuery { BusinessId = id };
                var result = await mediator.Send(query);
                if (result == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(result);
            });
        }
    }
}
