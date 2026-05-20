namespace JAPLearning.Mvc.ViewModels.Parameters
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string? Description { get; set; }
        public bool IsActived { get; set; }
    }
}
