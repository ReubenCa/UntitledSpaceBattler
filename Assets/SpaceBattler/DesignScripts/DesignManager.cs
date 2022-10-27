using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceBattler
{
    public class DesignManager : MonoBehaviour
    {
        //eg it might be loading an old design or not have selected a size yet
        public enum States
        {
            DesigningShip
        }
        public States State { private set; get; }
        private GameObject CurrentlySelected;
        private DesignModeComponent CurrentDesignComp;
        DesignGridClass Grid;
        public int MinX;
        public int MaxX;
        public int MinY;
        public int MaxY;
        public GameObject CockpitPrefab;
        private GameObject CurrentPrefab;
        // Start is called before the first frame update
        public List<GameObject> ComponentPrefabs;
        private Dictionary<string, GameObject> StringToPrefab = new Dictionary<string, GameObject>();

       


        private void Awake()
        {
            
        }

        void Start()
        {

            foreach (GameObject GO in ComponentPrefabs)
            {
                DesignModeComponent DMC = GO.GetComponent<DesignModeComponent>();
                DMC.GatherData();
                StringToPrefab.Add(DMC.ComponentID, GO);
            }
            Grid = new DesignGridClass(MinY, MaxX, MinY, MaxY);
            State = States.DesigningShip;
            GameObject Cockpit = InstansiateComponent(CockpitPrefab, 0, 0);//Instantiate(CockpitPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Cockpit.GetComponent<DesignModeComponent>().SetPlaced(true, 0, 0);
            Grid.RegisterComponent(Cockpit, 0, 0);

        }
        public GameObject InstansiateComponent(GameObject prefab, int X, int Y)
        {
            GameObject r = Instantiate(prefab, new Vector3(X, Y), Quaternion.identity);
            r.GetComponent<DesignModeComponent>().Init();
            return r;
        }

        // Update is called once per frame

        public void UpdateSelected(GameObject NewPrefab)
        {
            CurrentPrefab = NewPrefab;
            //GameObject NewObj = Instantiate(NewPrefab);
            GameObject NewObj = InstansiateComponent(NewPrefab,0,0);
            if (CurrentlySelected != null)
            {
                Destroy(CurrentlySelected);
            }
            CurrentlySelected = NewObj;
            CurrentDesignComp = NewObj.GetComponent<DesignModeComponent>();
            Debug.Assert(CurrentDesignComp != null);
        }
        void Update()
        {
            switch (State)
            {
                case States.DesigningShip:
                    UpdateDesigningShip();
                    break;
            }
        }

        void UpdateDesigningShip()
        {
            if (CurrentlySelected == null)
            {
                return;
            }

            GetMouseClippedPos(out int x, out int y);
            CurrentlySelected.transform.position = new Vector3(x, y);
            bool CanBePlaced;
            bool Connected = Grid.WouldBeConnected(CurrentDesignComp, x, y);
            if (Grid.CanBePlaced(x, y, CurrentDesignComp.XSize, CurrentDesignComp.YSize, true) && Connected)
            {
                CurrentDesignComp.SetState(DesignModeComponent.States.ValidUnplaced);
                CanBePlaced = true;
            }
            else
            {
                CurrentDesignComp.SetState(DesignModeComponent.States.InvalidUnplaced);
                CanBePlaced = false;
            }


            if (Input.GetKeyDown("e"))
            {
                CurrentDesignComp.Rotate(1);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (CanBePlaced)
                {
                    CurrentDesignComp.SetPlaced(Connected, x, y);//Needs to be first so its coords are set
                    Grid.RegisterComponent(CurrentlySelected, x, y);

                    CurrentlySelected = null;
                    CurrentDesignComp = null;
                    UpdateSelected(CurrentPrefab);
                }

            }
            if(Input.GetMouseButtonDown(1))
            {
                DesignModeComponent DMC = Grid.SafeGetDesignComp(x,y);
                if (DMC != null)
                {
                    Grid.DeleteComponent(x, y);
                }
            }
        }

        void GetMouseClippedPos(out int x, out int y)
        {
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pz.z = 0;
            gameObject.transform.position = pz;
            //   Debug.Log("Mouse clicked at " + (pz.x).ToString() + "," + (pz.y).ToString());
            x = (int)Mathf.Floor(pz.x + 0.5f);
            y = (int)Mathf.Floor(pz.y + 0.5f);
        }
    
        public void WriteShipToFile(string filepath)
        {
            ShipSaveSystem.WriteShip(Grid.ConvertToData(), filepath);
        }

        public void ReadShipFromFile(string filepath)
        {
            Grid.DeleteAll();
            Grid = new DesignGridClass(MinX, MaxX, MinY, MaxY);
            //OverallShipData ShipDat = ShipSaveSystem.ReadShip(filepath);
            OverallShipData ShipDat;
            try
            {
                ShipDat = ShipSaveSystem.ReadShip(filepath);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                return;
            }

            foreach ( IndividualComponentData Dat in ShipDat.Components)
            {
                GameObject Prefab = StringToPrefab[Dat.ComponentID];
                //GameObject Comp = Instantiate(Prefab, new Vector3(Dat.X, Dat.Y), Quaternion.identity);
                GameObject Comp = InstansiateComponent(Prefab, Dat.X, Dat.Y);
                
                DesignModeComponent DMC = Comp.GetComponent<DesignModeComponent>();
                DMC.SetRotation(Dat.Rotation);
                DMC.SetPlaced(false, Dat.X, Dat.Y);
                Grid.RegisterComponent(Comp, Dat.X, Dat.Y, true);
                
            }
            Grid.UpdateConnections();
        }

        
    }
}