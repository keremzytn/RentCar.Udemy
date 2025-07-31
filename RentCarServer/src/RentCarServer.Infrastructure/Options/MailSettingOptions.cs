namespace RentCarServer.Infrastructure.Options;

public sealed class MailSettingOptions
{
    public string Host { get; set; }=default!;
    public int Port { get; set; }
    public string Username { get; set; }=default!;
    public string Password { get; set; }=default!;
    public bool SSL { get; set; }
}