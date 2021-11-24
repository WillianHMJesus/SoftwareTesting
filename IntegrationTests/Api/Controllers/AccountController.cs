using Api.Models;
using Api.Repositories;
using Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly AppSettings _appSettings;

        private readonly IUserRepository _userRepository;

        public AccountController(IOptions<AppSettings> appSettings,
            IUserRepository userRepository)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("api/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel login)
        {
            var user = _userRepository.PasswordSignIn(login.Email, login.Senha);

            if (user != null)
            {
                return Ok(GenerateJwt(login.Email));
            }

            return BadRequest("Usuário ou Senha incorretos");
        }

        private string GenerateJwt(string email)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.ValidIn,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.Expiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var tokenResult = tokenHandler.WriteToken(token);
            return tokenResult;
        }
    }
}
