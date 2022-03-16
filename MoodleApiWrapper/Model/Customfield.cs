using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper;

public class Customfield : ICloneable
{
    public string type { get; set; }
    public string value { get; set; }
    public string name { get; set; }
    public string shortname { get; set; }

    [JsonConstructor]
    internal Customfield(string type, string value, string name, string shortname)
    {
        this.type = type;
        this.value = value;
        this.name = name;
        this.shortname = shortname;
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}