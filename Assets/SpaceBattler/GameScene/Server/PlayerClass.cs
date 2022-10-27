using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace SpaceBattler
{
    public abstract class Player
    {
        private Dictionary<int, OverallShipData> CreatableShips = new Dictionary<int, OverallShipData>();
        public Color color { private set; get; }
        public int PlayerID { private set; get; }
        public abstract bool IsAI { get; }

        public abstract void CheckCommands();
        [Server]
        protected void SubmitCommand(Command com)
        {
            ServerGameManager.instance.QueueCommand(com);
        }
        
        protected Player(int PlayerID, Color color)
        {
            this.PlayerID = PlayerID;
            this.color = color;
        }

        public GlobalPlayerInfo GetGlobalPlayerInfo()
        {
            return new GlobalPlayerInfo(PlayerID, IsAI, color);
        }

        public void RegisterShipDesign(int DesignID, OverallShipData ShipDat)
        {
            CreatableShips.Add(DesignID, ShipDat);
        }

        public OverallShipData GetShip(int DesignID)
        {
            return CreatableShips[DesignID];
        }
        public bool CheckDesignExists(int DesignID)
        {
            return CreatableShips.ContainsKey(DesignID);
        }
    }
}