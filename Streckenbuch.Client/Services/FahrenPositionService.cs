using MediatR;
using Microsoft.JSInterop;
using Streckenbuch.Client.Events.ApproachingStop;
using Streckenbuch.Client.Events.PositionRecieved;
using Streckenbuch.Client.Extensions;
using Streckenbuch.Client.Models.Fahren;
using Streckenbuch.Client.Models.Fahren.Betriebspunkt;
using Streckenbuch.Shared.Models;
using System.Runtime.InteropServices;

namespace Streckenbuch.Client.Services;

public sealed class FahrenPositionService
{
    public event EventHandler? DataChanged;

    private const double AccuracyFactor = 2;

    private List<IBaseEntry> _currentEntries = default!;
    private List<GeolocationPosition> _lastPositions = default!;
    private Action<Action> _beforeUpdateAction = default!;
    private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
    private readonly List<Guid> _stops = new List<Guid>();
    private readonly List<Guid> _stopsAnnounced = new List<Guid>();
    private readonly ISender _sender;

    public FahrenPositionService(ISender sender)
    {
        _sender = sender;
    }


    public List<IBaseEntry> Initialize(List<IBaseEntry> fahrplanEntries, Action<Action> beforeUpdateAction)
    {
        this._beforeUpdateAction = beforeUpdateAction;
        // Copy to get new Entries
        _currentEntries = fahrplanEntries.ToList();
        _lastPositions = new List<GeolocationPosition>();

        return _currentEntries;
    }

    public void SkipPosition()
    {
        _beforeUpdateAction(() =>
        {
            _currentEntries.RemoveAt(_currentEntries.Count - 1);
        });
    }

    public async Task UpdatePosition(GeolocationPosition newPosition)
    {
        try
        {
            await _semaphoreSlim.WaitAsync();
            
            if (newPosition.Coords.Accuracy <= 100)
            {
                await _sender.Send(new PositionRecievedEvent()
                {
                    Position = newPosition
                });
            }
            
            if (!HasValidEntries())
            {
                _semaphoreSlim.Release();

                return;
            }

            if (_lastPositions.Count == 0)
            {
                HandleFirstPosition(newPosition);

                _semaphoreSlim.Release();

                return;
            }

            if (IsWithinAccuracyThreshold(newPosition))
            {
                _semaphoreSlim.Release();

                return;
            }
            
            CheckForStationAnnouncement(newPosition);

            if (!HasPassedLastEntry(newPosition, _currentEntries.Last(), _lastPositions))
            {
                _lastPositions.Add(newPosition);

                _semaphoreSlim.Release();

                return;
            }

            RemoveLastEntry();
        }
        catch (Exception _)
        {
            _semaphoreSlim.Release();

            throw;
        }
    }

    public void SetStops(List<Guid> betriebspunktId)
    {
        ResetStopsForCurrentEntries();
        UpdateStopsWithCurrentEntries(betriebspunktId);

        OnDataChanged();
    }


    public void AddSpecialStop(Guid betriebspunktId)
    {
        IBetriebspunktEntry? entry = (IBetriebspunktEntry?)_currentEntries.SingleOrDefault(x => x is IBetriebspunktEntry bEntry && bEntry.Id == betriebspunktId);
        if (entry is null)
        {
            return;
        }

        switch (entry)
        {
            case BahnhofEntry bahnhofEntry:
                bahnhofEntry.StopSpecial = true;

                break;
            case HaltestelleEntry haltestelleEntry:
                haltestelleEntry.StopSpecial = true;

                break;
        }
        
        OnDataChanged();
    }

    private void CheckForStationAnnouncement(GeolocationPosition currentPosition)
    {
        IBetriebspunktEntry? nextStop = (IBetriebspunktEntry?) _currentEntries.Where(x => x is IBetriebspunktEntry bEntry && _stops.Contains(bEntry.Id)).MinBy(x => x.Location.GetDistanzInMeters(currentPosition));

        if (nextStop is null)
        {
            return;
        }
        
        if (nextStop.Location.GetDistanzInMeters(currentPosition) > 1000)
        {
            return;
        }
        
        if (_stopsAnnounced.Contains(nextStop.Id))
        {
            return;
        }

        _stopsAnnounced.Add(nextStop.Id);
        _sender.Send(new ApproachingStopEvent()
        {
            BetriebspunktId = nextStop.Id,
        }).ConfigureAwait(false);
    }

