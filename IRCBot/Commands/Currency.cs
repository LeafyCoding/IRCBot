// -----------------------------------------------------------
// This program is private software, based on C# source code.
// To sell or change credits of this software is forbidden,
// except if someone approves it from the LeafyCoding INC. team.
// -----------------------------------------------------------
// Copyrights (c) 2016 IRCBot INC. All rights reserved.
// -----------------------------------------------------------

using System;
using System.Net;

using ChatSharp.Events;

using Newtonsoft.Json.Linq;

namespace IRCBot.Commands
{
    internal class Currency
    {
        public static void Convert(PrivateMessageEventArgs e)
        {
            var success = false;

            double amount = 0;
            string curr1;
            string curr2;

            var msg = e.PrivateMessage.Message.Split(' ');

            try
            {
                amount = double.Parse(msg[1]);
                curr1 = msg[2];
                curr2 = msg[3];
            }
            catch(Exception)
            {
                Program.client.SendMessage($"{IRC.BOLD}{IRC.RED}ERROR: Cannot parse value. Usage: !cc <amount> <base currency> <desired currency>",
                    e.PrivateMessage.Source);
                return;
            }

            var globalvalues = new WebClient().DownloadString(new Uri("http://www.apilayer.net/api/live?access_key=ec9e918c4ccd18199fb51d991aa0274a"));
            dynamic data = JObject.Parse(globalvalues);

            if(curr1.ToLower() == "usd") // Value is already USD, no primary conversion needed.
            {
                foreach(var quote in data.quotes)
                {
                    if(quote.Name.ToString().ToLower() == $"usd{curr2}")
                    {
                        var response = amount * double.Parse(quote.Value.ToString());
                        Program.client.SendMessage($"{IRC.BOLD}[🍂]:{IRC.BOLD} {amount} {curr1.ToUpper()} ↔ {response} {curr2.ToUpper()}",
                            e.PrivateMessage.Source);
                        success = true;
                        break;
                    }
                }
            }
            else
            {
                double usdvalue = 0;

                foreach(var quote in data.quotes)
                {
                    if(quote.Name.ToString().ToLower() == $"usd{curr1.ToLower()}")
                    {
                        usdvalue = amount * (1 / double.Parse(quote.Value.ToString())); // Convert to USD
                        break;
                    }
                }

                foreach(var quote in data.quotes)
                {
                    if(quote.Name.ToString().ToLower() == $"usd{curr2}")
                    {
                        var response = usdvalue * double.Parse(quote.Value.ToString()); // Convert to curr2
                        Program.client.SendMessage($"{IRC.BOLD}[🍂]:{IRC.BOLD} {amount} {curr1.ToUpper()} ↔ {response} {curr2.ToUpper()}",
                            e.PrivateMessage.Source);
                        success = true;
                        break;
                    }
                }
            }

            if(!success)
            {
                Program.client.SendMessage($"{IRC.BOLD}[🍂]:{IRC.BOLD}{IRC.RED} Something went wrong :( One or both of the currencies may be unsupported :(",
                    e.PrivateMessage.Source);
            }
        }
    }
}