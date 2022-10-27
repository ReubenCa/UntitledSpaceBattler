using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceBattler
{
    public class InputManager : MonoBehaviour
    {

        private GameObject ShipListPanel;
        List<Ship> OwnedShips = new List<Ship>();


        public void AddShip(Ship ship)
        {
            OwnedShips.Add(ship);
        }

        public void RemoveShip(Ship ship)
        {
            OwnedShips.Remove(ship);
        }

        public void AddShipDesign(string DesignName, int DesignID)
        {

        }
    }
}