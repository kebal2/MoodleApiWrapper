using MoodleApiWrapper;

using Newtonsoft.Json;

namespace Console
{
    public static class CourseGenerator
    {
        public static async Task CreateCourses(this MoodleApi moodleMoodleApi, int courseCount, int batchSize = 5)
        {
            var courses = new List<CourseCreate>(
            Enumerable.Range(0, courseCount).Select(i =>
            {
                var courseFullName = LoremNET.Lorem.Words(10);
                var courseShortName = courseFullName.Replace(" ", String.Empty);

                return new CourseCreate(courseFullName, courseShortName, 1);
            }));

            for (int i = 0; i < courses.Count; i += batchSize)
            {
                var result = await moodleMoodleApi.CreateCourses(courses.Skip(i).Take(batchSize).ToArray());

                var res = JsonConvert.SerializeObject(result);

                System.Console.WriteLine(res);
            }
        }

        public static async Task DeleteCourses(this MoodleApi moodleMoodleApi, int batchSize = 15)
        {
            var getCourses = await moodleMoodleApi.GetCourses();

            var courseIds = getCourses.DataArray.Select(c => c.id).ToArray();

            for (int i = 0; i < courseIds.Length; i += batchSize)
            {
                var result = await moodleMoodleApi.DeleteCourses(courseIds.Skip(i).Take(batchSize).ToArray());

                var res = JsonConvert.SerializeObject(result);

                System.Console.WriteLine(res);
            }
        }
    }
}
