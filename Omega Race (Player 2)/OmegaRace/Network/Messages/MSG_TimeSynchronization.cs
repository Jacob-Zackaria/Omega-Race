using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
namespace OmegaRace
{
    // Message for time synch.
    [Serializable]
    public class MSG_TimeSynchronization : BaseMessage
    {
        public float serverTime;

        public MSG_TimeSynchronization()
        {

        }

        public MSG_TimeSynchronization(float newTime)
        {
            serverTime = newTime;
        }

        public override void Serialize(ref BinaryWriter writer)
        {
            writer.Write(serverTime);
        }

        public override void Deserialize(ref BinaryReader reader)
        {
            serverTime = reader.ReadSingle();
        }

        public override void Execute()
        {
            // if time was synced, dont sync again
            if (!TimeManager.Instance().timeSynced)
            {
                TimeManager.Instance().SetServerDelta(serverTime);
                Debug.WriteLine("<<Synchronized Time>>");
            }
        }
    }
}
