using NetTopologySuite.Geometries;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Client.Models.Fahren;

public class SpacerEntry : IBaseEntry
{
    public EntryType Type => EntryType.Spacer;

    public Coordinate Location => null!;
}