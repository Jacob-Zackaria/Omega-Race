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

    class MyClient
    {
        private static MyClient instance;
        public static MyClient Instance()
        {
            if (instance == null)
            {
                instance = new MyClient();
            }
            return instance;
        }

        NetClient client;
        NetworkInfo myClientInfo;

        NetworkInfo connectedServerInfo;

        public bool isConnected
        {
            get; set;
        }

        private MyClient()
        {
            Setup();
        }

        public void Setup()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("Connection Test");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            client = new NetClient(config);
            client.Start();

            myClientInfo = new NetworkInfo
            {
                IPADDRESS = client.Configuration.BroadcastAddress.ToString(),
                port = 14240
            };

            client.DiscoverLocalPeers(myClientInfo.port);
            //client.Connect("localhost", 14240);
            isConnected = false;
        }


        public void SendData(MixedMessage msg)
        {
            // create outgoing message.
            NetOutgoingMessage om = client.CreateMessage();

            // create memory stream.
            MemoryStream stream = new MemoryStream();

            // create binary writer.
            BinaryWriter writer = new BinaryWriter(stream);

            // serialize message.
            msg.Serialize(ref writer);

            // write the stream array.
            om.Write(stream.ToArray());

            // send data to server.
            client.SendMessage(om, msg.deliveryMethod, msg.sequenceChannel);
        }

        public void ReadInData()
        {
            NetIncomingMessage im;
            while ((im = client.ReadMessage()) != null)
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
                    // A server replied to out discovery request
                    case NetIncomingMessageType.DiscoveryResponse:
                        Debug.WriteLine("Found server at " + im.SenderEndPoint + " name: " + im.ReadString());
                        client.Connect(im.SenderEndPoint);
                        break;

                    // A client's connection status changed
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                        Debug.WriteLine("Connection status changed: " + status.ToString() + ": " + im.ReadString());
                        if(status == NetConnectionStatus.Connected)
                        {
                            isConnected = true;

                            connectedServerInfo = new NetworkInfo()
                            {
                                IPADDRESS = im.SenderEndPoint.Address.ToString(),
                                port = im.SenderEndPoint.Port
                            };

                            // send a message to server to synchronize time.
                            MSG_TimeRequest synchTime = new MSG_TimeRequest((im.SenderConnection.AverageRoundtripTime/ 2.0f));
                            MixedMessage newMix = new MixedMessage();
                            newMix.FillMessage(synchTime);
                            OutputMessageType outputMsg1 = new OutputMessageType
                            {
                                msg = newMix,
                                toNetwork = true
                            };
                            // add to output queue to process.
                            OutputQueue.AddToQueue(outputMsg1);
                        }
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
               client.Recycle(im);
            }
        }
    }
}
