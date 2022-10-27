using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace SpaceBattler
{
    public class Human : Player
    {
        public PlayerObject playerObj { private set;  get; }
        public override bool IsAI { get { return false; } }

        public Human(PlayerObject playerObject, int PlayerID, Color color) : base(PlayerID, color)
        {
            this.playerObj = playerObject;
        }

        public override void CheckCommands()
        {
            while (playerObj.CommandQ.Count>0)
            {
                SubmitCommand(playerObj.CommandQ.Dequeue());
            }
        }
    }
}