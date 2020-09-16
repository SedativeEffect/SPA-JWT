﻿using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPA.Models;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SPA.ViewModels;

namespace SPA.Controllers
{

    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private ApplicationContext db;
        public TestController(ApplicationContext context)
        {
            db = context;
        }
        [Authorize]
        [Route("getusers")]
        public async Task<IActionResult> GetUsers()
        {
            string jsonUsers = JsonSerializer.Serialize(await db.Users.ToListAsync());
            return Json(jsonUsers);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("deleteuser/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return Ok(new { text = $"Пользователь с логином {user.Login} удален из БД" });
            }

            return BadRequest(new { errorText = "Пользователь с таким Id не найден" });
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("createuser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (user == null)
                {
                    user = new User() { Email = model.Email, Login = model.Login, Name = model.Name, Password = model.Password, Role = model.Role };
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    return Ok(new { text = "Пользователь создан" });
                }
                return BadRequest(new { errorText = "Такой пользователь уже существует" });
            }
            return BadRequest(new { errorText = "Не в том формате указаны данные" });
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("updateuser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errorText = "Не валидная модель" });
            }
            var user = await db.Users.Where(u => u.Id == model.Id).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Name = model.Name;
                user.Login = model.Login;
                user.Email = model.Email;
                user.Password = model.Password;
                user.Role = model.Role;
                await db.SaveChangesAsync();
                return Ok(new { text = "Пользователь обновлен" });
            }
            return BadRequest(new { errorText = "Пользователь не найден" });
        }
    }
}