namespace Picpay.Domain.Entities;

using Picpay.Domain.Enums;

public class User : BaseEntity
{
    public string Name { get; private set; }
    public string Username { get; private set; }
    public string Document { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public UserType Type { get; private set; }
    public Wallet? Wallet { get; private set; }

    protected User() { }
    public User(string name, string username, string document, string email, string password, UserType type)
    {
        Name = name;
        Username = username;
        Document = document;
        Email = email;
        Password = password;
        Type = type;
    }

    public void Update(string name, string username, string document, string email, string password, UserType type)
    {
        Name = name;
        Username = username;
        Document = document;
        Email = email;
        Password = password;
        Type = type;
    }
}
