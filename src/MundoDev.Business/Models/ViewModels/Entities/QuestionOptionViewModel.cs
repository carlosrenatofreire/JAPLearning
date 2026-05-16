namespace MundoDev.Business.Models.ViewModels.Entities
{
    public class QuestionOptionViewModel
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsActived { get; set; }
    }
}
