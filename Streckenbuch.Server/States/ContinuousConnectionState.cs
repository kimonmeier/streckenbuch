using Google.Protobuf;
using MediatR;
using Streckenbuch.Miku.Models.Fahrten;
using Streckenbuch.Server.Data.Repositories;
using Streckenbuch.Shared.Contracts;
using Streckenbuch.Shared.Services;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Streckenbuch.Server.States;

public class ContinuousConnectionState
{
    private readonly ConcurrentDictionary<Guid, ConcurrentQueue<IRequest<Unit>>> _messageQeue = new();
    private readonly ConcurrentDictionary<Guid, int> _registeredTrains = new();
    private readonly Dictionary<Guid, List<Haltestellen>> _processedHaltestellen = new Dictionary<Guid, List<Haltestellen>>();

    private readonly BetriebspunkteRepository _betriebspunkteRepository;

    public ContinuousConnectionState(BetriebspunkteRepository betriebspunkteRepository)
    {
        _betriebspunkteRepository = betriebspunkteRepository;
    }

    public IReadOnlyList<Guid> GetRegisteredClients()
    {
        return _messageQeue.Keys.ToList().AsReadOnly();
    }

    public ReadOnlyDictionary<Guid, int> GetRegisteredTrainOperator()
    {
        return _registeredTrains.AsReadOnly();
    }

    public void RegisterTrain(Guid clientId, int trainNumber)
    {
        if (_registeredTrains.TryAdd(clientId, trainNumber))
        {
            return;
        }

        UnregisterTrain(clientId);
        RegisterTrain(clientId, trainNumber);
    }

    public void UnregisterTrain(Guid clientId)
    {
        _registeredTrains.TryRemove(clientId, out _);
    }

    public void DisconnectTrainOperator(Guid clientId)
    {
        _messageQeue.Remove(clientId, out _);
        _registeredTrains.Remove(clientId, out _);
    }

    public List<Message> GetMessagesInQueue(Guid clientId)
    {
        if (!_messageQeue.ContainsKey(clientId))
        {
            if (!_messageQeue.TryAdd(clientId, new ConcurrentQueue<IRequest<Unit>>()))
            {
                throw new Exception("Something went wrong");
            }
        }

        _messageQeue.TryGetValue(clientId, out ConcurrentQueue<IRequest<Unit>>? queue);


        List<Message> messages = new();

        while (queue!.TryDequeue(out IRequest<Unit>? request))
        {
            if (request is null)
            {
                throw new Exception("Something went wrong");
            }

            Message message = new Message()
            {
                Type = request.GetType().FullName, Data = ByteString.CopyFrom(JsonSerializer.SerializeToUtf8Bytes(request))
            };

            messages.Add(message);
        }

        return messages;
    }

    public async Task ProcessMikuInformation(Guid clientId, List<Haltestellen> haltestellen)
    {
        if (!_processedHaltestellen.TryGetValue(clientId, out var oldHaltestellen))
        {
            _processedHaltestellen.Add(clientId, haltestellen);

            EnqueueMessageClient(clientId, new StopsLoaded()
            {
                BetriebspunkteId = await _betriebspunkteRepository.ListIdsByMikuIds(haltestellen.Select(x => x.BetriebspunktId).ToList())
            });

            return;
        }

        var oldStops = oldHaltestellen.ToDictionary(h => h.BetriebspunktId);
        var newStops = haltestellen.ToDictionary(h => h.BetriebspunktId);

        var added = haltestellen.Where(h => !oldStops.ContainsKey(h.BetriebspunktId)).ToList();
        var removed = oldHaltestellen.Where(h => !newStops.ContainsKey(h.BetriebspunktId)).ToList();
        var existing = haltestellen.Where(h => oldStops.ContainsKey(h.BetriebspunktId)).ToList();

        var changed = new List<Haltestellen>();
        foreach (var stop in existing)
        {
            var oldStop = oldStops[stop.BetriebspunktId];

            if (!oldStop.Equals(stop))
            {
                changed.Add(stop);
            }
        }

        foreach (Haltestellen addedHaltestelle in added)
        {
            await EnqueueStopAddedMessageAsync(clientId, addedHaltestelle);
        }

        foreach (Haltestellen removedHaltestelle in removed)
        {
            await HandleStopRemovedMessageAsync(clientId, removedHaltestelle);
        }

        foreach (Haltestellen changedHaltestellen in changed)
        {
            await HandleChangedMessageAsync(clientId, changedHaltestellen, oldStops[changedHaltestellen.BetriebspunktId], haltestellen[haltestellen.IndexOf(changedHaltestellen) - 1]);
        }

        _processedHaltestellen[clientId] = haltestellen;
    }

