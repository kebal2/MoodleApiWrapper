using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace MoodleApiWrapper;

public class UpdateCourseRoot : IDataModel, ICloneable
{
    [JsonConstructor]
    internal UpdateCourseRoot(List<Warning> warnings)
    {
        this.warnings = warnings;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public List<Warning> warnings { get; set; }
}