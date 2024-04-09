using Newtonsoft.Json.Linq;

namespace MoodleApiWrapper;

internal class ApiResponseRaw
{
    public ApiResponseRaw(JObject data)
    {
        Data = data;
        Error = data;
    }

    public ApiResponseRaw(JArray data)
    {
        DataArray = data;
        Error = new JObject();
    }

    internal JArray DataArray { get; set; }
    internal JObject Data { get; set; }
    internal JObject Error { get; set; }
}