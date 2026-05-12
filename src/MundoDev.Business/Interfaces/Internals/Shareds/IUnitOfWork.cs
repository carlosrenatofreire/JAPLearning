namespace MundoDev.Business.Interfaces.Internals.Shareds
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
