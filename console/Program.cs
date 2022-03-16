// See https://aka.ms/new-console-template for more information

using CommandLine;

using Console;

using MoodleApiWrapper;

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

var moodleApiWrapper = new ApiWrapper(host, apiToken);

//await moodleApiWrapper.CreateCourses(courseCount);

await moodleApiWrapper.DeleteCourses();

System.Console.ReadKey();

