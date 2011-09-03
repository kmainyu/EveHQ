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
    public class SystemCelestials
    {
        public SortedList<int,Planet> Planets;
        public SortedList<int,Moon> Moons;
        public SortedList<int,Belt> OreBelts;
        public SortedList<int, Belt> IceBelts;
        public SortedList<int, StarGate> Gates;
        public SortedList<string, int> CelestToID;
        public int graphicID;
        public double SunR;
        public string Anomaly;
        public List<string> AnomEffects;


        public SystemCelestials()
        {
            Planets = new SortedList<int, Planet>();
            Moons = new SortedList<int, Moon>();
            OreBelts = new SortedList<int, Belt>();
            IceBelts = new SortedList<int, Belt>();
            Gates = new SortedList<int, StarGate>();
            CelestToID = new SortedList<string, int>();
            graphicID = 0;
            SunR = 0;
            Anomaly = "";
            AnomEffects = new List<string>();
        }

        public string GetPlanetMoonNameForID(int id)
        {
            if (Planets.ContainsKey(id))
                return Planets[id].Name;

            if (Moons.ContainsKey(id))
                return Moons[id].Name;

            return "";
        }

        public int[] GetPlanetMoonIDsForName(string name)
        {
            int[] rVal = new int[2];
            rVal[0] = -1;
            rVal[1] = -1;

            foreach (Planet p in Planets.Values)
            {
                foreach (Moon m in Moons.Values)
                {
                    if (p.CelIndex == m.CelIndex)
                    {
                        if (p.Name == name)
                        {
                            rVal[0] = p.ID;
                            rVal[1] = 0;
                            return rVal;
                        }
                        else if (m.Name == name)
                        {
                            rVal[0] = p.ID;
                            rVal[1] = m.ID;
                            return rVal;
                        }
                    }
                }
            }

            return rVal;
        }

        public int GetMoonIDForName(string name)
        {
            foreach (Moon m in Moons.Values)
                if (m.Name == name)
                    return m.ID;

            return -1;
        }

        public int GetPlanetIDForName(string name)
        {
            foreach (Planet p in Planets.Values)
                if (p.Name == name)
                    return p.ID;

            return -1;
        }

        public void GetSystemCelestial(SolarSystem ss)
        {
            if (ss == null)
                return;

            GetSystemCelestialForID(ss.ID);
        }

        public void GetSystemCelestialForID(int sID)
        {
            string strSQL, gName, strSQL2, effName, eff, effTyp, whp;
            DataSet systData, gateData, anomData, wcData;
            int grpID = 0, cls = 0;
            double val = 0;
            long rID, uID, whidn;
            Planet p;
            Planet cp = new Planet();
            StarGate g;
            Moon m;
            Belt b;
            bool whSys = false;

            if (sID <= 0)
                return;

            Anomaly = "";
            AnomEffects = new List<string>();

            strSQL = "SELECT *";
            strSQL += " FROM invTypes INNER JOIN (mapDenormalize INNER JOIN mapCelestialStatistics ON mapCelestialStatistics.celestialID = mapDenormalize.itemID) ON invTypes.typeID = mapDenormalize.typeID";
            strSQL += " WHERE mapDenormalize.solarSystemID=" + sID + ";";
            systData = EveHQ.Core.DataFunctions.GetData(strSQL);

            Planets.Clear();
            Moons.Clear();
            OreBelts.Clear();
            IceBelts.Clear();
            Gates.Clear();
            if ((systData != null) && !systData.Equals(System.DBNull.Value))
            {
                if (systData.Tables[0].Rows.Count > 0)
                {
                    cp = null;
                    foreach (DataRow row in systData.Tables[0].Rows)
                    {
                        grpID = Convert.ToInt32(row.ItemArray[18]);

                        switch (grpID)
                        {
                            case 6:
                                // Star - always center, ignore
                                graphicID = Convert.ToInt32(row.ItemArray[4]);
                                SunR = Convert.ToDouble(row.ItemArray[49]);
                                break;
                            case 7:
                                // Planet
                                p = new Planet();
                                p.Name = row.ItemArray[27].ToString();
                                p.Number = p.GetPlanetNumber(p.Name);
                                p.X = Convert.ToDouble(row.ItemArray[23]);
                                p.Y = Convert.ToDouble(row.ItemArray[24]);
                                p.Z = Convert.ToDouble(row.ItemArray[25]);
                                p.CelIndex = Convert.ToInt32(row.ItemArray[29]);
                                p.radius = Convert.ToDouble(row.ItemArray[49]);
                                p.orbitR = Convert.ToDouble(row.ItemArray[37]);
                                p.orbitE = Convert.ToDouble(row.ItemArray[38]);
                                p.orbitP = Convert.ToDouble(row.ItemArray[45]);
                                p.graphicID = Convert.ToInt32(row.ItemArray[4]);
                                p.ID = Convert.ToInt32(row.ItemArray[16]);

                                Planets.Add(Convert.ToInt32(row.ItemArray[16]), p);
                                if (!CelestToID.ContainsKey(p.Name))
                                    CelestToID.Add(p.Name, p.ID);

                                cp = p;
                                break;
                            case 8:
                                if (cp == null)
                                    break;

                                m = new Moon();

                                m.Name = row.ItemArray[27].ToString();
                                m.X = Convert.ToDouble(row.ItemArray[23]);
                                m.Y = Convert.ToDouble(row.ItemArray[24]);
                                m.Z = Convert.ToDouble(row.ItemArray[25]);
                                m.radius = Convert.ToDouble(row.ItemArray[49]);
                                m.orbitR = Convert.ToDouble(row.ItemArray[37]);
                                m.orbitP = Convert.ToDouble(row.ItemArray[45]);
                                m.CelIndex = Convert.ToInt32(row.ItemArray[29]);
                                m.OrbitIndex = Convert.ToInt32(row.ItemArray[30]);
                                m.graphicID = Convert.ToInt32(row.ItemArray[4]);
                                m.ID = Convert.ToInt32(row.ItemArray[16]);
                                if (!CelestToID.ContainsKey(m.Name))
                                    CelestToID.Add(m.Name, m.ID);

                                Moons.Add(Convert.ToInt32(row.ItemArray[16]), m);
                                // Moon
                                break;
                            case 9:
                                if (cp == null)
                                    break;

                                switch (Convert.ToInt32(row.ItemArray[15]))
                                {
                                    case 15:
                                        b = new Belt();
                                        // Asteroid Belt
                                        b.Name = row.ItemArray[27].ToString();
                                        b.X = Convert.ToDouble(row.ItemArray[23]);
                                        b.Y = Convert.ToDouble(row.ItemArray[24]);
                                        b.Z = Convert.ToDouble(row.ItemArray[25]);
                                        b.radius = 1;
                                        b.orbitR = Convert.ToDouble(row.ItemArray[37]);
                                        b.orbitP = Convert.ToDouble(row.ItemArray[45]);
                                        b.CelIndex = Convert.ToInt32(row.ItemArray[29]);
                                        b.OrbitIndex = Convert.ToInt32(row.ItemArray[30]);
                                        //b.graphicID = Convert.ToInt32(row.ItemArray[4]); -- Belts do not have a graphicID (is NULL)
                                        b.ID = Convert.ToInt32(row.ItemArray[16]);

                                        OreBelts.Add(Convert.ToInt32(row.ItemArray[16]), b);

                                        if (!CelestToID.ContainsKey(b.Name))
                                            CelestToID.Add(b.Name, b.ID);

                                        cp.Belts++;
                                        break;
                                    case 17774:
                                        break;
                                    default:
                                        // Do not care
                                        break;
                                }
                                // Belt
                                break;
                            case 10:
                                // Stargate
                                break;
                            case 15:
                                // Station
                                break;
                            default:
                                // Unknown Object
                                break;
                        }
                    }
                }
            }

            strSQL = "SELECT *";
            strSQL += " FROM mapDenormalize";
            strSQL += " WHERE mapDenormalize.solarSystemID=" + sID + " AND mapDenormalize.typeID=17774;";
            systData = EveHQ.Core.DataFunctions.GetData(strSQL);
            if (!systData.Equals(System.DBNull.Value))
            {
                if (systData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in systData.Tables[0].Rows)
                    {
                        b = new Belt();
                        // Ice Field
                        b.Ice = true;
                        b.Name = row.ItemArray[11].ToString();
                        b.X = Convert.ToDouble(row.ItemArray[7]);
                        b.Y = Convert.ToDouble(row.ItemArray[8]);
                        b.Z = Convert.ToDouble(row.ItemArray[9]);
                        b.radius = 1;
                        b.orbitR = 0;
                        b.orbitP = 0;
                        b.CelIndex = Convert.ToInt32(row.ItemArray[13]);
                        b.OrbitIndex = Convert.ToInt32(row.ItemArray[14]);
                        b.graphicID = 0;
                        b.ID = Convert.ToInt32(row.ItemArray[0]);

                        IceBelts.Add(b.ID, b);
                        if (!CelestToID.ContainsKey(b.Name))
                            CelestToID.Add(b.Name, b.ID);

                        cp.IceFields++;
                    }
                }
            }

            strSQL = "SELECT *";
            strSQL += " FROM invTypes INNER JOIN mapDenormalize ON invTypes.typeID = mapDenormalize.typeID";
            strSQL += " WHERE mapDenormalize.solarSystemID=" + sID + " AND mapDenormalize.groupID=10;";
            gateData = EveHQ.Core.DataFunctions.GetData(strSQL);

            if (!gateData.Equals(System.DBNull.Value))
            {
                if (gateData.Tables[0].Rows.Count > 0)
                {
                    cp = null;
                    foreach (DataRow row in gateData.Tables[0].Rows)
                    {
                        // Stargate
                        g = new StarGate();
                        // Need the following: X, Y, Z (like ones above)
                        // graphic ID - invTypes, ItemArray[4]
                        // dest ID - mapDenormalize, ItemArray[18]
                        g.X = Convert.ToDouble(row.ItemArray[23]);
                        g.Y = Convert.ToDouble(row.ItemArray[24]);
                        g.Z = Convert.ToDouble(row.ItemArray[25]);
                        g.radius = Convert.ToDouble(row.ItemArray[26]);
                        g.graphicID = Convert.ToInt32(row.ItemArray[4]);
                        g.destSys = row.ItemArray[27].ToString();
                        g.ID = Convert.ToInt32(row.ItemArray[16]);

                        Gates.Add(Convert.ToInt32(row.ItemArray[16]), g);
                        gName = g.destSys;
                        if (!CelestToID.ContainsKey(gName))
                            CelestToID.Add(gName, g.ID);
                    }
                }
            }

            SolarSystem s = PlugInData.GalMap.GetSystemByID(sID);
            if (s.Name[0].Equals('J'))
            {
                whp = s.Name.Replace("J", "");
                if (long.TryParse(whp, out whidn))
                {
                    whSys = true;
                }
            }

            if (whSys)
            {

                // Wormhole specific stuff
                strSQL2 = "SELECT * FROM invTypes, mapDenormalize WHERE mapDenormalize.solarSystemID=" + sID;
                strSQL2 += " AND invTypes.typeID=mapDenormalize.typeID AND invTypes.groupID=995;";
                anomData = EveHQ.Core.DataFunctions.GetData(strSQL2);
                rID = 0;
                if (!anomData.Equals(System.DBNull.Value))
                {
                    if (anomData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in anomData.Tables[0].Rows)
                        {
                            Anomaly = row.ItemArray[2].ToString();
                            Anomaly = Anomaly.Replace(" Star", "");
                            Anomaly = Anomaly.Replace('-', ' ');
                            rID = Convert.ToInt64(row.ItemArray[21]);
                        }
                    }
                }

                strSQL = "SELECT invTypes.typeName, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, dgmAttributeTypes.unitID, dgmAttributeTypes.displayName";
                strSQL += " FROM dgmAttributeTypes INNER JOIN (invTypes INNER JOIN dgmTypeAttributes ON invTypes.typeID = dgmTypeAttributes.typeID) ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID";
                strSQL += " WHERE (((invTypes.groupID)=920));";
                systData = EveHQ.Core.DataFunctions.GetData(strSQL);

                if (!systData.Equals(System.DBNull.Value))
                {
                    if (systData.Tables[0].Rows.Count > 0)
                    {
                        strSQL = "SELECT * FROM mapLocationWormholeClasses WHERE locationID=" + rID + ";";
                        wcData = EveHQ.Core.DataFunctions.GetData(strSQL);
                        if (!wcData.Equals(System.DBNull.Value))
                        {
                            if (wcData.Tables[0].Rows.Count > 0)
                            {
                                cls = Convert.ToInt32(wcData.Tables[0].Rows[0].ItemArray[1]);

                                if (Anomaly.Equals("Red Giant"))
                                    eff = Anomaly + " Beacon Class " + cls;
                                else
                                    eff = Anomaly + " Effect Beacon Class " + cls;

                                foreach (DataRow row in systData.Tables[0].Rows)
                                {
                                    val = GetDoubleFromVariableIA(row, 2, 3);
                                    effName = row.ItemArray[5].ToString();
                                    effTyp = row.ItemArray[0].ToString();
                                    uID = Convert.ToInt32(row.ItemArray[4]);

                                    effName = effName.Replace(" bonus", "");
                                    effName = effName.Replace(" Bonus", "");
                                    effName = effName.Replace("Resistance", "Resist");
                                    effName = effName.Replace("resistance", "Resist");
                                    effName = effName.Replace("multiplier", "Mult");

                                    effName = effName.Replace("kinetic", "Kin");
                                    effName = effName.Replace("Kinetic", "Kin");
                                    effName = effName.Replace("Thermal", "Thm");
                                    effName = effName.Replace("thermal", "Thm");
                                    effName = effName.Replace("explosive", "Exp");
                                    effName = effName.Replace("Explosive", "Exp");
                                    effName = effName.Replace("weapon", "Wep");
                                    effName = effName.Replace("Weapon", "Wep");
                                    effName = effName.Replace("Damage", "Dmg");
                                    effName = effName.Replace("damage", "Dmg");

                                    if (uID.Equals(124) || uID.Equals(105))
                                        val *= -1;

                                    if (effTyp.Equals(eff))
                                        AnomEffects.Add(effName + " [ " + val + " ]");

                                }
                            }
                        }
                    }
                }
            }
        }

        public ArrayList GetSystemMoonsForID(int sID)
        {
            string strSQL;
            ArrayList retVal = new ArrayList();
            DataSet systData;

            if (sID <= 0)
                return retVal;

            strSQL = "SELECT *";
            strSQL += " FROM invTypes INNER JOIN (mapDenormalize INNER JOIN mapCelestialStatistics ON mapCelestialStatistics.celestialID = mapDenormalize.itemID) ON invTypes.typeID = mapDenormalize.typeID";
            strSQL += " WHERE mapDenormalize.solarSystemID=" + sID + " AND invTypes.groupID=8;";
            systData = EveHQ.Core.DataFunctions.GetData(strSQL);

            if ((systData != null) && !systData.Equals(System.DBNull.Value))
            {
                if (systData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in systData.Tables[0].Rows)
                    {
                        retVal.Add(row.ItemArray[27].ToString());
                    }
                }
            }

            return retVal;
        }

        public void GetIcon(int ID)
        {
            Image img;
            string imgId;

            imgId = ID.ToString();
            img = EveHQ.Core.ImageHandler.GetImage(imgId);
        }

        private double GetDoubleFromVariableIA(DataRow dr, long aI_1, long aI_2)
        {
            double retVal = -1;

            if (!dr.ItemArray[aI_1].Equals(System.DBNull.Value))
            {
                retVal = Convert.ToDouble(dr.ItemArray[aI_1]);
            }
            else
            {
                retVal = Convert.ToDouble(dr.ItemArray[aI_2]);
            }

            return retVal;
        }


    }
}
