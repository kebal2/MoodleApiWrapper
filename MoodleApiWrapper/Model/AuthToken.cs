using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper.Model;

public class AuthToken : ICloneable, IDataModel
{
    public string token { get; set; }

    [JsonConstructor]
    internal AuthToken(string token)
    {
        this.token = token;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}