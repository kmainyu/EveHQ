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

namespace EveHQ.PosManager
{
    [Serializable]
    public class POS
    {
        // For NEW data variable additions, go to: UpdatePOSDesignData in POSDesigns.cs for load time addition and setup
        public Tower PosTower;
        public ArrayList Modules;
        public DateTime Fuel_TS, Stront_TS, API_TS, React_TS;
        public ArrayList Extra;
        public ArrayList ReactionLinks;

        public string Name, System, CorpName, Moon;
        public long itemID, locID, corpID;
        public int SovLevel;
        public bool Monitored;
        public bool FillCheck;
        public bool UseChart;
        public string Owner, FuelTech;
        public decimal ownerID, fuelTechID;
    
        public POS()
        {
            PosTower = new Tower();
            Name = "";
            System = "";
            CorpName = "";
            Moon = "";
            itemID = 0;
            locID = 0;
            corpID = 0;
            SovLevel = 0;
            Owner = "";
            FuelTech = "";
            ownerID = 0;
            fuelTechID = 0;
            Modules = new ArrayList();
            Monitored = false;
            Fuel_TS = DateTime.Now;
            Stront_TS = DateTime.Now;
            API_TS = DateTime.Now;
            React_TS = DateTime.Now;
            Extra = new ArrayList();
            FillCheck = false;
            UseChart = false;
            ReactionLinks = new ArrayList();
        }

        public POS(string nm)
        {
            PosTower = new Tower();
            Name = nm;
            System = "";
            CorpName = "";
            Moon = "";
            itemID = 0;
            locID = 0;
            SovLevel = 0;
            corpID = 0;
            Owner = "";
            FuelTech = "";
            ownerID = 0;
            fuelTechID = 0;
            Modules = new ArrayList();
            Monitored = false;
            Fuel_TS = DateTime.Now;
            Stront_TS = DateTime.Now;
            API_TS = DateTime.Now;
            React_TS = DateTime.Now;
            Extra = new ArrayList();
            FillCheck = false;
            UseChart = false;
            ReactionLinks = new ArrayList();
        }

        public POS(POS p)
        {
            PosTower = new Tower(p.PosTower);
            Name = p.Name;
            System = p.System;
            CorpName = p.CorpName;
            Moon = p.Moon;
            itemID = p.itemID;
            locID = p.locID;
            corpID = p.corpID;
            Owner = p.Owner;
            FuelTech = p.FuelTech;
            ownerID = p.ownerID;
            fuelTechID = p.fuelTechID;
            SovLevel = p.SovLevel;

            Modules = new ArrayList();
            foreach(Module m in p.Modules)
                Modules.Add(new Module(m));

            Monitored = p.Monitored;
            Fuel_TS = p.Fuel_TS;
            Stront_TS = p.Stront_TS;
            API_TS = p.API_TS;
            React_TS = p.React_TS;
            Extra = new ArrayList();
            foreach (Object o in p.Extra)
                Extra.Add(o);

            FillCheck = p.FillCheck;
            UseChart = p.UseChart;
            if (p.ReactionLinks != null)
            {
                ReactionLinks = new ArrayList();
                foreach (ReactionLink rl in p.ReactionLinks)
                    ReactionLinks.Add(new ReactionLink(rl));
            }
            else
                ReactionLinks = new ArrayList();
        }

        public POS(string nm, POS p)
        {
            PosTower = new Tower(p.PosTower);
            Name = nm;
            System = p.System;
            CorpName = p.CorpName;
            Moon = p.Moon;
            itemID = p.itemID;
            locID = p.locID;
            corpID = p.corpID;
            SovLevel = p.SovLevel;
            Owner = p.Owner;
            FuelTech = p.FuelTech;
            ownerID = p.ownerID;
            fuelTechID = p.fuelTechID;

            Modules = new ArrayList();
            foreach (Module m in p.Modules)
                Modules.Add(new Module(m));

            Monitored = p.Monitored;
            Fuel_TS = p.Fuel_TS;
            Stront_TS = p.Stront_TS;
            API_TS = p.API_TS;
            React_TS = p.React_TS;
            Extra = new ArrayList();
            foreach (Object o in p.Extra)
                Extra.Add(o);

            FillCheck = p.FillCheck;
            UseChart = p.UseChart;
            if (p.ReactionLinks != null)
            {
                ReactionLinks = new ArrayList();
                foreach (ReactionLink rl in p.ReactionLinks)
                    ReactionLinks.Add(new ReactionLink(rl));
            }
            else
                ReactionLinks = new ArrayList();
        }

