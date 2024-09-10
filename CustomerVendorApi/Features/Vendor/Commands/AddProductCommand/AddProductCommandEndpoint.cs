using Carter;
using MediatR;

namespace CustomerVendorApi.Features.Vendor.Commands.AddProductCommand
{
    public class AddProductCommandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/vendor/products/add", async (AddProductCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result ? Results.Ok("Product added successfully") : Results.BadRequest("Failed to add product");
            }).RequireAuthorization();
        }
    }
}
