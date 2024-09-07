using CustomerVendorApi.DTO;
using Mapster;
using MediatR;
using System.Text.Json;
using System.Text;

namespace CustomerVendorApi.Features.Admin.Queries.GetAllCustomersQuery
{
    public class GetAllCustomersQuery : IRequest<GetAllCustomersResponse>
    {

    }
    public class GetAllCustomersResponse
    {
        public IEnumerable<GetCustomerDto> Customers { get; set; }
        
    }
    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, GetAllCustomersResponse>
    {
        private readonly HttpClient _httpClient;
        public GetAllCustomersQueryHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthService");
        }
        public async Task<GetAllCustomersResponse> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync("api/auth/customers", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var customers = await response.Content.ReadFromJsonAsync<IEnumerable<GetCustomerDto>>(cancellationToken: cancellationToken);
                return new GetAllCustomersResponse { Customers = customers };
            }

            // Handle error cases
            throw new HttpRequestException($"Error fetching customers: {response.StatusCode}");
        }
    }

}