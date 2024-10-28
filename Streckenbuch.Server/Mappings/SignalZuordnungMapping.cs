using AutoMapper;
using Streckenbuch.Server.Data.Entities.Signale;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Mappings;

public class SignalZuordnungMapping : IMap<SignalStreckenZuordnung, SignalZuordnung>
{
    public void Mapping(IMappingExpression<SignalStreckenZuordnung, SignalZuordnung> mapping)
    {
        mapping
            .ForMember(dest => dest.StreckeBetriebspunktZuordnungId, opt => opt.MapFrom(src => src.StreckeId))
            .ForMember(dest => dest.SignalZuordnungId, opt => opt.MapFrom(src => src.Id));
    }
}
