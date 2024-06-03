public class JwtToken
{
    public string Access { get; set; }

    public string Type { get; set; }

    public int ExpiresSeconds { get; set; }

    public JwtToken()
    {
        Access = string.Empty;
        Type   = string.Empty;
    }

    public JwtToken(string access, string type, int expiresSeconds)
    {
        Access         = access;
        Type           = type;
        ExpiresSeconds = expiresSeconds;
    }
}