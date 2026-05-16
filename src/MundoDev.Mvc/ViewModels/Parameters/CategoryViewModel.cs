namespace MundoDev.Mvc.ViewModels.Parameters
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActived { get; set; }
    }
}
