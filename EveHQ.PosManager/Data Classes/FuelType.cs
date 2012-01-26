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
    public class FuelType
    {
        public string Name, UsedFor, itemID;
        public decimal BaseVol, QtyVol;
        public decimal Cost;
        public decimal Qty, BaseQty, PeriodQty, RunTime, LastQty, APIPerQty;
        public decimal VolForQty, CostForQty;
        public ArrayList Extra;
        public bool FuelRetry;

        public FuelType()
        {
            Name = "";
            UsedFor = "";
            itemID = "";
            BaseVol = 0;
            QtyVol = 0;
            Cost = 0;
            Qty = 0;
            BaseQty = 0;
            PeriodQty = 0;
            RunTime = 0;
            VolForQty = 0;
            CostForQty = 0;
            LastQty = 0;
            APIPerQty = 0;
            Extra = new ArrayList();
            FuelRetry = false;
        }

        public FuelType(FuelType ft)
        {
            Name = ft.Name;
            itemID = ft.itemID;
            UsedFor = ft.UsedFor;
            BaseVol = ft.BaseVol;
            QtyVol = ft.QtyVol;
            Cost = ft.Cost;
            Qty = ft.Qty;
            BaseQty = ft.BaseQty;
            PeriodQty = ft.PeriodQty;
            RunTime = ft.RunTime;
            VolForQty = ft.VolForQty;
            CostForQty = ft.CostForQty;
            LastQty = ft.LastQty;
            APIPerQty = ft.APIPerQty;
            Extra = new ArrayList(ft.Extra);
            FuelRetry = ft.FuelRetry;
        }

        public void UpdateBaseValues(FuelType ft)
        {
            BaseVol = ft.BaseVol;
            BaseQty = ft.BaseQty;
            Cost = ft.Cost;
            FuelRetry = false;
        }

        public void SetFuelQtyForPeriod(decimal sov, decimal cap, decimal used, decimal period)
        {
            Qty = GetFuelQtyForPeriod(sov, cap, used) * period;
        }

        public void SetFuelQtyForPeriodFromCurrent(decimal sov, decimal cap, decimal used, decimal period, decimal cVal)
        {
            decimal newVal = GetFuelQtyForPeriod(sov, cap, used) * period;

            if (newVal > cVal)
                Qty = (newVal - cVal);
            else
                Qty = 0;
        }

        public void DecrementFuelQtyForPeriod(decimal period, decimal sov, decimal cap, decimal used)
        {
            Qty = Math.Max((Qty - (GetFuelQtyForPeriod(sov, cap, used) * period)), 0);
        }

        public void IncrementFuelQtyForPeriod(decimal period, decimal sov, decimal cap, decimal used)
        {
            Qty += (GetFuelQtyForPeriod(sov, cap, used) * period);
        }

        public decimal GetFuelQtyForPeriod(decimal sov, decimal cap, decimal used)
        {
            decimal ret, pcMult, pQty, baseMax, tMult;
            decimal mulT = 1;

            baseMax = Math.Ceiling(sov * BaseQty); // Max possible fuel use based on SOV alone.

            if ((APIPerQty <= baseMax) && (APIPerQty != 0) && (mulT <= 1))
            {
                return APIPerQty;
            }

            pQty = PeriodQty;

            if ((cap == 1) && (used == 1))
            {
                ret = Math.Ceiling(sov * pQty);
            }
            else
            {
                if (cap > 0)
                    pcMult = (used / cap);
                else
                    pcMult = 1;

                if (used > 0)
                {
                    tMult = pcMult * sov * pQty;
                    tMult = Math.Round(tMult, 2);
                    ret = Math.Ceiling(tMult);
                }
                else
                    ret = Math.Floor(pcMult * sov * pQty);
            }

            if (ret < 0)
                ret = PeriodQty;
            
            return ret;
        }

        public void SetAPIFuelUse(decimal sov)
        {
            decimal fuelUsed, mult;
            decimal BaseUse;

            // Check against max possible for tower with current SoV value
            BaseUse = GetBaseFuelQtyForPeriod(sov, 1, 1);
            fuelUsed = LastQty - Qty;

            // Negative or zero fuel usage. Either tower is not using it, or someone put fuel longo the tower
            if (fuelUsed <= 0)
                return;

            // Definitely a Bogus situation, should not ever happen, but - hehe
            if (fuelUsed > BaseUse)
                return;
    
            // If there is a current API Qty stored, then do some mild filter checking for obvious BAD readings
            if (APIPerQty != 0)
            {
                mult = APIPerQty / fuelUsed;
                if ((mult > 1) && (!FuelRetry))
                {
                    // Fuel use not constant from last valid update. Either a bad read, module state change, etc...
                    // So, make the system wait until the next read comes in. If still out then, go ahead and update
                    // the fuel usage.
                    FuelRetry = true;
                    return;
                }
            }

            // If we get here, we have filtered the garbage - so, update the fuel usage please.
            APIPerQty = fuelUsed;
            FuelRetry = false;
        }

        public decimal GetBaseFuelQtyForPeriod(decimal sov, decimal cap, decimal used)
        {
            decimal ret, pcMult, pQty;

            pQty = PeriodQty;

            ret = pQty;

            if ((cap == 1) && (used == 1))
            {
                ret = Math.Ceiling(sov * pQty);
            }
            else
            {
                if (cap > 0)
                    pcMult = (used / cap);
                else
                    pcMult = 1;

                if (used > 0)
                    ret = Math.Ceiling(pcMult * sov * pQty);
                else
                    ret = Math.Floor(pcMult * sov * pQty);
            }

            if (ret < 0)
                ret = PeriodQty;

            return ret;
        }

        public void SetFuelRunTime(decimal sov, decimal cap, decimal used)
        {
            decimal perQ;
            perQ = GetFuelQtyForPeriod(sov, cap, used);

            if (perQ > 0)
                RunTime = (Qty / perQ);
            else
                RunTime = 999999;
        }

        public decimal SetAndReturnFuelRunTime(decimal sov, decimal cap, decimal used)
        {
            decimal perQ;
            perQ = GetFuelQtyForPeriod(sov, cap, used);

            if (perQ > 0)
                RunTime = (Qty / perQ);
            else
                RunTime = 999999;

            return RunTime;
        }

        public ArrayList SetAndReturnFuelRunTimeAndName(decimal sov, decimal cap, decimal used)
        {
            ArrayList retVal = new ArrayList();
            decimal perQ;
            perQ = GetFuelQtyForPeriod(sov, cap, used);


            if (perQ > 0)
                RunTime = (Qty / perQ);
            else
                RunTime = 999999;

            retVal.Add(RunTime);
            retVal.Add(Name);

            return retVal;
        }

        public void SubtractZeroMin(decimal qt)
        {
            Qty -= qt;

            if (Qty < 0)
                Qty = 0;
        }


    }
}
