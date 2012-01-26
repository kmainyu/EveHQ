// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2012  EveHQ Development Team
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

namespace EveHQ.PosManager
{
    [Serializable]
    public class ConfigData
    {
        public TFuelBay FuelCosts;
        public long SortedColumnIndex;
        public int MonSelIndex;
        public int FuelCat;
        public int AutoAPI;
        public SortOrder MonSortOrder;
        public ArrayList Extra;
        public string SelPos;
        public bool malongChart, malongStront, noNegs;
        public ArrayList dgMonBool, dgDesBool;
        public decimal malongTP, malongPV;
        // Extra - Storage and Values

        public ConfigData()
        {
            FuelCosts = new TFuelBay();
            Extra = new ArrayList();
            dgMonBool = new ArrayList();
            dgDesBool = new ArrayList();
            SortedColumnIndex = 3;
            MonSelIndex = 0;
            SelPos = "";
            MonSortOrder = SortOrder.Ascending;
            malongChart = true;
            malongStront = true;
            noNegs = false;
            malongTP = 0;
            malongPV = 1;
            FuelCat = 0;
            AutoAPI = 0;
        }
    }
}
