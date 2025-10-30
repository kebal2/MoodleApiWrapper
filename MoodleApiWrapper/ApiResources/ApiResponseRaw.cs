using Newtonsoft.Json.Linq;

namespace MoodleApiWrapper.ApiResources;


internal class ApiResponseRaw
{
    internal ApiResponseRaw(JContainer data)
    {
        Data = data;

        Error = data as JObject ?? new JObject();
    }

    internal JContainer Data { get; set; }
    internal JObject Error { get; set; }
}
