namespace JAPLearning.Business.Models.Settings
{
    public class RedisCacheSetting
    {
        public string ConnectionString { get; set; }
        public string InstanceName { get; set; }
        public int DefaultExpirationInMinutes { get; set; } = 60;
    }
}
