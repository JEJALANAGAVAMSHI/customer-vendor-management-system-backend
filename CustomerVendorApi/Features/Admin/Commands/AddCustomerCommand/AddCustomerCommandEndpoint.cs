using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerVendorApi.Features.Admin.Commands.AddCustomerCommand
{
    public class AddCustomerCommandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/register", async ([FromBody] AddCustomerCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                if (result)
                {
                    return Results.Ok(new { Message = "Customer registered successfully" });
                }
                return Results.StatusCode(500);
            })
            .WithName("RegisterCustomer")
            .WithTags("Customer");
        }
    }
}