using NetTopologySuite.Geometries;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Components.Models.Fahren;

public class SpacerEntry : IBaseEntry
{
    public EntryType Type => EntryType.Spacer;

    public Coordinate Location => null!;
}