using System.Collections.Generic;
using Core.Interfaces.Mappers;

namespace Core.Domain.Mappers
{
    public abstract class Mapper<TSource, TTarget>: IMapper<TSource, TTarget> where TSource: class where TTarget: class
    {
        public abstract TTarget Map(TSource source);

        public IEnumerable<TTarget> Map(IEnumerable<TSource> source)
        {
            var output = new List<TTarget>();
            foreach (var item in source)
            {
                output.Add(Map(item));
            }
            return output;
        }
    }
}