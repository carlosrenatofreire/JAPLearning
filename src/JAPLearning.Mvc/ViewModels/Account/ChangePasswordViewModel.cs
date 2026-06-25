using System.ComponentModel.DataAnnotations;

namespace JAPLearning.Mvc.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "A senha actual é obrigatória.")]
        [Display(Name = "Senha Actual")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "A nova senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A nova senha deve ter pelo menos 6 caracteres.")]
        [Display(Name = "Nova Senha")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirmação da senha é obrigatória.")]
        [Compare(nameof(NewPassword), ErrorMessage = "As senhas não coincidem.")]
        [Display(Name = "Confirmar Nova Senha")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
