using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OmegaRace
{
    // base message class for all other message types.
    [Serializable]
    public abstract class BaseMessage
    {
        // abstract serialize.
        public abstract void Serialize(ref BinaryWriter writer);

        // abstract deserialize.
        public abstract void Deserialize(ref BinaryReader reader);

        // abstract execute.
        public abstract void Execute();
    }
}
