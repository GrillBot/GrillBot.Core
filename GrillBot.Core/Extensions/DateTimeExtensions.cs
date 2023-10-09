namespace GrillBot.Core.Extensions;

public static class DateTimeExtensions
{
    public static DateTime WithKind(this DateTime date, DateTimeKind kind) 
        => new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond, date.Microsecond, kind);
}
