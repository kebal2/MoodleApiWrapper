using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MoodleApiWrapper.ApiResources;
using MoodleApiWrapper.Model;

using Newtonsoft.Json.Linq;

namespace MoodleApiWrapper;

internal class MoodleApi : IMoodleApi
{
    private const int PATH_LIMIT = 2000;

    private static readonly MediaTypeWithQualityHeaderValue mt = new(MediaTypeNames.Application.Json);
    private static readonly StringWithQualityHeaderValue encode = new("gzip");

    private readonly int retryCount;
    private readonly HttpClient client;
    private readonly MoodleRequestBuilder mrb;

    public MoodleApi(HttpClient client, MoodleRequestBuilder mrb) : this(client, mrb, 3)
    {
    }

    public MoodleApi(HttpClient client, MoodleRequestBuilder mrb, int retryCount)
    {
        this.retryCount = retryCount;
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
    public Task<ApiResponse<User>> GetUser(UserFields field, string value, CancellationToken cancellationToken = default) => Get<User>(mrb.GetUser(field, value), cancellationToken);
    public Task<ApiResponse<Users>> GetUsers(object criteria, CancellationToken cancellationToken = default) => Get<Users>(mrb.GetUsers(criteria), cancellationToken);

    public Task<ApiResponse<User[]>> GetUsers(UserFields field, string[] values, CancellationToken cancellationToken = default)
    {
        var path = mrb.GetUsers(field, values);

        if (path.Length > PATH_LIMIT)
        {
            return Get<User[]>(mrb.GetUriFor(Methods.core_user_get_users_by_field, _ => { }), new List<KeyValuePair<string, object>>
            {
                new("field", field.ToString()),
                new("values", values),
            }, cancellationToken);
        }

        return Get<User[]>(path, cancellationToken);
    }

    public Task<ApiResponse<Cources[]>> GetUserCourses(int userid, CancellationToken cancellationToken = default) => Get<Cources[]>(mrb.GetUserCourses(userid), cancellationToken);
    public Task<ApiResponse<NewUser>> CreateUser(UserData userOptionalProperties, CancellationToken cancellationToken = default) => Get<NewUser>(mrb.CreateUser(userOptionalProperties), cancellationToken);
    public Task<ApiResponse<Success>> UpdateUser(int id, UserData userOptionalProperties, CancellationToken cancellationToken = default) => Get<Success>(mrb.UpdateUser(id, userOptionalProperties), cancellationToken);
    public Task<ApiResponse<Success>> EnableUser(int id, CancellationToken cancellationToken = default) => Get<Success>(mrb.EnableUser(id), cancellationToken);
    public Task<ApiResponse<Success>> DisableUser(int id, CancellationToken cancellationToken = default) => Get<Success>(mrb.DisableUser(id), cancellationToken);

    public Task<ApiResponse<Success>> DeleteUser(int id, CancellationToken cancellationToken = default) => Get<Success>(mrb.DeleteUser(id), cancellationToken);

    public Task<ApiResponse<Success>> AssignRoles(int roleId, int userId, string contextId = "", string contextLevel = "", int instanceId = Int32.MinValue, CancellationToken cancellationToken = default) =>
        Get<Success>(mrb.AssignRoles(roleId, userId, contextId, contextLevel, instanceId), cancellationToken);

    public Task<ApiResponse<Success>> RevokeRoles(int roleId, int userId, string contextId = "", string contextLevel = "", int instanceId = Int32.MinValue, CancellationToken cancellationToken = default) =>
        Get<Success>(mrb.RevokeRoles(roleId, userId, contextId, contextLevel, instanceId), cancellationToken);

    /// <param name="courseId"></param>
    /// <param name="timeStart">UnixTimestamp</param>
    /// <param name="timeEnd">UnixTimestamp</param>
    /// <param name="roleId"></param>
    /// <param name="userId"></param>
    /// <param name="suspend"></param>
    /// <param name="cancellationToken"></param>
    public Task<ApiResponse<Success>> EnrolUser(int roleId, int userId, int courseId, int timeStart = Int32.MinValue, int timeEnd = Int32.MinValue, int suspend = Int32.MinValue, CancellationToken cancellationToken = default) =>
        Get<Success>(mrb.EnrolUser(roleId, userId, courseId, timeStart, timeEnd, suspend), cancellationToken);

    public Task<ApiResponse<Success>> AddGroupMember(int groupId, int userId, CancellationToken cancellationToken = default) => Get<Success>(mrb.AddGroupMember(groupId, userId), cancellationToken);
    public Task<ApiResponse<Success>> DeleteGroupMember(int groupId, int userId, CancellationToken cancellationToken = default) => Get<Success>(mrb.DeleteGroupMember(groupId, userId), cancellationToken);

    public Task<ApiResponse<Category[]>> GetCategories(string criteriaKey, string criteriaValue, int addSubCategories = 1, CancellationToken cancellationToken = default) =>
        Get<Category[]>(mrb.GetCategories(criteriaKey, criteriaValue, addSubCategories), cancellationToken);

    public Task<ApiResponse<Course[]>> GetCourses(int options = int.MinValue, CancellationToken cancellationToken = default) => Get<Course[]>(mrb.GetCourses(options), cancellationToken);

    public Task<ApiResponse<GetCourseResult>> GetCourses(int? id = null, int[] ids = null, string shortname = null, string idnumber = null, int? category = null, CancellationToken cancellationToken = default) =>
        Get<GetCourseResult>(mrb.GetCourses(id, ids, shortname, idnumber, category), cancellationToken);

    public Task<ApiResponse<Content[]>> GetContents(int courseId, CancellationToken cancellationToken = default) => Get<Content[]>(mrb.GetContents(courseId), cancellationToken);

    public Task<ApiResponse<Group>> GetGroup(int groupId, CancellationToken cancellationToken = default) => Get<Group>(mrb.GetGroup(groupId), cancellationToken);

    public Task<ApiResponse<Group[]>> GetGroups(int[] groupIds, CancellationToken cancellationToken = default) => Get<Group[]>(mrb.GetGroups(groupIds), cancellationToken);

    public Task<ApiResponse<Group[]>> GetCourseGroups(int courseId, CancellationToken cancellationToken = default) => Get<Group[]>(mrb.GetCourseGroups(courseId), cancellationToken);

    public Task<ApiResponse<EnrolledUser[]>> GetEnrolledUsersByCourse(int courseId, CancellationToken cancellationToken = default) => Get<EnrolledUser[]>(mrb.GetEnrolledUsersByCourse(courseId), cancellationToken);

    public Task<ApiResponse<NewCourse>> CreateCourse(CourseCreate course, CancellationToken cancellationToken = default) => Get<NewCourse>(mrb.CreateCourse(course), cancellationToken);
    public Task<ApiResponse<NewCourse[]>> CreateCourses(CourseCreate[] courses, int[] categoryIds = null, CancellationToken cancellationToken = default) => Get<NewCourse[]>(mrb.CreateCourses(courses, categoryIds), cancellationToken);

    public Task<ApiResponse<UpdateCourseRoot>> UpdateCourse(int id, CourseUpdate course, CancellationToken cancellationToken = default) => Get<UpdateCourseRoot>(mrb.UpdateCourse(id, course), cancellationToken);

    public Task<ApiResponse<Category[]>> GetGrades(int courseId, string component = "", int activityId = Int32.MaxValue, string[] userIds = null, CancellationToken cancellationToken = default) =>
        Get<Category[]>(mrb.GetGrades(courseId, component, activityId, userIds), cancellationToken);

    public Task<ApiResponse<Events[]>> GetCalendarEvents(int[] groupIds = null, int[] courseIds = null, int[] eventIds = null, CancellationToken cancellationToken = default) =>
        Get<Events[]>(mrb.GetCalendarEvents(groupIds, courseIds, eventIds), cancellationToken);

    public Task<ApiResponse<Events[]>> CreateCalendarEvents(string[] names, string[] descriptions = null,
        int[] formats = null, int[] groupIds = null, int[] courseIds = null, int[] repeats = null,
        string[] eventTypes = null, DateTime[] timeStarts = null, TimeSpan[] timeDurations = null,
        int[] visible = null, int[] sequences = null, CancellationToken cancellationToken = default) =>
        Get<Events[]>(mrb.CreateCalendarEvents(names, descriptions,
            formats, groupIds, courseIds, repeats,
            eventTypes, timeStarts, timeDurations,
            visible, sequences), cancellationToken);

    public Task<ApiResponse<Events[]>> DeleteCalendarEvents(int[] eventIds, int[] repeats, string[] descriptions = null, CancellationToken cancellationToken = default) =>
        Get<Events[]>(mrb.DeleteCalendarEvents(eventIds, repeats, descriptions), cancellationToken);

    public async Task<Group> GetGroupByName(string groupName, int courseId, CancellationToken cancellationToken = default)
    {
        var courseGroups = await GetCourseGroups(courseId, cancellationToken);
        return courseGroups.SuccessfulCall && courseGroups.Data?.Length > 0
            ? courseGroups.Data.SingleOrDefault(g => g.name == groupName)
            : null;
    }

    public Task<ApiResponse<Group[]>> CreateGroups(string[] names, int[] courseIds, string[] descriptions, int[] descriptionFormats = null, string[] enrolmentKeys = null, string[] idNumbers = null, int visibility = 0,
        CancellationToken cancellationToken = default) =>
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
        if (path.Length > PATH_LIMIT)
            throw new Exception("URI is too long should be split into multiple queries. Please use the other get method!");
        try
        {
            using var response = await client.GetAsync(path, cancellationToken);

            if (!response.IsSuccessStatusCode) throw new WebException(await response.Content.ReadAsStringAsync(cancellationToken));

            var result = await response.Content.ReadAsStringAsync(cancellationToken);

            return ResolveApiResponse<T>(path, result);
        }
        catch (HttpRequestException e)
        {
            var result = "{ exception: 'HttpRequestException', errorcode: '503', message: '" + e.Message + "' }";
            var data = JObject.Parse(result);
            return new ApiResponse<T>(new ApiResponseRaw(data))
            {
                RequestedPath = path,
                ResponseText = result
            };
        }
    }

