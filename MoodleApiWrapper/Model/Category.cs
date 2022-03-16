using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper;

public class Category : IDataModel, ICloneable
{
    [JsonConstructor]
    internal Category(int id, string name, string idnumber, string description, int descriptionformat, int parent, int sortorder, int coursecount, int visible, int visibleold, int timemodified, int depth, string path, object theme)
    {
        this.id = id;
        this.name = name;
        this.idnumber = idnumber;
        this.description = description;
        this.descriptionformat = descriptionformat;
        this.parent = parent;
        this.sortorder = sortorder;
        this.coursecount = coursecount;
        this.visible = visible;
        this.visibleold = visibleold;
        this.timemodified = timemodified;
        this.depth = depth;
        this.path = path;
        this.theme = theme;
    }

    public int id { get; set; }
    public string name { get; set; }
    public string idnumber { get; set; }
    public string description { get; set; }
    public int descriptionformat { get; set; }
    public int parent { get; set; }
    public int sortorder { get; set; }
    public int coursecount { get; set; }
    public int visible { get; set; }
    public int visibleold { get; set; }
    public int timemodified { get; set; }
    public int depth { get; set; }
    public string path { get; set; }
    public object theme { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}