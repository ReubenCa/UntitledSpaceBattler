using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace SpaceBattler
{
    public abstract class Entity : NetworkBehaviour
    {
        
        [Server]
        public void ServerInit()
        {
         
            ClientInit();
        }

        [ClientRpc]
        public virtual void ClientInit()
        {
           
        }


       
    } 
}
