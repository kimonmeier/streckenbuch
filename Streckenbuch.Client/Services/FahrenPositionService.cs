using Microsoft.JSInterop;
using Streckenbuch.Client.Extensions;
using Streckenbuch.Client.Models.Fahren;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Client.Services;

public class FahrenPositionService
{
    private List<IBaseEntry> currentEntries = default!;
    private List<GeolocationPosition> lastPositions = default!;
    private Action<Action> beforeUpdateAction = default!;


    public List<IBaseEntry> Initialize(List<IBaseEntry> fahrplanEntries, Action<Action> beforeUpdateAction)
    {
        this.beforeUpdateAction = beforeUpdateAction;
        // Copy to get new Entries
        currentEntries = fahrplanEntries.ToList();
        lastPositions = new List<GeolocationPosition>();

        return currentEntries;
    }

    public void SkipPosition()
    {
        beforeUpdateAction(() =>
        {
            currentEntries.RemoveAt(currentEntries.Count - 1);
        }); ;
    }

    public bool UpdatePosition(GeolocationPosition newPosition)
    {
        if (currentEntries.Count == 0)
        {
            return false;
        }
        
        if (lastPositions.Count == 0)
        {
            lastPositions.Add(newPosition);
            var closestBetriebspunkt = currentEntries.Where(x => x.Type == EntryType.Betriebspunkt).Select(x => new { Entry = x, Difference = x.Location.GetDistanzInMeters(newPosition) }).OrderBy(x => x.Difference).First().Entry;

            if (currentEntries.Last() == closestBetriebspunkt)
            {
                return false;
            }
            var currentEntry = currentEntries.Last();
            while (currentEntry != closestBetriebspunkt)
            {
                currentEntries.Remove(currentEntry);
                currentEntry = currentEntries.Last();
            }
            return true;
        }


        if (lastPositions.Last().GetDistanzInMeters(newPosition) <= newPosition.Coords.Accuracy * 1.50)
        {
            return false;
        }

        if (!HasPassedLastEntry(newPosition, currentEntries.Last(), lastPositions))
        {
            lastPositions.Add(newPosition);
            return false;
        }

        beforeUpdateAction(() =>
        {
            currentEntries.RemoveAt(currentEntries.Count - 1);
        });
        return true;
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
        bool isMovingAway = distances.Any(d => d > 0); 

        if (isApproaching && isMovingAway)
        {
            return true;
        }

        return false;
    }
}
