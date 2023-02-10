using System.Threading.Tasks;

namespace Hockey.Client.BusinessLayer.Abstraction;

public interface IFileService
{
    Task<T> GetFromFile<T>(string fileName);
    Task SaveToFile<T>(string fileName, T obj);
}