        public void ClearAllPOSData()
        {
            PosTower = new Tower();
            Name = "";
            System = "";
            CorpName = "";
            Moon = "";
            itemID = 0;
            locID = 0;
            SovLevel = 0;
            corpID = 0;
            Owner = "";
            FuelTech = "";
            ownerID = 0;
            fuelTechID = 0;
            Modules.Clear();
            Monitored = false;
            Fuel_TS = DateTime.Now;
            Stront_TS = DateTime.Now;
            API_TS = DateTime.Now;
            React_TS = DateTime.Now;
            Extra.Clear();
            FillCheck = false;
            UseChart = false;
            ReactionLinks.Clear();
        }

        public void CopyPOSData(POS p)
        {
            PosTower = new Tower(p.PosTower);
            Name = p.Name;
            System = p.System;
            CorpName = p.CorpName;
            Moon = p.Moon;
            itemID = p.itemID;
            locID = p.locID;
            corpID = p.corpID;
            SovLevel = p.SovLevel;
            Owner = p.Owner;
            FuelTech = p.FuelTech;
            ownerID = p.ownerID;
            fuelTechID = p.fuelTechID;

            Modules = new ArrayList();
            foreach (Module m in p.Modules)
                Modules.Add(new Module(m));

            Monitored = p.Monitored;
            Fuel_TS = p.Fuel_TS;
            Stront_TS = p.Stront_TS;
            API_TS = p.API_TS;
            React_TS = p.React_TS;
            Extra = new ArrayList();
            foreach (Object o in p.Extra)
                Extra.Add(o);

            FillCheck = p.FillCheck;
            UseChart = p.UseChart;
            if (p.ReactionLinks != null)
            {
                ReactionLinks = new ArrayList();
                foreach (ReactionLink rl in p.ReactionLinks)
                    ReactionLinks.Add(new ReactionLink(rl));
            }
            else
                ReactionLinks = new ArrayList();
        }

        // Copy over new data and information, but do not copy over link or monitor state information.
        public void ImportPOSData(POS p)
        {
            PosTower = new Tower(p.PosTower);
            System = p.System;
            CorpName = p.CorpName;
            Moon = p.Moon;
            locID = p.locID;
            corpID = p.corpID;
            SovLevel = p.SovLevel;
            Owner = p.Owner;
            FuelTech = p.FuelTech;
            ownerID = p.ownerID;
            fuelTechID = p.fuelTechID;

            Modules = new ArrayList();
            foreach (Module m in p.Modules)
                Modules.Add(new Module(m));

            Fuel_TS = p.Fuel_TS;
            Stront_TS = p.Stront_TS;
            API_TS = p.API_TS;
            React_TS = p.React_TS;
            Extra = new ArrayList();
            foreach (Object o in p.Extra)
                Extra.Add(o);

            FillCheck = p.FillCheck;
            UseChart = p.UseChart;
            if (p.ReactionLinks != null)
            {
                ReactionLinks = new ArrayList();
                foreach (ReactionLink rl in p.ReactionLinks)
                    ReactionLinks.Add(new ReactionLink(rl));
            }
            else
                ReactionLinks = new ArrayList();
        }

        public void RemoveModuleFromPOS(int rowIndex)
        {
            Module m;

            m = (Module)Modules[rowIndex];

            if (m.MSRList.Count > 0)
                ReactionLinks.Clear();

            Modules.RemoveAt(rowIndex);
        }

        public void RemoveTowerFromPOS()
        {
            PosTower = new Tower();
        }

        public decimal GetFuelRunTime(decimal qty, decimal pdQty)
        {
            if (pdQty > 0)
                return (qty / pdQty);
            else
                return 0;
        }

        public decimal GetSovMultiple()
        {
            decimal sov_mult;

            // Note: After Dominion patch, this will be either 1.0 or 0.75
            if (SovLevel == 5)
                sov_mult = 0.75M;
            else if (SovLevel == 0)
                sov_mult = 1.0M;
            else
                sov_mult = 0.75M;

            return sov_mult;
        }

