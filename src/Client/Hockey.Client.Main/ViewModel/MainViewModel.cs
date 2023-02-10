using Hockey.Client.Main.Events;
using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data;
using Hockey.Client.Shared.Extensions;
using Microsoft.Win32;
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

namespace Hockey.Client.Main.ViewModel;

internal class MainViewModel : ReactiveObject
{
    public IMainModel Model { get; }
    public IEventAggregator EventAggregator { get; }

    [Reactive] public ImageSource Frame { get; set; }
    [Reactive] public CancellationTokenSource LastTokenSource { get; set; }

    [Reactive] public bool IsLeftMenuOpen { get; set; }
    [Reactive] public bool IsRightMenuOpen { get; set; }

    public ICommand SaveProjectToFileCommand { get; }
    public ICommand SaveHomeTeamToFileCommand { get; }
    public ICommand SaveGuestTeamToFileCommand { get; }

    public ICommand ReadVideoFromFileCommand { get; }
    public ICommand ReadProjectFromFileCommand { get; }

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
            () => SaveTeam(Model.HomeTeam)
        );

        SaveGuestTeamToFileCommand = ReactiveCommand.CreateFromTask
        (
            () => SaveTeam(Model.GuestTeam)
        );

        ReadProjectFromFileCommand = ReactiveCommand.CreateFromTask
        (
            async () =>
            {
                OpenFileDialog openFileDialog = new();

                string ext = "hapj";
                openFileDialog.Filter = "Проект аналитики хоккейного матча".ConcatExtensions(ext);

                if (openFileDialog.ShowDialog() == false)
                {
                    return;
                }

                LastTokenSource?.Cancel();
                LastTokenSource = new();
                await Model.ReadProjectFromFile(openFileDialog.FileName, LastTokenSource.Token);
            }
        );

        SaveProjectToFileCommand = ReactiveCommand.CreateFromTask
        (
            async () =>
            {
                SaveFileDialog saveFileDialog = new();

                string ext = "hapj";
                saveFileDialog.Filter = "Проект аналитики хоккейного матча".ConcatExtensions(ext);
                saveFileDialog.FileName = $"Аналитика матча.{ext}";

                if (saveFileDialog.ShowDialog() == false)
                {
                    return;
                }

                await Model.SaveProjectToFile(saveFileDialog.FileName);
            }
        );

        ReadVideoFromFileCommand = ReactiveCommand.Create
        (
            async () =>
            {
                OpenFileDialog openFileDialog = new();

                string videoExtensions = "Видео".ConcatExtensions("AVI", "MP4", "MPEG", "MOV");
                string imageExtensions = "Изображения".ConcatExtensions("BMP", "JPEG", "JPG", "PNG");

                openFileDialog.Filter = string.Join('|', videoExtensions, imageExtensions);

                if (openFileDialog.ShowDialog() == false)
                {
                    return;
                }

                LastTokenSource?.Cancel();
                LastTokenSource = new();
                await Model.ReadVideoFromFile(openFileDialog.FileName, LastTokenSource.Token);
            }
        );

        StopVideoCommand = ReactiveCommand.Create(() => LastTokenSource.Cancel());

        ReversePauseCommand = ReactiveCommand.Create(() => Model.IsPaused = !Model.IsPaused);
        UserClickedCommand = ReactiveCommand.Create<bool>(x => Model.IsUserClick = x);
    }



    private async Task SaveTeam(TeamInfo teamInfo)
    {
        string ext = "tpj";
        string filter = "Файл команды для проекта аналитики хоккейного матча".ConcatExtensions(ext);
        string defaultFileName = $"{teamInfo.Name}.{ext}";

        if (!TrySaveFileDialog(defaultFileName, filter, out var fileName))
        {
            return;
        }

        await Model.SaveTeamToFile(fileName, teamInfo);
    }


    private static bool TrySaveFileDialog(string defaultFileName, string filter, out string fileName)
    {
        SaveFileDialog saveFileDialog = new();

        saveFileDialog.Filter = filter;
        saveFileDialog.FileName = defaultFileName;

        if (saveFileDialog.ShowDialog() == false)
        {
            fileName = default;
            return false;
        }

        fileName = saveFileDialog.FileName;
        return true;
    }
}
