using CustomerVendorApi.Data;
using CustomerVendorApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CustomerVendorApi.Features.Vendor.Commands.AddBusinessCommand
{
    public class AddBusinessCommand : IRequest<int>
    {
        public string BusinessName { get; set; }
        public string Category { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public int PostalCode { get; set; }
        public string Email { get; set; }
        public string DayFrom { get; set; }
        public string DayTo { get; set; }
        public TimeOnly TimeFrom { get; set; }
        public TimeOnly TimeTo { get; set; }
    }
    public class AddBusinessCommandHandler : IRequestHandler<AddBusinessCommand, int>
    {
        private readonly ProductsServicesDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddBusinessCommandHandler(ProductsServicesDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> Handle(AddBusinessCommand request, CancellationToken cancellationToken)
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var vendorIdClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            var vendorNameClaim = claimsIdentity?.FindFirst(ClaimTypes.Name);

            if (vendorIdClaim == null || vendorNameClaim == null)
            {
                throw new UnauthorizedAccessException("Vendor ID or Vendor Name is missing in the token");
            }

            string vendorId = vendorIdClaim.Value;
            string vendorName = vendorNameClaim.Value;

            // Check if the vendor already exists
            var vendor = await _dbContext.Vendors
                    .Include(v => v.Businesses)
                    .FirstOrDefaultAsync(v => v.VendorId == vendorId, cancellationToken);

            if (vendor == null)
            {
                // Add the vendor if it doesn't exist
                vendor = new Models.Vendor
                {
                    VendorId = vendorId,
                    UserName = vendorName,
                    Businesses = new List<Business>()
                };
                _dbContext.Vendors.Add(vendor);
            }

            var business = new Business
            {
                BusinessName = request.BusinessName,
                Category = request.Category,
                Address = request.Address,
                State = request.State,
                PostalCode = request.PostalCode,
                Email = request.Email,
                DayFrom = request.DayFrom,
                DayTo = request.DayTo,
                TimeFrom = request.TimeFrom,
                TimeTo = request.TimeTo,
                VendorId = vendorId,
                
            };

            _dbContext.Businesses.Add(business);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return business.BusinessId;
        }

    }
}
