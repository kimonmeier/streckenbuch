using AutoMapper;
using Streckenbuch.Server.Data.Entities.Shift;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Mappings;

public class WorkTripMapping : IMap<WorkTrip, HistoryEntries>
{
    public void Mapping(IMappingExpression<WorkTrip, HistoryEntries> mapping)
    {
        mapping
            .ForMember(dest => dest.TrainNumber, opt => opt.MapFrom(src => src.TripNumber))
            .ForMember(dest => dest.EntryId, opt => opt.MapFrom(src => src.Id));
    }
}