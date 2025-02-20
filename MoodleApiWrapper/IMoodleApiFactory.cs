namespace MoodleApiWrapper;

public interface IMoodleApiFactory
{
    MoodleApiWrapper.IMoodleApi Get(string lmsName);
}