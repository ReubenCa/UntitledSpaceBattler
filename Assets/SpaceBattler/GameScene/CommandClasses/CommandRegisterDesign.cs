using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceBattler
{


    public class CommandRegisterDesign : Command
    {
        readonly OverallShipData Ship;
        readonly int DesignID;
        readonly string ShipName;
        public CommandRegisterDesign(NetworkReader reader) : base(reader)
        {
            Ship = reader.Read<OverallShipData>();
            DesignID = reader.ReadInt();
            ShipName = reader.ReadString();
        }

        public CommandRegisterDesign(int PlayerID, int DesignID, string shipName, OverallShipData Ship) : base(PlayerID)
        {
           
            this.DesignID = DesignID;
            ShipName = shipName;
            this.Ship = Ship;
        }

        public override CommandTypes CommandType => CommandTypes.RegisterDesign;

        [Server]
        public override void Execute()
        {
            ServerGameManager.instance.playerManager.GetPlayer(PlayerID).RegisterShipDesign(DesignID,Ship);
        }

        public override bool Verify()
        {
            return Ship.Validate();
        }

        protected override void writeCommand(NetworkWriter writer)
        {
            writer.Write<OverallShipData>(Ship);
            writer.WriteInt(DesignID);
            writer.WriteString(ShipName);
        }

       

        public override void PostServerClientExecute(bool Success)
        {
            
           if(!Success)
            {
                Debug.Log("Ship Registration Failed on Server");
                return;
            }
            ClientGameManager.instance.AddShipButton(ShipName, DesignID);
        }
    }
}