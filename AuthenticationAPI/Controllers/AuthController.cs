using AuthenticationAPI.Contracts;
using AuthenticationAPI.Models;
using AuthenticationAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRegisterAdmin _registerAdminRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly IRegisterCustomer _registerCustomerRepository;
        private readonly IRegisterVendor _registerVendorRepository;
        private readonly ICustomerRepository _customerRepository;

        public AuthController(
            IRegisterAdmin registerAdminRepository,
            ILoginRepository loginRepostiory,
            IRegisterCustomer registerCustomerRepository,
            IRegisterVendor registerVendorRepository,
            ICustomerRepository customerRepository)
        {
           _registerAdminRepository = registerAdminRepository;
            _loginRepository = loginRepostiory;
            _registerCustomerRepository = registerCustomerRepository;
            _registerVendorRepository = registerVendorRepository;
            _customerRepository = customerRepository;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var token = await _loginRepository.Login(model);
            if (token != null)
            {
                return Ok(token);
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register-customer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] ApplicationUser model)
        {
            var isRegistered = await _registerCustomerRepository.Register(model);
            var response = new Response
            {
                Status = isRegistered.Status,
                Message = isRegistered.Message
            };
            if (isRegistered.Status == "Error")
            {
                return StatusCode(StatusCodes.Status500InternalServerError, isRegistered.Message);
            }
            return Ok(response);
        }
        [HttpPost]
        [Route("register-vendor")]
        public async Task<IActionResult> RegisterVendor([FromBody] ApplicationUser model)
        {
            var isRegistered = await _registerVendorRepository.RegisterVendor(model);
            var response = new Response
            {
                Status = isRegistered.Status,
                Message = isRegistered.Message
            };
            if (isRegistered.Status == "Error")
            {
                return StatusCode(StatusCodes.Status500InternalServerError, isRegistered.Message);
            }
            return Ok(response);
        }
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] ApplicationUser model)
        {
            var registered = await _registerAdminRepository.RegisterAdmin(model);
            if (registered.Status == "Error")
            {
                return StatusCode(StatusCodes.Status500InternalServerError, registered);
            }
            return Ok(registered);
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public IActionResult Temp()
        {
            return Ok("Ok");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("customers")]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _customerRepository.GetCustomers();
            if(customers == null)
            {
                return NotFound();
            }
            return Ok(customers);
        }

        [HttpDelete]
        [Route("delete-customer/{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var isDeleted = await _customerRepository.DeleteCustomerAsync(id);
            if (isDeleted)
            {
                return Ok(new { Message = "Customer deleted successfully" });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the customer.");
        }



    }
}
