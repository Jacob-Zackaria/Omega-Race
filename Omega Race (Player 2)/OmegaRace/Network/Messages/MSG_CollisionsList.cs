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
    public class MSG_CollisionsList : BaseMessage
    {
        // list of all collision messages.
        public List<MixedMessage> colList;

        public MSG_CollisionsList()
        {

        }

        public MSG_CollisionsList(List<MixedMessage> newList)
        {
            // copy collision list
            colList = new List<MixedMessage>(newList);
        }

        public override void Serialize(ref BinaryWriter writer)
        {
            // write list count.
            writer.Write(colList.Count);

            foreach (MixedMessage item in colList)
            {
                if (item.msgType == MessageType.MSG_FENCE_COLLISION)
                {
                    writer.Write((int)MessageType.MSG_FENCE_COLLISION);

                    // down cast
                    MSG_FenceCollision fenceCollision = (MSG_FenceCollision)item.baseMsg;
                    writer.Write(fenceCollision.fenceID);
                }
                else if (item.msgType == MessageType.MSG_FENCE_MISSILE_COLLISION)
                {
                    writer.Write((int)MessageType.MSG_FENCE_MISSILE_COLLISION);

                    // down cast
                    MSG_FenceMissileCollision fenceMissileCollision = (MSG_FenceMissileCollision)item.baseMsg;
                    writer.Write(fenceMissileCollision.missileID);
                    writer.Write(fenceMissileCollision.fenceID);
                }
                else if (item.msgType == MessageType.MSG_MISSILE_COLLISION)
                {
                    writer.Write((int)MessageType.MSG_MISSILE_COLLISION);

                    // down cast
                    MSG_MissileCollision missileCollision = (MSG_MissileCollision)item.baseMsg;
                    writer.Write(missileCollision.missileID);
                }
                else if (item.msgType == MessageType.MSG_SHIP_MISSILE_COLLISION)
                {
                    writer.Write((int)MessageType.MSG_SHIP_MISSILE_COLLISION);

                    // down cast
                    MSG_ShipMissileCollision shipMissileCollision = (MSG_ShipMissileCollision)item.baseMsg;
                    writer.Write(shipMissileCollision.playerID);
                    writer.Write(shipMissileCollision.missileID);
                }
            }
        }

        public override void Deserialize(ref BinaryReader reader)
        {
            // set list capacity
            colList = new List<MixedMessage>(reader.ReadInt32());

            int i = 0; 
            while (i++ < colList.Capacity)
            {
                MessageType newType = (MessageType)reader.ReadInt32();
                switch (newType)
                {
                    case MessageType.MSG_FENCE_COLLISION:
                        MSG_FenceCollision fenceCollision = new MSG_FenceCollision();
                        fenceCollision.fenceID = reader.ReadInt32();
                        MixedMessage fenceMix = new MixedMessage();
                        fenceMix.FillMessage(fenceCollision);
                        colList.Add(fenceMix);
                        break;
                    case MessageType.MSG_FENCE_MISSILE_COLLISION:
                        MSG_FenceMissileCollision fenceMissileCollision = new MSG_FenceMissileCollision();
                        fenceMissileCollision.missileID = reader.ReadInt32();
                        fenceMissileCollision.fenceID = reader.ReadInt32();
                        MixedMessage fenceMissileMix = new MixedMessage();
                        fenceMissileMix.FillMessage(fenceMissileCollision);
                        colList.Add(fenceMissileMix);
                        break;
                    case MessageType.MSG_MISSILE_COLLISION:
                        MSG_MissileCollision missileCollision = new MSG_MissileCollision();
                        missileCollision.missileID = reader.ReadInt32();
                        MixedMessage missileMix = new MixedMessage();
                        missileMix.FillMessage(missileCollision);
                        colList.Add(missileMix);
                        break;
                    case MessageType.MSG_SHIP_MISSILE_COLLISION:
                        MSG_ShipMissileCollision shipMissileCollision = new MSG_ShipMissileCollision();
                        shipMissileCollision.playerID = reader.ReadInt32();
                        shipMissileCollision.missileID = reader.ReadInt32();
                        MixedMessage shipMissileMix = new MixedMessage();
                        shipMissileMix.FillMessage(shipMissileCollision);
                        colList.Add(shipMissileMix);
                        break;
                    default:
                        break;
                }
            }
        }

        public override void Execute()
        {
            foreach (MixedMessage item in colList)
            {
                item.baseMsg.Execute();
            }
        }
    }
}
