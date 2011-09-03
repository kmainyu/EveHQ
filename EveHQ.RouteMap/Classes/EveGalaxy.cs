// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2011  EveHQ Development Team
// 
// This file is part of EveHQ.
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
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

namespace EveHQ.RouteMap
{
    [Serializable]
    public class EveGalaxy
    {
        private const double LIGHT_YEAR = 9.4605284E+15;
        private const double LY_DIV = 9.4605284E+15 / 175;
        private const double CEL_DIV = 10000000;
        public Galaxy GalData;
        public Rectangle FlatArea;
        public List<int> locAgent = new List<int>();
        //public UserData User;

        public enum RelativePosition
        {
            Top,
            TopLeft,
            TopRight,
            CenterLeft,
            CenterRight,
            Bottom,
            BottomLeft,
            BottomRight,
            Center,
            None
        };

        private static readonly int[] SkipRegions = new[]
                                                        {
                                                            10000004, 10000017, 10000019 //,
                                                            //11000001, 11000002, 11000003, 11000004,
                                                            //11000005, 11000006, 11000007, 11000008,
                                                            //11000009, 11000010, 11000011, 11000012,
                                                            //11000013, 11000014, 11000015, 11000016,
                                                            //11000017, 11000018, 11000019, 11000020,
                                                            //11000021, 11000022, 11000023, 11000024,
                                                            //11000025, 11000026, 11000027, 11000028,
                                                            //11000029, 11000030
                                                        };
        
        public EveGalaxy()
        {
            GalData = new Galaxy();
            FlatArea = new Rectangle();
        }

        public void FillFlatBoundaries()
        {
            Point FlatMin = new Point(int.MaxValue, int.MaxValue);
            Point FlatMax = new Point(int.MinValue, int.MinValue);

            foreach (var curRegion in GalData.Regions)
            {
                curRegion.Value.FillFlatBoundaries();

                if (curRegion.Value.FlatArea.Left < FlatMin.X) FlatMin.X = curRegion.Value.FlatArea.Left;
                if (curRegion.Value.FlatArea.Right > FlatMax.X) FlatMax.X = curRegion.Value.FlatArea.Right;
                if (curRegion.Value.FlatArea.Top < FlatMin.Y) FlatMin.Y = curRegion.Value.FlatArea.Top;
                if (curRegion.Value.FlatArea.Bottom > FlatMax.Y) FlatMax.Y = curRegion.Value.FlatArea.Bottom;
            }

            FlatArea = Rectangle.FromLTRB(FlatMin.X, FlatMin.Y, FlatMax.X, FlatMax.Y);
        }

        public SolarSystem GetSystemByName(string name)
        {
            SolarSystem system = new SolarSystem();

            if (string.IsNullOrEmpty(name))
                return null;

            foreach (var rgn in GalData.Regions)
            {
                foreach (var cns in rgn.Value.Constellations)
                {
                    foreach (var sys in cns.Value.Systems)
                    {
                        if (name.ToLower() == sys.Value.Name.ToLower())
                            system = sys.Value;
                    }
                }
            }
            return system;
        }

        public SolarSystem GetSystemByID(int ID)
        {
            SolarSystem system = new SolarSystem();

            foreach (var rgn in GalData.Regions)
            {
                foreach (var cns in rgn.Value.Constellations)
                {
                    foreach (var sys in cns.Value.Systems)
                    {
                        if (ID == sys.Value.ID)
                            system = sys.Value;
                    }
                }
            }
            return system;
        }

        public Constellation GetConstByID(int ID)
        {
            Constellation cnst = new Constellation();

            foreach (var rgn in GalData.Regions)
            {
                foreach (var cns in rgn.Value.Constellations)
                {
                    if (ID == cns.Value.ID)
                        cnst = cns.Value;
                }
            }
            return cnst;
        }

        public void BuildRegions()
        {
            string strSQL;
            Region nr;
            DataSet regnData;
            int rgnID = 0;
            int count = 0;
            List<Color> Rcolors;

            Rcolors = GenerateRandomizedColors();

            strSQL = "SELECT * FROM mapRegions;";
            regnData = EveHQ.Core.DataFunctions.GetData(strSQL);

            try
            {
                if (!regnData.Equals(System.DBNull.Value))
                {
                    if (regnData.Tables[0].Rows.Count > 0)
                    {
                        //GalData.Regions = new Dictionary<int, Region>();
                        foreach (DataRow row in regnData.Tables[0].Rows)
                        {
                            rgnID = Convert.ToInt32(row.ItemArray[0]);

                            if (SkipRegions.Contains(rgnID))
                                continue;

                            nr = new Region();
                            nr.ID = rgnID;
                            nr.Name = row.ItemArray[1].ToString();
                            nr.X = Convert.ToDecimal(row.ItemArray[2]);
                            nr.Y = Convert.ToDecimal(row.ItemArray[3]);
                            nr.Z = Convert.ToDecimal(row.ItemArray[4]);
                            nr.XMin = Convert.ToDecimal(row.ItemArray[5]);
                            nr.XMax = Convert.ToDecimal(row.ItemArray[6]);
                            nr.YMin = Convert.ToDecimal(row.ItemArray[7]);
                            nr.YMax = Convert.ToDecimal(row.ItemArray[8]);
                            nr.ZMin = Convert.ToDecimal(row.ItemArray[9]);
                            nr.ZMax = Convert.ToDecimal(row.ItemArray[10]);

                            if (row.ItemArray[11].Equals(System.DBNull.Value))
                                nr.FactID = 0;
                            else
                                nr.FactID = Convert.ToInt32(row.ItemArray[11]);

                            nr.RColor = Rcolors[count];
                            count++;
                            GalData.Regions.Add(nr.ID, nr);
                        }
                    }
                }
            }
            catch
            {
                DialogResult dr = MessageBox.Show("An Error was encountered while Reading in Region Data.", "RouteMap: DB Error", MessageBoxButtons.OK);
            }
        }

