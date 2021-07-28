using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Lidgren.Network;

namespace OmegaRace
{
    // Types of messages.
    public enum MessageType
    {
        MSG_COLLISION_LIST,
        MSG_FENCE_COLLISION,
        MSG_FENCE_MISSILE_COLLISION,
        MSG_FIRE_TRIGGER,
        MSG_FIRE_MESSAGE,
        MSG_MISSILE_COLLISION,
        MSG_MISSILE_UPDATE,
        MSG_PLAYER_HORIZONTAL_INPUT,
        MSG_PLAYER_VERTICAL_INPUT,
        MSG_PLAYER_UPDATE,
        MSG_SHIP_MISSILE_COLLISION,
        MSG_TIME_SYNCHRONIZATION,
        MSG_TIME_REQUEST
    }

    // Mixed messages.
    [Serializable]
    public class MixedMessage
    {
        public NetDeliveryMethod deliveryMethod;
        public int sequenceChannel;
        public MessageType msgType;
        public BaseMessage baseMsg;

        public void FillMessage(MSG_CollisionsList newMsg)
        {
            deliveryMethod = NetDeliveryMethod.ReliableOrdered;
            sequenceChannel = 4;
            msgType = MessageType.MSG_COLLISION_LIST;
            baseMsg = newMsg;
        }

        public void FillMessage(MSG_TimeSynchronization newMsg)
        {
            deliveryMethod = NetDeliveryMethod.ReliableOrdered;
            sequenceChannel = 4;
            msgType = MessageType.MSG_TIME_SYNCHRONIZATION;
            baseMsg = newMsg;
        }

        public void FillMessage(MSG_TimeRequest newMsg)
        {
            deliveryMethod = NetDeliveryMethod.ReliableOrdered;
            sequenceChannel = 4;
            msgType = MessageType.MSG_TIME_REQUEST;
            baseMsg = newMsg;
        }

        public void FillMessage(MSG_FenceCollision newMsg)
        {
            deliveryMethod = NetDeliveryMethod.ReliableOrdered;
            sequenceChannel = 4;
            msgType = MessageType.MSG_FENCE_COLLISION;
            baseMsg = newMsg;
        }

        public void FillMessage(MSG_FenceMissileCollision newMsg)
        {
            deliveryMethod = NetDeliveryMethod.ReliableOrdered;
            sequenceChannel = 4;
            msgType = MessageType.MSG_FENCE_MISSILE_COLLISION;
            baseMsg = newMsg;
        }

        public void FillMessage(MSG_FireTrigger newMsg)
        {
            deliveryMethod = NetDeliveryMethod.ReliableUnordered;
            sequenceChannel = 0;
            msgType = MessageType.MSG_FIRE_TRIGGER;
            baseMsg = newMsg;
        }

        public void FillMessage(MSG_FireMessage newMsg)
        {
            deliveryMethod = NetDeliveryMethod.ReliableUnordered;
            sequenceChannel = 0;
            msgType = MessageType.MSG_FIRE_MESSAGE;
            baseMsg = newMsg;
        }

        public void FillMessage(MSG_MissileCollision newMsg)
        {
            deliveryMethod = NetDeliveryMethod.ReliableOrdered;
            sequenceChannel = 4;
            msgType = MessageType.MSG_MISSILE_COLLISION;
            baseMsg = newMsg;
        }

        public void FillMessage(MSG_MissileUpdate newMsg)
        {
            deliveryMethod = NetDeliveryMethod.UnreliableSequenced;
            sequenceChannel = 2;
            msgType = MessageType.MSG_MISSILE_UPDATE;
            baseMsg = newMsg;
        }

        public void FillMessage(MSG_PlayerHorizontalInput newMsg)
        {
            deliveryMethod = NetDeliveryMethod.ReliableOrdered;
            sequenceChannel = 2;
            msgType = MessageType.MSG_PLAYER_HORIZONTAL_INPUT;
            baseMsg = newMsg;
        }

        public void FillMessage(MSG_PlayerVerticalInput newMsg)
        {
            deliveryMethod = NetDeliveryMethod.UnreliableSequenced;
            sequenceChannel = 2;
            msgType = MessageType.MSG_PLAYER_VERTICAL_INPUT;
            baseMsg = newMsg;
        }

        public void FillMessage(MSG_PlayerUpdate newMsg)
        {
            deliveryMethod = NetDeliveryMethod.UnreliableSequenced;
            sequenceChannel = 2;
            msgType = MessageType.MSG_PLAYER_UPDATE;
            baseMsg = newMsg;
        }

        public void FillMessage(MSG_ShipMissileCollision newMsg)
        {
            deliveryMethod = NetDeliveryMethod.ReliableOrdered;
            sequenceChannel = 4;
            msgType = MessageType.MSG_SHIP_MISSILE_COLLISION;
            baseMsg = newMsg;
        }

        public void Serialize(ref BinaryWriter writer)
        {
            writer.Write((int)msgType);
            baseMsg.Serialize(ref writer);
        }

        public void Deserialize(ref BinaryReader reader)
        {
            msgType = (MessageType)reader.ReadInt32();

            switch (msgType)
            {
                case MessageType.MSG_COLLISION_LIST:
                    baseMsg = new MSG_CollisionsList();
                    break;
                case MessageType.MSG_FENCE_COLLISION:
                    baseMsg = new MSG_FenceCollision();
                    break;
                case MessageType.MSG_FENCE_MISSILE_COLLISION:
                    baseMsg = new MSG_FenceMissileCollision();
                    break;
                case MessageType.MSG_FIRE_TRIGGER:
                    baseMsg = new MSG_FireTrigger();
                    break;
                case MessageType.MSG_FIRE_MESSAGE:
                    baseMsg = new MSG_FireMessage();
                    break;
                case MessageType.MSG_MISSILE_COLLISION:
                    baseMsg = new MSG_MissileCollision();
                    break;
                case MessageType.MSG_MISSILE_UPDATE:
                    baseMsg = new MSG_MissileUpdate();
                    break;
                case MessageType.MSG_PLAYER_HORIZONTAL_INPUT:
                    baseMsg = new MSG_PlayerHorizontalInput();
                    break;
                case MessageType.MSG_PLAYER_VERTICAL_INPUT:
                    baseMsg = new MSG_PlayerVerticalInput();
                    break;
                case MessageType.MSG_PLAYER_UPDATE:
                    baseMsg = new MSG_PlayerUpdate();
                    break;
                case MessageType.MSG_SHIP_MISSILE_COLLISION:
                    baseMsg = new MSG_ShipMissileCollision();
                    break;
                case MessageType.MSG_TIME_SYNCHRONIZATION:
                    baseMsg = new MSG_TimeSynchronization();
                    break;
                case MessageType.MSG_TIME_REQUEST:
                    baseMsg = new MSG_TimeRequest();
                    break;
            }

            baseMsg.Deserialize(ref reader);
        }

    }
}
