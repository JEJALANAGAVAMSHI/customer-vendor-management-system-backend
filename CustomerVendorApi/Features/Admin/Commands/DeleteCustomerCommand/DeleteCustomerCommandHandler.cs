using MediatR;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerVendorApi.Features.Admin.Commands.DeleteCustomerCommand
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public string CustomerId { get; set; }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly HttpClient _httpClient;

        public DeleteCustomerCommandHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthService");
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.DeleteAsync($"api/auth/delete-customer/{request.CustomerId}", cancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}
