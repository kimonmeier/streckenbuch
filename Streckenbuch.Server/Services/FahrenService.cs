﻿using AutoMapper;
using Blazor.Serialization.Extensions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Streckenbuch.Server.Background;
using Streckenbuch.Server.Data.Repositories;
using Streckenbuch.Server.Models;
using Streckenbuch.Server.States;
using Streckenbuch.Shared.Models;
using Streckenbuch.Shared.Services;
using System.Text;

namespace Streckenbuch.Server.Services;

public class FahrenService : Streckenbuch.Shared.Services.FahrenService.FahrenServiceBase
{
    private readonly FahrenRepository _fahrenRepository;
    private readonly IMapper _mapper;
    private readonly ContinuousConnectionState _continuousConnection;

    public FahrenService(FahrenRepository fahrenRepository, IMapper mapper, ContinuousConnectionState continuousConnection)
    {
        _fahrenRepository = fahrenRepository;
        _mapper = mapper;
        _continuousConnection = continuousConnection;
    }

    public override Task<FahrenResponse> FahrenByTrainNumber(FahrenRequestByTrainNumber request, ServerCallContext context)
    {
        List<FahrenTransferEntry> entries = _fahrenRepository.ListEntriesByLinieTrain(request.LinieTrainId).ToList();

        RemoveDuplicates(entries);
        CorrectIndices(entries);

        FahrenResponse response = new();
        response.Entries.AddRange(_mapper.Map<List<FahrenEntry>>(entries));

        return Task.FromResult(response);
    }

    public override Task<FahrenResponse> FahrenByLinie(FahrenRequestByLinie request, ServerCallContext context)
    {
        List<FahrenTransferEntry> entries = _fahrenRepository.ListEntriesByLinie(request.LinieId).ToList();

        RemoveDuplicates(entries);
        CorrectIndices(entries);

        FahrenResponse response = new();
        response.Entries.AddRange(_mapper.Map<List<FahrenEntry>>(entries));

        return Task.FromResult(response);
    }

    public override Task<FahrenResponse> FahrenByStrecken(FahrenRequestByStrecken request, ServerCallContext context)
    {
        List<FahrenTransferEntry> entries = request.Strecken.SelectMany(GetTrimmedEntries).ToList();

        RemoveDuplicates(entries);
        CorrectIndices(entries);

        FahrenResponse response = new();
        response.Entries.AddRange(_mapper.Map<List<FahrenEntry>>(entries));

        return Task.FromResult(response);
    }

    private void RemoveDuplicates(List<FahrenTransferEntry> entries)
    {
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

            while (currentIndex <= secondIndex)
            {
                entries.RemoveAt(currentIndex);
                secondIndex--;
            }
        }
    }

    private void CorrectIndices(List<FahrenTransferEntry> entries)
    {
        List<Guid> alreadyHandledIds = new();
        var originalIndices = entries.Where(x => x.OverrideIndex.HasValue).Select(x =>
            new
            {
                Index = entries.IndexOf(x), Id = x.SignalZuordnung!.SignalId
            }
        ).ToDictionary(x => x.Id, x => x.Index);

        for (int i = entries.Count - 1; i >= 0; i--)
        {
            var currentEntry = entries[i];

            if (currentEntry.OverrideIndex is null)
            {
                continue;
            }

            if (currentEntry.OverrideIndex.Value <= 0)
            {
                continue;
            }

            if (alreadyHandledIds.Contains(currentEntry.SignalZuordnung!.SignalId))
            {
                continue;
            }

            alreadyHandledIds.Add(currentEntry.SignalZuordnung!.SignalId);

            int currentEntryOverrideIndex = currentEntry.OverrideIndex.Value;
            int newIndex = originalIndices[currentEntry.SignalZuordnung!.SignalId] + currentEntryOverrideIndex;

            entries.Insert(newIndex + 1, currentEntry);
            entries.RemoveAt(i);

            i++;
        }
        
        for (int i = 0; i < entries.Count; i++)
        {
            var currentEntry = entries[i];

            if (currentEntry.OverrideIndex is null)
            {
                continue;
            }

            if (currentEntry.OverrideIndex.Value > 0)
            {
                continue;
            }

            if (alreadyHandledIds.Contains(currentEntry.SignalZuordnung!.SignalId))
            {
                continue;
            }

            alreadyHandledIds.Add(currentEntry.SignalZuordnung!.SignalId);

            int currentEntryOverrideIndex = currentEntry.OverrideIndex.Value;
            int newIndex = originalIndices[currentEntry.SignalZuordnung!.SignalId] + currentEntryOverrideIndex;

            entries.Insert(newIndex, currentEntry);
            entries.RemoveAt(i + 1);

            i--;
        }
    }

    public override Task<CaptureMessageResponse> CaptureRealtimeMessages(CaptureMessage request, ServerCallContext context)
    {
        CaptureMessageResponse response = new();
        response.Messages.AddRange(_continuousConnection.GetMessagesInQueue(request.ClientId));

        return Task.FromResult(response);
    }

    public override Task<Empty> RegisterOnTrain(RegisterOnTrainRequest request, ServerCallContext context)
    {
        _continuousConnection.RegisterTrain(request.ClientId, request.TrainNumber);

        return Task.FromResult(new Empty());
    }

    public override Task<Empty> UnregisterOnTrain(UnregisterOnTrainRequest request, ServerCallContext context)
    {
        _continuousConnection.UnregisterTrain(request.ClientId);

        return Task.FromResult(new Empty());
    }

    public override Task<Empty> DisconnectClient(DisconnectClientRequest request, ServerCallContext context)
    {
        _continuousConnection.DisconnectTrainOperator(request.ClientId);

        return Task.FromResult(new Empty());
    }

    private List<FahrenTransferEntry> GetTrimmedEntries(FahrenRequestStrecke strecke)
    {
        var entries = _fahrenRepository.ListEntriesByStrecke(strecke.StreckenKonfigurationId);

        while (!entries.First(x => x.Betriebspunkt != null).Betriebspunkt!.Id.Equals(strecke.VonBetriebspunktId))
        {
            entries.RemoveAt(0);
        }

        while (!entries.Last(x => x.Betriebspunkt != null).Betriebspunkt!.Id.Equals(strecke.BisBetriebspunktId))
        {
            entries.RemoveAt(entries.Count - 1);
        }

        return entries;
    }
}