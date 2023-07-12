using Hockey.Client.BusinessLayer.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hockey.Client.BusinessLayer.Abstraction;
public interface IVideoService
{
    IVideoReader ReadVideoFromFile(string fileName);
    Task WriteMomentToVideoFile(string inputFileName, string outputFileName, IEnumerable<GameMoment> moments, Action<double> videoSavingProgressCallback = null);
}
