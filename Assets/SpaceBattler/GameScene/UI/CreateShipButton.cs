using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpaceBattler
{
    public class CreateShipButton : MonoBehaviour
    {
        int DesignID;
        string DesignName;
        [SerializeField]
        private TMP_Text text;
        public void Init(string DesignName, int DesignID)
        {
            gameObject.SetActive(true);
            this.DesignName = DesignName;
            this.DesignID = DesignID;
            text.text = DesignName;
            
        }

        public void OnClick()
        {
            ClientGameManager.instance.CreateShip(DesignID);
           
        }

    }
}