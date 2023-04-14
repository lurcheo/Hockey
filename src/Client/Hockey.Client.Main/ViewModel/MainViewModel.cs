using Hockey.Client.Main.Events;
using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Shared.Extensions;
using OpenCvSharp.WpfExtensions;
using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;
using System.Threading;
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

    public ICommand UserClickedCommand { get; }
    public ICommand StopVideoCommand { get; }

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

        SaveHomeTeamToFileCommand = ReactiveCommand.CreateFromTask
        (
            async () =>
            {
                if (!TrySaveFileDialog($"Команда.{TEAM_PROJECT_FILE_EXTENSION}",
                                       "Файл команды хоккейного матча".ConcatExtensions(TEAM_PROJECT_FILE_EXTENSION),
                                       out var fileName))
                {
                    return;
                }

                await Model.SaveHomeTeamToFile(fileName);
            }
        );

        ReadHomeTeamToFileCommand = ReactiveCommand.CreateFromTask
        (
            async () =>
            {
                if (!TryOpenFileDialog("Файл команды хоккейного матча".ConcatExtensions(TEAM_PROJECT_FILE_EXTENSION),
                                       out var fileName))
                {
                    return;
                }

                await Model.ReadHomeTeamToFile(fileName);
            }
        );

        SaveGuestTeamToFileCommand = ReactiveCommand.CreateFromTask
        (
            async () =>
            {
                if (!TrySaveFileDialog($"Команда.{TEAM_PROJECT_FILE_EXTENSION}",
                                       "Файл команды хоккейного матча".ConcatExtensions(TEAM_PROJECT_FILE_EXTENSION),
                                       out var fileName))
                {
                    return;
                }

                await Model.SaveGuestTeamToFile(fileName);
            }
        );

        ReadGuestTeamToFileCommand = ReactiveCommand.CreateFromTask
        (
            async () =>
            {
                if (!TryOpenFileDialog("Файл команды хоккейного матча".ConcatExtensions(TEAM_PROJECT_FILE_EXTENSION),
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
                await Model.ReadProjectFromFile(fileName, LastTokenSource.Token);
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
                await Model.ReadVideoFromFile(fileName, LastTokenSource.Token);
            }
        );

        StopVideoCommand = ReactiveCommand.Create(() => LastTokenSource.Cancel());

        ReversePauseCommand = ReactiveCommand.Create(() => Model.IsPaused = !Model.IsPaused);
        UserClickedCommand = ReactiveCommand.Create<bool>(x => Model.IsUserClick = x);
    }
}
