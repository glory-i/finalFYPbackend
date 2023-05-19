using finalFYPbackend.Authentication;
using finalFYPbackend.Responses.Enums;
using finalFYPbackend.Responses;
using finalFYPbackend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace finalFYPbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;
        public AuthenticationController(IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }

        [HttpPost("SignUpUseless")]
        public async Task<ActionResult<ApiResponse>> Register(ValidateModel registerModel)
        {

            var response = await _authenticationServices.RegisterUser(registerModel);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [HttpPost("SignUp")]
        public async Task<ActionResult<ApiResponse>> SignUp(SignUpModel signUpModel)
        {

            var response = await _authenticationServices.SignUpUser(signUpModel);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [HttpPost("SignUpAAdmin")]
        public async Task<ActionResult<ApiResponse>> SignUpAdmin(RegisterAdminModel model)
        {

            var response = await _authenticationServices.RegisterAdmin(model);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [HttpPost("LoginAdmin")]
        public async Task<ActionResult<ApiResponse>> LoginAdmin(LoginAdminModel model)
        {

            var response = await _authenticationServices.LoginAdmin(model);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [HttpPost("Login")]
        public async Task<ActionResult<ApiResponse>> Login(LoginModel loginModel)
        {

            var response = await _authenticationServices.Login(loginModel);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [HttpPost("Validate")]
        public async Task<ActionResult<ApiResponse>> CheckValidations(ValidateModel registerModel)
        {

            var response = await _authenticationServices.CheckValidations(registerModel);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [HttpPost("SendOTP")]
        public async Task<ActionResult<ApiResponse>> SendOTP(string username, string email)
        {

            var response = await _authenticationServices.SendOTP(username, email);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [HttpPost("ValidateOTP")]
        public async Task<ActionResult<ApiResponse>> ValidateOTP(string inputPin, string username, string email)
        {

            var response = await _authenticationServices.ValidateOTP(inputPin, username, email);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [HttpPost("ValidateUserExists")]
        public async Task<ActionResult<ApiResponse>> ValidateUserExists(string email)
        {

            var response = await _authenticationServices.ValidateUserExists(email);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }


        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<ApiResponse>> ForgotPassword(string email, string newPassword, string confirmPassword)
        {

            var response = await _authenticationServices.ForgotPassword(email, newPassword, confirmPassword);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<ApiResponse>> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {

            var response = await _authenticationServices.ChangePassword(User.Identity.Name, currentPassword, newPassword, confirmPassword);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [Authorize]
        [HttpGet("ViewUser")]
        public async Task<ActionResult<ApiResponse>> ViewUser()
        {

            var response = await _authenticationServices.ViewUser(User.Identity.Name);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }


        [HttpPost("CalculateCalorieRequirements")]
        public async Task<ActionResult<ApiResponse>> CalculateRequirements(CalculateCalorieRequirementsModel model)
        {

            var response = await _authenticationServices.CalculateCalorieRequirements(model);
            return Ok(response);

        }

        [Authorize]
        [HttpPost("UpdateAccount")]
        public async Task<ActionResult<ApiResponse>> UpdateAccount(UpdateUserModel updateUserModel)
        {

            var response = await _authenticationServices.UpdateUser(User.Identity.Name, updateUserModel);
            if (response.Message == ApiResponseEnum.success.ToString())
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }
    }
}
