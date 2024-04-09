#nullable enable
using System;

namespace MoodleApiWrapper;

public abstract class CourseOptionalProperties
{
    public virtual string? idnumber { get; set; }
    public virtual string? summary { get; set; }
    public virtual int? summaryformat { get; set; }
    public virtual string? format { get; set; }
    public virtual int? showgrades { get; set; }
    public virtual int? newsitems { get; set; }
    public virtual DateTime? startdate { get; set; }
    public virtual int? numsections { get; set; }
    public virtual int? maxbytes { get; set; }
    public virtual int? showreports { get; set; }
    public virtual int? visible { get; set; }
    public virtual int? hiddensections { get; set; }
    public virtual int? groupmode { get; set; }
    public virtual int? groupmodeforce { get; set; }
    public virtual int? defaultgroupingid { get; set; }
    public virtual int? enablecompletion { get; set; }
    public virtual int? completenotify { get; set; }
    public virtual string? lang { get; set; }
    public virtual string? forcetheme { get; set; }
    public virtual string? courcCourseformatoption { get; set; }
}
