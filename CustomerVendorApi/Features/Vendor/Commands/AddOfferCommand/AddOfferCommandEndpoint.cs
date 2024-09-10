using Carter;
using MediatR;

namespace CustomerVendorApi.Features.Vendor.Commands.AddOfferCommand
{
    public class AddOfferCommandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/vendor/offers/add", async (AddOfferCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result ? Results.Ok("Offer added successfully") : Results.BadRequest("Failed to add offer");
            });
        }
    }
}