        public void BuildRegion(int regionID)
        {
            string strSQL;
            Region nr;
            DataSet regnData;
            int rgnID = 0;
            int count = 0;
            List<Color> Rcolors;

            Rcolors = GenerateRandomizedColors();

            strSQL = "SELECT * FROM mapRegions WHERE mapRegions.regionID=" + regionID + ";";
            regnData = EveHQ.Core.DataFunctions.GetData(strSQL);

            try
            {
                if (!regnData.Equals(System.DBNull.Value))
                {
                    if (regnData.Tables[0].Rows.Count > 0)
                    {
                        //GalData.Regions = new Dictionary<int, Region>();
                        foreach (DataRow row in regnData.Tables[0].Rows)
                        {
                            rgnID = Convert.ToInt32(row.ItemArray[0]);

                            if (SkipRegions.Contains(rgnID))
                                continue;

                            nr = new Region();
                            nr.ID = rgnID;
                            nr.Name = row.ItemArray[1].ToString();
                            nr.X = Convert.ToDecimal(row.ItemArray[2]);
                            nr.Y = Convert.ToDecimal(row.ItemArray[3]);
                            nr.Z = Convert.ToDecimal(row.ItemArray[4]);
                            nr.XMin = Convert.ToDecimal(row.ItemArray[5]);
                            nr.XMax = Convert.ToDecimal(row.ItemArray[6]);
                            nr.YMin = Convert.ToDecimal(row.ItemArray[7]);
                            nr.YMax = Convert.ToDecimal(row.ItemArray[8]);
                            nr.ZMin = Convert.ToDecimal(row.ItemArray[9]);
                            nr.ZMax = Convert.ToDecimal(row.ItemArray[10]);

                            if (row.ItemArray[11].Equals(System.DBNull.Value))
                                nr.FactID = 0;
                            else
                                nr.FactID = Convert.ToInt32(row.ItemArray[11]);

                            nr.RColor = Rcolors[count];
                            count++;
                            GalData.Regions.Add(nr.ID, nr);
                        }
                    }
                }
            }
            catch
            {
                DialogResult dr = MessageBox.Show("An Error was encountered while Reading in Region Data.", "RouteMap: DB Error", MessageBoxButtons.OK);
            }
        }

        public void BuildConstellations()
        {
            string strSQL;
            Constellation cn;
            DataSet regnData;
            int rgnID = 0;

            strSQL = "SELECT * FROM mapConstellations;";
            regnData = EveHQ.Core.DataFunctions.GetData(strSQL);

            //try
            //{
                if (!regnData.Equals(System.DBNull.Value))
                {
                    if (regnData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in regnData.Tables[0].Rows)
                        {
                            rgnID = Convert.ToInt32(row.ItemArray[0]);

                            if (SkipRegions.Contains(rgnID))
                                continue;

                            cn = new Constellation();
                            cn.RegionID = rgnID;
                            cn.ID = Convert.ToInt32(row.ItemArray[1]);
                            cn.Name = row.ItemArray[2].ToString();
                            cn.X = Convert.ToDecimal(row.ItemArray[3]);
                            cn.Y = Convert.ToDecimal(row.ItemArray[4]);
                            cn.Z = Convert.ToDecimal(row.ItemArray[5]);
                            cn.XMin = Convert.ToDecimal(row.ItemArray[6]);
                            cn.XMax = Convert.ToDecimal(row.ItemArray[7]);
                            cn.YMin = Convert.ToDecimal(row.ItemArray[8]);
                            cn.YMax = Convert.ToDecimal(row.ItemArray[9]);
                            cn.ZMin = Convert.ToDecimal(row.ItemArray[10]);
                            cn.ZMax = Convert.ToDecimal(row.ItemArray[11]);
                            cn.Radius = Convert.ToDecimal(row.ItemArray[13]);

                            if (row.ItemArray[12].Equals(System.DBNull.Value))
                                cn.FactID = 0;
                            else
                                cn.FactID = Convert.ToInt32(row.ItemArray[12]);

                            cn.CColor = GalData.Regions[cn.RegionID].RColor;
                            GalData.Regions[cn.RegionID].Constellations.Add(cn.ID, cn);
                            //GalData.Constellations.Add(cn.ID, cn.CopyConstellation(cn));
                        }
                    }
                }
            //}
            //catch
            //{
            //    DialogResult dr = MessageBox.Show("An Error was encountered while Reading in Constellation Data.", "RouteMap: DB Error", MessageBoxButtons.OK);
            //}
        }

