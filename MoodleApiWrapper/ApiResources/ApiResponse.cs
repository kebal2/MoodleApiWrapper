﻿namespace MoodleApiWrapper.ApiResources;

/// <summary>
/// Represents the API response.
/// </summary>
/// <typeparam name="T">The type of data that's going to be contained in the response.</typeparam>
public class ApiResponse<T> where T : IDataModel
{
    /// <summary>
    /// Gets the API response data.
    /// </summary>
    public string Status { get; private set; }

    public T[] DataArray { get; private set; }

    public T Data { get; private set; }

    public Error Error { get; }

    public string ResponseText { get; set; }

    public string RequestedPath { get; set; }
    internal ApiResponse(ApiResponseRaw rawResponse)
    {
        this.Error = rawResponse.Error.ToObject<Error>();

        if (Error.errorcode == null && Error.exception == null && Error.message == null)
            Status = "Succesful";
        else
            Status = "Failed";

        if (null != rawResponse.DataArray)
            this.DataArray = rawResponse.DataArray.ToObject<T[]>();
        else
            this.Data = rawResponse.Data.ToObject<T>();
    }
}
