namespace JAPLearning.Business.Models.Shareds
{
    public class Email
    {
        public string NameFrom { get; set; }
        public string EmailFrom { get; set; }
        public string? EmailCc { get; set; }
        public string NameTo { get; set; }
        public string EmailTo { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}
