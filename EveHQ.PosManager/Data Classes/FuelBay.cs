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

        public void SetCurrentFuelCosts(FuelBay fb)
        {
            if (fb.EnrUran.Cost > 0)
                EnrUran.CostForQty = (EnrUran.Qty * fb.EnrUran.Cost);
            else
                EnrUran.CostForQty = (EnrUran.Qty * EnrUran.Cost);
            FuelCost = EnrUran.CostForQty;

            if (fb.Oxygen.Cost > 0)
                Oxygen.CostForQty = (Oxygen.Qty * fb.Oxygen.Cost);
            else
                Oxygen.CostForQty = (Oxygen.Qty * Oxygen.Cost);
            FuelCost += Oxygen.CostForQty;

            if (fb.MechPart.Cost > 0)
                MechPart.CostForQty = (MechPart.Qty * fb.MechPart.Cost);
            else
                MechPart.CostForQty = (MechPart.Qty * MechPart.Cost);
            FuelCost += MechPart.CostForQty;

            if (fb.Coolant.Cost > 0)
                Coolant.CostForQty = (Coolant.Qty * fb.Coolant.Cost);
            else
                Coolant.CostForQty = (Coolant.Qty * Coolant.Cost);
            FuelCost += Coolant.CostForQty;

            if (fb.Robotics.Cost > 0)
                Robotics.CostForQty = (Robotics.Qty * fb.Robotics.Cost);
            else
                Robotics.CostForQty = (Robotics.Qty * Robotics.Cost);
            FuelCost += Robotics.CostForQty;

            if (fb.HvyWater.Cost > 0)
                HvyWater.CostForQty = (HvyWater.Qty * fb.HvyWater.Cost);
            else
                HvyWater.CostForQty = (HvyWater.Qty * HvyWater.Cost);
            FuelCost += HvyWater.CostForQty;

            if (fb.LiqOzone.Cost > 0)
                LiqOzone.CostForQty = (LiqOzone.Qty * fb.LiqOzone.Cost);
            else
                LiqOzone.CostForQty = (LiqOzone.Qty * LiqOzone.Cost);
            FuelCost += LiqOzone.CostForQty;

            if (fb.Charters.Cost > 0)
                Charters.CostForQty = (Charters.Qty * fb.Charters.Cost);
            else
                Charters.CostForQty = (Charters.Qty * Charters.Cost);
            FuelCost += Charters.CostForQty;

            if (fb.Strontium.Cost > 0)
                Strontium.CostForQty = (Strontium.Qty * fb.Strontium.Cost);
            else
                Strontium.CostForQty = (Strontium.Qty * Strontium.Cost);

            if (fb.N2Iso.Cost > 0)
                N2Iso.CostForQty = (N2Iso.Qty * fb.N2Iso.Cost);
            else
                N2Iso.CostForQty = (N2Iso.Qty * N2Iso.Cost);
            FuelCost += N2Iso.CostForQty;

            if (fb.H2Iso.Cost > 0)
                H2Iso.CostForQty = (H2Iso.Qty * fb.H2Iso.Cost);
            else
                H2Iso.CostForQty = (H2Iso.Qty * H2Iso.Cost);
            FuelCost += H2Iso.CostForQty;

            if (fb.HeIso.Cost > 0)
                HeIso.CostForQty = (HeIso.Qty * fb.HeIso.Cost);
            else
                HeIso.CostForQty = (HeIso.Qty * HeIso.Cost);
            FuelCost += HeIso.CostForQty;

            if (fb.O2Iso.Cost > 0)
                O2Iso.CostForQty = (O2Iso.Qty * fb.O2Iso.Cost);
            else
                O2Iso.CostForQty = (O2Iso.Qty * O2Iso.Cost);
            FuelCost += O2Iso.CostForQty;

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
            EnrUran.VolForQty = GetVolumeForFuel(EnrUran.Qty, EnrUran.BaseVol, EnrUran.QtyVol);
            Oxygen.VolForQty = GetVolumeForFuel(Oxygen.Qty, Oxygen.BaseVol, Oxygen.QtyVol);
            MechPart.VolForQty = GetVolumeForFuel(MechPart.Qty, MechPart.BaseVol, MechPart.QtyVol);
            Coolant.VolForQty = GetVolumeForFuel(Coolant.Qty, Coolant.BaseVol, Coolant.QtyVol);
            Robotics.VolForQty = GetVolumeForFuel(Robotics.Qty, Robotics.BaseVol, Robotics.QtyVol);
            HvyWater.VolForQty = GetVolumeForFuel(HvyWater.Qty, HvyWater.BaseVol, HvyWater.QtyVol);
            LiqOzone.VolForQty = GetVolumeForFuel(LiqOzone.Qty, LiqOzone.BaseVol, LiqOzone.QtyVol);
            Charters.VolForQty = GetVolumeForFuel(Charters.Qty, Charters.BaseVol, Charters.QtyVol);
            N2Iso.VolForQty = GetVolumeForFuel(N2Iso.Qty, N2Iso.BaseVol, N2Iso.QtyVol);
            H2Iso.VolForQty = GetVolumeForFuel(H2Iso.Qty, H2Iso.BaseVol, H2Iso.QtyVol);
            O2Iso.VolForQty = GetVolumeForFuel(O2Iso.Qty, O2Iso.BaseVol, O2Iso.QtyVol);
            HeIso.VolForQty = GetVolumeForFuel(HeIso.Qty, HeIso.BaseVol, HeIso.QtyVol);

            if (Strontium.QtyVol > 0)
                Strontium.VolForQty = ((Strontium.Qty / Strontium.QtyVol) * Strontium.BaseVol);
            else
                Strontium.VolForQty = ((Strontium.Qty / 3) * Strontium.BaseVol);

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
            HeIso.Qty += fb.HeIso.Qty;
            H2Iso.Qty += fb.H2Iso.Qty;
            O2Iso.Qty += fb.O2Iso.Qty;
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

        public void SetFuelQtyForPeriod(decimal period, decimal sov_mult, decimal cpu_b, decimal cpu_u, decimal pow_b, decimal pow_u)
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
            string[,] retVal = new string[13,4];

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

    }
}
