using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;

namespace Hockey.Client.Main.ViewModel;

internal class EventConstructorViewModel : ReactiveObject
{
    public IEventConstructorModel Model { get; }
    [Reactive] public EventFactory SelectedFactoryCreator { get; set; }

    public IEnumerable<Key> BindingKeys { get; } = Enum.GetValues<Key>();

    public ICommand DeleteEventFactoryCommand { get; }
    public ICommand AddEventFactoryCommand { get; }
    public ICommand DeleteParameterFactoryCommand { get; }
    public ICommand AddPlayerParameterCommand { get; }
    public ICommand AddTeamParameterCommand { get; }
    public ICommand AddTextParameterCommand { get; }

    public EventConstructorViewModel(IEventConstructorModel model)
    {
        Model = model;

        SelectedFactoryCreator = Model.Factories.FirstOrDefault();

        var isSelected = this.WhenAnyValue(x => x.SelectedFactoryCreator)
                             .Select(x => x is not null);

        DeleteEventFactoryCommand = ReactiveCommand.Create<EventFactory>
        (
            x => Model.Factories.Remove(x)
        );

        DeleteParameterFactoryCommand = ReactiveCommand.Create<EventParameterFactory>
        (
            x => SelectedFactoryCreator.ParameterFactories.Remove(x),
            isSelected
        );

        AddEventFactoryCommand = ReactiveCommand.Create
        (
            () =>
            {
                EventFactory newFactory = new()
                {
                    EventType = new("Новое событие"),
                    DefaultDuration = TimeSpan.FromSeconds(10),
                };

                Model.Factories.Add(newFactory);
                SelectedFactoryCreator = newFactory;
            }
        );

        AddPlayerParameterCommand = ReactiveCommand.Create
        (
            () => SelectedFactoryCreator.ParameterFactories
                                        .Add(new PlayerEventParameterFactory
                                        {
                                            Name = "Игрок",
                                            TeamName = "Команда",
                                        }),
            isSelected
        );

        AddTeamParameterCommand = ReactiveCommand.Create
        (
            () => SelectedFactoryCreator.ParameterFactories
                                        .Add(new TeamEventParameterFactory
                                        {
                                            Name = "Игрок",
                                        }),
            isSelected
        );

        AddTextParameterCommand = ReactiveCommand.Create
        (
            () => SelectedFactoryCreator.ParameterFactories
                                        .Add(new TextEventParameterFactory
                                        {
                                            Name = "Текст",
                                        }),
            isSelected
        );
    }
}