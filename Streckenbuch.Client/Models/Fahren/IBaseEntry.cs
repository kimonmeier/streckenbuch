using NetTopologySuite.Geometries;
using Streckenbuch.Shared.Models;

namespace Streckenbuch.Client.Models.Fahren;

public interface IBaseEntry
{
    public EntryType Type { get; }

    public Coordinate Location { get; }
}