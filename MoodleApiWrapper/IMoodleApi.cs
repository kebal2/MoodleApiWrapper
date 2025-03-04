using System;
using System.Threading;
using System.Threading.Tasks;

using MoodleApiWrapper.ApiResources;
using MoodleApiWrapper.Model;

namespace MoodleApiWrapper;

public interface IMoodleApi
{
    Task<ApiResponse<Success>> DeleteCourses(int[] courseIds, CancellationToken cancellationToken = default);
    Task<AuthentiactionResponse<AuthToken>> GetApiToken(string username, string password, string serviceHostName, CancellationToken cancellationToken = default);
    Task<ApiResponse<SiteInfo>> GetSiteInfo(string serviceHostName = "", CancellationToken cancellationToken = default);
    Task<ApiResponse<Users>> GetUsers(object criteria, CancellationToken cancellationToken = default);
    Task<ApiResponse<User[]>> GetUsers(UserFields field, string[] values, CancellationToken cancellationToken = default);
    Task<ApiResponse<User>> GetUser(UserFields field, string value, CancellationToken cancellationToken = default);
    Task<ApiResponse<Cources[]>> GetUserCourses(int userid, CancellationToken cancellationToken = default);
    Task<ApiResponse<NewUser>> CreateUser(UserData userOptionalProperties, CancellationToken cancellationToken = default);
    Task<ApiResponse<Success>> UpdateUser(int id, UserData userOptionalProperties, CancellationToken cancellationToken = default);
    Task<ApiResponse<Success>> EnableUser(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<Success>> DisableUser(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<Success>> DeleteUser(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<Success>> AssignRoles(int roleId, int userId, string contextId = "", string contextLevel = "", int instanceId = Int32.MinValue, CancellationToken cancellationToken = default);
    Task<ApiResponse<Success>> RevokeRoles(int roleId, int userId, string contextId = "", string contextLevel = "", int instanceId = Int32.MinValue, CancellationToken cancellationToken = default);

    /// <param name="timeStart">UnixTimestamp</param>
    /// <param name="timeEnd">UnixTimestamp</param>
    Task<ApiResponse<Success>> EnrolUser(int roleId, int userId, int courseId, int timeStart = Int32.MinValue, int timeEnd = Int32.MinValue, int suspend = Int32.MinValue, CancellationToken cancellationToken = default);

    Task<ApiResponse<Success>> AddGroupMember(int groupId, int userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<Success>> DeleteGroupMember(int groupId, int userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<Category[]>> GetCategories(string criteriaKey, string criteriaValue, int addSubCategories = 1, CancellationToken cancellationToken = default);
    Task<ApiResponse<Course[]>> GetCourses(int options = int.MinValue, CancellationToken cancellationToken = default);
    Task<ApiResponse<GetCourseResult>> GetCourses(int? id = null, int[] ids = null, string shortname = null, string idnumber = null, int? category = null, CancellationToken cancellationToken = default);
    Task<ApiResponse<Content[]>> GetContents(int courseId, CancellationToken cancellationToken = default);
    Task<ApiResponse<Group>> GetGroup(int groupId, CancellationToken cancellationToken = default);
    Task<ApiResponse<Group[]>> GetGroups(int[] groupIds, CancellationToken cancellationToken = default);
    Task<ApiResponse<Group>> GetCourseGroups(int courseId, CancellationToken cancellationToken = default);
    Task<ApiResponse<EnrolledUser>> GetEnrolledUsersByCourse(int courseId, CancellationToken cancellationToken = default);
    Task<ApiResponse<NewCourse>> CreateCourse(CourseCreate course, CancellationToken cancellationToken = default);
    Task<ApiResponse<NewCourse[]>> CreateCourses(CourseCreate[] courses, int[] categoryIds = default, CancellationToken cancellationToken = default);
    Task<ApiResponse<UpdateCourseRoot>> UpdateCourse(int id, CourseUpdate course, CancellationToken cancellationToken = default);
    Task<ApiResponse<Category[]>> GetGrades(int courseId, string component = "", int activityId = Int32.MaxValue, string[] userIds = null, CancellationToken cancellationToken = default);
    Task<ApiResponse<Events[]>> GetCalendarEvents(int[] groupIds = default, int[] courseIds = default, int[] eventIds = default, CancellationToken cancellationToken = default);

    Task<ApiResponse<Events[]>> CreateCalendarEvents(string[] names, string[] descriptions = default,
        int[] formats = default, int[] groupIds = default, int[] courseIds = default, int[] repeats = default,
        string[] eventTypes = default, DateTime[] timeStarts = default, TimeSpan[] timeDurations = default,
        int[] visible = default, int[] sequences = default, CancellationToken cancellationToken = default);

    Task<ApiResponse<Events[]>> DeleteCalendarEvents(int[] eventIds, int[] repeats, string[] descriptions = default, CancellationToken cancellationToken = default);

    Task<ApiResponse<Group[]>> CreateGroups(string[] names = default, int[] courseIds = default, string[] descriptions = default,
        int[] descriptionFormats = default, string[] enrolmentKeys = default, string[] idNumbers = default, CancellationToken cancellationToken = default);
}