        public void CalculatePOSFuelRunTime(API_List APIL, FuelBay fbay)
        {
            string status;
            DateTime F_TimeStamp, S_TimeStamp, C_TimeStamp, A_TimeStamp;
            DateTime A_In_TimeStamp = DateTime.Now;
            TimeSpan D_TimeStamp, API_TimeSpan;
            APITowerData apid = new APITowerData();
            FuelBay fb = new FuelBay(PosTower.Fuel);
            ArrayList shortRun;
            long F_HourDiff = 0, S_HourDiff = 0;
            long F_DayDiff = 0, F_MinDiff = 0, S_DayDiff = 0, S_MinDiff = 0;
            decimal sov_mult;

            sov_mult = GetSovMultiple();

            C_TimeStamp = DateTime.Now;

            // This function will calculate the fuel run times for the current POS.
            // Get Fuel calc start timestamp
            F_TimeStamp = Fuel_TS;
            S_TimeStamp = Stront_TS;
            A_TimeStamp = API_TS;
            status = PosTower.State;

            // Get monitored POS Fuel Values - ie: Not the design time values
            if (itemID != 0)
            {
                // LINKED POS
                apid = APIL.GetAPIDataMemberForTowerID(itemID);
                if ((apid == null) || (apid.curTime == null))
                    return;

                A_In_TimeStamp = Convert.ToDateTime(apid.curTime);
                A_In_TimeStamp = TimeZone.CurrentTimeZone.ToLocalTime(A_In_TimeStamp);

                if (!A_TimeStamp.Equals(A_In_TimeStamp))
                {
                    // POS is linked, API in tower is NOT Current, use new API Data
                    F_TimeStamp = A_In_TimeStamp;
                    S_TimeStamp = A_In_TimeStamp;

                    PosTower.Fuel.EnrUran.Qty = apid.EnrUr;
                    PosTower.Fuel.Oxygen.Qty = apid.Oxygn;
                    PosTower.Fuel.MechPart.Qty = apid.MechP;
                    PosTower.Fuel.Coolant.Qty = apid.Coolt;
                    PosTower.Fuel.Robotics.Qty = apid.Robot;
                    PosTower.Fuel.HvyWater.Qty = apid.HvyWt;
                    PosTower.Fuel.LiqOzone.Qty = apid.LiqOz;
                    PosTower.Fuel.Charters.Qty = apid.Charters;
                    PosTower.Fuel.N2Iso.Qty = apid.N2Iso;
                    PosTower.Fuel.HeIso.Qty = apid.HeIso;
                    PosTower.Fuel.H2Iso.Qty = apid.H2Iso;
                    PosTower.Fuel.O2Iso.Qty = apid.O2Iso;
                    PosTower.Fuel.Strontium.Qty = apid.Stront;

                    API_TimeSpan = F_TimeStamp.Subtract(A_TimeStamp);

                    if (PosTower.Fuel.EnrUran.LastQty == 0)
                    {
                        PosTower.Fuel.SetLastFuelRead();
                    }
                    else
                    {
                        if ((API_TimeSpan.Hours > 1) || ((API_TimeSpan.Days > 0)))
                        {
                            // We have a data time difference of greater than 1 hour - just leave it alone
                            // and do not update the values.
                        }
                        else if ((API_TimeSpan.Hours == 1) && (API_TimeSpan.Minutes < 5))
                        {
                            // We only want to apply new data if it is reasonably close to 1 hour apart.
                            PosTower.Fuel.SetAPIFuelUsage(sov_mult);
                        }
                        else if ((API_TimeSpan.Minutes > 50) && (API_TimeSpan.Hours == 0))
                        {
                            // We only want to apply new data if it is reasonably close to 1 hour apart.
                            PosTower.Fuel.SetAPIFuelUsage(sov_mult);
                        }
                        PosTower.Fuel.SetLastFuelRead();
                    }
                    PosTower.T_Fuel.CopyLastAndAPI(PosTower.Fuel);
                    PosTower.A_Fuel.CopyLastAndAPI(PosTower.Fuel);
                    PosTower.D_Fuel.CopyLastAndAPI(PosTower.Fuel);
                }
            }

            // Determine the difference in time between the Timestamp values, and current time
            D_TimeStamp = C_TimeStamp.Subtract(F_TimeStamp);
            F_HourDiff = D_TimeStamp.Hours;
            F_DayDiff = D_TimeStamp.Days;
            F_MinDiff = D_TimeStamp.Minutes;
            F_HourDiff += F_DayDiff * 24;
            D_TimeStamp = C_TimeStamp.Subtract(S_TimeStamp);
            S_HourDiff = D_TimeStamp.Hours;
            S_DayDiff = D_TimeStamp.Days;
            S_MinDiff = D_TimeStamp.Minutes;
            S_HourDiff += S_DayDiff * 24;

            if (status == "Offline")
            {
                F_HourDiff = 0;
                S_HourDiff = 0;
            }
            else if (status == "Reinforced")
            {
                if (S_HourDiff > 0)
                {
                    // At least 1 hour has expired, so we can reset the timestamp
                    Stront_TS = C_TimeStamp.Subtract(TimeSpan.FromMinutes(S_MinDiff));
                }
                // Tower is in Reinforced mode, Compute Strontium Run Time
                // A reinforced tower consumes ALL stront for reinforce time at the moment it enters
                // reinforced mode.
                // PosTower.Fuel.DecrementStrontQtyForPeriod(S_HourDiff, sov_mult);
            }

            Fuel_TS = F_TimeStamp;
            API_TS = A_In_TimeStamp;

            if (F_HourDiff > 0)
            {
                // At least 1 hour has expired, so we can reset the timestamp
                Fuel_TS = C_TimeStamp.Subtract(TimeSpan.FromMinutes(F_MinDiff));
            }

            // Determine actual CPU and POWER Fuel qty #s based upon CPU and Power usage of design
            PosTower.Fuel.DecrementFuelQtyForPeriod(F_HourDiff, sov_mult, PosTower.CPU, PosTower.CPU_Used, PosTower.Power, PosTower.Power_Used, UseChart);

            // Calculate Fuel Bay Volume (and Stront) based on Individual Fuel Volumes.
            PosTower.Fuel.SetCurrentFuelVolumes();

            shortRun = PosTower.Fuel.GetShortestFuelRunTimeAndName(PosTower.CPU, PosTower.CPU_Used, PosTower.Power, PosTower.Power_Used, sov_mult, UseChart);

            PosTower.F_RunTime = (decimal)shortRun[0];
            PosTower.Low_Fuel = (string)shortRun[1];
            PosTower.Fuel.SetCurrentFuelCosts(fbay);
            PosTower.Fuel.SetFuelRunTimes(PosTower.CPU, PosTower.CPU_Used, PosTower.Power, PosTower.Power_Used, sov_mult);
        }

