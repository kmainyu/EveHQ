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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EveHQ.PosManager
{
    public partial class UpdateTowerFuel : DevComponents.DotNetBar.Office2007Form
    {
        public POS SelPOS;

        public UpdateTowerFuel()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        public UpdateTowerFuel(POS p)
        {
            InitializeComponent();
            SelPOS = p;
            this.DialogResult = DialogResult.Cancel;
            DisplayFuelValues();
        }

        private void FuelValueChanged(object sender, EventArgs e)
        {
            UpdateTowerFuelDisplay();
        }

        private void DisplayFuelValues()
        {
            nud_EnrUran.Value = Convert.ToInt32(SelPOS.PosTower.Fuel.EnrUran.Qty);
            nud_Oxy.Value = Convert.ToInt32(SelPOS.PosTower.Fuel.Oxygen.Qty);
            nud_MechPart.Value = Convert.ToInt32(SelPOS.PosTower.Fuel.MechPart.Qty);
            nud_Robotic.Value = Convert.ToInt32(SelPOS.PosTower.Fuel.Robotics.Qty);
            nud_Coolant.Value = Convert.ToInt32(SelPOS.PosTower.Fuel.Coolant.Qty);
            nud_HvyWtr.Value = Convert.ToInt32(SelPOS.PosTower.Fuel.HvyWater.Qty);
            nud_LiqOzn.Value = Convert.ToInt32(SelPOS.PosTower.Fuel.LiqOzone.Qty);
            nud_Charter.Value = Convert.ToInt32(SelPOS.PosTower.Fuel.Charters.Qty);
            nud_Stront.Value = Convert.ToInt32(SelPOS.PosTower.Fuel.Strontium.Qty);
            if (SelPOS.PosTower.Fuel.HeIso.PeriodQty > 0)
                nud_Isotope.Value = Convert.ToInt32(SelPOS.PosTower.Fuel.HeIso.Qty);
            if (SelPOS.PosTower.Fuel.H2Iso.PeriodQty > 0)
                nud_Isotope.Value = Convert.ToInt32(SelPOS.PosTower.Fuel.H2Iso.Qty);
            if (SelPOS.PosTower.Fuel.O2Iso.PeriodQty > 0)
                nud_Isotope.Value = Convert.ToInt32(SelPOS.PosTower.Fuel.O2Iso.Qty);
            if (SelPOS.PosTower.Fuel.N2Iso.PeriodQty > 0)
                nud_Isotope.Value = Convert.ToInt32(SelPOS.PosTower.Fuel.N2Iso.Qty);

            UpdateTowerFuelDisplay();
        }

        private void UpdateTowerFuelDisplay()
        {
            FuelBay nud_fuel;
            decimal bay_p;
            decimal sov_mod, increment;

            nud_fuel = new FuelBay(SelPOS.PosTower.Fuel);
            nud_fuel.EnrUran.Qty = nud_EnrUran.Value;
            nud_fuel.Oxygen.Qty = nud_Oxy.Value;
            nud_fuel.MechPart.Qty = nud_MechPart.Value;
            nud_fuel.Robotics.Qty = nud_Robotic.Value;
            nud_fuel.Coolant.Qty = nud_Coolant.Value;
            nud_fuel.HvyWater.Qty = nud_HvyWtr.Value;
            nud_fuel.LiqOzone.Qty = nud_LiqOzn.Value;
            nud_fuel.Charters.Qty = nud_Charter.Value;
            if (SelPOS.PosTower.Fuel.N2Iso.PeriodQty > 0)
                nud_fuel.N2Iso.Qty = nud_Isotope.Value;
            else if (SelPOS.PosTower.Fuel.HeIso.PeriodQty > 0)
                nud_fuel.HeIso.Qty = nud_Isotope.Value;
            else if (SelPOS.PosTower.Fuel.H2Iso.PeriodQty > 0)
                nud_fuel.H2Iso.Qty = nud_Isotope.Value;
            else if (SelPOS.PosTower.Fuel.O2Iso.PeriodQty > 0)
                nud_fuel.O2Iso.Qty = nud_Isotope.Value;
            nud_fuel.Strontium.Qty = nud_Stront.Value;

            SelPOS.CalculatePOSAdjustRunTime(PlugInData.Config.data.FuelCosts, nud_fuel);

            sov_mod = SelPOS.GetSovMultiple();

            // Enr Uranium
            l_C_EnUr.Text = String.Format("{0:#,0.#}", SelPOS.PosTower.Fuel.EnrUran.Qty);
            l_R_EnUr.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.Fuel.EnrUran.RunTime);
            increment = SelPOS.PosTower.Fuel.EnrUran.GetFuelQtyForPeriod(sov_mod, 1, 1);
            nud_EnrUran.Increment = Convert.ToInt32(increment);
            l_QH_EnUr.Text = String.Format("{0:#,0.#}", increment);
            l_AR_EnUr.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.A_Fuel.EnrUran.RunTime);
            if (SelPOS.PosTower.A_Fuel.EnrUran.RunTime < SelPOS.PosTower.Fuel.EnrUran.RunTime)
                l_AR_EnUr.ForeColor = Color.Red;
            else
                l_AR_EnUr.ForeColor = Color.Green;
            if (nud_EnrUran.Value < SelPOS.PosTower.Fuel.EnrUran.Qty)
                nud_EnrUran.ForeColor = Color.Red;
            else if (nud_EnrUran.Value > SelPOS.PosTower.Fuel.EnrUran.Qty)
                nud_EnrUran.ForeColor = Color.Green;
            else
                nud_EnrUran.ForeColor = Color.Blue;

            // Oxygen
            l_C_Oxyg.Text = String.Format("{0:#,0.#}", SelPOS.PosTower.Fuel.Oxygen.Qty);
            l_R_Oxyg.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.Fuel.Oxygen.RunTime);
            increment = SelPOS.PosTower.Fuel.Oxygen.GetFuelQtyForPeriod(sov_mod, 1, 1);
            nud_Oxy.Increment = Convert.ToInt32(increment);
            l_QH_Oxyg.Text = String.Format("{0:#,0.#}", increment);
            l_AR_Oxyg.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.A_Fuel.Oxygen.RunTime);
            if (SelPOS.PosTower.A_Fuel.Oxygen.RunTime < SelPOS.PosTower.Fuel.Oxygen.RunTime)
                l_AR_Oxyg.ForeColor = Color.Red;
            else
                l_AR_Oxyg.ForeColor = Color.Green;
            if (nud_Oxy.Value > SelPOS.PosTower.Fuel.Oxygen.Qty)
                nud_Oxy.ForeColor = Color.Green;
            else if (nud_Oxy.Value < SelPOS.PosTower.Fuel.Oxygen.Qty)
                nud_Oxy.ForeColor = Color.Red;
            else
                nud_Oxy.ForeColor = Color.Blue;

            // Mechanical Parts
            l_C_McP.Text = String.Format("{0:#,0.#}", SelPOS.PosTower.Fuel.MechPart.Qty);
            l_R_McP.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.Fuel.MechPart.RunTime);
            increment = SelPOS.PosTower.Fuel.MechPart.GetFuelQtyForPeriod(sov_mod, 1, 1);
            nud_MechPart.Increment = Convert.ToInt32(increment);
            l_QH_McP.Text = String.Format("{0:#,0.#}", increment);
            l_AR_McP.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.A_Fuel.MechPart.RunTime);
            if (SelPOS.PosTower.A_Fuel.MechPart.RunTime < SelPOS.PosTower.Fuel.MechPart.RunTime)
                l_AR_McP.ForeColor = Color.Red;
            else
                l_AR_McP.ForeColor = Color.Green;
            if (nud_MechPart.Value > SelPOS.PosTower.Fuel.MechPart.Qty)
                nud_MechPart.ForeColor = Color.Green;
            else if (nud_MechPart.Value < SelPOS.PosTower.Fuel.MechPart.Qty)
                nud_MechPart.ForeColor = Color.Red;
            else
                nud_MechPart.ForeColor = Color.Blue;

            // Coolant
            l_C_Cool.Text = String.Format("{0:#,0.#}", SelPOS.PosTower.Fuel.Coolant.Qty);
            l_R_Cool.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.Fuel.Coolant.RunTime);
            increment = SelPOS.PosTower.Fuel.Coolant.GetFuelQtyForPeriod(sov_mod, 1, 1);
            nud_Coolant.Increment = Convert.ToInt32(increment);
            l_QH_Cool.Text = String.Format("{0:#,0.#}", increment);
            l_AR_Cool.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.A_Fuel.Coolant.RunTime);
            if (SelPOS.PosTower.A_Fuel.Coolant.RunTime < SelPOS.PosTower.Fuel.Coolant.RunTime)
                l_AR_Cool.ForeColor = Color.Red;
            else
                l_AR_Cool.ForeColor = Color.Green;
            if (nud_Coolant.Value > SelPOS.PosTower.Fuel.Coolant.Qty)
                nud_Coolant.ForeColor = Color.Green;
            else if (nud_Coolant.Value < SelPOS.PosTower.Fuel.Coolant.Qty)
                nud_Coolant.ForeColor = Color.Red;
            else
                nud_Coolant.ForeColor = Color.Blue;

            // Robotics
            l_C_Robt.Text = String.Format("{0:#,0.#}", SelPOS.PosTower.Fuel.Robotics.Qty);
            l_R_Robt.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.Fuel.Robotics.RunTime);
            increment = SelPOS.PosTower.Fuel.Robotics.GetFuelQtyForPeriod(sov_mod, 1, 1);
            nud_Robotic.Increment = Convert.ToInt32(increment);
            l_QH_Robt.Text = String.Format("{0:#,0.#}", increment);
            l_AR_Robt.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.A_Fuel.Robotics.RunTime);
            if (SelPOS.PosTower.A_Fuel.Robotics.RunTime < SelPOS.PosTower.Fuel.Robotics.RunTime)
                l_AR_Robt.ForeColor = Color.Red;
            else
                l_AR_Robt.ForeColor = Color.Green;
            if (nud_Robotic.Value > SelPOS.PosTower.Fuel.Robotics.Qty)
                nud_Robotic.ForeColor = Color.Green;
            else if (nud_Robotic.Value < SelPOS.PosTower.Fuel.Robotics.Qty)
                nud_Robotic.ForeColor = Color.Red;
            else
                nud_Robotic.ForeColor = Color.Blue;

            // Faction Charters
            l_C_Chrt.Text = String.Format("{0:#,0.#}", SelPOS.PosTower.Fuel.Charters.Qty);
            l_R_Chrt.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.Fuel.Charters.RunTime);
            increment = SelPOS.PosTower.Fuel.Charters.GetFuelQtyForPeriod(sov_mod, 1, 1);
            nud_Charter.Increment = Convert.ToInt32(increment);
            l_QH_Chrt.Text = String.Format("{0:#,0.#}", increment);
            if (!SelPOS.UseChart)
            {
                l_AR_Chrt.ForeColor = Color.Blue;
                l_AR_Chrt.Text = "NA";
            }
            else
            {
                l_AR_Chrt.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.A_Fuel.Charters.RunTime);
                if (SelPOS.PosTower.A_Fuel.Charters.RunTime < SelPOS.PosTower.Fuel.Charters.RunTime)
                    l_AR_Chrt.ForeColor = Color.Red;
                else
                    l_AR_Chrt.ForeColor = Color.Green;
            }
            if (nud_Charter.Value > SelPOS.PosTower.Fuel.Charters.Qty)
                nud_Charter.ForeColor = Color.Green;
            if (nud_Charter.Value < SelPOS.PosTower.Fuel.Charters.Qty)
                nud_Charter.ForeColor = Color.Red;
            else
                nud_Charter.ForeColor = Color.Blue;

            // Strontium
            l_C_Strn.Text = String.Format("{0:#,0.#}", SelPOS.PosTower.Fuel.Strontium.Qty);
            l_R_Strn.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.Fuel.Strontium.RunTime);
            increment = SelPOS.PosTower.Fuel.Strontium.GetFuelQtyForPeriod(sov_mod, 1, 1);
            nud_Stront.Increment = Convert.ToInt32(increment);
            l_QH_Strn.Text = String.Format("{0:#,0.#}", increment);
            l_AR_Strn.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.A_Fuel.Strontium.RunTime);
            if (SelPOS.PosTower.A_Fuel.Strontium.RunTime < SelPOS.PosTower.Fuel.Strontium.RunTime)
                l_AR_Strn.ForeColor = Color.Red;
            else
                l_AR_Strn.ForeColor = Color.Green;
            if (nud_Stront.Value > SelPOS.PosTower.Fuel.Strontium.Qty)
                nud_Stront.ForeColor = Color.Green;
            else if (nud_Stront.Value < SelPOS.PosTower.Fuel.Strontium.Qty)
                nud_Stront.ForeColor = Color.Red;
            else
                nud_Stront.ForeColor = Color.Blue;

            // Heavy Water
            l_C_HvyW.Text = String.Format("{0:#,0.#}", SelPOS.PosTower.Fuel.HvyWater.Qty);
            l_R_HvyW.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.Fuel.HvyWater.RunTime);
            increment = SelPOS.PosTower.Fuel.HvyWater.GetFuelQtyForPeriod(sov_mod, SelPOS.PosTower.CPU, SelPOS.PosTower.CPU_Used);
            nud_HvyWtr.Increment = Convert.ToInt32(increment);
            l_QH_HvyW.Text = String.Format("{0:#,0.#}", increment);
            l_AR_HvyW.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.A_Fuel.HvyWater.RunTime);
            if (SelPOS.PosTower.A_Fuel.HvyWater.RunTime < SelPOS.PosTower.Fuel.HvyWater.RunTime)
                l_AR_HvyW.ForeColor = Color.Red;
            else
                l_AR_HvyW.ForeColor = Color.Green;
            if (nud_HvyWtr.Value > SelPOS.PosTower.Fuel.HvyWater.Qty)
                nud_HvyWtr.ForeColor = Color.Green;
            else if (nud_HvyWtr.Value < SelPOS.PosTower.Fuel.HvyWater.Qty)
                nud_HvyWtr.ForeColor = Color.Red;
            else
                nud_HvyWtr.ForeColor = Color.Blue;

            // Liquid Ozone
            l_C_LiqO.Text = String.Format("{0:#,0.#}", SelPOS.PosTower.Fuel.LiqOzone.Qty);
            l_R_LiqO.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.Fuel.LiqOzone.RunTime);
            increment = SelPOS.PosTower.Fuel.LiqOzone.GetFuelQtyForPeriod(sov_mod, SelPOS.PosTower.Power, SelPOS.PosTower.Power_Used);
            nud_LiqOzn.Increment = Convert.ToInt32(increment);
            l_QH_LiqO.Text = String.Format("{0:#,0.#}", increment);
            l_AR_LiqO.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.A_Fuel.LiqOzone.RunTime);
            if (SelPOS.PosTower.A_Fuel.LiqOzone.RunTime < SelPOS.PosTower.Fuel.LiqOzone.RunTime)
                l_AR_LiqO.ForeColor = Color.Red;
            else
                l_AR_LiqO.ForeColor = Color.Green;
            if (nud_LiqOzn.Value > SelPOS.PosTower.Fuel.LiqOzone.Qty)
                nud_LiqOzn.ForeColor = Color.Green;
            else if (nud_LiqOzn.Value < SelPOS.PosTower.Fuel.LiqOzone.Qty)
                nud_LiqOzn.ForeColor = Color.Red;
            else
                nud_LiqOzn.ForeColor = Color.Blue;

            // Isotopes
            if (SelPOS.PosTower.Fuel.N2Iso.PeriodQty > 0)
            {
                l_M_IsoType.Text = "N2";
                l_C_Iso.Text = String.Format("{0:#,0.#}", SelPOS.PosTower.Fuel.N2Iso.Qty);
                l_R_Iso.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.Fuel.N2Iso.RunTime);
                increment = SelPOS.PosTower.Fuel.N2Iso.GetFuelQtyForPeriod(sov_mod, 1, 1);
                nud_Isotope.Increment = Convert.ToInt32(increment);
                l_QH_Iso.Text = String.Format("{0:#,0.#}", increment);
                l_AR_Iso.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.A_Fuel.N2Iso.RunTime);
                if (SelPOS.PosTower.A_Fuel.N2Iso.RunTime < SelPOS.PosTower.Fuel.N2Iso.RunTime)
                    l_AR_Iso.ForeColor = Color.Red;
                else
                    l_AR_Iso.ForeColor = Color.Green;
                if (nud_Isotope.Value > SelPOS.PosTower.Fuel.N2Iso.Qty)
                    nud_Isotope.ForeColor = Color.Green;
                else if (nud_Isotope.Value < SelPOS.PosTower.Fuel.N2Iso.Qty)
                    nud_Isotope.ForeColor = Color.Red;
                else
                    nud_Isotope.ForeColor = Color.Blue;
            }
            else if (SelPOS.PosTower.Fuel.H2Iso.PeriodQty > 0)
            {
                l_M_IsoType.Text = "H2";
                l_C_Iso.Text = String.Format("{0:#,0.#}", SelPOS.PosTower.Fuel.H2Iso.Qty);
                l_R_Iso.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.Fuel.H2Iso.RunTime);
                increment = SelPOS.PosTower.Fuel.H2Iso.GetFuelQtyForPeriod(sov_mod, 1, 1);
                nud_Isotope.Increment = Convert.ToInt32(increment);
                l_QH_Iso.Text = String.Format("{0:#,0.#}", increment);
                l_AR_Iso.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.A_Fuel.H2Iso.RunTime);
                if (SelPOS.PosTower.A_Fuel.H2Iso.RunTime < SelPOS.PosTower.Fuel.H2Iso.RunTime)
                    l_AR_Iso.ForeColor = Color.Red;
                else
                    l_AR_Iso.ForeColor = Color.Green;
                if (nud_Isotope.Value > SelPOS.PosTower.Fuel.H2Iso.Qty)
                    nud_Isotope.ForeColor = Color.Green;
                else if (nud_Isotope.Value < SelPOS.PosTower.Fuel.H2Iso.Qty)
                    nud_Isotope.ForeColor = Color.Red;
                else
                    nud_Isotope.ForeColor = Color.Blue;
            }
            else if (SelPOS.PosTower.Fuel.O2Iso.PeriodQty > 0)
            {
                l_M_IsoType.Text = "O2";
                l_C_Iso.Text = String.Format("{0:#,0.#}", SelPOS.PosTower.Fuel.O2Iso.Qty);
                l_R_Iso.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.Fuel.O2Iso.RunTime);
                increment = SelPOS.PosTower.Fuel.O2Iso.GetFuelQtyForPeriod(sov_mod, 1, 1);
                nud_Isotope.Increment = Convert.ToInt32(increment);
                l_QH_Iso.Text = String.Format("{0:#,0.#}", increment);
                l_AR_Iso.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.A_Fuel.O2Iso.RunTime);
                if (SelPOS.PosTower.A_Fuel.O2Iso.RunTime < SelPOS.PosTower.Fuel.O2Iso.RunTime)
                    l_AR_Iso.ForeColor = Color.Red;
                else
                    l_AR_Iso.ForeColor = Color.Green;
                if (nud_Isotope.Value > SelPOS.PosTower.Fuel.O2Iso.Qty)
                    nud_Isotope.ForeColor = Color.Green;
                else if (nud_Isotope.Value < SelPOS.PosTower.Fuel.O2Iso.Qty)
                    nud_Isotope.ForeColor = Color.Red;
                else
                    nud_Isotope.ForeColor = Color.Blue;
            }
            else if (SelPOS.PosTower.Fuel.HeIso.PeriodQty > 0)
            {
                l_M_IsoType.Text = "He";
                l_C_Iso.Text = String.Format("{0:#,0.#}", SelPOS.PosTower.Fuel.HeIso.Qty);
                l_R_Iso.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.Fuel.HeIso.RunTime);
                increment = SelPOS.PosTower.Fuel.HeIso.GetFuelQtyForPeriod(sov_mod, 1, 1);
                nud_Isotope.Increment = Convert.ToInt32(increment);
                l_QH_Iso.Text = String.Format("{0:#,0.#}", increment);
                l_AR_Iso.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.A_Fuel.HeIso.RunTime);
                if (SelPOS.PosTower.A_Fuel.HeIso.RunTime < SelPOS.PosTower.Fuel.HeIso.RunTime)
                    l_AR_Iso.ForeColor = Color.Red;
                else
                    l_AR_Iso.ForeColor = Color.Green;
                if (nud_Isotope.Value > SelPOS.PosTower.Fuel.HeIso.Qty)
                    nud_Isotope.ForeColor = Color.Green;
                else if (nud_Isotope.Value < SelPOS.PosTower.Fuel.HeIso.Qty)
                    nud_Isotope.ForeColor = Color.Red;
                else
                    nud_Isotope.ForeColor = Color.Blue;
            }
            else
            {
                l_M_IsoType.Text = "";
                l_C_Iso.Text = "0";
                l_R_Iso.Text = PlugInData.ConvertHoursToTextDisplay(0);
                l_QH_Iso.Text = "0";
                l_AR_Iso.Text = PlugInData.ConvertHoursToTextDisplay(0);
                l_AR_Iso.ForeColor = Color.Green;
                nud_Isotope.Increment = 0;
                nud_Isotope.ForeColor = Color.Blue;
            }

            bay_p = ComputeBayPercentage(SelPOS.PosTower.A_Fuel.FuelUsed, SelPOS.PosTower.A_Fuel.FuelCap);
            pb_FuelBayFill.Value = Convert.ToInt32(bay_p);
            pb_FuelBayFill.Text = SelPOS.PosTower.A_Fuel.FuelUsed + " / " + SelPOS.PosTower.A_Fuel.FuelCap;

            bay_p = ComputeBayPercentage(SelPOS.PosTower.A_Fuel.StrontUsed, SelPOS.PosTower.A_Fuel.StrontCap);
            pb_StrontBayFill.Value = Convert.ToInt32(bay_p);
            pb_StrontBayFill.Text = SelPOS.PosTower.A_Fuel.StrontUsed + " / " + SelPOS.PosTower.A_Fuel.StrontCap;
        }

        private decimal ComputeBayPercentage(decimal used, decimal cap)
        {
            decimal retVal = 0;

            if (cap > 0)
                retVal = ((used / cap) * 100);

            if (retVal > 100)
                retVal = 100;

            return retVal;
        }

        private void b_SetFuelLevel_Click(object sender, EventArgs e)
        {
            // Enr Uranium
            SelPOS.PosTower.Fuel.EnrUran.Qty = SelPOS.PosTower.A_Fuel.EnrUran.Qty;

            // Oxygen
            SelPOS.PosTower.Fuel.Oxygen.Qty = SelPOS.PosTower.A_Fuel.Oxygen.Qty;

            // Mechanical Parts
            SelPOS.PosTower.Fuel.MechPart.Qty = SelPOS.PosTower.A_Fuel.MechPart.Qty;

            // Coolant
            SelPOS.PosTower.Fuel.Coolant.Qty = SelPOS.PosTower.A_Fuel.Coolant.Qty;

            // Robotics
            SelPOS.PosTower.Fuel.Robotics.Qty = SelPOS.PosTower.A_Fuel.Robotics.Qty;

            // Faction Charters
            SelPOS.PosTower.Fuel.Charters.Qty = SelPOS.PosTower.A_Fuel.Charters.Qty;

            // Strontium
            SelPOS.PosTower.Fuel.Strontium.Qty = SelPOS.PosTower.A_Fuel.Strontium.Qty;

            // Heavy Water
            SelPOS.PosTower.Fuel.HvyWater.Qty = SelPOS.PosTower.A_Fuel.HvyWater.Qty;

            // Liquid Ozone
            SelPOS.PosTower.Fuel.LiqOzone.Qty = SelPOS.PosTower.A_Fuel.LiqOzone.Qty;

            // Isotope
            if (SelPOS.PosTower.Fuel.HeIso.PeriodQty > 0)
                SelPOS.PosTower.Fuel.HeIso.Qty = SelPOS.PosTower.A_Fuel.HeIso.Qty;
            if (SelPOS.PosTower.Fuel.H2Iso.PeriodQty > 0)
                SelPOS.PosTower.Fuel.H2Iso.Qty = SelPOS.PosTower.A_Fuel.H2Iso.Qty;
            if (SelPOS.PosTower.Fuel.N2Iso.PeriodQty > 0)
                SelPOS.PosTower.Fuel.N2Iso.Qty = SelPOS.PosTower.A_Fuel.N2Iso.Qty;
            if (SelPOS.PosTower.Fuel.O2Iso.PeriodQty > 0)
                SelPOS.PosTower.Fuel.O2Iso.Qty = SelPOS.PosTower.A_Fuel.O2Iso.Qty;

            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


    }
}
