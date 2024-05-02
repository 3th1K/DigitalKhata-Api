using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common;
using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DataService.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private const string TokenSecret = "ahdbhdcbudababubaufdafudbudahfouabcuda";
        private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(6);
        private readonly DigitalKhataDbContext _context;
        private readonly ILogger<IdentityRepository> _logger;
        public IdentityRepository(DigitalKhataDbContext context, ILogger<IdentityRepository> logger)
        {
            _context = context;
            _logger = logger;

        }

        public async Task<User?> ValidateUser(string username, string password)
        {
            User? user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username || u.Email == username);

            if (user != null && user.Password == password)
            {
                _logger.LogInformation("User Found");
                return user;
            }
            _logger.LogError("User Not Found");
            return null;
        }

        public async Task<string> GetToken(string username, string password)
        {
            _logger.LogInformation("Validating Input Credentials");
            User? user = await ValidateUser(username, password);
            if (user != null)
            {
                var token = GenerateJwtToken(user);
                _logger.LogInformation("Successfully Validated Credentials, Token Generated");
                return token;
            }
            _logger.LogError("Token Generation Failed");
            return string.Empty;
        }

        private string GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(TokenSecret);
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Name, $"{user.Username}"));
            claims.Add(new Claim("userId", user.UserId.ToString()));

            var identity = new ClaimsIdentity(claims);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.Add(TokenLifetime),
                Issuer = "mehedi-somesite",
                Audience = "janina-bhai",
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