        public decimal ComputeMaxPosRunTimeForLoad()
        {
            FuelBay fb = new FuelBay(PosTower.Fuel);
            decimal fuel_cap = 0;
            decimal period = 0;
            decimal sov_mult;

            sov_mult = GetSovMultiple();
            fuel_cap = PosTower.D_Fuel.FuelCap;

            while (fb.FuelUsed < fuel_cap)
            {
                period++;

                // Modify POS Fuel Numbers on amount (multiplicative)
                fb.SetFuelQtyForPeriod(period, sov_mult, PosTower.CPU, PosTower.CPU_Used, PosTower.Power, PosTower.Power_Used, UseChart);

                // Calculate Fuel Bay Volume (and Stront) based on Individual Fuel Volumes.
                fb.SetCurrentFuelVolumes();
            }

            return (period -1);
        }

        public decimal ComputeMaxPosStrontTime()
        {
            FuelBay fb = new FuelBay(PosTower.Fuel);
            decimal str_cap = 0;
            decimal period = 0;
            decimal sov_mult;

            sov_mult = GetSovMultiple();
            str_cap = PosTower.D_Fuel.StrontCap;

            while (fb.StrontUsed < str_cap)
            {
                period++;

                // Modify POS Fuel Numbers on amount (multiplicative)
                fb.SetStrontQtyForPeriod(period, sov_mult);

                // Calculate Fuel Bay Volume (and Stront) based on Individual Fuel Volumes.
                fb.SetCurrentFuelVolumes();
            }

            return (period - 1);
        }
        
