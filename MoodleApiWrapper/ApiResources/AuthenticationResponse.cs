using MoodleApiWrapper.Model;

namespace MoodleApiWrapper.ApiResources;

public class AuthenticationResponse<T> where T : IDataModel
{
    public T? Data { get; private set; }

    public AuthenticationError? Error { get; private set; }

    internal AuthenticationResponse(AuthentiactionResponseRaw rawResponse)
    {
        this.Error = rawResponse.Error.ToObject<AuthenticationError>();
        this.Data = rawResponse.Data.ToObject<T>();
    }
}