using Hockey.Client.BusinessLayer.Abstraction;
using Hockey.Client.BusinessLayer.Data;
using OpenCvSharp;
using System;

namespace Hockey.Client.BusinessLayer.Implementation;

internal class VideoReader : IVideoReader
{
    private readonly VideoCapture _videoCapture;

    public int MillisecondsPerFrame => (int)(1_000 / Fps);

    public double Fps { get; }
    public long FramesCount { get; }
    public TimeSpan Duration => TimeSpan.FromMilliseconds(MillisecondsPerFrame * FramesCount);

    private Mat _lastFrame;

    public VideoReader(IFactory<VideoCapture> videoCaptureFactory)
    {
        _videoCapture = videoCaptureFactory.Create();
        Fps = _videoCapture.Fps;
        FramesCount = (long)_videoCapture.Get(VideoCaptureProperties.FrameCount);
    }

    public void SetPosition(long position)
    {
        _videoCapture.Set(VideoCaptureProperties.PosFrames, position - 1);
    }

    public FrameInfo GetFrame()
    {
        _lastFrame?.Dispose();
        _lastFrame = new();

        var result = _videoCapture.Read(_lastFrame);

        if (result)
        {
            return new FrameInfo(_lastFrame, (long)_videoCapture.Get(VideoCaptureProperties.PosFrames));
        }
        else
        {
            return default;
        }
    }

    public void Dispose()
    {
        _videoCapture.Dispose();
    }

}
