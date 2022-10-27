using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceBattler
{
    public class ShipComponent : MonoBehaviour
    {
        [SerializeField]
        private string componentID;
        public string ComponentID
        {
            get
            {
                return componentID;
            }
        }
       
        

    }
}