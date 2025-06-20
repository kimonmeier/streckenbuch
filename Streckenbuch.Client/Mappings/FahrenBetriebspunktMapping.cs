using AutoMapper;
using Streckenbuch.Client.Models.Fahren.Betriebspunkt;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Client.Mappings;

public class FahrenBetriebspunktMapping : IMap<FahrenEntry, IBetriebspunktEntry>, IMap<FahrenEntry, BahnhofEntry>, IMap<FahrenEntry, DienstbahnhofEntry>, IMap<FahrenEntry, HaltestelleEntry>
{

    public void Mapping(IMappingExpression<FahrenEntry, IBetriebspunktEntry> mapping)
    {
        mapping
            .ConvertUsing<FahrenEntryConverter>();
    }

    public void Mapping(IMappingExpression<FahrenEntry, HaltestelleEntry> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BetriebspunktId))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Kommentar, opt => opt.MapFrom(src => src.Kommentar))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }

    public void Mapping(IMappingExpression<FahrenEntry, DienstbahnhofEntry> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BetriebspunktId))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }

    public void Mapping(IMappingExpression<FahrenEntry, BahnhofEntry> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BetriebspunktId))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}
