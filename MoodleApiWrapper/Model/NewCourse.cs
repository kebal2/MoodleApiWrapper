using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper;

public class NewCourse : ICloneable, IDataModel
{
    public int id { get; set; }
    public string shortname { get; set; }

    [JsonConstructor]
    internal NewCourse(int id, string username)
    {
        this.id = id;
        this.shortname = username;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}