using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace CustomerVendorApi.Features.Vendor.Commands.DeleteEventCommand
{
    public class DeleteEventCommandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/vendor/events/{id}", [Authorize(Roles = "Vendor")] async (int id, IMediator mediator) =>
            {
                var command = new DeleteEventCommand { EventId = id };
                await mediator.Send(command);
                return Results.Ok("Event Deleted Successfully");  
            });
        }
    }
}
