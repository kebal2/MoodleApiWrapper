namespace MoodleApiWrapper;

public interface IMoodleRequestBuilderFactory
{
    public MoodleRequestBuilder Create(string lmsName);
}