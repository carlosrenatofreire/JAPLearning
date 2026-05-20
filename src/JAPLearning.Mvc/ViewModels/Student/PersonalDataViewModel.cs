using System.ComponentModel.DataAnnotations;

namespace JAPLearning.Mvc.ViewModels.Student
{
    public class PersonalDataViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [Display(Name = "Primeiro Nome")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "O apelido é obrigatório.")]
        [Display(Name = "Apelido")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Telefone")]
        public string? Phone { get; set; }

        public string? CurrentPhotoUrl { get; set; }
    }
}
