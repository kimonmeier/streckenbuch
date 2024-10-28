using AutoMapper;
using Streckenbuch.Server.Data.Entities.Strecken;
using Streckenbuch.Server.Data.Repositories;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Mappings;

public class StreckenKonfigurationResolver : IMappingAction<Strecke, StreckenProto>
{
    private readonly IMapper _mapper;
    private readonly StreckenKonfigurationRepository _repository;

    public StreckenKonfigurationResolver(IMapper mapper, StreckenKonfigurationRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public void Process(Strecke source, StreckenProto destination, ResolutionContext context)
    {
        var konfigurationen = _repository.ListByStrecke(source.Id).ConfigureAwait(true).GetAwaiter().GetResult();

        destination.Konfigurationen.Add(_mapper.Map<List<StreckenKonfigurationProto>>(konfigurationen));
    }
}
