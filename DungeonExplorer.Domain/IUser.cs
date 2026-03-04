namespace DungeonExplorer.Api.Domain;

public interface IUserCredentials
{
    string Email { get; }

    string Password { get; }
}

public class UserCredentials : IUserCredentials
{
    private string email;
    private string password;

    public UserCredentials(string email, string password)
    {
        this.email = email;
        this.password = password;
    }

    public UserCredentials()
    {
        email = string.Empty;
        password = string.Empty;
    }

    public string Email { get => email; set => email = value; }
    public string Password { get => password; set => password = value; }
}   
