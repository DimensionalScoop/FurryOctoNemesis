//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Lidgren.Network;

//namespace X45Game.Network
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Program app = new Program();
//            app.Initialize();
//            while (true)
//                app.Update();
//        }


//        NetServer server;
//        NetClient client;
//        NetPeerConfiguration config;
//        NetIncomingMessage message;
//        NetXtea encryptor;

//        string userName="default";
//        string cryptoKey = "klsdlwe5fd41ef68974esd5c31df68g4rds65f4d89th4z5k4i89l4ui8964kj564rw384t56567u1756io4681tg5re4wf896ztu4k561df68ds4yc65a<14e89664r6ew854z85966u45t41fsd894r86345t4h8ztu4j65sdyd14f89aw3445865546i87o4k5j48io9pü486i4o89p6ä4pü8476864ewsd4658fb4m65,8848u4redc48x4v65b4u89i45u5ef468asc41568e4zu8694i5f16s8vd48a9er34w6a";
//        int port = 55566;


//        private void Update()
//        {
//            if (server != null)
//                UpdateServer();
//            if (client != null)
//                UpdateClient();
//        }

//        private void UpdateClient()
//        {
//            while ((message=client.ReadMessage())!=null)
//            {
//                message.Decrypt(encryptor);

//                switch (message.MessageType)
//                {
//                    case NetIncomingMessageType.ConnectionApproval:
//                        message.SenderConnection.Approve();
//                        break;

//                    case NetIncomingMessageType.Data:
//                        Console.WriteLine(message.ReadString());
//                        break;

//                    default:
//                        break;
//                }
//            }

//            if (Console.KeyAvailable)
//            {
//                if (Console.ReadKey().Key == ConsoleKey.Enter)
//                {
//                    Console.Write("Message: ");
//                    var send = client.CreateMessage("[" + userName + "]: " + Console.ReadLine());
//                    send.Encrypt(encryptor);
//                    client.SendMessage(send, NetDeliveryMethod.ReliableOrdered);
//                }
//            }
//        }

//        private void UpdateServer()
//        {
//            while ((message = server.ReadMessage()) != null)
//            {
                

//                switch (message.MessageType)
//                {
//                    case NetIncomingMessageType.ConnectionApproval:
//                        message.SenderConnection.Approve();
//                        break;

//                    case NetIncomingMessageType.Data:
//                        try
//                        {
//                            message.Decrypt(encryptor);
//                            string text = message.ReadString();
//                            Console.WriteLine(text);
//                            var send = server.CreateMessage(text);
//                            send.Encrypt(encryptor);
//                            server.SendToAll(send, NetDeliveryMethod.ReliableOrdered);
//                        }
//                        catch
//                        {
//                            Console.WriteLine("Unreadable data received.");
//                        }
//                        break;

//                    case  NetIncomingMessageType.DiscoveryRequest:
//                        server.SendDiscoveryResponse(server.CreateMessage("Hi!"), message.SenderEndpoint);
//                        break;

//                    case NetIncomingMessageType.StatusChanged:
//                        NetConnectionStatus status = (NetConnectionStatus)message.ReadByte();
//                        switch (status)
//                        {
//                            case NetConnectionStatus.Connected:
//                                var send = server.CreateMessage("[Server]: Client connected.");
//                                send.Encrypt(encryptor);
//                                server.SendToAll(send, NetDeliveryMethod.ReliableOrdered);
//                                break;
//                            case NetConnectionStatus.Disconnected:
//                                send = server.CreateMessage("[Server]: Client disconnected.");
//                                send.Encrypt(encryptor);
//                                server.SendToAll(send, NetDeliveryMethod.ReliableOrdered);
//                                break;
//                        }
//                        break;

//                    default:
//                        break;
//                }
//            }
//        }

//        private void Initialize()
//        {
//            while (userName == "")
//            {
//                Console.Write("Username: ");
//                userName = Console.ReadLine();
//            }

//            config = new NetPeerConfiguration("TestChat");
//            config.MaximumConnections = 32;

//            encryptor = new NetXtea(cryptoKey);

//            ConsoleKey answer = ConsoleKey.Spacebar;
//            while (answer !=  ConsoleKey.C && answer !=  ConsoleKey.J)
//            {
//                Console.WriteLine("Create (c) server or join (j) existing?");
//                answer = Console.ReadKey().Key;
//            }
//            if (answer == ConsoleKey.C)
//                CreateNewServer();
//            else
//                JoinServer();

//            Console.WriteLine("Done.");
//        }

//        private void JoinServer()
//        {
//            client = new NetClient(config);
//            client.Start();
//            while (true)
//            {
//                Console.Write("Server's IP (type \"a\" for peer discovery: ");
//                string ip = Console.ReadLine();

//                if (ip == "")
//                    continue;
//                else if (ip.ToLower() == "a")
//                {
//                    client.DiscoverLocalPeers(port);
//                    var response = client.WaitMessage(1000);
//                    if(response!=null)
//                        client.Connect(response.SenderEndpoint, client.CreateMessage("Connected with " + userName));
//                }
//                else
//                {
//                    client.Connect(ip, port, client.CreateMessage("Connected with " + userName));
//                }
//                var info = client.WaitMessage(1000);
//                if (info != null)
//                    break;
//            }
//        }

//        private void CreateNewServer()
//        {
//            config.Port = port;
//            server = new NetServer(config);
//            server.Start();
//        }
//    }
//}
