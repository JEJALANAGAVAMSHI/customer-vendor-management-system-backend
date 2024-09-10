using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace CustomerVendorApi.Features.Vendor.Commands.DeleteOfferCommand
{
    public class DeleteOfferCommandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/vendor/offers/{id}", [Authorize(Roles = "Vendor")] async (int id, IMediator mediator) =>
            {
                var command = new DeleteOfferCommand { OfferId = id };
                await mediator.Send(command);
                return Results.Ok("Offer Deleted Successfully");
            });
        }
    }
}
