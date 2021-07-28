using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OmegaRace
{
    // Message for initiating ship-missile collision.
    [Serializable]
    public class MSG_ShipMissileCollision : BaseMessage
    {
        public int playerID;
        public int missileID;

        public override void Serialize(ref BinaryWriter writer)
        {
            writer.Write(playerID);
            writer.Write(missileID);
        }

        public override void Deserialize(ref BinaryReader reader)
        {
            playerID = reader.ReadInt32();
            missileID = reader.ReadInt32();
        }

        public override void Execute()
        {
            // search active missile list.
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

            // get ship.
            Ship newShip = (Ship)GameManager.Instance().Find(playerID);

            // do ship collision.
            newShip.OnHit();
        }
    }
}
