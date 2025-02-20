using System.Net.Http;

namespace MoodleApiWrapper;

public class MoodleApiFactory : IMoodleApiFactory
{
    private readonly HttpClient client;
    private readonly IMoodleRequestBuilderFactory moodleRequestBuilderFactory;

    public MoodleApiFactory(HttpClient client, IMoodleRequestBuilderFactory moodleRequestBuilderFactory)
    {
        this.client = client;
        this.moodleRequestBuilderFactory = moodleRequestBuilderFactory;
    }

    public IMoodleApi Get(string lmsName)
    {
        return new MoodleApi(this.client, this.moodleRequestBuilderFactory.Create(lmsName));
    }
}