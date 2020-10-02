using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AlmacenWebService.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult<UserToken>> create([FromBody] UserInfo model)
        {
            var user = new User() { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest("Email or password are invalid");

            return BuildToken(model, new List<string>());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> login([FromBody] UserInfo userInfo)
        {
            var result = await signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                return BadRequest(ModelState);
            }

            var user = await userManager.FindByEmailAsync(userInfo.Email);
            var roles = await userManager.GetRolesAsync(user);
            return BuildToken(userInfo, new List<string>(roles));
        }


        [HttpPost("assign-role")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<ActionResult> addUserToARole([FromBody] UserRole userRole)
        {
            if (userRole == null)
                return BadRequest("Must provide a userId and Role to assign the user to");

            if (userRole.UserId == null)
                return BadRequest("Must provide an userId");

            if (userRole.Role == null)
                return BadRequest("Must provide a Role");

            var user = await userManager.FindByIdAsync(userRole.UserId);

            if (user == null)
                return NotFound();

            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, userRole.Role));
            await userManager.AddToRoleAsync(user, userRole.Role);

            return NoContent();
        }


        private UserToken BuildToken(UserInfo user, List<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName, user.Email),
                new Claim("miValor", "lo que yo quiera"),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            roles.ForEach(role => {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(
                    issuer: null,
                    audience: null,
                    claims: claims,
                    expires: expiration,
                    signingCredentials: creds
                );

            return new UserToken() { Expiration = expiration, Token = new JwtSecurityTokenHandler().WriteToken(token) };
        }
    }
}
