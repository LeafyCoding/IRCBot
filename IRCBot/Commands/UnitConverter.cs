// -----------------------------------------------------------
// This program is private software, based on C# source code.
// To sell or change credits of this software is forbidden,
// except if someone approves it from the LeafyCoding INC. team.
// -----------------------------------------------------------
// Copyrights (c) 2016 IRCBot INC. All rights reserved.
// -----------------------------------------------------------

using System;

using ChatSharp.Events;

using IRCBot.Units;

namespace IRCBot.Commands
{
    internal class UnitConverter
    {
        public static void Convert(PrivateMessageEventArgs e)
        {
            var success = false;

            double amount = 0;
            string unit1;
            string unit2;

            var msg = e.PrivateMessage.Message.Split(' ');

            try
            {
                amount = double.Parse(msg[1]);
                unit1 = msg[2];
                unit2 = msg[3];
            }
            catch(Exception)
            {
                Program.client.SendMessage($"{IRC.BOLD}{IRC.RED}ERROR: Cannot parse value. Usage: !cc <amount> <base currency> <desired currency>",
                    e.PrivateMessage.Source);
                return;
            }

            if(unit1.ToLower().Equals("kg") || unit1.ToLower().Equals("lbs") || unit1.ToLower().Equals("impton") || unit1.ToLower().Equals("uston")
               || unit1.ToLower().Equals("stone") || unit1.ToLower().Equals("ounce"))
            {
                if(ConvertWeight(amount, unit1, unit2, e))
                {
                    success = true;
                }
            }

            /*if(!success)
            {
                Program.client.SendMessage($"{IRC.BOLD}[🍂]:{IRC.BOLD}{IRC.RED} Something went wrong :( One or both of the currencies may be unsupported :(",
                    e.PrivateMessage.Source);
            }*/
        }

        private static bool ConvertWeight(double amount, string unit1, string unit2, PrivateMessageEventArgs e)
        {
            if(unit1.ToLower().Equals("kg"))
            {
                double response = 0;
                var success = false;

                switch(unit2.ToLower())
                {
                    case "lbs":
                        response = amount * Values.lbs;
                        success = true;
                        break;
                    case "impton":
                        response = amount * Values.impton;
                        success = true;
                        break;
                    case "uston":
                        response = amount * Values.uston;
                        success = true;
                        break;
                    case "stone":
                        response = amount * Values.stone;
                        success = true;
                        break;
                    case "ounce":
                        response = amount * Values.ounce;
                        success = true;
                        break;
                    case "kg":
                        response = amount;
                        success = true;
                        break;
                    default:
                        Program.client.SendMessage(
                            $"{IRC.BOLD}[🍂]:{IRC.BOLD}{IRC.RED} Something went wrong :( One or both of the units may be unsupported :(",
                            e.PrivateMessage.Source);
                        break;
                }

                if(success)
                {
                    Program.client.SendMessage($"{IRC.BOLD}[🍂]:{IRC.BOLD} {amount} {unit1} ↔ {response} {unit2}", e.PrivateMessage.Source);
                    return true;
                }
            }
            else
            {
                double kgvalue = 0;
                bool success;

                switch(unit1)
                {
                    case "lbs":
                        kgvalue = amount * (1 / Values.lbs);
                        success = true;
                        break;
                    case "impton":
                        kgvalue = amount * (1 / Values.lbs);
                        success = true;
                        break;
                    case "uston":
                        kgvalue = amount * (1 / Values.lbs);
                        success = true;
                        break;
                    case "stone":
                        kgvalue = amount * (1 / Values.lbs);
                        success = true;
                        break;
                    case "ounce":
                        kgvalue = amount * (1 / Values.lbs);
                        success = true;
                        break;
                    default:
                        Program.client.SendMessage(
                            $"{IRC.BOLD}[🍂]:{IRC.BOLD}{IRC.RED} Something went wrong :( One or both of the units may be unsupported :(",
                            e.PrivateMessage.Source);
                        success = false;
                        break;
                }

                if(!success)
                {
                    return false;
                }
                
                double response = 0;
                success = false;

                switch (unit2)
                {
                    case "lbs":
                        response = kgvalue * (1 / Values.lbs);
                        success = true;
                        break;
                    case "impton":
                        response = kgvalue * (1 / Values.impton);
                        success = true;
                        break;
                    case "uston":
                        response = kgvalue * (1 / Values.uston);
                        success = true;
                        break;
                    case "stone":
                        response = kgvalue * (1 / Values.stone);
                        success = true;
                        break;
                    case "ounce":
                        response = kgvalue * (1 / Values.ounce);
                        success = true;
                        break;
                    case "kg":
                        response = kgvalue * (1 / Values.kg);
                        success = true;
                        break;
                    default:
                        Program.client.SendMessage(
                            $"{IRC.BOLD}[🍂]:{IRC.BOLD}{IRC.RED} Something went wrong :( One or both of the units may be unsupported :(",
                            e.PrivateMessage.Source);
                        success = false;
                        break;
                }

                if (success)
                {
                    Program.client.SendMessage($"{IRC.BOLD}[🍂]:{IRC.BOLD} {amount} {unit1} ↔ {response} {unit2}", e.PrivateMessage.Source);
                    return true;
                }
            }

            return false;
        }
    }
}