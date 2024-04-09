using System;
using System.Linq;
using System.Text;

using Microsoft.Extensions.Options;

using MoodleApiWrapper.Model;
using MoodleApiWrapper.Options;

namespace MoodleApiWrapper;

public class MoodleRequestBuilder
{
    private readonly string apiToken;

    public MoodleRequestBuilder(IOptions<Moodle> options)
    {
        if (string.IsNullOrEmpty(options.Value.ApiToken)) throw new ArgumentNullException(nameof(apiToken));
        apiToken = options.Value.ApiToken;
    }

    public string DeleteCourses(int[] courseIds)
    {
        var query = GetDefaultQuery(Methods.core_course_delete_courses);

        for (var i = 0; i < courseIds.Length; i++)
            query.Append($"&courseids[{i}]={courseIds[i]}");

        return query.ToString();
    }

    public string GetApiToken(string username, string password, string serviceHostName)
    {
        return "login/token.php" +
               $"?username={username}" +
               $"&password={password}" +
               $"&service={serviceHostName}";
    }

    public string GetSiteInfo(string serviceHostName = "")
    {
        var query = GetDefaultQuery(Methods.core_webservice_get_site_info);

        if (serviceHostName.Any())
            query.Append($"&serviceshortnames[0]={serviceHostName}");

        return query.ToString();
    }

    public string GetUsers(object criteria)
    {
        var query = GetDefaultQuery(Methods.core_user_get_users);
        var index = 0;
        foreach (var field in criteria.GetType().GetFields())
            query.Append(
                $"&criteria[{index}][key]={field.Name}" +
                $"&criteria[{index++}][value]={field.GetValue(criteria)}");

        return query.ToString();
    }

    public string GetUserCourses(int userid)
    {
        var query = GetDefaultQuery(Methods.core_enrol_get_users_courses);
        query.Append($"&userid={userid}");

        return query.ToString();
    }

    private void UserDataQueryBuilder(UserOptionalProperties userOptionalProperties, StringBuilder query)
    {
        if (!string.IsNullOrEmpty(userOptionalProperties.auth)) query.Append($"&users[0][{nameof(userOptionalProperties.auth)}]={userOptionalProperties.auth}");
        if (!string.IsNullOrEmpty(userOptionalProperties.idnumber)) query.Append($"&user[0][{nameof(userOptionalProperties.idnumber)}]={userOptionalProperties.idnumber}");
        if (!string.IsNullOrEmpty(userOptionalProperties.lang)) query.Append($"&user[0][{nameof(userOptionalProperties.lang)}]={userOptionalProperties.lang}");
        if (!string.IsNullOrEmpty(userOptionalProperties.calendartye)) query.Append($"&user[0][{nameof(userOptionalProperties.calendartye)}]={userOptionalProperties.calendartye}");
        if (!string.IsNullOrEmpty(userOptionalProperties.theme)) query.Append($"&user[0][{nameof(userOptionalProperties.theme)}]={userOptionalProperties.theme}");
        if (!string.IsNullOrEmpty(userOptionalProperties.timezone)) query.Append($"&user[0][{nameof(userOptionalProperties.timezone)}]={userOptionalProperties.timezone}");
        if (!string.IsNullOrEmpty(userOptionalProperties.mailformat)) query.Append($"&user[0][{nameof(userOptionalProperties.mailformat)}]={userOptionalProperties.mailformat}");
        if (!string.IsNullOrEmpty(userOptionalProperties.description)) query.Append($"&user[0][{nameof(userOptionalProperties.description)}]={userOptionalProperties.description}");
        if (!string.IsNullOrEmpty(userOptionalProperties.city)) query.Append($"&user[0][{nameof(userOptionalProperties.city)}]={userOptionalProperties.city}");
        if (!string.IsNullOrEmpty(userOptionalProperties.country)) query.Append($"&user[0][{nameof(userOptionalProperties.country)}]={userOptionalProperties.country}");
        if (!string.IsNullOrEmpty(userOptionalProperties.firstnamephonetic)) query.Append($"&user[0][{nameof(userOptionalProperties.firstnamephonetic)}]={userOptionalProperties.firstnamephonetic}");
        if (!string.IsNullOrEmpty(userOptionalProperties.lastnamephonetic)) query.Append($"&user[0][{nameof(userOptionalProperties.lastnamephonetic)}]={userOptionalProperties.lastnamephonetic}");
        if (!string.IsNullOrEmpty(userOptionalProperties.middlename)) query.Append($"&user[0][{nameof(userOptionalProperties.middlename)}]={userOptionalProperties.middlename}");
        if (!string.IsNullOrEmpty(userOptionalProperties.alternatename)) query.Append($"&user[0][{nameof(userOptionalProperties.alternatename)}]={userOptionalProperties.alternatename}");
        if (!string.IsNullOrEmpty(userOptionalProperties.preferences_type)) query.Append($"&user[0][{nameof(userOptionalProperties.preferences_type)}]={userOptionalProperties.preferences_type}");
        if (!string.IsNullOrEmpty(userOptionalProperties.preferences_value)) query.Append($"&user[0][{nameof(userOptionalProperties.preferences_value)}]={userOptionalProperties.preferences_value}");
        if (!string.IsNullOrEmpty(userOptionalProperties.customfields_type)) query.Append($"&user[0][{nameof(userOptionalProperties.customfields_type)}]={userOptionalProperties.customfields_type}");
        if (!string.IsNullOrEmpty(userOptionalProperties.customfields_value)) query.Append($"&user[0][{nameof(userOptionalProperties.customfields_value)}]={userOptionalProperties.customfields_value}");
    }

