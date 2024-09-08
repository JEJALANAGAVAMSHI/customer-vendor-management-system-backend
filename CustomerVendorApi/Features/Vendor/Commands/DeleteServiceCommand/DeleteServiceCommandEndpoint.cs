using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace CustomerVendorApi.Features.Vendor.Commands.DeleteServiceCommand
{
    public class DeleteServiceCommandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/services/{id}", [Authorize(Roles = "Vendor")] async (int id, IMediator mediator) =>
            {
                var command = new DeleteServiceCommand { ServiceId = id };
                await mediator.Send(command);
                return Results.Ok("Service Deleted successfully");  // Return NoContent (204) on successful deletion
            })
            .WithName("DeleteService")
            .WithTags("Services");
        }
    }
}
