using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapi_iot_growdata5.Models;

namespace webapi_iot_growdata5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IOUnitAuthentication : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public IOUnitAuthentication(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //[HttpPost("login")]
        //public IActionResult Login([FromBody] IOUnitAuthenticationLoginModel model)
        //{
        //    // Dummy-Validierung (ersetzen Sie dies durch Ihre Benutzerlogik)
        //    if (model.Username == "admin" && model.Password == "password")
        //    {
        //        var token = GenerateJwtToken(model.Username);
        //        return Ok(new { Token = token });
        //    }
        //    return Unauthorized("Ungültige Anmeldeinformationen");
        //}

        [HttpPost("login")]
        public IActionResult Login([FromBody] Dictionary<string, string> credentials)
        {
            if (credentials == null || !credentials.ContainsKey("Username") || !credentials.ContainsKey("Password"))
            {
                return BadRequest("Invalid input. Provide 'Username' and 'Password' as JSON.");
            }

            string username = credentials["Username"];
            string password = credentials["Password"];

            if (username == "admin" && password == "password")
            {
                var token = GenerateJwtToken(username);
                return Ok(new { token });
            }

            return Unauthorized("Invalid credentials.");
        }

        private string GenerateJwtToken(string username)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, username)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
