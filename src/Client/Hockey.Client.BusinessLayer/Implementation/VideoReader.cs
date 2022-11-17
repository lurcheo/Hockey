using Hockey.Client.BusinessLayer.Abstraction;
using Hockey.Client.BusinessLayer.Data;
using OpenCvSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Hockey.Client.BusinessLayer.Implementation;

internal class VideoReader : IVideoReader
{
    private readonly VideoCapture _videoCapture;

    public int MillisecondsPerFrame => (int)(1_000 / Fps);

    public double Fps { get; }
    public long FramesCount { get; }
    public TimeSpan Duration => TimeSpan.FromMilliseconds(MillisecondsPerFrame * FramesCount);

    public VideoReader(IFactory<VideoCapture> videoCaptureFactory)
    {
        _videoCapture = videoCaptureFactory.Create();
        Fps = _videoCapture.Fps;
        FramesCount = (long)_videoCapture.Get(VideoCaptureProperties.FrameCount);
    }

    public void SetPosition(long position)
    {
        _videoCapture.Set(VideoCaptureProperties.PosFrames, position);
    }

    public IEnumerator<FrameInfo> GetEnumerator()
    {
        var st = Stopwatch.StartNew();
        while (true)
        {
            st.Restart();
            using Mat frame = new();
            if (!_videoCapture.Read(frame))
            {
                break;
            }

            yield return new FrameInfo(frame, (long)_videoCapture.Get(VideoCaptureProperties.PosFrames));

            long delay = MillisecondsPerFrame - st.ElapsedMilliseconds;

            if (delay > 0)
            {
                Thread.Sleep((int)delay);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Dispose()
    {
        _videoCapture.Dispose();
    }

}
