using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper;

public class EnrolledCourse : ICloneable
{
    [JsonConstructor]
    internal EnrolledCourse(int id, string fullname, string shortname)
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
