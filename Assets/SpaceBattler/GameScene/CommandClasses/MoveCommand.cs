using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceBattler
{
    public class MoveCommand : GroupOrder
    {
        public override CommandTypes CommandType => CommandTypes.Move;

        protected override OrderTypes OrderType => OrderTypes.Move;

        public MoveCommand(NetworkReader reader) : base(reader)
        {

        }

        public override void Execute()
        {
            throw new System.NotImplementedException();
        }

        public override bool Verify()
        {
            throw new System.NotImplementedException();
        }

        protected override void writeCommand(NetworkWriter writer)
        {
            throw new System.NotImplementedException();
        }

        protected override void writeCommand2(NetworkWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}