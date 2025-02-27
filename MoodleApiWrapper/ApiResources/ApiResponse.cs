using System;
using System.Runtime.InteropServices.ComTypes;

using Newtonsoft.Json;

namespace MoodleApiWrapper.ApiResources;

/// <summary>
/// Represents the API response.
/// </summary>
/// <typeparam name="T">The type of data that's going to be contained in the response.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Gets the API response data.
    /// </summary>
    public bool SuccessfulCall { get; private set; }

    public T Data { get; private set; }

    public Error Error { get; }

    public string ResponseText { get; set; }

    public string RequestedPath { get; set; }
    internal ApiResponse(ApiResponseRaw rawResponse)
    {
        Error = rawResponse.Error.ToObject<Error>();

        SuccessfulCall = Error.errorcode == null && Error.exception == null && Error.message == null;

        if (!SuccessfulCall)
        {
            Data = default;
            return;
        }

        try
        {
            Data = rawResponse.Data.ToObject<T>();
        }
        catch (JsonSerializationException)
        {
            var a =rawResponse.Data.ToObject<T[]>();

            if (a.Length > 1)
                throw new InvalidOperationException("Single entry expected, got an array");

            Data = a.Length == 0
                ? default
                : a[0];
        }
    }
}
