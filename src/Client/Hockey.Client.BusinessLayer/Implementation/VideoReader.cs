using Hockey.Client.BusinessLayer.Abstraction;
using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;

namespace Hockey.Client.BusinessLayer.Implementation;

internal class VideoReader : IVideoReader
{
    private readonly VideoCapture _videoCapture;

    public VideoReader(IFactory<VideoCapture> videoCaptureFactory)
    {
        _videoCapture = videoCaptureFactory.Create();
    }

    public IEnumerator<Mat> GetEnumerator()
    {
        while (true)
        {
            using Mat frame = new();
            if (!_videoCapture.Read(frame))
            {
                break;
            }
            yield return frame;
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
