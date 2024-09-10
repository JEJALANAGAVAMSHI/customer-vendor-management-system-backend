using Carter;
using MediatR;

namespace CustomerVendorApi.Features.Vendor.Queries.GetAllEventsQuery
{
    public class GetAllEventsQueryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/businesses/{businessId}/events", async (int businessId, IMediator mediator) =>
            {
                var query = new GetAllEventsQuery { BusinessId = businessId };
                var events = await mediator.Send(query);

                if (events == null || events.Count == 0)
                {
                    return Results.NotFound("No events found for this business.");
                }

                return Results.Ok(events);
            });
        }
    }
}
