using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper.Model;

public class Preference : ICloneable
{
    public string name { get; set; }
    public object value { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    [JsonConstructor]
    internal Preference(string name, string value)
    {
        this.name = name;
        this.value = value;
    }
}