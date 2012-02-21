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
// ========================================================================using System;
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
    public class New_Tower
    {
        public Defense Armor, Shield, Struct;
        public CT_Bonus Bonuses;
        public TFuelBay Fuel, D_Fuel, A_Fuel, T_Fuel;
        public long typeID, groupID;
        public string  State, Desc, OtherInfo, Name;
        public string  Location, Category, Low_Fuel;
        public SortedList<string, decimal> Data;
        public ArrayList Extra;

        public New_Tower()
        {
            Armor   = new Defense(0, 0, 0, 0, 0);
            Shield  = new Defense(0, 0, 0, 0, 0);
            Struct  = new Defense(0, 0, 0, 0, 0);
            Bonuses = new CT_Bonus();
            Data = new SortedList<string, decimal>();

            Data.Add("CPU", 0);
            Data.Add("CPU_Used", 0);
            Data.Add("Power", 0);
            Data.Add("Power_Used", 0);
            Data.Add("Sig_Radius", 0);
            Data.Add("Anchor_Time", 0);
            Data.Add("Online_Time", 0);
            Data.Add("UnAnchor_Time", 0);
            Data.Add("Volume", 0);
            Data.Add("Cost", 0);
            Data.Add("Capacity", 0);
            Data.Add("Cycle_Period", 0);
            Data.Add("Design_Interval", 0);
            Data.Add("Design_Interval_Qty", 0);
            Data.Add("Design_Stront_Qty", 0);
            Data.Add("F_RunTime", 0);
            Data.Add("S_RunTime", 0);
            Data.Add("Req_Isotope", 0);   // N2 = 1, O2 = 2, He = 3, H2 = 4
            Data.Add("Block_Qty", 450);   // Isotope Qty to for now anyways track Fuel Block Quantities until the DB is fixed 1/22/12
                       
            typeID = 0;
            groupID = 0;
            State = "Online";  // Online, Offline, Reinforced
            Desc = "";
            OtherInfo = "";
            Name = "";
            Location = "";
            Category = "";
            Low_Fuel = "";

            Fuel = new TFuelBay();
            A_Fuel = new TFuelBay();
            D_Fuel = new TFuelBay();
            T_Fuel = new TFuelBay();

            Extra = new ArrayList();
        }

        public New_Tower(New_Tower t)
        {
            Armor = new Defense(t.Armor);
            Shield = new Defense(t.Shield);
            Struct = new Defense(t.Struct);
            Fuel = new TFuelBay(t.Fuel);
            A_Fuel = new TFuelBay(t.A_Fuel);
            D_Fuel = new TFuelBay(t.D_Fuel);
            T_Fuel = new TFuelBay(t.T_Fuel);

            Bonuses = new CT_Bonus(t.Bonuses);
            typeID = t.typeID;
            groupID = t.groupID;
            State = t.State;  // Online, Offline, Reinforced
            Desc = t.Desc;
            OtherInfo = t.OtherInfo;
            Name = t.Name;

            Data = new SortedList<string, decimal>();
            foreach (var v in t.Data)
                Data.Add(v.Key, v.Value);

            Location = t.Location;
            Category = t.Category;

            Low_Fuel = t.Low_Fuel;

            Extra = new ArrayList();
            foreach (var s in t.Extra)
                Extra.Add(s);
        }

        public New_Tower(Tower t)
        {
            decimal bQty = 40;

            Armor = new Defense(t.Armor);
            Shield = new Defense(t.Shield);
            Struct = new Defense(t.Struct);
            Fuel = new TFuelBay(t.Fuel);
            A_Fuel = new TFuelBay(t.A_Fuel);
            D_Fuel = new TFuelBay(t.D_Fuel);
            T_Fuel = new TFuelBay(t.T_Fuel);

            Bonuses = new CT_Bonus(t.Bonuses);
            typeID = t.typeID;
            groupID = t.groupID;
            State = t.State;  // Online, Offline, Reinforced
            Desc = t.Desc;
            OtherInfo = t.OtherInfo;
            Name = t.Name;

            Data = new SortedList<string, decimal>();
            Data.Add("CPU", t.CPU);
            Data.Add("CPU_Used", t.CPU_Used);
            Data.Add("Power", t.Power);
            Data.Add("Power_Used", t.Power_Used);
            Data.Add("Sig_Radius", t.SigRad);
            Data.Add("Anchor_Time", t.Anchor_Time);
            Data.Add("Online_Time", t.Online_Time);
            Data.Add("UnAnchor_Time", t.UnAnchor_Time);
            Data.Add("Volume", t.Volume);
            Data.Add("Cost", t.Cost);

            if (Name.Contains("Medium"))
                Data.Add("Capacity", 70000);
            else if (Name.Contains("Small"))
                Data.Add("Capacity", 35000);
            else
                Data.Add("Capacity", 140000);

            Data.Add("Cycle_Period", t.Cycle_Period);
            Data.Add("Design_Interval", t.Design_Interval);
            Data.Add("Design_Interval_Qty", t.Design_Int_Qty);
            Data.Add("Design_Stront_Qty", t.Design_Stront_Qty);
            Data.Add("F_RunTime", t.F_RunTime);
            Data.Add("S_RunTime", t.S_RunTime);

            // N2 = 1, O2 = 2, He = 3, H2 = 4
            if (t.Fuel.N2Iso.PeriodQty > 0)
            {
                Data.Add("Req_Isotope", 1);
                Data.Add("Block_Qty", t.Fuel.N2Iso.PeriodQty);   // Isotope Qty to for now anyways
            }
            else if (t.Fuel.O2Iso.PeriodQty > 0)
            {
                Data.Add("Req_Isotope", 2);
                Data.Add("Block_Qty", t.Fuel.O2Iso.PeriodQty);   // Isotope Qty to for now anyways
            }
            else if (t.Fuel.HeIso.PeriodQty > 0)
            {
                Data.Add("Req_Isotope", 3);
                Data.Add("Block_Qty", t.Fuel.HeIso.PeriodQty);   // Isotope Qty to for now anyways
            }
            else if (t.Fuel.H2Iso.PeriodQty > 0)
            {
                Data.Add("Req_Isotope", 4);
                Data.Add("Block_Qty", t.Fuel.H2Iso.PeriodQty);   // Isotope Qty to for now anyways
            }
            else
            {
                Data.Add("Req_Isotope", 0);
                if (Name.Contains("Medium"))
                    Data.Add("Block_Qty", 225);   // Isotope Qty to for now anyways
                else if(Name.Contains("Small"))
                    Data.Add("Block_Qty", 113);   // Isotope Qty to for now anyways
                else 
                    Data.Add("Block_Qty", 450);   // Isotope Qty to for now anyways                   
            }

            bQty = ComputeBlocksForTower(Data["Block_Qty"]);
            Fuel.FuelCap = Data["Capacity"];
            Fuel.Blocks.APIPerQty = bQty;
            Fuel.Blocks.BaseQty = bQty;
            Fuel.Blocks.PeriodQty = bQty;
            A_Fuel.FuelCap = Data["Capacity"];
            A_Fuel.Blocks.APIPerQty = bQty;
            A_Fuel.Blocks.BaseQty = bQty;
            A_Fuel.Blocks.PeriodQty = bQty;
            D_Fuel.FuelCap = Data["Capacity"];
            D_Fuel.Blocks.APIPerQty = bQty;
            D_Fuel.Blocks.BaseQty = bQty;
            D_Fuel.Blocks.PeriodQty = bQty;
            T_Fuel.FuelCap = Data["Capacity"];
            T_Fuel.Blocks.APIPerQty = bQty;
            T_Fuel.Blocks.BaseQty = bQty;
            T_Fuel.Blocks.PeriodQty = bQty;

            Location = t.Location;
            Category = t.Category;

            Low_Fuel = t.Low_Fuel;

            Extra = new ArrayList();
            foreach (var s in t.Extra)
                Extra.Add(s);
        }

        public decimal ComputeBlocksForTower(decimal bVal)
        {
            switch ((int)bVal)
            {
                case 450:
                    return 40;
                case 383:
                    return 36;
                case 338:
                    return 32;
                case 225:
                    return 20;
                case 192:
                    return 18;
                case 169:
                    return 16;
                case 113:
                    return 10;
                case 97:
                    return 9;
                case 85:
                    return 8;
                default:
                    return 40;
            }
        }

    }
}
