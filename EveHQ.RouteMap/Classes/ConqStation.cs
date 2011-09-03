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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace EveHQ.RouteMap
{
    [Serializable]
    public class ConqStation
    {
        public int ID;
        public string Name;
        public string CorpName;
        public int StnTypeID;
        public int SSysID;
        public int CorpID;
        public List<string> Services;
        public double Refine;
        public bool Factory;
        public bool Labs;
        public bool Cloning;

        public ConqStation()
        {
            ID = 0;
            Name = "";
            CorpName = "";
            StnTypeID = 0;
            SSysID = 0;
            CorpID = 0;
            Services = new List<string>();
            Refine = 0;
            Factory = false;
            Labs = false;
            Cloning = false;
        }
    }
}
