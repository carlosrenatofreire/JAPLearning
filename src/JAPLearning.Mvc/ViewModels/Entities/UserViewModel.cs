using System.ComponentModel.DataAnnotations;

namespace JAPLearning.Mvc.ViewModels.Entities
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Perfil é obrigatório.")]
        [Display(Name = "Perfil")]
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MaxLength(100)]
        [Display(Name = "Nome")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Apelido é obrigatório.")]
        [MaxLength(100)]
        [Display(Name = "Apelido")]
        public string LastName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [MaxLength(200)]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        [Display(Name = "Senha")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
        [Display(Name = "Confirmar Senha")]
        public string? ConfirmPassword { get; set; }

        [MaxLength(20)]
        [Display(Name = "Telefone")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Foto (URL)")]
        public string? PhotoUrl { get; set; }

        [Display(Name = "Morada")]
        public string? Address { get; set; }

        [Display(Name = "Número")]
        public string? Number { get; set; }

        [Display(Name = "Cidade")]
        public string? City { get; set; }

        [Display(Name = "Estado")]
        public string? State { get; set; }

        [Display(Name = "País")]
        public string? Country { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ChangedDate { get; set; }

        [Display(Name = "Activo")]
        public bool IsActived { get; set; } = true;
    }
}
