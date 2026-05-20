namespace JAPLearning.Business.Interfaces.Internals.Shareds
{
    public interface IMapperService
    {
        TDestination Map<TSource, TDestination>(TSource source);
        TDestination Map<TDestination>(object source);
    }
}
