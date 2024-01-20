using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AtmServerApp
{
    /**
    * THE CLASS IS TAILORED TO GENERATE TOKEN STRING USING USERNAME AND ROLE 
    * THE TOKEN WILL BE VALIDATED ON SECURED END-POINTS WITHIN EXISTING CONTROLLERS
    */
    public class JWTAuthManager
    {
        private readonly string secretKey;

        public JWTAuthManager(string secretKey)
        {
            this.secretKey = secretKey;
        }

        public string GenerateToken(string username, string role = "user")
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(secretKey);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
