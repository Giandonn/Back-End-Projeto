using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Prova.Services
{
    public class JwtService
    {
        private const string _secretKey = "minha-chave-secreta-de-assinatura";
        private const string _issuer = "meu-sistema"; 
        private const string _audience = "meu-cliente";

        public string GerarToken(string usuario, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario), 
                new Claim(JwtRegisteredClaimNames.Email, email), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
            };

            var token = new JwtSecurityToken(
                issuer: _issuer, 
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddHours(10),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public string ValidarToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = _issuer,
                    ValidAudience = _audience 
                }, out var validatedToken);

                return "Token é válido.";
            }
            catch (SecurityTokenExpiredException)
            {
                return "Token expirado.";
            }
            catch (SecurityTokenException)
            {
                return "Token inválido.";
            }
            catch (Exception)
            {
                return "Erro ao validar o token.";
            }
        }
    }
}