        public void CalculatePOSDesignFuelValues(FuelBay fb)
        {
            decimal period, s_period, maxPer;
            decimal sov_mult;

            sov_mult = GetSovMultiple();
            maxPer = ComputeMaxPosRunTimeForLoad();
            switch ((int)PosTower.Design_Interval)
            {
                case 0: // Hours
                    period = PosTower.Design_Int_Qty;
                    if (period > maxPer)
                        period = maxPer;
                    // Adjust quantity if needed for max stability
                    PosTower.Design_Int_Qty = period;
                    break;
                case 1: // Days
                    period = PosTower.Design_Int_Qty * 24;
                    if (period > maxPer)
                        period = maxPer;
                    // Adjust quantity if needed for max stability
                    PosTower.Design_Int_Qty = Math.Floor(period/24);
                    break;
                case 2: // Weeks
                    period = PosTower.Design_Int_Qty * 24 * 7;
                    if (period > maxPer)
                        period = maxPer;
                    // Adjust quantity if needed for max stability
                    PosTower.Design_Int_Qty = Math.Floor(period / (24*7));
                    break;
                case 3: // Maximum Run
                    period = maxPer;
                    // Adjust quantity if needed for max stability
                    PosTower.Design_Int_Qty = period;
                    break;
                default:
                    period = 1;
                    break;
            }

            s_period = PosTower.Design_Stront_Qty;

            // Determine actual CPU and POWER Fuel qty #s based upon CPU and Power usage of design
            PosTower.D_Fuel.SetFuelQtyForPeriod(period, sov_mult, PosTower.CPU, PosTower.CPU_Used, PosTower.Power, PosTower.Power_Used, UseChart);
            PosTower.D_Fuel.SetStrontQtyForPeriod(s_period, sov_mult);
          
            // Set Fuel Run Times
            PosTower.D_Fuel.EnrUran.RunTime = period;
            PosTower.D_Fuel.Oxygen.RunTime = period;
            PosTower.D_Fuel.MechPart.RunTime = period;
            PosTower.D_Fuel.Coolant.RunTime = period;
            PosTower.D_Fuel.Robotics.RunTime = period;
            PosTower.D_Fuel.HvyWater.RunTime = period;
            PosTower.D_Fuel.LiqOzone.RunTime = period;
            PosTower.D_Fuel.Charters.RunTime = period;
            PosTower.D_Fuel.N2Iso.RunTime = period;
            PosTower.D_Fuel.HeIso.RunTime = period;
            PosTower.D_Fuel.H2Iso.RunTime = period;
            PosTower.D_Fuel.O2Iso.RunTime = period;
            PosTower.D_Fuel.Strontium.RunTime = s_period;

            // Calculate Fuel Bay Volume (and Stront) based on Individual Fuel Volumes.
            PosTower.D_Fuel.SetCurrentFuelVolumes();
            PosTower.D_Fuel.SetCurrentFuelCosts(fb);
        }

        public void CalculatePOSAdjustRunTime(FuelBay fbay, FuelBay nud_fuel)
        {
            decimal sov_mult;

            PosTower.A_Fuel = new FuelBay(PosTower.Fuel);

            sov_mult = GetSovMultiple();

            PosTower.A_Fuel.SetFuelQty(nud_fuel);

            // Compute Fuel Run Times
            PosTower.A_Fuel.SetFuelRunTimes(PosTower.CPU, PosTower.CPU_Used, PosTower.Power, PosTower.Power_Used, sov_mult);

            // Calculate Fuel Bay Volume (and Stront) based on Individual Fuel Volumes.
            PosTower.A_Fuel.SetCurrentFuelVolumes();
            PosTower.A_Fuel.SetCurrentFuelCosts(fbay);
        }

