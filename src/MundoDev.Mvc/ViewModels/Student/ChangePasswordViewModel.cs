using System.ComponentModel.DataAnnotations;

namespace MundoDev.Mvc.ViewModels.Student
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "A senha atual é obrigatória.")]
        [Display(Name = "Senha Atual")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "A nova senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres.")]
        [Display(Name = "Nova Senha")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirme a nova senha.")]
        [Compare("NewPassword", ErrorMessage = "As senhas não coincidem.")]
        [Display(Name = "Confirmar Nova Senha")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
