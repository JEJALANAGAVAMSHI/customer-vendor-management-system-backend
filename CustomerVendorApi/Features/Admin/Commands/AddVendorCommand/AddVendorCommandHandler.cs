using CustomerVendorApi.DTO;
using Mapster;
using MediatR;
using System.Text.Json;
using System.Text;

namespace CustomerVendorApi.Features.Admin.Commands.AddVendorCommand
{
    public class AddVendorCommand : IRequest<bool>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }
    public class AddVendorCommandHandler : IRequestHandler<AddVendorCommand, bool>
    {
        private readonly HttpClient _httpClient;

        public AddVendorCommandHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthService");
        }
        public async Task<bool> Handle(AddVendorCommand request, CancellationToken cancellationToken)
        {
            var registerDto = request.Adapt<RegisterCustomerDto>();
            var content = new StringContent(JsonSerializer.Serialize(registerDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/register-vendor", content, cancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}
