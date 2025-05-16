using Microsoft.JSInterop;
using Streckenbuch.Client.Extensions;
using Streckenbuch.Client.Models.Fahren;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Client.Services;

public class FahrenPositionService
{
    private const double AccuracyFactor = 1.50;

    private List<IBaseEntry> _currentEntries = default!;
    private List<GeolocationPosition> _lastPositions = default!;
    private Action<Action> _beforeUpdateAction = default!;
    private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);


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
            _semaphoreSlim.Release();
        });
    }

    public async Task UpdatePosition(GeolocationPosition newPosition)
    {
        try
        {
            await _semaphoreSlim.WaitAsync();

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
        }
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

        return true;
    }

    private IBaseEntry? FindClosestBetriebspunkt(GeolocationPosition position)
    {
        return _currentEntries
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
        bool isApproaching = distances.Any(d => d < 0);
        bool isMovingAway = distances.Any(d => d > 0);

        if (isApproaching && isMovingAway)
        {
            return true;
        }

        return false;
    }
}