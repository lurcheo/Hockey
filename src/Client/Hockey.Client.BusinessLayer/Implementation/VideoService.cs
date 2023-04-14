using Hockey.Client.BusinessLayer.Abstraction;
using Hockey.Client.BusinessLayer.Data;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    public Task WriteMomentToVideoFile(string inputFileName, string outputFileName, IEnumerable<GameMoment> moments, Action<double> videoSavingProgressCallback = null)
    {
        return Task.Run(() =>
        {
            using VideoCapture videoCapture = new(inputFileName);

            double fps = videoCapture.Get(VideoCaptureProperties.Fps);
            double width = videoCapture.Get(VideoCaptureProperties.FrameWidth);
            double height = videoCapture.Get(VideoCaptureProperties.FrameHeight);

            using VideoWriter videoWriter = new(outputFileName, FourCC.MPG4, fps, new Size(width, height));

            double totalFrameNums = moments.Select(x => (x.End - x.Start).TotalSeconds * fps).Sum();

            int writedFrames = 0;
            foreach (var moment in moments)
            {
                int startFrame = (int)(moment.Start.TotalSeconds * fps);
                int endFrame = (int)(moment.End.TotalSeconds * fps);

                videoCapture.Set(VideoCaptureProperties.PosFrames, startFrame);
                for (int i = startFrame; i <= endFrame; i++)
                {
                    using Mat mat = new();
                    bool res = videoCapture.Read(mat);

                    if (!res)
                    {
                        break;
                    }

                    videoWriter.Write(mat);

                    writedFrames++;
                    videoSavingProgressCallback?.Invoke(writedFrames / totalFrameNums);
                }
            }
        });
    }
}
