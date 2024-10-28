using AutoMapper;
using Streckenbuch.Server.Data.Entities.Strecken;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Mappings;

public class StreckenMapping : IMap<Strecke, StreckenProto>, IMap<StreckenKonfiguration, StreckenKonfigurationProto>
{
    public void Mapping(IMappingExpression<Strecke, StreckenProto> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nummer, opt => opt.MapFrom(src => src.StreckenNummer))
            .ForMember(dest => dest.Konfigurationen, opt => opt.MapAtRuntime())
            .AfterMap<StreckenKonfigurationResolver>();
    }

    public void Mapping(IMappingExpression<StreckenKonfiguration, StreckenKonfigurationProto> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt =>
            {
                opt.Condition(x => !string.IsNullOrEmpty(x.Name));
                opt.MapFrom(src => src.Name);
            })
            .ForMember(dest => dest.BisBetriebspunktName, opt => opt.MapFrom(src => src.BisBetriebspunkt.Name))
            .ForMember(dest => dest.VonBetriebspunktName, opt => opt.MapFrom(src => src.VonBetriebspunkt.Name));
    }
}
