using Microsoft.JSInterop;

namespace Streckenbuch.Client.Extensions;

public static class GeoLocationExtensions
{
    public static double GetDistanzInMeters(this GeolocationPosition geolocationPosition, GeolocationPosition geolocationPosition2)
    {
        if (geolocationPosition == null)
        {
            return double.MaxValue;
        }

        if (geolocationPosition2 == null)
        {
            return double.MaxValue;
        }

        return GetDistanceBetweenTwoPointsInMeters(geolocationPosition.Coords.Longitude, geolocationPosition.Coords.Latitude, geolocationPosition2.Coords.Latitude, geolocationPosition2.Coords.Longitude);
    }

    public static double GetDistanzInMeters(this GeolocationPosition geolocationPosition, NetTopologySuite.Geometries.Coordinate coordinate)
    {
        if (coordinate == null)
        {
            return double.MaxValue;
        }

        if (geolocationPosition == null)
        {
            return double.MaxValue;
        }

        return GetDistanceBetweenTwoPointsInMeters(geolocationPosition.Coords.Longitude, geolocationPosition.Coords.Latitude, coordinate.X, coordinate.Y);
    }

    public static double GetDistanzInMeters(this NetTopologySuite.Geometries.Coordinate coordinate, GeolocationPosition geolocationPosition)
    {
        return geolocationPosition.GetDistanzInMeters(coordinate);
    }

    private static double GetDistanceBetweenTwoPointsInMeters(double latitude1, double longitude1, double latitude2, double longitude2)
    {
        double theta = longitude1 - longitude2;
        double distance = Math.Sin(ConvertDecimalDegreesToRadians(latitude1)) * Math.Sin(ConvertDecimalDegreesToRadians(latitude2)) +
                          Math.Cos(ConvertDecimalDegreesToRadians(latitude1)) * Math.Cos(ConvertDecimalDegreesToRadians(latitude2)) * Math.Cos(ConvertDecimalDegreesToRadians(theta));

        distance = Math.Acos(distance);
        distance = ConvertRadiansToDecimalDegrees(distance);
        distance = distance * 60 * 1.1515; // Convert to miles
        return distance * 1.609344 * 1000; // Convert to kilometer and then to meter
    }

    private static double ConvertDecimalDegreesToRadians(double degree)
    {
        return (degree * Math.PI / 180.0);
    }

    private static double ConvertRadiansToDecimalDegrees(double radian)
    {
        return (radian / Math.PI * 180.0);
    }
}
