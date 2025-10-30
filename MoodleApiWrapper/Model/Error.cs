using System;

namespace MoodleApiWrapper.Model;

public class Error : ICloneable
{
    public Error(string exception, string errorcode, string message)
    {
        this.exception = exception;
        this.errorcode = errorcode;
        this.message = message;
    }

    public string exception { get; set; }
    public string errorcode { get; set; }
    public string message { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}