using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper.Model;

public class Courseformatoption : ICloneable
{
    [JsonConstructor]
    internal Courseformatoption(string name, int value)
    {
        this.name = name;
        this.value = value;
    }

    public string name { get; set; }
    public int value { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}