using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Lidgren.Network;

namespace ScoopFramework.Network
{
    public class NetOutgoingMessageWithDeliveryMethod
    {
        public NetOutgoingMessage OutgoingMessage;
        public NetDeliveryMethod DeliveryMethod;
        public int? TargetHost;

        public NetOutgoingMessageWithDeliveryMethod(NetOutgoingMessage outgoingMessage, NetDeliveryMethod deliveryMethod, int? targetHost = null)
        {
            OutgoingMessage = outgoingMessage;
            DeliveryMethod = deliveryMethod;
            TargetHost = targetHost;
        }
    }

    public class NetworkManager : IDisposable
    {
        public bool IsConnected
        {
            get
            {
                return peer.Connections.Count > 0;
            }
        }
        public int CountConnections
        {
            get
            {
                return peer.Connections.Count;
            }
        }

        readonly int port;
        readonly string appName;

        Thread worker;
        bool threadAlive;
        bool workerThreadFinished;

        NetPeerConfiguration config;
        NetPeer peer;
        NetXtea crypto;

        Queue<string> connectionsToOpen;
        bool runPeerDiscovery;

        Queue<NetIncomingMessage> received;
        Queue<NetOutgoingMessageWithDeliveryMethod> toSend;
        NetOutgoingMessageWithDeliveryMethod outMessage;
        NetIncomingMessage inMessage;

        public const int MaxConnections = 128;


        public NetworkManager(string appName, int port, string cryptoKey)
        {
            this.appName = appName;
            this.port = port;

            this.config = new NetPeerConfiguration(appName);
            this.config.Port = port;
            this.config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            this.config.AcceptIncomingConnections = true;
            this.crypto = new NetXtea(cryptoKey);
            this.connectionsToOpen = new Queue<string>();
            this.received = new Queue<NetIncomingMessage>();
            this.toSend = new Queue<NetOutgoingMessageWithDeliveryMethod>();

            this.peer = new NetPeer(config);
            this.peer.Start();


            this.worker = new Thread(new ThreadStart(WorkerProcess));
            this.worker.Start();
        }

        public void Connect(string ip)
        {
            lock (connectionsToOpen)
                connectionsToOpen.Enqueue(ip);
        }

        public void EnablePeerDiscovery()
        {
            runPeerDiscovery = true;
        }

        public void DisablePeerDiscovery()
        {
            runPeerDiscovery = false;
        }

        public NetOutgoingMessageWithDeliveryMethod CreateMessage(MessageTypes type, NetDeliveryMethod method = NetDeliveryMethod.ReliableOrdered)
        {
            NetOutgoingMessageWithDeliveryMethod returnValue = new NetOutgoingMessageWithDeliveryMethod(peer.CreateMessage(), method);
            returnValue.OutgoingMessage.Write((byte)type);
            return returnValue;
        }

        public NetIncomingMessage Receive()
        {
            if (received.Count > 0)
                lock (received)
                    return received.Dequeue();
            else
                return null;
        }

        public void Send(NetOutgoingMessageWithDeliveryMethod msg)
        {
            lock (toSend)
                toSend.Enqueue(msg);
        }

        public void Dispose()
        {
            threadAlive = false;
            Thread.Sleep(1);
            worker.Abort();
            while (!workerThreadFinished)
                Thread.Sleep(1);
        }



        void WorkerProcess()
        {
            threadAlive = true;

            while (threadAlive)
            {
                if (peer == null)
                {
                    Thread.Sleep(33);
                    continue;
                }

                HandleNetEvents();
                HandleLocalEvents();
            }
            if (peer != null)
                peer.Shutdown("Worker thread died.");
            workerThreadFinished = true;
        }

        void HandleLocalEvents()
        {
            while (toSend.Count > 0)
            {
                lock (toSend)
                    outMessage = toSend.Dequeue();
                if (peer.ConnectionsCount > 0)
                {
                    outMessage.OutgoingMessage.Encrypt(crypto);

                    if (outMessage.TargetHost == null)
                        peer.SendMessage(outMessage.OutgoingMessage, peer.Connections, outMessage.DeliveryMethod, 0);
                    else//BUG: send message to specific doesn't work (in games with more than two players)
                        peer.SendMessage(outMessage.OutgoingMessage,peer.Connections[(int)outMessage.TargetHost] , outMessage.DeliveryMethod, 0);
                }
            }

            while (connectionsToOpen.Count > 0)
            {
                string ip;
                lock (connectionsToOpen)
                    ip = connectionsToOpen.Dequeue();
                ConnectTo(ip);
            }

            if (runPeerDiscovery)
            {
                peer.Configuration.SetMessageTypeEnabled(NetIncomingMessageType.DiscoveryRequest, false);//we don't want to answer our own request
                peer.DiscoverLocalPeers(port);
                var response = peer.WaitMessage(5000);
                if (response != null && response.MessageType == NetIncomingMessageType.DiscoveryResponse)
                {
                    string ip = response.SenderEndPoint.Address.ToString();
                    peer.Configuration.SetMessageTypeEnabled(NetIncomingMessageType.DiscoveryRequest, true);
                    ConnectTo(ip);
                }

                runPeerDiscovery = false;
            }
        }

