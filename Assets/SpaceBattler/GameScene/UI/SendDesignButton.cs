using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace SpaceBattler
{
    public class SendDesignButton : MonoBehaviour
    {
        public TMP_Dropdown dropdown;
        public void OnClick()
        {
            string name = dropdown.options[dropdown.value].text;
            ClientGameManager.instance.SendShipToServer(name);
        }
    }
}