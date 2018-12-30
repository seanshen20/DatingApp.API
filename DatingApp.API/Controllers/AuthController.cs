using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto user)
        {
            // validate request
            user.Name = user.Name.ToLower();
            if (await _repo.UserExists(user.Name))
                return BadRequest("User already exsits");

            var userToCreate = new User
            {
                Name = user.Name
            };

            userToCreate = await _repo.Register(userToCreate, user.Password);

            // create at route, define adress to retrive value
            return StatusCode(201);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto user)
        {
            User u = await _repo.Login(user.Name, user.Password);
            // Unauthorized();
            if (u == null) return BadRequest("Cannot log in");

            //create JWT, claims, credential, tokenDescriptor
            var alist = new string[]{"apple", "orange"};
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, u.Id.ToString()),
                new Claim(ClaimTypes.Name, u.Name),
                new Claim(ClaimTypes.UserData, string.Join(",", alist))
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            // hash
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            // issue token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
                Issuer = "google"
            };

            // display token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return Ok(new
            {
                issueToken = tokenHandler.WriteToken(token)
            });
        }
    }
}
