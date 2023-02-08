using Hockey.Client.Main.Model.Abstraction;
using Hockey.Client.Main.Model.Data.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;

namespace Hockey.Client.Main.ViewModel;

internal class EventConstructorViewModel : ReactiveObject
{
    public IEventConstructorModel Model { get; }
    [Reactive] public EventFactoryCreator SelectedFactoryCreator { get; set; }


    public ICommand AddEventFactoryCommand { get; }
    public ICommand AddPlayerParameterCommand { get; }
    public ICommand AddTeamParameterCommand { get; }
    public ICommand AddTextParameterCommand { get; }
    public ICommand CreateSelectedFactoryCommand { get; }

    public EventConstructorViewModel(IEventConstructorModel model)
    {
        Model = model;

        SelectedFactoryCreator = Model.FactoryCreators.FirstOrDefault();

        var isSelected = this.WhenAnyValue(x => x.SelectedFactoryCreator)
                             .Select(x => x is not null);

        CreateSelectedFactoryCommand = ReactiveCommand.Create
        (
            () => Model.AddEventFactory(SelectedFactoryCreator),
            isSelected.CombineLatest(this.WhenAnyValue(x => x.SelectedFactoryCreator.IsCreated),
                                     (s, c) => s && !c)
        );

        AddEventFactoryCommand = ReactiveCommand.Create
        (
            () =>
            {
                EventFactoryCreator newFactory = new()
                {
                    Name = "Новое событие",
                    DefaultTimeSpan = TimeSpan.FromSeconds(10),
                };

                Model.FactoryCreators.Add(newFactory);
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
