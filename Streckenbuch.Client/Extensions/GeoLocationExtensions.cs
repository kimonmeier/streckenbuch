using Microsoft.JSInterop;

namespace Streckenbuch.Client.Extensions;

public static class GeoLocationExtensions
{
    public static double GetDistanzInMeters(this GeolocationPosition? geolocationPosition, GeolocationPosition? geolocationPosition2)
    {
        if (geolocationPosition is null)
        {
            return double.MaxValue;
        }

        if (geolocationPosition2 is null)
        {
            return double.MaxValue;
        }

        return GetDistanceBetweenTwoPointsInMeters(geolocationPosition.Coords.Latitude, geolocationPosition.Coords.Longitude, geolocationPosition2.Coords.Latitude, geolocationPosition2.Coords.Longitude);
    }

    public static double GetDistanzInMeters(this GeolocationPosition? geolocationPosition, NetTopologySuite.Geometries.Coordinate? coordinate)
    {
        if (coordinate is null)
        {
            return double.MaxValue;
        }

        if (geolocationPosition is null)
        {
            return double.MaxValue;
        }

        return GetDistanceBetweenTwoPointsInMeters(geolocationPosition.Coords.Latitude, geolocationPosition.Coords.Longitude, coordinate.X, coordinate.Y);
    }

    public static double GetDistanzInMeters(this NetTopologySuite.Geometries.Coordinate coordinate, GeolocationPosition geolocationPosition)
    {
        return geolocationPosition.GetDistanzInMeters(coordinate);
    }

    private static double GetDistanceBetweenTwoPointsInMeters(double lat1, double lon1, double lat2, double lon2)
    {
        // Convert latitude and longitude to radians
        double rlat1 = Math.PI * lat1 / 180;
        double rlon1 = Math.PI * lon1 / 180;
        double rlat2 = Math.PI * lat2 / 180;
        double rlon2 = Math.PI * lon2 / 180;

        // Calculate the distance using the Haversine formula
        double dlon = rlon2 - rlon1;
        double dlat = rlat2 - rlat1;
        double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Pow(Math.Sin(dlon / 2), 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = 6371 * c; // Earth's radius in kilometers
        distance *= 1000; // Convert kilometers to meters

        return distance;
    }
}
