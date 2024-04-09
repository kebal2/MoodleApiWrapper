using System;

namespace MoodleApiWrapper;

public static class TimeHelper
{
    public static int ToUnixTimestamp(this DateTime dateTime) =>
        Convert.ToInt32((TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds);
}
