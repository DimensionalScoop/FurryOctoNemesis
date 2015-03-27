using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Net;
using System.Threading;

namespace Linux.Code
{
    public class Server:GameComponent
    {
        const int Port = 31415;
        static readonly TimeSpan DiscoveryTimeout = TimeSpan.FromMilliseconds(500);
        static readonly TimeSpan ConnectingTimeout = TimeSpan.FromMilliseconds(5000);

        Guid ID = Guid.NewGuid();
        
        NetPeerConfiguration Config;
        NetPeer Peer;
        class NetPlayer { public NetConnection Connection; public Player Player; public NetPlayer(NetConnection conn, Player player) { Connection = conn; Player = player; } }
        List<NetPlayer> RecognizedConnections = new List<NetPlayer>();
        NetIncomingMessage incMessage;


        public Server(Game game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();
            Console.WriteLine("--------------------");
            Console.WriteLine("Initializing Network...");

            Config = new NetPeerConfiguration("FurryOctoNemesis");
            Config.Port = Port;
            Config.AcceptIncomingConnections = true;
            Config.EnableUPnP = true;
            Peer = new NetPeer(Config);
            Peer.Start();

            Console.WriteLine("Client discovery...");
            Config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            Config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            Peer.DiscoverLocalPeers(Port);
            DateTime timer = DateTime.Now;
            while (DateTime.Now - timer < DiscoveryTimeout)
            {
                incMessage = Peer.ReadMessage();
                if(incMessage!=null)
                if (incMessage.MessageType == NetIncomingMessageType.DiscoveryResponse)
                {
                    Console.WriteLine("Found other client, ip: " + incMessage.SenderEndPoint.Address.ToString());

                    if (TryConnect(Peer.Connect(incMessage.SenderEndPoint))) return;
                    break;
                }
            }

            Console.WriteLine("Client discovery was not successful after " + DiscoveryTimeout.TotalMilliseconds + "ms");

            while (true)
            {
                Console.WriteLine("Press enter if you are the first client or input the ip of another client");
                Console.Write("%: ");
                var ip = Console.ReadLine();
                if (ip == "")
                {
                    InitializeServer();
                    break;
                }
                else
                {
                    if (TryConnect(Peer.Connect(ip,Port)))
                        break;
                }
            }
        }

