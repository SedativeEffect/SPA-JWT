using System.ComponentModel.DataAnnotations;
using SPA.Models;

namespace SPA.ViewModels
{
    public class CreateModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указан email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Повторный пароль введен не верно")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Не указана роль")]
        public Roles Role { get; set; }

    }
}