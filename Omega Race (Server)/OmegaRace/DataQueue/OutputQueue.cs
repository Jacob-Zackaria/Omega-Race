using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaRace
{
    // structure to store message and boolean representing if the message goes to server.
    public struct OutputMessageType
    {
        public MixedMessage msg;
        public bool toNetwork;
    }

    class OutputQueue
    {
        private static OutputQueue instance = null;
        public static OutputQueue Instance()
        {
            if (instance == null)
            {
                instance = new OutputQueue();
            }
            return instance;
        }

        Queue<OutputMessageType> pOutputQueue;

        private OutputQueue()
        {
            pOutputQueue = new Queue<OutputMessageType>();
        }

        public static void AddToQueue(OutputMessageType msg)
        {
            instance.pOutputQueue.Enqueue(msg);
        }

        public static void Process()
        {
            // recieve from network and add to output queue.
            MyServer.Instance().ReadInData();

            while (instance.pOutputQueue.Count > 0)
            {
                // get output message type
                OutputMessageType outputMsg = instance.pOutputQueue.Dequeue();

                // if game mode is in record mode, write all messages to file.
                if (GameMode.Instance().Mode == GameMode.TargetMode.RECORD)
                {
                    // write data to file.
                    GameMode.Instance().WriteToFile(outputMsg);
                }

                // if it is to network.
                if (outputMsg.toNetwork)
                {
                    // no data is send to client on playback mode.
                    if (GameMode.Instance().Mode != GameMode.TargetMode.PLAYBACK)
                    {
                        // send to client.
                        MyServer.Instance().SendData(outputMsg.msg);
                    }
                }
                else
                {
                    // add to input queue.
                    InputQueue.AddToQueue(outputMsg.msg);
                }

            }
        }


    }
}
