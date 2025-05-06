namespace Streckenbuch.Shared.Types;

public partial class DateOnlyProto
{
    public static implicit operator DateOnly?(DateOnlyProto? dateOnlyProto)
    {
        if (dateOnlyProto == null)
        {
            return null;
        }
        return DateOnly.FromDayNumber(dateOnlyProto.DaySinceUnix);
    }

    public static implicit operator DateOnlyProto(DateOnly dateOnly)
    {
        return new DateOnlyProto()
        {
            DaySinceUnix = dateOnly.DayNumber
        };
    }

    public static implicit operator DateOnlyProto(DateTime date)
    {
        return new DateOnlyProto()
        {
            DaySinceUnix = DateOnly.FromDateTime(date).DayNumber
        };
    }
}
