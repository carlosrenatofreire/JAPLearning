namespace JAPLearning.Business.Interfaces.Internals.Shareds
{
    public interface IAccessorService
    {
        string GetUsername();
        Guid GetUserId();
        bool IsAuthenticated();
        bool IsInRole(string role);
        string GetRemoteIpAddress();
        string GetLocalIpAddress();
    }
}
