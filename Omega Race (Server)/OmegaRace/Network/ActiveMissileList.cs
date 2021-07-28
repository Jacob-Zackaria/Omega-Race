using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaRace
{
    public struct ActiveMissileList
    {
        // to check if new missile is fired
        public static bool newMissileFired;

        // list of active missiles.
        public static List<Missile> activeMissiles = new List<Missile>();
    }
}
