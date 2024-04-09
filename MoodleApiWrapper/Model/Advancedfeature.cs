using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper.Model;

public class Advancedfeature : ICloneable
{
    public string name { get; set; }
    public int value { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    [JsonConstructor]
    internal Advancedfeature(string name, int value)
    {
        this.name = name;
        this.value = value;
    }
}
