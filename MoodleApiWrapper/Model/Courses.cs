using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper.Model;

public class Courses : ICloneable, IDataModel
{
    [JsonConstructor]
    internal Courses(int id, string shortname, string fullname, int enrolledusercount, string idnumber,
        int visible, string summary, int summaryformat, string format, bool showgrades,
        string lang, bool enablecompletion)
    {
        this.id = id;
        this.shortname = shortname;
        this.fullname = fullname;
        this.enrolledusercount = enrolledusercount;
        this.idnumber = idnumber;
        this.visible = visible;
        this.summary = summary;
        this.summaryformat = summaryformat;
        this.format = format;
        this.showgrades = showgrades;
        this.lang = lang;
        this.enablecompletion = enablecompletion;
    }

    public int id { get; set; }
    public string shortname { get; set; }
    public string fullname { get; set; }
    public int enrolledusercount { get; set; }
    public string idnumber { get; set; }
    public int visible { get; set; }
    public string summary { get; set; }
    public int summaryformat { get; set; }
    public string format { get; set; }
    public bool showgrades { get; set; }
    public string lang { get; set; }
    public bool enablecompletion { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}