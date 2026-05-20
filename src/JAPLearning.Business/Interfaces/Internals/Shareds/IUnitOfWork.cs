namespace JAPLearning.Business.Interfaces.Internals.Shareds
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
