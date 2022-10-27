using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace SpaceBattler
{
    public abstract class Command
    {
        private static long nextCommandID = 0;
        public static long NextCommandID
        {
            get
            {
                nextCommandID++;
                return nextCommandID;

            }
        }
        public int PlayerID { private set; get; }

        public readonly long CommandID;

        public bool VerifyAndExecute()
        {
            if (Verify())
            {
                Execute();
                return true;
            }
            return false;
        }

        public virtual void PreServerClientExecute()
        {

        }

        public virtual void PostServerClientExecute(bool Success)
        {

        }


        public abstract void Execute();


        public abstract bool Verify();

        public abstract CommandTypes CommandType { get; }

        protected abstract void writeCommand(NetworkWriter writer);

        public void WriteCommand(NetworkWriter writer)
        {
            writer.WriteShort((short)CommandType);
            writer.WriteInt(PlayerID);
            writer.WriteLong(CommandID);
            writeCommand(writer);
        }

        //  protected abstract void readCommand(NetworkReader reader);
        protected Command(NetworkReader reader)
        {
            PlayerID = reader.ReadInt();
            CommandID = reader.ReadLong();
        }

        protected Command(int PlayerID)
        {
            CommandID = NextCommandID;
            this.PlayerID = PlayerID;
        }

        public static Command Read(NetworkReader reader)
        {
            CommandTypes Type = (CommandTypes)reader.ReadShort();


            switch (Type)
            {
                case CommandTypes.Move:
                    return new MoveCommand(reader);
                case CommandTypes.RegisterDesign:
                    return new CommandRegisterDesign(reader);
                case CommandTypes.CreateShip:
                    return new CreateShipCommand(reader);
                default:
                    throw new System.Exception("Command Type not recoginsed when Reading Command");
            }
        }

       
        protected Player  player{
            [Server]
            get
            {
                return ServerGameManager.instance.playerManager.GetPlayer(PlayerID);
            }
        }

    }

    public enum CommandTypes
    {
        Move,
        RegisterDesign,
        CreateShip
    }
}