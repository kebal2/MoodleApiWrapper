// See https://aka.ms/new-console-template for more information

using CommandLine;

using MoodleApiWrapper;

using Newtonsoft.Json;

Uri host = default;
string apiToken = default;
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

var courses = new[]{new CourseModel("Alma", "Alma", 0)};

var result = await moodleApiWrapper.CreateCourses(courses);

var res = JsonConvert.SerializeObject(result);

Console.WriteLine(res);

Console.ReadKey();