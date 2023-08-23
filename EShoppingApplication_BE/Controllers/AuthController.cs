using CommonLayer.Models;
using Inventory_Management_System_BE.Common_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayers.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EShoppingApplication_BE.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRL _authRL;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthRL authRL, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _authRL =  authRL;
            _logger = logger;
            
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpRequest request)
        {
            SignUpResponse response = new SignUpResponse();
            try
            {
                _logger.LogInformation($"SignUp Calling In AdminController.... Time : {DateTime.Now}");
                response = await _authRL.SignUp(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError("Exception Occur In AuthController : Message : ", ex.Message);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(SignInRequest request)
        {
            SignInResponse response = new SignInResponse();
            try
            {
                _logger.LogInformation($"SignIn Calling In AdminController.... Time : {DateTime.Now}");
                response = await _authRL.SignIn(request);
                if (response.IsSuccess)
                {
                    string Type = string.Empty;
                    if (response.data.Role.Equals("manager"))
                    {
                        Type = "Manager Login";
                    }
                    else
                    {
                        Type = "Customer Login";
                    }
                    response = await CreateToken(response, Type);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError("Exception Occur In AuthController : Message : ", ex.Message);

            }

            return Ok(response);
        }
        
        [HttpPost]
        public async Task<ActionResult> AddCustomerDetail(AddCustomerDetailRequest request)
        {
            AddCustomerDetailResponse response = new AddCustomerDetailResponse();
            try
            {
                _logger.LogInformation($"AddCustomerDetail Calling In AdminController.... Time : {DateTime.Now}");
                response = await _authRL.AddCustomerDetail(request);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError("Exception Occur In AuthController : Message : ", ex.Message);

            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetCustomerDetail([FromQuery]int UserID)
        {
            GetCustomerDetailResponse response = new GetCustomerDetailResponse();
            try
            {
                _logger.LogInformation($"AddCustomerDetail Calling In AdminController.... Time : {DateTime.Now}");
                response = await _authRL.GetCustomerDetail(UserID);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError("Exception Occur In AuthController : Message : ", ex.Message);

            }

            return Ok(response);
        }


        [HttpPost]
        public async Task<ActionResult> CustomerList(CustomerListRequest request)
        {
            CustomerListResponse response = new CustomerListResponse();
            try
            {
                _logger.LogInformation($"CustomerList Calling In AdminController.... Time : {DateTime.Now}");
                response = await _authRL.CustomerList(request);
                
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError("Exception Occur In AuthController : Message : ", ex.Message);

            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> AddCustomerAdderess(AddCustomerAdderessRequest request)
        {
            AddCustomerAdderessResponse response = new AddCustomerAdderessResponse();
            try
            {
                _logger.LogInformation($"AddCustomerAdderess Calling In AdminController.... Time : {DateTime.Now}");
                response = await _authRL.AddCustomerAdderess(request);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError("Exception Occur In AuthController : Message : ", ex.Message);

            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetCustomerAdderess([FromQuery]int UserID)
        {
            GetCustomerAdderessResponse response = new GetCustomerAdderessResponse();
            try
            {
                _logger.LogInformation($"GetCustomerAdderess Calling In AdminController.... Time : {DateTime.Now}");
                response = await _authRL.GetCustomerAdderess(UserID);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError("Exception Occur In AuthController : Message : ", ex.Message);

            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetIsCustomerDetailsFound(int UserID)
        {
            GetIsCustomerDetailsFoundResponse response = new GetIsCustomerDetailsFoundResponse();

            try
            {
                _logger.LogInformation($"GetIsCustomerDetailsFound Calling In AdminController.... Time : {DateTime.Now}");
                response = await _authRL.GetIsCustomerDetailsFound(UserID);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                _logger.LogError("Exception Occur In AuthController : Message : ", ex.Message);

            }

            return Ok(response);
        }



        //Method to create JWT token
        private async Task<SignInResponse> CreateToken(SignInResponse request, string Type)
        {
            try
            {
                var symmetricSecuritykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signingCreds = new SigningCredentials(symmetricSecuritykey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Role, request.data.Role));
                claims.Add(new Claim("UserName", request.data.UserName.ToString()));
                claims.Add(new Claim("UserId", request.data.UserId.ToString()));
                claims.Add(new Claim("TokenType", Type));

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audiance"],
                    claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: signingCreds);
                request.data.Token = new JwtSecurityTokenHandler().WriteToken(token);

            }
            catch (Exception ex)
            {
                request.IsSuccess = false;
                request.Message = "Exception Occur In Token Creation : Message : " + ex.Message;
                _logger.LogError("Exception Occur In Token Creation : Message : ", ex.Message);
            }
            return request;
        }

    }
}
