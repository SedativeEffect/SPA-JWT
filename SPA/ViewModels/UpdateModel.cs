using System.ComponentModel.DataAnnotations;
using SPA.Models;

namespace SPA.ViewModels
{
    public class UpdateModel
    {
        [Required(ErrorMessage = "Не указан ID")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указан email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Не указана роль")]
        public Roles Role { get; set; }
    }
}