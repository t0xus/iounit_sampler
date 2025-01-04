using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using webapi_iot_growdata5.Models;

namespace webapi_iot_growdata5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IOUnitAuthentication : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private ApplicationDbContext _dbContext;

        public IOUnitAuthentication(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
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

            var user_obj = _dbContext.iounit_users
                .Where(s => s.username == credentials["Username"])
                .ToList();
            //HashPassword("test123");

            if (credentials == null || !credentials.ContainsKey("Username") || !credentials.ContainsKey("Password"))
            {
                return BadRequest("Invalid input. Provide 'Username' and 'Password' as JSON.");
            }

            if (user_obj.Count > 0)
            {
                if (credentials["Username"] == user_obj[0].username && HashPassword(credentials["Password"]) == user_obj[0].pw_hash)
                {
                    var role_obj = _dbContext.iounit_user_roles
                        .Where(s => s.id == user_obj[0].id_ur)
                        .ToList();
                    
                    var token = GenerateJwtToken(user_obj[0].username, role_obj[0].rolename);
                    return Ok(new { token });
                }
            }
            

            return Unauthorized("Invalid credentials.");
        }

        private string GenerateJwtToken(string username, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, username),

            // Hier die Rolle hinzufügen:
            new Claim(ClaimTypes.Role, role)
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

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                // BitConverter liefert ein Hex-String mit Bindestrichen:
                // Daher entfernen und zu lower-case konvertieren
                return BitConverter
                    .ToString(hashedBytes)
                    .Replace("-", "")
                    .ToLowerInvariant();
            }
        }
    }
}
