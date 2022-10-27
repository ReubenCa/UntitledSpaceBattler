using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace SpaceBattler
{
    public class ShipManager : NetworkBehaviour
    {
        public GameObject ShipPrefab;
        public void SpawnShip(OverallShipData ShipData, int PlayerID)
        {
            HashSet<IndividualComponentData> IndividualComponents = ShipData.Components;
            GameObject ship = Instantiate(ShipPrefab);
            foreach(IndividualComponentData Comp in IndividualComponents)
            {
                GameObject prefab = ServerGameManager.instance.GetComponentPrefab(Comp.ComponentID);
                GameObject.Instantiate(prefab, ship.transform);
            }



             NetworkServer.Spawn(ship);
        }
    }
}