        void HandleNetEvents()
        {
            while ((inMessage = peer.ReadMessage()) != null)
            {
                inMessage.Decrypt(crypto);

                switch (inMessage.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        var msg = peer.CreateMessage();
                        msg.Write((byte)MessageTypes.Null);
                        inMessage.SenderConnection.Approve(msg);
                        Console.WriteLine("A connection was approved.");
                        break;

                    case NetIncomingMessageType.Data:
                        try
                        {
                            switch ((MessageTypes)inMessage.PeekByte())
                            {
                                case MessageTypes.PeerListRequest:
                                    SendPeerList(inMessage.SenderConnection);
                                    break;
                                case MessageTypes.PeerListAnswer:
                                    inMessage.ReadByte();
                                    ReceivePeerList(inMessage);
                                    break;
                                case MessageTypes.Null:
                                    Console.WriteLine("Received null data.");
                                    break;
                                default:
                                    lock (received)
                                        received.Enqueue(inMessage);
                                    break;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Received unhandled data.");
                        }
                        break;

                    case NetIncomingMessageType.DiscoveryRequest:
                        msg = peer.CreateMessage();
                        msg.Write((byte)MessageTypes.Null);
                        peer.SendDiscoveryResponse(msg, inMessage.SenderEndPoint);
                        Console.WriteLine("Answered discovery request.");
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)inMessage.ReadByte();
                        switch (status)
                        {
                            case NetConnectionStatus.Connected:
                                Console.WriteLine("Peer connected.");
                                peer.Connections.Add(inMessage.SenderConnection);
                                RequestPeerList();
                                break;
                            case NetConnectionStatus.Disconnected:
                                Console.WriteLine("Peer disconnected.");
                                break;
                            default:
                                try
                                {
                                    Console.WriteLine("StatusChanged: " + Enum.GetName(typeof(NetConnectionStatus), status));
                                }
                                catch
                                {
                                    Console.WriteLine("StatusChanged: " + status);
                                }
                                break;
                        }
                        break;

                    default:
                        Console.WriteLine("Default case: " + System.Text.Encoding.Default.GetString(inMessage.Data));
                        break;
                }
            }
        }

        bool ConnectTo(string ip)
        {
            peer.Connect(ip, port);

            NetIncomingMessage info = null;

            for (int t = 0; t < 1000; t++)
            {
                if ((info = peer.ReadMessage()) != null)
                    break;
                Thread.Sleep(10);
            }

            return info != null;
        }

        void RequestPeerList()
        {
            var msg = peer.CreateMessage();
            msg.Write((byte)MessageTypes.PeerListRequest);
            peer.SendMessage(msg, peer.Connections, NetDeliveryMethod.ReliableOrdered, 0);//XXX: maybe change to peer.Connections[0]
            Console.WriteLine("Requested peer list.");
        }

        void SendPeerList(NetConnection receiver)
        {
            var peerListMessage = peer.CreateMessage();
            peerListMessage.Write((byte)MessageTypes.PeerListAnswer);

            List<string> conn = new List<string>();
            foreach (var elem in peer.Connections)
                conn.Add(elem.RemoteEndPoint.Address.ToString());
            conn.Remove(receiver.RemoteEndPoint.Address.ToString());
            peerListMessage.WriteVariableInt32(conn.Count);
            foreach (var elem in conn)
                peerListMessage.Write(elem);

            peer.SendMessage(peerListMessage, receiver, NetDeliveryMethod.ReliableOrdered);
            Console.WriteLine("Peer list was sent.");
        }

        void ReceivePeerList(NetIncomingMessage message)
        {
            List<string> ips = new List<string>();
            int count = message.ReadVariableInt32();
            Console.WriteLine("Received peer list (" + count + ").");
            for (int i = 0; i < count; i++)
                ips.Add(message.ReadString());


            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("Progressing IP " + (i + 1) + "/" + count + "...");
                if (!peer.Connections.Any(p => p.RemoteEndPoint.Address.ToString() == ips[i]))
                {
                    Console.WriteLine("Initializing connect");
                    ConnectTo(ips[i]);
                }
                else
                    Console.WriteLine("Already connected.");
            }
        }
    }
}
