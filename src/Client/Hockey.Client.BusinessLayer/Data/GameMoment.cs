using System;

namespace Hockey.Client.BusinessLayer.Data;

public class GameMoment
{

    public TimeSpan Start { get; }
    public TimeSpan End { get; }

    public GameMoment(TimeSpan start, TimeSpan end)
    {
        Start = start;
        End = end;
    }
}
