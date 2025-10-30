using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper.Model;

public class Warning : ICloneable
{
    [JsonConstructor]
    internal Warning(string item, string warningcode, string message, int itemid)
    {
        this.item = item;
        this.itemid = itemid;
        this.warningcode = warningcode;
        this.message = message;
    }

    public int itemid { get; set; }
    public string item { get; set; }
    public string warningcode { get; set; }
    public string message { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}