using Hockey.Client.Shared.Dialogs.ViewModels;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace Hockey.Client.Shared.Dialogs;

public static class DialogExtensionsMethods
{
    public static async Task<bool> Confirm(this IDialogService dialogService, string text)
    {
        var result = await dialogService.ShowDialog(DialogNames.Confirm, (nameof(ConfirmDialogViewModel.Text), text));

        return result.Result switch
        {
            ButtonResult.Yes => true,
            ButtonResult.No => false,
            _ => false
        };
    }

    private static Task<IDialogResult> ShowDialog(this IDialogService dialogService, string dialogName, params (string name, object value)[] parameters)
    {
        TaskCompletionSource<IDialogResult> taskCompletionSource = new();

        DialogParameters dialogParameters = new();
        foreach (var (name, value) in parameters)
        {
            dialogParameters.Add(name, value);
        }

        dialogService.ShowDialog(dialogName, dialogParameters, taskCompletionSource.SetResult);

        return taskCompletionSource.Task;
    }
}
