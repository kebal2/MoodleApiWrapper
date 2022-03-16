using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper;

public class Enrolledcours : ICloneable
{
    [JsonConstructor]
    internal Enrolledcours(int id, string fullname, string shortname)
    {
        this.id = id;
        this.fullname = fullname;
        this.shortname = shortname;
    }

    public int id { get; set; }
    public string fullname { get; set; }
    public string shortname { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}