namespace MoodleApiWrapper;

public class AuthentiactionResponse<T> where T : IDataModel
{
    public T Data { get; private set; }

    public AuthenticationError Error { get; private set; }

    internal AuthentiactionResponse(AuthentiactionResponseRaw rawResponse)
    {
        this.Error = rawResponse.Error.ToObject<AuthenticationError>();
        this.Data = rawResponse.Data.ToObject<T>();
    }

}