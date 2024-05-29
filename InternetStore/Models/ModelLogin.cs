    using System.ComponentModel.DataAnnotations;

    namespace InternetStore.Models
    {
        public class ModelLogin
        {
            [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
            [Display(Name = "Логін")]
            public string UserName { get; set; }
            [Required(ErrorMessage = "Поле обов'язкове для заповнення!")]
            [DataType(DataType.Password, ErrorMessage = "Неправильний формат пароля!")]
            [Display(Name = "Пароль")]
            public string Password { get; set; }
            [Display(Name = "Запам'ятати мене")]
            public bool RememberMe { get; set; } = false;
        }
    }
