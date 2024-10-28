namespace Streckenbuch.Server.Configuration;

public class MailConfiguration
{
    public string? Server { get; set; }

    public int Port { get; set; }

    public bool SSL { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Mail { get; set; }
}
