using System.Collections.Generic;
using UnityEngine;
namespace SpaceBattler
{
    public class DesignGridClass
    {
        GameObject[,] GOs;
        DesignModeComponent[,] DesignComps;
        List<DesignModeComponent> CompsList;
        (int, int, int, int) GridCoords;

        #region GridAccesorMethods
        public void SetGO(GameObject GO, int X, int Y)
        {
            GOs[X - GridCoords.Item1, Y - GridCoords.Item2] = GO;
        }


        public GameObject GetGO(int X, int Y)
        {

            return GOs[X - GridCoords.Item1, Y - GridCoords.Item2];
        }
        /// <summary>
        /// Returns null if out of bounds
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public GameObject SafeGetGO(int X, int Y)
        {
            int TrueX = X - GridCoords.Item1;
            int TrueY = Y - GridCoords.Item2;
            if (TrueX < GOs.GetLength(0) && TrueY < GOs.GetLength(1) && TrueX >= 0 && TrueY >= 0)
            {
                //Debug.Log(X.ToString() + "," + Y.ToString());
                return GOs[TrueX, TrueY];
            }
            else
            {
                return null;
            }
        }

        public void SetComp(DesignModeComponent DMC, int X, int Y)
        {
            DesignComps[X - GridCoords.Item1, Y - GridCoords.Item2] = DMC;
        }
        /// <summary>
        /// Returns null if out of bounds
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public DesignModeComponent SafeGetDesignComp(int X, int Y)
        {
            int TrueX = X - GridCoords.Item1;
            int TrueY = Y - GridCoords.Item2;
            if (TrueX < GOs.GetLength(0) && TrueY < GOs.GetLength(1) && TrueX >= 0 && TrueY >= 0)
            {
                //Debug.Log(X.ToString() + "," + Y.ToString());
                return DesignComps[TrueX, TrueY];
            }
            else
            {
                return null;
            }
        }
        public DesignModeComponent GetDesignComp(int X, int Y)
        {
            return DesignComps[X - GridCoords.Item1, Y - GridCoords.Item2];
        }

        #endregion

        public DesignGridClass(int XMin, int XMax, int YMin, int YMax)
        {
            int Width = XMax - XMin;
            int Height = YMax - YMin;
            GOs = new GameObject[Width, Height];
            DesignComps = new DesignModeComponent[Width, Height];
            GridCoords = (XMin, YMin, XMax, YMax);
            CompsList = new List<DesignModeComponent>();
        }
        /// <summary>
        /// LowX, LowY, HighX, HighY
        /// </summary>
        (int, int, int, int) CockpitCoords;



        public void RegisterComponent(GameObject Component, int X, int Y, bool NoUpdates = false)
        {

            //ShipComponent ShipComp = Component.GetComponent<ShipComponent>();
            
            DesignModeComponent DesignModeComp = Component.GetComponent<DesignModeComponent>();
            Debug.Assert(DesignModeComp != null);
            CompsList.Add(DesignModeComp);
            int LowX = X;
            int LowY = Y;
            int HighX = X + DesignModeComp.XSize - 1;
            int HighY = Y + DesignModeComp.YSize - 1;

            if (DesignModeComp.Cockpit)
            {
                CockpitCoords = (X, Y, X + DesignModeComp.YSize - 1, Y + DesignModeComp.XSize - 1);
            }


            for (int i = LowX; i <= HighX; i++)
            {
                for (int j = LowY; j <= HighY; j++)
                {
                    if (SafeGetGO(i, j) != null)
                    {
                        DeleteComponent(i, j, NoUpdates);
                    }
                    SetGO(Component, i, j);
                    SetComp(DesignModeComp, i, j);
                }
            }
            if (!NoUpdates)
            {
                UpdateConnections();
            }
        }

        public void DeleteComponent(int x, int y, bool NoUpdates = false)
        {
            DesignModeComponent Comp = SafeGetDesignComp(x, y);
            CompsList.Remove(Comp);
            if (Comp == null)
            {
                Debug.Log("Tried to delete non existant component");
                return;
            }
            int XSize = Comp.XSize;
            int YSize = Comp.YSize;
            for (int i = x; i < XSize + x; i++)
            {
                for (int j = y; j < YSize + y; j++)
                {
                    SetGO(null, i, j);
                    SetComp(null, i, j);
                }
            }
            GameObject.Destroy(Comp.gameObject);
            if (!NoUpdates)
            {
                UpdateConnections();
            }
        }