        public void BuildConstellation(int regionID)
        {
            string strSQL;
            Constellation cn;
            DataSet regnData;
            int rgnID = 0;

            strSQL = "SELECT * FROM mapConstellations WHERE mapConstellations.regionID=" + regionID + ";";
            regnData = EveHQ.Core.DataFunctions.GetData(strSQL);

            //try
            //{
                if (!regnData.Equals(System.DBNull.Value))
                {
                    if (regnData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in regnData.Tables[0].Rows)
                        {
                            rgnID = Convert.ToInt32(row.ItemArray[0]);

                            if (SkipRegions.Contains(rgnID))
                                continue;

                            cn = new Constellation();
                            cn.RegionID = rgnID;
                            cn.ID = Convert.ToInt32(row.ItemArray[1]);
                            cn.Name = row.ItemArray[2].ToString();
                            cn.X = Convert.ToDecimal(row.ItemArray[3]);
                            cn.Y = Convert.ToDecimal(row.ItemArray[4]);
                            cn.Z = Convert.ToDecimal(row.ItemArray[5]);
                            cn.XMin = Convert.ToDecimal(row.ItemArray[6]);
                            cn.XMax = Convert.ToDecimal(row.ItemArray[7]);
                            cn.YMin = Convert.ToDecimal(row.ItemArray[8]);
                            cn.YMax = Convert.ToDecimal(row.ItemArray[9]);
                            cn.ZMin = Convert.ToDecimal(row.ItemArray[10]);
                            cn.ZMax = Convert.ToDecimal(row.ItemArray[11]);
                            cn.Radius = Convert.ToDecimal(row.ItemArray[13]);

                            if (row.ItemArray[12].Equals(System.DBNull.Value))
                                cn.FactID = 0;
                            else
                                cn.FactID = Convert.ToInt32(row.ItemArray[12]);

                            cn.CColor = GalData.Regions[cn.RegionID].RColor;
                            GalData.Regions[cn.RegionID].Constellations.Add(cn.ID, cn);
                        }
                    }
                }
            //}
            //catch
            //{
            //    DialogResult dr = MessageBox.Show("An Error was encountered while Reading in Constellation Data.", "RouteMap: DB Error", MessageBoxButtons.OK);
            //}
        }

        public void BuildSolarSystems()
        {
            string strSQL;
            SolarSystem ss;
            DataSet regnData;
            int rgnID = 0;

            strSQL = "SELECT * FROM mapSolarSystems;";
            regnData = EveHQ.Core.DataFunctions.GetData(strSQL);

            //try
            //{
            if (!regnData.Equals(System.DBNull.Value))
            {
                if (regnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in regnData.Tables[0].Rows)
                    {
                        rgnID = Convert.ToInt32(row.ItemArray[0]);

                        if (SkipRegions.Contains(rgnID))
                            continue;

                        ss = new SolarSystem();
                        ss.RegionID = rgnID;
                        ss.ConstID = Convert.ToInt32(row.ItemArray[1]);
                        ss.ID = Convert.ToInt32(row.ItemArray[2]);
                        ss.Name = row.ItemArray[3].ToString();
                        ss.Coords = new PointF_3D(Convert.ToDouble(row.ItemArray[4]), Convert.ToDouble(row.ItemArray[6]), Convert.ToDouble(row.ItemArray[5]));
                        ss.OrgCoord.X = Convert.ToInt32(Math.Round(ss.Coords.X / LY_DIV));
                        ss.OrgCoord.Y = Convert.ToInt32(Math.Round(ss.Coords.Y / LY_DIV));

                        ss.XMin = Convert.ToDecimal(row.ItemArray[7]);
                        ss.XMax = Convert.ToDecimal(row.ItemArray[8]);
                        ss.YMin = Convert.ToDecimal(row.ItemArray[9]);
                        ss.YMax = Convert.ToDecimal(row.ItemArray[10]);
                        ss.ZMin = Convert.ToDecimal(row.ItemArray[11]);
                        ss.ZMax = Convert.ToDecimal(row.ItemArray[12]);
                        ss.Luminosity = Convert.ToDecimal(row.ItemArray[13]);
                        ss.Security = Convert.ToDouble(row.ItemArray[21]);
                        ss.Radius = Convert.ToDecimal(row.ItemArray[23]);
                        ss.SunTypID = Convert.ToDecimal(row.ItemArray[24]);
                        ss.SecClass = row.ItemArray[25].ToString();
                        ss.LabelLocation = SolarSystem.RelativePosition.BottomLeft;

                        if (row.ItemArray[22].Equals(System.DBNull.Value))
                            ss.FactID = 0;
                        else
                            ss.FactID = Convert.ToInt32(row.ItemArray[22]);

                        GalData.Regions[rgnID].Constellations[ss.ConstID].Systems.Add(ss.ID, ss);
                        GalData.TotalSystems++;
                        //ThreadPool.QueueUserWorkItem(new WaitCallback(GetSystemCelestial), (ss.ID.ToString()));
                        GetSystemCelestial(ss.ID, ss.RegionID, ss.ConstID);
                        //ThreadPool.QueueUserWorkItem(new WaitCallback(BuildStations), (ss.ID.ToString()));
                        BuildStations(ss.ID);
                    }
                }
            }
            //}
            //catch
            //{
            //    DialogResult dr = MessageBox.Show("An Error was encountered while Reading in System Data.", "RouteMap: DB Error", MessageBoxButtons.OK);
            //}


        }

        public void BuildSolarSystemsForRegion(int regionID)
        {
            string strSQL;
            SolarSystem ss;
            DataSet regnData;
            int rgnID = 0;

            strSQL = "SELECT * FROM mapSolarSystems WHERE mapSolarSystems.regionID="+regionID+";";
            regnData = EveHQ.Core.DataFunctions.GetData(strSQL);

            //try
            //{
            if (!regnData.Equals(System.DBNull.Value))
            {
                if (regnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in regnData.Tables[0].Rows)
                    {
                        rgnID = Convert.ToInt32(row.ItemArray[0]);

                        if (SkipRegions.Contains(rgnID))
                            continue;

                        ss = new SolarSystem();
                        ss.RegionID = rgnID;
                        ss.ConstID = Convert.ToInt32(row.ItemArray[1]);
                        ss.ID = Convert.ToInt32(row.ItemArray[2]);
                        ss.Name = row.ItemArray[3].ToString();
                        ss.Coords = new PointF_3D(Convert.ToDouble(row.ItemArray[4]), Convert.ToDouble(row.ItemArray[6]), Convert.ToDouble(row.ItemArray[5]));
                        ss.OrgCoord.X = Convert.ToInt32(Math.Round(ss.Coords.X / LY_DIV));
                        ss.OrgCoord.Y = Convert.ToInt32(Math.Round(ss.Coords.Y / LY_DIV));

                        ss.XMin = Convert.ToDecimal(row.ItemArray[7]);
                        ss.XMax = Convert.ToDecimal(row.ItemArray[8]);
                        ss.YMin = Convert.ToDecimal(row.ItemArray[9]);
                        ss.YMax = Convert.ToDecimal(row.ItemArray[10]);
                        ss.ZMin = Convert.ToDecimal(row.ItemArray[11]);
                        ss.ZMax = Convert.ToDecimal(row.ItemArray[12]);
                        ss.Luminosity = Convert.ToDecimal(row.ItemArray[13]);
                        ss.Security = Convert.ToDouble(row.ItemArray[21]);
                        ss.Radius = Convert.ToDecimal(row.ItemArray[23]);
                        ss.SunTypID = Convert.ToDecimal(row.ItemArray[24]);
                        ss.SecClass = row.ItemArray[25].ToString();
                        ss.LabelLocation = SolarSystem.RelativePosition.BottomLeft;

                        if (row.ItemArray[22].Equals(System.DBNull.Value))
                            ss.FactID = 0;
                        else
                            ss.FactID = Convert.ToInt32(row.ItemArray[22]);

                        GalData.Regions[rgnID].Constellations[ss.ConstID].Systems.Add(ss.ID, ss);
                        GalData.TotalSystems++;
                        GetSystemCelestial(ss.ID, ss.RegionID, ss.ConstID);
                        BuildStations(ss.ID);
                    }
                }
            }
            //}
            //catch
            //{
            //    DialogResult dr = MessageBox.Show("An Error was encountered while Reading in System Data.", "RouteMap: DB Error", MessageBoxButtons.OK);
            //}


        }

        public void GetSystemCelestial(int sID, int rID, int cID)
        {
            string strSQL;
            DataSet systData;

            // Planets
            strSQL = "SELECT *";
            strSQL += " FROM mapDenormalize";
            strSQL += " WHERE mapDenormalize.solarSystemID=" + sID + " AND mapDenormalize.groupID = 7;";
            systData = EveHQ.Core.DataFunctions.GetData(strSQL);

            if (!systData.Equals(System.DBNull.Value))
            {
                GalData.Regions[rID].Constellations[cID].Systems[sID].Planets = systData.Tables[0].Rows.Count;
            }

            // Moons
            strSQL = "SELECT *";
            strSQL += " FROM mapDenormalize";
            strSQL += " WHERE mapDenormalize.solarSystemID=" + sID + " AND mapDenormalize.groupID = 8;";
            systData = EveHQ.Core.DataFunctions.GetData(strSQL);

            if (!systData.Equals(System.DBNull.Value))
            {
                GalData.Regions[rID].Constellations[cID].Systems[sID].Moons = systData.Tables[0].Rows.Count;
            }

            // Ore Belts
            strSQL = "SELECT *";
            strSQL += " FROM mapDenormalize";
            strSQL += " WHERE mapDenormalize.solarSystemID=" + sID + " AND mapDenormalize.groupID = 9 AND mapDenormalize.typeID = 15;";
            systData = EveHQ.Core.DataFunctions.GetData(strSQL);

            if (!systData.Equals(System.DBNull.Value))
            {
                GalData.Regions[rID].Constellations[cID].Systems[sID].OreBelts = systData.Tables[0].Rows.Count;
            }

            // Ice Belts
            strSQL = "SELECT *";
            strSQL += " FROM mapDenormalize";
            strSQL += " WHERE mapDenormalize.solarSystemID=" + sID + " AND mapDenormalize.groupID = 9 AND mapDenormalize.typeID = 17774;";
            systData = EveHQ.Core.DataFunctions.GetData(strSQL);

            if ((systData != null) && !systData.Equals(System.DBNull.Value))
            {
                GalData.Regions[rID].Constellations[cID].Systems[sID].IceBelts = systData.Tables[0].Rows.Count;
            }
        }

        public void BuildStations(int sID)
        {
            string strSQL;
            Station ns;
            DataSet stnData, stnServ;
            int rgnID = 0, srvcID = 0, cnID = 0, sysID = 0;
            Dictionary<int, List<string>> staService = new Dictionary<int, List<string>>();

            strSQL = "SELECT * FROM staStations";
            strSQL += " WHERE ((staStations.stationTypeID < 12242) AND (staStations.solarSystemID = " + sID + "));";
            stnData = EveHQ.Core.DataFunctions.GetData(strSQL);

            strSQL = "SELECT * FROM staOperationServices INNER JOIN staServices ON staOperationServices.serviceID = staServices.serviceID;";
            stnServ = EveHQ.Core.DataFunctions.GetData(strSQL);

            //try
            //{
                if (!stnServ.Equals(System.DBNull.Value))
                {
                    if (stnServ.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in stnServ.Tables[0].Rows)
                        {
                            switch(Convert.ToInt32(dr.ItemArray[1]))
                            {
                                case 4:
                                case 64:
                                case 256:
                                case 1024:
                                case 2048:
                                case 32768:
                                case 131072:
                                case 262144:
                                case 524288:
                                case 2097152:
                                case 33554432:
                                    break;
                                default:
                                    srvcID = Convert.ToInt32(dr.ItemArray[0]);
                                    if (!staService.Keys.Contains(srvcID))
                                        staService.Add(srvcID, new List<string> { dr.ItemArray[3].ToString() });
                                    else
                                        staService[srvcID].Add(dr.ItemArray[3].ToString());
                                    break;
                             }
                        }
                    }
                }

                if (!stnData.Equals(System.DBNull.Value))
                {
                    if (stnData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in stnData.Tables[0].Rows)
                        {
                            rgnID = Convert.ToInt32(row.ItemArray[10]);
                            cnID = Convert.ToInt32(row.ItemArray[9]);

                            if (SkipRegions.Contains(rgnID))
                                continue;

                            ns = new Station();
                            ns.ID = Convert.ToInt32(row.ItemArray[0]);
                            ns.stationTypeID = Convert.ToInt32(row.ItemArray[6]);
                            ns.corpID = Convert.ToInt32(row.ItemArray[7]);
                            ns.Name = row.ItemArray[11].ToString();
                            ns.X = Convert.ToDouble(row.ItemArray[12]);
                            ns.Y = Convert.ToDouble(row.ItemArray[13]);
                            ns.Z = Convert.ToDouble(row.ItemArray[14]);
                            ns.Refine = Convert.ToDouble(row.ItemArray[15]);
                            ns.Services = staService[Convert.ToInt32(row.ItemArray[5])];
                            ns.Faction = Convert.ToInt32(row.ItemArray[7]);
                            sysID = Convert.ToInt32(row.ItemArray[8]);

                            GalData.Regions[rgnID].Constellations[cnID].Systems[sysID].Stations.Add(ns.ID, ns);
                            //ThreadPool.QueueUserWorkItem(new WaitCallback(BuildAgents), (ns.ID.ToString()));
                            BuildAgents(ns.ID);
                        }
                    }
                }
            //}
            //catch
            //{
            //    DialogResult dr = MessageBox.Show("An Error was encountered while Reading in Station Data.", "RouteMap: DB Error", MessageBoxButtons.OK);
            //}
        }

        public void SetupLocAgents()
        {
            string strSQL;
            DataSet lcaData;

            strSQL = "SELECT * FROM agtConfig;";
            lcaData = EveHQ.Core.DataFunctions.GetData(strSQL);

            if (!lcaData.Equals(System.DBNull.Value))
            {
                if (lcaData.Tables[0].Rows.Count > 0)
                {
                    locAgent.Clear();

                    foreach (DataRow dr in lcaData.Tables[0].Rows)
                    {
                        if (dr.ItemArray[1].ToString().StartsWith("agent.Locate"))
                            locAgent.Add(Convert.ToInt32(dr.ItemArray[0].ToString()));
                    }
                }
            }
        }

        public void BuildAgents(int stnID)
        {
            string strSQL, arsSQL, resType, resList;
            DataSet agtData, arsData;
            Agent na;
            int rID, cID, sID, atID;

            strSQL = "SELECT * ";
            strSQL += " FROM agtAgents, agtAgentTypes, crpNPCDivisions, eveNames, staStations";
            strSQL += " WHERE ((agtAgents.agentTypeID = agtAgentTypes.agentTypeID) AND (crpNPCDivisions.divisionID = agtAgents.divisionID)";
            strSQL += " AND (agtAgents.agentID = eveNames.itemID) AND (staStations.stationID = agtAgents.locationID) AND (staStations.stationID = " + stnID + "));";
            agtData = EveHQ.Core.DataFunctions.GetData(strSQL);

            //try
            //{
                if (!agtData.Equals(System.DBNull.Value))
                {
                    if (agtData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in agtData.Tables[0].Rows)
                        {
                            if (row.ItemArray[3].Equals(System.DBNull.Value))
                                continue;

                            na = new Agent();
                            na.ID = Convert.ToInt32(row.ItemArray[0]);
                            na.corpID = Convert.ToInt32(row.ItemArray[2]);
                            na.Type = row.ItemArray[8].ToString();

                            if (!GalData.AgentTypes.Contains(na.Type))
                                GalData.AgentTypes.Add(na.Type);

                            na.Level = Convert.ToInt16(row.ItemArray[4]);
                            if (row.ItemArray[5].Equals(System.DBNull.Value))
                                na.Quality = 0;
                            else
                                na.Quality = Convert.ToInt16(row.ItemArray[5]);
                            na.Divis = row.ItemArray[10].ToString();
                            na.Name = row.ItemArray[14].ToString();
                            na.StationID = Convert.ToInt32(row.ItemArray[3].ToString());
                            atID = Convert.ToInt32(row.ItemArray[7]);

                            if (atID == 4)
                            {
                                arsSQL = "SELECT * ";
                                arsSQL += " FROM agtResearchAgents, invTypes";
                                arsSQL += " WHERE ((agtResearchAgents.typeID = invTypes.typeID) AND (agtResearchAgents.agentID = " + na.ID + "))";
                                arsData = EveHQ.Core.DataFunctions.GetData(arsSQL);

                                resList = "";
                                for (int x = 0; x < arsData.Tables[0].Rows.Count; x++)
                                {
                                    resType = arsData.Tables[0].Rows[x].ItemArray[4].ToString();
                                    if (x>0)
                                        resList += ", " + resType;
                                    else
                                        resList += resType;

                                    if (!GalData.ResearchTypes.Contains(resType))
                                        GalData.ResearchTypes.Add(resType);
                                }
                                if (!resList.Equals(""))
                                    na.ResearchType = resList;
                                else
                                    na.ResearchType = "None";
                            }
                            else
                                na.ResearchType = "None";

                            if (locAgent.Contains(na.ID))
                                na.Locate = true;

                            rID = Convert.ToInt32(row.ItemArray[28]);
                            cID = Convert.ToInt32(row.ItemArray[27]);
                            sID = Convert.ToInt32(row.ItemArray[26]);

                            if (GalData.Regions[rID].Constellations[cID].Systems[sID].Stations.ContainsKey(na.StationID))  
                                GalData.Regions[rID].Constellations[cID].Systems[sID].Stations[na.StationID].Agents.Add(na);
                        }
                    }
                }
            //}
            //catch
            //{
            //    DialogResult dr = MessageBox.Show("An Error was encountered while Reading in Agent Data.", "RouteMap: DB Error", MessageBoxButtons.OK);
            //}
        }

        public void BuildSolarSystemJumps()
        {
            string strSQL;
            DataSet ssjData;
            StarGate sg;
            int frmRgn = 0, toRgn = 0;
            int fSys = 0, tSys = 0;
            int fCn = 0, tCn = 0;

            strSQL = "SELECT * FROM mapSolarSystemJumps;";
            ssjData = EveHQ.Core.DataFunctions.GetData(strSQL);

            //try
            //{
                if (!ssjData.Equals(System.DBNull.Value))
                {
                    if (ssjData.Tables[0].Rows.Count > 0)
                    {
                        GalData.StarGates = new List<StarGate>();
                        foreach (DataRow dr in ssjData.Tables[0].Rows)
                        {
                            frmRgn = Convert.ToInt32(dr.ItemArray[0]);
                            toRgn = Convert.ToInt32(dr.ItemArray[5]);

                            if (SkipRegions.Contains(frmRgn) || SkipRegions.Contains(toRgn))
                                continue;

                            sg = new StarGate();
                            fSys = Convert.ToInt32(dr.ItemArray[2]);
                            tSys = Convert.ToInt32(dr.ItemArray[3]);
                            fCn = Convert.ToInt32(dr.ItemArray[1]);
                            tCn = Convert.ToInt32(dr.ItemArray[4]);

                            if (frmRgn != toRgn)
                                sg.Type = GateType.InterRegion;
                            else if (fCn != tCn)
                                sg.Type = GateType.InterConst;
                            else
                                sg.Type = GateType.Normal;

                            sg.To = GetSystemByID(tSys);
                            sg.From = GetSystemByID(fSys);

                            GalData.StarGates.Add(sg);
                            sg.From.Gates.Add(tSys);
                        }
                    }
                }

            //}
            //catch
            //{
            //    DialogResult dr = MessageBox.Show("An Error was encountered while Reading in Star Gate Data.", "RouteMap: DB Error", MessageBoxButtons.OK);
            //}
            CleanupStargates();
        }

        public void CleanupStargates()
        {
            //Removes duplicate gates from the store.
            for (int i = GalData.StarGates.Count - 1; i >= 0; i--)
            {
                StarGate curGate = GalData.StarGates[i];
                for (int j = GalData.StarGates.Count - 1; j >= 0; j--)
                {
                    StarGate destGate = GalData.StarGates[j];
                    if (curGate.From == destGate.To &&
                        curGate.To == destGate.From)
                    {
                        GalData.StarGates.RemoveAt(i);

                        break;
                    }
                }
            }
        }

        public void BuildStationTypeIDList()
        {
            string strSQL;
            StationType nt;
            DataSet stnData;
            GalData.StationTypes = new SortedList<int, StationType>();

            strSQL = "SELECT * FROM staStationTypes;";
            stnData = EveHQ.Core.DataFunctions.GetData(strSQL);

            try
            {
                if (!stnData.Equals(System.DBNull.Value))
                {
                    if (stnData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in stnData.Tables[0].Rows)
                        {
                            nt = new StationType();

                            if (dr.ItemArray[0].Equals(System.DBNull.Value))
                                nt.ID = 0;
                            else
                                nt.ID = Convert.ToInt16(dr.ItemArray[0]);

                            if (dr.ItemArray[7].Equals(System.DBNull.Value))
                                nt.opID = 0;
                            else
                                nt.opID = Convert.ToInt16(dr.ItemArray[7]);

                            if (dr.ItemArray[8].Equals(System.DBNull.Value))
                                nt.OffSlots = 0;
                            else
                                nt.OffSlots = Convert.ToInt16(dr.ItemArray[8]);

                            if (dr.ItemArray[9].Equals(System.DBNull.Value))
                                nt.ReprocEff = 0;
                            else
                                nt.ReprocEff = Convert.ToDouble(dr.ItemArray[9]);

                            if (dr.ItemArray[10].ToString().Equals("true"))
                                nt.Conquerable = true;
                            else
                                nt.Conquerable = false;

                            GalData.StationTypes.Add(nt.ID, nt);
                        }
                    }
                }
            }
            catch
            {
                DialogResult dr = MessageBox.Show("An Error was encountered while Reading in Station Type Data.", "RouteMap: DB Error", MessageBoxButtons.OK);
            }

        }

        public bool SaveGalaxy(string LC)
        {
            string fname;

            fname = Path.Combine(EveHQ.Core.HQ.appFolder, "DB_EveGalaxy.bin");

            // Save the Serialized data to Disk
            Stream pStream = File.Create(fname);
            BinaryFormatter pBF = new BinaryFormatter();
            pBF.Serialize(pStream, GalData);
            pStream.Close();

            if (LC.Length > 0)
            {
                if (!Directory.Exists(PlugInData.RMapCache_Path))
                    Directory.CreateDirectory(PlugInData.RMapCache_Path);

                fname = Path.Combine(PlugInData.RMapCache_Path, "version.txt");
                StreamWriter sw = new StreamWriter(fname);
                sw.Write(LC);
                sw.Close();
            }

            return true;
        }

        public bool UpdateLastCacheRefresh(string LC)
        {
            string fname;

            if (LC.Length > 0)
            {
                if (!Directory.Exists(PlugInData.RMapCache_Path))
                    Directory.CreateDirectory(PlugInData.RMapCache_Path);
                
                fname = Path.Combine(PlugInData.RMapCache_Path, "version.txt");
                StreamWriter sw = new StreamWriter(fname);
                sw.Write(LC);
                sw.Close();
            }

            return true;
        }

        public bool LoadGalaxy()
        {
            string fname;
            Stream cStr;
            BinaryFormatter myBf;

            fname = Path.Combine(EveHQ.Core.HQ.appFolder, "DB_EveGalaxy.bin");

            // Load the Data from Disk
            if (File.Exists(fname))
            {
                // We have a configuration data file
                cStr = File.OpenRead(fname);
                myBf = new BinaryFormatter();

                try
                {
                    GalData = (Galaxy)myBf.Deserialize(cStr);
                }
                catch
                {
                }

                cStr.Close();
            }

            return true;
        }

        public void UpdateGalaxyData(Object o)
        {
            int sel = (int)o;

            SetupLocAgents();
            BuildStationTypeIDList();   // Can easily run parallel

            // This should ONLY run via PlugInData - it will take a while :)

            //ORDER IS IMPORTANT!
            if (sel == 0)
            {
                GalData.Regions.Clear();
                GalData.StarGates.Clear();

                BuildRegions();
                BuildConstellations();
                BuildSolarSystems();
            }

            if (Interlocked.Decrement(ref PlugInData.numBusy) == 0)
            {
                PlugInData.doneEvent.Set();
            }

            return;
        }

        public void UpdateRegionData(Object o)
        {
            int rID;

            rID = (int)o;

            BuildRegion(rID);
            BuildConstellation(rID);
            BuildSolarSystemsForRegion(rID);

            if (Interlocked.Decrement(ref PlugInData.numBusy) == 0)
            {
                BuildSolarSystemJumps();    // Must run semi-last
                PlugInData.doneEvent.Set();
            }
        }

        public static List<Color> GenerateRandomizedColors()
        {
            const byte alpha = 0x90;
            Random rg = new Random();
            var colors = new List<Color>();

            for (int r = 0; r <= 4; r++)
                for (int g = 0; g <= 4; g++)
                    for (int b = 0; b <= 4; b++)
                        colors.Add(Color.FromArgb(alpha, (r * 255 / 4), (g * 255 / 4), (b * 255 / 4)));


            for (int i = 0; i < colors.Count; i++) // knuth permutation, swap each element with a randomly chosen one
            {
                int j = rg.Next(0, colors.Count);
                Color temp = colors[j];
                colors[j] = colors[i];
                colors[i] = temp;
            }

            return colors;
        }

        public ArrayList GetSystemsCanJumpTo(SolarSystem ss, double JR)
        {
            ArrayList retSystems = new ArrayList();
            JumpDistance jd;
            double dist;

            foreach (var region in GalData.Regions)
            {
                foreach (var constellation in region.Value.Constellations)
                {
                    foreach (var system in constellation.Value.Systems)
                    {
                        if (ss.ID == system.Value.ID)
                            continue;
                        if (system.Value.Security >= 0.45f)
                            continue;
                        dist = ss.GetJumpDistance(system.Value);
                        if (dist >= JR) // Max for Carrier Class
                            continue;

                        jd = new JumpDistance(system.Value, dist);
                        retSystems.Add(jd);
                    }
                }
            }

            retSystems.Sort();
            return retSystems;
        }

        public List<SolarSystem> GetSystemListCanJumpTo(SolarSystem ss, double JR)
        {
            List<SolarSystem> retSystems = new List<SolarSystem>();
            double dist;

            foreach (var region in GalData.Regions)
            {
                foreach (var constellation in region.Value.Constellations)
                {
                    foreach (var system in constellation.Value.Systems)
                    {
                        if (ss.ID == system.Value.ID)
                            continue;
                        if (system.Value.Security >= 0.45f)
                            continue;
                        dist = ss.GetJumpDistance(system.Value);
                        if (dist >= JR) // Max for Carrier Class
                            continue;

                        retSystems.Add(system.Value);
                    }
                }
            }

            return retSystems;
        }

        public ArrayList GetSystemsJFCanJumpFrom(SolarSystem ss, double JR)
        {
            ArrayList retSystems = new ArrayList();
            JumpDistance jd;
            double dist;

            if (ss.Security > 0.45)
                return retSystems;

            foreach (var region in GalData.Regions)
            {
                foreach (var constellation in region.Value.Constellations)
                {
                    foreach (var system in constellation.Value.Systems)
                    {
                        if (ss.ID == system.Value.ID)
                            continue;
                        dist = ss.GetJumpDistance(system.Value);
                        if (dist >= JR) // Max distance for a JF
                            continue;

                        jd = new JumpDistance(system.Value, dist);
                        retSystems.Add(jd);
                    }
                }
            }

            retSystems.Sort();
            return retSystems;
        }

        public void ComputeConstJumpDriveAdj()
        {
            //foreach (var region in GalData.Regions)
            //{
            //    foreach (var constellation in region.Value.Constellations)
            //    {
            //        foreach (var system in constellation.Value.Systems)
            //        {
            //            foreach (var rgn in GalData.Regions)
            //            {
            //                foreach (var cnst in rgn.Value.Constellations)
            //                {
            //                    foreach (var pair in cnst.Value.Systems)
            //                    {
            //                        if (pair.Value.ID == system.Value.ID)
            //                            continue;

            //                        if (pair.Value.Security >= 0.45f)
            //                            continue;

            //                        if (system.Value.GetJumpDistance(pair.Value) >= 14.625d)
            //                            continue;

            //                        if (constellation.Value.AdjConstJump == null)
            //                            constellation.Value.AdjConstJump = new List<int>();

            //                        if (!constellation.Value.AdjConstJump.Contains(pair.Value.ConstID))
            //                            constellation.Value.AdjConstJump.Add(pair.Value.ConstID);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

        public string GetIce(int regionID, double secLev)
        {
            string IceList = "";

            if ((secLev <= 0.35) && (secLev > 0.15))
                IceList += "Glare Crust";
            else if ((secLev <= 0.15) && (secLev >= -0.15))
                IceList += "Glare Crust, Dark Glitter";
            else if ((secLev <= -0.15) && (secLev >= -0.449))
                IceList += "Dark Glitter, Gelidus";
            else if ((secLev < -0.449) && (secLev > -0.5))
                IceList += "Gelidus";
            else if ((secLev <= -0.5) && (secLev > -0.65))
                IceList += "Gelidus, Krystallos";
            else if ((secLev <= -0.65) && (secLev >= -1))
                IceList += "Krystallos";

            switch (regionID)
            {
                case 10000001:
                case 10000014:
                case 10000020:
                case 10000022:
                case 10000036:
                case 10000038:
                case 10000039:
                case 10000043:
                case 10000047:
                case 10000049:
                case 10000050:
                case 10000052:
                case 10000054:
                case 10000059:
                case 10000060:
                case 10000063:
                case 10000065:
                case 10000067:
                    if ((secLev <= 1) && (secLev > 0.35))
                        IceList += "Clear Icicle";
                    if ((secLev <= 0.35) && (secLev >= 0.05))
                        IceList += ", Clear Icicle";
                    if ((secLev <= 0.05) && (secLev >= -1))
                        IceList += ", Enriched Clear Icicle";
                break;
                case 10000002:
                case 10000003:
                case 10000010:
                case 10000015:
                case 10000016:
                case 10000023:
                case 10000029:
                case 10000033:
                case 10000035:
                case 10000045:
                case 10000055:
                case 10000069:
                    if ((secLev <= 1) && (secLev > 0.35))
                        IceList += "White Glaze";
                    if ((secLev <= 0.35) && (secLev >= 0.05))
                        IceList += ", White Glaze";
                    if ((secLev <= 0.05) && (secLev >= -1))
                        IceList += ", Pristine White Glaze";
                    break;
                case 10000005:
                case 10000006:
                case 10000007:
                case 10000008:
                case 10000009:
                case 10000011:
                case 10000012:
                case 10000025:
                case 10000028:
                case 10000030:
                case 10000031:
                case 10000042:
                case 10000056:
                case 10000061:
                case 10000062:
                    if ((secLev <= 1) && (secLev > 0.35))
                        IceList += "Glacial Mass";
                    if ((secLev <= 0.35) && (secLev >= 0.05))
                        IceList += ", Glacial Mass";
                    if ((secLev <= 0.05) && (secLev >= -1))
                        IceList += ", Smooth Glacial Mass";
                    break;
                case 10000013:
                case 10000018:
                case 10000021:
                case 10000027:
                case 10000032:
                case 10000034:
                case 10000037:
                case 10000040:
                case 10000041:
                case 10000044:
                case 10000046:
                case 10000048:
                case 10000051:
                case 10000053:
                case 10000057:
                case 10000058:
                case 10000064:
                case 10000066:
                case 10000068:
                    if ((secLev <= 1) && (secLev > 0.35))
                        IceList += "Blue Ice";
                    if ((secLev <= 0.35) && (secLev >= 0.05))
                        IceList += ", Blue Ice";
                    if ((secLev <= 0.05) && (secLev >= -1))
                        IceList += ", Thick Blue Ice";
                    break;
                default:
                    IceList += ", Unknown";
                    break;
            }

            return IceList;
        }

        public string GetOre(string sClass)
        {
            string rVal;

            switch(sClass)
            {
                case "A": rVal = "Veldspar, Scordite"; break;
                case "B": rVal = "Veldspar, Scordite, Pyroxeres"; break;
                case "B1": rVal = "Veldspar, Scordite, Pyroxeres, Kernite"; break;
                case "B2": rVal = "Veldspar, Scordite, Pyroxeres, Kernite, Jaspet"; break;
                case "B3": rVal = "Veldspar, Scordite, Pyroxeres, Kernite, Jaspet, Hemorphite"; break;
                case "C": rVal = "Veldspar, Scordite, Plagioclase, Pyroxeres"; break;
                case "C1": rVal = "Veldspar, Scordite, Plagioclase, Pyroxeres, Kernite"; break;
                case "C2": rVal = "Veldspar, Scordite, Plagioclase, Pyroxeres, Kernite, Hedbergite"; break;
                case "D": rVal = "Veldspar, Scordite, Plagioclase"; break;
                case "D1": rVal = "Veldspar, Scordite, Plagioclase, Omber"; break;
                case "D2": rVal = "Veldspar, Scordite, Plagioclase, Omber, Jaspet"; break;
                case "D3": rVal = "Veldspar, Scordite, Plagioclase, Omber, Jaspet, Hemorphite"; break;
                case "E": rVal = "Veldspar, Scordite, Plagioclase, Omber, Kernite"; break;
                case "E1": rVal = "Veldspar, Scordite, Plagioclase, Omber, Kernite, Hedbergite"; break;
                case "F": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber"; break;
                case "F1": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Spodumain"; break;
                case "F2": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Spodumain, Gneiss"; break;
                case "F3": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Spodumain, Gneiss, Bistot"; break;
                case "F4": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Spodumain, Gneiss, Bistot, Arkonor"; break;
                case "F5": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Spodumain, Gneiss, Bistot, Arkonor, Pyroxeres"; break;
                case "F6": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Spodumain, Gneiss, Bistot, Arkonor, Pyroxeres, Mercoxit"; break;
                case "F7": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Spodumain, Gneiss, Bistot, Arkonor, Pyroxeres, Mercoxit, Plagioclase"; break;
                case "G": rVal = "Veldspar, Scordite, Plagioclase, Omber, Kernite"; break;
                case "G1": rVal = "Veldspar, Scordite, Plagioclase, Omber, Kernite, Gneiss"; break;
                case "G2": rVal = "Veldspar, Scordite, Plagioclase, Omber, Kernite, Gneiss, Pyroxeres"; break;
                case "G3": rVal = "Veldspar, Scordite, Plagioclase, Omber, Kernite, Gneiss, Pyroxeres, Spodumain"; break;
                case "G4": rVal = "Veldspar, Scordite, Plagioclase, Omber, Kernite, Gneiss, Pyroxeres, Spodumain, Bistot"; break;
                case "G5": rVal = "Veldspar, Scordite, Plagioclase, Omber, Kernite, Gneiss, Pyroxeres, Spodumain, Bistot, Crokite"; break;
                case "G6": rVal = "Veldspar, Scordite, Plagioclase, Omber, Kernite, Gneiss, Pyroxeres, Spodumain, Bistot, Crokite, Mercoxit"; break;
                case "G7": rVal = "Veldspar, Scordite, Plagioclase, Omber, Kernite, Gneiss, Pyroxeres, Spodumain, Bistot, Crokite, Mercoxit, Dark Ochre"; break;
                case "H": rVal = "Veldspar, Scordite, Pyroxeres, Hemorphite, Jaspet"; break;
                case "H1": rVal = "Veldspar, Scordite, Pyroxeres, Hemorphite, Jaspet, Hedbergite"; break;
                case "H2": rVal = "Veldspar, Scordite, Pyroxeres, Hemorphite, Jaspet, Hedbergite, Dark Ochre"; break;
                case "H3": rVal = "Veldspar, Scordite, Pyroxeres, Hemorphite, Jaspet, Hedbergite, Dark Ochre, Kernite"; break;
                case "H4": rVal = "Veldspar, Scordite, Pyroxeres, Hemorphite, Jaspet, Hedbergite, Dark Ochre, Kernite, Crokite"; break;
                case "H5": rVal = "Veldspar, Scordite, Pyroxeres, Hemorphite, Jaspet, Hedbergite, Dark Ochre, Kernite, Crokite, Spodumain"; break;
                case "H6": rVal = "Veldspar, Scordite, Pyroxeres, Hemorphite, Jaspet, Hedbergite, Dark Ochre, Kernite, Crokite, Spodumain, Mercoxit"; break;
                case "H7": rVal = "Veldspar, Scordite, Pyroxeres, Hemorphite, Jaspet, Hedbergite, Dark Ochre, Kernite, Crokite, Spodumain, Mercoxit, Bistot"; break;
                case "I": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber"; break;
                case "I1": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Jaspet"; break;
                case "I2": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Jaspet, Spodumain"; break;
                case "I3": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Jaspet, Spodumain, Gneiss"; break;
                case "I4": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Jaspet, Spodumain, Gneiss, Dark Ochre"; break;
                case "I5": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Jaspet, Spodumain, Gneiss, Dark Ochre, Arkonor"; break;
                case "I6": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Jaspet, Spodumain, Gneiss, Dark Ochre, Arkonor, Mercoxit"; break;
                case "I7": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Jaspet, Spodumain, Gneiss, Dark Ochre, Arkonor, Mercoxit, Kernite"; break;
                case "J": rVal = "Veldspar, Scordite, Pyroxeres, Plagioclase, Jaspet"; break;
                case "J1": rVal = "Veldspar, Scordite, Pyroxeres, Plagioclase, Jaspet, Dark Ochre"; break;
                case "J2": rVal = "Veldspar, Scordite, Pyroxeres, Plagioclase, Jaspet, Dark Ochre, Crokite"; break;
                case "J3": rVal = "Veldspar, Scordite, Pyroxeres, Plagioclase, Jaspet, Dark Ochre, Crokite, Bistot"; break;
                case "J4": rVal = "Veldspar, Scordite, Pyroxeres, Plagioclase, Jaspet, Dark Ochre, Crokite, Bistot, Hemorphite"; break;
                case "J5": rVal = "Veldspar, Scordite, Pyroxeres, Plagioclase, Jaspet, Dark Ochre, Crokite, Bistot, Hemorphite, Hedbergite"; break;
                case "J6": rVal = "Veldspar, Scordite, Pyroxeres, Plagioclase, Jaspet, Dark Ochre, Crokite, Bistot, Hemorphite, Hedbergite, Mercoxit"; break;
                case "J7": rVal = "Veldspar, Scordite, Pyroxeres, Plagioclase, Jaspet, Dark Ochre, Crokite, Bistot, Hemorphite, Hedbergite, Mercoxit, Arkonor"; break;
                case "K": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber"; break;
                case "K1": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Dark Ochre"; break;
                case "K2": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Dark Ochre, Spodumain"; break;
                case "K3": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Dark Ochre, Spodumain, Crokite"; break;
                case "K4": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Dark Ochre, Spodumain, Crokite, Bistot"; break;
                case "K5": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Dark Ochre, Spodumain, Crokite, Bistot, Arkonor"; break;
                case "K6": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Dark Ochre, Spodumain, Crokite, Bistot, Arkonor, Mercoxit"; break;
                case "K7": rVal = "Veldspar, Scordite, Hedbergite, Hemorphite, Omber, Dark Ochre, Spodumain, Crokite, Bistot, Arkonor, Mercoxit, Gneiss"; break;
                default: rVal = "Unknown"; break;
            }

            return rVal;
        }
    }
}
