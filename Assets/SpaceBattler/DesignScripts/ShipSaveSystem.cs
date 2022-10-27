using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Mirror;
namespace SpaceBattler
{
    public static class ShipSaveSystem
    {
        const string SavesListfp = "saveslist.saves";
        public static void WriteShip(OverallShipData Ship, string filepath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + filepath + ".ship";
            FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, Ship);
            stream.Close();
            Debug.Log("Saved to: " + path);
            if (!Saves.Contains(filepath))
            {
                saves.Add(filepath);//Means saves list is still up to date
                WriteSavesSet();
            }
            
            SavesUpdated?.Invoke();//This then triggers the dropdown to read the saves list which triggers it reading again.
        }
        public static OverallShipData ReadShip(string filepath)
        {
            string path = Application.persistentDataPath + "/" + filepath + ".ship";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                OverallShipData ship = formatter.Deserialize(stream) as OverallShipData;
                stream.Close();
                return ship;
            }
            else
            {
                Debug.LogError("Path does not exist: " + filepath);
                throw new System.Exception();
            }
        }
        static bool SavesListUpToDate = false;
        private static void WriteSavesSet()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + SavesListfp;
            FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, Saves);
            stream.Close();
            
            Debug.Log("File Saved as " + path);
        }

        private static void ReadSavesSet()
        {
            string path = Application.persistentDataPath + "/" + SavesListfp;

            if (!File.Exists(path))
            {
                Debug.Log("Could not find saves list, saving new one now" + SavesListfp);
                saves = new HashSet<string>();
                WriteSavesSet();
            }
            else
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                if (stream.Length > 0)
                {
                    saves = formatter.Deserialize(stream) as HashSet<string>;
                }
                else
                {
                    saves = new HashSet<string>();
                }
                stream.Close();
            }
            SavesListUpToDate = true;
            
        }

        public static HashSet<string> Saves
        {
            get
            {
                if (!SavesListUpToDate)
                {
                    ReadSavesSet();
                }

                HashSet<string> r = new HashSet<string>();
                foreach (string s in saves)
                {
                    r.Add(s);
                }
                return r;
            }
        }


        private static HashSet<string> saves;

        public delegate void TestDelegate();
           


        public static event TestDelegate SavesUpdated;
    }

    [System.Serializable]
    public class OverallShipData
    {
        
        public HashSet<IndividualComponentData> Components { private set; get; }
        public OverallShipData(HashSet<IndividualComponentData> Components)
        {
            this.Components = Components;
         
        }

        public bool Validate()
        {
            Debug.Log("Ship Validation not Implemented yet");
            return true;
        }
    }
    [System.Serializable]
    public struct IndividualComponentData
    {
        public readonly string ComponentID;
        public readonly int X;
        public readonly int Y;
        //
        public DesignModeComponent.Rotations Rotation;
       
       
        public IndividualComponentData(string ComponentID, DesignModeComponent.Rotations Rotation,int X, int Y)
        {
            this.ComponentID = ComponentID;
            this.X = X;
            this.Y = Y;
            this.Rotation = Rotation;
        }

    }
}