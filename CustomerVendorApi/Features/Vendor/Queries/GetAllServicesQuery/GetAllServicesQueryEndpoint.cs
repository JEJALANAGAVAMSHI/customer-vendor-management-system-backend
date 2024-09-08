using Carter;
using CustomerVendorApi.Models;
using MediatR;

namespace CustomerVendorApi.Features.Vendor.Queries.GetAllServicesQuery
{
    public class GetAllServicesQueryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/businesses/{businessId}/services", async (int businessId, IMediator mediator) =>
            {
                var query = new GetAllServicesQuery { BusinessId = businessId };
                var services = await mediator.Send(query);

                if (services == null || services.Count == 0)
                {
                    return Results.NotFound("No services found for this business.");
                }

                return Results.Ok(services);
            })
            .WithName("GetServicesByBusinessId")
            .Produces<List<Service>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
