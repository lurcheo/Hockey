using Hockey.Client.BusinessLayer.Abstraction;
using OpenCvSharp;

namespace Hockey.Client.BusinessLayer.Implementation;

internal class VideoService : IVideoService
{
    public IVideoReader ReadVideoFromFile(string fileName)
    {
        return new VideoReader
        (
            new DefaultFactory<VideoCapture>(() => new VideoCapture(fileName))
        );
    }
}
