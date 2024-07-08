using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.Extensions.Options;
using MoodleApiWrapper.Model;
using MoodleApiWrapper.Options;

namespace MoodleApiWrapper;

internal static class Helpers
{
    internal static void AddField(this NameValueCollection query, List<string> list, string name, string value)
    {
        if (list == null || !list.Any()) return;

        for (var i = 0; i < list.Count; i++)
            query[$"users[0][{name}][{i}][{value}]"] = list[i];
    }
}

public class MoodleRequestBuilder
{
    private const string WebserviceEndpoint = "/webservice/rest/server.php";
    private const string LoginEndpoint = "/login/token.php";


    private readonly string apiToken;
    private readonly string host;

    public MoodleRequestBuilder(IOptions<Moodle> options)
    {
        if (string.IsNullOrEmpty(options.Value.ApiToken)) throw new ArgumentNullException(nameof(apiToken));
        if (string.IsNullOrEmpty(options.Value.Host)) throw new ArgumentNullException(nameof(apiToken));
        apiToken = options.Value.ApiToken;
        host = options.Value.Host;
    }

    public string DeleteCourses(int[] courseIds)
    {
        return GetUriFor(Methods.core_course_delete_courses, q =>
        {
            for (var i = 0; i < courseIds.Length; i++)
                q[$"courseids[{i}]"] = courseIds[i].ToString();
        });
    }

    public string GetApiToken(string username, string password, string serviceHostName)
    {
        return LoginEndpoint +
               $"?username={username}" +
               $"&password={password}" +
               $"&service={serviceHostName}";
    }

    public string GetSiteInfo(string serviceHostName = "")
    {
        return GetUriFor(Methods.core_webservice_get_site_info, q =>
        {
            if (serviceHostName.Any())
                q["serviceshortnames[0]"] = serviceHostName;
        });
    }

    public string GetUsers(object criteria)
    {
        return GetUriFor(Methods.core_user_get_users, q =>
        {
            for (var i = 0; i < criteria.GetType().GetFields().Length; i++)
            {
                var field = criteria.GetType().GetFields()[i];
                q[$"criteria[{i}][key]"] = field.Name;
                q[$"criteria[{i}][value]"] = field.GetValue(criteria).ToString();
            }
        });
    }

    public string GetUsers(UserFields field, string[] values)
    {
        return GetUriFor(Methods.core_user_get_users_by_field, q =>
        {
            q["field"] = field.ToString();
            int i = 0;
            foreach (var value in values)
            {
                q[$"values[{i++}]"] = value.ToLowerInvariant();
            }
        });
    }

    public string GetUser(UserFields field, string value)
    {
        return GetUriFor(Methods.core_user_get_users_by_field, q =>
        {
            q["field"] = field.ToString();
            q["values[]"] = value;
        });
    }

    public string GetUserCourses(int userid)
    {
        return GetUriFor(Methods.core_enrol_get_users_courses, query => query["userid"] = userid.ToString());
    }

