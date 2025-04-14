using AutoMapper;
using Grpc.Core;
using Streckenbuch.Server.Data.Repositories;
using Streckenbuch.Server.Models;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Services;

public class FahrenService : Streckenbuch.Shared.Services.FahrenService.FahrenServiceBase
{
    private readonly FahrenRepository _fahrenRepository;
    private readonly IMapper _mapper;

    public FahrenService(FahrenRepository fahrenRepository, IMapper mapper)
    {
        _fahrenRepository = fahrenRepository;
        _mapper = mapper;
    }

    public override Task<FahrenResponse> FahrenByLinie(FahrenRequestByLinie request, ServerCallContext context)
    {
        List<FahrenTransferEntry> entries = _fahrenRepository.ListEntriesByLinie(request.LinieId).ToList();
        
        RemoveDuplicates(entries);
        
        FahrenResponse response = new();
        response.Entries.AddRange(_mapper.Map<List<FahrenEntry>>(entries));
        return Task.FromResult(response);
    }

    public override Task<FahrenResponse> FahrenByStrecken(FahrenRequestByStrecken request, ServerCallContext context)
    {
        List<FahrenTransferEntry> entries = request.Strecken.SelectMany(GetTrimmedEntries).ToList();

        RemoveDuplicates(entries);

        FahrenResponse response = new();
        response.Entries.AddRange(_mapper.Map<List<FahrenEntry>>(entries));
        return Task.FromResult(response);
    }

    private void RemoveDuplicates(List<FahrenTransferEntry> entries) {
        var duplicateEntries = entries.Where(x => x.Betriebspunkt is not null).GroupBy(x => x.Betriebspunkt.Id).Where(x => x.Count() > 1).ToList();

        foreach (var item in duplicateEntries)
        {
            if (item.Count() > 2)
            {
                throw new Exception("Something is really wrong");
            }

            var firstEntry = item.First();
            var secondEntry = item.Last();

            var firstIndex = entries.IndexOf(firstEntry);
            var secondIndex = entries.IndexOf(secondEntry);

            var currentIndex = firstIndex + 1;

            while (currentIndex < secondIndex)
            {
                entries.RemoveAt(currentIndex);
                currentIndex++;
            }
        }
    }

    private List<FahrenTransferEntry> GetTrimmedEntries(FahrenRequestStrecke strecke)
    {
        var entries = _fahrenRepository.ListEntriesByStrecke(strecke.StreckenKonfigurationId);

        while (!entries.Where(x => x.Betriebspunkt != null).First().Betriebspunkt!.Id.Equals(strecke.VonBetriebspunktId))
        {
            entries.RemoveAt(0);
        }

        while (!entries.Where(x => x.Betriebspunkt != null).Last().Betriebspunkt!.Id.Equals(strecke.BisBetriebspunktId))
        {
            entries.RemoveAt(entries.Count - 1);
        }

        return entries;
    }
}
