using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace SpaceBattler
{
    public class AI : Player
    {
        public override bool IsAI {get{ return true; } }

        public AI(int PlayerID, Color color) : base(PlayerID, color)
        {
         
        }
        public override void CheckCommands()
        {
            
        }
    }
}