    private void UserDataQueryBuilder(UserOptionalProperties userOptionalProperties, NameValueCollection query)
    {
        if (!string.IsNullOrEmpty(userOptionalProperties.auth)) query[$"users[0][{nameof(userOptionalProperties.auth)}]"] = userOptionalProperties.auth;
        if (!string.IsNullOrEmpty(userOptionalProperties.idnumber)) query[$"users[0][{nameof(userOptionalProperties.idnumber)}]"] = userOptionalProperties.idnumber;
        if (!string.IsNullOrEmpty(userOptionalProperties.lang)) query[$"users[0][{nameof(userOptionalProperties.lang)}]"] = userOptionalProperties.lang;
        if (!string.IsNullOrEmpty(userOptionalProperties.calendartye)) query[$"users[0][{nameof(userOptionalProperties.calendartye)}]"] = userOptionalProperties.calendartye;
        if (!string.IsNullOrEmpty(userOptionalProperties.theme)) query[$"users[0][{nameof(userOptionalProperties.theme)}]"] = userOptionalProperties.theme;
        if (!string.IsNullOrEmpty(userOptionalProperties.timezone)) query[$"users[0][{nameof(userOptionalProperties.timezone)}]"] = userOptionalProperties.timezone;
        if (!string.IsNullOrEmpty(userOptionalProperties.mailformat)) query[$"users[0][{nameof(userOptionalProperties.mailformat)}]"] = userOptionalProperties.mailformat;
        if (!string.IsNullOrEmpty(userOptionalProperties.description)) query[$"users[0][{nameof(userOptionalProperties.description)}]"] = userOptionalProperties.description;
        if (!string.IsNullOrEmpty(userOptionalProperties.city)) query[$"users[0][{nameof(userOptionalProperties.city)}]"] = userOptionalProperties.city;
        if (!string.IsNullOrEmpty(userOptionalProperties.country)) query[$"users[0][{nameof(userOptionalProperties.country)}]"] = userOptionalProperties.country;
        if (!string.IsNullOrEmpty(userOptionalProperties.firstnamephonetic)) query[$"users[0][{nameof(userOptionalProperties.firstnamephonetic)}]"] = userOptionalProperties.firstnamephonetic;
        if (!string.IsNullOrEmpty(userOptionalProperties.lastnamephonetic)) query[$"users[0][{nameof(userOptionalProperties.lastnamephonetic)}]"] = userOptionalProperties.lastnamephonetic;
        if (!string.IsNullOrEmpty(userOptionalProperties.middlename)) query[$"users[0][{nameof(userOptionalProperties.middlename)}]"] = userOptionalProperties.middlename;
        if (!string.IsNullOrEmpty(userOptionalProperties.alternatename)) query[$"users[0][{nameof(userOptionalProperties.alternatename)}]"] = userOptionalProperties.alternatename;

        if (!string.IsNullOrEmpty(userOptionalProperties.institution)) query[$"users[0][{nameof(userOptionalProperties.institution)}]"] = userOptionalProperties.institution;
        if (!string.IsNullOrEmpty(userOptionalProperties.department)) query[$"users[0][{nameof(userOptionalProperties.department)}]"] = userOptionalProperties.department;
        if (!string.IsNullOrEmpty(userOptionalProperties.phone1)) query[$"users[0][{nameof(userOptionalProperties.phone1)}]"] = userOptionalProperties.phone1;
        if (!string.IsNullOrEmpty(userOptionalProperties.phone2)) query[$"users[0][{nameof(userOptionalProperties.phone2)}]"] = userOptionalProperties.phone2;
        if (!string.IsNullOrEmpty(userOptionalProperties.address)) query[$"users[0][{nameof(userOptionalProperties.address)}]"] = userOptionalProperties.address;

        query.AddField(userOptionalProperties.preferences_type, "preferences", "type");
        query.AddField(userOptionalProperties.preferences_value, "preferences", "value");
        query.AddField(userOptionalProperties.customfields_type, "customfields", "type");
        query.AddField(userOptionalProperties.customfields_value, "customfields", "value");
    }


    public string CreateUser(UserData userOptionalProperties)
    {
        return GetUriFor(Methods.core_user_create_users, query =>
        {
            UserDataQueryBuilder(userOptionalProperties, query);

            query[$"users[0][{nameof(userOptionalProperties.username)}]"] = userOptionalProperties.username;
            query[$"users[0][{nameof(userOptionalProperties.password)}]"] = userOptionalProperties.password;
            query[$"users[0][{nameof(userOptionalProperties.firstname)}]"] = userOptionalProperties.firstname;
            query[$"users[0][{nameof(userOptionalProperties.lastname)}]"] = userOptionalProperties.lastname;
            query[$"users[0][{nameof(userOptionalProperties.email)}]"] = userOptionalProperties.email;
        });
    }

