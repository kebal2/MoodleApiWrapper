// See https://aka.ms/new-console-template for more information

using MoodleApiWrapper;

using Newtonsoft.Json;

var host = new Uri("http://localhost:8000/");
var apiToken = "80b8c01e3f20f01bde6d185669540c26";

var moodleApiWrapper = new ApiWrapper(host, apiToken);

var courses = new[]{new CourseModel("Alma", "Alma", 0)};

var result = await moodleApiWrapper.CreateCourses(courses);

var res = JsonConvert.SerializeObject(result);

Console.WriteLine(res);

Console.ReadKey();