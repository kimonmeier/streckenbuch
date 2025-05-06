using AutoMapper;
using Streckenbuch.Client.Models.Fahren.Signal;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Client.Mappings;

public class FahrenSignalMapping : IMap<FahrenEntry, ISignalEntry>, IMap<FahrenEntry, ChivronSignalEntry>, IMap<FahrenEntry, HauptSignalEntry>, IMap<FahrenEntry, KombiniertSignalEntry>, IMap<FahrenEntry, VorsignalEntry>, IMap<FahrenEntry, WiederholungsSignalEntry>
{
    public void Mapping(IMappingExpression<FahrenEntry, ISignalEntry> mapping)
    {
        mapping
            .ConvertUsing<FahrenEntryConverter>();
    }

    public void Mapping(IMappingExpression<FahrenEntry, ChivronSignalEntry> mapping)
    {
        mapping
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location));
    }

    public void Mapping(IMappingExpression<FahrenEntry, HauptSignalEntry> mapping)
    {
        mapping
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Kommentar, opt => opt.MapFrom(src => src.Kommentar))
            .ForMember(dest => dest.SignalSeite, opt => opt.MapFrom(src => src.DisplaySeite));
    }

    public void Mapping(IMappingExpression<FahrenEntry, KombiniertSignalEntry> mapping)
    {
        mapping
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Kommentar, opt => opt.MapFrom(src => src.Kommentar))
            .ForMember(dest => dest.SignalSeite, opt => opt.MapFrom(src => src.DisplaySeite));
    }

    public void Mapping(IMappingExpression<FahrenEntry, VorsignalEntry> mapping)
    {
        mapping
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location));
    }

    public void Mapping(IMappingExpression<FahrenEntry, WiederholungsSignalEntry> mapping)
    {
        mapping
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location));
    }
}
