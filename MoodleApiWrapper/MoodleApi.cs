using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MoodleApiWrapper.ApiResources;
using MoodleApiWrapper.Model;
using Newtonsoft.Json.Linq;

namespace MoodleApiWrapper;

//TODO: interface
public class MoodleApi
{
    private readonly HttpClient client;
    private readonly MoodleRequestBuilder mrb;

    public MoodleApi(HttpClient client, MoodleRequestBuilder mrb)
    {
        this.client = client;
        this.mrb = mrb;
    }

    public Task<ApiResponse<Success>> DeleteCourses(int[] courseIds, CancellationToken cancellationToken = default) => Get<Success>(mrb.DeleteCourses(courseIds), cancellationToken);

    public Task<AuthentiactionResponse<AuthToken>> GetApiToken(string username, string password, string serviceHostName, CancellationToken cancellationToken = default)
    {
        string query =
            "login/token.php" +
            $"?username={username}" +
            $"&password={password}" +
            $"&service={serviceHostName}";

        return GetAuth<AuthToken>(query, cancellationToken);
    }

    public Task<ApiResponse<SiteInfo>> GetSiteInfo(string serviceHostName = "", CancellationToken cancellationToken = default) => Get<SiteInfo>(mrb.GetSiteInfo(serviceHostName), cancellationToken);
    public Task<ApiResponse<Users>> GetUsers(object criteria, CancellationToken cancellationToken = default) => Get<Users>(mrb.GetUsers(criteria), cancellationToken);
    public Task<ApiResponse<User>> GetUser(UserFields field, string value, CancellationToken cancellationToken = default) => Get<User>(mrb.GetUser(field, value), cancellationToken);
    public Task<ApiResponse<User[]>> GetUsers(UserFields field, string[] values, CancellationToken cancellationToken = default) => Get<User[]>(mrb.GetUsers(field, values), cancellationToken);
    public Task<ApiResponse<Cources[]>> GetUserCourses(int userid, CancellationToken cancellationToken = default) => Get<Cources[]>(mrb.GetUserCourses(userid), cancellationToken);
    public Task<ApiResponse<NewUser>> CreateUser(UserCreate userOptionalProperties, CancellationToken cancellationToken = default) => Get<NewUser>(mrb.CreateUser(userOptionalProperties), cancellationToken);
    public Task<ApiResponse<Success>> UpdateUser(int id, UserUpdate userOptionalProperties, CancellationToken cancellationToken = default) => Get<Success>(mrb.UpdateUser(id, userOptionalProperties), cancellationToken);
    public Task<ApiResponse<Success>> DeleteUser(int id, CancellationToken cancellationToken = default) => Get<Success>(mrb.DeleteUser(id), cancellationToken);

    public Task<ApiResponse<Success>> AssignRoles(int roleId, int userId, string contextId = "", string contextLevel = "", int instanceId = Int32.MinValue, CancellationToken cancellationToken = default) =>
        Get<Success>(mrb.AssignRoles(roleId, userId, contextId, contextLevel, instanceId), cancellationToken);

    public Task<ApiResponse<Success>> RevokeRoles(int roleId, int userId, string contextId = "", string contextLevel = "", int instanceId = Int32.MinValue, CancellationToken cancellationToken = default) =>
        Get<Success>(mrb.RevokeRoles(roleId, userId, contextId, contextLevel, instanceId), cancellationToken);


    /// <param name="timeStart">UnixTimestamp</param>
    /// <param name="timeEnd">UnixTimestamp</param>
    public Task<ApiResponse<Success>> EnrolUser(int roleId, int userId, int courseId, int timeStart = Int32.MinValue, int timeEnd = Int32.MinValue, int suspend = Int32.MinValue, CancellationToken cancellationToken = default) =>
        Get<Success>(mrb.EnrolUser(roleId, userId, courseId, timeStart, timeEnd, suspend), cancellationToken);

    public Task<ApiResponse<Success>> AddGroupMember(int groupId, int userId, CancellationToken cancellationToken = default) => Get<Success>(mrb.AddGroupMember(groupId, userId), cancellationToken);
    public Task<ApiResponse<Success>> DeleteGroupMember(int groupId, int userId, CancellationToken cancellationToken = default) => Get<Success>(mrb.DeleteGroupMember(groupId, userId), cancellationToken);

    public Task<ApiResponse<Category[]>> GetCategories(string criteriaKey, string criteriaValue, int addSubCategories = 1, CancellationToken cancellationToken = default) =>
        Get<Category[]>(mrb.GetCategories(criteriaKey, criteriaValue, addSubCategories), cancellationToken);

    public Task<ApiResponse<Course[]>> GetCourses(int options = int.MinValue, CancellationToken cancellationToken = default) => Get<Course[]>(mrb.GetCourses(options), cancellationToken);

    public Task<ApiResponse<GetCourseResult>> GetCourses(int? id = null, int[] ids = null, string? shortname = null, string? idnumber = null, int? category = null, CancellationToken cancellationToken = default) =>
        Get<GetCourseResult>(mrb.GetCourses(id, ids, shortname, idnumber, category), cancellationToken);

