using System.ComponentModel.DataAnnotations;
using SPA.Models;

namespace SPA.ViewModels
{
    public class RegisterViewModel : UserViewModel
    {
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Повторный пароль введен не верно")]
        public string ConfirmPassword { get; set; }

    }
}