    private async Task<ApiResponse<T>> Get<T>(string path, ICollection<KeyValuePair<string, object>> getData, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, path);

        request.Headers.Accept.Add(mt);
        request.Headers.AcceptEncoding.Add(encode);

        request.Content = GetPostData(getData);

        HttpResponseMessage response = null;

        try
        {
            var i = 0;
            while (i++ <= retryCount && response is not { StatusCode: HttpStatusCode.OK })
            {
                if (response != null)
                {
                    Thread.Sleep(1000 * i);
                    response.Dispose();
                }

                response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }

            if (i > retryCount || response == null) throw new Exception("Failed to get response.");

            var result = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            return ResolveApiResponse<T>(path, result);
        }
        finally
        {
            response?.Dispose();
        }
    }

    private static ApiResponse<T> ResolveApiResponse<T>(string path, string result)
    {
        if (result.ToLower() == "null")
            result = "{IsSuccessful: true,}";

        JContainer data;

        try
        {
            data = JArray.Parse(result);
        }
        catch (Exception)
        {
            data = JObject.Parse(result);
        }

        return new ApiResponse<T>(new ApiResponseRaw(data))
        {
            RequestedPath = path,
            ResponseText = result
        };
    }

    private static HttpContent GetPostData(ICollection<KeyValuePair<string, object>> valueToPost)
    {
        var formContent = new MultipartFormDataContent();
        // throw new NotImplementedException();
        foreach (var kvp in valueToPost)
        {
            if (kvp.Value is Array array)
            {
                int i = 0;
                foreach (var item in array)
                {
                    formContent.Add(new StringContent(item?.ToString() ?? string.Empty, Encoding.UTF8, MediaTypeNames.Text.Plain), string.Join("", kvp.Key, $"[{i++}]"));
                }
            }
            else
            {
                formContent.Add(new StringContent(kvp.Value?.ToString() ?? string.Empty, Encoding.UTF8, MediaTypeNames.Text.Plain), kvp.Key);
            }
        }

        return formContent;
    }
}