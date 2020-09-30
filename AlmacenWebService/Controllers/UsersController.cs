using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AlmacenWebService.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AlmacenWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration configuration;

        public UsersController (
                     UserManager<User> userManager,
                     SignInManager<User> signInManager,
                     IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }


        [HttpPost("create")]
        public async Task<ActionResult<UserToken>> create([FromBody] UserInfo model)
        {
            var user = new User() { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest("Email or password are invalid");

            return BuildToken(model);
        }

        //[HttpPost("login")]
        //public async Task<ActionResult<UserToken>> login([FromBody] UserInfo userInfo)
        //{
        //    var result = await signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password);
        //}


        private UserToken BuildToken(UserInfo user)
        {
            var claims = new[]
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName, user.Email),
                new Claim("miValor", "lo que yo quiera"),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

            return new UserToken() { Expiration = expiration, Token = new JwtSecurityTokenHandler().WriteToken(token) };
        }
    }
}
