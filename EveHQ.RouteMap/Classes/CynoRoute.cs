// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2011  EveHQ Development Team
// 
// This file is part of the "EveHQ RouteMap" plug-in
//
// EveHQ RouteMap derives from copyrighted works licensed under the GNU 
// General Public License originally created by Lhyda Souljacker.
//
// This version has been modified pursuant to the GNU General Public 
// License as of 4 September 2011, and as distributed, it includes or is 
// derivative of works licensed under the GNU General Public License.
//
// EveHQ is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// EveHQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
// ========================================================================
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace EveHQ.RouteMap
{
    [Serializable]
    public class CynoRoute
    {
        private const float MAXIMUM_JUMP_DISTANCE = 14.6251f;
        private const float MAXIMUM_JUMP_SEC = 0.45f;

        #region Delegates

        public delegate void SystemProcessedHandler(object sender, ProgressChangedEventArgs e);

        #endregion

        private static readonly Dictionary<SolarSystem, Vertex> SystemsList =
            new Dictionary<SolarSystem, Vertex>(Convert.ToInt32(PlugInData.GalMap.GalData.TotalSystems));

        private static Route RouteParameters;
        public static Dictionary<SolarSystem, List<SolarSystem>> StaticAdjacencies; //This list gets serialized 

        private readonly Dictionary<SolarSystem, FibonacciHeap<Vertex>.Node> NodeList =
            new Dictionary<SolarSystem, FibonacciHeap<Vertex>.Node>();

        private readonly FibonacciHeap<Vertex> SystemPriorityQueue = new FibonacciHeap<Vertex>();
        private float PilotMaxRange;

        public CynoRoute()
        {
            StaticAdjacencies = new Dictionary<SolarSystem, List<SolarSystem>>(PlugInData.UniverseGraph);

            foreach (Region r in PlugInData.GalMap.GalData.Regions.Values)
            {
                foreach (Constellation cc in r.Constellations.Values)
                {
                    foreach (SolarSystem ss in cc.Systems.Values)
                    {
                        Vertex newVert = new Vertex();
                        newVert.SolarSystem = ss;
                        newVert.Cost = float.MaxValue;
                        newVert.LYTraveled = float.MaxValue;

                        if (!SystemsList.ContainsKey(newVert.SolarSystem))
                            SystemsList.Add(newVert.SolarSystem, newVert);
                    }
                }
            }
        }

        public event SystemProcessedHandler SystemProcessed;

        private void ResetGraph()
        {
            foreach (var v in SystemsList.Values)
            {
                v.Cost = float.MaxValue;
                v.LYTraveled = float.MaxValue;
            }
            NodeList.Clear();
            SystemPriorityQueue.clear();
        }

        public List<Vertex> GetShortestPath(SolarSystem Start, SolarSystem End)
        {
            Vertex Current;
            SystemsList[Start].Cost = 0;
            SystemsList[Start].LYTraveled = 0;

            SystemPriorityQueue.insert(SystemsList[Start], SystemsList[Start].Cost);

            while (!SystemPriorityQueue.isEmpty())
            {
                Current = SystemPriorityQueue.removeMin().datum;

                /* If you want to do jumpbridge routing options here would be the place to do it */

                foreach (SolarSystem AdjacentID in StaticAdjacencies[Current.SolarSystem])
                {
                    Vertex AdjacentSystem = SystemsList[AdjacentID];

                    float oldcost = SystemsList[AdjacentID].Cost;
                    float RangeToNextSystem = (float)Current.SolarSystem.GetJumpDistance(AdjacentID);
                    if (RangeToNextSystem > PilotMaxRange)
                        continue;

                    CostWrapper NextJump = GetCost(Current.SolarSystem, AdjacentID, RangeToNextSystem);

                    if (Current.Cost + NextJump.Cost < AdjacentSystem.Cost ||
                        (Current.Cost + NextJump.Cost == AdjacentSystem.Cost &&
                         Current.LYTraveled + RangeToNextSystem < AdjacentSystem.LYTraveled))
                    {
                        /* Update the algorithm variables */
                        AdjacentSystem.Cost = Current.Cost + NextJump.Cost;
                        AdjacentSystem.LYTraveled = Current.LYTraveled + RangeToNextSystem;
                        AdjacentSystem.Path = Current.SolarSystem;

                        if (NextJump.JmpType == PlugInData.JumpType.Bridge)
                            AdjacentSystem.LOCost = NextJump.FuelCost;
                        else
                            AdjacentSystem.FuelCost = NextJump.FuelCost;

                        AdjacentSystem.JumpTyp = NextJump.JmpType;
                        AdjacentSystem.Bridge = NextJump.Brige;

                        if (oldcost == float.MaxValue)
                            NodeList[AdjacentID] = SystemPriorityQueue.insert(AdjacentSystem, AdjacentSystem.Cost);
                        else
                            SystemPriorityQueue.decreaseKey(NodeList[AdjacentID], AdjacentSystem.Cost);
                    }
                }

                //Processed a system, raise an event.
                SystemProcessed(null, null);
            }
            //Once Dijkstras finds the path, start at the end and trace backwards

            var SystemsRoute = new List<Vertex>();
            Vertex BackTrack = SystemsList[End].Copy();

            while (BackTrack.SolarSystem != Start)
            {
                SystemsRoute.Insert(0, BackTrack);
                BackTrack = SystemsList[BackTrack.Path].Copy();
            }

            return SystemsRoute;
        }

        public List<Vertex> GetRoute(Route JumpRoute)
        {
            var completeRoute = new List<Vertex>();

            RouteParameters = JumpRoute;
            RouteParameters.WayPoints.Insert(0, JumpRoute.Start);
            RouteParameters.WayPoints.Add(JumpRoute.Dest);
            PilotMaxRange = (float)JumpRoute.ShipJumpRange;

            //BuildGraph(JumpRoute.ShipJumpRange);

            for (int i = 0; i < RouteParameters.WayPoints.Count - 1; i++)
            {
                ResetGraph();
                completeRoute.AddRange(GetShortestPath(RouteParameters.WayPoints[i], RouteParameters.WayPoints[i + 1]));
            }

            //ResetGraph();
            //completeRoute.AddRange(GetShortestPath(JumpRoute.Start, JumpRoute.Dest));
            completeRoute.Insert(0, SystemsList[RouteParameters.Start]);

            return completeRoute;
        }

        //Build the List of adjacencies that describes the graph/universe
        public static Dictionary<SolarSystem, List<SolarSystem>> BuildGraph(double JR)
        {
            StaticAdjacencies = new Dictionary<SolarSystem, List<SolarSystem>>();

            foreach (Region r in PlugInData.GalMap.GalData.Regions.Values)
            {
                foreach (Constellation cc in r.Constellations.Values)
                {
                    foreach (SolarSystem ss in cc.Systems.Values)
                    {
                        StaticAdjacencies.Add(ss, new List<SolarSystem>());
                        StaticAdjacencies[ss].AddRange(PlugInData.GalMap.GetSystemListCanJumpTo(ss, JR));
                        //foreach (int AdjConstID in cc.AdjConstJump)
                        //{
                        //    Constellation nextConst = PlugInData.GalMap.GetConstByID(AdjConstID);
                        //    foreach (SolarSystem pC in nextConst.Systems.Values)
                        //    {
                        //        if (pC.Security > MAXIMUM_JUMP_SEC)
                        //            continue;
                        //        if (ss.GetJumpDistance(pC) <= MAXIMUM_JUMP_DISTANCE)
                        //            StaticAdjacencies[ss].Add(pC);
                        //    }
                        //}
                    }
                }
            }
            return StaticAdjacencies;
        }

        public static Dictionary<SolarSystem, List<SolarSystem>> BuildGraph()
        {
            StaticAdjacencies = new Dictionary<SolarSystem, List<SolarSystem>>();

            foreach (Region r in PlugInData.GalMap.GalData.Regions.Values)
            {
                foreach (Constellation cc in r.Constellations.Values)
                {
                    foreach (SolarSystem ss in cc.Systems.Values)
                    {
                        StaticAdjacencies.Add(ss, new List<SolarSystem>());
                        StaticAdjacencies[ss].AddRange(PlugInData.GalMap.GetSystemListCanJumpTo(ss, MAXIMUM_JUMP_DISTANCE));
                        //foreach (int AdjConstID in cc.AdjConstJump)
                        //{
                        //    Constellation nextConst = PlugInData.GalMap.GetConstByID(AdjConstID);
                        //    foreach (SolarSystem pC in nextConst.Systems.Values)
                        //    {
                        //        if (pC.Security > MAXIMUM_JUMP_SEC)
                        //            continue;
                        //        if (ss.GetJumpDistance(pC) < MAXIMUM_JUMP_DISTANCE)
                        //            StaticAdjacencies[ss].Add(pC);
                        //    }
                        //}
                    }
                }
            }
            return StaticAdjacencies;
        }

        private static CostWrapper GetCost(SolarSystem fromSystem, SolarSystem toSystem, float Distance)
        {
            //bool WayPoint = false;
            bool CynoJam = false;
            CostWrapper EdgeCost = new CostWrapper();
            JumpBridge JB;
            bool Safe = false;
            DataRow[] sr;

            /* Start with the basic normal cyno and update it if we can find an edge with lower weight */
            EdgeCost.Cost = PlugInData.Config.Weights["Jump Default"];
            EdgeCost.FuelCost = Convert.ToInt32(Ship.GetFuel(Distance, RouteParameters, 0));
            EdgeCost.JmpType = PlugInData.JumpType.Cyno;

            /* System on Avoid Listing */
            if (RouteParameters.AvoidSystems.Count > 0 && RouteParameters.AvoidSystems.ContainsKey(toSystem))
            {
                EdgeCost.Cost = (10 - PlugInData.Config.Weights["Jump High"]) * 100000;
                EdgeCost.JmpType = PlugInData.JumpType.Undefined;
                return EdgeCost;
            }

            /* Cyno Generator */
            if (RouteParameters.UseCynoBeacon && PlugInData.CynoGenJamList.ContainsKey(toSystem.ID) && !PlugInData.CynoGenJamList[toSystem.ID].IsJammer)
            {
                /* The system we have been asked to consider has a beacon */
                //if (!WayPoint)
                EdgeCost.Cost = 10 - PlugInData.Config.Weights["Jump Cyno"];

                EdgeCost.JmpType = PlugInData.JumpType.Beacon;
                EdgeCost.Beacon = PlugInData.CynoGenJamList[toSystem.ID];
                EdgeCost.FuelCost = Convert.ToInt32(Ship.GetFuel(Distance, RouteParameters, 0));
            }
            else if (PlugInData.CynoGenJamList.ContainsKey(toSystem.ID) && PlugInData.CynoGenJamList[toSystem.ID].IsJammer)
            {
                /* Cynojammer - um, kinda makes a cyno jump impossible */
                // This will override a waypoint - period
                EdgeCost.Cost = 10000000;   // Huge - want to exlore other options, but report undefined if cannot find another way
                EdgeCost.JmpType = PlugInData.JumpType.Undefined;
                EdgeCost.FuelCost = 0;
                CynoJam = true;
            }

            /* Bridges go here - if there is a JB in this system keyed on toSystem */
            // If the system is Cyno Jammed, Always allow use of a known Jump Bridge for Entry
            if (RouteParameters.UseJumpBridge || CynoJam)
            {
                JB = PlugInData.DoesBridgeLinkExist(fromSystem.ID, toSystem.ID);
                if (JB != null)
                {
                    //if (!WayPoint)
                    EdgeCost.Cost = 10 - PlugInData.Config.Weights["Jump Bridge"];

                    EdgeCost.JmpType = PlugInData.JumpType.Bridge;
                    EdgeCost.Brige = new JumpBridge(JB);
                    EdgeCost.FuelCost = Convert.ToInt32(Ship.GetFuel(Distance, RouteParameters, 1));
                }
            }

            sr = PlugInData.SysD.SystemTable.Select("SID=" + toSystem.ID + " AND Safe=True");
            if (sr.Length > 0)
                Safe = true;
            else
            {
                sr = PlugInData.SysD.MoonTable.Select("SID=" + toSystem.ID + " AND Safe=True");
                if (sr.Length > 0)
                    Safe = true;
            }

            /* Cyno Safe Tower Preference */
            if (RouteParameters.UseCynoSafeTwr && Safe)
            {
                //if (!WayPoint)
                    EdgeCost.Cost = 10 - PlugInData.Config.Weights["Jump Tower"];

                EdgeCost.JmpType = PlugInData.JumpType.CynoSafe;
                EdgeCost.FuelCost = Convert.ToInt32(Ship.GetFuel(Distance, RouteParameters, 0));
            }

            /* Station System Preference */
            if (((10 - PlugInData.Config.Weights["Jump Station"]) < EdgeCost.Cost) && RouteParameters.PreferStation && ((toSystem.Stations.Count > 0) || ((toSystem.ConqStations != null) && (toSystem.ConqStations.Count > 0))))
            {
                //if (!WayPoint)
                EdgeCost.Cost = 10 - PlugInData.Config.Weights["Jump Station"];

                EdgeCost.JmpType = PlugInData.JumpType.Cyno;
                EdgeCost.FuelCost = Convert.ToInt32(Ship.GetFuel(Distance, RouteParameters, 0));
            }

            return EdgeCost;
        }

        public ArrayList GetShortestPathAndTable(SolarSystem Start, SolarSystem End)
        {
            Vertex Current;
            ArrayList RetVal = new ArrayList();
            SystemsList[Start].Cost = 0;
            SystemsList[Start].LYTraveled = 0;

            SystemPriorityQueue.insert(SystemsList[Start], SystemsList[Start].Cost);

            while (!SystemPriorityQueue.isEmpty())
            {
                Current = SystemPriorityQueue.removeMin().datum;

                /* If you want to do jumpbridge routing options here would be the place to do it */

                foreach (SolarSystem AdjacentID in StaticAdjacencies[Current.SolarSystem])
                {
                    Vertex AdjacentSystem = SystemsList[AdjacentID];

                    float oldcost = SystemsList[AdjacentID].Cost;
                    float RangeToNextSystem = (float)Current.SolarSystem.GetJumpDistance(AdjacentID);
                    if (RangeToNextSystem > PilotMaxRange)
                        continue;

                    CostWrapper NextJump = GetCost(Current.SolarSystem, AdjacentID, RangeToNextSystem);

                    if (Current.Cost + NextJump.Cost < AdjacentSystem.Cost ||
                        (Current.Cost + NextJump.Cost == AdjacentSystem.Cost &&
                         Current.LYTraveled + RangeToNextSystem < AdjacentSystem.LYTraveled))
                    {
                        /* Update the algorithm variables */
                        AdjacentSystem.Cost = Current.Cost + NextJump.Cost;
                        AdjacentSystem.LYTraveled = Current.LYTraveled + RangeToNextSystem;
                        AdjacentSystem.Path = Current.SolarSystem;

                        if (NextJump.JmpType == PlugInData.JumpType.Bridge)
                            AdjacentSystem.LOCost = NextJump.FuelCost;
                        else
                            AdjacentSystem.FuelCost = NextJump.FuelCost;

                        AdjacentSystem.JumpTyp = NextJump.JmpType;

                        if (oldcost == float.MaxValue)
                            NodeList[AdjacentID] = SystemPriorityQueue.insert(AdjacentSystem, AdjacentSystem.Cost);
                        else
                            SystemPriorityQueue.decreaseKey(NodeList[AdjacentID], AdjacentSystem.Cost);
                    }
                }

                //Processed a system, raise an event.
                SystemProcessed(null, null);
            }

            //Once Dijkstras finds the path, start at the end and trace backwards
            var SystemsRoute = new List<Vertex>();
            Vertex BackTrack = SystemsList[End].Copy();

            while (BackTrack.SolarSystem != Start)
            {
                SystemsRoute.Insert(0, BackTrack);
                BackTrack = SystemsList[BackTrack.Path].Copy();
            }

            RetVal.Add(SystemsRoute);
            RetVal.Add(SystemsList);

            return RetVal;
        }

        public ArrayList GetRouteAndTable(Route JumpRoute)
        {
            ArrayList RetVal = new ArrayList();
            ArrayList Shortest = new ArrayList();
            RouteParameters = JumpRoute;
            RouteParameters.WayPoints.Insert(0, JumpRoute.Start);
            RouteParameters.WayPoints.Add(JumpRoute.Dest);
            PilotMaxRange = (float)JumpRoute.ShipJumpRange;

            var completeRoute = new List<Vertex>();
            for (int i = 0; i < RouteParameters.WayPoints.Count - 1; i++)
            {
                ResetGraph();
                Shortest = GetShortestPathAndTable(RouteParameters.WayPoints[i], RouteParameters.WayPoints[i + 1]);
                completeRoute.AddRange((List<Vertex>)Shortest[0]);
                RetVal.Add((Dictionary<SolarSystem, Vertex>)Shortest[1]);
            }

            completeRoute.Insert(0, SystemsList[RouteParameters.Start]);
            RetVal.Add(completeRoute);

            return RetVal;
        }


        public List<Vertex> GetIGBRoute(Route JumpRoute)
        {
            var completeRoute = new List<Vertex>();

            RouteParameters = JumpRoute;
            RouteParameters.WayPoints.Insert(0, JumpRoute.Start);
            RouteParameters.WayPoints.Add(JumpRoute.Dest);
            PilotMaxRange = (float)JumpRoute.ShipJumpRange;

            //BuildGraph(JumpRoute.ShipJumpRange);

            for (int i = 0; i < RouteParameters.WayPoints.Count - 1; i++)
            {
                ResetGraph();
                completeRoute.AddRange(GetIGBShortestPath(RouteParameters.WayPoints[i], RouteParameters.WayPoints[i + 1]));
            }

            //ResetGraph();
            //completeRoute.AddRange(GetShortestPath(JumpRoute.Start, JumpRoute.Dest));
            completeRoute.Insert(0, SystemsList[RouteParameters.Start]);

            return completeRoute;
        }

        public List<Vertex> GetIGBShortestPath(SolarSystem Start, SolarSystem End)
        {
            Vertex Current;
            SystemsList[Start].Cost = 0;
            SystemsList[Start].LYTraveled = 0;

            SystemPriorityQueue.insert(SystemsList[Start], SystemsList[Start].Cost);

            while (!SystemPriorityQueue.isEmpty())
            {
                Current = SystemPriorityQueue.removeMin().datum;

                /* If you want to do jumpbridge routing options here would be the place to do it */

                foreach (SolarSystem AdjacentID in StaticAdjacencies[Current.SolarSystem])
                {
                    Vertex AdjacentSystem = SystemsList[AdjacentID];

                    float oldcost = SystemsList[AdjacentID].Cost;
                    float RangeToNextSystem = (float)Current.SolarSystem.GetJumpDistance(AdjacentID);
                    if (RangeToNextSystem > PilotMaxRange)
                        continue;

                    CostWrapper NextJump = GetCost(Current.SolarSystem, AdjacentID, RangeToNextSystem);

                    if (Current.Cost + NextJump.Cost < AdjacentSystem.Cost ||
                        (Current.Cost + NextJump.Cost == AdjacentSystem.Cost &&
                         Current.LYTraveled + RangeToNextSystem < AdjacentSystem.LYTraveled))
                    {
                        /* Update the algorithm variables */
                        AdjacentSystem.Cost = Current.Cost + NextJump.Cost;
                        AdjacentSystem.LYTraveled = Current.LYTraveled + RangeToNextSystem;
                        AdjacentSystem.Path = Current.SolarSystem;

                        if (NextJump.JmpType == PlugInData.JumpType.Bridge)
                            AdjacentSystem.LOCost = NextJump.FuelCost;
                        else
                            AdjacentSystem.FuelCost = NextJump.FuelCost;

                        AdjacentSystem.JumpTyp = NextJump.JmpType;
                        AdjacentSystem.Bridge = NextJump.Brige;

                        if (oldcost == float.MaxValue)
                            NodeList[AdjacentID] = SystemPriorityQueue.insert(AdjacentSystem, AdjacentSystem.Cost);
                        else
                            SystemPriorityQueue.decreaseKey(NodeList[AdjacentID], AdjacentSystem.Cost);
                    }
                }
            }
            //Once Dijkstras finds the path, start at the end and trace backwards

            var SystemsRoute = new List<Vertex>();
            Vertex BackTrack = SystemsList[End].Copy();

            while (BackTrack.SolarSystem != Start)
            {
                SystemsRoute.Insert(0, BackTrack);
                BackTrack = SystemsList[BackTrack.Path].Copy();
            }

            return SystemsRoute;
        }


    }
}
