using Hockey.Client.Main.Model;
using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Events;
using Hockey.Client.Shared.Extensions;
using Microsoft.Win32;
using OpenCvSharp.WpfExtensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;

namespace Hockey.Client.Main.ViewModel;

internal class MainViewModel : ReactiveObject
{
    public IMainModel Model { get; }
    [Reactive] public ImageSource Frame { get; set; }
    [Reactive] public int TabIndex { get; set; }
    [Reactive] public CancellationTokenSource LastTokenSource { get; set; }
    [Reactive] public EventModel CurrentEvent { get; set; }

    public ICommand ReadVideoFromFileCommand { get; }
    public ICommand ReversePauseCommand { get; }

    public ICommand AddPlayerToGuestTeamCommand { get; }
    public ICommand RemovePlayerFromGuestTeamCommand { get; }

    public ICommand AddPlayerToHomeTeamCommand { get; }
    public ICommand RemovePlayerFromHomeTeamCommand { get; }
    public ICommand UserClickedCommand { get; }
    public ICommand StopVideoCommand { get; }

    public ICommand AddEventCommand { get; }
    public ICommand EditEventCommand { get; }
    public ICommand ShowEventCommand { get; }

    private readonly int _editEventIndex = 1;

    public MainViewModel(IMainModel model)
    {
        Model = model;

        Model.WhenAnyValue(x => x.CurrentFrame)
             .Select(x => x is null || x.IsDisposed ? default : BitmapSourceConverter.ToBitmapSource(x))
             .Subscribe(x => Frame = x)
             .Cache();

        ReadVideoFromFileCommand = ReactiveCommand.CreateFromTask
        (
            async () =>
            {
                OpenFileDialog openFileDialog = new();

                if (openFileDialog.ShowDialog() == false)
                {
                    return;
                }

                LastTokenSource = new();
                await Model.ReadVideoFromFile(openFileDialog.FileName, LastTokenSource.Token);
            }
        );

        ReversePauseCommand = ReactiveCommand.Create(() => Model.IsPaused = !Model.IsPaused);
        UserClickedCommand = ReactiveCommand.Create<bool>(x => Model.IsUserClick = x);

        AddPlayerToGuestTeamCommand = ReactiveCommand.Create(() => AddPlayerToTeam(Model.GuestTeam));
        RemovePlayerFromGuestTeamCommand = ReactiveCommand.Create<PlayerModel>(x => RemovePlayerFromTeam(Model.GuestTeam, x));

        AddPlayerToHomeTeamCommand = ReactiveCommand.Create(() => AddPlayerToTeam(Model.HomeTeam));
        RemovePlayerFromHomeTeamCommand = ReactiveCommand.Create<PlayerModel>(x => RemovePlayerFromTeam(Model.HomeTeam, x));
        ShowEventCommand = ReactiveCommand.Create<EventModel>(x =>
        {
            if (x is null)
            {
                return;
            }

            Model.SetPosition(x.FramePosition);
        });

        AddEventCommand = ReactiveCommand.Create<EventTypeModel>(x =>
        {
            EventModel ev = new(x, Model.FrameNumber);
            Model.Events.Add(ev);

            CurrentEvent = ev;
            TabIndex = _editEventIndex;
        });

        EditEventCommand = ReactiveCommand.Create<EventModel>(x =>
        {
            CurrentEvent = x;
            TabIndex = _editEventIndex;
        });
    }

    private void AddPlayerToTeam(TeamModel teamModel)
    {
        int index = teamModel.Players.Count + 1;
        teamModel.Players.Add(new() { PlayerNumber = index, Name = $"Игрок {index}" });
    }

    private void RemovePlayerFromTeam(TeamModel teamModel, PlayerModel playerModel)
    {
        teamModel.Players.Remove(playerModel);
    }
}
