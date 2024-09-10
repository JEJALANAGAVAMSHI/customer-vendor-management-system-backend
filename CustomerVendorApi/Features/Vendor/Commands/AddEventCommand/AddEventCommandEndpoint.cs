using Carter;
using MediatR;

namespace CustomerVendorApi.Features.Vendor.Commands.AddEventCommand
{
    public class AddEventCommandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/vendor/events/add", async (AddEventCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result ? Results.Ok("Event added successfully") : Results.BadRequest("Failed to add product");
            }).RequireAuthorization();
        }
    }
}
