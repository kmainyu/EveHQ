﻿// ========================================================================
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
    public class Tower
    {
        public Defense Armor, Shield, Struct;
        public CT_Bonus Bonuses;
        public FuelBay Fuel, D_Fuel, A_Fuel, T_Fuel;
        public long typeID, groupID;
        public string  State, Desc, OtherInfo, Name;
        public decimal CPU, Power, CPU_Used, Power_Used, SigRad;
        public decimal Anchor_Time, Online_Time, UnAnchor_Time;
        public decimal Volume, Cost, Capacity, Cycle_Period;
        public decimal Design_Interval, Design_Int_Qty, Design_Stront_Qty;
        public decimal F_RunTime, S_RunTime;
        public string  Location, Category, Low_Fuel;
        public ArrayList Extra;

        public Tower()
        {
            Armor   = new Defense(0, 0, 0, 0, 0);
            Shield  = new Defense(0, 0, 0, 0, 0);
            Struct  = new Defense(0, 0, 0, 0, 0);
            Fuel    = new FuelBay();
            D_Fuel = new FuelBay();
            A_Fuel = new FuelBay();
            T_Fuel = new FuelBay();
            Bonuses = new CT_Bonus();
            typeID = 0;
            groupID = 0;
            State = "Online";  // Online, Offline, Reinforced
            Desc = "";
            OtherInfo = "";
            Name = "";
            CPU = 0;
            CPU_Used = 0;
            Power = 0;
            Power_Used = 0;
            SigRad = 0;
            Anchor_Time = 0;
            Online_Time = 0;
            UnAnchor_Time = 0;
            Volume = 0;
            Cost = 0;
            Capacity = 0;
            Cycle_Period = 0;
            Design_Interval = 0;
            Design_Int_Qty = 0;
            Location = "";
            Category = "";
            Extra = new ArrayList();
            F_RunTime = 0;
            S_RunTime = 0;
            Low_Fuel = "";
            Design_Stront_Qty = 0;
        }

        public Tower(Tower t)
        {
            Armor = new Defense(t.Armor);
            Shield = new Defense(t.Shield);
            Struct = new Defense(t.Struct);
            Fuel = new FuelBay(t.Fuel);
            D_Fuel = new FuelBay(t.D_Fuel);
            A_Fuel = new FuelBay(t.A_Fuel);
            T_Fuel = new FuelBay(t.T_Fuel);
            Bonuses = new CT_Bonus(t.Bonuses);
            typeID = t.typeID;
            groupID = t.groupID;
            State = t.State;  // Online, Offline, Reinforced
            Desc = t.Desc;
            OtherInfo = t.OtherInfo;
            Name = t.Name;
            CPU = t.CPU;
            CPU_Used = t.CPU_Used;
            Power = t.Power;
            Power_Used = t.Power_Used;
            SigRad = t.SigRad;
            Anchor_Time = t.Anchor_Time;
            Online_Time = t.Online_Time;
            UnAnchor_Time = t.UnAnchor_Time;
            Volume = t.Volume;
            Cost = t.Cost;
            Capacity = t.Capacity;
            Cycle_Period = t.Cycle_Period;
            Design_Interval = t.Design_Interval;
            Design_Int_Qty = t.Design_Int_Qty;
            Design_Stront_Qty = t.Design_Stront_Qty;
            Location = t.Location;
            Category = t.Category;

            Extra = new ArrayList();
            foreach (object o in t.Extra)
                Extra.Add(o);

            F_RunTime = t.F_RunTime;
            S_RunTime = t.S_RunTime;
            Low_Fuel = t.Low_Fuel;
        }

    }
}
