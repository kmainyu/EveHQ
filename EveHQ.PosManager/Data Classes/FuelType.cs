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
            decimal ret, pcMult, pQty;

            if (PeriodQty == 0)
                pQty = APIPerQty;
            else if ((PeriodQty != 0) && (PeriodQty != APIPerQty) && (APIPerQty != 0))
                pQty = APIPerQty;
            else
                pQty = PeriodQty;

            if ((cap == 1) && (used == 1))
            {
                ret = Math.Ceiling(sov * pQty);
            }
            else
            {
                if(cap > 0)
                    pcMult = (used / cap);
                else
                    pcMult = 1;

                if ((PeriodQty == 0) || ((PeriodQty != 0) && (PeriodQty != APIPerQty) && (APIPerQty != 0)))
                    ret = APIPerQty;
                else
                {
                    if (used > 0)
                        ret = Math.Floor((pcMult * sov * pQty) + 1);
                    else
                        ret = Math.Floor(pcMult * sov * pQty);
                }
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


    }
}
