using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Box2DX.Common;

namespace OmegaRace
{
    // Message which initiate fire missile event.
    [Serializable]
    public class MSG_FireTrigger : BaseMessage
    {
        public int playerID;

        public override void Serialize(ref BinaryWriter writer)
        {
            writer.Write(playerID);
        }

        public override void Deserialize(ref BinaryReader reader)
        {
            playerID = reader.ReadInt32();
        }

        public override void Execute()
        {
            MSG_FireMessage newFireMsg = new MSG_FireMessage
            {
                playerID = this.playerID
            };
            MixedMessage mixMsg = new MixedMessage();
            mixMsg.FillMessage(newFireMsg);

            // To client
            {
                OutputMessageType outputMsg2 = new OutputMessageType
                {
                    msg = mixMsg,
                    toNetwork = true
                };
                // add to output queue to process.
                OutputQueue.AddToQueue(outputMsg2);
            }

            // Internal message
            {
                OutputMessageType outputMsg2 = new OutputMessageType
                {
                    msg = mixMsg,
                    toNetwork = false
                };
                // add to output queue to process.
                OutputQueue.AddToQueue(outputMsg2);
            }
        }
    }
}
