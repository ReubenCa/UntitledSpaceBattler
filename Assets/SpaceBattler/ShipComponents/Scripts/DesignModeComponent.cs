using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceBattler
{
   
    public class DesignModeComponent : MonoBehaviour
    {
        //public ShipComponent ShipComp { private set; get; }
        public bool Connected;
        private bool Placed;
        public int XSize = 1;
        public int YSize = 1;

        public bool CanBeDeleted = true;

        public bool ConnectableUp;
        public bool ConnectableRight;
        public bool ConnectableDown;
        public bool ConnectableLeft;

        public Sprite PlacedSprite;
        public Sprite ValidUnplacedSprite;
        public Sprite InvalidUnplacedSprite;
        public Sprite InvalidPlacedSprite;


        /// <summary>
        /// (X1, Y1, X2, Y2) X1<X2 , Y1<Y2  In situations where the size of the Comp is known or not relevant the position can be referred to with only the first two elements
        /// </summary>
        public (int, int, int, int) Coords { private set; get; }
        [SerializeField]
        private bool cockpit;
        public bool Cockpit
        {
            get
            {
                return cockpit;
            }
        }


        
        public enum Rotations
        {
            up = 0,
            right = 1,
            down = 2,
            left = 3
        }

        private Rotations rotation;
        public Rotations Rotation
        {
            get
            {
                return rotation;
            }
           private set
            {
                rotation = value;
                UpdateSpriteRotation();
            }
        }

        public enum States
        {
            Placed,
            ValidUnplaced,
            InvalidUnplaced,
            InvalidPlaced
        }
        private States state;
        States State
        {
            get { return state; }
            set
            {
                //Debug.Log("State Set " + value.ToString());
                switch (value)
                {
                    case States.Placed:
                        SwitchToPlaced();
                        break;
                    case States.ValidUnplaced:
                        SwitchToValidUnPlaced();
                        break;
                    case States.InvalidPlaced:
                        SwitchToInValidPlaced();
                        break;
                    case States.InvalidUnplaced:
                        SwitchToInValidUnPlaced();
                        break;
                }
                state = value;
            }
        }


        #region SpriteManagement

        new private SpriteRenderer renderer;
        private void SwitchToPlaced()
        {
            renderer.sprite = PlacedSprite;
            renderer.color = Color.white;
            Placed = true;
        }

        private void SwitchToValidUnPlaced()
        {
            if (ValidUnplacedSprite != null)
            {
                renderer.sprite = ValidUnplacedSprite;
            }
            renderer.color = Color.green;
            Placed = false;
        }
        private void SwitchToInValidUnPlaced()
        {
            if (InvalidUnplacedSprite != null)
            {
                renderer.sprite = InvalidUnplacedSprite;
            }
            else if (InvalidPlacedSprite != null)
            {
                renderer.sprite = InvalidPlacedSprite;
            }
            renderer.color = Color.red;
            Placed = false;
        }

        private void SwitchToInValidPlaced()
        {
            if (InvalidPlacedSprite != null)
            {
                renderer.sprite = InvalidPlacedSprite;
            }
            else if (InvalidUnplacedSprite != null)
            {
                renderer.sprite = InvalidUnplacedSprite;
            }
            renderer.color = Color.red;
            Placed = true;
        }

        #endregion
        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
            Rotation = Rotations.up;
           // GatherData();
        }

        public void Init()
        {
           
        }

        #region ManageDesign
        public void SetState(States State)
        {
            this.State = State;
        }


        public void SetPlaced(bool valid, int X, int Y)
        {
            Coords = (X, Y, X + XSize - 1, Y + YSize - 1);
            if (valid)
            {
                SetState(States.Placed);
            }
            else
            {
                SetState(States.InvalidPlaced);
            }

        }
        public void SetRotation(Rotations r)
        {
            Rotation = r;
        }

        public void Rotate(int amount)
        {
            Rotation = (Rotations)(((int)Rotation + amount) % 4);

        }
        private void UpdateSpriteRotation()
        {
            //Quaternion fromAngle = transform.rotation;
            Quaternion toAngle = Quaternion.Euler(new Vector3(0, 0, -90f * (int)rotation));
            transform.rotation = toAngle;
        }

        public void RefreshValidity()
        {
            if (!Placed)
            {
                return;
            }
            if (Connected)
            {
                State = States.Placed;
            }
            else
            {
                State = States.InvalidPlaced;
            }
        }

        public bool CanConnect(Rotations Direction)
        {
            Rotations RelativeDirection = (Rotations)(((int)Direction - (int)Rotation + 4) % 4);
            return RelativeDirection switch
            {
                Rotations.up => ConnectableUp,
                Rotations.right => ConnectableRight,
                Rotations.down => ConnectableDown,
                Rotations.left => ConnectableLeft,
                _ => throw new System.Exception(),
            };
        }
        #endregion


        #region DataGathering

        public GameObject ActivePrefab;

        private string componentID;
        public string ComponentID { 
            private set
            {
                componentID = value;
            }
            
            get
            {
                if (componentID != null)
                {
                    return componentID;
                }
                componentID = GetID();
                 return componentID;
            }
                
                }
        public void GatherData()
        {
            ShipComponent ShipComp = ActivePrefab.GetComponent<ShipComponent>();
            Debug.Assert(ShipComp != null);
            ComponentID = ShipComp.ComponentID;
        }

        public string GetID()
        {
            ShipComponent ShipComp = ActivePrefab.GetComponent<ShipComponent>();
            Debug.Assert(ShipComp != null);
            return ShipComp.ComponentID;
        }
        #endregion
    }
}