        public bool CanBePlaced(int X, int Y, int XSize, int YSize, bool PlaceOnTop)
        {
            if (!WithinGrid(X, Y, XSize, YSize))
            {
                return false;
            }
            for (int x = X; x < X + XSize; x++)
            {
                for (int y = Y; y < Y + YSize; y++)
                {

                    if (SafeGetDesignComp(x, y) != null)
                    {
                        if (!PlaceOnTop || !SafeGetDesignComp(x, y).CanBeDeleted)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool WithinGrid(int X, int Y, int XSize, int YSize)
        {
            return X >= GridCoords.Item1 && Y >= GridCoords.Item2 && X <= GridCoords.Item3 && Y <= GridCoords.Item4;
        }

        public void UpdateConnections()
        {
            
            foreach (DesignModeComponent DMC in CompsList)
            {
                DMC.Connected = false;
            }


            Queue<DesignModeComponent> UncheckedQ = new Queue<DesignModeComponent>();
            //minx, miny, maxx, maxy
            UncheckedQ.Enqueue(SafeGetDesignComp(CockpitCoords.Item1, CockpitCoords.Item2));
            while (UncheckedQ.Count > 0)
            {
                //(DesignModeComponent, int, int ,int ,int) DQ = UncheckedQ.Dequeue();
                DesignModeComponent CurrentItem = UncheckedQ.Dequeue();
                if (CurrentItem.Connected)
                {
                    continue;//Component was already Q'd by something else
                }

                int XMin = CurrentItem.Coords.Item1;
                int YMin = CurrentItem.Coords.Item2;
                int XMax = CurrentItem.Coords.Item3;
                int YMax = CurrentItem.Coords.Item4;
                //Debug.Log(XMin.ToString() + "," + YMin.ToString());
                CurrentItem.Connected = true;
                //Add all neighbouring Components
                //Make sure to convert size to xmin xmax
                for (int x = XMin; x <= XMax; x++)
                {
                    if (CurrentItem.CanConnect(DesignModeComponent.Rotations.down) && WithinGrid(x, YMin - 1, 1, 1))
                    {
                        DesignModeComponent DMC = SafeGetDesignComp(x, YMin - 1);
                        if (DMC != null && DMC.CanConnect(DesignModeComponent.Rotations.up))
                        {
                            UncheckedQ.Enqueue(DMC);
                        }
                    }

                    if (CurrentItem.CanConnect(DesignModeComponent.Rotations.up) && WithinGrid(x, YMin + 1, 1, 1))
                    {
                        DesignModeComponent DMC = SafeGetDesignComp(x, YMin + 1);
                        if (DMC != null && DMC.CanConnect(DesignModeComponent.Rotations.down))
                        {
                            UncheckedQ.Enqueue(DMC);
                        }
                    }
                }


                for (int y = YMin; y <= YMax; y++)
                {
                    if (CurrentItem.CanConnect(DesignModeComponent.Rotations.left) && WithinGrid(y, XMin - 1, 1, 1))
                    {
                        DesignModeComponent DMC = SafeGetDesignComp(XMin - 1, y);
                        if (DMC != null && DMC.CanConnect(DesignModeComponent.Rotations.right))
                        {
                            UncheckedQ.Enqueue(DMC);
                        }
                    }

                    if (CurrentItem.CanConnect(DesignModeComponent.Rotations.right) && WithinGrid(y, XMin + 1, 1, 1))
                    {
                        DesignModeComponent DMC = SafeGetDesignComp(XMin + 1, y);
                        if (DMC != null && DMC.CanConnect(DesignModeComponent.Rotations.left))
                        {
                            UncheckedQ.Enqueue(DMC);
                        }
                    }
                }


            }
            foreach (DesignModeComponent DMC in CompsList)
            {
                DMC.RefreshValidity();
            }
        }


       
        /// <summary>
        /// Returns false if the part would not be connected or lies outside the grid
        /// </summary>
        /// <param name="DMC"></param>
        /// <param name="XMin"></param>
        /// <param name="YMin"></param>
        /// <returns></returns>
        public bool WouldBeConnected(DesignModeComponent DMC, int XMin, int YMin)
        {

            int XMax = XMin + DMC.XSize - 1;
            int YMax = YMin + DMC.YSize - 1;
            if (!WithinGrid(XMin, YMin, DMC.XSize, DMC.YSize))
            {
                return false;
            }
            for (int x = XMin; x <= XMax; x++)
            {
                if (DMC.CanConnect(DesignModeComponent.Rotations.down) && WithinGrid(x, YMin - 1, 1, 1))
                {
                   DesignModeComponent LowerDMC = SafeGetDesignComp(x, YMin - 1);
                    if (LowerDMC != null && LowerDMC.CanConnect(DesignModeComponent.Rotations.up) && LowerDMC.Connected)
                    {
                        return true;
                    }
                }

                if (DMC.CanConnect(DesignModeComponent.Rotations.up) && WithinGrid(x, YMin + 1, 1, 1))
                {
                    DesignModeComponent UpperDMC = SafeGetDesignComp(x, YMin + 1);
                    if (UpperDMC != null && UpperDMC.CanConnect(DesignModeComponent.Rotations.down) && UpperDMC.Connected)
                    {
                        return true;
                    }
                }
            }


            for (int y = YMin; y <= YMax; y++)
            {
                if (DMC.CanConnect(DesignModeComponent.Rotations.left) && WithinGrid(y, XMin - 1, 1, 1))
                {
                    DesignModeComponent LeftDMC = SafeGetDesignComp(XMin - 1, y);
                    if (LeftDMC != null && LeftDMC.CanConnect(DesignModeComponent.Rotations.right) && LeftDMC.Connected)
                    {
                        return true;
                    }
                }

                if (DMC.CanConnect(DesignModeComponent.Rotations.right) && WithinGrid(y, XMin + 1, 1, 1))
                {
                    DesignModeComponent RightDMC = SafeGetDesignComp(XMin + 1, y);
                    if (RightDMC != null && RightDMC.CanConnect(DesignModeComponent.Rotations.left) && RightDMC.Connected)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    
        public OverallShipData ConvertToData()
        {
            HashSet<IndividualComponentData> L = new HashSet<IndividualComponentData>();
            foreach(DesignModeComponent Comp in CompsList)
            {
                L.Add( new IndividualComponentData(Comp.ComponentID,Comp.Rotation,Comp.Coords.Item1, Comp.Coords.Item2));
            }

            return new OverallShipData(L);
        }
        

        public void DeleteAll()
        {
            Queue<(int, int)> DeleteQ = new Queue<(int, int)>();
            foreach (DesignModeComponent C in CompsList)
            {
                DeleteQ.Enqueue((C.Coords.Item1, C.Coords.Item2));
            }
            while (DeleteQ.Count > 0)
            {
                (int, int) XY = DeleteQ.Dequeue();
                DeleteComponent(XY.Item1, XY.Item2,true);

            }
        }

        
    }
}