using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace CustomerVendorApi.Features.Vendor.Commands.DeleteProductCommand
{
    public class DeleteProductCommandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/vendor/products/{id}", [Authorize(Roles = "Vendor")] async (int id, IMediator mediator) =>
            {
                var command = new DeleteProductCommand { ProductId = id };
                await mediator.Send(command);
                return Results.Ok("Product Deleted Successfully");  // Return NoContent (204) on successful deletion
            })
            .WithName("DeleteProduct")
            .WithTags("Products");
        }
    }
}
