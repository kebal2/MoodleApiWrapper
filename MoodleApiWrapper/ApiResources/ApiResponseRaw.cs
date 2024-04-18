using Newtonsoft.Json.Linq;

namespace MoodleApiWrapper;

internal class ApiResponseRaw
{
    public ApiResponseRaw(JContainer data)
    {
        Data = data;

        Error = data as JObject ?? new JObject();
    }

    internal JContainer Data { get; set; }
    internal JObject Error { get; set; }
}
