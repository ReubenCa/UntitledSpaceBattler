using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBattler
{
    public class ClientGameManager : MonoBehaviour
    {

        static int NextDesignID = 0;
        
      
        public GameObject CreateShipButtonPrefab;
        public GameObject CreateshipPanel;
        public static ClientGameManager instance;
        public PlayerManager PlayerMan { private set; get; }
        public InputManager InputMan { private set; get; }
        public PlayerObject PlayerObj { private set; get; }
        public PlayerList playerList { private set; get; }
        public enum States
        {
            Halted,
            Spectating,
            Running
        }
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogError("Client Game Manager already exists");
                Destroy(this);
                return;
            }
        }

        private void Start()
        {
            PlayerMan = FindObjectOfType<PlayerManager>();
            InputMan = FindObjectOfType<InputManager>();
            playerList = FindObjectOfType<PlayerList>();
            
        }
        public void RegisterPlayerObj(PlayerObject PO)
        {
            PlayerObj = PO;
        }

        public void SendCommand(Command C)
        {
            PlayerObj.SendCommand(C);
        }
        public void CreateShip(int DesignID)
        {
            throw new System.NotImplementedException();
        }
        public void SendShipToServer(string ShipSaveName)
        {
            OverallShipData Ship = ShipSaveSystem.ReadShip(ShipSaveName);
            Command C = new CommandRegisterDesign(PlayerObj.PlayerID, NextDesignID, ShipSaveName, Ship);
            NextDesignID++;
            SendCommand(C);
           
        }

        public void AddShipButton(string ShipName, int DesignID)
        {
            GameObject NewButton = Instantiate(CreateShipButtonPrefab, CreateshipPanel.transform);
            NewButton.GetComponent<CreateShipButton>().Init(ShipName, DesignID);
        }
    }
}