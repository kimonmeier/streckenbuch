using AutoMapper;
using Streckenbuch.Server.Data.Entities.Signale;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Mappings;

public class SignaleMapping : IMap<Signal, SignalProto>
{
    public void Mapping(IMappingExpression<Signal, SignalProto> mapping)
    {
        mapping
            .ForMember(dest => dest.SignalId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.BetriebspunktId, opt => opt.MapFrom(src => src.BetriebspunktId))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.SignalSeite, opt => opt.MapFrom(src => (int)src.Seite))
            .ForMember(dest => dest.SignalTyp, opt => opt.MapFrom(src => (int)src.Typ));
    }
}
