using System.ComponentModel.DataAnnotations;

namespace JAPLearning.Mvc.ViewModels.Parameters
{
    public class ModuleViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        [Display(Name = "Nome")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Descrição")]
        public string? Description { get; set; }

        [Display(Name = "Activo")]
        public bool IsActived { get; set; } = true;

        // leitura — número de permissões associadas
        public int PermissionCount { get; set; }
    }
}
