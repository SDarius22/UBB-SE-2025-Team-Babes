using System;

public class User
{
    [Key]
    public long Id { get; set; }

    public required string Username { get; set; }

    [EmailAddress]
    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public User()
	{

    }
}
