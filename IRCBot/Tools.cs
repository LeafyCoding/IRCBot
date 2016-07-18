// -----------------------------------------------------------
// This program is private software, based on C# source code.
// To sell or change credits of this software is forbidden,
// except if someone approves it from the LeafyCoding INC. team.
// -----------------------------------------------------------
// Copyrights (c) 2016 IRCBot INC. All rights reserved.
// -----------------------------------------------------------

using System;

namespace IRCBot
{
    internal class Tools
    {
        public static void ColoredWrite(ConsoleColor color, string text)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text + Environment.NewLine);
            Console.ForegroundColor = originalColor;
        }

        public static void SemiColoredWrite(ConsoleColor color, string coloredText, string noColorText)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(coloredText);
            Console.ForegroundColor = originalColor;
            Console.Write(noColorText + Environment.NewLine);
        }

        public static string Time() => $"[{DateTime.Now.ToString("dd-MM-yyyy @ HH:mm")}] ";
    }
}