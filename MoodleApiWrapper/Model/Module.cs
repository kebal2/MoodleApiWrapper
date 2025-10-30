using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper.Model;

public class Module : ICloneable
{
    [JsonConstructor]
    internal Module(int id, string name, int visible, string modicon, string modname, string modplural, string availability, int indent, string url)
    {
        this.id = id;
        this.name = name;
        this.visible = visible;
        this.modicon = modicon;
        this.modname = modname;
        this.modplural = modplural;
        this.availability = availability;
        this.indent = indent;
        this.url = url;
    }

    public int id { get; set; }
    public string name { get; set; }
    public int visible { get; set; }
    public string modicon { get; set; }
    public string modname { get; set; }
    public string modplural { get; set; }
    public string availability { get; set; }
    public int indent { get; set; }
    public string url { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}