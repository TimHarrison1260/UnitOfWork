using System.Collections.Generic;

namespace Core.Interfaces.Mappers
{
    public interface IMapper<TSource, TTarget> where TSource : class where TTarget : class
    {
        TTarget Map(TSource source);

        IEnumerable<TTarget> Map(IEnumerable<TSource> source);
    }
}