    public string CreateUser(UserCreate userOptionalProperties)
    {
        var query = GetDefaultQuery(Methods.core_user_create_users);
        query.Append(
            $"&users[0][{nameof(userOptionalProperties.username)}]={userOptionalProperties.username}" +
            $"&users[0][{nameof(userOptionalProperties.password)}]={userOptionalProperties.password}" +
            $"&users[0][{nameof(userOptionalProperties.firstname)}]={userOptionalProperties.firstname}" +
            $"&users[0][{nameof(userOptionalProperties.lastname)}]={userOptionalProperties.lastname}" +
            $"&users[0][{nameof(userOptionalProperties.email)}]={userOptionalProperties.email}");

        UserDataQueryBuilder(userOptionalProperties, query);

        return query.ToString();
    }

    public string UpdateUser(int id, UserUpdate userOptionalProperties)
    {
        var query = GetDefaultQuery(Methods.core_user_update_users);
        query.Append($"&users[0][id]={id}");

        if (!string.IsNullOrEmpty(userOptionalProperties.username)) query.Append($"&users[0][{nameof(userOptionalProperties.username)}]={userOptionalProperties.username}");
        if (!string.IsNullOrEmpty(userOptionalProperties.password)) query.Append($"&users[0][{nameof(userOptionalProperties.password)}]={userOptionalProperties.password}");
        if (!string.IsNullOrEmpty(userOptionalProperties.firstname)) query.Append($"&users[0][{nameof(userOptionalProperties.firstname)}]={userOptionalProperties.firstname}");
        if (!string.IsNullOrEmpty(userOptionalProperties.lastname)) query.Append($"&users[0][{nameof(userOptionalProperties.lastname)}]={userOptionalProperties.lastname}");
        if (!string.IsNullOrEmpty(userOptionalProperties.email)) query.Append($"&users[0][{nameof(userOptionalProperties.email)}]={userOptionalProperties.email}");

        UserDataQueryBuilder(userOptionalProperties, query);

        return query.ToString();
    }

    public string DeleteUser(int id)
    {
        var query = GetDefaultQuery(Methods.core_user_delete_users);
        query.Append($"&userids[0]={id}");

        return query.ToString();
    }

    public string AssignRoles(int roleId, int userId, string contextId = "",
        string contextLevel = "", int instanceId = int.MinValue)
    {
        var query = GetDefaultQuery(Methods.core_role_assign_roles);
        query.Append(
            $"&assignments[0][roleid]={roleId}" +
            $"&assignments[0][userid]={userId}");
        if (contextId.Any()) query.Append($"&assignments[0][contextid]={contextId}");
        if (contextLevel.Any()) query.Append($"&assignments[0][contextlevel]={contextLevel}");
        if (instanceId != int.MinValue) query.Append($"&assignments[0][instanceid]={instanceId}");

        return query.ToString();
    }

    public string RevokeRoles(int roleId, int userId, string contextId = "",
        string contextLevel = "", int instanceId = int.MinValue)
    {
        var query = GetDefaultQuery(Methods.core_role_unassign_roles);
        query.Append(
            $"&unassignments[0][roleid]={roleId}" +
            $"&unassignments[0][userid]={userId}");
        if (contextId.Any()) query.Append($"&unassignments[0][contextid]={contextId}");
        if (contextLevel.Any()) query.Append($"&unassignments[0][contextlevel]={contextLevel}");
        if (instanceId != int.MinValue) query.Append($"&unassignments[0][instanceid]={instanceId}");

        return query.ToString();
    }

