namespace CommonVoice.API.Domain.ValueObjects;

public record GeoPoint
{
    public double Latitude  { get; }
    public double Longitude { get; }

    public GeoPoint(double lat, double lng)
    {
        if (lat is < -90 or > 90)    throw new ArgumentOutOfRangeException(nameof(lat));
        if (lng is < -180 or > 180)  throw new ArgumentOutOfRangeException(nameof(lng));
        Latitude  = lat;
        Longitude = lng;
    }

    // Haversine — boundary check fallback for in-process validation
    public double DistanceMetresTo(GeoPoint other)
    {
        const double R = 6_371_000;
        var dLat = ToRad(other.Latitude  - Latitude);
        var dLng = ToRad(other.Longitude - Longitude);
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
              + Math.Cos(ToRad(Latitude)) * Math.Cos(ToRad(other.Latitude))
              * Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
        return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    }

    private static double ToRad(double deg) => deg * Math.PI / 180;
}
