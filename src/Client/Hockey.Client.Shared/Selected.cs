using ReactiveUI;

namespace Hockey.Client.Shared;

public class Selected<T> : ReactiveObject
{
    public bool IsSelected
    {
        get => isSelected;
        set => this.RaiseAndSetIfChanged(ref isSelected, value);
    }
    private bool isSelected = true;

    public T Item { get; }

    public Selected(T item)
    {
        Item = item;
    }
}
