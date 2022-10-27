using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace SpaceBattler
{
    public class PlayerObject : NetworkBehaviour
    {
        
        public int PlayerID { get; private set; }
        private JoinGameButton JGB;
        /// <summary>
        /// Prevents multiple join requests being processed
        /// </summary>
        bool ServerRegisteredJoinRequest = false;
        private void Start()
        {
            CommandQ = new Queue<Command>();
            if (isLocalPlayer)
            {
                ClientGameManager.instance.RegisterPlayerObj(this);
                JGB = FindObjectOfType<JoinGameButton>(true);
                JGB.Enable();
            }
        }

        [ClientRpc]
        public void OnSuccesfulGameJoin(int PlayerID)
        {
            if(!isLocalPlayer)
            {
                return;
            }
            Debug.Log("PlayerID Set to: " + PlayerID);
            this.PlayerID = PlayerID;
        }



        public void JoinGame()
        {
            CmdJoinGame();
        }

        [Command]
        void CmdJoinGame()
        {
            if(ServerRegisteredJoinRequest)
            {
                return;
            }
            ServerRegisteredJoinRequest = true;
            ServerGameManager.instance.playerManager.AddNewHumanPlayer(this);
        }

        public Queue<Command> CommandQ { private set; get; }
        [Command]
        void CmdReceiveCommand(Command command)
        {
            Debug.Log("Receiving Command from Player: " + command.PlayerID.ToString() + " CommandID: " + command.CommandID);
            CommandQ.Enqueue(command);
        }

        Dictionary<long, Command> CommandsNotRespondedToYet= new Dictionary<long, Command>();
        public void SendCommand(Command command)
        {
           
            CommandsNotRespondedToYet.Add(command.CommandID, command);
            //command.CommandID = ID;
            Debug.Log("Sending Command ID: " + command.CommandID);
            command.PreServerClientExecute();
            CmdReceiveCommand(command);
            
        }

        [ClientRpc]
        public void  CommandResponse(long ID, bool Success)
        {
            if(!isLocalPlayer)
            {
                return;
            }
            Debug.Log("Received Server Response from CommandID: " + ID + " Success: " + Success.ToString());
            if (!CommandsNotRespondedToYet.TryGetValue(ID, out Command command))
            {
                throw new System.Exception("Server responded to command that client does not have stored");
            }
            command.PostServerClientExecute(Success);
            CommandsNotRespondedToYet.Remove(ID);
        }
    }
}