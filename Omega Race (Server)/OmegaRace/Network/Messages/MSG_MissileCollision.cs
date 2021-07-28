using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OmegaRace
{
    // Message for initiating missile collision.
    [Serializable]
    public class MSG_MissileCollision : BaseMessage
    {
        public int missileID;


        public override void Serialize(ref BinaryWriter writer)
        {
            writer.Write(missileID);
        }

        public override void Deserialize(ref BinaryReader reader)
        {
            missileID = reader.ReadInt32();
        }

        public override void Execute()
        {
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
