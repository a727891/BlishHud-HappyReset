using Microsoft.Xna.Framework;
using System;

namespace HappyReset.Features.Shared.Services;

public class ResetsWatcherService : IDisposable
{

    public event EventHandler<DateTime>? DailyReset;

    public DateTime NextDailyReset { get; private set; }
    public DateTime LastDailyReset { get; private set; }

    public ResetsWatcherService()
    {
        CalcNextDailyReset();
    }

    public void CalcNextDailyReset()
    {
        var now = DateTime.UtcNow;

        NextDailyReset = now.AddDays(1).Date;
        LastDailyReset = NextDailyReset.AddDays(-1);

        //NextDailyReset = now.AddSeconds(45);
        //LastDailyReset = NextDailyReset.AddSeconds(-45);
    }

  
    public static DateTime NextDayOfWeek(DayOfWeek weekday, int hour, int minute)
    {
        var today = DateTime.UtcNow;

        if (today.Hour < hour && today.DayOfWeek == weekday)
        {
            return today.Date.AddHours(hour).AddMinutes(minute);
        }
        else
        {
            var nextReset = today.AddDays(1);

            while (nextReset.DayOfWeek != weekday)
            {
                nextReset = nextReset.AddDays(1);
            }

            return nextReset.Date.AddHours(hour).AddMinutes(minute);
        }
    }

    public void Update(GameTime gametime)
    {
        var now = DateTime.UtcNow;
        if (now >= NextDailyReset)
        {
            CalcNextDailyReset();
            DailyReset?.Invoke(this, NextDailyReset);
        }
    }

    public void Dispose()
    {

    }
}