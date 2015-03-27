using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linux.Code
{
    public class SyncObject
    {
        static short IdCounter=(short)Server.Commands.LastSystemCommand+1;
        public readonly short ID=IdCounter++;

        Server Server;

        public SyncObject(Server server) { Server = server; }

        public delegate void MessageReceivedHandler(Player sender, NetBuffer buffer);
        public delegate void LateMessageReceivedHandler(Player sender, uint frame, NetBuffer buffer);
        public MessageReceivedHandler MessageReceived;
        public LateMessageReceivedHandler LateMessageReceived;

        uint messageFrame = 0;
        public NetOutgoingMessage CreateMessage(uint frame)
        {
            var message = Server.Peer.CreateMessage();
            Server.WriteMessageHeader(message, ID, frame);
            if (frame != 0) throw new Exception("Only one message at a time!");
            messageFrame = frame;
            return message;
        }
        public void SendMessage(NetOutgoingMessage message)
        {
            if (Server.RecognizedConnections.Count > 0)
                Server.Peer.SendMessage(message, Server.RecognizedConnections.Select(p => p.Connection).ToList(), NetDeliveryMethod.ReliableOrdered, 0);
            message.Position = 0;
            Server.MessageQueue.Add(new Server.Message() {sender=GameControl.LocalPlayer, frame=messageFrame, command=ID, msg=message });
            messageFrame = 0;
        }
    }
}
