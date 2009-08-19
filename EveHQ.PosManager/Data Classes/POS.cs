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
        public Tower PosTower;
        public ArrayList Modules;
        public DateTime Fuel_TS, Stront_TS, API_TS, React_TS;
        public ArrayList Extra;
        public ArrayList ReactionLinks;

        public string Name, System, CorpName, Moon;
        public int itemID, locID, SovLevel, corpID;
        public bool Monitored;
        public bool FillCheck;
        public bool UseChart;
    
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
            SovLevel = p.SovLevel;
            Modules = new ArrayList(p.Modules);
            Monitored = p.Monitored;
            Fuel_TS = p.Fuel_TS;
            Stront_TS = p.Stront_TS;
            API_TS = p.API_TS;
            React_TS = p.React_TS;
            Extra = new ArrayList(p.Extra);
            FillCheck = p.FillCheck;
            UseChart = p.UseChart;
            if(p.ReactionLinks != null)
                ReactionLinks = new ArrayList(p.ReactionLinks);
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
            Modules = new ArrayList(p.Modules);
            Monitored = p.Monitored;
            Fuel_TS = p.Fuel_TS;
            Stront_TS = p.Stront_TS;
            API_TS = p.API_TS;
            React_TS = p.React_TS;
            Extra = new ArrayList(p.Extra);
            FillCheck = p.FillCheck;
            UseChart = p.UseChart;
            if (p.ReactionLinks != null)
                ReactionLinks = new ArrayList(p.ReactionLinks);
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
            Modules = new ArrayList(p.Modules);
            Monitored = p.Monitored;
            Fuel_TS = p.Fuel_TS;
            Stront_TS = p.Stront_TS;
            API_TS = p.API_TS;
            React_TS = p.React_TS;
            Extra = new ArrayList(p.Extra);
            FillCheck = p.FillCheck;
            UseChart = p.UseChart;
            if (p.ReactionLinks != null)
                ReactionLinks = new ArrayList(p.ReactionLinks);
            else
                ReactionLinks = new ArrayList();
        }

        public void RemoveModuleFromPOS(int rowIndex)
        {
            Module m;

            m = (Module)Modules[rowIndex];
            foreach (ReactionLink rl in ReactionLinks)
            {
                if (rl.InpID == m.ModuleID)
                {
                    ReactionLinks.Remove(rl);
                    break;
                }
            }
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

            if (SovLevel == 5)
                sov_mult = 0.7M;
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
            TimeSpan D_TimeStamp;
            API_Data apid = new API_Data();
            FuelBay fb = new FuelBay(PosTower.Fuel);
            ArrayList shortRun;
            int F_HourDiff = 0, S_HourDiff = 0;
            int F_DayDiff = 0, F_MinDiff = 0, S_DayDiff = 0, S_MinDiff = 0;
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
                A_In_TimeStamp = Convert.ToDateTime(apid.cacheUntil);

                // Now, while this is accurate, need to remove 24 hours from the time
                // to make it work correctly
                A_In_TimeStamp = A_In_TimeStamp.Subtract(TimeSpan.FromHours(24));
                A_In_TimeStamp = TimeZone.CurrentTimeZone.ToLocalTime(A_In_TimeStamp);

                if (!A_TimeStamp.Equals(A_In_TimeStamp))
                {
                    // POS is linked, API is NOT Current, use API Data
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
                PosTower.Fuel.DecrementStrontQtyForPeriod(S_HourDiff, sov_mult);
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
                fb.SetFuelQtyForPeriod(period, sov_mult, PosTower.CPU, PosTower.CPU_Used, PosTower.Power, PosTower.Power_Used);

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
            PosTower.D_Fuel.SetFuelQtyForPeriod(period, sov_mult, PosTower.CPU, PosTower.CPU_Used, PosTower.Power, PosTower.Power_Used);
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

                fb.SetFuelQtyForPeriod(period, sov_mult, PosTower.CPU, PosTower.CPU_Used, PosTower.Power, PosTower.Power_Used);
                fb.SetCurrentFuelVolumes();

                // Calculate Fuel Bay Volume (and Stront) based on Individual Fuel Volumes.
                fuel_use = fb.FuelUsed;
            }

            return (period - 1);
        }

        public decimal ComputePosFuelUsageForFillTracking(int period, decimal value, FuelBay fb)
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

            PosTower.T_Fuel.SetFuelQtyForPeriod(run_time, sov_mult, PosTower.CPU, PosTower.CPU_Used, PosTower.Power, PosTower.Power_Used);
            PosTower.T_Fuel.SetStrontQtyForPeriodOrMax(run_time, sov_mult);

            // Modify numbers based upon current bay quantities
            PosTower.T_Fuel.SubtractFuelQty(PosTower.Fuel);

            // Calculate Fuel Bay Volume 
            PosTower.T_Fuel.SetCurrentFuelVolumes();

            // Set the Fuel Bay Costs
            PosTower.T_Fuel.SetCurrentFuelCosts(fb);

            return run_time;
        }

        public decimal ComputePosFuelNeedForFillTracking(int period, decimal value, FuelBay fb)
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

            PosTower.T_Fuel.SetFuelQtyForPeriod(run_time, sov_mult, PosTower.CPU, PosTower.CPU_Used, PosTower.Power, PosTower.Power_Used);
            PosTower.T_Fuel.SetStrontQtyForPeriodOrMax(run_time, sov_mult);

            // Modify numbers based upon current bay quantities
            //PosTower.T_Fuel.SubtractFuelQty(PosTower.Fuel);

            // Calculate Fuel Bay Volume 
            PosTower.T_Fuel.SetCurrentFuelVolumes();

            // Set the Fuel Bay Costs
            PosTower.T_Fuel.SetCurrentFuelCosts(fb);

            return run_time;
        }

        public void CalculateReactions()
        {
            DateTime C_TimeStamp;
            TimeSpan D_TimeStamp;
            int hours;
            bool inpValid, outValid;

            C_TimeStamp = DateTime.Now;
            D_TimeStamp = C_TimeStamp.Subtract(React_TS);

            if (D_TimeStamp.Hours > 0)
            {
                // It has been at least an hour since the last update - set quantity values appropriately
                // Store hours expired
                hours = D_TimeStamp.Hours;
                // Set current React_TS to the correct new value
                React_TS = DateTime.Now.Subtract(new TimeSpan(0, D_TimeStamp.Minutes, D_TimeStamp.Seconds));

                // Check each module to see if it is a reaction or harvester module
                for (int ct = 0; ct < hours; ct++)
                {
                    foreach (Module m in Modules)
                    {
                        switch (Convert.ToInt64(m.ModType))
                        {
                            case 1:
                                // Moon Miner / Harvestor - Verify output link exists and has room
                                if (m.State == "Online")
                                {
                                    // Search ReactionLinks for this module being the input ID (only 1 is valid)
                                    foreach (ReactionLink rl in ReactionLinks)
                                    {
                                        if (rl.InpID == m.ModuleID)
                                        {
                                            // Found link, now validate output module has room
                                            foreach (Module om in Modules)
                                            {
                                                if (om.ModuleID == rl.OutID)
                                                {
                                                    if ((om.MaxQty >= (om.CapQty + rl.XferQty)) && (om.State == "Online"))
                                                    {
                                                        // We have room - move it now!
                                                        om.CapQty += (rl.XferQty);
                                                        om.CapVol += (rl.XferVol);
                                                    }
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                                break;
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                // Reaction Module
                                if (m.State == "Online")
                                {
                                    // Verify Input Link(s) are Online, and Have Required Qty Amounts available
                                    inpValid = false;
                                    foreach (InOutData iod in m.selReact.inputs)
                                    {
                                        // For each reaction input
                                        foreach (ReactionLink rl in ReactionLinks)
                                        {
                                            // Verify the input module for the Reaction Link
                                            // Is Online, and has enough minerals to satisfy the reaction
                                            if (m.ModuleID == rl.OutID)
                                            {
                                                foreach (Module im in Modules)
                                                {
                                                    if (im.ModuleID == rl.InpID)
                                                    {
                                                        if (im.State == "Online")
                                                        {
                                                            if (im.CapQty >= rl.XferQty)
                                                                inpValid = true;
                                                            else
                                                                inpValid = false;
                                                        }
                                                        else
                                                            inpValid = false;
                                                        break;
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                        if (!inpValid)
                                            break;
                                    }

                                    if (!inpValid) // Not all inputs are available - get out
                                        return;

                                    outValid = false;

                                    // Verify Output Link(s) are Online, and Have Required Qty Room available
                                    foreach (InOutData iod in m.selReact.outputs)
                                    {
                                        foreach (ReactionLink rl in ReactionLinks)
                                        {
                                            if (m.ModuleID == rl.InpID)
                                            {
                                                foreach (Module om in Modules)
                                                {
                                                    if (om.ModuleID == rl.OutID)
                                                    {
                                                        if (om.State == "Online")
                                                        {
                                                            // I may need a special handler for Junction Modules here
                                                            if ((om.CapQty + rl.XferQty) <= om.MaxQty)
                                                                outValid = true;
                                                            else
                                                                outValid = false;

                                                            if (!outValid)
                                                            {
                                                                // If Junction
                                                                if (om.ModType == 10)
                                                                {
                                                                    foreach (ReactionLink rlo in ReactionLinks)
                                                                    {
                                                                        if (rlo.InpID == om.ModuleID)
                                                                        {
                                                                            foreach (Module jom in Modules)
                                                                            {
                                                                                if (jom.ModuleID == rlo.OutID)
                                                                                {
                                                                                    switch (Convert.ToInt64(jom.ModType))
                                                                                    {
                                                                                        case 2:
                                                                                        case 8:
                                                                                        case 9:
                                                                                        case 11:
                                                                                        case 12:
                                                                                        case 13:
                                                                                            if ((jom.CapQty + rl.XferQty) >= jom.MaxQty)
                                                                                                outValid = true;
                                                                                            break;
                                                                                        default:
                                                                                            outValid = false;
                                                                                            break;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                            outValid = false;
                                                        break;
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                        if (!outValid)
                                            break;
                                    }

                                    if (!outValid) // An output location cannot accept the Qty - leave here
                                        return;

                                    // Now repeat above while changing Qty/Vol values
                                    foreach (InOutData iod in m.selReact.inputs)
                                    {
                                        foreach (ReactionLink rl in ReactionLinks)
                                        {
                                            if (m.ModuleID == rl.OutID)  // Reaction link input is this module
                                            {
                                                foreach (Module im in Modules)
                                                {
                                                    if (im.ModuleID == rl.InpID)
                                                    {
                                                        if (iod.typeID == im.selMineral.typeID)
                                                        {
                                                            im.CapQty -= rl.XferQty;
                                                            im.CapVol -= rl.XferVol;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    foreach (InOutData iod in m.selReact.outputs)
                                    {
                                        foreach (ReactionLink rl in ReactionLinks)
                                        {
                                            if (m.ModuleID == rl.InpID)  // Reaction link output is this module
                                            {
                                                foreach (Module om in Modules)
                                                {
                                                    if (om.ModuleID == rl.OutID)
                                                    {
                                                        if (iod.typeID == om.selMineral.typeID)
                                                        {
                                                            om.CapQty += rl.XferQty;
                                                            om.CapVol += rl.XferVol;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                                break;
                            case 10:
                                // We have a junction module - need to xfer from this module to it's output module
                                // if the output is a Silo. (otherwise the above cases will handle it)
                                if (m.State == "Online")
                                {
                                    // Search ReactionLinks for this module being the input ID (only 1 is valid)
                                    foreach (ReactionLink rl in ReactionLinks)
                                    {
                                        if (rl.InpID == m.ModuleID)
                                        {
                                            // Found link, now validate output module has room
                                            foreach (Module om in Modules)
                                            {
                                                if (om.ModuleID == rl.OutID)
                                                {
                                                    if ((om.MaxQty >= (om.CapQty + rl.XferQty)) && (om.State == "Online"))
                                                    {
                                                        // We have room - move it now!
                                                        if (m.CapQty >= rl.XferQty)
                                                        {
                                                            m.CapQty -= rl.XferQty;
                                                            om.CapQty += rl.XferQty;
                                                            m.CapVol -= rl.XferVol;
                                                            om.CapVol -= rl.XferVol;
                                                        }
                                                    }
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }

                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}
