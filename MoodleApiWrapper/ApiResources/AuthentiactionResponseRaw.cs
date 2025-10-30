using Newtonsoft.Json.Linq;

namespace MoodleApiWrapper.ApiResources;

internal class AuthentiactionResponseRaw
{
    internal JObject Data { get; set; }
    internal JObject Error { get; set; }

    public AuthentiactionResponseRaw(JObject data)
    {
        Data = data;
        Error = data;
    }
}