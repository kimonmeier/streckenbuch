using AutoMapper;
using Streckenbuch.Server.Data.Entities.Linien;
using Streckenbuch.Server.Data.Repositories;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Mappings;

public class LinienStreckenResolver : IMappingAction<Linie, LinienProto>
{
    private readonly IMapper _mapper;
    private readonly LinienKonfigurationRepository _repository;

    public LinienStreckenResolver(IMapper mapper, LinienKonfigurationRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public void Process(Linie source, LinienProto destination, ResolutionContext context)
    {
        var list = _repository.ListByLinie(source.Id).ConfigureAwait(true).GetAwaiter().GetResult();

        destination.Strecken.Add(_mapper.Map<List<LinienStreckenProto>>(list));
    }
}
