namespace JAPLearning.Mvc.ViewModels.Parameters
{
    public class SubjectViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActived { get; set; }
    }
}
