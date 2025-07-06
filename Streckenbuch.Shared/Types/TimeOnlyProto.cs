namespace Streckenbuch.Shared.Types;

public partial class TimeOnlyProto
{
    public static implicit operator TimeOnly?(TimeOnlyProto? timeOnlyProto)
    {
        if (timeOnlyProto == null)
        {
            return null;
        }
        return TimeOnly.FromTimeSpan(TimeSpan.FromTicks(timeOnlyProto.TicksSinceDayStart));
    }

    public static implicit operator TimeOnlyProto(TimeOnly dateOnly)
    {
        return new TimeOnlyProto()
        {
            TicksSinceDayStart = dateOnly.Ticks
        };
    }

    public static implicit operator TimeOnlyProto(DateTime date)
    {
        return TimeOnly.FromDateTime(date);
    }
}