    public string UpdateUser(int id, UserData userOptionalProperties)
    {
        return GetUriFor(Methods.core_user_update_users, query =>
        {
            query[$"users[0][{nameof(id)}]"] = id.ToString();
            UserDataQueryBuilder(userOptionalProperties, query);

            if (!string.IsNullOrEmpty(userOptionalProperties.username)) query[$"users[0][{nameof(userOptionalProperties.username)}]"] = userOptionalProperties.username;
            // if (!string.IsNullOrEmpty(userOptionalProperties.password)) query[$"users[0][{nameof(userOptionalProperties.password)}]"] = userOptionalProperties.password;
            if (!string.IsNullOrEmpty(userOptionalProperties.firstname)) query[$"users[0][{nameof(userOptionalProperties.firstname)}]"] = userOptionalProperties.firstname;
            if (!string.IsNullOrEmpty(userOptionalProperties.lastname)) query[$"users[0][{nameof(userOptionalProperties.lastname)}]"] = userOptionalProperties.lastname;
            if (!string.IsNullOrEmpty(userOptionalProperties.email)) query[$"users[0][{nameof(userOptionalProperties.email)}]"] = userOptionalProperties.email;
        });
    }

    public string DeleteUser(int id)
    {
        return GetUriFor(Methods.core_user_delete_users, q => q["userids[0]"] = id.ToString());
    }

    public string AssignRoles(int roleId, int userId, string contextId = "", string contextLevel = "", int instanceId = int.MinValue)
    {
        return GetUriFor(Methods.core_role_assign_roles, query =>
        {
            query["assignments[0][roleid]"] = roleId.ToString();
            query["assignments[0][userid]"] = userId.ToString();
            if (contextId.Any()) query["assignments[0][contextid]"] = contextId;
            if (contextLevel.Any()) query["assignments[0][contextlevel]"] = contextLevel;
            if (instanceId != int.MinValue) query["assignments[0][instanceid]"] = instanceId.ToString();
        });
    }

    public string RevokeRoles(int roleId, int userId, string contextId = "",
        string contextLevel = "", int instanceId = int.MinValue)
    {
        return GetUriFor(Methods.core_role_unassign_roles, query =>
        {
            query["unassignments[0][roleid]"] = roleId.ToString();
            query["unassignments[0][userid]"] = userId.ToString();
            if (contextId.Any()) query["unassignments[0][contextid]"] = contextId;
            if (contextLevel.Any()) query["unassignments[0][contextlevel]"] = contextLevel;
            if (instanceId != int.MinValue) query["unassignments[0][instanceid]"] = instanceId.ToString();
        });
    }

    public string EnrolUser(int roleId, int userId, int courseId,
        int timeStart = int.MinValue, int timeEnd = int.MinValue, int suspend = int.MinValue)
    {
        return GetUriFor(Methods.enrol_manual_enrol_users, q =>
        {
            q["enrolments[0][roleid]"] = roleId.ToString();
            q["enrolments[0][userid]"] = userId.ToString();
            q["enrolments[0][courseid]"] = courseId.ToString();
            if (timeStart != int.MinValue) q["enrolments[0][timestart]"] = timeStart.ToString();
            if (timeEnd != int.MinValue) q["enrolments[0][timeend]"] = timeEnd.ToString();
            if (suspend != int.MinValue) q["enrolments[0][suspend]"] = suspend.ToString();
        });
    }

    public string AddGroupMember(int groupId, int userId)
    {
        return GetUriFor(Methods.core_group_add_group_members, q =>
        {
            q["members[0][groupid]"] = groupId.ToString();
            q["members[0][userid]"] = userId.ToString();
        });
    }

    public string DeleteGroupMember(int groupId, int userId)
    {
        return GetUriFor(Methods.core_group_delete_group_members, q =>
        {
            q["members[0][groupid]"] = groupId.ToString();
            q["members[0][userid]"] = userId.ToString();
        });
    }

    public string GetCategories(string criteriaKey, string criteriaValue, int addSubCategories = 1)
    {
        return GetUriFor(Methods.core_course_get_categories, q =>
        {
            q["criteria[0][key]"] = criteriaKey;
            q["criteria[0][value]"] = criteriaValue;
            if (addSubCategories != 1) q["addsubcategories"] = addSubCategories.ToString();
        });
    }

