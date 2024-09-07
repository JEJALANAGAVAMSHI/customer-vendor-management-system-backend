using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerVendorApi.Features.Admin.Commands.DeleteVendorCommand
{
    public class DeleteVendorCommandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/delete-vendor/{vendorId}", async ([FromRoute] string vendorId, IMediator mediator) =>
            {
                var command = new DeleteVendorCommand { VendorId = vendorId };
                var result = await mediator.Send(command);
                if (result)
                {
                    return Results.Ok(new { Message = "Vendor deleted successfully" });
                }
                return Results.StatusCode(500);
            })
            .WithName("DeleteVendor")
            .WithTags("Vendor");
        }
    }
}
