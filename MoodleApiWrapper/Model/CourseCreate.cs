namespace MoodleApiWrapper.Model;

public class CourseCreate : CourseOptionalProperties
{
    public readonly string fullname;
    public readonly string shortname;
    public readonly int categoryid;

    public CourseCreate(string fullname, string shortname, int categoryid)
    {
        this.fullname = fullname;
        this.shortname = shortname;
        this.categoryid = categoryid;
    }
}
