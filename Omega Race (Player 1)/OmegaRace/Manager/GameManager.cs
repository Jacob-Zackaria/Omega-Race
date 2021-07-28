using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Box2DX.Dynamics;
using Box2DX.Collision;
using Box2DX.Common;

namespace OmegaRace
{
    public enum GAME_STATE
    {
        PLAY
    }

    public class GameManager 
    {
        private static GameManager instance = null;
        public static GameManager Instance()
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }

        List<GameObject> destroyList;
        List<GameObject> gameObjList;

        public Ship player1;
        public Ship player2;

        public int p2Score;
        public int p1Score;

        GameManager_UI gamManUI;

        private GameManager()
        {
            destroyList = new List<GameObject>();
            gameObjList = new List<GameObject>();

            gamManUI = new GameManager_UI();
        }

        public static void Start()
        {
            LoadLevel_Helper.LoadLevel();
        }


        public static void Update(float gameTime)
        {
            GameManager inst = Instance();
            
            inst.pUpdate();
        }

        public static void Draw()
        {
            GameManager inst = Instance();
            
            inst.pDraw();
        }

        public GameObject Find(int id)
        {
            GameObject toReturn = null;

            foreach (GameObject obj in gameObjList)
            {
                if (obj.getID() == id)
                {
                    toReturn = obj;
                    break;
                }
            }

            return toReturn;
        }
        public static void RecieveMessage(MixedMessage msg)
        {
            msg.baseMsg.Execute();
        }


        private void pUpdate()
        {
            // -------------------------- PLAYER INPUT ------------------------------ //

            // get user input state.
            int player1Horz = InputManager.GetAxis(INPUTAXIS.HORIZONTAL_P1);
            int player1Vert = InputManager.GetAxis(INPUTAXIS.VERTICAL_P1);

            // only send horizontal input, if user pressed hoprizontal input keys.
            if(player1Horz != 0)
            {
                // get player 1 input.
                MSG_PlayerHorizontalInput msg1 = new MSG_PlayerHorizontalInput
                {
                    playerID = player1.getID(),
                    horzInput = player1Horz
                };
                // send player 1 input to server.
                MixedMessage mixMsg1 = new MixedMessage();
                mixMsg1.FillMessage(msg1);
                OutputMessageType outputMsg1 = new OutputMessageType
                {
                    msg = mixMsg1,
                    toNetwork = true
                };
                // add to output queue to process.
                OutputQueue.AddToQueue(outputMsg1);
            }

            // only send vertical input, if user pressed vertical input keys.
            if (player1Vert > 0)
            {
                // get player 1 input.
                MSG_PlayerVerticalInput msg1 = new MSG_PlayerVerticalInput
                {
                    playerID = player1.getID(),
                    vertInput = player1Vert
                };
                // send player 1 input to server.
                MixedMessage mixMsg1 = new MixedMessage();
                mixMsg1.FillMessage(msg1);
                OutputMessageType outputMsg1 = new OutputMessageType
                {
                    msg = mixMsg1,
                    toNetwork = true
                };
                // add to output queue to process.
                OutputQueue.AddToQueue(outputMsg1);
            }

            // ----------------------------------------------------------------------- //
            // --------------------------- FIRE MISSILE ------------------------------ //

            // check if player 1 fired and missiles are available.
            if (InputManager.GetButtonDown(INPUTBUTTON.P1_FIRE) && (player1.MissileCount() > 0))
            {
                // if yes, send fire command to server.
                MSG_FireTrigger fireMsg = new MSG_FireTrigger
                {
                    playerID = player1.getID()
                };
                MixedMessage mixMsg2 = new MixedMessage();
                mixMsg2.FillMessage(fireMsg);
                OutputMessageType outputMsg2 = new OutputMessageType
                {
                    msg = mixMsg2,
                    toNetwork = true
                };
                // add to output queue to process.
                OutputQueue.AddToQueue(outputMsg2);
            }

            // ----------------------------------------------------------------------- //
            // ---------------------- ADD TO ACTIVE MISSILE LIST --------------------- //

            // if a new missile is fired.
            if (ActiveMissileList.newMissileFired)
            {
                // search the game object list to find new missiles
                foreach (GameObject obj in gameObjList)
                {
                    // if object is a missile object and if missile list doesn't already contains the object.
                    if (obj.type == GAMEOBJECT_TYPE.MISSILE && !ActiveMissileList.activeMissiles.Contains((Missile)obj))
                    {
                        // add missile to active missile list.
                        ActiveMissileList.activeMissiles.Add((Missile)obj);
                    }
                }

                // set to false.
                ActiveMissileList.newMissileFired = false;
            }

            // ----------------------------------------------------------------------- //

            //**** General engine operations. No touchy!
            for (int i = gameObjList.Count - 1; i >= 0; i--)
            {
                gameObjList[i].Update();
            }
            gamManUI.Update(); // Note: Game UI (score display) is not processed as a game object
        }
        
        private void pDraw()
        {
            player1.Draw();
            player2.Draw();

            for (int i = 0; i < gameObjList.Count; i++)
            {
                gameObjList[i].Draw();
            }

            gamManUI.Draw();
        }
        


        public static void PlayerKilled(Ship s)
        {
            Instance().pPlayerKilled(s);
        }
        

        void pPlayerKilled(Ship shipKilled)
        {

            // Player 1 is Killed
            if(player1.getID() == shipKilled.getID())
            {
                p2Score++;

                player1.Respawn(new Vec2(400, 100));
                player2.Respawn(new Vec2(400, 400));
            }
            // Player 2 is Killed
            else if (player2.getID() == shipKilled.getID())
            {
                p1Score++;
                player1.Respawn(new Vec2(400, 100));
                player2.Respawn(new Vec2(400, 400));
                  
            }
        }

        public static void MissileDestroyed(Missile m)
        {
            GameManager inst = Instance();

            if (m.GetOwnerID() == inst.player1.getID())
            {
                inst.player1.GiveMissile();
            }
            else if (m.GetOwnerID() == inst.player2.getID())
            {
                inst.player2.GiveMissile();
            }
        }

        public static void FireMissile(Ship ship)
        {
            if (ship.UseMissile())
            {
                ship.Update();
                Vec2 pos = ship.GetWorldPosition();
                Vec2 direction = ship.GetHeading();
                Missile m = new Missile(new Azul.Rect(pos.X, pos.Y, 20, 5), ship.getID(), direction, ship.getColor());
                Instance().gameObjList.Add(m);
                AudioManager.PlaySoundEvent(AUDIO_EVENT.MISSILE_FIRE);

            }
        }

        

        public static void AddGameObject(GameObject obj)
        {
            Instance().gameObjList.Add(obj);
        }

        public static void CleanUp()
        {
            foreach (GameObject obj in Instance().destroyList)
            {
                Instance().gameObjList.Remove(obj);
                obj.Destroy();
            }

            Instance().destroyList.Clear();
        }
        
        public void DestroyAll()
        {
            foreach(GameObject obj in gameObjList)
            {
                destroyList.Add(obj);
            }
            gameObjList.Clear();
        }
            
        public static void DestroyObject(GameObject obj)
        {
            obj.setAlive(false);
            Instance().destroyList.Add(obj);
        }
        
        
    }
}
