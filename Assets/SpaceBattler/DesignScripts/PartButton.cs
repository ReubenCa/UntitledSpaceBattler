using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceBattler
{
    public class PartButton : MonoBehaviour
    {
        public GameObject PartPrefab;
        private DesignManager DesignMan;
        private void Awake()
        {
            Debug.Assert(PartPrefab != null);
            DesignMan = FindObjectOfType<DesignManager>();

        }


        public void OnClick()
        {
            GameObject NewObj = Instantiate(PartPrefab);
            DesignMan.UpdateSelected(NewObj);
        }

    }
}