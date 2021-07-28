using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace OmegaRace
{
    struct NetworkInfo
    {
        public string IPADDRESS;
        public int port;
    }

    class MyServer
    {
        private static MyServer instance;
        public static MyServer Instance()
        {
            if (instance == null)
            {
                instance = new MyServer();
            }
            return instance;
        }

        NetServer server;
        NetworkInfo networkInfo;

        private MyServer()
        {
            Setup();
        }

        void Setup()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("Connection Test");
            config.AutoFlushSendQueue = true;
            config.AcceptIncomingConnections = true;
            config.MaximumConnections = 100;
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.Port = 14240;

            //config.SimulatedLoss = 0.1f;
            //config.SimulatedRandomLatency = 0.05f;

            server = new NetServer(config);
            server.Start();

            networkInfo = new NetworkInfo
            {
                IPADDRESS = server.Configuration.BroadcastAddress.ToString(),
                port = 14240
            };

        }

        public void SendData(MixedMessage msg)
        {
            if (server.ConnectionsCount > 0)
            {
                // loop through all clients.
                foreach (NetConnection con in server.Connections)
                {
                    // create outgoing message.
                    NetOutgoingMessage om = server.CreateMessage();

                    // create memory stream.
                    MemoryStream stream = new MemoryStream();

                    // create binary writer.
                    BinaryWriter writer = new BinaryWriter(stream);

                    // serialize message.
                    msg.Serialize(ref writer);

                    // write the stream array.
                    om.Write(stream.ToArray());

                    // send data to client
                    server.SendMessage(om, con, msg.deliveryMethod, msg.sequenceChannel);
                }
            }
        }

        public void ReadInData()
        {
            NetIncomingMessage im;
            while ((im = server.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.Data:

                        // if data, read bytes from incoming message.
                        byte[] msg = im.ReadBytes(im.LengthBytes);

                        // create binary reader.
                        BinaryReader reader = new BinaryReader(new MemoryStream(msg));

                        // create mixed message instance.
                        MixedMessage dataMsg = new MixedMessage();

                        // deserialize data.
                        dataMsg.Deserialize(ref reader);

                        // output message type
                        OutputMessageType outputMsg = new OutputMessageType()
                        {
                            msg = dataMsg,
                            toNetwork = false
                        };

                        // add to output queue to process.
                        OutputQueue.AddToQueue(outputMsg);

                        break;

                    //**********************************
                    // A client is enquiring about a possible connection
                    case NetIncomingMessageType.DiscoveryRequest:
                        Debug.WriteLine("Answering Discovery Request from " + im.SenderEndPoint);
                        NetOutgoingMessage om = server.CreateMessage();
                        om.Write("Welcome to this cool server");
                        server.SendDiscoveryResponse(om, im.SenderEndPoint);
                        break;

                    // A client's connection status changed
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                        Debug.WriteLine("Connection status changed: " + status.ToString() + ": " + im.ReadString());
                        break;
                    // These are other Lidgren status messages that we likely shouldn't have to deal with
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                    case NetIncomingMessageType.UnconnectedData:
                        Debug.WriteLine("Status Message:" + im.MessageType + " from [" + im.SenderEndPoint + "]: " + im.ReadString());
                        break;

                }
               server.Recycle(im);
            }
        }
    }
}

