using AutoMapper;
using Streckenbuch.Server.Data.Entities.Linien;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Mappings;

public class LinienMapping : IMap<Linie, LinienProto>, IMap<LinienStreckenKonfigurationen, LinienStreckenProto>
{
    public void Mapping(IMappingExpression<Linie, LinienProto> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.VonBetriebspunktId, opt => opt.MapFrom(src => src.VonBetriebspunktId))
            .ForMember(dest => dest.BisBetriebspunktId, opt => opt.MapFrom(src => src.BisBetriebspunktId))
            .ForMember(dest => dest.Typ, opt => opt.MapFrom(src => src.Typ))
            .ForMember(dest => dest.Nummer, opt => opt.MapFrom(src => src.Nummer))
            .ForMember(dest => dest.Strecken, opt => opt.MapAtRuntime())
            .AfterMap<LinienStreckenResolver>();
    }

    public void Mapping(IMappingExpression<LinienStreckenKonfigurationen, LinienStreckenProto> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StreckenKonfigurationId, opt => opt.MapFrom(src => src.StreckenKonfigurationId))
            .ForMember(dest => dest.VonBetriebspunktId, opt => opt.MapFrom(src => src.VonBetriebspunktId))
            .ForMember(dest => dest.BisBetriebspunktId, opt => opt.MapFrom(src => src.BisBetriebspunktId))
            .ForMember(dest => dest.SortingNumber, opt => opt.MapFrom(src => src.Order));
    }
}
