using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Box2DX.Common;

namespace OmegaRace
{
    public struct MissileData
    {
        public int missileID;
        public float playerPosX;
        public float playerPosY;
        public float playerAngle;
    }

    // Message to update missile.
    [Serializable]
    public class MSG_MissileUpdate : BaseMessage
    {
        // list of all active missile data.
        public List<MissileData> missileData;

        public MSG_MissileUpdate()
        {
            missileData = new List<MissileData>();
        }

        public void AddMissileData(Missile newMissile)
        {
            MissileData mData;
            mData.missileID = newMissile.getID();
            mData.playerPosX = newMissile.GetWorldPosition().X;
            mData.playerPosY = newMissile.GetWorldPosition().Y;
            mData.playerAngle = newMissile.GetAngle_Deg();

            missileData.Add(mData);
        }

        public override void Serialize(ref BinaryWriter writer)
        {
            // write list count.
            writer.Write(missileData.Count);

            foreach (MissileData item in missileData)
            {
                writer.Write(item.missileID);
                writer.Write(item.playerPosX);
                writer.Write(item.playerPosY);
                writer.Write(item.playerAngle);
            }
        }

        public override void Deserialize(ref BinaryReader reader)
        {
            // get list count.
            missileData.Capacity = reader.ReadInt32();

            MissileData mData;
            int i = 0;
            while (i++ < missileData.Capacity)
            {
                mData.missileID = reader.ReadInt32();
                mData.playerPosX = reader.ReadSingle();
                mData.playerPosY = reader.ReadSingle();
                mData.playerAngle = reader.ReadSingle();

                missileData.Add(mData);
            }
        }

        public override void Execute()
        {
            // search active missile list
            foreach (MissileData item in missileData)
            {
                foreach (Missile missile in ActiveMissileList.activeMissiles)
                {
                    if (item.missileID == missile.getID())
                    {
                        missile.SetPosAndAngle(item.playerPosX, item.playerPosY, item.playerAngle);
                    }
                }
            }
        }

    }
}
