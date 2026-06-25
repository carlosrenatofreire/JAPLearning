using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Models.Domains.Auxiliaries
{
    public enum VersionItemType
    {
        Feature     = 1,    // Novo
        Improvement = 2,    // Melhoria
        Fix         = 3     // Correcção
    }

    public class AppVersionItem : Entity
    {
        public Guid VersionId         { get; set; }
        public VersionItemType Type   { get; set; } = VersionItemType.Feature;
        public string Description     { get; set; } = string.Empty;
        public int Order              { get; set; } = 0;

        public AppVersion Version { get; set; } = null!;
    }
}
