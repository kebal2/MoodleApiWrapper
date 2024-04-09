using CommandLine;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using MoodleApiWrapper;
using MoodleApiWrapper.Options;

using Options = Console.Options;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
Uri host = default;
string apiToken = default;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

int courseCount = 1;

Parser.Default.ParseArguments<Options>(args)
    .WithParsed(o =>
    {
        host = new Uri(o.Host);
        apiToken = o.Token;

        if (o.CourseCount.HasValue)
            courseCount = o.CourseCount.Value;
    });

var client = new HttpClient { BaseAddress = host };

var services = new ServiceCollection();

services.AddOptions<Moodle>().Configure(o => o.ApiToken = apiToken);

services.AddSingleton(client);
services.AddTransient<MoodleRequestBuilder>();
services.AddTransient<MoodleApi>();

var serviceProvider = services.BuildServiceProvider();


var moodleApi = serviceProvider.GetService<MoodleApi>();

//await moodleApi.CreateCourses(courseCount);
//await moodleApi.DeleteCourses();

System.Console.ReadKey();
