using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Core.Interfaces;
using Core.Models.Business;
using Core.Models.Dto;
using Core.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.Business;
using Shared.Services;

namespace Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenOption _tokenOption;

        public TokenService(UserManager<AppUser> userManager, IOptions<TokenOption> tokenOption)
        {
            _userManager = userManager;
            _tokenOption = tokenOption.Value;
        }

        private string CreateRefreshToken()
        {
            var bytes = new byte[32];

            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }

        private IEnumerable<Claim> GetClaims(AppUser user, List<string> audiences)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                // new(JwtRegisteredClaimNames.Email, user.Email),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            claims.AddRange(audiences.Select(s => new Claim(JwtRegisteredClaimNames.Aud, s)));

            return claims;
        }

        private IEnumerable<Claim> GetClaims(Client client)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, client.Id),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(client.Audiences.Select(s => new Claim(JwtRegisteredClaimNames.Aud, s)));

            return claims;
        }

        public Token CreateToken(AppUser user)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.RefreshTokenExpiration);

            var securityKey = SignInService.GetSymmetricSecurityKey(_tokenOption.SecurityKey);
            var signInCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOption.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaims(user, _tokenOption.Audiences),
                signingCredentials: signInCredentials
                );
            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);

            return new Token
            {
                AccessToken = token,
                AccessTokenExpiration = accessTokenExpiration,
                RefreshToken = CreateRefreshToken(),
                RefreshTokenExpiration = refreshTokenExpiration
            };
        }

        public ClientToken CreateToken(Client client)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);

            var securityKey = SignInService.GetSymmetricSecurityKey(_tokenOption.SecurityKey);
            var signInCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOption.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaims(client),
                signingCredentials: signInCredentials
                );
            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);

            return new ClientToken
            {
                AccessToken = token,
                Expiration = accessTokenExpiration
            };
        }
    }
}