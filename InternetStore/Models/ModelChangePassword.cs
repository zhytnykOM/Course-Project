using System.ComponentModel.DataAnnotations;

namespace InternetStore.Models
{
    public class ModelChangePassword
    {
        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [DataType(DataType.Password)]
        [Display(Name = "Ваш пароль")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [StringLength(100, ErrorMessage = "Довжина пароля менше 8 символів!", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Новий пароль")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердіть новий пароль")]
        [Compare("NewPassword", ErrorMessage = "Паролі не співпадають!")]
        public string ConfirmPassword { get; set; }
    }
}
