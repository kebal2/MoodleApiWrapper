namespace MoodleApiWrapper.Model;

public class GetCourseResult : IDataModel
{
    public Course[] courses { get; set; } = null!;
    public Warning[] warnings { get; set; } = null!;
}