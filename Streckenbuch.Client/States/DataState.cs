using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MudBlazor.Extensions;
using Streckenbuch.Shared.Services;
using System.Runtime.CompilerServices;

namespace Streckenbuch.Client.States;

public class DataState
{
    public IReadOnlyList<BetriebspunktProto> Betriebspunkte => _betriebspunkte.AsReadOnly();
    
    
    private readonly List<BetriebspunktProto> _betriebspunkte = new();

    public DataState(BetriebspunkteService.BetriebspunkteServiceClient betriebspunkteService)
    {
        FetchBetriebspunkte(betriebspunkteService);
    }

    private void FetchBetriebspunkte(BetriebspunkteService.BetriebspunkteServiceClient betriebspunkteService)
    {
        var responseTask = betriebspunkteService.ListAllBetriebspunkteAsync(new Empty()).ResponseAsync;

        responseTask.ConfigureAwait(false);
        responseTask.ContinueWith(x => _betriebspunkte.AddRange(x.Result.Betriebspunkte));
    }
    
    
}