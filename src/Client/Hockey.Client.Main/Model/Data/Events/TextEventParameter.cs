using ReactiveUI.Fody.Helpers;

namespace Hockey.Client.Main.Model.Data.Events;
internal class TextEventParameter : EventParameter
{
    [Reactive] public string Text { get; set; }

    public TextEventParameter(string name = "Текст")
        : base(name)
    {
    }
}
