using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace CustomerVendorApi.Features.Admin.Queries.GetAllVendorsQuery
{
    public class GetAllVendorsQueryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/vendors", [Authorize(Roles = "Admin")] async (HttpContext httpContext, IMediator mediator) =>
            {
                var query = new GetAllVendorsQuery();
                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
            .WithName("GetAllVendors")
            .WithTags("Vendors");
        }
    }
}

