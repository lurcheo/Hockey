using Hockey.Client.BusinessLayer.Abstraction;
using Hockey.Client.Settings.Model.Abstraction;
using Hockey.Client.Shared.Extensions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Hockey.Client.Settings.Model;
internal class SettingsModel : ReactiveObject, ISettingsModel
{
    public IAppHelper AppHelper { get; }

    [Reactive] public bool IsDark { get; set; } = true;
    [Reactive] public bool IsMaximized { get; set; } = true;

    public SettingsModel(IAppHelper appHelper)
    {
        AppHelper = appHelper;

        this.WhenAnyValue(x => x.IsDark)
            .Subscribe(AppHelper.ChangeTheme)
            .Cache();
    }
}
