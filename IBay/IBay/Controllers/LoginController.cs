using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAL.Model;
using Microsoft.AspNetCore.Authorization;

namespace IBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IIbayContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(IIbayContext context, IConfiguration configuration)
        {
            this._context = context;
            this._configuration = configuration;
        }

        /// <summary> Se connecter </summary>
        /// <param name="userEmail">Email de l'utilisateur</param>
        /// <param name="userPassword">Mot de passe de l'utilisateur</param>
        /// <returns>Token et utilisateur</returns>
        /// <response code="200">Retourne le token et l'utilisateur</response>
        /// <response code="401">Identifiants invalides</response>
        /// <response code="500">Erreur interne</response>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(string userEmail, string userPassword)
        {
            var user = _context.GetUserByEmail(userEmail);

            if (user == null)
            {
                return Unauthorized("Identifiants invalides");
            }

            if(!BCrypt.Net.BCrypt.Verify(userPassword, user.UserPassword))
            {
                return Unauthorized("Invalid credentials");
            }
            
            var token = GenerateJwtToken(user);

            return Ok(new { Token = token, User = user });
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserEmail)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}