public class RabbitMQSettings
{
    public string HostName { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public int Port { get; set; }

    public RabbitMQSettings()
    {
        HostName = string.Empty;
        UserName = string.Empty;
        Password = string.Empty;
    }
}