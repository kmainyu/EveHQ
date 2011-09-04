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
    public class SystemDetails
    {
        public int CelestialID;
        public string Name, Corp, Alliance, Type, Password, SysName;
        public bool HasCynoGen, HasCynoJam, HasJumpBridge;
        public bool HasCHA, HasSMA, HasCapSMA, HasCapAssembly;
        public bool IsMining, HasIHub, HasTCU, CynoSafeSpot;
        public int Defenses;  // 0 = None, 1 = Defenses, 2 = Death Star, 3 = Dik Star
        public ArrayList MoonGoo;
        public int pID, mID;
        public double X, Y, Z;

        public SystemDetails()
        {
            CelestialID = 0;
            pID = 0;
            mID = 0;
            X = 0;
            Y = 0;
            Z = 0;
            Name = "";
            Corp = "";
            Alliance = "";
            Type = "";
            Password = "";
            SysName = "";
            HasCynoGen = false;
            HasCynoJam = false;
            HasJumpBridge = false;
            HasCHA = false;
            HasSMA = false;
            HasCapSMA = false;
            HasCapAssembly = false;
            HasIHub = false;
            HasTCU = false;
            IsMining = false;
            CynoSafeSpot = false;
            Defenses = 0;
            MoonGoo = new ArrayList();
            for (int x = 0; x < 8; x++)
                MoonGoo.Add("Unknown");
        }

        public bool IsDataDifferentFromDefault(SystemDetails SD)
        {
            if (!SD.Name.Equals("") && !SD.Name.Equals("Unknown"))
                return true;
            else if (!SD.Corp.Equals("") && !SD.Corp.Equals("Unknown"))
                return true;
            else if (!SD.Alliance.Equals("") && !SD.Alliance.Equals("Unknown"))
                return true;
            else if (!SD.Type.Equals("") && !SD.Type.Equals("Unknown"))
                return true;
            else if (!SD.Password.Equals(""))
                return true;
            else if (!SD.HasCynoGen.Equals(false))
                return true;
            else if (!SD.HasCynoJam.Equals(false))
                return true;
            else if (!SD.HasJumpBridge.Equals(false))
                return true;
            else if (!SD.HasCHA.Equals(false))
                return true;
            else if (!SD.HasSMA.Equals(false))
                return true;
            else if (!SD.HasCapSMA.Equals(false))
                return true;
            else if (!SD.HasCapAssembly.Equals(false))
                return true;
            else if (!SD.IsMining.Equals(false))
                return true;
            else if (!SD.CynoSafeSpot.Equals(false))
                return true;
            else if (!SD.HasCynoGen.Equals(false))
                return true;
            else if (!SD.Defenses.Equals(0))
                return true;
            else if (!SD.HasCynoGen.Equals(false))
                return true;
            else if (!SD.HasIHub.Equals(false))
                return true;
            else if (!SD.HasTCU.Equals(false))
                return true;
            else
            {
                foreach (string s in SD.MoonGoo)
                    if (!s.Equals("Unknown"))
                        return true;
            }

            return false;
        }

    }
}
