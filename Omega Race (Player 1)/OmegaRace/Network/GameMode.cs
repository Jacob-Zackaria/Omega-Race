using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
namespace OmegaRace
{
    public class GameMode
    {
        private static GameMode instance;
        public static GameMode Instance()
        {
            if (instance == null)
            {
                instance = new GameMode();
            }
            return instance;
        }

        public void Initialize(TargetMode newMode, string file = "")
        {
            // set target mode.
            Mode = newMode;

            if(Mode == TargetMode.RECORD)
            {
                writer = new BinaryWriter(new FileStream("../bin/Debug/" + file, FileMode.Create, FileAccess.Write));
            }
            else if(Mode == TargetMode.PLAYBACK)
            {
                try
                {
                    reader = new BinaryReader(new FileStream("../bin/Debug/" + file, FileMode.Open, FileAccess.Read));
                }
                catch(Exception e)
                {
                    // if file doesnt exist.
                    Debug.WriteLine("{0}", e.ToString());
                }
               
            }

            // set sequence number to zero.
            sequenceNum = 0;
        }

        public void WriteToFile(OutputMessageType newMsg)
        {
            // write sequence number.
            writer.Write(sequenceNum++);

            // write game time.
            writer.Write(TimeManager.Instance().GameTime());

            // wite message type.
            writer.Write(newMsg.toNetwork);

            // serialize message to file stream.
            newMsg.msg.Serialize(ref writer);
        }

        public void ReadFromFile()
        {
            while (!playbackEnded)
            {
                if(getNext)
                {
                    if (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        // get sequence number
                        Debug.WriteLine("\nSeq:#{0}", reader.ReadInt32());

                        // get game time
                        msgTime = reader.ReadSingle();
                        Debug.WriteLine("Time:{0}", msgTime);

                        // create mixed message instance.
                        outMsg.msg = new MixedMessage();

                        // get message type.
                        outMsg.toNetwork = reader.ReadBoolean();
                        if (outMsg.toNetwork)
                        {
                            Debug.WriteLine("Client >> Server:");
                        }
                        else
                        {
                            Debug.WriteLine("Server >> Client:");
                        }

                        // deserialize data.
                        outMsg.msg.Deserialize(ref reader);

                        // get message type
                        Debug.WriteLine("{0}", outMsg.msg.msgType);

                        getNext = false;
                    }
                    else
                    {
                        playbackEnded = true;
                        break;
                    }
                }
                
                // if current game time is greater than message time.
                if ((TimeManager.Instance().GameTime() >= msgTime) && (getNext == false))
                {
                    // if it is a message to server execute, else if it is a message from server discard.
                    if (outMsg.toNetwork)
                    {
                        // output message type
                        OutputMessageType outputMsg = new OutputMessageType()
                        {
                            msg = outMsg.msg,
                            toNetwork = false
                        };

                        // add to output queue to process.
                        OutputQueue.AddToQueue(outputMsg);
                    }

                    // get next data.
                    getNext = true;
                }
                else
                {
                    break;
                }
            }
        }


        public enum TargetMode
        {
            NORMAL,
            RECORD,
            PLAYBACK
        }

        public TargetMode Mode { get; set; }
        private BinaryWriter writer;
        private BinaryReader reader;

        // Data
        private int sequenceNum;
        private bool getNext = true;
        private float msgTime = 0.0f;
        private OutputMessageType outMsg = new OutputMessageType();
        public bool playbackEnded = false;
    }
}
