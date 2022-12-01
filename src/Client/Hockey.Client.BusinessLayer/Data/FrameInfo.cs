using OpenCvSharp;
using System;

namespace Hockey.Client.BusinessLayer.Data;

public record FrameInfo(Mat Frame, long FrameNumber);
