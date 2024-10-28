using NetTopologySuite.Geometries;

namespace Streckenbuch.Shared.Types;

public partial class LocationProto
{
    public static implicit operator Coordinate(LocationProto proto)
    {
        return new Coordinate(proto.Longitude, proto.Latitude);
    }

    public static implicit operator LocationProto(Coordinate coordinate)
    {
        return new LocationProto()
        {
            Latitude = coordinate.Y,
            Longitude = coordinate.X,
        };
    }
}
