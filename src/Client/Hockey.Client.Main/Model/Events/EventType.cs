using System.ComponentModel;

namespace Hockey.Client.Main.Model.Events;

internal enum EventType
{
    [Description("Толчок соперника")]
    OpponentSPush,
    [Description("Блокировка ")]
    Lock,
    [Description("Задержка соперника")]
    OpponentSDelay,
    [Description("Задержка соперника клюшкой (зацеп)")]
    DelayOfTheOpponentWithTheStick,
    [Description("Задержка клюшки соперника (руками)")]
    DelayOfTheOpponentSStick,
    [Description("Атака сзади")]
    AttackFromBehind,
    [Description("Опасная игра высоко поднятой клюшкой")]
    DangerousGameWithAHighRaisedStick,
    [Description("Положение <вне игры>")]
    Offside,
    [Description("Проброс шайбы")]
    PositionPuckDrop,
    [Description("Гол")]
    Goal,
    [Description("Автогол")]
    Autogall,
    [Description("Голевой пас")]
    ScoringPass,
    [Description("Вбрасывание")]
    FaceOff,
    [Description("Пас через две линии")]
    PassThroughTwoLines,
    [Description("Буллит")]
    Bullitt,
    [Description("Атака")]
    Attack,
    [Description("Защита")]
    Protection,
    [Description("Бросок")]
    Cast,
    [Description("Щелчок")]
    Click,
    [Description("Кистевой бросок")]
    WristThrow,
    [Description("Бросок с неудобной руки")]
    ThrowWithAnUncomfortableHand,
    [Description("Ван-таймер")]
    VanTimer,
    [Description("Пас")]
    Pass,
    [Description("Обводка")]
    Outline,
    [Description("Финт")]
    Feint,
    [Description("Спин-о-рама ")]
    SpinOFrame,
    [Description("Силовой прием")]
    PowerReception,
    [Description("Мельница")]
    Mill,
    [Description("Отбор шайбы")]
    PuckSelection,
    [Description("Прессинг")]
    Pressure,
    [Description("Сэйв")]
    Save
}
