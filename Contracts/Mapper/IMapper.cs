using System.Threading.Tasks;

namespace Contracts.Mapper
{
    public interface IMapper
    {
        Task<TDestination> MapAsync<TSource, TDestination>(TSource source, TDestination destination);
    }
}