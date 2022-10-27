using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBattler
{
    /// <summary>
    /// This class is for when a network command can be converted to an individual Order for a list of ships
    /// </summary>
    public abstract class GroupOrder : Command
    {
        protected abstract OrderTypes OrderType { get; }
        HashSet<int> Ships;

        protected GroupOrder(NetworkReader reader) : base(reader)
        {
            int ShipCount = reader.ReadInt();
            for(int i =0; i<ShipCount;i++)
            {
                Ships.Add(reader.ReadInt());
            }

        }

        protected override void writeCommand(NetworkWriter writer)
        {
          
            writer.WriteInt(Ships.Count);
            foreach(int sh in Ships)
            {
                writer.WriteInt(sh);
            }
            writeCommand2(writer);
        }
        protected abstract void writeCommand2(NetworkWriter writer);
    }
}