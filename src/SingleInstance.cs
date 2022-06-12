using System.Net.Http;

/// <summary>
/// A class that contains objects that should ONLY have ONE instance on memory.
/// </summary>
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