using System;
using System.Diagnostics;
using Lidgren.Network;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;


namespace OmegaRace
{
    class NetworkGame : Azul.Game
    {
        
        //-----------------------------------------------------------------------------
        // Game::Initialize()
        //		Allows the engine to perform any initialization it needs to before 
        //      starting to run.  This is where it can query for any required services 
        //      and load any non-graphic related content. 
        //-----------------------------------------------------------------------------
        public override void Initialize()
        {
            // Game Window Device setup
            this.SetWindowName("Omega Race");
            this.SetWidthHeight(800, 500);
            this.SetClearColor(0.2f, 0.2f, 0.2f, 1.0f);


        }

        //-----------------------------------------------------------------------------
        // Game::LoadContent()
        //		Allows you to load all content needed for your engine,
        //	    such as objects, graphics, etc.
        //-----------------------------------------------------------------------------
        public override void LoadContent()
        {
            // set game mode.
            GameMode.Instance().Initialize(GameMode.TargetMode.NORMAL);
            //GameMode.Instance().Initialize(GameMode.TargetMode.RECORD, "TestRec.ice");
            //GameMode.Instance().Initialize(GameMode.TargetMode.PLAYBACK, "TestRec.ice");

            GameManager.Instance();

            PhysicWorld.Instance();
            ParticleSpawner.Instance();
            AudioManager.Instance();

            // client start
            MyClient.Instance();

            InputQueue.Instance();
            OutputQueue.Instance();

            // start timer manager.
            TimeManager.Instance().Create(GetTime());

            GameManager.Start();
        }

        //-----------------------------------------------------------------------------
        // Game::Update()
        //      Called once per frame, update data, tranformations, etc
        //      Use this function to control process order
        //      Input, AI, Physics, Animation, and Graphics
        //-----------------------------------------------------------------------------

       // static int number = 0;
        public override void Update()
        {
            // update timer.
            TimeManager.Instance().Update(GetTime());

            // print time
            Debug.WriteLine("Time:{0}", TimeManager.Instance().GameTime());

            // if game mode is playback mode.
            if (GameMode.Instance().Mode == GameMode.TargetMode.PLAYBACK)
            {
                //read data from file.
                GameMode.Instance().ReadFromFile();
            }
            else
            {
                // users input.
                InputManager.Update();
            }

            if ((GameMode.Instance().Mode == GameMode.TargetMode.PLAYBACK) && !GameMode.Instance().playbackEnded)
            {
                // update physics.
                PhysicWorld.Update(TimeManager.Instance().GameElapsedTime());
            }

            // update gameobjects
            GameManager.Update(TimeManager.Instance().GameElapsedTime());

            // process messages from output queue and add to input queue, if internal messages.
            OutputQueue.Process();

            // process input queue.
            InputQueue.Process();

            // clean mark for delete objects
            GameManager.CleanUp();            
        }

        //-----------------------------------------------------------------------------
        // Game::Draw()
        //		This function is called once per frame
        //	    Use this for draw graphics to the screen.
        //      Only do rendering here
        //-----------------------------------------------------------------------------
        public override void Draw()
        {
            GameManager.Draw();        
        }

        //-----------------------------------------------------------------------------
        // Game::UnLoadContent()
        //       unload content (resources loaded above)
        //       unload all content that was loaded before the Engine Loop started
        //-----------------------------------------------------------------------------
        public override void UnLoadContent()
        {
        }

    }
}

