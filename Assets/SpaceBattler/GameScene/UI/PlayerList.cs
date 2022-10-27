using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceBattler
{
    public class PlayerList : MonoBehaviour
    {
        public GameObject PlayerPanelPrefab;
        public GameObject PanelParent;
        Dictionary<int, PlayerPanel> Panels = new Dictionary<int, PlayerPanel>();

        //List<(GlobalPlayerInfo, PlayerPanel)> Players = new List<(GlobalPlayerInfo, PlayerPanel)>();
        public void RefreshDisplay(List<GlobalPlayerInfo> newInfo)
        {
            HashSet<int> PlayerIDsinNewList = new HashSet<int>();
            foreach (GlobalPlayerInfo info in newInfo)
            {
                PlayerIDsinNewList.Add(info.PlayerID);
                if(Panels.TryGetValue(info.PlayerID, out PlayerPanel panel))
                {
                    panel.UpdateInfo(info);
                }
                else
                {
                    GameObject NewPlayerPanelGameObj = Instantiate(PlayerPanelPrefab, PanelParent.transform);
                    PlayerPanel NewPlayerPanel = NewPlayerPanelGameObj.GetComponent<PlayerPanel>();
                    Panels[info.PlayerID] = NewPlayerPanel;
                    NewPlayerPanel.UpdateInfo(info);
                    
                }
            }
            foreach(KeyValuePair<int, PlayerPanel> KeyVal in Panels)
            {
                PlayerPanel panel = KeyVal.Value;
                int ID = KeyVal.Key;
                if (!PlayerIDsinNewList.Contains(ID))
                {
                    Destroy(panel.gameObject);
                    Panels.Remove(ID);
                }
            }
        }
    }
}