    public string EnrolUser(int roleId, int userId, int courseId,
        int timeStart = int.MinValue, int timeEnd = int.MinValue, int suspend = int.MinValue)
    {
        var query = GetDefaultQuery(Methods.enrol_manual_enrol_users);
        query.Append(
            $"&enrolments[0][roleid]={roleId}" +
            $"&enrolments[0][userid]={userId}" +
            $"&enrolments[0][courseid]={courseId}");
        if (timeStart != int.MinValue) query.Append($"&enrolments[0][timestart]={timeStart}");
        if (timeEnd != int.MinValue) query.Append($"&enrolments[0][timeend]={timeEnd}");
        if (suspend != int.MinValue) query.Append($"&enrolments[0][suspend]={suspend}");

        return query.ToString();
    }

    public string AddGroupMember(int groupId, int userId)
    {
        var query = GetDefaultQuery(Methods.core_group_add_group_members);
        query.Append(
            $"&members[0][groupid]={groupId}" +
            $"&members[0][userid]={userId}");

        return query.ToString();
    }

    public string DeleteGroupMember(int groupId, int userId)
    {
        var query = GetDefaultQuery(Methods.core_group_delete_group_members);
        query.Append(
            $"&members[0][groupid]={groupId}" +
            $"&members[0][userid]={userId}");

        return query.ToString();
    }

    public string GetCategories(string criteriaKey, string criteriaValue, int addSubCategories = 1)
    {
        var query = GetDefaultQuery(Methods.core_course_get_categories);
        query.Append(
            $"&criteria[0][key]={criteriaKey}" +
            $"&criteria[0][value]={criteriaValue}");

        if (addSubCategories != 1) query.Append($"&addsubcategories={addSubCategories}");

        return query.ToString();
    }

    public string GetCourses(int options = int.MinValue)
    {
        var query = GetDefaultQuery(Methods.core_course_get_courses);

        if (options != int.MinValue) query.Append($"&addsubcategories={options}");

        return query.ToString();
    }

    public string GetContents(int courseId)
    {
        var query = GetDefaultQuery(Methods.core_course_get_contents);
        query.Append($"&courseid={courseId}");

        return query.ToString();
    }

    public string GetGroup(int groupId)
    {
        var query = GetDefaultQuery(Methods.core_group_get_groups);
        query.Append($"&groupids[0]={groupId}");

        return query.ToString();
    }

    public string GetGroups(int[] groupIds)
    {
        var query = GetDefaultQuery(Methods.core_group_get_groups);

        for (var i = 0; i < groupIds.Length; i++)
            query.Append($"&groupids[{i}]={groupIds[i]}");

        return query.ToString();
    }

    public string GetCourseGroups(int courseId)
    {
        var query = GetDefaultQuery(Methods.core_group_get_course_groups);
        query.Append($"&courseid={courseId}");

        return query.ToString();
    }

    public string GetEnrolledUsersByCourse(int courseId)
    {
        var query = GetDefaultQuery(Methods.core_enrol_get_enrolled_users);
        query.Append($"&courseid={courseId}");

        return query.ToString();
    }

