using AutoMapper;
using Streckenbuch.Client.Models.Fahren;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Services;
using Streckenbuch.Shared.Types;

namespace Streckenbuch.Client.Mappings;

public class LinieMapping : IMap<FahrenStreckenEntryModel, LinienStreckenProto>
{
    public void Mapping(IMappingExpression<FahrenStreckenEntryModel, LinienStreckenProto> mapping)
    {
        mapping
            .ForMember(dest => dest.VonBetriebspunktId, opt => opt.MapFrom(src => src.VonBetriebspunkt.Id))
            .ForMember(dest => dest.BisBetriebspunktId, opt => opt.MapFrom(src => src.BisBetriebspunkt.Id))
            .ForMember(dest => dest.StreckenKonfigurationId, opt => opt.MapFrom(src => src.SelectedKonfiguration.Id))
            .ForMember(dest => dest.SortingNumber, opt => opt.Ignore());
    }
}
