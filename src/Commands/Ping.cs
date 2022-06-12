using Discord;
using Discord.WebSocket;

namespace RAT;

internal partial class Commands
{
    internal static async Task Ping(SocketSlashCommand cmdSket)
    {
        await cmdSket.DeferAsync();
        EmbedBuilder e = new() { Title = "Ping", Description = $"The current ping to the gateway is {MainRuntime.client.Latency}ms", Footer = Utils.GetTimeFooter() };
        await cmdSket.FollowupAsync(embed: e.Build());
    }
}