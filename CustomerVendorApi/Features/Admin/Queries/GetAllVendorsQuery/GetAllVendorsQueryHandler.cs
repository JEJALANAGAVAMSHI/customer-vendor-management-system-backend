using CustomerVendorApi.DTO;
using Mapster;
using MediatR;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace CustomerVendorApi.Features.Admin.Queries.GetAllVendorsQuery
{
    public class GetAllVendorsQuery : IRequest<GetAllVendorsResponse>
    {

    }

    public class GetAllVendorsResponse
    {
        public IEnumerable<GetVendorDto> Vendors { get; set; }
    }

    public class GetAllVendorsQueryHandler : IRequestHandler<GetAllVendorsQuery, GetAllVendorsResponse>
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllVendorsQueryHandler(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("AuthService");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetAllVendorsResponse> Handle(GetAllVendorsQuery request, CancellationToken cancellationToken)
        {
            var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader))
            {
                throw new HttpRequestException("Authorization header is missing.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authHeader.Replace("Bearer ", ""));

            var response = await _httpClient.GetAsync("api/auth/vendors", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var vendors = await response.Content.ReadFromJsonAsync<IEnumerable<GetVendorDto>>(cancellationToken: cancellationToken);
                return new GetAllVendorsResponse { Vendors = vendors };
            }

            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error fetching vendors: {response.StatusCode}, Content: {content}");
        }
    }
}