        public decimal ComputeMaxPosRunTimeForPeriod(decimal per)
        {
            decimal fuel_use = 0, fuel_cap = 0;
            decimal period = 0;
            decimal sov_mult;
            FuelBay fb = new FuelBay(PosTower.D_Fuel);

            sov_mult = GetSovMultiple();

            fuel_cap = PosTower.D_Fuel.FuelCap;

            while ((fuel_use < fuel_cap) && (period < per))
            {
                period++;

                fb.SetFuelQtyForPeriod(period, sov_mult, PosTower.CPU, PosTower.CPU_Used, PosTower.Power, PosTower.Power_Used, UseChart);
                fb.SetCurrentFuelVolumes();

                // Calculate Fuel Bay Volume (and Stront) based on Individual Fuel Volumes.
                fuel_use = fb.FuelUsed;
            }

            return (period - 1);
        }

        public FuelBay ComputeMaxPosRunForVolume(decimal vol)
        {
            decimal fuel_use = 0, fuel_cap = 0;
            decimal period = 0;
            decimal sov_mult;
            FuelBay fb = new FuelBay(PosTower.D_Fuel);
            FuelBay retFB = new FuelBay();

            sov_mult = GetSovMultiple();

            fuel_cap = PosTower.D_Fuel.FuelCap;

            while (fuel_use < vol)
            {
                period++;

                fb.SetFuelQtyForPeriod(period, sov_mult, PosTower.CPU, PosTower.CPU_Used, PosTower.Power, PosTower.Power_Used, UseChart);
                fb.SubtractZeroMin(PosTower.Fuel);
                fb.SetCurrentFuelVolumes();

                // Calculate Fuel Bay Volume (and Stront) based on Individual Fuel Volumes.
                fuel_use = fb.FuelUsed;

                if (fuel_use <= vol)
                {
                    retFB = new FuelBay(fb);
                    retFB.Extra = new ArrayList();
                    retFB.Extra.Add(period);
                }
            }

            return retFB;
        }

        public decimal ComputePosFuelUsageForFillTracking(long period, decimal value, FuelBay fb)
        {
            decimal run_time, run_perd;
            decimal sov_mult;

            sov_mult = GetSovMultiple();

            switch (period)
            {
                case 0:     // Hours
                    run_perd = value;
                    run_time = run_perd;
                    break;
                case 1:     // Days
                    run_perd = 24 * value;
                    run_time = run_perd;
                    break;
                case 2:     // Weeks
                    run_perd = (24 * 7) * value;
                    run_time = run_perd;
                    break;
                case 3:     // Months
                    run_perd = (24 * 30) * value;
                    run_time = run_perd;
                    break;
                case 4:     // Fill
                    run_perd = 9999;
                    run_time = ComputeMaxPosRunTimeForPeriod(run_perd);
                    break;
                default:
                    run_perd = 9999;
                    run_time = run_perd;
                    break;
            }

            // 3. Compute fuel vols, etc for the period
            if (PosTower.T_Fuel == null)
                PosTower.T_Fuel = new FuelBay();

            PosTower.T_Fuel.SetFuelQtyForPeriod(run_time, sov_mult, PosTower.CPU, PosTower.CPU_Used, PosTower.Power, PosTower.Power_Used, UseChart);
            PosTower.T_Fuel.SetStrontQtyForPeriodOrMax(run_time, sov_mult);

            // Modify numbers based upon current bay quantities
            PosTower.T_Fuel.SubtractFuelQty(PosTower.Fuel);

            // Calculate Fuel Bay Volume 
            PosTower.T_Fuel.SetCurrentFuelVolumes();

            // Set the Fuel Bay Costs
            PosTower.T_Fuel.SetCurrentFuelCosts(fb);

            return run_time;
        }

