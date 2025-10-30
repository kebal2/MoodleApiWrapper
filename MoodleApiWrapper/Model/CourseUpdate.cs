#nullable enable
namespace MoodleApiWrapper.Model;

public class CourseUpdate : CourseOptionalProperties
{
    public string? fullname { get; set; }
    public string? shortname { get; set; }
    public int? categoryid { get; set; }
}
