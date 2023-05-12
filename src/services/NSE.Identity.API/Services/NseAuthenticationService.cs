using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSE.Identity.API.Data;
using NSE.Identity.API.Extensions;
using NSE.Identity.API.Models;
using NSE.Identity.API.Services.Interfaces;
using NSE.WebAPI.Core.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace NSE.Identity.API.Services
{
    public class NseAuthenticationService : INseAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _context;
        private readonly AppTokenSettings _appTokenSettings;

        public NseAuthenticationService(UserManager<IdentityUser> userManager, 
                                        IOptions<AppSettings> appSettings, 
                                        ApplicationDbContext applicationDbContext,
                                        IOptions<AppTokenSettings> appTokenSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _context = applicationDbContext;
            _appTokenSettings = appTokenSettings.Value;
        }

        public async Task<UserIdentityResponseLogin> GenerateJwtAsync(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            var claims = await GetClaimsAsync(user);

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);
            var encodedToken = GenerateToken(identityClaims);

            var refreshToken = await GenerateRefreshToken(user.Email);

            var response = BuildUserResponseLogin(encodedToken, user, claims, refreshToken);

            return response;
        }

        public async Task<RefreshToken> GetRefreshToken(Guid refreshToken)
        {
            var token = await _context.RefreshTokens.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Token == refreshToken);

            return token != null && token.ExpirationDate.ToLocalTime() > DateTime.Now
                ? token
                : null;
        }

        private async Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            return claims;
        }

        private string GenerateToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.ValidatedAt,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }

        private UserIdentityResponseLogin BuildUserResponseLogin(string encodedToken, IdentityUser user, IList<Claim> claims, RefreshToken refreshToken)
        {
            var response = new UserIdentityResponseLogin()
            {
                AccessToken = encodedToken,
                RefreshToken = refreshToken.Token,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
                UserToken = new UserIdentityToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(x => new UserIdentityClaim { Type = x.Type, Value = x.Value })
                }
            };

            return response;
        }

        private static long ToUnixEpochDate(DateTime date) =>
            (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private async Task<RefreshToken> GenerateRefreshToken(string email)
        {
            var refreshToken = new RefreshToken
            {
                Username = email,
                ExpirationDate = DateTime.UtcNow.AddHours(_appTokenSettings.RefreshTokenExpiration)
            };

            _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(u => u.Username == email));
            await _context.RefreshTokens.AddAsync(refreshToken);

            await _context.SaveChangesAsync();

            return refreshToken;
        }
    }
}
