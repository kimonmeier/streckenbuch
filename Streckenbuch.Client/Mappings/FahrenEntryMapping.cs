using AutoMapper;
using Streckenbuch.Client.Models.Fahren;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Client.Mappings;

public class FahrenEntryMapping : IMap<FahrenEntry, IBaseEntry>
{
    public void Mapping(IMappingExpression<FahrenEntry, IBaseEntry> mapping)
    {
        mapping.ConvertUsing<FahrenEntryConverter>();
    }
}