    private async Task EnqueueStopAddedMessageAsync(Guid clientId, Haltestellen addedHaltestelle)
    {
        var betriebspunktId = await _betriebspunkteRepository.GetIdByMikuId(addedHaltestelle.BetriebspunktId);

        EnqueueMessageClient(clientId, new StopAdded()
        {
            BetriebspunktId = betriebspunktId,
        });
    }

    private async Task HandleStopRemovedMessageAsync(Guid clientId, Haltestellen removedHaltestelle)
    {
        var betriebspunktId = await _betriebspunkteRepository.GetIdByMikuId(removedHaltestelle.BetriebspunktId);

        EnqueueMessageClient(clientId, new StopRemoved()
        {
            BetriebspunktId = betriebspunktId,
        });
    }

    private async Task HandleChangedMessageAsync(Guid clientId, Haltestellen changedHaltestelle, Haltestellen oldHaltestelle, Haltestellen previousHaltestelle)
    {
        var betriebspunktId = await _betriebspunkteRepository.GetIdByMikuId(changedHaltestelle.BetriebspunktId);

        HandleStopCancellation(clientId, changedHaltestelle, oldHaltestelle, betriebspunktId);
        HandleStopDelay(clientId, changedHaltestelle, oldHaltestelle, betriebspunktId, previousHaltestelle);
    }
    
    private void HandleStopCancellation(Guid clientId, Haltestellen changedHaltestelle, Haltestellen oldHaltestelle, Guid betriebspunktId)
    {
        var isNewCancellation = changedHaltestelle.Ausfallgrund is not null
                                && changedHaltestelle.Ausfallgrund != oldHaltestelle.Ausfallgrund;

        if (!isNewCancellation)
        {
            return;
        }
        
        EnqueueMessageClient(clientId, new StopCanceled
        {
            BetriebspunktId = betriebspunktId, Reason = changedHaltestelle.Ausfallgrund!.Deutsch
        });
    }
    
    private void HandleStopDelay(Guid clientId, Haltestellen changedHaltestelle, Haltestellen oldHaltestelle, Guid betriebspunktId, Haltestellen previousHaltestelle)
    {
        if (changedHaltestelle.Verspaetungsgrund is null)
        {
            return;
        }

        if (previousHaltestelle.Verspaetungsgrund is not null)
        {
            return;
        }

        var delayToReport = DetermineReportableDelay(changedHaltestelle, oldHaltestelle);

        if (!delayToReport.HasValue)
        {
            return;
        }
        
        EnqueueMessageClient(clientId, new StopDelayIntroduced
        {
            BetriebspunktId = betriebspunktId, MinutesDelayed = delayToReport.Value, Reason = changedHaltestelle.Verspaetungsgrund.Deutsch
        });
    }

    /// <summary>
    /// Ermittelt, ob eine meldenswerte Verspätung für Ankunft oder Abfahrt vorliegt.
    /// </summary>
    private static int? DetermineReportableDelay(Haltestellen changed, Haltestellen old)
    {
        var isNewDelayReason = old.Verspaetungsgrund is null;

        static bool HasSignificantIncrease(int? newDelay, int? oldDelay) =>
            newDelay.HasValue && ((!oldDelay.HasValue && newDelay.Value > 3) || (oldDelay.HasValue && newDelay.Value > oldDelay.Value + 2));

        if (HasSignificantIncrease(changed.Ankunftszeiten.Verspaetung, old.Ankunftszeiten.Verspaetung)
            || (isNewDelayReason && changed.Ankunftszeiten.Verspaetung.HasValue))
        {
            return changed.Ankunftszeiten.Verspaetung;
        }

        if (HasSignificantIncrease(changed.Abfahrtszeiten.Verspaetung, old.Abfahrtszeiten.Verspaetung)
            || (isNewDelayReason && changed.Abfahrtszeiten.Verspaetung.HasValue))
        {
            return changed.Abfahrtszeiten.Verspaetung;
        }

        return null;
    }


    private void EnqueueMessageTrain(int trainNumber, IRequest<Unit> request)
    {
        foreach (Guid clientId in _registeredTrains.Where(x => x.Value == trainNumber).Select(x => x.Key))
        {
            EnqueueMessageClient(clientId, request);
        }
    }

    private void EnqueueMessageClient(Guid clientId, IRequest<Unit> request)
    {
        _messageQeue[clientId].Enqueue(request);
    }
}