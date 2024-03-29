﻿using KalendarDoktori.Models;
using KalendarDoktori.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KalendarDoktori.Models.InputModels;
using KalendarDoktori.Utilities;

namespace KalendarDoktori.Controllers
{
    [Route("authenticate")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthenticateController> _logger;
        private readonly ITokenService _tokenService;

        public AuthenticateController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<AuthenticateController> logger
        , ITokenService tokenService)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInput model)
        {
            _logger.LogInformation(model.UserName + model.Email + model.IsDoctor);
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true
            };
			_logger.LogInformation(user.UserName+user.Email);

			var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return NotFound(result.Errors);

            if (model.IsDoctor)
                await _userManager.AddToRoleAsync(user, ApplicationRoles.Doctor);
            else
                await _userManager.AddToRoleAsync(user, ApplicationRoles.User);

            var token = await _tokenService.GenerateToken(user);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1)
            };
            Response.Cookies.Append("token", token, cookieOptions);

            return new JsonResult(new { token });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInput model)
        {
            // validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // authenticate the user
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized();
            }
            // generate a token
            var token = await _tokenService.GenerateToken(user);
            // create a cookie with the token
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1)
            };
            Response.Cookies.Append("token", token, cookieOptions);
            // return the token as JSON
            return new JsonResult(new { token });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            Response.Cookies.Delete("token");
            _logger.LogInformation("User logged out.");

            return Ok();
        }

        [HttpGet("username")]
        public async Task<IActionResult> Username() {
			_logger.LogInformation("_userManager: {0}",_userManager);
			_logger.LogInformation("User: {0}",User);

			_logger.LogInformation("Username REQUESTED");
			var user = await _userManager.GetUserAsync(User);
            _logger.LogInformation("USERNAME: " + user.UserName);
			if (user==null) {
				return BadRequest("User not found");
			}

			return Ok(user.UserName);
		}

        [HttpGet("id")]
        public async Task<IActionResult> Id() {
            var user = await _userManager.GetUserAsync(User);

			if (user==null) {
				return BadRequest("User not found");
			}

			return Ok(user.Id);
		}
    }
}
