using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper;

public class AuthenticationError : IDataModel, ICloneable
{
    [JsonConstructor]
    internal AuthenticationError(string error, object stacktrace, object debuginfo, object reproductionlink)
    {
        this.error = error;
        this.stacktrace = stacktrace;
        this.debuginfo = debuginfo;
        this.reproductionlink = reproductionlink;
    }

    public string error { get; set; }
    public object stacktrace { get; set; }
    public object debuginfo { get; set; }
    public object reproductionlink { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}