using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace SpaceBattler
{
    public class LoadButton : MonoBehaviour
    {
        public TMP_Dropdown dropdown;
        public DesignManager designManager;
        public void Onclick()
        {
            int index = dropdown.value;
            string name = dropdown.options[index].text;
            //try
            //{
            //    OverallShipData Dat = ShipSaveSystem.ReadShip(name);
            //}
            //catch (System.Exception e)
            //{
            //    Debug.LogException(e);
            //    return;
            //}
            designManager.ReadShipFromFile(name);

        }
    }
}