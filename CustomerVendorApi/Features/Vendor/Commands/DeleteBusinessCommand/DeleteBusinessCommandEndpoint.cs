using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace CustomerVendorApi.Features.Vendor.Commands.DeleteBusinessCommand
{
    public class DeleteBusinessCommandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/vendor/businesses/{id}", [Authorize(Roles = "Vendor")] async (int id, IMediator mediator) =>
            {
                var command = new DeleteBusinessCommand { BusinessId = id };
                await mediator.Send(command);
                return Results.Ok("Deleted Successfully");  // Return NoContent (204) on successful deletion
            })
            .WithName("DeleteBusiness")
            .WithTags("Businesses");
        }
    }
}
