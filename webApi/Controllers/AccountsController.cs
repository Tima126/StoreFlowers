using BusinessLogic.Authorization;
using BusinessLogic.Models.Accounts;
using DataAccess.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
namespace webApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwareded-For"))
                return Request.Headers["X-Forwareded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate(AuthenticateRequest model)
        {
            var response = await _accountService.Authenticate(model, ipAddress());
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthenticateResponse>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var response = await _accountService.RefreshToken(refreshToken, ipAddress());
            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(RevokeTokenRequest model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is requred" });
            if (!User.OwnsToken(token) && User.role != Role.Admin)
                return Unauthorized(new { message = "Unauthorized" });
            await _accountService.RevokeToken(token, ipAddress());
            return Ok(new { message = "Token revoked" });


        }



        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            await _accountService.Register(model, Request.Headers["origin"]);
            return Ok(new { message = "Registration successful, please check your email for verification instructions" });

        }



        [AllowAnonymous]
        [HttpPost("validate-reset-token")]
        public async Task<IActionResult> ValidateResetToken(ValidateResetTokenRequest model)
        {
            await _accountService.ValidateResetToken(model);
            return Ok(new { message = "Token is valid" });
        }




        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            await _accountService.ResetPassword(model);
            return Ok(new { message = "Password reset successful, you can now login" });
        }



        [Authorize(Roles = nameof(Role.Admin))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountResponse>>> GetAll()
        {
            var account = await _accountService.GetAll();
            return Ok(account);
        }




        [HttpGet("{id:int}")]
        public async Task<ActionResult<AccountResponse>> GetById(int id)
        {
            if (id != User.UserId && User.role != Role.Admin)
                return Unauthorized(new { message = "Unauthorized" });

            var account = await _accountService.GetById(id);
            return Ok(account);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AccountResponse>> Create(CreateRequest model)
        {
            var account = await _accountService.Create(model);
            return Ok(account);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<AccountResponse>> Update(int id, UpdateRequest model)
        {
            if (id != User.UserId && User.role != Role.Admin)
                return Unauthorized(new { message = "Unauthorized" });

            if (User.role != Role.Admin)
                model.Role = null;

            var account = await _accountService.Update(id, model);
            return Ok(account);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id != User.UserId && User.role != Role.Admin)
                return Unauthorized(new { message = "Unauthorized" });

             await _accountService.Delete(id);
            return Ok(new { message = "Account deleted" });
        }







    }
}