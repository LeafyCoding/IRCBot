// -----------------------------------------------------------
// This program is private software, based on C# source code.
// To sell or change credits of this software is forbidden,
// except if someone approves it from the LeafyCoding INC. team.
// -----------------------------------------------------------
// Copyrights (c) 2016 IRCBot INC. All rights reserved.
// -----------------------------------------------------------

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using ChatSharp;

using IRCBot.Commands;

namespace IRCBot
{
    internal class Program
    {
        public static IrcClient client;

        private static void Main(string[] args)
        {
            Config.InitConfig();

            Tools.SemiColoredWrite(ConsoleColor.Yellow, "[IRC] ", "Initialising IRC client.");

            client = Config.IRC_SSL
                ? new IrcClient(Config.IRC_Server, new IrcUser(Config.IRC_BotName, Config.IRC_Ident, Config.IRC_Password), true)
                : new IrcClient(Config.IRC_Server, new IrcUser(Config.IRC_BotName, Config.IRC_Ident, Config.IRC_Password));

            Console.Title = $"{Config.IRC_BotName} on {Config.IRC_Server} in channel {Config.IRC_ChannelName}.";
            Tools.ColoredWrite(ConsoleColor.Green, "*** Initialising " + Console.Title);

            client.ConnectAsync();

            Tools.SemiColoredWrite(ConsoleColor.Yellow, "[IRC] ", "Connected to IRC server.");

            var MenuEnabled = false;
            client.ConnectionComplete += (s, e) =>
            {
                Tools.SemiColoredWrite(ConsoleColor.Yellow, "[IRC] ", "Identifying to NickServ.");
                client.SendMessage($"identify {Config.IRC_NSPassword}", "NickServ");
                Thread.Sleep(200);
                Tools.SemiColoredWrite(ConsoleColor.Yellow, "[IRC] ", "Enabling vHost.");
                client.SendMessage("hs on", "HostServ");
                Thread.Sleep(200);
                Tools.SemiColoredWrite(ConsoleColor.Yellow, "[IRC] ", "Joining default channel.");
                client.JoinChannel(Config.IRC_ChannelName);
                Tools.SemiColoredWrite(ConsoleColor.Yellow, "[IRC] ", "Joined channel, enabling menu.");
                Thread.Sleep(100);
                MenuEnabled = true;
            };

            client.ChannelMessageRecieved += (s, e) =>
            {
                if(e.PrivateMessage.Message.ToLower().StartsWith("!cc "))
                {
                    Currency.Convert(e);
                }
                if(e.PrivateMessage.Message.ToLower().StartsWith("!convert "))
                {
                    UnitConverter.Convert(e);
                }
                if(e.PrivateMessage.Message.ToLower().Equals("!spotify") && e.PrivateMessage.User.Nick.Equals("editio"))
                {
                    var proc = Process.GetProcessesByName("Spotify");

                    foreach(var p in proc.Where(p => p.MainWindowTitle != string.Empty)) {
                        client.SendMessage($"{IRC.BOLD}[🎵]:{IRC.BOLD}{IRC.CYAN} Now playing: {p.MainWindowTitle}", e.PrivateMessage.Source);
                    }
                }
            };

            do
            {
                Thread.Sleep(100);
            }
            while(!MenuEnabled);

            var command = string.Empty;
            do
            {
                Console.ReadKey(true);
                Console.Write("> ");
                command = Console.ReadLine();
                switch(command)
                {
                    case "exit":
                        break;
                    case "say":
                        Tools.ColoredWrite(ConsoleColor.DarkGray, "--- say");
                        Tools.ColoredWrite(ConsoleColor.Cyan, "Enter message to send:");
                        var msg = Console.ReadLine();
                        Tools.ColoredWrite(ConsoleColor.Cyan, "Enter channel to send to:");
                        var channel = Console.ReadLine();
                        client.Channels[channel].SendMessage(msg);
                        Tools.ColoredWrite(ConsoleColor.DarkGray, "--- end_say");
                        continue;
                    case "join":
                        Tools.ColoredWrite(ConsoleColor.DarkGray, "--- join");
                        Tools.ColoredWrite(ConsoleColor.Cyan, "Enter channel to join:");
                        client.JoinChannel(Console.ReadLine());
                        Tools.ColoredWrite(ConsoleColor.DarkGray, "--- end_join");
                        continue;
                    case "part":
                        Tools.ColoredWrite(ConsoleColor.DarkGray, "--- part");
                        Tools.ColoredWrite(ConsoleColor.Cyan, "Enter channel to part:");
                        client.PartChannel(Console.ReadLine());
                        Tools.ColoredWrite(ConsoleColor.DarkGray, "--- end_part");
                        continue;
                    default:
                        Tools.ColoredWrite(ConsoleColor.DarkGray, "--- err");
                        Tools.ColoredWrite(ConsoleColor.Red, "Invalid command.");
                        Tools.ColoredWrite(ConsoleColor.DarkGray, "--- end_err");
                        continue;
                }
            }
            while(command != "exit");
        }
    }
}