using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace MoodleApiWrapper;

public class User : ICloneable, IDataModel
{
    [JsonConstructor]
    internal User(int id, string username, string firstname, string lastname, string fullname, string email, string department,
        int firstaccess, int lastaccess, string description, int descriptionformat, string profileimageurlsmall, string profileimageurl,
        string country, List<Customfield> customfields, List<Preference> preferences)
    {
        this.id = id;
        this.username = username;
        this.firstname = firstname;
        this.lastname = lastname;
        this.fullname = fullname;
        this.email = email;
        this.department = department;
        this.firstaccess = firstaccess;
        this.lastaccess = lastaccess;
        this.description = description;
        this.descriptionformat = descriptionformat;
        this.profileimageurlsmall = profileimageurlsmall;
        this.profileimageurl = profileimageurl;
        this.country = country;
        this.customfields = customfields;
        this.preferences = preferences;
    }

    public int id { get; set; }
    public string username { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string fullname { get; set; }
    public string email { get; set; }
    public string department { get; set; }
    public int firstaccess { get; set; }
    public int lastaccess { get; set; }
    public string description { get; set; }
    public int descriptionformat { get; set; }
    public string profileimageurlsmall { get; set; }
    public string profileimageurl { get; set; }
    public string country { get; set; }
    public List<Customfield> customfields { get; set; }
    public List<Preference> preferences { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
