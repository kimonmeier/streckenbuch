using AutoMapper;
using Streckenbuch.Server.Data.Entities.Betriebspunkte;
using Streckenbuch.Server.Data.Entities.Signale;
using Streckenbuch.Server.Models;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Models;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Mappings;

public class FahrenEntryMap : IMap<FahrenTransferEntry, FahrenEntry>, IMap<Betriebspunkt, FahrenEntry>, IMap<SignalStreckenZuordnung, FahrenEntry>
{

    public void Mapping(IMappingExpression<FahrenTransferEntry, FahrenEntry> mapping)
    {
        mapping.ConvertUsing<FahrenEntryConverter>();
    }

    public void Mapping(IMappingExpression<SignalStreckenZuordnung, FahrenEntry> mapping)
    {
        mapping
            .ForMember(dest => dest.EntryTyp, opt => opt.MapFrom(_ => EntryType.Signal))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Signal.Location))
            .ForMember(dest => dest.SignalTyp, opt => opt.MapFrom(src => src.Signal.Typ))
            .ForMember(dest => dest.DisplaySeite, opt => opt.MapAtRuntime())
            .ForMember(dest => dest.Kommentar, opt =>
            {
                opt.Condition(src => !string.IsNullOrEmpty(src.NonStandardKommentar));
                opt.MapFrom(src => src.NonStandardKommentar);
            })
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .ForMember(dest => dest.BetriebspunktTyp, opt => opt.Ignore())
            .ForMember(dest => dest.BetriebspunktId, opt => opt.Ignore());
    }

    public void Mapping(IMappingExpression<Betriebspunkt, FahrenEntry> mapping)
    {
        mapping
            .ForMember(dest => dest.EntryTyp, opt => opt.MapFrom(_ => EntryType.Betriebspunkt))
            .ForMember(dest => dest.BetriebspunktTyp, opt => opt.MapFrom(src => src.Typ))
            .ForMember(dest => dest.Kommentar, opt =>
            {
                opt.Condition(src => !string.IsNullOrEmpty(src.Kommentar));
                opt.MapFrom(src => src.Kommentar);
            })
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.BetriebspunktId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.DisplaySeite, opt => opt.Ignore())
            .ForMember(dest => dest.SignalTyp, opt => opt.Ignore());
    }
}
