#nullable enable

namespace MoodleApiWrapper.Model;

public class UserData : UserOptionalProperties
{
    public string username;
    public string firstname;
    public string lastname;
    public string email;
    public string password;

    public UserData(string username, string firstname, string lastname, string email, string password)
    {
        this.username = username;
        this.firstname = firstname;
        this.lastname = lastname;
        this.email = email;
        this.password = password;
    }


}
