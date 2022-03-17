using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace MoodleApiWrapper
{
    public class ApiWrapper : IDisposable
    {
        private readonly HttpClient client;
        private readonly Uri host;
        private readonly string apiToken = "";

        public ApiWrapper(Uri host)
        {
            client = new HttpClient
            {
                BaseAddress = host
            };
        }

        public ApiWrapper(Uri host, string apiToken) : this(host)
        {
            this.host = host;
            this.apiToken = apiToken;
        }

        private bool TokenIsSet => apiToken.Any();
        private bool HostIsSet => host.AbsoluteUri.Any();

        private static int DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return Convert.ToInt32((TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                                    new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds);
        }

        public Task<ApiResponse<Success>> DeleteCourses(int[] courseIds, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                string query =
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_course_delete_courses}&" +
                    $"moodlewsrestformat={Format.json}&";

                for (int i = 0; i < courseIds.Length; i++)
                    query += $"courseids[{i}]={courseIds[i]}&";

                return Get<Success>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// Returns your Api Token needed to make any calls
        /// <para />
        /// service shortname - The service shortname is usually hardcoded in the pre-build service (db/service.php files).
        /// Moodle administrator will be able to edit shortnames for service created on the fly: MDL-29807.
        /// If you want to use the Mobile service, its shortname is moodle_mobile_app. Also useful to know,
        /// the database shortname field can be found in the table named external_services.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="serviceHostName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<AuthentiactionResponse<AuthToken>> GetapiToken(string username, string password, string serviceHostName, CancellationToken cancellationToken = default)
        {
            if (!HostIsSet) throw new Exception("Host is not set");

            string query =
                "login/token.php" +
                $"?username={username}" +
                $"&password={password}" +
                $"&service={serviceHostName}";

            return GetAuth<AuthToken>(query.ToString(), cancellationToken);
        }

        /// <summary>
        /// This API will return information about the site, web services users, and authorized API actions. This call is useful for getting site information and the capabilities of the web service user. 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="serviceHostName"></param>
        /// <param names="serviceHostNames">Returns information about a particular service.</param>
        /// <returns></returns>
        public Task<ApiResponse<Site_info>> GetSiteInfo(string serviceHostName = "", CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                string query;
                if (!serviceHostName.Any())
                {
                    query = string.Format("webservice/rest/server.php" +
                                          "?wstoken={0}&moodlewsrestformat={1}&wsfunction={2}",
                        apiToken, Format.json.ToString(), Methods.core_webservice_get_site_info.ToString());
                }
                else
                {
                    query = string.Format("webservice/rest/server.php" +
                                          "?wstoken={0}&moodlewsrestformat={1}&wsfunction={2}&serviceshortnames[0]={3}",
                        apiToken, Format.json.ToString(), Methods.core_webservice_get_site_info.ToString(),
                        serviceHostName);
                }

                return Get<Site_info>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// Search for users matching the parameters of the call. This call will return matching user accounts with profile fields.
        ///  The key/value pairs to be considered in user search. Values can not be empty. Specify different keys only once
        ///  (fullname =&gt; 'user1', auth =&gt; 'manual', ...) - key occurences are forbidden. The search is executed with AND operator on the criterias.
        ///  Invalid criterias (keys) are ignored, the search is still executed on the valid criterias. You can search without criteria,
        ///  but the function is not designed for it. It could very slow or timeout. The function is designed to search some specific users.
        /// <para />
        /// "id" (int) matching user id<para />
        ///"lastname" (string) user last names (Note: you can use % for searching but it may be considerably slower!)<para />
        ///"firstname" (string) user first names (Note: you can use % for searching but it may be considerably slower!)<para />
        ///"idnumber" (string) matching user idnumber<para />
        ///"username" (string) matching user username<para />
        ///"email" (string) user email (Note: you can use % for searching but it may be considerably slower!)<para />
        ///"auth" (string) matching user auth plugin<para />
        /// </summary>
        /// <param names="criteria_key0">Key of the first search parameter.</param>
        /// <param names="criteria_value0">Value of the first search term.</param>
        /// <param names="criteria_key1">Key of the second search parameter.</param>
        /// <param names="criteria_value1">Value of the second search term.</param>
        /// <returns></returns>
        public Task<ApiResponse<Users>> GetUsers(string criteria_key0, string criteria_value0, string criteria_key1 = "", string criteria_value1 = "", CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                string query;
                if (criteria_key1.Any() && criteria_value1.Any())
                {
                    query =
                        "webservice/rest/server.php?" +
                        $"wstoken={apiToken}&" +
                        $"wsfunction={Methods.core_user_get_users}&" +
                        $"moodlewsrestformat={Format.json}&" +
                        $"criteria[0][key]={criteria_key0}&" +
                        $"criteria[0][value]={criteria_value0}" +
                        $"criteria[1][key]={criteria_key0}&" +
                        $"criteria[1][value]={criteria_value0}";
                }
                else
                {
                    query =
                        "webservice/rest/server.php?" +
                        $"wstoken={apiToken}&" +
                        $"wsfunction={Methods.core_user_get_users}&" +
                        $"moodlewsrestformat={Format.json}&" +
                        $"criteria[0][key]={criteria_key0}&" +
                        $"criteria[0][value]={criteria_value0}";
                }

                return Get<Users>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// Retrieve users information for a specified unique field - If you want to do a user search, use GetUsers()
        /// 
        /// Avaiable Criteria:
        ///"id" (int) matching user id
        ///"lastname" (string) user last names (Note: you can use % for searching but it may be considerably slower!)
        ///"firstname" (string) user first names (Note: you can use % for searching but it may be considerably slower!)
        ///"idnumber" (string) matching user idnumber
        ///"username" (string) matching user username
        ///"email" (string) user email (Note: you can use % for searching but it may be considerably slower!)
        ///"auth" (string) matching user auth plugin
        /// </summary>
        /// <param names="criteria_key">Key of the first search parameter.</param>
        /// <param names="criteria_value">Value of the first search term.</param>
        /// <returns></returns>
        public Task<ApiResponse<Users>> GetUsersByField(string criteria_key, string criteria_value, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                string query =
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_user_get_users_by_field}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"criteria[0][key]={criteria_key}&" +
                    $"criteria[0][value]={criteria_value}";

                return Get<Users>(query.ToString(), cancellationToken);
            }

            if (!HostIsSet && TokenIsSet)
                throw new Exception("Host & token are not set");
            else if (!HostIsSet)
                throw new Exception("Host is not set");
            else
                throw new Exception("Token is not set");
        }

        /// <summary>
        /// Get the list of courses where a user is enrolled in 
        /// </summary>
        /// <param names="userid"></param>
        /// <returns></returns>
        public Task<ApiResponse<Cources>> GetUserCourses(int userid, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                string query =
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_enrol_get_users_courses}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"userid={userid}";

                return Get<Cources>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// Create a User.
        /// </summary>
        /// <param names="username"></param>
        /// <param names="firstname"></param>
        /// <param names="lastname"></param>
        /// <param names="email"></param>
        /// <param names="password"></param>
        /// <param names="auth"></param>
        /// <param names="idnumber"></param>
        /// <param names="lang"></param>
        /// <param names="calendartye"></param>
        /// <param names="theme"></param>
        /// <param names="timezone"></param>
        /// <param names="mailformat"></param>
        /// <param names="description"></param>
        /// <param names="city"></param>
        /// <param names="country"></param>
        /// <param names="firstnamephonetic"></param>
        /// <param names="lastnamephonetic"></param>
        /// <param names="middlename"></param>
        /// <param names="alternatename"></param>
        /// <param names="preferences_type"></param>
        /// <param names="preferences_value"></param>
        /// <param names="customfields_type"></param>
        /// <param names="customfields_value"></param>
        /// <returns></returns>
        public Task<ApiResponse<NewUser>> CreateUser(string username, string firstname, string lastname,
            string email, string password,
            string auth = "", string idnumber = "", string lang = "", string calendartye = "", string theme = "",
            string timezone = "",
            string mailformat = "", string description = "", string city = "", string country = "",
            string firstnamephonetic = "",
            string lastnamephonetic = "", string middlename = "", string alternatename = "",
            string preferences_type = "", string preferences_value = "",
            string customfields_type = "", string customfields_value = "", CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder querybuilder = new();
                querybuilder.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&wsfunction={Methods.core_user_create_users}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"users[0][username]={@username}&" +
                    $"users[0][password]={@password}&" +
                    $"users[0][firstname]={@firstname}&" +
                    $"users[0][lastname]={@lastname}&" +
                    $"users[0][email]={@email}");
                if (auth.Any()) querybuilder.Append($"&users[0][auth]={auth}");
                if (idnumber.Any()) querybuilder.Append($"&users[0][auth]={idnumber}");
                if (lang.Any()) querybuilder.Append($"&users[0][auth]={lang}");
                if (calendartye.Any()) querybuilder.Append($"&users[0][auth]={calendartye}");
                if (theme.Any()) querybuilder.Append($"&users[0][auth]={theme}");
                if (timezone.Any()) querybuilder.Append($"&users[0][auth]={timezone}");
                if (mailformat.Any()) querybuilder.Append($"&users[0][auth]={mailformat}");
                if (description.Any()) querybuilder.Append($"&users[0][auth]={description}");
                if (city.Any()) querybuilder.Append($"&users[0][auth]={city}");
                if (country.Any()) querybuilder.Append($"&users[0][auth]={country}");
                if (firstnamephonetic.Any()) querybuilder.Append($"&users[0][auth]={firstnamephonetic}");
                if (lastnamephonetic.Any()) querybuilder.Append($"&users[0][auth]={lastnamephonetic}");
                if (middlename.Any()) querybuilder.Append($"&users[0][auth]={middlename}");
                if (alternatename.Any()) querybuilder.Append($"&users[0][auth]={alternatename}");
                if (preferences_type.Any()) querybuilder.Append($"&users[0][auth]={preferences_type}");
                if (preferences_value.Any()) querybuilder.Append($"&users[0][auth]={preferences_value}");
                if (customfields_type.Any()) querybuilder.Append($"&users[0][auth]={customfields_type}");
                if (customfields_value.Any()) querybuilder.Append($"&users[0][auth]={customfields_value}");


                return Get<NewUser>(querybuilder.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param names="id"></param>
        /// <param names="username"></param>
        /// <param names="firstname"></param>
        /// <param names="lastname"></param>
        /// <param names="email"></param>
        /// <param names="password"></param>
        /// <param names="auth"></param>
        /// <param names="idnumber"></param>
        /// <param names="lang"></param>
        /// <param names="calendartye"></param>
        /// <param names="theme"></param>
        /// <param names="timezone"></param>
        /// <param names="mailformat"></param>
        /// <param names="description"></param>
        /// <param names="city"></param>
        /// <param names="country"></param>
        /// <param names="firstnamephonetic"></param>
        /// <param names="lastnamephonetic"></param>
        /// <param names="middlename"></param>
        /// <param names="alternatename"></param>
        /// <param names="preferences_type"></param>
        /// <param names="preferences_value"></param>
        /// <param names="customfields_type"></param>
        /// <param names="customfields_value"></param>
        /// <returns></returns>
        public Task<ApiResponse<Success>> UpdateUser(int id, string username = "", string firstname = "",
            string lastname = "",
            string email = "", string password = "", string auth = "", string idnumber = "", string lang = "",
            string calendartye = "", string theme = "",
            string timezone = "", string mailformat = "", string description = "", string city = "", string country = "",
            string firstnamephonetic = "",
            string lastnamephonetic = "", string middlename = "", string alternatename = "",
            string preferences_type = "", string preferences_value = "",
            string customfields_type = "", string customfields_value = "", CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder querybuilder = new();
                querybuilder.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&wsfunction={Methods.core_user_update_users}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"users[0][id]={id}");

                if (username.Any()) querybuilder.Append($"&users[0][username]={username}");
                if (password.Any()) querybuilder.Append($"&users[0][password]={password}");
                if (firstname.Any()) querybuilder.Append($"&users[0][firstname]={firstname}");
                if (lastname.Any()) querybuilder.Append($"&users[0][lastname]={lastname}");
                if (email.Any()) querybuilder.Append($"&users[0][email]={email}");
                if (auth.Any()) querybuilder.Append($"&users[0][auth]={auth}");
                if (idnumber.Any()) querybuilder.Append($"&users[0][auth]={idnumber}");
                if (lang.Any()) querybuilder.Append($"&users[0][auth]={lang}");
                if (calendartye.Any()) querybuilder.Append($"&users[0][auth]={calendartye}");
                if (theme.Any()) querybuilder.Append($"&users[0][auth]={theme}");
                if (timezone.Any()) querybuilder.Append($"&users[0][auth]={timezone}");
                if (mailformat.Any()) querybuilder.Append($"&users[0][auth]={mailformat}");
                if (description.Any()) querybuilder.Append($"&users[0][auth]={description}");
                if (city.Any()) querybuilder.Append($"&users[0][auth]={city}");
                if (country.Any()) querybuilder.Append($"&users[0][auth]={country}");
                if (firstnamephonetic.Any()) querybuilder.Append($"&users[0][auth]={firstnamephonetic}");
                if (lastnamephonetic.Any()) querybuilder.Append($"&users[0][auth]={lastnamephonetic}");
                if (middlename.Any()) querybuilder.Append($"&users[0][auth]={middlename}");
                if (alternatename.Any()) querybuilder.Append($"&users[0][auth]={alternatename}");
                if (preferences_type.Any()) querybuilder.Append($"&users[0][auth]={preferences_type}");
                if (preferences_value.Any()) querybuilder.Append($"&users[0][auth]={preferences_value}");
                if (customfields_type.Any()) querybuilder.Append($"&users[0][auth]={customfields_type}");
                if (customfields_value.Any()) querybuilder.Append($"&users[0][auth]={customfields_value}");


                return Get<Success>(querybuilder.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param names="id"></param>
        /// <returns></returns>
        public Task<ApiResponse<Success>> DeleteUser(int id, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                string query =
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_user_delete_users}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"userids[0]={id}";

                return Get<Success>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// Manual role assignments. This call should be made in an array. 
        /// </summary>
        /// <param names="role_id">
        /// <summary>Role to assign to the user</summary>
        /// </param>
        /// <param names="user_id"></param>
        /// <param names="context_id"></param>
        /// <param names="context_level"></param>
        /// <param names="instance_id"></param>
        /// <returns></returns>
        public Task<ApiResponse<Success>> AssignRoles(int role_id, int user_id, string context_id = "",
            string context_level = "", int instance_id = Int32.MinValue, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_role_assign_roles}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"assignments[0][roleid]={role_id}&" +
                    $"assignments[0][userid]={user_id}");
                if (context_id.Any()) query.Append($"&assignments[0][contextid]={context_id}");
                if (context_level.Any()) query.Append($"&assignments[0][contextlevel]={context_level}");
                if (instance_id != Int32.MinValue) query.Append($"&assignments[0][instanceid]={instance_id}");

                return Get<Success>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param names="role_id"></param>
        /// <param names="user_id"></param>
        /// <param names="context_id"></param>
        /// <param names="context_level"></param>
        /// <param names="instance_id"></param>
        /// <returns></returns>
        public Task<ApiResponse<Success>> UnassignRoles(int role_id, int user_id, string context_id = "",
            string context_level = "", int instance_id = Int32.MinValue, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_role_unassign_roles}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"unassignments[0][roleid]={role_id}&" +
                    $"unassignments[0][userid]={user_id}");
                if (context_id.Any()) query.Append($"&unassignments[0][contextid]={context_id}");
                if (context_level.Any()) query.Append($"&unassignments[0][contextlevel]={context_level}");
                if (instance_id != Int32.MinValue) query.Append($"&unassignments[0][instanceid]={instance_id}");

                return Get<Success>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param names="role_id"></param>
        /// <param names="user_id"></param>
        /// <param names="cource_id"></param>
        /// <param names="timestart"></param>
        /// <param names="timeend"></param>
        /// <param names="suspend"></param>
        /// <returns></returns>
        public Task<ApiResponse<Success>> EnrolUser(int role_id, int user_id, int cource_id,
            int timestart = Int32.MinValue, int timeend = Int32.MinValue, int suspend = Int32.MinValue, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.enrol_manual_enrol_users}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"enrolments[0][roleid]={role_id}&" +
                    $"enrolments[0][userid]={user_id}&" +
                    $"enrolments[0][courceid]={cource_id}");
                if (timestart != Int32.MinValue) query.Append($"&enrolments[0][timestart]={timestart}");
                if (timeend != Int32.MinValue) query.Append($"&enrolments[0][timeend]={timeend}");
                if (suspend != Int32.MinValue) query.Append($"&enrolments[0][suspend]={suspend}");

                return Get<Success>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param names="group_id"></param>
        /// <param names="user_id"></param>
        /// <returns></returns>
        public Task<ApiResponse<Success>> AddGroupMember(int group_id, int user_id, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_group_add_group_members}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"members[0][groupid]={group_id}&" +
                    $"members[0][userid]={user_id}");

                return Get<Success>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param names="group_id"></param>
        /// <param names="user_id"></param>
        /// <returns></returns>
        public Task<ApiResponse<Success>> DeleteGroupMember(int group_id, int user_id, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_group_delete_group_members}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"members[0][groupid]={group_id}&" +
                    $"members[0][userid]={user_id}");

                return Get<Success>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// Get a listing of categories in the system. 
        /// </summary>
        /// <param names="criteria_key">
        /// <summary>
        /// criteria[0][key] - The category column to search, expected keys (value format) are:"id" (int) the category id,"names" (string)
        ///  the category names,"parent" (int) the parent category id,"idnumber" (string) category idnumber - user must have 'moodle/category:manage'
        ///  to search on idnumber,"visible" (int) whether the returned categories must be visible or hidden.
        ///  If the key is not passed, then the function return all categories that the user can see. - user must have 'moodle/category:manage'
        ///  or 'moodle/category:viewhiddencategories' to search on visible,"theme" (string) only return the categories having this theme
        ///  - user must have 'moodle/category:manage' to search on theme
        /// </summary>
        /// </param>
        /// <param names="criteria_value"><summary>Criteria[0][value] - The value to match</summary></param>
        /// <param names="addSubCategories"><summary>Return the sub categories infos (1 - default) otherwise only the category info (0)</summary></param>
        /// <returns></returns>
        public Task<ApiResponse<Category>> GetCategories(string criteria_key, string criteria_value, int addSubCategories = 1, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_course_get_categories}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"criteria[0][key]={criteria_key}&" +
                    $"criteria[0][value]={criteria_value}");

                if (addSubCategories != 1) query.Append($"&addsubcategories={addSubCategories}");

                return Get<Category>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// Get a listing of courses in the system. 
        /// </summary>
        /// <param names="options"><summary>List of course id.If empty return all courses except front page course.</summary></param>
        /// <returns></returns>
        public Task<ApiResponse<Course>> GetCourses(int options = int.MinValue, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_course_get_courses}&" +
                    $"moodlewsrestformat={Format.json}");
                if (options != int.MinValue) query.Append($"&addsubcategories={options}");

                return Get<Course>(query.ToString(), cancellationToken);
            }

            if (!HostIsSet && TokenIsSet)
                throw new Exception("Host & token are not set");

            if (!HostIsSet)
                throw new Exception("Host is not set");

            throw new Exception("Token is not set");
        }

        /// <summary>
        /// Get course contents
        /// </summary>
        /// <param names="course_id"><summary>Course Id</summary></param>
        /// <returns></returns>
        public Task<ApiResponse<Content>> GetContents(int course_id, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_course_get_contents}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"courseid={course_id}");

                return Get<Content>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// Returns group details. 
        /// </summary>
        /// <param names="group_id">Group id</param>
        /// <returns></returns>
        public Task<ApiResponse<Group>> GetGroup(int group_id, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_group_get_groups}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"groupids[0]={group_id}");

                return Get<Group>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// Returns group details. 
        /// </summary>
        /// <param names="group_ids"><summary>Group Ids</summary></param>
        /// <returns></returns>
        public Task<ApiResponse<Group>> GetGroups(int[] group_ids, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_group_get_groups}&" +
                    $"moodlewsrestformat={Format.json}");

                for (int i = 0; i < group_ids.Length; i++)
                {
                    query.Append($"&groupids[{i}]={group_ids[i]}");
                }

                return Get<Group>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// Returns all groups in specified course. 
        /// </summary>
        /// <param names="course_id"><summary>Course Id</summary></param>
        /// <returns></returns>
        public Task<ApiResponse<Group>> GetCourseGroups(int course_id, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_group_get_course_groups}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"courseid={course_id}");

                return Get<Group>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// Get enrolled users by course id. 
        /// </summary>
        /// <param names="course_id"></param>
        /// <returns></returns>
        public Task<ApiResponse<EnrolledUser>> GetEnrolledUsersByCourse(int course_id, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_enrol_get_enrolled_users}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"courseid={course_id}");

                return Get<EnrolledUser>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// Create new course
        /// </summary>
        /// <param names="fullname"><summary>Full names of the course</summary></param>
        /// <param names="shortname"><summary>Shortname of the course</summary></param>
        /// <param names="category_id"><summary>Category ID of the course</summary></param>
        /// <param names="idnumber"><summary>Optional //id number</summary></param>
        /// <param names="summary"><summary>Optional //summary</summary></param>
        /// <param names="summaryformat"><summary>Default to "1" //summary format (1 = HTML, 0 = MOODLE, 2 = PLAIN or 4 = MARKDOWN)</summary></param>
        /// <param names="format"><summary>Default to "topics" //course format: weeks, topics, social, site,..</summary></param>
        /// <param names="showgrades"><summary>Default to "0" //1 if grades are shown, otherwise 0</summary></param>
        /// <param names="newsitems"><summary>Default to "0" //number of recent items appearing on the course page</summary></param>
        /// <param names="startdate"><summary>Optional //timestamp when the course start</summary></param>
        /// <param names="numsections"><summary>Optional //(deprecated, use courseformatoptions) number of weeks/topics</summary></param>
        /// <param names="maxbytes"><summary>Default to "104857600" //largest size of file that can be uploaded into the course</summary></param>
        /// <param names="showreports"><summary>Default to "1" //are activity report shown (yes = 1, no =0)</summary></param>
        /// <param names="visible"><summary>Optional //1: available to student, 0:not available</summary></param>
        /// <param names="hiddensections"><summary>Optional //(deprecated, use courseformatoptions) How the hidden sections in the course are displayed to students</summary></param>
        /// <param names="groupmode"><summary>Default to "0" //no group, separate, visible</summary></param>
        /// <param names="groupmodeforce"><summary>Default to "0" //1: yes, 0: no</summary></param>
        /// <param names="defaultgroupingid"><summary>Default to "0" //default grouping id</summary></param>
        /// <param names="enablecompletion"><summary>Optional //Enabled, control via completion and activity settings. Disabled, not shown in activity settings.</summary></param>
        /// <param names="completenotify"><summary>Optional //1: yes 0: no</summary></param>
        /// <param names="lang"><summary>//forced course language</summary></param>
        /// <param names="forcetheme"><summary>Optional //names of the force theme</summary></param>
        /// <param names="courcCourseformatoption"><summary>Optional //additional options for particular course format list of ( object { names string //course format option names
        ///value string //course format option value } )} )</summary></param>
        /// <returns></returns>
        public Task<ApiResponse<NewCourse>> CreateCourse(CourseModel course, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_course_create_courses}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"courses[0][fullname]={course.Fullname}&" +
                    $"courses[0][shortname]={course.Shortname}&" +
                    $"courses[0][categoryid]={course.CategoryId}");

                if (course.idnumber.Any()) query.Append($"&courses[0][idnumber]={course.idnumber}");
                if (course.summary.Any()) query.Append($"&courses[0][summary]={course.summary}");
                if (course.summaryformat != 1) query.Append($"&courses[0][summaryformat ]={course.summaryformat}");
                if (course.format.Any()) query.Append($"&courses[0][format]={course.format}");
                if (course.showgrades != 0) query.Append($"&courses[0][showgrades]={course.showgrades}");
                if (!course.startdate.Equals(default)) query.Append($"&courses[0][startdate]={DateTimeToUnixTimestamp(course.startdate)}");
                if (course.newsitems != 0) query.Append($"&courses[0][newsitems]={course.newsitems}");
                if (course.numsections != int.MaxValue) query.Append($"&courses[0][numsections]={course.numsections}");
                if (course.maxbytes != 104857600) query.Append($"&courses[0][maxbytes]={course.CategoryId}");
                if (course.showreports != 1) query.Append($"&courses[0][showreports]={course.showreports}");
                if (course.visible != 0) query.Append($"&courses[0][visible]={course.visible}");
                if (course.hiddensections != int.MaxValue) query.Append($"&courses[0][hiddensections]={course.hiddensections}");
                if (course.groupmode != 0) query.Append($"&courses[0][groupmode]={course.groupmode}");
                if (course.groupmodeforce != 0) query.Append($"&courses[0][groupmodeforce]={course.groupmodeforce}");
                if (course.defaultgroupingid != 0) query.Append($"&courses[0][defaultgroupingid]={course.defaultgroupingid}");
                if (course.enablecompletion != int.MaxValue) query.Append($"&courses[0][enablecompletion]={course.enablecompletion}");
                if (course.completenotify != 0) query.Append($"&courses[0][completenotify]={course.completenotify}");
                if (course.lang.Any()) query.Append($"&courses[0][lang]={course.lang}");
                if (course.forcetheme.Any()) query.Append($"&courses[0][forcetheme]={course.forcetheme}");

                return Get<NewCourse>(query.ToString(), cancellationToken);
            }

            if (!HostIsSet && TokenIsSet)
                throw new Exception("Host & token are not set");
            if (!HostIsSet)
                throw new Exception("Host is not set");

            throw new Exception("Token is not set");
        }

        /// <summary>
        /// Create new courses
        /// </summary>
        /// <param names="fullname"></param>
        /// <param names="shortname"></param>
        /// <param names="category_ids"></param>
        /// <returns></returns>
        public Task<ApiResponse<NewCourse>> CreateCourses(CourseModel[] courses, int[] category_ids = default, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_course_create_courses}&" +
                    $"moodlewsrestformat={Format.json}");

                for (var i = 0; i < courses.Length; i++)
                {
                    var course = courses[i];

                    query.Append($"&courses[{i}][{nameof(course.Fullname).ToLower()}]={course.Fullname}");
                    query.Append($"&courses[{i}][{nameof(course.Shortname).ToLower()}]={course.Shortname}");
                    query.Append($"&courses[{i}][{nameof(course.CategoryId).ToLower()}]={course.CategoryId}");
                }

                return Get<NewCourse>(query.ToString(), cancellationToken);
            }

            if (!HostIsSet && TokenIsSet)
                throw new Exception("Host & token are not set");
            if (!HostIsSet)
                throw new Exception("Host is not set");

            throw new Exception("Token is not set");
        }

        public Task<ApiResponse<UpdateCourseRoot>> UpdateCourse(int id, string fullname = "", string shortname = "", int category_id = Int32.MaxValue,
            string idnumber = "", string summary = "", int summaryformat = 1, string format = "", int showgrades = 0, int newsitems = 0,
            DateTime startdate = default, int numsections = int.MaxValue, int maxbytes = 104857600, int showreports = 1,
            int visible = 0, int hiddensections = int.MaxValue, int groupmode = 0,
            int groupmodeforce = 0, int defaultgroupingid = 0, int enablecompletion = int.MaxValue,
            int completenotify = 0, string lang = "", string forcetheme = "",
            string courcCourseformatoption = "" /*not implemented*/, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_course_update_courses}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"courses[0][id]={id}");

                if (fullname.Any()) query.Append($"&courses[0][fullname]={fullname}");
                if (shortname.Any()) query.Append($"&courses[0][shortname]={shortname}");
                if (category_id != Int32.MaxValue) query.Append($"&courses[0][categoryid]={category_id}");
                if (idnumber.Any()) query.Append($"&courses[0][idnumber]={idnumber}");
                if (summary.Any()) query.Append($"&courses[0][summary]={summary}");
                if (summaryformat != 1) query.Append($"&courses[0][summaryformat ]={summaryformat}");
                if (format.Any()) query.Append($"&courses[0][format]={format}");
                if (showgrades != 0) query.Append($"&courses[0][showgrades]={showgrades}");
                if (!startdate.Equals(default)) query.Append($"&courses[0][startdate]={DateTimeToUnixTimestamp(startdate)}");
                if (newsitems != 0) query.Append($"&courses[0][newsitems]={newsitems}");
                if (numsections != int.MaxValue) query.Append($"&courses[0][numsections]={numsections}");
                if (maxbytes != 104857600) query.Append($"&courses[0][maxbytes]={category_id}");
                if (showreports != 1) query.Append($"&courses[0][showreports]={showreports}");
                if (visible != 0) query.Append($"&courses[0][visible]={visible}");
                if (hiddensections != int.MaxValue) query.Append($"&courses[0][hiddensections]={hiddensections}");
                if (groupmode != 0) query.Append($"&courses[0][groupmode]={groupmode}");
                if (groupmodeforce != 0) query.Append($"&courses[0][groupmodeforce]={groupmodeforce}");
                if (defaultgroupingid != 0) query.Append($"&courses[0][defaultgroupingid]={defaultgroupingid}");
                if (enablecompletion != int.MaxValue) query.Append($"&courses[0][enablecompletion]={enablecompletion}");
                if (completenotify != 0) query.Append($"&courses[0][completenotify]={completenotify}");
                if (lang.Any()) query.Append($"&courses[0][lang]={lang}");
                if (forcetheme.Any()) query.Append($"&courses[0][forcetheme]={forcetheme}");

                return Get<UpdateCourseRoot>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// Returns grade item details and optionally student grades. 
        /// </summary>
        /// <param names="criteria_key"></param>
        /// <param names="criteria_value"></param>
        /// <param names="addSubCategories"></param>
        /// <returns></returns>
        public Task<ApiResponse<Category>> GetGrades(int courseid, string component = "", int activityid = Int32.MaxValue, string[] userids = null, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_grades_get_grades}&" +
                    $"moodlewsrestformat={Format.json}&" +
                    $"courseid={courseid}");

                if (component.Any()) query.Append($"&component={component}");
                if (activityid != int.MaxValue) query.Append($"&activityid={activityid}");
                if (userids != null) query.Append($"&userids={userids}");
                if (component.Any()) query.Append($"&component={component}");

                return Get<Category>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupids"></param>
        /// <param name="courseids"></param>
        /// <param name="eventids"></param>
        /// <returns></returns>
        public Task<ApiResponse<Events>> GetCalanderEvents(int[] groupids = default, int[] courseids = default, int[] eventids = default, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_calendar_get_calendar_events}&" +
                    $"moodlewsrestformat={Format.json}");

                if (groupids != null)
                    for (int i = 0; i < groupids.Length; i++)
                        query.Append($"&events[groupids][{i}]={groupids[i]}");

                if (courseids != null)
                    for (int i = 0; i < courseids.Length; i++)
                        query.Append($"&events[courseids][{i}]={courseids[i]}");

                if (eventids != null)
                    for (int i = 0; i < eventids.Length; i++)
                        query.Append($"&events[eventids][{i}]={eventids[i]}");

                return Get<Events>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="names"></param>
        /// <param name="descriptions"></param>
        /// <param name="formats"></param>
        /// <param name="groupids"></param>
        /// <param name="courseids"></param>
        /// <param name="repeats"></param>
        /// <param name="eventtypes"></param>
        /// <param name="timestarts"></param>
        /// <param name="timedurations"></param>
        /// <param name="visible"></param>
        /// <param name="sequences"></param>
        /// <returns></returns>
        public Task<ApiResponse<Events>> CreateCalanderEvents(string[] names, string[] descriptions = default,
            int[] formats = default, int[] groupids = default, int[] courseids = default, int[] repeats = default,
            string[] eventtypes = default, DateTime[] timestarts = default, TimeSpan[] timedurations = default,
            int[] visible = default, int[] sequences = default, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_calendar_create_calendar_events}&" +
                    $"moodlewsrestformat={Format.json}");

                for (int i = 0; i < names.Length; i++)
                    query.Append($"&events[{i}][name]={names[i]}");

                if (groupids != null)
                    for (int i = 0; i < groupids.Length; i++)
                        query.Append($"&events[{i}][groupid]={groupids[i]}");

                if (courseids != null)
                    for (int i = 0; i < courseids.Length; i++)
                        query.Append($"&events[{i}][courseid]={courseids[i]}");

                if (descriptions != null)
                    for (int i = 0; i < descriptions.Length; i++)
                        query.Append($"&events[{i}][description]={descriptions[i]}");

                if (formats != null)
                    for (int i = 0; i < formats.Length; i++)
                        query.Append($"&events[{i}][format]={formats[i]}");

                if (repeats != null)
                    for (int i = 0; i < repeats.Length; i++)
                        query.Append($"&events[{i}][repeats]={repeats[i]}");

                if (timestarts != null)
                    for (int i = 0; i < timestarts.Length; i++)
                        query.Append($"&events[{i}][timestart]={DateTimeToUnixTimestamp(timestarts[i])}");

                if (timedurations != null)
                    for (int i = 0; i < timedurations.Length; i++)
                        query.Append($"&events[{i}][timeduration]={timedurations[i].TotalSeconds}");

                if (visible != null)
                    for (int i = 0; i < visible.Length; i++)
                        query.Append($"&events[{i}][visible]={visible[i]}");

                if (sequences != null)
                    for (int i = 0; i < sequences.Length; i++)
                        query.Append($"&events[{i}][sequence]={sequences[i]}");

                return Get<Events>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventids"></param>
        /// <param name="repeats"></param>
        /// <param name="descriptions"></param>
        /// <returns></returns>
        public Task<ApiResponse<Events>> DeleteCalanderEvents(int[] eventids, int[] repeats, string[] descriptions = default, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction={Methods.core_calendar_delete_calendar_events}&" +
                    $"moodlewsrestformat={Format.json}");

                if (repeats != null)
                    for (int i = 0; i < repeats.Length; i++)
                        query.Append($"&events[{i}][repeat]={repeats[i]}");

                if (eventids != null)
                    for (int i = 0; i < eventids.Length; i++)
                        query.Append($"&events[{i}][eventid]={eventids[i]}");


                if (descriptions != null)
                    for (int i = 0; i < descriptions.Length; i++)
                        query.Append($"&events[{i}][description]={descriptions[i]}");

                return Get<Events>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="names"></param>
        /// <param name="courseids"></param>
        /// <param name="descriptions"></param>
        /// <param name="descriptionformats"></param>
        /// <param name="enrolmentkeys"></param>
        /// <param name="idnumbers"></param>
        /// <returns></returns>
        public Task<ApiResponse<Events>> CreateGroups(string[] names = default, int[] courseids = default, string[] descriptions = default,
            int[] descriptionformats = default, string[] enrolmentkeys = default, string[] idnumbers = default, CancellationToken cancellationToken = default)
        {
            if (HostIsSet && TokenIsSet)
            {
                StringBuilder query = new();
                query.Append(
                    "webservice/rest/server.php?" +
                    $"wstoken={apiToken}&" +
                    $"wsfunction=core_group_create_groups&" +
                    $"moodlewsrestformat={Format.json}");

                if (names != null)
                    for (int i = 0; i < names.Length; i++)
                        query.Append($"&groups[{i}][name]={names[i]}");

                if (courseids != null)
                    for (int i = 0; i < courseids.Length; i++)
                        query.Append($"&groups[{i}][courseid]={courseids[i]}");

                if (descriptions != null)
                    for (int i = 0; i < descriptions.Length; i++)
                        query.Append($"&groups[{i}][description]={descriptions[i]}");

                if (descriptionformats != null)
                    for (int i = 0; i < descriptionformats.Length; i++)
                        query.Append($"&groups[{i}][descriptionformat]={descriptionformats[i]}");

                if (enrolmentkeys != null)
                    for (int i = 0; i < enrolmentkeys.Length; i++)
                        query.Append($"&groups[{i}][enrolmentkey]={enrolmentkeys[i]}");

                if (idnumbers != null)
                    for (int i = 0; i < idnumbers.Length; i++)
                        query.Append($"&groups[{i}][idnumber]={idnumbers[i]}");


                return Get<Events>(query.ToString(), cancellationToken);
            }
            else
            {
                if (!HostIsSet && TokenIsSet)
                    throw new Exception("Host & token are not set");
                else if (!HostIsSet)
                    throw new Exception("Host is not set");
                else
                    throw new Exception("Token is not set");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam names="T"></typeparam>
        /// <param names="uri"></param>
        /// <returns></returns>
        private async Task<ApiResponse<T>> Get<T>(string uri, CancellationToken cancellationToken) where T : IDataModel
        {
            if (uri.Length > 2000)
                throw new Exception("URI is too long should be splitted into multiple queries");

            var response = await client.GetAsync(uri, cancellationToken);

            if (!response.IsSuccessStatusCode) throw new WebException(await response.Content.ReadAsStringAsync(cancellationToken));

            var result = await response.Content.ReadAsStringAsync(cancellationToken);

            if (result.ToLower() == "null")
                result = "{IsSuccessful: true,}";

            try
            {
                var data = JArray.Parse(result);
                return new ApiResponse<T>(new ApiResponseRaw(data));
            }
            catch (Exception)
            {
                var data = JObject.Parse(result);
                return new ApiResponse<T>(new ApiResponseRaw(data));
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            client?.Dispose();
        }
    }
}