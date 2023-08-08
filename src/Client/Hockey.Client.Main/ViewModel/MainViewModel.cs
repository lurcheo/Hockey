using Hockey.Client.Main.Events;
using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data.Events;
using Hockey.Client.Shared.Extensions;
using OpenCvSharp.WpfExtensions;
using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

using static Hockey.Client.BusinessLayer.Data.FileExtensions;
using static Hockey.Client.Shared.Extensions.DialogExtensionsMethods;

namespace Hockey.Client.Main.ViewModel;

internal class MainViewModel : ReactiveObject
{
    public IMainModel Model { get; }
    public IEventAggregator EventAggregator { get; }

    [Reactive] public ImageSource Frame { get; set; }
    [Reactive] public CancellationTokenSource LastTokenSource { get; set; }
    private Task lastReadingTask;

    [Reactive] public bool IsLeftMenuOpen { get; set; }
    [Reactive] public bool IsRightMenuOpen { get; set; }

    public ICommand SaveHomeTeamToFileCommand { get; }
    public ICommand ReadHomeTeamToFileCommand { get; }
    public ICommand SaveGuestTeamToFileCommand { get; }
    public ICommand ReadGuestTeamToFileCommand { get; }

    public ICommand ReadVideoFromFileCommand { get; }

    public ICommand ReadProjectFromFileCommand { get; }
    public ICommand SaveProjectToFileCommand { get; }

    public ICommand ReversePauseCommand { get; }

    public ICommand ShowNextFrameCommand { get; }
    public ICommand ShowPreviousFrameCommand { get; }

    public ICommand ShowNext10SecondsCommand { get; }
    public ICommand ShowPrevious10SecondsCommand { get; }

    public ICommand UserClickedCommand { get; }

    public ICommand AddEventCommand { get; }

    public ICommand ReadEventsCommand { get; }
    public ICommand SaveEventsCommand { get; }


