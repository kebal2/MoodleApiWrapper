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

var courses = new List<CourseModel>(
    Enumerable.Range(0, courseCount).Select(i =>
    {
        var courseFullName = LoremNET.Lorem.Words(10);
        var courseShortName = courseFullName.Replace(" ", String.Empty);

        return new CourseModel(courseFullName, courseShortName, 1);
    }));

var result = await moodleApiWrapper.CreateCourses(courses.ToArray());

var res = JsonConvert.SerializeObject(result);

Console.WriteLine(res);

Console.ReadKey();