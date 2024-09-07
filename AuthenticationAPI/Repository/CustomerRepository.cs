using AuthenticationAPI.Contracts;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace AuthenticationAPI.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CustomerRepository(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

       

        public async Task<IEnumerable<CustomerDto>> GetCustomers()
        {

            var customerRole = await _roleManager.FindByNameAsync("Customer");
            if (customerRole == null)
            {
                return null;
            }

            var customers = await _userManager.GetUsersInRoleAsync(customerRole.Name);

            var customerDtos = customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                UserName = c.UserName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address,
                State = c.State,
                PostalCode = c.PostalCode,
        
            }).ToList();
            return customerDtos;
        }
    }
}
