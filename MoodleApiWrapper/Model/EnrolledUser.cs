using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace MoodleApiWrapper;

public class EnrolledUser : IDataModel, ICloneable
{
    [JsonConstructor]
    internal EnrolledUser(int id, string username, string firstname, string lastname, string fullname, string email, string department, int firstaccess, int lastaccess, string description, int descriptionformat, string city,
        string country, string profileimageurlsmall, string profileimageurl, List<Group> groups, List<Role> roles, List<EnrolledCourse> enrolledcourses, List<Preference> preferences)
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
        this.city = city;
        this.country = country;
        this.profileimageurlsmall = profileimageurlsmall;
        this.profileimageurl = profileimageurl;
        this.groups = groups;
        this.roles = roles;
        this.enrolledcourses = enrolledcourses;
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
    public string city { get; set; }
    public string country { get; set; }
    public string profileimageurlsmall { get; set; }
    public string profileimageurl { get; set; }
    public List<Group> groups { get; set; }
    public List<Role> roles { get; set; }
    public List<EnrolledCourse> enrolledcourses { get; set; }
    public List<Preference> preferences { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}
