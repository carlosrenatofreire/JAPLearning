using JAPLearning.Business.Models.Domains.Auxiliaries;

namespace JAPLearning.Mvc.ViewModels.Auxiliaries
{
    public class AppVersionViewModel
    {
        public Guid     Id            { get; set; }
        public string   VersionNumber { get; set; } = string.Empty;
        public string   Title         { get; set; } = string.Empty;
        public DateTime ReleaseDate   { get; set; } = DateTime.UtcNow;
        public bool     IsPublished   { get; set; }
        public List<AppVersionItemViewModel> Items { get; set; } = new();
    }

    public class AppVersionItemViewModel
    {
        public Guid            Id          { get; set; }
        public Guid            VersionId   { get; set; }
        public VersionItemType Type        { get; set; } = VersionItemType.Feature;
        public string          Description { get; set; } = string.Empty;
        public int             Order       { get; set; }
    }
}
