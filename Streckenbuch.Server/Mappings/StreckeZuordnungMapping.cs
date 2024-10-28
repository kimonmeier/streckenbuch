using AutoMapper;
using Streckenbuch.Server.Data.Entities.Betriebspunkte;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Mappings;

public class StreckeZuordnungMapping : IMap<BetriebspunktStreckenZuordnung, StreckeZuordnung>, IMap<BetriebspunktStreckenZuordnung, StreckeZuordnungSignal>
{
    public void Mapping(IMappingExpression<BetriebspunktStreckenZuordnung, StreckeZuordnung> mapping)
    {
        mapping
            .ForMember(dest => dest.StreckenZuordnungId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.BetriebspunktId, opt => opt.MapFrom(src => src.BetriebspunktId))
            .ForMember(dest => dest.SortNummer, opt => opt.MapFrom(src => src.SortNummer));
    }

    public void Mapping(IMappingExpression<BetriebspunktStreckenZuordnung, StreckeZuordnungSignal> mapping)
    {
        mapping
            .ForMember(dest => dest.StreckenZuordnungId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StreckeNummer, opt => opt.MapFrom(src => src.StreckenKonfiguration.Strecke.StreckenNummer))
            .ForMember(dest => dest.StreckenZusatz, opt =>
            {
                opt.Condition(src => !string.IsNullOrEmpty(src.StreckenKonfiguration.Name));
                opt.MapFrom(src => src.StreckenKonfiguration.Name);
            });
    }
}
