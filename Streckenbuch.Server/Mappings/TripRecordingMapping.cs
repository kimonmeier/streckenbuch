using AutoMapper;
using Streckenbuch.Server.Data.Entities.Shift;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Mappings;

public class TripRecordingMapping : IMap<TripRecording, HistoryPositionData>
{
    public void Mapping(IMappingExpression<TripRecording, HistoryPositionData> mapping)
    {
        mapping
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location));
    }
}