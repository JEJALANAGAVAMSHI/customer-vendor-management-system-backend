using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerVendorApi.Features.Admin.Commands.AddVendorCommand
{
    public class AddVendorCommandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/register-vendor", [Authorize(Roles = "Admin")] async ([FromBody] AddVendorCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                if (result)
                {
                    return Results.Ok(new { Message = "Vendor registered successfully" });
                }
                return Results.StatusCode(500);
            })
            .WithName("RegisterVendor")
            .WithTags("Vendor");
        }
    }
}
