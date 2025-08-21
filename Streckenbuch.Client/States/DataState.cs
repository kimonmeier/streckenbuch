using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MudBlazor.Extensions;
using Streckenbuch.Components.States;
using Streckenbuch.Shared.Services;
using System.Runtime.CompilerServices;

namespace Streckenbuch.Client.States;

public class DataState : IDataState
{
    private readonly BetriebspunkteService.BetriebspunkteServiceClient _betriebspunkteService;

    private readonly List<BetriebspunktProto> _betriebspunkte = new();

    public DataState(BetriebspunkteService.BetriebspunkteServiceClient betriebspunkteService)
    {
        _betriebspunkteService = betriebspunkteService;
    }

    public async Task<List<BetriebspunktProto>> FetchBetriebspunkte()
    {
        if (_betriebspunkte.Count == 0)
        {
            _betriebspunkte.AddRange((await _betriebspunkteService.ListAllBetriebspunkteAsync(new Empty())).Betriebspunkte);
        }

        return _betriebspunkte;
    }

    public async Task<BetriebspunktProto?> FetchBetriebspunkt(Guid id)
    {
        var list = await FetchBetriebspunkte();
        return list.FirstOrDefault(x => x.Id == id);
    }
}