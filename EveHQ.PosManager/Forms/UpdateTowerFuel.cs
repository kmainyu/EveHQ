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
        public New_POS SelPOS;

        public UpdateTowerFuel()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        public UpdateTowerFuel(New_POS p)
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
            nud_EnrUran.Value = Convert.ToInt32(SelPOS.PosTower.Fuel.Blocks.Qty);
            nud_Charter.Value = Convert.ToInt32(SelPOS.PosTower.Fuel.Charters.Qty);
            nud_Stront.Value = Convert.ToInt32(SelPOS.PosTower.Fuel.Strontium.Qty);

            UpdateTowerFuelDisplay();
        }

        private void UpdateTowerFuelDisplay()
        {
            TFuelBay nud_fuel;
            decimal bay_p;
            decimal sov_mod, increment;

            nud_fuel = new TFuelBay(SelPOS.PosTower.Fuel);
            nud_fuel.Blocks.Qty = nud_EnrUran.Value;
            nud_fuel.Charters.Qty = nud_Charter.Value;
            nud_fuel.Strontium.Qty = nud_Stront.Value;

            SelPOS.CalculatePOSAdjustRunTime(PlugInData.Config.data.FuelCosts, nud_fuel);

            sov_mod = SelPOS.GetSovMultiple();

            // Enr Uranium
            l_C_EnUr.Text = String.Format("{0:#,0.#}", SelPOS.PosTower.Fuel.Blocks.Qty);
            l_R_EnUr.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.Fuel.Blocks.RunTime);
            increment = SelPOS.PosTower.Fuel.Blocks.GetFuelQtyForPeriod(sov_mod, 1, 1);
            nud_EnrUran.Increment = Convert.ToInt32(increment);
            l_QH_EnUr.Text = String.Format("{0:#,0.#}", increment);
            l_AR_EnUr.Text = PlugInData.ConvertHoursToTextDisplay(SelPOS.PosTower.A_Fuel.Blocks.RunTime);
            if (SelPOS.PosTower.A_Fuel.Blocks.RunTime < SelPOS.PosTower.Fuel.Blocks.RunTime)
                l_AR_EnUr.ForeColor = Color.Red;
            else
                l_AR_EnUr.ForeColor = Color.Green;
            if (nud_EnrUran.Value < SelPOS.PosTower.Fuel.Blocks.Qty)
                nud_EnrUran.ForeColor = Color.Red;
            else if (nud_EnrUran.Value > SelPOS.PosTower.Fuel.Blocks.Qty)
                nud_EnrUran.ForeColor = Color.Green;
            else
                nud_EnrUran.ForeColor = Color.Blue;

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
            // Fuel Blocks
            SelPOS.PosTower.Fuel.Blocks.Qty = SelPOS.PosTower.A_Fuel.Blocks.Qty;

            // Faction Charters
            SelPOS.PosTower.Fuel.Charters.Qty = SelPOS.PosTower.A_Fuel.Charters.Qty;

            // Strontium
            SelPOS.PosTower.Fuel.Strontium.Qty = SelPOS.PosTower.A_Fuel.Strontium.Qty;

            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

        private void b_Cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


    }
}
