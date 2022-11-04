namespace Hockey.Client.BusinessLayer.Abstraction;
public interface IVideoService
{
    IVideoReader ReadVideoFromFile(string fileName);
}
