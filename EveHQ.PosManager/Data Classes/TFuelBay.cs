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
    public class TFuelBay
    {
        public FuelType Blocks;
        public FuelType Strontium, Charters;
        public decimal FuelCap, FuelUsed, StrontCap, StrontUsed, FuelCost;
        public SortedList<string, decimal> ExtraD;
        public SortedList<string, string> ExtraS;
        public ArrayList Extra;

        public TFuelBay()
        {
            Blocks = new FuelType();
            Strontium = new FuelType();
            Charters = new FuelType();
            FuelCap = 0;
            FuelUsed = 0;
            StrontCap = 0;
            StrontUsed = 0;
            ExtraD = new SortedList<string, decimal>();
            ExtraS = new SortedList<string, string>();
            Extra = new ArrayList();
        }

        public TFuelBay(TFuelBay fb)
        {
            if (fb == null)
                return;

            if (fb.Blocks == null)
                Blocks = new FuelType();
            else
                Blocks = new FuelType(fb.Blocks);
            Strontium = new FuelType(fb.Strontium);
            Charters = new FuelType(fb.Charters);
            FuelCap = fb.FuelCap;
            FuelUsed = fb.FuelUsed;
            StrontCap = fb.StrontCap;
            StrontUsed = fb.StrontUsed;
            ExtraD = new SortedList<string, decimal>();
            ExtraS = new SortedList<string, string>();
            Extra = new ArrayList();
            foreach (var v in fb.ExtraD)
                ExtraD.Add(v.Key, v.Value);
            foreach (var v in fb.ExtraS)
                ExtraS.Add(v.Key, v.Value);
            foreach (var v in fb.Extra)
                Extra.Add(v);
        }

        public TFuelBay(FuelBay fb)
        {
            if (fb == null)
                return;

            Blocks = new FuelType(PlugInData.BBStats.Blocks);

            Strontium = new FuelType(fb.Strontium);
            Charters = new FuelType(fb.Charters);
            FuelCap = fb.FuelCap;
            FuelUsed = fb.FuelUsed;
            StrontCap = fb.StrontCap;
            StrontUsed = fb.StrontUsed;
            ExtraD = new SortedList<string, decimal>();
            ExtraS = new SortedList<string, string>();
            Extra = new ArrayList();
            foreach (var v in fb.Extra)
                Extra.Add(v);
        }

        public void SetLastFuelRead()
        {
            // Do all types, although at this time only HW and LO matter
            Blocks.LastQty = Blocks.Qty;
            Strontium.LastQty = Strontium.Qty;
            Charters.LastQty = Charters.Qty;
        }

        public void SetAPIFuelUsage(decimal sov_mult)
        {
            Blocks.SetAPIFuelUse(sov_mult);
            Strontium.SetAPIFuelUse(sov_mult);
            Charters.SetAPIFuelUse(sov_mult);
        }

        public void CopyLastAndAPI(TFuelBay fb)
        {
            Blocks.LastQty = fb.Blocks.LastQty;
            Blocks.APIPerQty = fb.Blocks.APIPerQty;
            Strontium.LastQty = fb.Strontium.LastQty;
            Strontium.APIPerQty = fb.Strontium.APIPerQty;
            Charters.LastQty = fb.Charters.LastQty;
            Charters.APIPerQty = fb.Charters.APIPerQty;
        }

        public void SetFuelItemID(TFuelBay fb)
        {
            Blocks.itemID = fb.Blocks.itemID;
            Strontium.itemID = fb.Strontium.itemID;
            Charters.itemID = fb.Charters.itemID;
        }

        public void SetFuelBaseValues(TFuelBay fb)
        {
            Blocks.UpdateBaseValues(fb.Blocks);
            Strontium.UpdateBaseValues(fb.Strontium);
            Charters.UpdateBaseValues(fb.Charters);
        }

        private decimal GetFuelItemCost(string id, int cat)
        {
            decimal fCost = 0;

            if (id.Equals("0"))
                return 1000;

            switch (cat)
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

            fCost = GetFuelItemCost(Charters.itemID, cat);
            if (fb.Charters.Cost > 0)
                Charters.CostForQty = (Charters.Qty * fb.Charters.Cost);
            else
            {
                Charters.CostForQty = (Charters.Qty * fCost);
            }
            FuelCost += Charters.CostForQty;

            fCost = GetFuelItemCost("0", cat);
            if (fb.Blocks.Cost > 0)
                Blocks.CostForQty = (Blocks.Qty * fb.Blocks.Cost);
            else
                Blocks.CostForQty = (Blocks.Qty * fCost);

            FuelCost = Blocks.CostForQty;
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
            Blocks.VolForQty = GetVolumeForFuel(Blocks.Qty, PlugInData.BBStats.Blocks.BaseVol, PlugInData.BBStats.Blocks.QtyVol);
            Charters.VolForQty = GetVolumeForFuel(Charters.Qty, PlugInData.BFStats.Charters.BaseVol, PlugInData.BFStats.Charters.QtyVol);
            
            if (Strontium.QtyVol > 0)
                Strontium.VolForQty = ((Strontium.Qty / PlugInData.BFStats.Strontium.QtyVol) * PlugInData.BFStats.Strontium.BaseVol);
            else
                Strontium.VolForQty = ((Strontium.Qty / 3) * PlugInData.BFStats.Strontium.BaseVol);

            StrontUsed = Strontium.VolForQty;
        }

        public void AddFuelQty(TFuelBay fb)
        {
            Blocks.Qty += fb.Blocks.Qty;
            Strontium.Qty += fb.Strontium.Qty;
            Charters.Qty += fb.Charters.Qty;
        }

        public void SetFuelQty(TFuelBay fb)
        {
            Blocks.Qty = fb.Blocks.Qty;
            Strontium.Qty = fb.Strontium.Qty;
            Charters.Qty = fb.Charters.Qty;
        }

        public void SubtractFuelQty(TFuelBay fb)
        {
            Blocks.Qty -= fb.Blocks.Qty;
            Strontium.Qty -= fb.Strontium.Qty;
            Charters.Qty -= fb.Charters.Qty;
        }

        public void SubtractZeroMin(TFuelBay fb)
        {
            Blocks.SubtractZeroMin(fb.Blocks.Qty);
            Strontium.SubtractZeroMin(fb.Strontium.Qty);
            Charters.SubtractZeroMin(fb.Charters.Qty);
        }

        public void SetFuelQtyForPeriod(decimal period, decimal sov_mult, bool useChart)
        {
            Blocks.SetFuelQtyForPeriod(sov_mult, 1, 1, period);

            if (useChart)
                Charters.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            else
                Charters.Qty = 0;
        }

        public void SetFuelQtyForPeriodFromCurrent(decimal period, decimal sov_mult)
        {
            Blocks.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
            Charters.SetFuelQtyForPeriod(sov_mult, 1, 1, period);
        }

        public void SetStrontQtyForPeriod(decimal period, decimal sov_mult)
        {
            Strontium.Qty = Math.Ceiling(Strontium.PeriodQty * sov_mult) * period;
        }

        public void SetStrontQtyForPeriodOrMax(decimal period, decimal sov_mult)
        {
            decimal maxSP;
            decimal StrVolPer, StrQpp;

            StrQpp = Math.Max((Math.Ceiling(Strontium.PeriodQty * sov_mult)), 1);

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

        public void DecrementFuelQtyForPeriod(decimal period, decimal sov_mult, bool useChart)
        {
            Blocks.DecrementFuelQtyForPeriod(period, sov_mult, 1, 1);
            if (useChart)
                Charters.DecrementFuelQtyForPeriod(period, sov_mult, 1, 1);
        }

        public void DecrementStrontQtyForPeriod(decimal period, decimal sov_mult)
        {
            Strontium.Qty -= Math.Max((Math.Ceiling(Strontium.PeriodQty * sov_mult) * period), 0);
        }

        public void SetFuelRunTimes(decimal sov_mult)
        {
            Blocks.SetFuelRunTime(sov_mult, 1, 1);
            Charters.SetFuelRunTime(1, 1, 1);
            Strontium.SetFuelRunTime(sov_mult, 1, 1);
        }

        public ArrayList GetShortestFuelRunTimeAndName(decimal sov_mod, bool useChart)
        {
            ArrayList rt_nm;
            ArrayList retList = new ArrayList();
            decimal f_run;
            decimal m_run = 9999999999999999;

            rt_nm = Blocks.SetAndReturnFuelRunTimeAndName(sov_mod, 1, 1);
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
            string[,] retVal = new string[3, 4];

            retVal[0, 0] = Blocks.Name;
            retVal[0, 1] = Blocks.Qty.ToString();
            retVal[0, 2] = Blocks.VolForQty.ToString();
            retVal[0, 3] = Blocks.CostForQty.ToString();

            retVal[1, 0] = Charters.Name;
            retVal[1, 1] = Charters.Qty.ToString();
            retVal[1, 2] = Charters.VolForQty.ToString();
            retVal[1, 3] = Charters.CostForQty.ToString();

            retVal[2, 0] = Strontium.Name;
            retVal[2, 1] = Strontium.Qty.ToString();
            retVal[2, 2] = Strontium.VolForQty.ToString();
            retVal[2, 3] = Strontium.CostForQty.ToString();

            return retVal;
        }

        public string[,] GetFuelBayAndBurnTotals(New_POS p, decimal sov_mod)
        {
            string[,] retVal = new string[3, 7];

            retVal[0, 0] = Blocks.Name;
            retVal[0, 1] = Blocks.Qty.ToString();
            retVal[0, 2] = Blocks.VolForQty.ToString();
            retVal[0, 3] = p.PosTower.Fuel.Blocks.Qty.ToString();
            retVal[0, 4] = Blocks.GetFuelQtyForPeriod(sov_mod, 1, 1).ToString();
            retVal[0, 5] = p.PosTower.Fuel.Blocks.RunTime.ToString();
            retVal[0, 6] = Blocks.RunTime.ToString();

            retVal[1, 0] = Charters.Name;
            retVal[1, 1] = Charters.Qty.ToString();
            retVal[1, 2] = Charters.VolForQty.ToString();
            retVal[1, 3] = p.PosTower.Fuel.Charters.Qty.ToString();
            retVal[1, 4] = Charters.GetFuelQtyForPeriod(sov_mod, 1, 1).ToString();
            retVal[1, 5] = p.PosTower.Fuel.Charters.RunTime.ToString();
            retVal[1, 6] = Charters.RunTime.ToString();

            retVal[2, 0] = Strontium.Name;
            retVal[2, 1] = Strontium.Qty.ToString();
            retVal[2, 2] = Strontium.VolForQty.ToString();
            retVal[2, 3] = p.PosTower.Fuel.Strontium.Qty.ToString();
            retVal[2, 4] = Strontium.GetFuelQtyForPeriod(sov_mod, 1, 1).ToString();
            retVal[2, 5] = p.PosTower.Fuel.Strontium.RunTime.ToString();
            retVal[2, 6] = Strontium.RunTime.ToString();

            return retVal;
        }

        public string[,] GetFuelAmountsAndBayTotals(New_POS p, decimal sov_mod)
        {
            string[,] retVal = new string[2, 6];

            retVal[0, 0] = Blocks.Name;
            retVal[0, 1] = Blocks.Qty.ToString();
            retVal[0, 2] = Blocks.RunTime.ToString();
            retVal[0, 3] = Blocks.VolForQty.ToString();
            retVal[0, 4] = (Blocks.Qty + p.PosTower.Fuel.Blocks.Qty).ToString();
            retVal[0, 5] = (Blocks.RunTime + p.PosTower.Fuel.Blocks.RunTime).ToString();

            retVal[1, 0] = Charters.Name;
            retVal[1, 1] = Charters.Qty.ToString();
            retVal[1, 2] = Charters.RunTime.ToString();
            retVal[1, 3] = Charters.VolForQty.ToString();
            retVal[1, 4] = (Charters.Qty + p.PosTower.Fuel.Charters.Qty).ToString();
            retVal[1, 5] = (Charters.RunTime + p.PosTower.Fuel.Charters.RunTime).ToString();

            return retVal;
        }

    }
}
