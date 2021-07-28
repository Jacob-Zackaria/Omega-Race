using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaRace
{
    public class TimeManager
    {
        private static TimeManager instance;
        private float currTime;
        private float prevTime;
        private float gameElapsedTime;
        private float serverDelta;
        public bool timeSynced;

        public static TimeManager Instance()
        {
            if (instance == null)
            {
                instance = new TimeManager();
            }
            return instance;
        }

        public void Create(float gameTime)
        {
            prevTime = gameTime;
            currTime = gameTime;
        }

        public void Update(float gameTime)
        {
            currTime = serverDelta + gameTime;
            gameElapsedTime = (currTime - prevTime);
            prevTime = currTime;
        }

        public float GameElapsedTime()
        {
            return gameElapsedTime;
        }

        public float GameTime()
        {
            return currTime;
        }

        public void SetServerDelta(float newTime)
        {
            serverDelta = (newTime - currTime);
            timeSynced = true;
        }
    }
}