    public string GetCourses(int options = int.MinValue)
    {
        return GetUriFor(Methods.core_course_get_courses, q =>
        {
            if (options != int.MinValue) q["addsubcategories"] = options.ToString();
        });
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id">course id</param>
    /// <param name="ids">comma separated course ids</param>
    /// <param name="shortname"> course short name</param>
    /// <param name="idnumber">course id number</param>
    /// <param name="category">category id the course belongs to</param>
    /// <returns></returns>
    public string GetCourses(int? id, int[] ids, string? shortname, string? idnumber, int? category)
    {
        return GetUriFor(Methods.core_course_get_courses_by_field, q =>
        {
            if (shortname != null)
            {
                q["field"] = nameof(shortname);
                q["value"] = shortname;
            }
            else if (id != null)
            {
                q["field"] = nameof(id);
                q["value"] = id.ToString();
            }
            else if (ids != null)
            {
                q["field"] = nameof(ids);
                q["value"] = string.Join(",", ids);
            }
            else if (idnumber != null)
            {
                q["field"] = nameof(idnumber);
                q["value"] = idnumber;
            }
            else if (category != null)
            {
                q["field"] = nameof(category);
                q["value"] = category.ToString();
            }
        });
    }

    public string GetContents(int courseId)
    {
        return GetUriFor(Methods.core_course_get_contents, q => q["courseid"] = courseId.ToString());
    }

    public string GetGroup(int groupId)
    {
        return GetUriFor(Methods.core_group_get_groups, q => q["groupids[0]"] = groupId.ToString());
    }

    public string GetGroups(int[] groupIds)
    {
        return GetUriFor(Methods.core_group_get_groups, q =>
        {
            for (var i = 0; i < groupIds.Length; i++)
                q[$"groupids[{i}]"] = groupIds[i].ToString();
        });
    }

    public string GetCourseGroups(int courseId)
    {
        return GetUriFor(Methods.core_group_get_course_groups, q => q["courseid"] = courseId.ToString());
    }

    public string GetEnrolledUsersByCourse(int courseId)
    {
        return GetUriFor(Methods.core_enrol_get_enrolled_users, q => q["courseid"] = courseId.ToString());
    }

    private void CourseQueryBuilder(CourseOptionalProperties course, NameValueCollection query)
    {
        if (!string.IsNullOrEmpty(course.idnumber)) query[$"courses[0][{nameof(course.idnumber)}]"] = course.idnumber;
        if (!string.IsNullOrEmpty(course.summary)) query[$"courses[0][{nameof(course.summary)}]"] = course.summary;
        if (!string.IsNullOrEmpty(course.format)) query[$"courses[0][{nameof(course.format)}]"] = course.format;
        if (!string.IsNullOrEmpty(course.lang)) query[$"courses[0][{nameof(course.lang)}]"] = course.lang;
        if (!string.IsNullOrEmpty(course.forcetheme)) query[$"courses[0][{nameof(course.forcetheme)}]"] = course.forcetheme;

        if (course.completenotify != null) query[$"courses[0][{nameof(course.completenotify)}]"] = course.completenotify.ToString();
        if (course.defaultgroupingid != null) query[$"courses[0][{nameof(course.defaultgroupingid)}]"] = course.defaultgroupingid.ToString();
        if (course.enablecompletion != null) query[$"courses[0][{nameof(course.enablecompletion)}]"] = course.enablecompletion.ToString();
        if (course.groupmode != null) query[$"courses[0][{nameof(course.groupmode)}]"] = course.groupmode.ToString();
        if (course.groupmodeforce != null) query[$"courses[0][{nameof(course.groupmodeforce)}]"] = course.groupmodeforce.ToString();
        if (course.hiddensections != null) query[$"courses[0][{nameof(course.hiddensections)}]"] = course.hiddensections.ToString();
        if (course.maxbytes != null) query[$"courses[0][{nameof(course.maxbytes)}]"] = course.maxbytes.ToString();
        if (course.newsitems != null) query[$"courses[0][{nameof(course.newsitems)}]"] = course.newsitems.ToString();
        if (course.numsections != null) query[$"courses[0][{nameof(course.numsections)}]"] = course.numsections.ToString();
        if (course.showgrades != null) query[$"courses[0][{nameof(course.showgrades)}]"] = course.showgrades.ToString();
        if (course.showreports != null) query[$"courses[0][{nameof(course.showreports)}]"] = course.showreports.ToString();
        if (course.summaryformat != null) query[$"courses[0][{nameof(course.summaryformat)}]"] = course.summaryformat.ToString();
        if (course.visible != null) query[$"courses[0][{nameof(course.visible)}]"] = course.visible.ToString();

        if (course.startdate != null && !course.startdate.Equals(default)) query[$"courses[0][{nameof(course.startdate)}]"] = course.startdate.Value.ToUnixTimestamp().ToString();
    }

    public string CreateCourse(CourseCreate course)
    {
        return GetUriFor(Methods.core_course_create_courses, q =>
        {
            q[$"courses[0][{nameof(course.fullname)}]"] = course.fullname;
            q[$"courses[0][{nameof(course.shortname)}]"] = course.shortname;
            q[$"courses[0][{nameof(course.categoryid)}]"] = course.categoryid.ToString();
            CourseQueryBuilder(course, q);
        });
    }

    public string CreateCourses(CourseCreate[] courses, int[] categoryIds = default)
    {
        return GetUriFor(Methods.core_course_create_courses, q =>
        {
            for (var i = 0; i < courses.Length; i++)
            {
                var course = courses[i];

                q[$"courses[{i}][{nameof(course.fullname)}]"] = course.fullname;
                q[$"courses[{i}][{nameof(course.shortname)}]"] = course.shortname;
                q[$"courses[{i}][{nameof(course.categoryid)}]"] = course.categoryid.ToString();
                CourseQueryBuilder(course, q);
            }
        });
    }

    public string UpdateCourse(int id, CourseUpdate course)
    {
        return GetUriFor(Methods.core_course_update_courses, q =>
        {
            q[$"courses[0][id]"] = id.ToString();
            if (!string.IsNullOrEmpty(course.fullname)) q[$"courses[0][{nameof(course.fullname)}]"] = course.fullname;
            if (!string.IsNullOrEmpty(course.shortname)) q[$"courses[0][{nameof(course.shortname)}]"] = course.shortname;
            if (course.categoryid != null) q[$"courses[0][{nameof(course.categoryid)}]"] = course.categoryid.ToString();
            CourseQueryBuilder(course, q);
        });
    }

    public string GetGrades(int courseId, string component = "", int activityId = int.MaxValue, string[] userIds = null)
    {
        return GetUriFor(Methods.core_grades_get_grades, q =>
        {
            q["courseid"] = courseId.ToString();
            if (component.Any()) q["component"] = component;
            if (activityId != int.MaxValue) q["activityid"] = activityId.ToString();
            if (userIds != null) q["userids"] = string.Join(",", userIds);
        });
    }

    public string GetCalendarEvents(int[] groupIds = default, int[] courseIds = default, int[] eventids = default)
    {
        return GetUriFor(Methods.core_calendar_get_calendar_events, q =>
        {
            if (groupIds != null)
                for (var i = 0; i < groupIds.Length; i++)
                    q[$"events[groupids][{i}]"] = groupIds[i].ToString();

            if (courseIds != null)
                for (var i = 0; i < courseIds.Length; i++)
                    q["events[courseids][{i}]"] = courseIds[i].ToString();

            if (eventids != null)
                for (var i = 0; i < eventids.Length; i++)
                    q["events[eventids][{i}]"] = eventids[i].ToString();
        });
    }

    public string CreateCalendarEvents(string[] names, string[] descriptions = default,
        int[] formats = default, int[] groupIds = default, int[] courseIds = default, int[] repeats = default,
        string[] eventTypes = default, DateTime[] timeStarts = default, TimeSpan[] timeDurations = default,
        int[] visible = default, int[] sequences = default)
    {
        return GetUriFor(Methods.core_calendar_create_calendar_events, q =>
        {
            for (var i = 0; i < names.Length; i++)
                q[$"events[{i}][name]"] = names[i];

            if (groupIds != null)
                for (var i = 0; i < groupIds.Length; i++)
                    q[$"events[{i}][groupid]"] = groupIds[i].ToString();

            if (courseIds != null)
                for (var i = 0; i < courseIds.Length; i++)
                    q[$"events[{i}][courseid]"] = courseIds[i].ToString();

            if (descriptions != null)
                for (var i = 0; i < descriptions.Length; i++)
                    q[$"events[{i}][description]"] = descriptions[i];

            if (formats != null)
                for (var i = 0; i < formats.Length; i++)
                    q[$"events[{i}][format]"] = formats[i].ToString();

            if (repeats != null)
                for (var i = 0; i < repeats.Length; i++)
                    q[$"events[{i}][repeats]"] = repeats[i].ToString();

            if (timeStarts != null)
                for (var i = 0; i < timeStarts.Length; i++)
                    q[$"events[{i}][timestart]"] = timeStarts[i].ToUnixTimestamp().ToString();

            if (timeDurations != null)
                for (var i = 0; i < timeDurations.Length; i++)
                    q[$"events[{i}][timeduration]"] = timeDurations[i].TotalSeconds.ToString(CultureInfo.InvariantCulture);

            if (visible != null)
                for (var i = 0; i < visible.Length; i++)
                    q[$"events[{i}][visible]"] = visible[i].ToString();

            if (sequences != null)
                for (var i = 0; i < sequences.Length; i++)
                    q[$"events[{i}][sequence]"] = sequences[i].ToString();
        });
    }

    public string DeleteCalendarEvents(int[] eventids, int[] repeats, string[] descriptions = default)
    {
        return GetUriFor(Methods.core_calendar_delete_calendar_events, q =>
        {
            if (repeats != null)
                for (var i = 0; i < repeats.Length; i++)
                    q[$"events[{i}][repeat]"] = repeats[i].ToString();

            if (eventids != null)
                for (var i = 0; i < eventids.Length; i++)
                    q[$"events[{i}][eventid]"] = eventids[i].ToString();

            if (descriptions != null)
                for (var i = 0; i < descriptions.Length; i++)
                    q[$"events[{i}][description]"] = descriptions[i];
        });
    }

    public string CreateGroups(string[] names = default, int[] courseIds = default, string[] descriptions = default,
        int[] descriptionFormats = default, string[] enrolmentKeys = default, string[] idNumbers = default)
    {
        return GetUriFor(Methods.core_group_create_groups, q =>
        {
            if (names != null)
                for (var i = 0; i < names.Length; i++)
                    q[$"groups[{i}][name]"] = names[i];

            if (courseIds != null)
                for (var i = 0; i < courseIds.Length; i++)
                    q[$"groups[{i}][courseid]"] = courseIds[i].ToString();

            if (descriptions != null)
                for (var i = 0; i < descriptions.Length; i++)
                    q[$"groups[{i}][description]"] = descriptions[i];

            if (descriptionFormats != null)
                for (var i = 0; i < descriptionFormats.Length; i++)
                    q[$"groups[{i}][descriptionformat]"] = descriptionFormats[i].ToString();

            if (enrolmentKeys != null)
                for (var i = 0; i < enrolmentKeys.Length; i++)
                    q[$"groups[{i}][enrolmentkey]"] = enrolmentKeys[i];

            if (idNumbers != null)
                for (var i = 0; i < idNumbers.Length; i++)
                    q[$"groups[{i}][idnumber]"] = idNumbers[i];
        });
    }

    private string GetUriFor(Methods method, Action<NameValueCollection> queryCallback, Format format = Format.json)
    {
        var uriBuilder = new UriBuilder(host.TrimEnd('/'));
        uriBuilder.Path += WebserviceEndpoint;

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query["wstoken"] = apiToken;
        query["wsfunction"] = method.ToString();
        query["moodlewsrestformat"] = format.ToString();

        queryCallback(query);

        uriBuilder.Query = query.ToString();

        return uriBuilder.ToString();
    }
}
