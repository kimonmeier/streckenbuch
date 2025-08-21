using AutoMapper;
using Streckenbuch.Components.Models;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Client.Mappings;

public class StreckenKonfigurationResolver : IValueResolver<StreckenProto, StreckenModel, List<StreckenKonfigurationenModel>>
{
    private readonly IMapper _mapper;

    public StreckenKonfigurationResolver(IMapper mapper)
    {
        _mapper = mapper;
    }

    public List<StreckenKonfigurationenModel> Resolve(StreckenProto source, StreckenModel destination, List<StreckenKonfigurationenModel> destMember, ResolutionContext context)
    {
        return _mapper.Map<List<StreckenKonfigurationenModel>>(source.Konfigurationen.ToList());
    }
}