    public MainViewModel(IMainModel model, IEventAggregator eventAggregator)
    {
        Model = model;
        EventAggregator = eventAggregator;

        Model.WhenAnyValue(x => x.CurrentFrame)
             .Select(x => x is null || x.IsDisposed ? default : BitmapSourceConverter.ToBitmapSource(x))
             .Subscribe(x => Frame = x)
             .Cache();

        EventAggregator.GetEvent<EventAdded>()
                       .ToObservable()
                       .Subscribe(_ => IsRightMenuOpen = true)
                       .Cache();

        var videoOpenChanged = Model.WhenAnyValue(x => x.IsVideoOpen);

        ShowNextFrameCommand = ReactiveCommand.Create
        (
            () => SetNextFramePosition(1),
            videoOpenChanged
        );

        AddEventCommand = ReactiveCommand.Create<EventFactory>(x => Model.Events.Add(Model.CreateEvent(x)));

        ShowPreviousFrameCommand = ReactiveCommand.Create
        (
            () => SetNextFramePosition(-1),
            videoOpenChanged
        );

        ShowNext10SecondsCommand = ReactiveCommand.Create
        (
            () => SetNextFramePosition(TimeSpan.FromSeconds(10)),
            videoOpenChanged
        );

        ShowPrevious10SecondsCommand = ReactiveCommand.Create
        (
            () => SetNextFramePosition(-TimeSpan.FromSeconds(10)),
            videoOpenChanged
        );

        SaveEventsCommand = ReactiveCommand.CreateFromTask
        (
            (async () =>
            {
                if (!TrySaveFileDialog($"События.{EVENTS_FILE_EXTENSION}",
                                       "Файл событий".ConcatExtensions(EVENTS_FILE_EXTENSION),
                                       out var fileName))
                {
                    return;
                }

                await Model.SaveEventsToFile(fileName);
            })
        );

        ReadEventsCommand = ReactiveCommand.CreateFromTask
        (
            (async () =>
            {
                if (!TryOpenFileDialog("Файл событий".ConcatExtensions(EVENTS_FILE_EXTENSION),
                                       out var fileName))
                {
                    return;
                }

                await Model.ReadEventsFromFile(fileName);
            })
        );

        SaveHomeTeamToFileCommand = ReactiveCommand.CreateFromTask
        (
            (async () =>
            {
                if (!TrySaveFileDialog($"Команда.{TEAM_FILE_EXTENSION}",
                                       "Файл команды хоккейного матча".ConcatExtensions(TEAM_FILE_EXTENSION),
                                       out var fileName))
                {
                    return;
                }

                await Model.SaveHomeTeamToFile(fileName);
            })
        );

        ReadHomeTeamToFileCommand = ReactiveCommand.CreateFromTask
        (
            (async () =>
            {
                if (!TryOpenFileDialog("Файл команды хоккейного матча".ConcatExtensions(TEAM_FILE_EXTENSION),
                                       out var fileName))
                {
                    return;
                }

                await Model.ReadHomeTeamToFile(fileName);
            })
        );

        SaveGuestTeamToFileCommand = ReactiveCommand.CreateFromTask
        (
            (async () =>
            {
                if (!TrySaveFileDialog($"Команда.TEAM_FILE_EXTENSION",
                                       "Файл команды хоккейного матча".ConcatExtensions(TEAM_FILE_EXTENSION),
                                       out var fileName))
                {
                    return;
                }

                await Model.SaveGuestTeamToFile(fileName);
            })
        );

        ReadGuestTeamToFileCommand = ReactiveCommand.CreateFromTask
        (
            async () =>
            {
                if (!TryOpenFileDialog("Файл команды хоккейного матча".ConcatExtensions(TEAM_FILE_EXTENSION),
                                       out var fileName))
                {
                    return;
                }

                await Model.ReadGuestTeamToFile(fileName);
            }
        );

        ReadProjectFromFileCommand = ReactiveCommand.Create
        (
            async () =>
            {
                if (!TryOpenFileDialog("Проект аналитики хоккейного матча".ConcatExtensions(PROJECT_FILE_EXTENSION), out var fileName))
                {
                    return;
                }

                LastTokenSource?.Cancel();
                LastTokenSource = new();

                if (lastReadingTask is not null)
                {
                    await lastReadingTask;
                }

                lastReadingTask = Model.ReadProjectFromFile(fileName, LastTokenSource.Token);
                await lastReadingTask;
            }
        );

        SaveProjectToFileCommand = ReactiveCommand.CreateFromTask
        (
            async () =>
            {
                if (!TrySaveFileDialog($"Аналитика матча.{PROJECT_FILE_EXTENSION}",
                                       "Проект аналитики хоккейного матча".ConcatExtensions(PROJECT_FILE_EXTENSION),
                                       out var fileName))
                {
                    return;
                }

                await Model.SaveProjectToFile(fileName);
            }
        );

        ReadVideoFromFileCommand = ReactiveCommand.Create
        (
            async () =>
            {
                string videoExtensions = "Видео".ConcatExtensions("AVI", "MP4", "MPEG", "MOV");
                string imageExtensions = "Изображения".ConcatExtensions("BMP", "JPEG", "JPG", "PNG");

                string filter = string.Join('|', videoExtensions, imageExtensions);

                if (!TryOpenFileDialog(filter, out string fileName))
                {
                    return;
                }

                LastTokenSource?.Cancel();
                LastTokenSource = new();

                if (lastReadingTask is not null)
                {
                    await lastReadingTask;
                }

                lastReadingTask = Model.ReadVideoFromFile(fileName, LastTokenSource.Token);
                await lastReadingTask;
            }
        );

        ReversePauseCommand = ReactiveCommand.Create
        (
            () => Model.IsPaused = !Model.IsPaused,
            videoOpenChanged
        );

        UserClickedCommand = ReactiveCommand.Create<bool>
        (
            x => Model.IsUserClick = x,
            videoOpenChanged
        );
    }

    private void SetNextFramePosition(long deltaFrameNumber)
    {
        Model.SetUserPosition(Model.FrameNumber + deltaFrameNumber);
    }

    private void SetNextFramePosition(TimeSpan deltaFrame)
    {
        SetNextFramePosition((long)(deltaFrame.TotalMilliseconds / Model.MillisecondsPerFrame));
    }
}
