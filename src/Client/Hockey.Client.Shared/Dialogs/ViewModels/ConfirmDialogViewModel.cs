using Prism.Services.Dialogs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Windows.Input;

namespace Hockey.Client.Shared.Dialogs.ViewModels;

public class ConfirmDialogViewModel : ReactiveObject, IDialogAware
{
    public string Title { get; } = "Подтверждение операции";
    [Reactive] public string Text { get; set; }

    public event Action<IDialogResult> RequestClose;

    public ICommand YesCloseDialogCommand { get; }
    public ICommand NoCloseDialogCommand { get; }

    public ConfirmDialogViewModel()
    {
        YesCloseDialogCommand = ReactiveCommand.Create(() => CloseDialog(true));
        NoCloseDialogCommand = ReactiveCommand.Create(() => CloseDialog(false));
    }

    private void CloseDialog(bool confirm)
    {
        RequestClose?.Invoke(new DialogResult(confirm ? ButtonResult.Yes : ButtonResult.No));
    }

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        Text = parameters.GetValue<string>(nameof(Text));
    }
}
