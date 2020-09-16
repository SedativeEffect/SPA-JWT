using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SPA.Models;
using SPA.ViewModels;

namespace SPA.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext db;
        public AccountController(ApplicationContext context)
        {
            db = context;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (user == null)
                {
                    user = new User() { Email = model.Email, Login = model.Login, Name = model.Name, Password = model.Password, Role = Roles.User };
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    return Ok(new { text = "Пользователь зарегистрирован" });
                }
                return BadRequest(new { errorText = "Такой пользователь уже существует" });
            }
            return BadRequest(new { errorText = "Не в том формате указаны данные" });
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await db.Users
                    .FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                var identity = Authenticate(user);
                if (identity == null)
                {
                    return BadRequest(new { errorText = "Не правильный логин или пароль" });
                }

                var now = DateTime.UtcNow;
                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var response = new
                {
                    access_token = encodedJwt,
                    username = identity.Name,
                    role = identity.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value,
                };
                return Json(response);
            }

            return BadRequest(new { errorText = "Не в том формате указаны логин или пароль" });
        }
        private ClaimsIdentity Authenticate(User user)
        {
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };
                var id = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return id;
            }
            return null;
        }
        
    }
}
