using Carter;
using MediatR;

namespace CustomerVendorApi.Features.Vendor.Commands.AddLocationCommand
{
    public class AddLocationCommandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/vendor/location/add", async (AddLocationCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result!=0 ? Results.Ok("Location added successfully") : Results.BadRequest("Failed to add Location");
            });
        }
    }
}
