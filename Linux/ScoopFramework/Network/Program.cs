using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace ScoopFramework.Network
{
    class Program
    {
        enum MessageType:byte
        {
            String,PeerList
        }

        static void Main(string[] args)
        {
            Program app = new Program();
            app.Initialize();
            while (true)
                app.Update();
        }

        NetPeer peer;
        NetPeerConfiguration config;
        NetIncomingMessage message;
        NetXtea encryptor;

        string userName="";
        string cryptoKey = "klsdlwe5fd41ef68974esd5c31df68g4rds65f4d89th4z5k4i89l4ui8964kj564rw384t56567u1756io4681tg5re4wf896ztu4k561df68ds4yc65a<14e89664r6ew854z85966u45t41fsd894r86345t4h8ztu4j65sdyd14f89aw3445865546i87o4k5j48io9pü486i4o89p6ä4pü8476864ewsd4658fb4m65,8848u4redc48x4v65b4u89i45u5ef468asc41568e4zu8694i5f16s8vd48a9er34w6a";
        int port = 55566;


        private void Update()
        {
            if (peer != null)
                UpdatePeer();
        }

        private void UpdatePeer()
        {
            while ((message = peer.ReadMessage()) != null)
            {
                message.Decrypt(encryptor);

                switch (message.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        message.SenderConnection.Approve(peer.CreateMessage("Hi!"));
                        Console.WriteLine("A connection was approved.");
                        break;

                    case NetIncomingMessageType.Data:
                        HandleData(message);
                        break;

                    case NetIncomingMessageType.DiscoveryRequest:
                        peer.SendDiscoveryResponse(peer.CreateMessage("Hi!"), message.SenderEndpoint);
                        Console.WriteLine("Answered discovery request!");
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)message.ReadByte();
                        switch (status)
                        {
                            case NetConnectionStatus.Connected:
                                Console.WriteLine("Peer connected.");
                                SendPeerList(message);
                                break;
                            case NetConnectionStatus.Disconnected:
                                Console.WriteLine("Peer disconnected.");
                                break;
                            default:
                                Console.WriteLine("StatusChanged: " + status);
                                break;
                        }
                        break;

                    default:
                        Console.WriteLine("Default case: " + message.MessageType);
                        break;
                }
            }
            if (Console.KeyAvailable)
            {
                string chat = Console.ReadKey().KeyChar.ToString();
                var send = peer.CreateMessage();
                send.Write((byte)MessageType.String);
                chat += Console.ReadLine();
                send.Write("[" + userName + "]: " + chat.ToString());
                send.Encrypt(encryptor);
                if (peer.ConnectionsCount > 0)
                    peer.SendMessage(send, peer.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                else
                    Console.WriteLine("You are not connected to anyone!");
            }
        }

        private void SendPeerList(NetIncomingMessage message)
        {
            var peerListMessage = peer.CreateMessage();
            peerListMessage.Write((byte)MessageType.PeerList);
            List<string> conn = new List<string>();
            foreach (var elem in peer.Connections)
                conn.Add(elem.RemoteEndpoint.Address.ToString());
            conn.Remove(message.SenderEndpoint.Address.ToString());

            peerListMessage.WriteVariableInt32(conn.Count);
            foreach (var elem in conn)
                peerListMessage.Write(elem);

            peer.SendMessage(peerListMessage, message.SenderConnection, NetDeliveryMethod.ReliableOrdered);
        }

        private void HandleData(NetIncomingMessage message)
        {
            try
            {
                MessageType type = (MessageType)message.ReadByte();
                switch (type)
                {
                    case MessageType.String:
                        Console.WriteLine(message.ReadString());
                        break;

                    case MessageType.PeerList:
                        List<string> ips = new List<string>();
                        int count = message.ReadVariableInt32();
                        for (int i = 0; i < count; i++)
                            ips.Add(message.ReadString());

                        Console.WriteLine("Received peer list.");
                        foreach (var elem in ips)
                        {
                            if (!peer.Connections.Any(p => p.RemoteEndpoint.Address.ToString() == elem))
                                ConnectTo(elem);
                        }
                        Console.WriteLine("Finished peer list.");
                        break;

                    default:
                        Console.WriteLine("Received unhandled data.");
                        break;
                }
            }
            catch
            {
                Console.WriteLine("Unreadable data received.");
            }
        }

        private void Initialize()
        {
            while (userName == "")
            {
                Console.Write("Username: ");
                userName = Console.ReadLine();
            }

            config = new NetPeerConfiguration("TestChatPeer");
            config.MaximumConnections = 32;

            encryptor = new NetXtea(cryptoKey);

            ConsoleKey answer = ConsoleKey.Spacebar;
            while (answer !=  ConsoleKey.L && answer !=  ConsoleKey.J)
            {
                Console.WriteLine("Create a lonely (l) peer or join (j) a peer?");
                answer = Console.ReadKey().Key;
            }

            CreateNewPeer();

            if (answer == ConsoleKey.J)
                JoinPeer();

            Console.WriteLine("\nDone.");
        }

        private void CreateNewPeer()
        {
            config.Port = port;
            config.SetMessageTypeEnabled(NetIncomingMessageType.ConnectionApproval, true);
            peer = new NetPeer(config);
            peer.Start();
        }

        private void JoinPeer()
        {
            while (true)
            {
                Console.Write("Peer's IP (type \"a\" for peer discovery): ");
                string ip = Console.ReadLine();

                if (ip == "")
                    continue;
                else if (ip.ToLower() == "a")
                {
                    peer.Configuration.SetMessageTypeEnabled(NetIncomingMessageType.DiscoveryRequest, false);//we don't want to answer our own request
                    peer.DiscoverLocalPeers(port);
                    var response = peer.WaitMessage(5000);
                    if (response != null && response.MessageType == NetIncomingMessageType.DiscoveryResponse)
                    {
                        ip = response.SenderEndpoint.Address.ToString();
                        peer.Configuration.SetMessageTypeEnabled(NetIncomingMessageType.DiscoveryRequest, true);
                    }
                    else
                        continue;
                }

                if (ConnectTo(ip))
                    break;
            }
        }



        private bool ConnectTo(string ip)
        {
            Console.Write("Connecting to " + ip + "...");
            peer.Connect(ip, port, peer.CreateMessage("Hi!"));
            var info = peer.WaitMessage(5000);
            if (info != null)
            {
                peer.Connections.Add(info.SenderConnection);
                Console.WriteLine("   done.");
                return true;
            }
            else
            {
                Console.WriteLine("   failed.");
                return false;
            }
        }
    }
}