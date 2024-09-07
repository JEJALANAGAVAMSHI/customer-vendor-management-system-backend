using CustomerVendorApi.DTO;
using Mapster;
using MediatR;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetAllCustomersQueryHandler(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("AuthService");
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<GetAllCustomersResponse> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader))
            {
                throw new HttpRequestException("Authorization header is missing.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authHeader.Replace("Bearer ", ""));

            var response = await _httpClient.GetAsync("api/auth/customers", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var customers = await response.Content.ReadFromJsonAsync<IEnumerable<GetCustomerDto>>(cancellationToken: cancellationToken);
                return new GetAllCustomersResponse { Customers = customers };
            }

            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error fetching customers: {response.StatusCode}, Content: {content}");
        }
    }

}