using AutoMapper;
using Streckenbuch.Server.Data.Entities.Betriebspunkte;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Mappings;

public class BetriebspunkteMapping : IMap<Betriebspunkt, BetriebspunktProto>
{
    public void Mapping(IMappingExpression<Betriebspunkt, BetriebspunktProto> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Kommentar, opt => opt.MapFrom(src => src.Kommentar))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.MikuId, opt => opt.MapFrom(src => src.MikuId));
    }
}