    private void UpdateStopsWithCurrentEntries(List<Guid> betriebspunktId)
    {
        _stops.AddRange(betriebspunktId);

        // Remove all stops that are already passed
        _stops.RemoveAll(stopId => !_currentEntries.Where(x => x is IBetriebspunktEntry).Any(entry => (entry as IBetriebspunktEntry)!.Id.Equals(stopId)));

        foreach (IBaseEntry entry in _currentEntries.Where(x => x is IBetriebspunktEntry bEntry && _stops.Contains(bEntry.Id)))
        {
            switch (entry)
            {
                case BahnhofEntry bahnhofEntry:
                    bahnhofEntry.Stop = true;

                    break;
                case HaltestelleEntry haltestelleEntry:
                    haltestelleEntry.Stop = true;

                    break;
            }

        }
    }

    private void ResetStopsForCurrentEntries()
    {
        foreach (IBaseEntry entry in _currentEntries.Where(x => x is IBetriebspunktEntry bEntry))
        {
            switch (entry)
            {
                case BahnhofEntry bahnhofEntry:
                    bahnhofEntry.Stop = false;

                    break;
                case HaltestelleEntry haltestelleEntry:
                    haltestelleEntry.Stop = false;

                    break;
            }

        }

        _stops.Clear();
        _stopsAnnounced.Clear();
    }

    private bool HasValidEntries()
    {
        return _currentEntries.Count > 0;
    }

    private bool HandleFirstPosition(GeolocationPosition newPosition)
    {
        _lastPositions.Add(newPosition);
        var closestBetriebspunkt = FindClosestBetriebspunkt(newPosition);

        if (closestBetriebspunkt is null)
        {
            throw new Exception("No Betriebspunkt found");
        }

        if (_currentEntries.Last() == closestBetriebspunkt)
        {
            return false;
        }

        RemoveEntriesUntilBetriebspunkt(closestBetriebspunkt);

        OnDataChanged();
        
        return true;
    }

    private IBetriebspunktEntry? FindClosestBetriebspunkt(GeolocationPosition position)
    {
        return (IBetriebspunktEntry?) _currentEntries
            .Where(x => x.Type == EntryType.Betriebspunkt)
            .MinBy(x => x.Location.GetDistanzInMeters(position));
    }

    private void RemoveEntriesUntilBetriebspunkt(IBaseEntry targetBetriebspunkt)
    {
        while (_currentEntries.Last() != targetBetriebspunkt)
        {
            _currentEntries.Remove(_currentEntries.Last());
        }
    }

    private bool IsWithinAccuracyThreshold(GeolocationPosition newPosition)
    {
        return _lastPositions.Last().GetDistanzInMeters(newPosition) <= newPosition.Coords.Accuracy * AccuracyFactor;
    }

    private void RemoveLastEntry()
    {
        _beforeUpdateAction(() =>
        {
            _currentEntries.RemoveAt(_currentEntries.Count - 1);
            _semaphoreSlim.Release();
        });
    }

    private static bool HasPassedLastEntry(GeolocationPosition newPosition, IBaseEntry lastEntry, List<GeolocationPosition> lastPositions)
    {
        if (lastPositions.Count < 3)
        {
            return false;
        }

        int rangeSize = Math.Min(lastPositions.Count, 50); // You can adjust this size for flexibility
        var distances = new List<double>();

        for (int i = 0; i < rangeSize - 1; i++)
        {
            var distance = lastEntry.Location.GetDistanzInMeters(lastPositions[lastPositions.Count - rangeSize + i]);
            var nextDistance = lastEntry.Location.GetDistanzInMeters(lastPositions[lastPositions.Count - rangeSize + i + 1]);
            distances.Add(nextDistance - distance);
        }

        // Check if any distances are increasing and decreasing. This way we can be sure that it has passed the entry
        bool isApproaching = distances.Count(d => d < 0) >= 2;
        bool isMovingAway = distances.Count(d => d > 0) >= 2;

        if (isApproaching && isMovingAway)
        {
            return true;
        }

        return false;
    }

    private void OnDataChanged()
    {
        DataChanged?.Invoke(this, EventArgs.Empty);
    }
}