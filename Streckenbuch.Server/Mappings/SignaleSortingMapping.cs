using AutoMapper;
using NetTopologySuite.GeometriesGraph;
using Streckenbuch.Server.Data.Entities.Signale;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Mappings;

public class SignaleSortingMapping : IMap<SignalStreckenZuordnungSortingStrecke, SignalSortingStrecke>, IMap<SignalStreckenZuordnungSortingBetriebspunkt, SignalSortingBetriebspunkt>, IMap<SignalStreckenZuordnungSortingSignal, SignalSortingSignal>
{
    public void Mapping(IMappingExpression<SignalStreckenZuordnungSortingStrecke, SignalSortingStrecke> mapping)
    {
        mapping
            .ForMember(dest => dest.SortingId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.VonDatum, opt => opt.MapFrom(src => src.GueltigVon))
            .ForMember(dest => dest.BisDatum, opt => opt.MapFrom(src => src.GueltigBis))
            .ForMember(dest => dest.Betriebspunkte, opt => opt.MapFrom(src => src.Betriebspunkte));
    }

    public void Mapping(IMappingExpression<SignalStreckenZuordnungSortingBetriebspunkt, SignalSortingBetriebspunkt> mapping)
    {
        mapping
            .ForMember(dest => dest.BetriebspunktId, opt => opt.MapFrom(src => src.BetriebspunktId))
            .ForMember(dest => dest.Signale, opt => opt.MapFrom(src => src.Signale));
    }

    public void Mapping(IMappingExpression<SignalStreckenZuordnungSortingSignal, SignalSortingSignal> mapping)
    {
        mapping
            .ForMember(dest => dest.SignalId, opt => opt.MapFrom(src => src.SignalId))
            .ForMember(dest => dest.SortingNumber, opt => opt.MapFrom(src => src.SortingOrder));
    }
}