using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace MoodleApiWrapper;

public class Users : ICloneable, IDataModel
{
    [JsonConstructor]
    internal Users(List<User> users, List<Warning> warnings)
    {
        this.users = users;
        this.warnings = warnings;
    }

    public List<User> users { get; set; }
    public List<Warning> warnings { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}