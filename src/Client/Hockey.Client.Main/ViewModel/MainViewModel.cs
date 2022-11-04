using Hockey.Client.Main.Model.Abstraction;
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
    public ICommand ReadVideoFromFileCommand { get; }
    [Reactive] public ImageSource Frame { get; set; }

    public MainViewModel(IMainModel model)
    {
        Model = model;

        Model.WhenAnyValue(x => x.CurrentFrame)
             .Select(x => x is null ? default : BitmapSourceConverter.ToBitmapSource(x))
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

                CancellationTokenSource cts = new();
                await Model.ReadVideoFromFile(openFileDialog.FileName, cts.Token);
            }
        );
    }
}
