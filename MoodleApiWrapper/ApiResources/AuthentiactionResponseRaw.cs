using Newtonsoft.Json.Linq;

namespace MoodleApiWrapper;

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