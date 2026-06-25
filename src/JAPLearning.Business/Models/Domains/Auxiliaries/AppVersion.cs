using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Auxiliaries
{
    public class AppVersion : Entity
    {
        public string VersionNumber { get; set; } = string.Empty;   // ex: "0.18"
        public string Title        { get; set; } = string.Empty;   // ex: "Junho 2026"
        public DateTime ReleaseDate { get; set; } = DateTime.UtcNow;
        public bool IsPublished     { get; set; } = false;

        public ICollection<AppVersionItem> Items { get; set; } = new List<AppVersionItem>();
    }
}
