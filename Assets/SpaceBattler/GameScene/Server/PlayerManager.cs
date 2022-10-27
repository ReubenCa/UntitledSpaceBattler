using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace SpaceBattler
{
    public class PlayerManager : NetworkBehaviour
    {
        
        List<GlobalPlayerInfo> SyncedPlayerInfo = new List<GlobalPlayerInfo>();
        /// <summary>
        /// PlayerID, Player Object
        /// </summary>
        Dictionary<int, Player> Players = new();

        [ClientRpc]
        void RpcSyncedPlayerInfoUpdate(List<GlobalPlayerInfo> SyncedInfo)
        {
            Debug.Log("Received Synced Player Update");
            SyncedPlayerInfo = SyncedInfo;
            ClientGameManager.instance.playerList.RefreshDisplay(SyncedPlayerInfo);
        }
        private int NextPlayerID = 0;
        [Server]
        public void AddAI()
        {
            AI NewAI = new AI(NextPlayerID, RandomColorGenerator.GetColor());
            NextPlayerID++;
            Players.Add(NewAI.PlayerID, NewAI);
            SyncedPlayerInfo.Add(NewAI.GetGlobalPlayerInfo());
            RpcSyncedPlayerInfoUpdate(SyncedPlayerInfo);
        }
        [Server]
        public void AddNewHumanPlayer(PlayerObject PlayerObj)
        {
            int ID = NextPlayerID;
            NextPlayerID++;
            Human NewHuman = new Human(PlayerObj, ID, RandomColorGenerator.GetColor());
            
            Players.Add(NewHuman.PlayerID, NewHuman);
            SyncedPlayerInfo.Add(NewHuman.GetGlobalPlayerInfo());
            RpcSyncedPlayerInfoUpdate(SyncedPlayerInfo);
            PlayerObj.OnSuccesfulGameJoin(ID);
        }
    
        public void ReceiveHumanInputs()
        {
            foreach(KeyValuePair<int, Player> KeyVal in Players)
            {
                Player p = KeyVal.Value;
                if(!p.IsAI)
                {
                    p.CheckCommands();
                }
            }
        }

        public void ComputeAICommands()
        {
            foreach (KeyValuePair<int, Player> KeyVal in Players)
            {
                Player p = KeyVal.Value;
                if (p.IsAI)
                {
                    p.CheckCommands();
                }
            }
        }

        public Player GetPlayer(int ID)
        {
            return Players[ID];
        }

        
    }

    public struct GlobalPlayerInfo
    {
        public readonly int PlayerID;
        public readonly bool AI;
        public readonly Color color;

        public GlobalPlayerInfo(int PlayerID, bool AI, Color color)
        {
            this.PlayerID = PlayerID;
            this.AI = AI;
            this.color = color;
        }

    }
}
