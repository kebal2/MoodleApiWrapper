﻿using System;

using Newtonsoft.Json;

namespace MoodleApiWrapper;

public class Success : IDataModel, ICloneable
{
    private bool IsSuccessful { get; set; }

    [JsonConstructor]
    internal Success(bool isSuccessful)
    {
        IsSuccessful = isSuccessful;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}