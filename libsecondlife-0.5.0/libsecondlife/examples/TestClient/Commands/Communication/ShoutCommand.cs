using System;
using System.Collections.Generic;
using System.Text;
using libsecondlife;
using libsecondlife.Packets;

namespace libsecondlife.TestClient
{
    public class ShoutCommand : Command
    {
        public ShoutCommand(TestClient testClient)
        {
            Name = "shout";
            Description = "Shout something.";
        }

        public override string Execute(string[] args, LLUUID fromAgentID)
        {
            int channel = 0;
            int startIndex = 0;
            string message = String.Empty;
            if (args.Length < 1)
            {
                return "usage: shout (optional channel) whatever";
            }
            else if (args.Length > 1)
            {
                try
                {
                    channel = Convert.ToInt32(args[0]);
                    startIndex = 1;
                }
                catch (FormatException)
                {
                    channel = 0;
                }
            }

            for (int i = startIndex; i < args.Length; i++)
            {
                message += args[i] + " ";
            }

            Client.Self.Chat(message, channel, ChatType.Shout);

            return "Shouted " + message;
        }
    }
}
