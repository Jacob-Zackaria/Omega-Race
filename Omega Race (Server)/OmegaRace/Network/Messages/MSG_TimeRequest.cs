using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace OmegaRace
{
    // Message to req time.
    [Serializable]
    public class MSG_TimeRequest : BaseMessage
    {
        public float halfRoundTripTime;

        public MSG_TimeRequest()
        {

        }

        public MSG_TimeRequest(float newTime)
        {
            halfRoundTripTime = newTime;
        }

        public override void Serialize(ref BinaryWriter writer)
        {
            writer.Write(halfRoundTripTime);
        }

        public override void Deserialize(ref BinaryReader reader)
        {
            halfRoundTripTime = reader.ReadSingle();
        }

        public override void Execute()
        {
            float synchedTime = TimeManager.Instance().GameTime() + halfRoundTripTime;
            Debug.WriteLine("Synchronized Time: {0} Server Time: {1} RTT: {2}", synchedTime, synchedTime - halfRoundTripTime, halfRoundTripTime * 2.0f);

            // send a message to client to synchronize time.
            MSG_TimeSynchronization synchTime = new MSG_TimeSynchronization(synchedTime);
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
    }
}
