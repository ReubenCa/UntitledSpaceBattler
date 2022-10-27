using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace SpaceBattler
{
    public class Ship : Entity
    {
        

        [Server]
        public void ServerInit(OverallShipData Dat)
        {
            base.ServerInit();
            
        }

        [ClientRpc]
        public override void ClientInit()
        {
            base.ClientInit();
            ClientGameManager.instance.InputMan.AddShip(this);
        }



        // Update is called once per frame
        void Update()
        {

        }
    }
}