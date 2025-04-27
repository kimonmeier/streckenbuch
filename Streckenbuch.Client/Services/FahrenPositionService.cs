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
        // Calculate distance from the new position to the last entry
        var distanceToEntryFromNewPosition = lastEntry.Location.GetDistanzInMeters(newPosition);

        // If we have less than two positions, we can't determine the direction yet
        if (lastPositions.Count < 3)
        {
            return false;
        }

        var oldPositionThree = lastPositions[lastPositions.Count - 3];
        var oldPositionTwo = lastPositions[lastPositions.Count - 2];
        var oldPositionOne = lastPositions[lastPositions.Count - 1];

        // Calculate the distance from previous positions to the last entry
        var distanceFromOldPositionOne = lastEntry.Location.GetDistanzInMeters(oldPositionOne);
        var distanceFromOldPositionTwo = lastEntry.Location.GetDistanzInMeters(oldPositionTwo);
        var distanceFromOldPositionThree = lastEntry.Location.GetDistanzInMeters(oldPositionThree);

        // Check if the distance to the last entry is increasing or decreasing
        if (distanceFromOldPositionOne < distanceFromOldPositionTwo &&
            distanceFromOldPositionTwo < distanceFromOldPositionThree)
        {
            // If the distances are decreasing, we are moving towards the entry
            return false;
        }

        // If the distance is increasing, we have passed the last entry
        return true;
    }
}