        private bool TryConnect(NetConnection server)
        {
            RecognizedConnections.Clear();
            Console.WriteLine("Connecting to peer...");

            Thread.Sleep(1000);

            var msg = Peer.CreateMessage();
            WriteMessageHeader(msg, Commands.OpenFirstConnection);
            Peer.SendMessage(msg, server, NetDeliveryMethod.ReliableOrdered);
            
            Console.WriteLine("Waiting for response...");
            
            DateTime timer = DateTime.Now;
            while (DateTime.Now - timer < ConnectingTimeout)
            {
                while ((incMessage = Peer.ReadMessage()) != null)
                {
                    switch (incMessage.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            Console.WriteLine("[Lidgren]: Receiving data...");
                            ReceiveMessage();
                            break;

                        default:
                            Console.WriteLine("[Lidgren]: Unhandled message type: " + incMessage.MessageType);
                            break;
                    }
                    Peer.Recycle(incMessage);
                }
            }
            if (RecognizedConnections.Count > 0)
            {
                AutoChoosePlayer();
                msg = Peer.CreateMessage();
                WriteMessageHeader(msg, Commands.PlayerDataUpdate);
                GameControl.LocalPlayer.WriteToStream(msg);
                Peer.SendMessage(msg, RecognizedConnections.Select(p => p.Connection).ToList(), NetDeliveryMethod.ReliableOrdered, 0);
                ShowConnectedPplayers();
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private void AutoChoosePlayer()
        {
            for (int i = 0; i < Player.Colors.Length; i++)
                if (GameControl.AllPlayers.All(p => p.Color != Player.Colors[i]))
                {
                    GameControl.LocalPlayer.Color = Player.Colors[i];
                    break;
                }

            if (GameControl.LocalPlayer.Color == Color.Black) throw new Exception("Too many players!");
            
            for (byte i = 0; i < byte.MaxValue; i++)
                if (GameControl.AllPlayers.All(p => p.NetID != i))
                {
                    GameControl.LocalPlayer.NetID = i;
                    break;
                }
        }

        private void InitializeServer()
        {
            GameControl.LocalPlayer.NetID = 1;
            GameControl.LocalPlayer.Color = Player.Colors[0];
        }

        public override void Update(GameTime gameTime)
        {
            while ((incMessage = Peer.ReadMessage()) != null)
            {
                switch (incMessage.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine("[Lidgren]: "+incMessage.ReadString());
                        break;

                    case NetIncomingMessageType.DiscoveryRequest:
                        var msg = Peer.CreateMessage();
                        WriteMessageHeader(msg, Commands.Null);
                        Peer.SendDiscoveryResponse(msg, incMessage.SenderEndPoint);
                        break;

                    case NetIncomingMessageType.ConnectionApproval:
                        Console.WriteLine("[Lidgren]: Approving connection.");
                        incMessage.SenderConnection.Approve();
                        break;

                    case NetIncomingMessageType.Data:
                        Console.WriteLine("[Lidgren]: Incoming data.");
                        ReceiveMessage();
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        switch ((NetConnectionStatus)incMessage.ReadByte())
                        {
                            case NetConnectionStatus.InitiatedConnect: Console.WriteLine("[Lidgren]: Initiating connection with "+incMessage.SenderEndPoint.Address); break;
                            case NetConnectionStatus.Connected: Console.WriteLine("[Lidgren]: Connection established with " + incMessage.SenderEndPoint.Address); break;
                            case NetConnectionStatus.Disconnected: Console.WriteLine("[Lidgren]: Disconnected from " + incMessage.SenderEndPoint.Address); break;
                            default: Console.WriteLine("[Lidgren]: Status " + incMessage.ReadString()); break;
                        }
                        break;

                    default:
                        Console.WriteLine("[Lidgren]: Unhandled message type: " + incMessage.MessageType);
                        Console.WriteLine("[Lidgren]: " + incMessage.ReadString());
                        break;


                }

                Peer.Recycle(incMessage);
            }

            base.Update(gameTime);
        }

        private void SendPlayerDataTo(NetConnection netConnection)
        {
            var msg = Peer.CreateMessage();
            WriteMessageHeader(msg, Commands.SendPlayerDataTo);
            msg.Write(netConnection.RemoteEndPoint.Address.ToString());
            if (RecognizedConnections.Count > 0)
                Peer.SendMessage(msg, RecognizedConnections.Select(p => p.Connection).ToList(), NetDeliveryMethod.ReliableOrdered, 0);

            Console.WriteLine("[Lidgren]: Sending player data");
            msg = Peer.CreateMessage();
            WriteMessageHeader(msg, Commands.PlayerDataUpdate);
            GameControl.LocalPlayer.WriteToStream(msg);
            Peer.SendMessage(msg, netConnection, NetDeliveryMethod.ReliableOrdered);
        }

        private void WriteMessageHeader(NetOutgoingMessage msg, Commands command, uint frame = 0)
        {
            msg.Write((byte)GameControl.LocalPlayer.NetID);
            msg.Write(frame);
            msg.Write((short)command);
        }

        public enum Commands:short
        {
            Null,
            Error,
            OpenFirstConnection,
            SendPlayerDataTo,
            PlayerDataUpdate,

        }

        void ReceiveMessage()
        {
            byte senderNetID = incMessage.ReadByte();
            uint frame=incMessage.ReadUInt32();
            short command = incMessage.ReadInt16();

            switch ((Commands)command)
            {
                case Commands.Null:
                    Console.WriteLine("[Lidegren]: Null received.");
                    break;

                case Commands.OpenFirstConnection:
                    Console.WriteLine("[Lidgren]: Sending request to send player data.");    
                    SendPlayerDataTo(incMessage.SenderConnection);
                    break;

                case Commands.SendPlayerDataTo:
                    Console.WriteLine("[Lidgren]: Sending player data");
                    var ip = incMessage.ReadString();
                    var connection= Peer.Connect(ip,Port);
                    var msg = Peer.CreateMessage();
                    WriteMessageHeader(msg,Commands.PlayerDataUpdate);
                    msg.WriteAllFields(GameControl.LocalPlayer, System.Reflection.BindingFlags.Public);
                    Peer.SendMessage(msg, connection, NetDeliveryMethod.ReliableOrdered);
                    break;

                case Commands.PlayerDataUpdate:
                    Console.WriteLine("[Lidgren]: Receiving update on player data.");
                    if (senderNetID == GameControl.LocalPlayer.NetID) break;

                    var sender = GameControl.GetPlayerByID(senderNetID);
                    if (sender == null)
                    {
                        var player = new Player();
                        player.ReadFromStream(incMessage);
                        GameControl.AllPlayers.Add(player);
                        RecognizedConnections.Add(new NetPlayer(incMessage.SenderConnection, player));
                    }
                    else
                    {
                        sender.ReadFromStream(incMessage);
                    }
                    ShowConnectedPplayers();
                    break;

                default:
                    Console.WriteLine("[Lidgren]: Command not implemented!");
                    throw new NotImplementedException();
                    break;

            }
        }

        private void ShowConnectedPplayers()
        {
            Console.WriteLine("[Lidgren]: Recognized Players:");
            Console.WriteLine("[Lidgren]: NetID " + GameControl.LocalPlayer.NetID + ", " + GameControl.LocalPlayer.Color.ToString() + ", IP: local");
            foreach (var player in RecognizedConnections)
                Console.WriteLine("[Lidgren]: NetID " + player.Player.NetID + ", " + player.Player.Color.ToString() + ", IP: " + player.Connection.RemoteEndPoint.Address.ToString());
        }
    }
}
