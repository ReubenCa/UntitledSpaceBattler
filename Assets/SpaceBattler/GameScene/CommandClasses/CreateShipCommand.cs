using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceBattler
{
    public class CreateShipCommand : Command
    {
        readonly Vector2 Position;
        readonly int DesignID;
        public CreateShipCommand(NetworkReader reader) : base(reader)
        {

            DesignID = reader.ReadInt();
            Position = reader.ReadVector2();
        }

        public CreateShipCommand(int PlayerID, Vector2 Position, int DesignID) : base(PlayerID)
        {
            this.DesignID = DesignID;
            this.Position = Position;
        }

        public override CommandTypes CommandType => CommandTypes.CreateShip;

        [Server]
        public override void Execute()
        {
            OverallShipData Ship = player.GetShip(DesignID);
            ServerGameManager.instance.shipManager.SpawnShip(Ship, DesignID);
        }

        public override bool Verify()
        {
            return player.CheckDesignExists(DesignID);
        }

        protected override void writeCommand(NetworkWriter writer)
        {
            writer.Write(DesignID);
            writer.Write(Position);
        }
    }
}