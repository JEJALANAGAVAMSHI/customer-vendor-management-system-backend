using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerVendorApi.Features.Admin.Commands.DeleteVendorCommand
{
    public class DeleteVendorCommandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/vendor/{id}", [Authorize(Roles = "Admin")] async ([FromRoute] string id, HttpContext httpContext, IMediator mediator) =>
            {
                var authHeader = httpContext.Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authHeader))
                {
                    return Results.Unauthorized();
                }

                var result = await mediator.Send(new DeleteVendorCommand { VendorId = id, AuthorizationHeader = authHeader });
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
