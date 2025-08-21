using AutoMapper;
using Streckenbuch.Components.Models;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Client.Mappings;

public class StreckenProtoMapping : IMap<StreckenProto, StreckenModel>, IMap<StreckenKonfigurationProto, StreckenKonfigurationenModel>
{
    public void Mapping(IMappingExpression<StreckenProto, StreckenModel> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nummer, opt => opt.MapFrom(src => src.Nummer))
            .ForMember(dest => dest.Konfigurationen, opt => opt.MapFrom<StreckenKonfigurationResolver>())
            .ForMember(dest => dest.ShowDetails, opt => opt.Ignore());
    }

    public void Mapping(IMappingExpression<StreckenKonfigurationProto, StreckenKonfigurationenModel> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.VonBetriebspunkt, opt => opt.MapFrom(src => src.VonBetriebspunktName))
            .ForMember(dest => dest.BisBetriebspunkt, opt => opt.MapFrom(src => src.BisBetriebspunktName));
    }
}
