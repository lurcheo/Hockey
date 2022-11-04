using OpenCvSharp;
using System;
using System.Collections.Generic;

namespace Hockey.Client.BusinessLayer.Abstraction;

public interface IVideoReader : IDisposable, IEnumerable<Mat>
{
}
