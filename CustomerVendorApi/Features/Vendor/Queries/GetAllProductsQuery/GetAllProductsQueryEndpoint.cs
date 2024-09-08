using Carter;
using CustomerVendorApi.Models;
using MediatR;

namespace CustomerVendorApi.Features.Vendor.Queries.GetAllProductsQuery
{
    public class GetAllProductsQueryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/businesses/{businessId}/products", async (int businessId, IMediator mediator) =>
            {
                var query = new GetAllProductsQuery { BusinessId = businessId };
                var products = await mediator.Send(query);

                if (products == null || products.Count == 0)
                {
                    return Results.NotFound("No services found for this business.");
                }

                return Results.Ok(products);
            })
            .WithName("GetProductsByBusinessId")
            .Produces<List<Service>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
