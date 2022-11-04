namespace Hockey.Client.BusinessLayer.Abstraction;

public interface IFactory<T>
{
    T Create();
}
