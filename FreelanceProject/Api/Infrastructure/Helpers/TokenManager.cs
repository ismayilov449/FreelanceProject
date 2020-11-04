using Api.Infrastructure.RequestModels;
using Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infrastructure.Helpers
{
    public class TokenManager
    {


        public static string GenerateToken(IAuthService _authService, Guid userId, IConfiguration _configuration)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var now = DateTime.UtcNow;

            var roles =  _authService.GetRolesByUserId(userId.ToString()).Result.ToList();


            var claims = new List<Claim> {
                new Claim("UserId", userId.ToString()),
            };

            if (roles != null && roles.Count > 0)
            {
                claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x.Name)));
            }

            var tokenDescriptor = new JwtSecurityToken(
                claims: claims.ToArray(),
                notBefore: now,
                expires: now.AddDays(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return token;
        }

        public static ClaimsPrincipal GetPrincipal(string token, IConfiguration _configuration)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ValidateToken(string token, IConfiguration _configuration)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token, _configuration);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;
            return username;
        }


    }
}
