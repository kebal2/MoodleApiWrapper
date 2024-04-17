namespace MoodleApiWrapper;

public class GetCourseResult : IDataModel
{
    
    public Course[] courses { get; set; }
    public Warning[] warnings { get; set; }
}