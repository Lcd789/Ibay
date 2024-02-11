using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace IBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Login Endpoint")]
    public class LoginController(IIbayContext context, IConfiguration configuration) : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation("User Login")]
        [SwaggerResponse(200, "Login successful")]
        [SwaggerResponse(401, "Unauthorized", null)]
        public IActionResult Login(string userEmail, string userPassword)
        {
            var user = context.GetUserByEmail(userEmail);

            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            if(!BCrypt.Net.BCrypt.Verify(userPassword, user.user_password))
            {
                return Unauthorized("Invalid credentials");
            }
            
            var token = GenerateJwtToken(user);

            return Ok(new { Token = token, User = user });
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.user_id.ToString()),
                    new Claim(ClaimTypes.Name, user.user_email)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}