        public decimal ComputePosFuelNeedForFillTracking(long period, decimal value, FuelBay fb)
        {
            decimal run_time, run_perd;
            decimal sov_mult;

            sov_mult = GetSovMultiple();

            switch (period)
            {
                case 0:     // Hours
                    run_perd = value;
                    run_time = run_perd;
                    break;
                case 1:     // Days
                    run_perd = 24 * value;
                    run_time = run_perd;
                    break;
                case 2:     // Weeks
                    run_perd = (24 * 7) * value;
                    run_time = run_perd;
                    break;
                case 3:     // Months
                    run_perd = (24 * 30) * value;
                    run_time = run_perd;
                    break;
                case 4:     // Fill
                    run_perd = 9999;
                    run_time = ComputeMaxPosRunTimeForPeriod(run_perd);
                    break;
                default:
                    run_perd = 9999;
                    run_time = run_perd;
                    break;
            }

            // 3. Compute fuel vols, etc for the period
            if (PosTower.T_Fuel == null)
                PosTower.T_Fuel = new FuelBay();

            PosTower.T_Fuel.SetFuelQtyForPeriod(run_time, sov_mult, PosTower.CPU, PosTower.CPU_Used, PosTower.Power, PosTower.Power_Used, UseChart);
            PosTower.T_Fuel.SetStrontQtyForPeriodOrMax(run_time, sov_mult);

            // Modify numbers based upon current bay quantities
            //PosTower.T_Fuel.SubtractFuelQty(PosTower.Fuel);

            // Calculate Fuel Bay Volume 
            PosTower.T_Fuel.SetCurrentFuelVolumes();

            // Set the Fuel Bay Costs
            PosTower.T_Fuel.SetCurrentFuelCosts(fb);

            return run_time;
        }

        public bool RemoveQtyAndVolFromModuleID(decimal ID, decimal qty, decimal vol)
        {
            bool retVal = false;

            foreach (Module m in Modules)
            {
                if (m.ModuleID == ID)
                {
                    if (m.State == "Online")
                    {
                        if (m.CapQty >= qty)
                        {
                            // If dest module is not a reaction type, then increment qty values
                            m.CapQty -= (qty);
                            m.CapVol -= (vol);
                            retVal = true;
                        }
                    }
                    break;
                }
            }

            return retVal;
        }

        public bool MoveQtyAndVolToModuleID(decimal ID, decimal qty, decimal vol)
        {
            bool retVal = false;

            foreach (Module m in Modules)
            {
                if (m.ModuleID == ID)
                {
                    if (m.State == "Online")
                    {
                        if (IsModuleReactionType(Convert.ToInt32(m.ModType)))
                        {
                            // If module is reaction type, just return true - src needs to decrement
                            retVal = true;
                        }
                        else if (m.MaxQty >= (m.CapQty + qty))
                        {
                            // If dest module is not a reaction type, then increment qty values
                            m.CapQty += (qty);
                            m.CapVol += (vol);
                            retVal = true;
                        }
                    }
                    break;
                }
            }

            return retVal;
        }

        public bool DoesModuleHaveQtyToMove(decimal ID, decimal qty)
        {
            foreach (Module m in Modules)
            {
                if (m.ModuleID == ID)
                {
                    if (m.State == "Online")
                    {
                        if (m.CapQty >= qty)
                            return true;
                        else if (IsModuleReactionType(Convert.ToInt32(m.ModType)))
                            return true;
                        else if (IsModuleHarvestorType(Convert.ToInt32(m.ModType)))
                            return true;
                        else
                            return false;
                    }
                }
            }
            return false;
        }
        
        public bool DoesModuleHaveRoomToMoveQty(decimal ID, decimal qty)
        {
            foreach (Module m in Modules)
            {
                if (m.ModuleID == ID)
                {
                    if (m.State == "Online")
                    {
                        if ((m.CapQty+qty) <= m.MaxQty)
                            return true;
                        else
                            return false;
                    }
                }
            }
            return false;
        }

