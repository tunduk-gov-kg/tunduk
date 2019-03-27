using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Catalog.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Catalog.WebApi.Controllers {
    [Route("[controller]/[action]")]
    public class AccountController : Controller {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CatalogUser> _signInManager;
        private readonly UserManager<CatalogUser> _userManager;

        public AccountController(
            SignInManager<CatalogUser> signInManager
            , UserManager<CatalogUser> userManager
            , IConfiguration configuration
        ) {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }


        [HttpPost]
        public async Task<object> Login([FromBody] LoginData model) {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded) {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                return await GenerateJwtToken(model.Email, appUser);
            }

            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }

        [HttpPost]
        public async Task<object> Register([FromBody] RegisterData model) {
            var user = new CatalogUser {
                UserName = model.UserName,
                Email = model.Email,
                Name = model.Name
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded) {
                await _signInManager.SignInAsync(user, false);
                return await GenerateJwtToken(model.Email, user);
            }

            return BadRequest(result.Errors);
        }

        private async Task<object> GenerateJwtToken(string email, IdentityUser user) {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key     = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds   = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class LoginData {
            [Required] public string Email { get; set; }

            [Required] public string Password { get; set; }
        }

        public class RegisterData {
            [Required] public string Email { get; set; }
            [Required] public string UserName { get; set; }
            [Required] public string Name { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
            public string Password { get; set; }
        }
    }
}