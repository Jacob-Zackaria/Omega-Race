using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OmegaRace
{
    // Message for initiating fence-missile collision.
    [Serializable]
    public class MSG_FenceMissileCollision : BaseMessage
    {
        public int missileID;
        public int fenceID;

        public override void Serialize(ref BinaryWriter writer)
        {
            writer.Write(missileID);
            writer.Write(fenceID);
        }

        public override void Deserialize(ref BinaryReader reader)
        {
            missileID = reader.ReadInt32();
            fenceID = reader.ReadInt32();
        }

        public override void Execute()
        {
            // get fence.
            Fence newFence = (Fence)GameManager.Instance().Find(fenceID);

            // do fence collision.
            newFence.OnHit();

            // search active missile list
            foreach (Missile missile in ActiveMissileList.activeMissiles)
            {
                if (missileID == missile.getID())
                {
                    // call missile on hit
                    missile.OnHit();

                    // remove missile from active list
                    ActiveMissileList.activeMissiles.Remove(missile);

                    break;
                }
            }

        }
    }
}
