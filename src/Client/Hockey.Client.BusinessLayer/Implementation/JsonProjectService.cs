using Hockey.Client.BusinessLayer.Abstraction;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Hockey.Client.BusinessLayer.Implementation;

internal class JsonFileService : IFileService
{
    public async Task<T> GetFromFile<T>(string fileName)
    {
        return JsonConvert.DeserializeObject<T>(await File.ReadAllTextAsync(fileName));
    }

    public Task SaveToFile<T>(string fileName, T obj)
    {
        return File.WriteAllTextAsync(fileName, JsonConvert.SerializeObject(obj));
    }
}
