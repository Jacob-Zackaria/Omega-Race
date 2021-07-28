using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OmegaRace
{
    // Message for initiating fence collision.
    [Serializable]
    public class MSG_FenceCollision : BaseMessage
    {
        public int fenceID;

        public override void Serialize(ref BinaryWriter writer)
        {
            writer.Write(fenceID);
        }

        public override void Deserialize(ref BinaryReader reader)
        {
            fenceID = reader.ReadInt32();
        }

        public override void Execute()
        {
            // find fence
            Fence f = (Fence)GameManager.Instance().Find(fenceID);

            // do fence collision.
            f.OnHit();
        }
    }
}