    private void CourseQueryBuilder(CourseOptionalProperties course, StringBuilder query)
    {
        if (!string.IsNullOrEmpty(course.idnumber)) query.Append($"&courses[0][{nameof(course.idnumber)}]={course.idnumber}");
        if (!string.IsNullOrEmpty(course.summary)) query.Append($"&courses[0][{nameof(course.summary)}]={course.summary}");
        if (!string.IsNullOrEmpty(course.format)) query.Append($"&courses[0][{nameof(course.format)}]={course.format}");
        if (!string.IsNullOrEmpty(course.lang)) query.Append($"&courses[0][{nameof(course.lang)}]={course.lang}");
        if (!string.IsNullOrEmpty(course.forcetheme)) query.Append($"&courses[0][{nameof(course.forcetheme)}]={course.forcetheme}");

        if (course.completenotify != null) query.Append($"&courses[0][{nameof(course.completenotify)}]={course.completenotify}");
        if (course.defaultgroupingid != null) query.Append($"&courses[0][{nameof(course.defaultgroupingid)}]={course.defaultgroupingid}");
        if (course.enablecompletion != null) query.Append($"&courses[0][{nameof(course.enablecompletion)}]={course.enablecompletion}");
        if (course.groupmode != null) query.Append($"&courses[0][{nameof(course.groupmode)}]={course.groupmode}");
        if (course.groupmodeforce != null) query.Append($"&courses[0][{nameof(course.groupmodeforce)}]={course.groupmodeforce}");
        if (course.hiddensections != null) query.Append($"&courses[0][{nameof(course.hiddensections)}]={course.hiddensections}");
        if (course.maxbytes != null) query.Append($"&courses[0][{nameof(course.maxbytes)}]={course.maxbytes}");
        if (course.newsitems != null) query.Append($"&courses[0][{nameof(course.newsitems)}]={course.newsitems}");
        if (course.numsections != null) query.Append($"&courses[0][{nameof(course.numsections)}]={course.numsections}");
        if (course.showgrades != null) query.Append($"&courses[0][{nameof(course.showgrades)}]={course.showgrades}");
        if (course.showreports != null) query.Append($"&courses[0][{nameof(course.showreports)}]={course.showreports}");
        if (course.summaryformat != null) query.Append($"&courses[0][{nameof(course.summaryformat)}]={course.summaryformat}");
        if (course.visible != null) query.Append($"&courses[0][{nameof(course.visible)}]={course.visible}");

        if (course.startdate != null && !course.startdate.Equals(default)) query.Append($"&courses[0][{nameof(course.startdate)}]={course.startdate.Value.ToUnixTimestamp()}");
    }

    public string CreateCourse(CourseCreate course)
    {
        var query = GetDefaultQuery(Methods.core_course_create_courses);
        query.Append(
            $"&courses[0][{nameof(course.fullname)}]={course.fullname}" +
            $"&courses[0][{nameof(course.shortname)}]={course.shortname}" +
            $"&courses[0][{nameof(course.categoryid)}]={course.categoryid}");

        CourseQueryBuilder(course, query);

        return query.ToString();
    }

    public string CreateCourses(CourseCreate[] courses, int[] categoryIds = default)
    {
        var query = GetDefaultQuery(Methods.core_course_create_courses);

        for (var i = 0; i < courses.Length; i++)
        {
            var course = courses[i];

            query.Append($"&courses[{i}][{nameof(course.fullname).ToLower()}]={course.fullname}");
            query.Append($"&courses[{i}][{nameof(course.shortname).ToLower()}]={course.shortname}");
            query.Append($"&courses[{i}][{nameof(course.categoryid).ToLower()}]={course.categoryid}");
        }

        return query.ToString();
    }

    public string UpdateCourse(int id, CourseUpdate course)
    {
        var query = GetDefaultQuery(Methods.core_course_update_courses);
        query.Append($"&courses[0][id]={id}");

        if (!string.IsNullOrEmpty(course.fullname)) query.Append($"&courses[0][{nameof(course.fullname)}]={course.fullname}");
        if (!string.IsNullOrEmpty(course.shortname)) query.Append($"&courses[0][{nameof(course.shortname)}]={course.shortname}");
        if (course.categoryid != null) query.Append($"&courses[0][{nameof(course.categoryid)}]={course.categoryid}");

        CourseQueryBuilder(course, query);

        return query.ToString();
    }

    public string GetGrades(int courseId, string component = "", int activityId = int.MaxValue, string[] userIds = null)
    {
        var query = GetDefaultQuery(Methods.core_grades_get_grades);

        query.Append($"&courseid={courseId}");

        if (component.Any()) query.Append($"&component={component}");
        if (activityId != int.MaxValue) query.Append($"&activityid={activityId}");
        if (userIds != null) query.Append($"&userids={userIds}");
        if (component.Any()) query.Append($"&component={component}");

        return query.ToString();
    }

    public string GetCalendarEvents(int[] groupIds = default, int[] courseIds = default, int[] eventids = default)
    {
        var query = GetDefaultQuery(Methods.core_calendar_get_calendar_events);

        if (groupIds != null)
            for (var i = 0; i < groupIds.Length; i++)
                query.Append($"&events[groupids][{i}]={groupIds[i]}");

        if (courseIds != null)
            for (var i = 0; i < courseIds.Length; i++)
                query.Append($"&events[courseids][{i}]={courseIds[i]}");

        if (eventids != null)
            for (var i = 0; i < eventids.Length; i++)
                query.Append($"&events[eventids][{i}]={eventids[i]}");

        return query.ToString();
    }