    public Task<ApiResponse<Content[]>> GetContents(int courseId, CancellationToken cancellationToken = default) => Get<Content[]>(mrb.GetContents(courseId), cancellationToken);

    public Task<ApiResponse<Group>> GetGroup(int groupId, CancellationToken cancellationToken = default) => Get<Group>(mrb.GetGroup(groupId), cancellationToken);

    public Task<ApiResponse<Group[]>> GetGroups(int[] groupIds, CancellationToken cancellationToken = default) => Get<Group[]>(mrb.GetGroups(groupIds), cancellationToken);

    public Task<ApiResponse<Group>> GetCourseGroups(int courseId, CancellationToken cancellationToken = default) => Get<Group>(mrb.GetCourseGroups(courseId), cancellationToken);

    public Task<ApiResponse<EnrolledUser>> GetEnrolledUsersByCourse(int courseId, CancellationToken cancellationToken = default) => Get<EnrolledUser>(mrb.GetEnrolledUsersByCourse(courseId), cancellationToken);

    public Task<ApiResponse<NewCourse>> CreateCourse(CourseCreate course, CancellationToken cancellationToken = default) => Get<NewCourse>(mrb.CreateCourse(course), cancellationToken);
    public Task<ApiResponse<NewCourse[]>> CreateCourses(CourseCreate[] courses, int[] categoryIds = default, CancellationToken cancellationToken = default) => Get<NewCourse[]>(mrb.CreateCourses(courses, categoryIds), cancellationToken);

    public Task<ApiResponse<UpdateCourseRoot>> UpdateCourse(int id, CourseUpdate course, CancellationToken cancellationToken = default) => Get<UpdateCourseRoot>(mrb.UpdateCourse(id, course), cancellationToken);

    public Task<ApiResponse<Category[]>> GetGrades(int courseId, string component = "", int activityId = Int32.MaxValue, string[] userIds = null, CancellationToken cancellationToken = default) =>
        Get<Category[]>(mrb.GetGrades(courseId, component, activityId, userIds), cancellationToken);

    public Task<ApiResponse<Events[]>> GetCalendarEvents(int[] groupIds = default, int[] courseIds = default, int[] eventIds = default, CancellationToken cancellationToken = default) =>
        Get<Events[]>(mrb.GetCalendarEvents(groupIds, courseIds, eventIds), cancellationToken);

    public Task<ApiResponse<Events[]>> CreateCalendarEvents(string[] names, string[] descriptions = default,
        int[] formats = default, int[] groupIds = default, int[] courseIds = default, int[] repeats = default,
        string[] eventTypes = default, DateTime[] timeStarts = default, TimeSpan[] timeDurations = default,
        int[] visible = default, int[] sequences = default, CancellationToken cancellationToken = default) =>
        Get<Events[]>(mrb.CreateCalendarEvents(names, descriptions,
            formats, groupIds, courseIds, repeats,
            eventTypes, timeStarts, timeDurations,
            visible, sequences), cancellationToken);

    public Task<ApiResponse<Events[]>> DeleteCalendarEvents(int[] eventIds, int[] repeats, string[] descriptions = default, CancellationToken cancellationToken = default) =>
        Get<Events[]>(mrb.DeleteCalendarEvents(eventIds, repeats, descriptions), cancellationToken);

    public Task<ApiResponse<Group[]>> CreateGroups(string[] names = default, int[] courseIds = default, string[] descriptions = default,
        int[] descriptionFormats = default, string[] enrolmentKeys = default, string[] idNumbers = default, CancellationToken cancellationToken = default) =>
        Get<Group[]>(mrb.CreateGroups(names, courseIds, descriptions, descriptionFormats, enrolmentKeys, idNumbers), cancellationToken);

    private async Task<AuthentiactionResponse<T>> GetAuth<T>(string uri, CancellationToken cancellationToken) where T : IDataModel
    {
        try
        {
            using var response = await client.GetAsync(uri, cancellationToken);

            var responseStream = await response.Content.ReadAsStringAsync(cancellationToken);

            var data = JObject.Parse(responseStream);

            return new AuthentiactionResponse<T>(new AuthentiactionResponseRaw(data));
        }
        catch (WebException)
        {
            // No internet connection
            throw new WebException("No internet connection.");
        }
    }

    private async Task<ApiResponse<T>> Get<T>(string path, CancellationToken cancellationToken = default)
    {
        if (path.Length > 2000)
            throw new Exception("URI is too long should be split into multiple queries");

        using var response = await client.GetAsync(path, cancellationToken);

        if (!response.IsSuccessStatusCode) throw new WebException(await response.Content.ReadAsStringAsync(cancellationToken));

        var result = await response.Content.ReadAsStringAsync(cancellationToken);

        if (result.ToLower() == "null")
            result = "{IsSuccessful: true,}";

        ApiResponse<T> rv;
        JContainer data;

        try
        {
            data = JArray.Parse(result);
        }
        catch (Exception ex)
        {
            data = JObject.Parse(result);
        }

        rv = new ApiResponse<T>(new ApiResponseRaw(data))
        {
            RequestedPath = path,
            ResponseText = result
        };

        return rv;
    }
}