        public bool IsModuleReactionType(long mType)
        {
            switch (mType)
            {
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsModuleHarvestorType(long mType)
        {
            switch (mType)
            {
                case 1:
                    return true;
                default:
                    return false;
            }
        }

        // To calculate reactions, I need to do the following:
        // 1. Move mats from Junction Modules to Silo's
        // 2. Move mats from Harvestors to Junction/Silo/Reaction
        // 3. Move mats from Reaction to Junction/Silo
        // 4. Move mats from Silo to Reaction
        public bool CalculateReactions()
        {
            DateTime C_TimeStamp;
            TimeSpan D_TimeStamp;
            long hours;
            bool changed = false;
            bool reacIn = false;
            bool reacOut = false;

            // I now have some additional info possible from a link
            // Check for linked module, and if linked use the module link timestamp instead

            C_TimeStamp = DateTime.Now;
            D_TimeStamp = C_TimeStamp.Subtract(React_TS);

            if (D_TimeStamp.Hours > 0)
            {
                // It has been at least an hour since the last update - set quantity values appropriately
                // Store hours expired
                hours = D_TimeStamp.Hours;
                hours += (D_TimeStamp.Days * 24);

                // Set current React_TS to the correct new value (ie: pull hours, keep minutes and seconds)
                React_TS = DateTime.Now.Subtract(new TimeSpan(0, D_TimeStamp.Minutes, D_TimeStamp.Seconds));

                // For each hour of expired time, do:
                for (int ct = 0; ct < hours; ct++)
                {
                    // 1. Move mats from Junction Modules to Silo's or Reactions
                    foreach (Module m in Modules)
                    {
                        if ((Convert.ToInt64(m.ModType)) == 10)
                        {
                            // Junction, need to move materials from here to downstream Silo/Etc if present and has room
                            if (m.State == "Online")
                            {
                                if (m.Extra.Count > 0)
                                {
                                    // Linked to API
                                }
                                // Search ReactionLinks for this module being the input ID (only 1 is valid)
                                foreach (ReactionLink rl in ReactionLinks)
                                {
                                    if (rl.InpID == m.ModuleID)
                                    {
                                        if (MoveQtyAndVolToModuleID(rl.OutID, m.CapQty, m.CapVol))
                                        {
                                            // Qty added to output, subtract from current input
                                            m.CapQty = 0;
                                            m.CapVol = 0;
                                            changed = true;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 2. Move mats from Harvestors to Junction/Silo/Reaction
                    foreach (Module m in Modules)
                    {
                        if ((Convert.ToInt64(m.ModType)) == 1)
                        {
                            // Harvestor, need to move materials from here to downstream Silo/Etc if present and has room
                            if (m.State == "Online")
                            {
                                // Search ReactionLinks for this module being the input ID (only 1 is valid)
                                foreach (ReactionLink rl in ReactionLinks)
                                {
                                    if (rl.InpID == m.ModuleID)
                                    {
                                        // Output can be:
                                        // 1. Junction, Silo or Reaction
                                        if (MoveQtyAndVolToModuleID(rl.OutID, rl.XferQty, rl.XferVol))
                                        {
                                            // Qty added to output -- since we are a harvestor, no need to
                                            // do anything for current module qty values
                                            changed = true;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 3. Move mats to & from Reaction for Junction/Silo/Reaction
                    foreach (Module m in Modules)
                    {
                        if (IsModuleReactionType(Convert.ToInt32(m.ModType)))
                        {
                            // Reaction, need to move materials from here to downstream Junction/Silo/Etc if present and has room
                            if (m.State == "Online")
                            {
                                reacIn = true;
                                reacOut = true;

                                // Search ReactionLinks for this module being the Output ID 
                                foreach (ReactionLink rl in ReactionLinks)
                                {
                                    if (rl.OutID == m.ModuleID)
                                    {
                                        reacIn = DoesModuleHaveQtyToMove(rl.InpID, rl.XferQty);

                                        if (!reacIn)
                                            break;
                                    }
                                }

                                // Search ReactionLinks for this module being the input ID
                                foreach (ReactionLink rl in ReactionLinks)
                                {
                                    if (rl.InpID == m.ModuleID)
                                    {
                                        reacOut = DoesModuleHaveRoomToMoveQty(rl.OutID, rl.XferQty);

                                        if (!reacOut)
                                            break;
                                    }
                                }

                                if (reacIn && reacOut)
                                {
                                    // Inputs here
                                    // 1. Junction, Silo or Reaction
                                    foreach (ReactionLink rl in ReactionLinks)
                                    {
                                        if (rl.OutID == m.ModuleID)
                                        {
                                            if (RemoveQtyAndVolFromModuleID(rl.InpID, rl.XferQty, rl.XferVol))
                                            {
                                                // Qty added to output -- since we are a Reactor, no need to
                                                // do anything for current module qty values
                                                changed = true;
                                            }
                                        }

                                        if (rl.InpID == m.ModuleID)
                                        {
                                            // Outputs here
                                            // 1. Junction, Silo or Reaction
                                            if (MoveQtyAndVolToModuleID(rl.OutID, rl.XferQty, rl.XferVol))
                                            {
                                                // Qty added to output -- since we are a Reactor, no need to
                                                // do anything for current module qty values
                                                changed = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return changed;
       }

    }
}
