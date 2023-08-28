using Microsoft.Xna.Framework;
using System;

namespace HappyReset.Services;

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

#if DEBUG
        NextDailyReset = now.AddSeconds(20);
        LastDailyReset = NextDailyReset.AddSeconds(-30);
#else
        NextDailyReset = now.AddDays(1).Date;
        LastDailyReset = NextDailyReset.AddDays(-1);
#endif

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