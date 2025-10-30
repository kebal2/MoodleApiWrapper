using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper.Model;

public class Role : ICloneable
{
    [JsonConstructor]
    internal Role(int roleid, string name, string shortname, int sortorder)
    {
        this.roleid = roleid;
        this.name = name;
        this.shortname = shortname;
        this.sortorder = sortorder;
    }

    public int roleid { get; set; }
    public string name { get; set; }
    public string shortname { get; set; }
    public int sortorder { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}