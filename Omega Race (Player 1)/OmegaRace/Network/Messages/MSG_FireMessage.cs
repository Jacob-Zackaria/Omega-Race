using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OmegaRace
{
    // Message which fires missile.
    [Serializable]
    public class MSG_FireMessage : BaseMessage
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
            if (playerID == GameManager.Instance().player1.getID())
            {
                // if player 1, fire its missile.
                GameManager.FireMissile(GameManager.Instance().player1);
            }
            else if (playerID == GameManager.Instance().player2.getID())
            {
                // if player 2, fire its missile.
                GameManager.FireMissile(GameManager.Instance().player2);
            }

            // set to get missile id later.
            ActiveMissileList.newMissileFired = true;
        }
    }
}
