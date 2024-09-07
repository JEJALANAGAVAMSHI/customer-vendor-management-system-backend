using MediatR;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerVendorApi.Features.Admin.Commands.DeleteVendorCommand
{
    public class DeleteVendorCommand : IRequest<bool>
    {
        public string VendorId { get; set; }
        public string AuthorizationHeader { get; set; }
    }

    public class DeleteVendorCommandHandler : IRequestHandler<DeleteVendorCommand, bool>
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteVendorCommandHandler(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("AuthService");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.AuthorizationHeader))
            {
                throw new HttpRequestException("Authorization header is missing.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AuthorizationHeader.Replace("Bearer ", ""));

            var response = await _httpClient.DeleteAsync($"api/auth/delete-vendor/{request.VendorId}", cancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}
