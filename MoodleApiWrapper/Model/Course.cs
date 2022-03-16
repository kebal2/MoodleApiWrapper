using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace MoodleApiWrapper;

public class Course : ICloneable, IDataModel
{
    [JsonConstructor]
    internal Course(int id, string shortname, int categoryid, int categorysortorder, string fullname, string displayname, string idnumber, string summary, int summaryformat, string format, int showgrades, int newsitems, int startdate,
        int numsections, int maxbytes, int showreports, int visible, int groupmode, int groupmodeforce, int defaultgroupingid, int timecreated, int timemodified, int enablecompletion, int completionnotify, string lang,
        string forcetheme, List<Courseformatoption> courseformatoptions, int? hiddensections)
    {
        this.id = id;
        this.shortname = shortname;
        this.categoryid = categoryid;
        this.categorysortorder = categorysortorder;
        this.fullname = fullname;
        this.displayname = displayname;
        this.idnumber = idnumber;
        this.summary = summary;
        this.summaryformat = summaryformat;
        this.format = format;
        this.showgrades = showgrades;
        this.newsitems = newsitems;
        this.startdate = startdate;
        this.numsections = numsections;
        this.maxbytes = maxbytes;
        this.showreports = showreports;
        this.visible = visible;
        this.groupmode = groupmode;
        this.groupmodeforce = groupmodeforce;
        this.defaultgroupingid = defaultgroupingid;
        this.timecreated = timecreated;
        this.timemodified = timemodified;
        this.enablecompletion = enablecompletion;
        this.completionnotify = completionnotify;
        this.lang = lang;
        this.forcetheme = forcetheme;
        this.courseformatoptions = courseformatoptions;
        this.hiddensections = hiddensections;
    }

    public int id { get; set; }
    public string shortname { get; set; }
    public int categoryid { get; set; }
    public int categorysortorder { get; set; }
    public string fullname { get; set; }
    public string displayname { get; set; }
    public string idnumber { get; set; }
    public string summary { get; set; }
    public int summaryformat { get; set; }
    public string format { get; set; }
    public int showgrades { get; set; }
    public int newsitems { get; set; }
    public int startdate { get; set; }
    public int numsections { get; set; }
    public int maxbytes { get; set; }
    public int showreports { get; set; }
    public int visible { get; set; }
    public int groupmode { get; set; }
    public int groupmodeforce { get; set; }
    public int defaultgroupingid { get; set; }
    public int timecreated { get; set; }
    public int timemodified { get; set; }
    public int enablecompletion { get; set; }
    public int completionnotify { get; set; }
    public string lang { get; set; }
    public string forcetheme { get; set; }
    public List<Courseformatoption> courseformatoptions { get; set; }
    public int? hiddensections { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}