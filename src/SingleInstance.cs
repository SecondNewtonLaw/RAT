using System.Net.Http;

public static class SingleInstance
{
    /// <summary>
    /// HttpClient: Used to perform HTTP requests to the outside world.
    /// </summary>
    public static HttpClient HttpClient { get; } = new(new HttpClientHandler()
    {
        SslProtocols = System.Security.Authentication.SslProtocols.Tls12
    });
}