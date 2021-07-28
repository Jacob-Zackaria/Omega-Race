using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Box2DX.Common;

namespace OmegaRace
{
    struct PlayerData
    {
        public float playerPosX;
        public float playerPosY;
        public float playerAngle;
    }

    // Message to update player state.
    [Serializable]
    public class MSG_PlayerUpdate : BaseMessage
    {
        PlayerData player1Data;
        PlayerData player2Data;

        public MSG_PlayerUpdate()
        {

        }

        public MSG_PlayerUpdate(GameObject newPlayer1, GameObject newPlayer2)
        {
            player1Data.playerPosX = newPlayer1.GetWorldPosition().X;
            player1Data.playerPosY = newPlayer1.GetWorldPosition().Y;
            player1Data.playerAngle = newPlayer1.GetAngle_Deg();

            player2Data.playerPosX = newPlayer2.GetWorldPosition().X;
            player2Data.playerPosY = newPlayer2.GetWorldPosition().Y;
            player2Data.playerAngle = newPlayer2.GetAngle_Deg();
        }

        public override void Serialize(ref BinaryWriter writer)
        {
            writer.Write(player1Data.playerPosX);
            writer.Write(player1Data.playerPosY);
            writer.Write(player1Data.playerAngle);

            writer.Write(player2Data.playerPosX);
            writer.Write(player2Data.playerPosY);
            writer.Write(player2Data.playerAngle);
        }

        public override void Deserialize(ref BinaryReader reader)
        {
            player1Data.playerPosX = reader.ReadSingle();
            player1Data.playerPosY = reader.ReadSingle();
            player1Data.playerAngle = reader.ReadSingle();

            player2Data.playerPosX = reader.ReadSingle();
            player2Data.playerPosY = reader.ReadSingle();
            player2Data.playerAngle = reader.ReadSingle();
        }

        public override void Execute()
        {
            //player 1, update its rotation and position.
            GameManager.Instance().player1.SetPosAndAngle(player1Data.playerPosX, player1Data.playerPosY, player1Data.playerAngle);


            // player 2, update its rotation and position.
            GameManager.Instance().player2.SetPosAndAngle(player2Data.playerPosX, player2Data.playerPosY, player2Data.playerAngle);
        }
    }
}
