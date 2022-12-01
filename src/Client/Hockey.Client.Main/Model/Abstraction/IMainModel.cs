using Hockey.Client.BusinessLayer.Abstraction;
using Hockey.Client.Main.Model.Events;
using OpenCvSharp;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Hockey.Client.Main.Model.Abstraction;

internal interface IMainModel : IReactiveObject
{
    bool IsPaused { get; set; }
    Mat CurrentFrame { get; set; }
    long FrameNumber { get; set; }
    long FramesCount { get; set; }

    TeamModel HomeTeam { get; }
    TeamModel GuestTeam { get; }
    bool IsUserClick { get; set; }
    ObservableCollection<EventModel> Events { get; }
    IEnumerable<EventTypeModel> EventTypes { get; }

    Task ReadVideoFromFile(string fileName, CancellationToken cancellationToken);
    void SetPosition(long position);
}
