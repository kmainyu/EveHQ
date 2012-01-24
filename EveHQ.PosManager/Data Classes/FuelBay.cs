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
    public class FuelBay
    {
        public FuelType EnrUran, Oxygen, MechPart, Coolant, Robotics;
        public FuelType HeIso, H2Iso, N2Iso, O2Iso;
        public FuelType HvyWater, LiqOzone, Charters, Strontium;
        public decimal   FuelCap, FuelUsed, StrontCap, StrontUsed, FuelCost;
        public ArrayList Extra;

        public FuelBay()
        {
            EnrUran = new FuelType();
            Oxygen = new FuelType();
            MechPart = new FuelType();
            Coolant = new FuelType();
            Robotics = new FuelType();
            HeIso = new FuelType();
            H2Iso = new FuelType();
            N2Iso = new FuelType();
            O2Iso = new FuelType();
            HvyWater = new FuelType();
            LiqOzone = new FuelType();
            Charters = new FuelType();
            Strontium = new FuelType();
            FuelCap = 0;
            FuelUsed = 0;
            StrontCap = 0;
            StrontUsed = 0;
            Extra = new ArrayList();
        }

        public FuelBay(FuelBay fb)
        {
            EnrUran = new FuelType(fb.EnrUran);
            Oxygen = new FuelType(fb.Oxygen);
            MechPart = new FuelType(fb.MechPart);
            Coolant = new FuelType(fb.Coolant);
            Robotics = new FuelType(fb.Robotics);
            HeIso = new FuelType(fb.HeIso);
            H2Iso = new FuelType(fb.H2Iso);
            N2Iso = new FuelType(fb.N2Iso);
            O2Iso = new FuelType(fb.O2Iso);
            HvyWater = new FuelType(fb.HvyWater);
            LiqOzone = new FuelType(fb.LiqOzone);
            Charters = new FuelType(fb.Charters);
            Strontium = new FuelType(fb.Strontium);
            FuelCap = fb.FuelCap;
            FuelUsed = fb.FuelUsed;
            StrontCap = fb.StrontCap;
            StrontUsed = fb.StrontUsed;
            Extra = new ArrayList(fb.Extra);
        }

        public void SetLastFuelRead()
        {
            // Do all types, although at this time only HW and LO matter
            EnrUran.LastQty = EnrUran.Qty;
            Oxygen.LastQty = Oxygen.Qty;
            MechPart.LastQty = MechPart.Qty;
            Coolant.LastQty = Coolant.Qty;
            Robotics.LastQty = Robotics.Qty;
            HvyWater.LastQty = HvyWater.Qty;
            LiqOzone.LastQty = LiqOzone.Qty;
            Charters.LastQty = Charters.Qty;
            N2Iso.LastQty = N2Iso.Qty;
            H2Iso.LastQty = H2Iso.Qty;
            O2Iso.LastQty = O2Iso.Qty;
            HeIso.LastQty = HeIso.Qty;
            Strontium.LastQty = Strontium.Qty;
        }

        public void SetAPIFuelUsage(decimal sov_mult)
        {
            // Due to the issue of actual times - I must go back to insisting the time values be
            // Do all types, although at this time only HW and LO matter
            // These calculations have been Drastically OFF - there is a hole here somewhere !!!!
            EnrUran.SetAPIFuelUse(sov_mult);
            Oxygen.SetAPIFuelUse(sov_mult);
            MechPart.SetAPIFuelUse(sov_mult);
            Coolant.SetAPIFuelUse(sov_mult);
            Robotics.SetAPIFuelUse(sov_mult);
            HvyWater.SetAPIFuelUse(sov_mult);
            LiqOzone.SetAPIFuelUse(sov_mult);
            Charters.SetAPIFuelUse(sov_mult);
            N2Iso.SetAPIFuelUse(sov_mult);
            HeIso.SetAPIFuelUse(sov_mult);
            H2Iso.SetAPIFuelUse(sov_mult);
            O2Iso.SetAPIFuelUse(sov_mult);
            Strontium.SetAPIFuelUse(sov_mult);
        }

        public void CopyLastAndAPI(FuelBay fb)
        {
            EnrUran.LastQty = fb.EnrUran.LastQty;
            Oxygen.LastQty = fb.Oxygen.LastQty;
            MechPart.LastQty = fb.MechPart.LastQty;
            Coolant.LastQty = fb.Coolant.LastQty;
            Robotics.LastQty = fb.Robotics.LastQty;
            HvyWater.LastQty = fb.HvyWater.LastQty;
            LiqOzone.LastQty = fb.LiqOzone.LastQty;
            Charters.LastQty = fb.Charters.LastQty;
            N2Iso.LastQty = fb.N2Iso.LastQty;
            H2Iso.LastQty = fb.H2Iso.LastQty;
            O2Iso.LastQty = fb.O2Iso.LastQty;
            HeIso.LastQty = fb.HeIso.LastQty;
            Strontium.LastQty = fb.Strontium.LastQty;

            EnrUran.APIPerQty = fb.EnrUran.APIPerQty;
            Oxygen.APIPerQty = fb.Oxygen.APIPerQty;
            MechPart.APIPerQty = fb.MechPart.APIPerQty;
            Coolant.APIPerQty = fb.Coolant.APIPerQty;
            Robotics.APIPerQty = fb.Robotics.APIPerQty;
            HvyWater.APIPerQty = fb.HvyWater.APIPerQty;
            LiqOzone.APIPerQty = fb.LiqOzone.APIPerQty;
            Charters.APIPerQty = fb.Charters.APIPerQty;
            N2Iso.APIPerQty = fb.N2Iso.APIPerQty;
            H2Iso.APIPerQty = fb.H2Iso.APIPerQty;
            O2Iso.APIPerQty = fb.O2Iso.APIPerQty;
            HeIso.APIPerQty = fb.HeIso.APIPerQty;
            Strontium.APIPerQty = fb.Strontium.APIPerQty;
        }

        public void SetFuelItemID(FuelBay fb)
        {
            EnrUran.itemID = fb.EnrUran.itemID;
            Oxygen.itemID = fb.Oxygen.itemID;
            MechPart.itemID = fb.MechPart.itemID;
            Coolant.itemID = fb.Coolant.itemID;
            Robotics.itemID = fb.Robotics.itemID;
            HvyWater.itemID = fb.HvyWater.itemID;
            LiqOzone.itemID = fb.LiqOzone.itemID;
            Charters.itemID = fb.Charters.itemID;
            N2Iso.itemID = fb.N2Iso.itemID;
            H2Iso.itemID = fb.H2Iso.itemID;
            O2Iso.itemID = fb.O2Iso.itemID;
            HeIso.itemID = fb.HeIso.itemID;
            Strontium.itemID = fb.Strontium.itemID;
        }

        public void SetFuelBaseValues(FuelBay fb)
        {
            EnrUran.UpdateBaseValues(fb.EnrUran);
            Oxygen.UpdateBaseValues(fb.Oxygen);
            MechPart.UpdateBaseValues(fb.MechPart);
            Coolant.UpdateBaseValues(fb.Coolant);
            Robotics.UpdateBaseValues(fb.Robotics);
            HvyWater.UpdateBaseValues(fb.HvyWater);
            LiqOzone.UpdateBaseValues(fb.LiqOzone);
            Charters.UpdateBaseValues(fb.Charters);
            N2Iso.UpdateBaseValues(fb.N2Iso);
            H2Iso.UpdateBaseValues(fb.H2Iso);
            O2Iso.UpdateBaseValues(fb.O2Iso);
            HeIso.UpdateBaseValues(fb.HeIso);
            Strontium.UpdateBaseValues(fb.Strontium);
        }

        private decimal GetFuelItemCost(string id, int cat)
        {
            decimal fCost = 0;

            switch(cat)
            {
                case 0:
                    fCost = Convert.ToDecimal(EveHQ.Core.DataFunctions.GetPrice(id));   // Custom > Market > Default
                    break;
                case 1:
                    if (EveHQ.Core.HQ.MarketPriceList.ContainsKey(id))
                        fCost = Convert.ToDecimal(EveHQ.Core.HQ.MarketPriceList[id]);       // Market Price Only
                    break;
            }
            return fCost;
        }

        public void SetCurrentFuelCosts(TFuelBay fb)
        {
            decimal fCost;
            int cat;

            cat = PlugInData.Config.data.FuelCat;

            // Fuel Blocks - Placeholder

            //fCost = GetFuelItemCost("44", cat);
            //if (fb.EnrUran.Cost > 0)
            //    EnrUran.CostForQty = (EnrUran.Qty * fb.EnrUran.Cost);
            //else
            //    EnrUran.CostForQty = (EnrUran.Qty * fCost);
            //FuelCost = EnrUran.CostForQty;

            fCost = GetFuelItemCost(Charters.itemID, cat);
            if (fb.Charters.Cost > 0)
                Charters.CostForQty = (Charters.Qty * fb.Charters.Cost);
            else
            {
                Charters.CostForQty = (Charters.Qty * fCost);
            }
            FuelCost += Charters.CostForQty;

            fCost = GetFuelItemCost("16275", cat);
            if (fb.Strontium.Cost > 0)
                Strontium.CostForQty = (Strontium.Qty * fb.Strontium.Cost);
            else
                Strontium.CostForQty = (Strontium.Qty * fCost);
        }

        public decimal GetVolumeForFuel(decimal qty, decimal vol, decimal qtyVol)
        {
            decimal retVal;

            if (qtyVol > 0)
                retVal = ((qty / qtyVol) * vol);
            else
                retVal = 0;

            FuelUsed += retVal;
            return retVal;
        }

        public void SetCurrentFuelVolumes()
        {
            FuelUsed = 0;
            // Calculate Fuel Bay Volume 
            EnrUran.VolForQty = GetVolumeForFuel(EnrUran.Qty, PlugInData.BFStats.EnrUran.BaseVol, PlugInData.BFStats.EnrUran.QtyVol);
            Oxygen.VolForQty = GetVolumeForFuel(Oxygen.Qty, PlugInData.BFStats.Oxygen.BaseVol, PlugInData.BFStats.Oxygen.QtyVol);
            MechPart.VolForQty = GetVolumeForFuel(MechPart.Qty, PlugInData.BFStats.MechPart.BaseVol, PlugInData.BFStats.MechPart.QtyVol);
            Coolant.VolForQty = GetVolumeForFuel(Coolant.Qty, PlugInData.BFStats.Coolant.BaseVol, PlugInData.BFStats.Coolant.QtyVol);
            Robotics.VolForQty = GetVolumeForFuel(Robotics.Qty, PlugInData.BFStats.Robotics.BaseVol, PlugInData.BFStats.Robotics.QtyVol);
            HvyWater.VolForQty = GetVolumeForFuel(HvyWater.Qty, PlugInData.BFStats.HvyWater.BaseVol, PlugInData.BFStats.HvyWater.QtyVol);
            LiqOzone.VolForQty = GetVolumeForFuel(LiqOzone.Qty, PlugInData.BFStats.LiqOzone.BaseVol, PlugInData.BFStats.LiqOzone.QtyVol);
            Charters.VolForQty = GetVolumeForFuel(Charters.Qty, PlugInData.BFStats.Charters.BaseVol, PlugInData.BFStats.Charters.QtyVol);
            N2Iso.VolForQty = GetVolumeForFuel(N2Iso.Qty, PlugInData.BFStats.N2Iso.BaseVol, PlugInData.BFStats.N2Iso.QtyVol);
            H2Iso.VolForQty = GetVolumeForFuel(H2Iso.Qty, PlugInData.BFStats.H2Iso.BaseVol, PlugInData.BFStats.H2Iso.QtyVol);
            O2Iso.VolForQty = GetVolumeForFuel(O2Iso.Qty, PlugInData.BFStats.O2Iso.BaseVol, PlugInData.BFStats.O2Iso.QtyVol);
            HeIso.VolForQty = GetVolumeForFuel(HeIso.Qty, PlugInData.BFStats.HeIso.BaseVol, PlugInData.BFStats.HeIso.QtyVol);

            if (Strontium.QtyVol > 0)
                Strontium.VolForQty = ((Strontium.Qty / PlugInData.BFStats.Strontium.QtyVol) * PlugInData.BFStats.Strontium.BaseVol);
            else
                Strontium.VolForQty = ((Strontium.Qty / 3) * PlugInData.BFStats.Strontium.BaseVol);

            StrontUsed = Strontium.VolForQty;
        }

        public void AddFuelQty(FuelBay fb)
        {
            EnrUran.Qty += fb.EnrUran.Qty;
            Oxygen.Qty += fb.Oxygen.Qty;
            MechPart.Qty += fb.MechPart.Qty;
            Coolant.Qty += fb.Coolant.Qty;
            Robotics.Qty += fb.Robotics.Qty;
            HvyWater.Qty += fb.HvyWater.Qty;
            LiqOzone.Qty += fb.LiqOzone.Qty;
            Charters.Qty += fb.Charters.Qty;
            N2Iso.Qty += fb.N2Iso.Qty;
            if (N2Iso.BaseVol == 0)
                N2Iso.BaseVol = fb.N2Iso.BaseVol;
            if (N2Iso.QtyVol == 0)
                N2Iso.QtyVol = fb.N2Iso.QtyVol;
            if (N2Iso.Cost == 0)
                N2Iso.Cost = fb.N2Iso.Cost;
            HeIso.Qty += fb.HeIso.Qty;
            if (HeIso.BaseVol == 0)
                HeIso.BaseVol = fb.HeIso.BaseVol;
            if (HeIso.QtyVol == 0)
                HeIso.QtyVol = fb.HeIso.QtyVol;
            if (HeIso.Cost == 0)
                HeIso.Cost = fb.HeIso.Cost;
            H2Iso.Qty += fb.H2Iso.Qty;
            if (H2Iso.BaseVol == 0)
                H2Iso.BaseVol = fb.H2Iso.BaseVol;
            if (H2Iso.QtyVol == 0)
                H2Iso.QtyVol = fb.H2Iso.QtyVol;
            if (H2Iso.Cost == 0)
                H2Iso.Cost = fb.H2Iso.Cost;
            O2Iso.Qty += fb.O2Iso.Qty;
            if (O2Iso.BaseVol == 0)
                O2Iso.BaseVol = fb.O2Iso.BaseVol;
            if (O2Iso.QtyVol == 0)
                O2Iso.QtyVol = fb.O2Iso.QtyVol;
            if (O2Iso.Cost == 0)
                O2Iso.Cost = fb.O2Iso.Cost;
            Strontium.Qty += fb.Strontium.Qty;
        }

        public void SetFuelQty(FuelBay fb)
        {
            EnrUran.Qty = fb.EnrUran.Qty;
            Oxygen.Qty = fb.Oxygen.Qty;
            MechPart.Qty = fb.MechPart.Qty;
            Coolant.Qty = fb.Coolant.Qty;
            Robotics.Qty = fb.Robotics.Qty;
            HvyWater.Qty = fb.HvyWater.Qty;
            LiqOzone.Qty = fb.LiqOzone.Qty;
            Charters.Qty = fb.Charters.Qty;
            N2Iso.Qty = fb.N2Iso.Qty;
            HeIso.Qty = fb.HeIso.Qty;
            H2Iso.Qty = fb.H2Iso.Qty;
            O2Iso.Qty = fb.O2Iso.Qty;
            Strontium.Qty = fb.Strontium.Qty;
        }

        public void SubtractFuelQty(FuelBay fb)
        {
            EnrUran.Qty -= fb.EnrUran.Qty;
            Oxygen.Qty -= fb.Oxygen.Qty;
            MechPart.Qty -= fb.MechPart.Qty;
            Coolant.Qty -= fb.Coolant.Qty;
            Robotics.Qty -= fb.Robotics.Qty;
            HvyWater.Qty -= fb.HvyWater.Qty;
            LiqOzone.Qty -= fb.LiqOzone.Qty;
            Charters.Qty -= fb.Charters.Qty;
            N2Iso.Qty -= fb.N2Iso.Qty;
            HeIso.Qty -= fb.HeIso.Qty;
            H2Iso.Qty -= fb.H2Iso.Qty;
            O2Iso.Qty -= fb.O2Iso.Qty;
            Strontium.Qty -= fb.Strontium.Qty;
        }

        public void SubtractZeroMin(FuelBay fb)
        {
            EnrUran.SubtractZeroMin(fb.EnrUran.Qty);
            Oxygen.SubtractZeroMin(fb.Oxygen.Qty);
            MechPart.SubtractZeroMin(fb.MechPart.Qty);
            Coolant.SubtractZeroMin(fb.Coolant.Qty);
            Robotics.SubtractZeroMin(fb.Robotics.Qty);
            HvyWater.SubtractZeroMin(fb.HvyWater.Qty);
            LiqOzone.SubtractZeroMin(fb.LiqOzone.Qty);
            Charters.SubtractZeroMin(fb.Charters.Qty);
            N2Iso.SubtractZeroMin(fb.N2Iso.Qty);
            HeIso.SubtractZeroMin(fb.HeIso.Qty);
            H2Iso.SubtractZeroMin(fb.H2Iso.Qty);
            O2Iso.SubtractZeroMin(fb.O2Iso.Qty);
            Strontium.SubtractZeroMin(fb.Strontium.Qty);
        }

        public void SetFuelQtyForPeriod(decimal period, decimal sov_mult, decimal cpu_b, decimal cpu_u, decimal pow_b, decimal pow_u, bool useChart)
        {
            EnrUran.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            Oxygen.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            MechPart.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            Coolant.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            Robotics.SetFuelQtyForPeriod(sov_mult, 1, 1, period);

            HvyWater.SetFuelQtyForPeriod(sov_mult, cpu_b, cpu_u, period);
            LiqOzone.SetFuelQtyForPeriod(sov_mult, pow_b, pow_u, period);

            if (useChart)
                Charters.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            else
                Charters.Qty = 0;

            N2Iso.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            HeIso.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            H2Iso.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            O2Iso.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
        }

        public void SetFuelQtyForPeriodFromCurrent(decimal period, decimal sov_mult, decimal cpu_b, decimal cpu_u, decimal pow_b, decimal pow_u)
        {
            EnrUran.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            Oxygen.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            MechPart.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            Coolant.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            Robotics.SetFuelQtyForPeriod(sov_mult, 1, 1, period);

            HvyWater.SetFuelQtyForPeriod(sov_mult, cpu_b, cpu_u, period);
            LiqOzone.SetFuelQtyForPeriod(sov_mult, pow_b, pow_u, period);

            Charters.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            N2Iso.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            HeIso.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            H2Iso.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            O2Iso.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
        }

        public void SetStrontQtyForPeriod(decimal period, decimal sov_mult)
        {
            Strontium.Qty = Math.Ceiling(Strontium.PeriodQty * sov_mult) * period;
        }

        public void SetStrontQtyForPeriodOrMax(decimal period, decimal sov_mult)
        {
            decimal maxSP;
            decimal StrVolPer, StrQpp;

            StrQpp = Math.Max((Math.Ceiling(Strontium.PeriodQty * sov_mult)),1);

            if (Strontium.QtyVol > 0)
                StrVolPer = ((StrQpp / Strontium.QtyVol) * Strontium.BaseVol);
            else
                StrVolPer = 3;

            maxSP = Math.Floor(StrontCap / StrVolPer);

            if (period > maxSP)
            {
                Strontium.Qty = Math.Ceiling(Strontium.PeriodQty * sov_mult) * maxSP;
            }
            else
            {
                Strontium.Qty = Math.Ceiling(Strontium.PeriodQty * sov_mult) * period;
            }
        }

        public void DecrementFuelQtyForPeriod(decimal period, decimal sov_mult, decimal cpu_b, decimal cpu_u, decimal pow_b, decimal pow_u, bool useChart)
        {
            EnrUran.DecrementFuelQtyForPeriod(period, sov_mult, 1, 1);
            Oxygen.DecrementFuelQtyForPeriod(period, sov_mult, 1, 1);
            MechPart.DecrementFuelQtyForPeriod(period, sov_mult, 1, 1);
            Coolant.DecrementFuelQtyForPeriod(period, sov_mult, 1, 1);
            Robotics.DecrementFuelQtyForPeriod(period, sov_mult, 1, 1);

            HvyWater.DecrementFuelQtyForPeriod(period, sov_mult, cpu_b, cpu_u);
            LiqOzone.DecrementFuelQtyForPeriod(period, sov_mult, pow_b, pow_u);
            
            if (useChart)
                Charters.DecrementFuelQtyForPeriod(period, sov_mult, 1, 1);

            N2Iso.DecrementFuelQtyForPeriod(period, sov_mult, 1, 1);
            HeIso.DecrementFuelQtyForPeriod(period, sov_mult, 1, 1);
            H2Iso.DecrementFuelQtyForPeriod(period, sov_mult, 1, 1);
            O2Iso.DecrementFuelQtyForPeriod(period, sov_mult, 1, 1);
        }

        public void DecrementStrontQtyForPeriod(decimal period, decimal sov_mult)
        {
            Strontium.Qty -= Math.Max((Math.Ceiling(Strontium.PeriodQty * sov_mult) * period), 0);
        }

        public void SetFuelRunTimes(decimal cpu_b, decimal cpu_u, decimal pow_b, decimal pow_u, decimal sov_mult)
        {
            EnrUran.SetFuelRunTime(sov_mult, 1, 1);
            Oxygen.SetFuelRunTime(sov_mult, 1, 1);
            MechPart.SetFuelRunTime(sov_mult, 1, 1);
            Coolant.SetFuelRunTime(sov_mult, 1, 1);
            Robotics.SetFuelRunTime(sov_mult, 1, 1);
            HeIso.SetFuelRunTime(sov_mult, 1, 1);
            H2Iso.SetFuelRunTime(sov_mult, 1, 1);
            N2Iso.SetFuelRunTime(sov_mult, 1, 1);
            O2Iso.SetFuelRunTime(sov_mult, 1, 1);
            HvyWater.SetFuelRunTime(sov_mult, cpu_b, cpu_u);
            LiqOzone.SetFuelRunTime(sov_mult, pow_b, pow_u);
            Charters.SetFuelRunTime(1, 1, 1);
            Strontium.SetFuelRunTime(sov_mult, 1, 1);
        }

        public ArrayList GetShortestFuelRunTimeAndName(decimal cpu_b, decimal cpu_u, decimal pow_b, decimal pow_u, decimal sov_mod, bool useChart)
        {
            ArrayList rt_nm;
            ArrayList retList = new ArrayList();
            decimal f_run;
            decimal m_run = 9999999999999999;

            rt_nm = EnrUran.SetAndReturnFuelRunTimeAndName(sov_mod, 1, 1);
            f_run = (decimal)(rt_nm[0]);
            if (f_run < m_run)
            {
                m_run = f_run;
                retList = new ArrayList(rt_nm);
            }

            rt_nm = Oxygen.SetAndReturnFuelRunTimeAndName(sov_mod, 1, 1);
            f_run = (decimal)(rt_nm[0]);
            if (f_run < m_run)
            {
                m_run = f_run;
                retList = new ArrayList(rt_nm);
            }

            rt_nm = MechPart.SetAndReturnFuelRunTimeAndName(sov_mod, 1, 1);
            f_run = (decimal)(rt_nm[0]);
            if (f_run < m_run)
            {
                m_run = f_run;
                retList = new ArrayList(rt_nm);
            }

            rt_nm = Coolant.SetAndReturnFuelRunTimeAndName(sov_mod, 1, 1);
            f_run = (decimal)(rt_nm[0]);
            if (f_run < m_run)
            {
                m_run = f_run;
                retList = new ArrayList(rt_nm);
            }

            rt_nm = Robotics.SetAndReturnFuelRunTimeAndName(sov_mod, 1, 1);
            f_run = (decimal)(rt_nm[0]);
            if (f_run < m_run)
            {
                m_run = f_run;
                retList = new ArrayList(rt_nm);
            }

            rt_nm = HeIso.SetAndReturnFuelRunTimeAndName(sov_mod, 1, 1);
            f_run = (decimal)(rt_nm[0]);
            if ((f_run < m_run) && ((string)rt_nm[1] != ""))
            {
                m_run = f_run;
                retList = new ArrayList(rt_nm);
            }

            rt_nm = H2Iso.SetAndReturnFuelRunTimeAndName(sov_mod, 1, 1);
            f_run = (decimal)(rt_nm[0]);
            if ((f_run < m_run) && ((string)rt_nm[1] != ""))
            {
                m_run = f_run;
                retList = new ArrayList(rt_nm);
            }

            rt_nm = N2Iso.SetAndReturnFuelRunTimeAndName(sov_mod, 1, 1);
            f_run = (decimal)(rt_nm[0]);
            if ((f_run < m_run) && ((string)rt_nm[1] != ""))
            {
                m_run = f_run;
                retList = new ArrayList(rt_nm);
            }

            rt_nm = O2Iso.SetAndReturnFuelRunTimeAndName(sov_mod, 1, 1);
            f_run = (decimal)(rt_nm[0]);
            if ((f_run < m_run) && ((string)rt_nm[1] != ""))
            {
                m_run = f_run;
                retList = new ArrayList(rt_nm);
            }

            rt_nm = HvyWater.SetAndReturnFuelRunTimeAndName(sov_mod, cpu_b, cpu_u);
            f_run = (decimal)(rt_nm[0]);
            if (f_run < m_run)
            {
                m_run = f_run;
                retList = new ArrayList(rt_nm);
            }

            rt_nm = LiqOzone.SetAndReturnFuelRunTimeAndName(sov_mod, pow_b, pow_u);
            f_run = (decimal)(rt_nm[0]);
            if (f_run < m_run)
            {
                m_run = f_run;
                retList = new ArrayList(rt_nm);
            }

            if (useChart)
            {
                rt_nm = Charters.SetAndReturnFuelRunTimeAndName(1, 1, 1);
                f_run = (decimal)(rt_nm[0]);
                if (f_run < m_run)
                {
                    m_run = f_run;
                    retList = new ArrayList(rt_nm);
                }
            }
            return retList;
        }

        public string[,] GetFuelBayTotals()
        {
            string[,] retVal = new string[13, 4];

            retVal[0, 0] = EnrUran.Name;
            retVal[0, 1] = EnrUran.Qty.ToString();
            retVal[0, 2] = EnrUran.VolForQty.ToString();
            retVal[0, 3] = EnrUran.CostForQty.ToString();

            retVal[1, 0] = Oxygen.Name;
            retVal[1, 1] = Oxygen.Qty.ToString();
            retVal[1, 2] = Oxygen.VolForQty.ToString();
            retVal[1, 3] = Oxygen.CostForQty.ToString();

            retVal[2, 0] = MechPart.Name;
            retVal[2, 1] = MechPart.Qty.ToString();
            retVal[2, 2] = MechPart.VolForQty.ToString();
            retVal[2, 3] = MechPart.CostForQty.ToString();

            retVal[3, 0] = Coolant.Name;
            retVal[3, 1] = Coolant.Qty.ToString();
            retVal[3, 2] = Coolant.VolForQty.ToString();
            retVal[3, 3] = Coolant.CostForQty.ToString();

            retVal[4, 0] = Robotics.Name;
            retVal[4, 1] = Robotics.Qty.ToString();
            retVal[4, 2] = Robotics.VolForQty.ToString();
            retVal[4, 3] = Robotics.CostForQty.ToString();

            retVal[5, 0] = "Helium Isotopes";
            retVal[5, 1] = HeIso.Qty.ToString();
            retVal[5, 2] = HeIso.VolForQty.ToString();
            retVal[5, 3] = HeIso.CostForQty.ToString();

            retVal[6, 0] = "Hydrogen Isotopes";
            retVal[6, 1] = H2Iso.Qty.ToString();
            retVal[6, 2] = H2Iso.VolForQty.ToString();
            retVal[6, 3] = H2Iso.CostForQty.ToString();

            retVal[7, 0] = "Nitrogen Isotopes";
            retVal[7, 1] = N2Iso.Qty.ToString();
            retVal[7, 2] = N2Iso.VolForQty.ToString();
            retVal[7, 3] = N2Iso.CostForQty.ToString();

            retVal[8, 0] = "Oxygen Isotopes";
            retVal[8, 1] = O2Iso.Qty.ToString();
            retVal[8, 2] = O2Iso.VolForQty.ToString();
            retVal[8, 3] = O2Iso.CostForQty.ToString();

            retVal[9, 0] = HvyWater.Name;
            retVal[9, 1] = HvyWater.Qty.ToString();
            retVal[9, 2] = HvyWater.VolForQty.ToString();
            retVal[9, 3] = HvyWater.CostForQty.ToString();

            retVal[10, 0] = LiqOzone.Name;
            retVal[10, 1] = LiqOzone.Qty.ToString();
            retVal[10, 2] = LiqOzone.VolForQty.ToString();
            retVal[10, 3] = LiqOzone.CostForQty.ToString();

            retVal[11, 0] = Charters.Name;
            retVal[11, 1] = Charters.Qty.ToString();
            retVal[11, 2] = Charters.VolForQty.ToString();
            retVal[11, 3] = Charters.CostForQty.ToString();

            retVal[12, 0] = Strontium.Name;
            retVal[12, 1] = Strontium.Qty.ToString();
            retVal[12, 2] = Strontium.VolForQty.ToString();
            retVal[12, 3] = Strontium.CostForQty.ToString();

            return retVal;
        }

        public string[,] GetFuelBayAndBurnTotals(POS p, decimal sov_mod)
        {
            string[,] retVal = new string[13, 7];

            retVal[0, 0] = EnrUran.Name;
            retVal[0, 1] = EnrUran.Qty.ToString();
            retVal[0, 2] = EnrUran.VolForQty.ToString();
            retVal[0, 3] = p.PosTower.Fuel.EnrUran.Qty.ToString();
            retVal[0, 4] = EnrUran.GetFuelQtyForPeriod(sov_mod, 1, 1).ToString();
            retVal[0, 5] = p.PosTower.Fuel.EnrUran.RunTime.ToString();
            retVal[0, 6] = EnrUran.RunTime.ToString();

            retVal[1, 0] = Oxygen.Name;
            retVal[1, 1] = Oxygen.Qty.ToString();
            retVal[1, 2] = Oxygen.VolForQty.ToString();
            retVal[1, 3] = p.PosTower.Fuel.Oxygen.Qty.ToString();
            retVal[1, 4] = Oxygen.GetFuelQtyForPeriod(sov_mod, 1, 1).ToString();
            retVal[1, 5] = p.PosTower.Fuel.Oxygen.RunTime.ToString();
            retVal[1, 6] = Oxygen.RunTime.ToString();

            retVal[2, 0] = MechPart.Name;
            retVal[2, 1] = MechPart.Qty.ToString();
            retVal[2, 2] = MechPart.VolForQty.ToString();
            retVal[2, 3] = p.PosTower.Fuel.MechPart.Qty.ToString();
            retVal[2, 4] = MechPart.GetFuelQtyForPeriod(sov_mod, 1, 1).ToString();
            retVal[2, 5] = p.PosTower.Fuel.MechPart.RunTime.ToString();
            retVal[2, 6] = MechPart.RunTime.ToString();

            retVal[3, 0] = Coolant.Name;
            retVal[3, 1] = Coolant.Qty.ToString();
            retVal[3, 2] = Coolant.VolForQty.ToString();
            retVal[3, 3] = p.PosTower.Fuel.Coolant.Qty.ToString();
            retVal[3, 4] = Coolant.GetFuelQtyForPeriod(sov_mod, 1, 1).ToString();
            retVal[3, 5] = p.PosTower.Fuel.Coolant.RunTime.ToString();
            retVal[3, 6] = Coolant.RunTime.ToString();

            retVal[4, 0] = Robotics.Name;
            retVal[4, 1] = Robotics.Qty.ToString();
            retVal[4, 2] = Robotics.VolForQty.ToString();
            retVal[4, 3] = p.PosTower.Fuel.Robotics.Qty.ToString();
            retVal[4, 4] = Robotics.GetFuelQtyForPeriod(sov_mod, 1, 1).ToString();
            retVal[4, 5] = p.PosTower.Fuel.Robotics.RunTime.ToString();
            retVal[4, 6] = Robotics.RunTime.ToString();

            retVal[5, 0] = "Helium Isotopes";
            retVal[5, 1] = HeIso.Qty.ToString();
            retVal[5, 2] = HeIso.VolForQty.ToString();
            retVal[5, 3] = p.PosTower.Fuel.HeIso.Qty.ToString();
            retVal[5, 4] = HeIso.GetFuelQtyForPeriod(sov_mod, 1, 1).ToString();
            retVal[5, 5] = p.PosTower.Fuel.HeIso.RunTime.ToString();
            retVal[5, 6] = HeIso.RunTime.ToString();

            retVal[6, 0] = "Hydrogen Isotopes";
            retVal[6, 1] = H2Iso.Qty.ToString();
            retVal[6, 2] = H2Iso.VolForQty.ToString();
            retVal[6, 3] = p.PosTower.Fuel.H2Iso.Qty.ToString();
            retVal[6, 4] = H2Iso.GetFuelQtyForPeriod(sov_mod, 1, 1).ToString();
            retVal[6, 5] = p.PosTower.Fuel.H2Iso.RunTime.ToString();
            retVal[6, 6] = H2Iso.RunTime.ToString();

            retVal[7, 0] = "Nitrogen Isotopes";
            retVal[7, 1] = N2Iso.Qty.ToString();
            retVal[7, 2] = N2Iso.VolForQty.ToString();
            retVal[7, 3] = p.PosTower.Fuel.N2Iso.Qty.ToString();
            retVal[7, 4] = N2Iso.GetFuelQtyForPeriod(sov_mod, 1, 1).ToString();
            retVal[7, 5] = p.PosTower.Fuel.N2Iso.RunTime.ToString();
            retVal[7, 6] = N2Iso.RunTime.ToString();

            retVal[8, 0] = "Oxygen Isotopes";
            retVal[8, 1] = O2Iso.Qty.ToString();
            retVal[8, 2] = O2Iso.VolForQty.ToString();
            retVal[8, 3] = p.PosTower.Fuel.O2Iso.Qty.ToString();
            retVal[8, 4] = O2Iso.GetFuelQtyForPeriod(sov_mod, 1, 1).ToString();
            retVal[8, 5] = p.PosTower.Fuel.O2Iso.RunTime.ToString();
            retVal[8, 6] = O2Iso.RunTime.ToString();

            retVal[9, 0] = HvyWater.Name;
            retVal[9, 1] = HvyWater.Qty.ToString();
            retVal[9, 2] = HvyWater.VolForQty.ToString();
            retVal[9, 3] = p.PosTower.Fuel.HvyWater.Qty.ToString();
            retVal[9, 4] = HvyWater.GetFuelQtyForPeriod(sov_mod, p.PosTower.CPU, p.PosTower.CPU_Used).ToString();
            retVal[9, 5] = p.PosTower.Fuel.HvyWater.RunTime.ToString();
            retVal[9, 6] = HvyWater.RunTime.ToString();

            retVal[10, 0] = LiqOzone.Name;
            retVal[10, 1] = LiqOzone.Qty.ToString();
            retVal[10, 2] = LiqOzone.VolForQty.ToString();
            retVal[10, 3] = p.PosTower.Fuel.LiqOzone.Qty.ToString();
            retVal[10, 4] = LiqOzone.GetFuelQtyForPeriod(sov_mod, p.PosTower.Power, p.PosTower.Power_Used).ToString();
            retVal[10, 5] = p.PosTower.Fuel.LiqOzone.RunTime.ToString();
            retVal[10, 6] = LiqOzone.RunTime.ToString();

            retVal[11, 0] = Charters.Name;
            retVal[11, 1] = Charters.Qty.ToString();
            retVal[11, 2] = Charters.VolForQty.ToString();
            retVal[11, 3] = p.PosTower.Fuel.Charters.Qty.ToString();
            retVal[11, 4] = Charters.GetFuelQtyForPeriod(sov_mod, 1, 1).ToString();
            retVal[11, 5] = p.PosTower.Fuel.Charters.RunTime.ToString();
            retVal[11, 6] = Charters.RunTime.ToString();

            retVal[12, 0] = Strontium.Name;
            retVal[12, 1] = Strontium.Qty.ToString();
            retVal[12, 2] = Strontium.VolForQty.ToString();
            retVal[12, 3] = p.PosTower.Fuel.Strontium.Qty.ToString();
            retVal[12, 4] = Strontium.GetFuelQtyForPeriod(sov_mod, 1, 1).ToString();
            retVal[12, 5] = p.PosTower.Fuel.Strontium.RunTime.ToString();
            retVal[12, 6] = Strontium.RunTime.ToString();

            return retVal;
        }

        public string[,] GetFuelAmountsAndBayTotals(POS p, decimal sov_mod)
        {
            string[,] retVal = new string[12, 6];

            retVal[0, 0] = EnrUran.Name;
            retVal[0, 1] = EnrUran.Qty.ToString();
            retVal[0, 2] = EnrUran.RunTime.ToString();
            retVal[0, 3] = EnrUran.VolForQty.ToString();
            retVal[0, 4] = (EnrUran.Qty + p.PosTower.Fuel.EnrUran.Qty).ToString();
            retVal[0, 5] = (EnrUran.RunTime + p.PosTower.Fuel.EnrUran.RunTime).ToString();

            retVal[1, 0] = Oxygen.Name;
            retVal[1, 1] = Oxygen.Qty.ToString();
            retVal[1, 2] = Oxygen.RunTime.ToString();
            retVal[1, 3] = Oxygen.VolForQty.ToString();
            retVal[1, 4] = (Oxygen.Qty + p.PosTower.Fuel.Oxygen.Qty).ToString();
            retVal[1, 5] = (Oxygen.RunTime + p.PosTower.Fuel.Oxygen.RunTime).ToString();

            retVal[2, 0] = MechPart.Name;
            retVal[2, 1] = MechPart.Qty.ToString();
            retVal[2, 2] = MechPart.RunTime.ToString();
            retVal[2, 3] = MechPart.VolForQty.ToString();
            retVal[2, 4] = (MechPart.Qty + p.PosTower.Fuel.MechPart.Qty).ToString();
            retVal[2, 5] = (MechPart.RunTime + p.PosTower.Fuel.MechPart.RunTime).ToString();

            retVal[3, 0] = Coolant.Name;
            retVal[3, 1] = Coolant.Qty.ToString();
            retVal[3, 2] = Coolant.RunTime.ToString();
            retVal[3, 3] = Coolant.VolForQty.ToString();
            retVal[3, 4] = (Coolant.Qty + p.PosTower.Fuel.Coolant.Qty).ToString();
            retVal[3, 5] = (Coolant.RunTime + p.PosTower.Fuel.Coolant.RunTime).ToString();

            retVal[4, 0] = Robotics.Name;
            retVal[4, 1] = Robotics.Qty.ToString();
            retVal[4, 2] = Robotics.RunTime.ToString();
            retVal[4, 3] = Robotics.VolForQty.ToString();
            retVal[4, 4] = (Robotics.Qty + p.PosTower.Fuel.Robotics.Qty).ToString();
            retVal[4, 5] = (Robotics.RunTime + p.PosTower.Fuel.Robotics.RunTime).ToString();

            retVal[5, 0] = "Helium Isotopes";
            retVal[5, 1] = HeIso.Qty.ToString();
            retVal[5, 2] = HeIso.RunTime.ToString();
            retVal[5, 3] = HeIso.VolForQty.ToString();
            retVal[5, 4] = (HeIso.Qty + p.PosTower.Fuel.HeIso.Qty).ToString();
            retVal[5, 5] = (HeIso.RunTime + p.PosTower.Fuel.HeIso.RunTime).ToString();

            retVal[6, 0] = "Hydrogen Isotopes";
            retVal[6, 1] = H2Iso.Qty.ToString();
            retVal[6, 2] = H2Iso.RunTime.ToString();
            retVal[6, 3] = H2Iso.VolForQty.ToString();
            retVal[6, 4] = (H2Iso.Qty + p.PosTower.Fuel.H2Iso.Qty).ToString();
            retVal[6, 5] = (H2Iso.RunTime + p.PosTower.Fuel.H2Iso.RunTime).ToString();

            retVal[7, 0] = "Nitrogen Isotopes";
            retVal[7, 1] = N2Iso.Qty.ToString();
            retVal[7, 2] = N2Iso.RunTime.ToString();
            retVal[7, 3] = N2Iso.VolForQty.ToString();
            retVal[7, 4] = (N2Iso.Qty + p.PosTower.Fuel.N2Iso.Qty).ToString();
            retVal[7, 5] = (N2Iso.RunTime + p.PosTower.Fuel.N2Iso.RunTime).ToString();

            retVal[8, 0] = "Oxygen Isotopes";
            retVal[8, 1] = O2Iso.Qty.ToString();
            retVal[8, 2] = O2Iso.RunTime.ToString();
            retVal[8, 3] = O2Iso.VolForQty.ToString();
            retVal[8, 4] = (O2Iso.Qty + p.PosTower.Fuel.O2Iso.Qty).ToString();
            retVal[8, 5] = (O2Iso.RunTime + p.PosTower.Fuel.O2Iso.RunTime).ToString();

            retVal[9, 0] = HvyWater.Name;
            retVal[9, 1] = HvyWater.Qty.ToString();
            retVal[9, 2] = HvyWater.RunTime.ToString();
            retVal[9, 3] = HvyWater.VolForQty.ToString();
            retVal[9, 4] = (HvyWater.Qty + p.PosTower.Fuel.HvyWater.Qty).ToString();
            retVal[9, 5] = (HvyWater.RunTime + p.PosTower.Fuel.HvyWater.RunTime).ToString();

            retVal[10, 0] = LiqOzone.Name;
            retVal[10, 1] = LiqOzone.Qty.ToString();
            retVal[10, 2] = LiqOzone.RunTime.ToString();
            retVal[10, 3] = LiqOzone.VolForQty.ToString();
            retVal[10, 4] = (LiqOzone.Qty + p.PosTower.Fuel.LiqOzone.Qty).ToString();
            retVal[10, 5] = (LiqOzone.RunTime + p.PosTower.Fuel.LiqOzone.RunTime).ToString();

            retVal[11, 0] = Charters.Name;
            retVal[11, 1] = Charters.Qty.ToString();
            retVal[11, 2] = Charters.RunTime.ToString();
            retVal[11, 3] = Charters.VolForQty.ToString();
            retVal[11, 4] = (Charters.Qty + p.PosTower.Fuel.Charters.Qty).ToString();
            retVal[11, 5] = (Charters.RunTime + p.PosTower.Fuel.Charters.RunTime).ToString();

            return retVal;
        }

    }
}
