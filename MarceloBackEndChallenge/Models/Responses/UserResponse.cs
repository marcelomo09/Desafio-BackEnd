public class UserResponse
{
    public string IdUser { get; set; }

    public string UserName { get; set; }

    public string UserGroup { get; set; }

    public UserResponse()
    {
        IdUser    = string.Empty;
        UserName  = string.Empty;
        UserGroup = string.Empty;
    }

    public UserResponse(User user)
    {
        IdUser    = user.Id.ToString();
        UserName  = user.UserName;
        UserGroup = user.UserGroup;
    }
}