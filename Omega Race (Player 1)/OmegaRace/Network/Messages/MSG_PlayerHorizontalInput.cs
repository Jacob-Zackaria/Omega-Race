using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OmegaRace
{
    // Message which updates player on server side.
    [Serializable]
    public class MSG_PlayerHorizontalInput : BaseMessage
    {
        public int playerID;
        public int horzInput;


        public override void Serialize(ref BinaryWriter writer)
        {
            writer.Write(playerID);
            writer.Write(horzInput);
        }

        public override void Deserialize(ref BinaryReader reader)
        {
            playerID = reader.ReadInt32();
            horzInput = reader.ReadInt32();
        }

        public override void Execute()
        {
            if (playerID == GameManager.Instance().player1.getID())
            {
                // if player 1, update its rotation and position.
                GameManager.Instance().player1.Rotate(horzInput);
            }
            else if (playerID == GameManager.Instance().player2.getID())
            {
                // if player 2, update its rotation and position.
                GameManager.Instance().player2.Rotate(horzInput);
            }
        }
    }
}
