using MediatR;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerVendorApi.Features.Admin.Commands.DeleteVendorCommand
{
    public class DeleteVendorCommand : IRequest<bool>
    {
        public string VendorId { get; set; } // Assuming VendorId is a string
    }
    public class DeleteVendorCommandHandler : IRequestHandler<DeleteVendorCommand, bool>
    {
        private readonly HttpClient _httpClient;

        public DeleteVendorCommandHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthService");
        }

        public async Task<bool> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.DeleteAsync($"api/auth/delete-vendor/{request.VendorId}", cancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}
