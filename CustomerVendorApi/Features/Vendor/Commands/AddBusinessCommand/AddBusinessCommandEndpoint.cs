using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace CustomerVendorApi.Features.Vendor.Commands.AddBusinessCommand
{
    public class AddBusinessCommandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/businesses", [Authorize(Roles = "Vendor")] async (AddBusinessCommand command, IMediator mediator) =>
            {
                var businessId = await mediator.Send(command);
                return Results.Created($"/businesses/{businessId}", new { BusinessId = businessId });
            })
                .WithName("AddBusiness")
                .WithTags("Businesses");
        }
    }
}
