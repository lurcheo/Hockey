using OpenCvSharp;
using ReactiveUI;
using System.Threading;
using System.Threading.Tasks;

namespace Hockey.Client.Main.Model.Abstraction;

internal interface IMainModel : IReactiveObject
{
    bool Paused { get; set; }
    Mat CurrentFrame { get; set; }
    Task ReadVideoFromFile(string fileName, CancellationToken cancellationToken);
}
