using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper.Model;

public class Group : ICloneable, IDataModel
{
    [JsonConstructor]
    internal Group(int id, int courseid, string name, string description, int descriptionformat, string enrolmentkey)
    {
        this.id = id;
        this.courseid = courseid;
        this.name = name;
        this.description = description;
        this.descriptionformat = descriptionformat;
        this.enrolmentkey = enrolmentkey;
    }

    public int id { get; set; }
    public int courseid { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int descriptionformat { get; set; }
    public string enrolmentkey { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}