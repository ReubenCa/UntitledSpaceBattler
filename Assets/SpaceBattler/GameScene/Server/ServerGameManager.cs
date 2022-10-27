using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace SpaceBattler
{
    public class ServerGameManager : NetworkBehaviour
    {
       // Dictionary<long, Entity> Entities = new Dictionary<long, Entity>();
        public enum States
        {
            Running,
            Halted
        }
        States State = States.Halted;
        static public ServerGameManager instance;
        private Queue<Command> commands = new Queue<Command>();
        [SerializeField]
        private float PlayerInputUpdatesFrequency;
        [SerializeField]
        private float AIInputUpdaterequency;
        [SerializeField]
        private List<GameObject> SpawnableShipComponents;
        private Dictionary<string, GameObject> ShipCompsByID;
        void Awake()
        {
            
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogError("GameManager Already Exists");
                Destroy(this);
                return;
            }
            HumanUpdateTimespan = 1 / PlayerInputUpdatesFrequency;
            AIUpdateTimespan = 1 / AIInputUpdaterequency;
            foreach(GameObject shipComponent in SpawnableShipComponents)
            {
                ShipCompsByID.Add(shipComponent.GetComponent<ShipComponent>().ComponentID, shipComponent);
            }
        }

        public PlayerManager playerManager { private set; get; }
        public ShipManager shipManager { private set; get; }
        public GameObject GetComponentPrefab(string ID)
        {
            
           return ShipCompsByID[ID];
            
            
        }

        
        private void Start()
        {
            playerManager = FindObjectOfType<PlayerManager>();
            shipManager = FindObjectOfType<ShipManager>();
            Debug.Assert(playerManager is not null && shipManager is not null);
        }

        public override void OnStartServer()
        {
            Debug.Log("ServerGame Manager initiliazed");
            if(isServerOnly)
            {
                FindObjectOfType<JoinGameButton>()?.gameObject.SetActive(false);
            }
        }
        
        public void QueueCommand(Command com)
        {
            commands.Enqueue(com);
        }

        float HumanUpdateTimespan;
        float AIUpdateTimespan;
        float TimeSinceHumanUpdate = 0;
        float TimesinceAIUpdate = 0;
        private void Update()
        {
            TimeSinceHumanUpdate += Time.deltaTime;
            TimesinceAIUpdate += Time.deltaTime;
            if(TimeSinceHumanUpdate >= HumanUpdateTimespan)
            {
                playerManager.ReceiveHumanInputs();
                TimeSinceHumanUpdate = 0;
            }
            if(TimesinceAIUpdate >= AIUpdateTimespan)
            {
                playerManager.ComputeAICommands();
                TimesinceAIUpdate = 0;
            }
            while(commands.Count>0)
            {
                Command C = commands.Dequeue();
                bool Success = C.VerifyAndExecute();

                Player player = playerManager.GetPlayer(C.PlayerID);
                if(!player.IsAI)
                {
                    ((Human)player).playerObj.CommandResponse(C.CommandID, Success);
                }
            }
        }
    }
}