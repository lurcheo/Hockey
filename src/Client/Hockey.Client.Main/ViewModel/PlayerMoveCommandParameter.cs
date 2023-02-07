using Hockey.Client.Main.Model.Data;

namespace Hockey.Client.Main.ViewModel;

internal record PlayerMoveCommandParameter(int NewLink, PlayerInfo PlayerInfo);
