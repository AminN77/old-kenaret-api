using System.Threading.Tasks;
using AutoMapper;
using IMapper = Contracts.Mapper.IMapper;

namespace Infrastructure.Mapper
{
    public class Mapper : IMapper
    {
         public Task<TDestination> MapAsync<TSource, TDestination>(TSource source, TDestination destination)
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<TSource, TDestination>();
            });
            var mapper = mapperConfiguration.CreateMapper();
            return Task.Run(() => mapper.Map(source, destination));
        }
         
    }
}