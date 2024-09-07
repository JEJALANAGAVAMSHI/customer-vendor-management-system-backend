using CustomerVendorApi.DTO;
using MediatR;
using System.Text.Json;
using System.Text;
using Mapster;

namespace CustomerVendorApi.Features.Admin.Commands.AddCustomerCommand
{
    public class AddCustomerCommand : IRequest<bool>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }
    public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, bool>
    {
        private readonly HttpClient _httpClient;

        public AddCustomerCommandHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthService");
        }

        public async Task<bool> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            var registerDto = request.Adapt<RegisterCustomerDto>();
            var content = new StringContent(JsonSerializer.Serialize(registerDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/register-customer", content, cancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}
