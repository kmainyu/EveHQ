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
    public class GateRoute
    {
        #region Delegates

        public delegate void SystemProcessedHandler(object sender, ProgressChangedEventArgs e);

        #endregion

        private static readonly Dictionary<int, List<Vertex>> BridgeList =
            new Dictionary<int, List<Vertex>>();

        private static Route RouteParameters;
        
        public static Dictionary<SolarSystem, List<SolarSystem>> StaticAdjacencies; //This list gets serialized 

        private readonly Dictionary<SolarSystem, FibonacciHeap<Vertex>.Node> NodeList =
            new Dictionary<SolarSystem, FibonacciHeap<Vertex>.Node>();

        private readonly FibonacciHeap<Vertex> SystemPriorityQueue = new FibonacciHeap<Vertex>();

        // Master list of ALL the SolarSystems as represented by Vertices
        private readonly Dictionary<SolarSystem, Vertex> VertexList =
            new Dictionary<SolarSystem, Vertex>(Convert.ToInt32(PlugInData.GalMap.GalData.TotalSystems));

        public event SystemProcessedHandler SystemProcessed;

        private void ResetGraph()
        {
            foreach (var v in VertexList.Values)
            {
                v.Cost = float.MaxValue;
                //v.LYTraveled = float.MaxValue;
            }
            NodeList.Clear();
            SystemPriorityQueue.clear();
        }

        private void BuildGraph()
        {
            JumpBridge rjb;

            VertexList.Clear();
            NodeList.Clear();
            SystemPriorityQueue.clear();

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
                        newVert.Adjacencies = new List<Vertex>();
                        newVert.JumpTyp = PlugInData.JumpType.Gate;

                        VertexList[ss] = newVert;
                    }
                }
            }

            foreach (StarGate gate in PlugInData.GalMap.GalData.StarGates)
            {
                if (!VertexList[gate.From].Adjacencies.Contains(VertexList[gate.To]))
                {
                    VertexList[gate.From].Adjacencies.Add(VertexList[gate.To]);
                    VertexList[gate.To].Adjacencies.Add(VertexList[gate.From]);
                }
            }

            /* Split the bi-directionaly JumpBridge type into two one-way JumpBridgeEdges for bridge routing */
            BridgeList.Clear();
            foreach (var jbl in PlugInData.JumpBridgeList.Values)
            {
                foreach (var jb in jbl.Values)
                {
                    if (!BridgeList.ContainsKey(jb.From.ID))
                        BridgeList[jb.From.ID] = new List<Vertex>();
                    if (!BridgeList.ContainsKey(jb.To.ID))
                        BridgeList[jb.To.ID] = new List<Vertex>();

                    Vertex fJump = new Vertex
                    {
                        SolarSystem = jb.From,
                        Path = jb.To,
                        Bridge = jb,
                        Cost = 0,
                        JumpTyp = PlugInData.JumpType.Bridge
                    };

                    rjb = new JumpBridge(jb.To, jb.From, jb.ToMoon, jb.FromMoon, jb.Password, jb.AllianceName, jb.BridgeLineColor, jb.Distance, jb.Online);
                    Vertex bJump = new Vertex
                    {
                        SolarSystem = jb.To,
                        Path = jb.From,
                        Bridge = rjb,
                        Cost = 0,
                        JumpTyp = PlugInData.JumpType.Bridge
                    };

                    BridgeList[jb.From.ID].Add(fJump);
                    BridgeList[jb.To.ID].Add(bJump);
                }
            }
        }

        public List<Vertex> GetRoute(Route GateRoute)
        {
            List<Vertex> CompleteRoute = new List<Vertex>();

            RouteParameters = GateRoute;
            RouteParameters.WayPoints.Insert(0, GateRoute.Start);
            RouteParameters.WayPoints.Add(GateRoute.Dest);

            BuildGraph();

            for (int i = 0; i < RouteParameters.WayPoints.Count - 1; i++)
            {
                ResetGraph();
                CompleteRoute.AddRange(GetShortestPath(RouteParameters.WayPoints[i], RouteParameters.WayPoints[i + 1]));
            }

            CompleteRoute.Insert(0, VertexList[RouteParameters.Start]);

            return CompleteRoute;
        }

        private List<Vertex> GetShortestPath(SolarSystem start, SolarSystem end)
        {
            Vertex current;

            VertexList[start].Cost = 0;
            SystemPriorityQueue.insert(VertexList[start], VertexList[start].Cost);

            while (!SystemPriorityQueue.isEmpty())
            {
                /* Dequeue the next lowest cost system to consider. */
                current = SystemPriorityQueue.removeMin().datum;

                /* If the current system has bridges out of it, check them */
                if (RouteParameters.UseJumpBridge &&
                    BridgeList.ContainsKey(current.SolarSystem.ID) &&
                    BridgeList[current.SolarSystem.ID] != null)
                {
                    foreach (Vertex adjacent in BridgeList[current.SolarSystem.ID])
                    {
                        Vertex neighbor = VertexList[adjacent.Bridge.To];
                        float oldcost = neighbor.Cost;
                        if (current.Cost + (10 - PlugInData.Config.Weights["Gate Bridge"]) < neighbor.Cost)
                        {
                            neighbor.Cost = current.Cost + (10 - PlugInData.Config.Weights["Gate Bridge"]);
                            neighbor.Path = current.SolarSystem;

                            neighbor.LOCost = (int)Ship.GetFuel(adjacent.Bridge.From.GetJumpDistance(adjacent.Bridge.To), RouteParameters, 1);
                            neighbor.JumpTyp = PlugInData.JumpType.Bridge;
                            neighbor.Bridge = adjacent.Bridge;

                            if (oldcost == float.MaxValue)
                                NodeList[neighbor.SolarSystem] = SystemPriorityQueue.insert(neighbor, neighbor.Cost);

                            else
                            {
                                SystemPriorityQueue.decreaseKey(NodeList[neighbor.SolarSystem],
                                                                 current.Cost + (10 - PlugInData.Config.Weights["Gate Bridge"]));
                            }
                        }
                    }
                }

                /* Check regular gate connections here */
                foreach (Vertex adjacent in current.Adjacencies)
                {
                    float oldcost = adjacent.Cost;
                    CostWrapper nextJump = GetCost(adjacent.SolarSystem);
                    if (current.Cost + nextJump.Cost < adjacent.Cost)
                    {
                        adjacent.Cost = current.Cost + nextJump.Cost;
                        adjacent.Path = current.SolarSystem;
                        adjacent.LOCost = 0;

                        adjacent.JumpTyp = nextJump.JmpType;

                        if (oldcost == float.MaxValue)
                            NodeList[adjacent.SolarSystem] = SystemPriorityQueue.insert(adjacent, adjacent.Cost);
                        else
                        {
                            SystemPriorityQueue.decreaseKey(NodeList[adjacent.SolarSystem],
                                                             current.Cost + nextJump.Cost);
                        }
                    }
                }

                //Processed a system, raise an event.
                SystemProcessed(null, null);
            }

            var CompleteRoute = new List<Vertex>();

            //Make copies of the vertex since the vertices values will be reset if waypoints are used.
            //Subsequently screwing up all the references the verticies had about the previous path(s).
            Vertex backTrack = VertexList[end].Copy();
            while (backTrack.SolarSystem != start)
            {
                //adding items to the front since were working backwards
                CompleteRoute.Insert(0, backTrack);
                backTrack = VertexList[backTrack.Path].Copy();
            }

            return CompleteRoute;
        }

        private CostWrapper GetCost(SolarSystem toSystemId)
        {
            SolarSystem toSystem = toSystemId;

            CostWrapper edgeCost = new CostWrapper { Cost = 10 - PlugInData.Config.Weights["Gate Default"], JmpType = PlugInData.JumpType.Gate };

            /* If we are considering a system thats on the avoid list */
            foreach (var systemToAvoid in RouteParameters.AvoidSystems)
            {
                if (toSystem.ID == systemToAvoid.Key.ID)
                {
                    edgeCost.Cost = (10 - PlugInData.Config.Weights["Gate High"]) * 1000;
                    return edgeCost;
                }
            }

            /* Checking security status */
            if (toSystem.Security < RouteParameters.minSec)
                edgeCost.Cost = (10 - PlugInData.Config.Weights["Gate High"]) * 1000;

            if (toSystem.Security > RouteParameters.maxSec)
                edgeCost.Cost = (10 - PlugInData.Config.Weights["Gate High"]) * 1000;

            return edgeCost;
        }

        public Dictionary<SolarSystem, Vertex> GetRouteTable(Route GateRoute)
        {
            RouteParameters = GateRoute;
            RouteParameters.WayPoints.Insert(0, GateRoute.Start);

            BuildGraph();

            return GetVertexListForSearch(GateRoute.Start);
        }

        private Dictionary<SolarSystem, Vertex> GetVertexListForSearch(SolarSystem start)
        {
            Vertex current;

            VertexList[start].Cost = 0;
            SystemPriorityQueue.insert(VertexList[start], VertexList[start].Cost);

            while (!SystemPriorityQueue.isEmpty())
            {
                /* Dequeue the next lowest cost system to consider. */
                current = SystemPriorityQueue.removeMin().datum;

                /* If the current system has bridges out of it, check them */
                if (RouteParameters.UseJumpBridge &&
                    BridgeList.ContainsKey(current.SolarSystem.ID) &&
                    BridgeList[current.SolarSystem.ID] != null)
                {
                    foreach (Vertex adjacent in BridgeList[current.SolarSystem.ID])
                    {
                        Vertex neighbor = VertexList[adjacent.Bridge.To];
                        float oldcost = neighbor.Cost;
                        if (current.Cost + (10 - PlugInData.Config.Weights["Gate Bridge"]) < neighbor.Cost)
                        {
                            neighbor.Cost = current.Cost + (10 - PlugInData.Config.Weights["Gate Bridge"]);
                            neighbor.Path = current.SolarSystem;

                            neighbor.LOCost = (int)Ship.GetFuel(adjacent.Bridge.From.GetJumpDistance(adjacent.Bridge.To), RouteParameters, 1);
                            neighbor.JumpTyp = PlugInData.JumpType.Bridge;
                            neighbor.Bridge = adjacent.Bridge;

                            if (oldcost == float.MaxValue)
                                NodeList[neighbor.SolarSystem] = SystemPriorityQueue.insert(neighbor, neighbor.Cost);

                            else
                            {
                                SystemPriorityQueue.decreaseKey(NodeList[neighbor.SolarSystem],
                                                                 current.Cost + (10 - PlugInData.Config.Weights["Gate Bridge"]));
                            }
                        }
                    }
                }

                /* Check regular gate connections here */
                foreach (Vertex adjacent in current.Adjacencies)
                {
                    float oldcost = adjacent.Cost;
                    CostWrapper nextJump = GetCost(adjacent.SolarSystem);
                    if (current.Cost + nextJump.Cost < adjacent.Cost)
                    {
                        adjacent.Cost = current.Cost + nextJump.Cost;
                        adjacent.Path = current.SolarSystem;
                        adjacent.LOCost = 0;

                        adjacent.JumpTyp = nextJump.JmpType;

                        if (oldcost == float.MaxValue)
                            NodeList[adjacent.SolarSystem] = SystemPriorityQueue.insert(adjacent, adjacent.Cost);
                        else
                        {
                            SystemPriorityQueue.decreaseKey(NodeList[adjacent.SolarSystem],
                                                             current.Cost + nextJump.Cost);
                        }
                    }
                }

                //Processed a system, raise an event.
                //SystemProcessed(null, null);
            }

            return VertexList;
        }


        public List<Vertex> GetIGBRoute(Route GateRoute)
        {
            List<Vertex> CompleteRoute = new List<Vertex>();

            RouteParameters = GateRoute;
            RouteParameters.WayPoints.Insert(0, GateRoute.Start);
            RouteParameters.WayPoints.Add(GateRoute.Dest);

            BuildGraph();

            for (int i = 0; i < RouteParameters.WayPoints.Count - 1; i++)
            {
                ResetGraph();
                CompleteRoute.AddRange(GetIGBShortestPath(RouteParameters.WayPoints[i], RouteParameters.WayPoints[i + 1]));
            }

            CompleteRoute.Insert(0, VertexList[RouteParameters.Start]);

            return CompleteRoute;
        }

        private List<Vertex> GetIGBShortestPath(SolarSystem start, SolarSystem end)
        {
            Vertex current;

            VertexList[start].Cost = 0;
            SystemPriorityQueue.insert(VertexList[start], VertexList[start].Cost);

            while (!SystemPriorityQueue.isEmpty())
            {
                /* Dequeue the next lowest cost system to consider. */
                current = SystemPriorityQueue.removeMin().datum;

                /* If the current system has bridges out of it, check them */
                if (RouteParameters.UseJumpBridge &&
                    BridgeList.ContainsKey(current.SolarSystem.ID) &&
                    BridgeList[current.SolarSystem.ID] != null)
                {
                    foreach (Vertex adjacent in BridgeList[current.SolarSystem.ID])
                    {
                        Vertex neighbor = VertexList[adjacent.Bridge.To];
                        float oldcost = neighbor.Cost;
                        if (current.Cost + (10 - PlugInData.Config.Weights["Gate Bridge"]) < neighbor.Cost)
                        {
                            neighbor.Cost = current.Cost + (10 - PlugInData.Config.Weights["Gate Bridge"]);
                            neighbor.Path = current.SolarSystem;

                            neighbor.LOCost = (int)Ship.GetFuel(adjacent.Bridge.From.GetJumpDistance(adjacent.Bridge.To), RouteParameters, 1);
                            neighbor.JumpTyp = PlugInData.JumpType.Bridge;
                            neighbor.Bridge = adjacent.Bridge;

                            if (oldcost == float.MaxValue)
                                NodeList[neighbor.SolarSystem] = SystemPriorityQueue.insert(neighbor, neighbor.Cost);

                            else
                            {
                                SystemPriorityQueue.decreaseKey(NodeList[neighbor.SolarSystem],
                                                                 current.Cost + (10 - PlugInData.Config.Weights["Gate Bridge"]));
                            }
                        }
                    }
                }

                /* Check regular gate connections here */
                foreach (Vertex adjacent in current.Adjacencies)
                {
                    float oldcost = adjacent.Cost;
                    CostWrapper nextJump = GetCost(adjacent.SolarSystem);
                    if (current.Cost + nextJump.Cost < adjacent.Cost)
                    {
                        adjacent.Cost = current.Cost + nextJump.Cost;
                        adjacent.Path = current.SolarSystem;
                        adjacent.LOCost = 0;

                        adjacent.JumpTyp = nextJump.JmpType;

                        if (oldcost == float.MaxValue)
                            NodeList[adjacent.SolarSystem] = SystemPriorityQueue.insert(adjacent, adjacent.Cost);
                        else
                        {
                            SystemPriorityQueue.decreaseKey(NodeList[adjacent.SolarSystem],
                                                             current.Cost + nextJump.Cost);
                        }
                    }
                }
            }

            var CompleteRoute = new List<Vertex>();

            //Make copies of the vertex since the vertices values will be reset if waypoints are used.
            //Subsequently screwing up all the references the verticies had about the previous path(s).
            Vertex backTrack = VertexList[end].Copy();
            while (backTrack.SolarSystem != start)
            {
                //adding items to the front since were working backwards
                CompleteRoute.Insert(0, backTrack);
                backTrack = VertexList[backTrack.Path].Copy();
            }

            return CompleteRoute;
        }


    }

}
