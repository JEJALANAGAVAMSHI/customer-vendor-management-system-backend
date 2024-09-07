using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerVendorApi.Features.Admin.Commands.DeleteCustomerCommand
{
    public class DeleteCustomerCommandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/customer/{id}", async ([FromRoute] string id, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeleteCustomerCommand { CustomerId = id });
                if (result)
                {
                    return Results.Ok(new { Message = "Customer deleted successfully" });
                }
                return Results.StatusCode(500);
            })
            .WithName("DeleteCustomer")
            .WithTags("Customer");
        }
    }
}
