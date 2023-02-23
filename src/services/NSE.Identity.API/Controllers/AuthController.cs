using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSE.Identity.API.Extensions;
using NSE.Identity.API.Models;
using NSE.Identity.API.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace NSE.Identity.API.Controllers
{
    [ApiController]
    [Route("api/identidade")]
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;
        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IJwtService jwtService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Register(UserRegister userRegister)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = new IdentityUser
            {
                UserName = userRegister.Email,
                Email = userRegister.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userRegister.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                return Ok(await _jwtService.GenerateJwtAsync(userRegister.Email));
            }

            return BadRequest();
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return Ok(await _jwtService.GenerateJwtAsync(userLogin.Email));

            }

            return BadRequest();
        }

        //private async Task<UserResponseLogin> GenerateJwt(string email)
        //{
        //    var user = await _userManager.FindByNameAsync(email);
        //    var claims = await _userManager.GetClaimsAsync(user);
        //    var userRoles = await _userManager.GetRolesAsync(user);

        //    claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        //    claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        //    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        //    claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        //    claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

        //    foreach (var userRole in userRoles)
        //    {
        //        claims.Add(new Claim("role", userRole));
        //    }

        //    var identityClaims = new ClaimsIdentity();
        //    identityClaims.AddClaims(claims);

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

        //    var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        //    {
        //        Issuer = _appSettings.Issuer,
        //        Audience = _appSettings.ValidatedAt,
        //        Subject = identityClaims,
        //        Expires = DateTime.Now.AddHours(_appSettings.ExpirationHours),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    });

        //    var encodedToken = tokenHandler.WriteToken(token);

        //    var respnse = new UserResponseLogin()
        //    {
        //        AccessToken = encodedToken,
        //        ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
        //        UserToken = new UserToken
        //        {
        //            Id = user.Id,
        //            Email = user.Email,
        //            Claims = claims.Select(x => new UserClaim { Type = x.Type, Value = x.Value })
        //        }
        //    };

        //    return respnse;
        //}

        //private static long ToUnixEpochDate(DateTime date) =>
        //    (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
