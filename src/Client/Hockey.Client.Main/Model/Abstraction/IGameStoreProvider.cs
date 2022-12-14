namespace Hockey.Client.Main.Model.Abstraction;
internal interface IGameStoreProvider
{
    IGameStore CreateDefault();
}