    public string CreateCalendarEvents(string[] names, string[] descriptions = default,
        int[] formats = default, int[] groupIds = default, int[] courseIds = default, int[] repeats = default,
        string[] eventTypes = default, DateTime[]  timeStarts = default, TimeSpan[]  timeDurations = default,
        int[] visible = default, int[] sequences = default)
    {
        var query = GetDefaultQuery(Methods.core_calendar_create_calendar_events);

        for (var i = 0; i < names.Length; i++)
            query.Append($"&events[{i}][name]={names[i]}");

        if (groupIds != null)
            for (var i = 0; i < groupIds.Length; i++)
                query.Append($"&events[{i}][groupid]={groupIds[i]}");

        if (courseIds != null)
            for (var i = 0; i < courseIds.Length; i++)
                query.Append($"&events[{i}][courseid]={courseIds[i]}");

        if (descriptions != null)
            for (var i = 0; i < descriptions.Length; i++)
                query.Append($"&events[{i}][description]={descriptions[i]}");

        if (formats != null)
            for (var i = 0; i < formats.Length; i++)
                query.Append($"&events[{i}][format]={formats[i]}");

        if (repeats != null)
            for (var i = 0; i < repeats.Length; i++)
                query.Append($"&events[{i}][repeats]={repeats[i]}");

        if (timeStarts != null)
            for (var i = 0; i < timeStarts.Length; i++)
                query.Append($"&events[{i}][timestart]={timeStarts[i].ToUnixTimestamp()}");

        if (timeDurations != null)
            for (var i = 0; i < timeDurations.Length; i++)
                query.Append($"&events[{i}][timeduration]={timeDurations[i].TotalSeconds}");

        if (visible != null)
            for (var i = 0; i < visible.Length; i++)
                query.Append($"&events[{i}][visible]={visible[i]}");

        if (sequences != null)
            for (var i = 0; i < sequences.Length; i++)
                query.Append($"&events[{i}][sequence]={sequences[i]}");

        return query.ToString();
    }

    public string DeleteCalendarEvents(int[] eventids, int[] repeats, string[] descriptions = default)
    {
        var query = GetDefaultQuery(Methods.core_calendar_delete_calendar_events);

        if (repeats != null)
            for (var i = 0; i < repeats.Length; i++)
                query.Append($"&events[{i}][repeat]={repeats[i]}");

        if (eventids != null)
            for (var i = 0; i < eventids.Length; i++)
                query.Append($"&events[{i}][eventid]={eventids[i]}");


        if (descriptions != null)
            for (var i = 0; i < descriptions.Length; i++)
                query.Append($"&events[{i}][description]={descriptions[i]}");

        return query.ToString();
    }

    public string CreateGroups(string[] names = default, int[] courseIds = default, string[] descriptions = default,
        int[] descriptionFormats = default, string[] enrolmentKeys = default, string[] idNumbers = default)
    {
        var query = GetDefaultQuery(Methods.core_group_create_groups);

        if (names != null)
            for (var i = 0; i < names.Length; i++)
                query.Append($"&groups[{i}][name]={names[i]}");

        if (courseIds != null)
            for (var i = 0; i < courseIds.Length; i++)
                query.Append($"&groups[{i}][courseid]={courseIds[i]}");

        if (descriptions != null)
            for (var i = 0; i < descriptions.Length; i++)
                query.Append($"&groups[{i}][description]={descriptions[i]}");

        if (descriptionFormats != null)
            for (var i = 0; i < descriptionFormats.Length; i++)
                query.Append($"&groups[{i}][descriptionformat]={descriptionFormats[i]}");

        if (enrolmentKeys != null)
            for (var i = 0; i < enrolmentKeys.Length; i++)
                query.Append($"&groups[{i}][enrolmentkey]={enrolmentKeys[i]}");

        if (idNumbers != null)
            for (var i = 0; i < idNumbers.Length; i++)
                query.Append($"&groups[{i}][idnumber]={idNumbers[i]}");

        return query.ToString();
    }

    private StringBuilder GetDefaultQuery(Methods method, Format format = Format.json)
    {
        return new StringBuilder("webservice/rest/server.php?" +
                                 $"wstoken={apiToken}&" +
                                 $"wsfunction={method}&" +
                                 $"moodlewsrestformat={format}");
    }
}
