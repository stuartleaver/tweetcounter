using System;

namespace TweetCounter.Api.Extensions;

public static class DateExtensions
{
    public static DateTime StartOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
    }
    
    public static DateTime EndOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
    }
}