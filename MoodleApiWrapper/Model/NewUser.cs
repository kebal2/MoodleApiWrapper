using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper.Model;

public class NewUser : ICloneable, IDataModel
{
    public int id { get; set; }
    public string username { get; set; }

    [JsonConstructor]
    internal NewUser(int id, string username)
    {
        this.id = id;
        this.username = username;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}