using NetTopologySuite.Geometries;

namespace Streckenbuch.Shared.Types;

public partial class LocationProto
{
    public static implicit operator Coordinate(LocationProto proto)
    {
        return new Coordinate(proto.Latitude, proto.Longitude);
    }

    public static implicit operator LocationProto(Coordinate coordinate)
    {
        return new LocationProto()
        {
            Latitude = coordinate.X,
            Longitude = coordinate.Y,
        };
    }
}
