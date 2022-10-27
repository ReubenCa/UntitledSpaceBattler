using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;
namespace SpaceBattler
{
    public class SaveButton : MonoBehaviour
    {
        public InputField Input;
        DesignManager DM;

        private void Start()
        {
            DM = FindObjectOfType<DesignManager>();
        }
        public void OnClick()
        {
            string name = Input.text;
            if (name == "" || name == null)
            {
                Debug.LogError("Invalid save name");
                return;
            }
            DM.WriteShipToFile(name);
        }
    }
}