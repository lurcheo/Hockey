using Hockey.Client.BusinessLayer.Data;
using System;
using System.Collections.Generic;

namespace Hockey.Client.BusinessLayer.Abstraction;

public interface IVideoReader : IDisposable, IEnumerable<FrameInfo>
{
    public int MillisecondsPerFrame { get; }
    public double Fps { get; }
    long FramesCount { get; }
    TimeSpan Duration { get; }

    void SetPosition(long position);
}
