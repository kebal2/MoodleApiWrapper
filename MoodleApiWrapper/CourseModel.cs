using System;

namespace MoodleApiWrapper;

public class CourseModel
{
    public readonly string Fullname;
    public readonly string Shortname;
    public readonly int CategoryId;

    public CourseModel(string fullname, string shortname, int categoryId)
    {
        this.Fullname = fullname;
        this.Shortname = shortname;
        this.CategoryId = categoryId;
    }

    public string idnumber = "";
    public string summary = "";
    public int summaryformat = 1;
    public string format = "";
    public int showgrades = 0;
    public int newsitems = 0;
    public DateTime startdate = default;
    public int numsections = int.MaxValue;
    public int maxbytes = 104857600;
    public int showreports = 1;
    public int visible = 0;
    public int hiddensections = int.MaxValue;
    public int groupmode = 0;
    public int groupmodeforce = 0;
    public int defaultgroupingid = 0;
    public int enablecompletion = int.MaxValue;
    public int completenotify = 0;
    public string lang = "";
    public string forcetheme = "";
    public string courcCourseformatoption = "";
}