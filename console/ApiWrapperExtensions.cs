using MoodleApiWrapper;

using Newtonsoft.Json;

namespace Console
{
    public static class ApiWrapperExtensions
    {
        public static async Task CreateCourses(this ApiWrapper moodleApiWrapper, int courseCount, int batchSize = 5)
        {
            var courses = new List<CourseModel>(
            Enumerable.Range(0, courseCount).Select(i =>
            {
                var courseFullName = LoremNET.Lorem.Words(10);
                var courseShortName = courseFullName.Replace(" ", String.Empty);

                return new CourseModel(courseFullName, courseShortName, 1);
            }));

            for (int i = 0; i < courses.Count; i += batchSize)
            {
                var result = await moodleApiWrapper.CreateCourses(courses.Skip(i).Take(batchSize).ToArray());

                var res = JsonConvert.SerializeObject(result);

                System.Console.WriteLine(res);
            }
        }

        public static async Task DeleteCourses(this ApiWrapper moodleApiWrapper, int batchSize = 15)
        {
            var getCourses = await moodleApiWrapper.GetCourses();

            var courseAzonositok = getCourses.DataArray.Select(c => c.id).ToArray();

            for (int i = 0; i < courseAzonositok.Length; i += batchSize)
            {
                var result = await moodleApiWrapper.DeleteCourses(courseAzonositok.Skip(i).Take(batchSize).ToArray());

                var res = JsonConvert.SerializeObject(result);

                System.Console.WriteLine(res);
            }
        }
    }
}
