using System;
using Discord;
using Discord.WebSocket;

namespace RAT;

internal partial class Commands
{
    internal static async Task RebuildCommands(SocketSlashCommand cmdSket)
    {
        await cmdSket.DeferAsync();
        await cmdSket.FollowupAsync(embed: new EmbedBuilder { Title = "Rebuilding Commands...", Description = $"The current slash commands are being rebuilt, please hold...", Footer = Utils.GetTimeFooter() }.Build());
        await CommandBuilder.Shared.BuildFor((await cmdSket.GetChannelAsync() as ISocketMessageChannel)!.GetGuild());
        await cmdSket.ModifyOriginalResponseAsync(ogrsp =>
        {
            ogrsp.Embed = new EmbedBuilder
            {
                Title = "Commands Rebuilt.",
                Description = "The available commands were rebuilt for this guild.",
                Footer = Utils.GetTimeFooter()
            }.Build();
        });
    }
}