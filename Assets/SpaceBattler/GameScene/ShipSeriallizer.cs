using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace SpaceBattler
{
    public static partial class CustomSerializer
    {
        public static void WriteOverallShipData(this NetworkWriter writer, OverallShipData value)
        {
      
            writer.WriteHashSet<IndividualComponentData>(value.Components);
           
        }
        public static void WriteComponent(this NetworkWriter writer, IndividualComponentData value)
        {
            writer.WriteString(value.ComponentID);
            writer.WriteInt(value.X);
            writer.WriteInt(value.Y);
            writer.WriteInt((int)value.Rotation);
        }
        public static IndividualComponentData ReadComponent(this NetworkReader reader)
        {
            string ComponentID = reader.ReadString();
            int X = reader.ReadInt();
            int Y = reader.ReadInt();
            DesignModeComponent.Rotations Rotation = (DesignModeComponent.Rotations)reader.ReadInt();
            return new IndividualComponentData(ComponentID, Rotation, X, Y);
        }
        public static OverallShipData ReadShipData(this NetworkReader reader)
        {
            HashSet<IndividualComponentData> Comps = reader.ReadHashSet<IndividualComponentData>();

            return new OverallShipData(Comps);
        }

        public static void WriteHashSet<T>(this NetworkWriter writer, HashSet<T> Set)
        {
          
            writer.Write(Set.Count);
       
            foreach (T C in Set)
            {
              writer.Write((T)C);
            }
        }


     
        public static HashSet<T> ReadHashSet<T>(this NetworkReader reader)
        {
            int Count = reader.ReadInt();
            HashSet<T> Set = new HashSet<T>(Count);
            for (int i = 0; i < Count; i++)
            {
                Set.Add(reader.Read<T>());
            }
            return Set;
        }
    }
}