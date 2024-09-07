using Carter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace CustomerVendorApi.Features.Admin.Queries.GetAllCustomersQuery
{
    public class GetAllCustomersQueryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/customers", [Authorize(Roles="Admin")] async (HttpContext httpContext,IMediator mediator) =>
            {
               
                var query = new GetAllCustomersQuery();
                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
            .WithName("GetAllCustomers")
            .WithTags("Customers");
        }
    }
}
