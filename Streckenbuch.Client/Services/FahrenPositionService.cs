using Microsoft.JSInterop;
using Streckenbuch.Client.Extensions;
using Streckenbuch.Client.Models.Fahren;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Client.Services;

public class FahrenPositionService
{
    private List<IBaseEntry> allEntries = default!;
    private List<IBaseEntry> currenEntries = default!;
    private List<GeolocationPosition> lastPositions = default!;


    public List<IBaseEntry> Initialize(List<IBaseEntry> fahrplanEntries)
    {
        allEntries = fahrplanEntries;
        // Copy to get new Entries
        currenEntries = fahrplanEntries.ToList();
        lastPositions = new List<GeolocationPosition>();

        return currenEntries;
    }

    public bool UpdatePosition(GeolocationPosition newPosition, Action beforeUpdateAction)
    {
        if (lastPositions.Count == 0)
        {
            lastPositions.Add(newPosition);
            var closestBetriebspunkt = currenEntries.Where(x => x.Type == EntryType.Betriebspunkt).Select(x => new { Entry = x, Difference = x.Location.GetDistanzInMeters(newPosition) }).OrderBy(x => x.Difference).First().Entry;

            if (currenEntries.Last() == closestBetriebspunkt)
            {
                return false;
            }
            var currentEntry = currenEntries.Last();
            while (currentEntry != closestBetriebspunkt)
            {
                currenEntries.Remove(currentEntry);
            }
            return true;
        }

        if (lastPositions.Last().GetDistanzInMeters(newPosition) <= 75)
        {
            return false;
        }

        if (!HasPassedLastEntry(newPosition, currenEntries.Last(), lastPositions))
        {
            lastPositions.Add(newPosition);
            return false;
        }

        beforeUpdateAction();

        currenEntries.RemoveAt(currenEntries.Count - 1);
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
