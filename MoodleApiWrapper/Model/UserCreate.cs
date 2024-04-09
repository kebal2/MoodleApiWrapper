#nullable enable
namespace MoodleApiWrapper.Model;

public class UserCreate : UserOptionalProperties
{
    internal string username;
    internal string firstname;
    internal string lastname;
    internal string email;
    internal string password;

    public UserCreate(string username, string firstname, string lastname, string email, string password)
    {
        this.username = username;
        this.firstname = firstname;
        this.lastname = lastname;
        this.email = email;
        this.password = password;
    }
}
