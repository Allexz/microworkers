namespace Microworkers.Application.Common;

public class JsonSerializer : ISerializer
{
    public string Serialize(object obj)
    {
        return System.Text.Json.JsonSerializer.Serialize(obj);
    }

    public T Deserialize<T>(string json)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(json);
    }
}
