public class UserSettings
{
    public string Name { get; set;}

    public List<string> UserGroups { get; set;}

    public UserSettings()
    {
        Name       = string.Empty;
        UserGroups = new List<string>();
    }
}