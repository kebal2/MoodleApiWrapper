using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper;

public class Event : ICloneable
{
    [JsonConstructor]
    internal Event(int id, string name, string description, int format, int courseid, int groupid, int userid, int repeatid, string modulename, int instance, string eventtype, int timestart, int timeduration, int visible, string uuid,
        int sequence, int timemodified, object subscriptionid)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.format = format;
        this.courseid = courseid;
        this.groupid = groupid;
        this.userid = userid;
        this.repeatid = repeatid;
        this.modulename = modulename;
        this.instance = instance;
        this.eventtype = eventtype;
        this.timestart = timestart;
        this.timeduration = timeduration;
        this.visible = visible;
        this.uuid = uuid;
        this.sequence = sequence;
        this.timemodified = timemodified;
        this.subscriptionid = subscriptionid;
    }

    public int id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int format { get; set; }
    public int courseid { get; set; }
    public int groupid { get; set; }
    public int userid { get; set; }
    public int repeatid { get; set; }
    public string modulename { get; set; }
    public int instance { get; set; }
    public string eventtype { get; set; }
    public int timestart { get; set; }
    public int timeduration { get; set; }
    public int visible { get; set; }
    public string uuid { get; set; }
    public int sequence { get; set; }
    public int timemodified { get; set; }
    public object subscriptionid { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}