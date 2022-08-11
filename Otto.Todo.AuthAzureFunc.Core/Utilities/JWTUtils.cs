using System.Security.Claims;
using System.Text;
using Otto.Todo.AuthAzureFunc.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Otto.Todo.AuthAzureFunc.Core.Utilities
{
    public static class JWTUtils
    {
        public static string GenerateToken(AuthRequestDTO authuser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", authuser.UserId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public static Token GenerateTokenAsymetric(AuthRequestDTO authuser)
        {
            using RSA rsa = RSA.Create();
            rsa.ImportRSAPrivateKey( // Convert the loaded key from base64 to bytes.
                source: Convert.FromBase64String(Environment.GetEnvironmentVariable("PRIVATE_KEY")), // Use the private key to sign tokens
                bytesRead: out int _); // Discard the out variable 

            var signingCredentials = new SigningCredentials(
                key: new RsaSecurityKey(rsa),
                algorithm: SecurityAlgorithms.RsaSha256 // Important to use RSA version of the SHA algo 
            );

            DateTime jwtDate = DateTime.Now;

            var jwt = new JwtSecurityToken(
                audience: "otto-todo-frontend",
                issuer: "otto-todo-authservice",
                claims: new Claim[] { new Claim("id", authuser.UserId), 
                                      new Claim("appid", authuser.AppId),
                                      new Claim("phonenumber", authuser.PhoneNumber),
                },
                notBefore: jwtDate,
                expires: jwtDate.AddHours(2),
                signingCredentials: signingCredentials
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new Token
            {
                TokenType = "Bearer",
                IdToken = token,
                Expiry = new DateTimeOffset(jwtDate).ToUnixTimeMilliseconds()
            };
        }

        public static long? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = long.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        public static long? ValidateTokenAsymetric(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            using RSA rsa = RSA.Create();
            rsa.ImportRSAPublicKey( // Convert the loaded key from base64 to bytes.
                source: Convert.FromBase64String(Environment.GetEnvironmentVariable("PUBLIC_KEY")), // Use the public key to decrypt tokens
                bytesRead: out int _); // Discard the out variable 
            SecurityKey rsakey = new RsaSecurityKey(rsa);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = rsakey,
                    ValidAudience = "otto-todo-frontend",
                    ValidIssuer = "otto-todo-authservice",
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = long.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
