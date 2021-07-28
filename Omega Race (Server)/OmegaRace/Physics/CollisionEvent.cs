using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaRace
{

    class CollisionEvent
    {
        // static list of collision messages
        public static List<MixedMessage> collisionList = new List<MixedMessage>();

        public static void Action(Fence f, Missile m)
        {
            // create message
            MSG_FenceMissileCollision newMsg = new MSG_FenceMissileCollision
            {
                missileID = m.getID(),
                fenceID = f.getID()
            };
            MixedMessage mixMsg = new MixedMessage();
            mixMsg.FillMessage(newMsg);

            // add to collision list
            collisionList.Add(mixMsg);
        }

        public static void Action(FencePost f, Missile m)
        {
            // create message
            MSG_MissileCollision newMsg = new MSG_MissileCollision
            {
                missileID = m.getID()
            };
            MixedMessage mixMsg = new MixedMessage();
            mixMsg.FillMessage(newMsg);

            // add to collision list
            collisionList.Add(mixMsg);
        }

        public static void Action(Ship s, Missile m)
        {
            // create message
            MSG_ShipMissileCollision newMsg = new MSG_ShipMissileCollision
            {
                playerID = s.getID(),
                missileID = m.getID()
            };
            MixedMessage mixMsg = new MixedMessage();
            mixMsg.FillMessage(newMsg);

            // add to collision list
            collisionList.Add(mixMsg);
        }

        public static void Action(Ship s, Fence f)
        {
            // create message
            MSG_FenceCollision newMsg = new MSG_FenceCollision
            {
                fenceID = f.getID()
            };
            MixedMessage mixMsg = new MixedMessage();
            mixMsg.FillMessage(newMsg);

            // add to collision list
            collisionList.Add(mixMsg);
        }
    }
}
