using AutoMapper;

namespace Streckenbuch.Shared.Mapping;

public interface IMap<TSource, TDestination>
    where TSource : class
    where TDestination : class
{
    public void Mapping(IMappingExpression<TSource, TDestination> mapping);
}
