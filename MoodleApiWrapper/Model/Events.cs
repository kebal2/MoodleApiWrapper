using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace MoodleApiWrapper.Model;

public class Events : ICloneable, IDataModel
{
    [JsonConstructor]
    internal Events(List<Event> events, List<Warning> warnings)
    {
        this.events = events;
        this.warnings = warnings;
    }

    public List<Event> events { get; set; }
    public List<Warning> warnings { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}