using Carter;
using MediatR;

namespace CustomerVendorApi.Features.Vendor.Commands.AddServiceCommand
{
    public class AddServiceCommandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/vendor/services/add", async (AddServiceCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result ? Results.Ok("Service added successfully") : Results.BadRequest("Failed to add service");
            }).RequireAuthorization();
        }
    }
}
