using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SuperAgentCore.Settings.BearerToken;
using TaskScheduler.DTOs.DemoUserModel;
using TaskScheduler.Services.Security;
using TaskScheduler.Services.Tokken.Models;

namespace TaskScheduler.Services.Tokken
{
    public class TokkenFactoryService : ITokkenFactoryService
    {
        private readonly ISecurityService _securityService;
        private readonly BearerTokensSettings _configuration;

        public TokkenFactoryService(ISecurityService securityService, IOptions<BearerTokensSettings> configuration)
        {
            _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService)); ;
            _configuration = configuration.Value ?? throw new ArgumentNullException(nameof(configuration.Value));
        }

        public JwtTokensData CreateJwtTokensAsync(AspNetUsers user, string Role, int IdBoker)
        {
            var (accessToken, claims) = CreateAccessTokenAsync(user, Role, IdBoker);
            var (refreshTokenValue, refreshTokenSerial) = CreateRefreshToken();
            return new JwtTokensData
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                RefreshTokenSerial = refreshTokenSerial,
                Claims = claims
            };
        }

        public string GetRefreshTokenSerial(string refreshTokenValue)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenValue))
            {
                return null;
            }

            ClaimsPrincipal decodedRefreshTokenPrincipal = null;
            try
            {
                decodedRefreshTokenPrincipal = new JwtSecurityTokenHandler().ValidateToken(
                    refreshTokenValue,
                    new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key)),
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    },
                    out _
                );
            }
            catch 
            {
                return null;
            }

            return decodedRefreshTokenPrincipal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
        }

        private (string RefreshTokenValue, string RefreshTokenSerial) CreateRefreshToken()
        {
            var refreshTokenSerial = _securityService.CreateCryptographicallySecureGuid().ToString().Replace("-", "");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, _securityService.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, _configuration.Issuer),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration.Issuer, ClaimValueTypes.String, _configuration.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToLocalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _configuration.Issuer),
                new Claim(ClaimTypes.SerialNumber, refreshTokenSerial, ClaimValueTypes.String, _configuration.Issuer)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_configuration.RefreshTokenExpirationMinutes),
                signingCredentials: credentials);
            var refreshTokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return (refreshTokenValue, refreshTokenSerial);
        }

        private (string AccessToken, IEnumerable<Claim> Claims) CreateAccessTokenAsync(AspNetUsers user, string Role, int IdBroker)
        {
            if (Role == "Call-centre")
            {
                Role = "CallСentre";
            }
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, _securityService.CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, _configuration.Issuer),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration.Issuer, ClaimValueTypes.String, _configuration.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToLocalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _configuration.Issuer),
                new Claim(JwtRegisteredClaimNames.Email, user.Email, ClaimValueTypes.String, _configuration.Issuer),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName, ClaimValueTypes.String, _configuration.Issuer),
                new Claim( "IdBroker", IdBroker.ToString(), ClaimValueTypes.String, _configuration.Issuer),
                new Claim(ClaimTypes.SerialNumber, user.SecurityStamp, ClaimValueTypes.String, _configuration.Issuer),
                new Claim(ClaimTypes.UserData, user.Id, ClaimValueTypes.String, _configuration.Issuer),
                new Claim(ClaimTypes.Role, Role, ClaimValueTypes.String, _configuration.Issuer),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_configuration.AccessTokenExpirationMinutes),
                signingCredentials: credentials);
            return (new JwtSecurityTokenHandler().WriteToken(token), claims);
        }
    }
}
