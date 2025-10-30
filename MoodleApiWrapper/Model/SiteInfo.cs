using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace MoodleApiWrapper.Model;

/// <summary>
/// Represents the data associated to the site information
/// </summary>
public class SiteInfo : ICloneable, IDataModel
{
    public string sitename { get; set; }
    public string username { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string fullname { get; set; }
    public string lang { get; set; }
    public int userid { get; set; }
    public string siteurl { get; set; }
    public string userpictureurl { get; set; }
    public List<Function> functions { get; set; }
    public int downloadfiles { get; set; }
    public int uploadfiles { get; set; }
    public string release { get; set; }
    public string version { get; set; }
    public string mobilecssurl { get; set; }
    public List<Advancedfeature> advancedfeatures { get; set; }
    public bool usercanmanageownfiles { get; set; }
    public int userquota { get; set; }
    public int usermaxuploadfilesize { get; set; }
    public int userhomepage { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    [JsonConstructor]
    internal SiteInfo(string sitename, string username, string firstname, string lastname, string fullname,
        string lang, int userid, string siteurl, string userpictureurl, List<Function> functions, int downloadfiles,
        int uploadfiles, string release, string version, string mobilcssurl, List<Advancedfeature> advancedfeatures,
        bool usercanmanageownfiles, int userquota, int usermaxuploadfilesize, int userhomepage)
    {
        this.sitename = sitename;
        this.username = username;
        this.firstname = firstname;
        this.lastname = lastname;
        this.fullname = fullname;
        this.lang = lang;
        this.userid = userid;
        this.siteurl = siteurl;
        this.userpictureurl = userpictureurl;
        this.functions = functions;
        this.downloadfiles = downloadfiles;
        this.uploadfiles = uploadfiles;
        this.release = release;
        this.version = version;
        this.mobilecssurl = mobilcssurl;
        this.advancedfeatures = advancedfeatures;
        this.usercanmanageownfiles = usercanmanageownfiles;
        this.userquota = userquota;
        this.usermaxuploadfilesize = usermaxuploadfilesize;
        this.userhomepage = userhomepage;
    }
}
