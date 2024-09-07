using MediatR;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerVendorApi.Features.Admin.Commands.DeleteCustomerCommand
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public string CustomerId { get; set; }
        public string AuthorizationHeader { get; set; }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteCustomerCommandHandler(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("AuthService");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.AuthorizationHeader))
            {
                throw new HttpRequestException("Authorization header is missing.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AuthorizationHeader.Replace("Bearer ", ""));

            var response = await _httpClient.DeleteAsync($"api/auth/delete-customer/{request.CustomerId}", cancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}
