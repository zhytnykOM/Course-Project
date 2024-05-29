using System.ComponentModel.DataAnnotations;

namespace InternetStore.Models
{
    public class ModelRegister
    {
        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Ім'я користувача")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Display(Name = "Логін")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [EmailAddress]
        [Display(Name = "Пошта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [Phone]
        [Display(Name = "Мобільний телефон")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердіть пароль")]
        [Compare("Password", ErrorMessage = "Паролі не збігаються.")]
        public string ConfirmPassword { get; set; }
    }
}
