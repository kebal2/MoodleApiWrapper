using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace MoodleApiWrapper;

public class Content : ICloneable, IDataModel
{
    public int id { get; set; }
    public string name { get; set; }
    public int visible { get; set; }
    public string summary { get; set; }
    public int summaryformat { get; set; }
    public List<Module> modules { get; set; }

    [JsonConstructor]
    internal Content(int id, string name, int visible, string summary, int summaryformat, List<Module> modules)
    {
        this.id = id;
        this.name = name;
        this.visible = visible;
        this.summary = summary;
        this.summaryformat = summaryformat;
        this.modules = modules;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}