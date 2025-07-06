using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Streckenbuch.Server.Data.Entities.Shift;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Mappings;
    
public class WorkShiftMapping : IMap<WorkShift, HistoryDays>
{
    public void Mapping(IMappingExpression<WorkShift, HistoryDays> mapping)
    {
        mapping
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Datum))
            .ForMember(dest => dest.Entries, opt => opt.MapFrom(src => src.WorkTrips));
    }
}