using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace SpaceBattler
{
    public  static partial class CustomSerializer
    {
        public static void WriteCommand(this NetworkWriter writer, Command value)
        {
            value.WriteCommand(writer);
            
        }

        public static Command ReadCommand(this NetworkReader reader)
        {
            return Command.Read(reader);
        }
    }
}