using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace EveHQ.PosManager
{
    public partial class PoSManMainForm : Form
    {
        public ArrayList MonSel_L = new ArrayList();

        POS Design = new POS();                         // New POS Structure
        CategoryList CL = new CategoryList();           // POS Categories
        public POSDesigns POSList = new POSDesigns();   // New List of POS Designs
        public TowerListing TL = new TowerListing();    // Tower Listings
        public ModuleListing ML = new ModuleListing();  // Module Listings
        private FuelBay A_Fuel = new FuelBay();         // Fuel Bay to be used for Adjustments
        private FuelBay D_Fuel = new FuelBay();         // Fuel Bay to be used for the Designer
        public API_List API_D = new API_List();         // API Tower Listing (if it Exists)
        public DPListing DPLst = new DPListing();       // Damage Profile Listing
        public AllianceList AL = new AllianceList();
        public SystemList SL = new SystemList();
        Configuration Config = new Configuration();
        public static BackgroundWorker apiWorker;
        PoS_Item api;
        FuelBay tt = null;
        bool load = false;
        bool update = false;
        bool PosChanged = false;
        private int Mon_dg_indx = 0;
        private int Fil_dg_indx = 0;
        private string Mon_dg_Pos = "";
        private string selName, AllPosFillText, SelPosFillText;
        public string CurrentName = "", NewName = "";
        enum MonDG {Name, FuelR, StrontR, State, Link, Cache, CPU, Power, EnrUr, Oxy, McP, Cool, Rbt, Iso, HvW, LqO, Cht, Strt, useC };
        enum dgPM {Name, Qty, State, Opt, fOff, dmg, rof, dps, trk, prox, swDly, Chg, cost };
        enum fillDG { Name, Loc, EnrUr, Oxy, McP, Cool, Rbt, Iso, HvW, LqO, Cht, Strt };
        enum fuelDG { type, amount, vol, cost };
    
#region Main Form Handlers

        public PoSManMainForm()
        {
            InitializeComponent();
        }
        
        private void frmMainForm_Load(object sender, EventArgs e)
        {
            load = true;

            Config.LoadConfiguration();     // Load PoS Manager Configuration Information

            CL.LoadCategoryList();          // Load Category Listing from Disk
            TL.LoadTowerListing();          // Load Tower Listing from Disk
            ML.LoadModuleListing();         // Load Tower Modules Listing from Disk
            POSList.LoadDesignListing();    // Loads Desgined POS's from Disk
            API_D.LoadAPIListing(TL);       // Load Tower API Data from Disk
            DPLst.LoadDPListing();          // Load Damage Profile List
            SL.LoadSystemListFromDisk();
            AL.LoadAllianceListFromDisk();
            PopulateDPList("Omni-Type");
            PopulateSystemList();

            // Build corp selection listing
            cb_CorpName.Items.Clear();
            cb_CorpName.Items.Add("Undefined");
            foreach (API_Data cd in API_D.apic)
                cb_CorpName.Items.Add(cd.corpName);

            // Add PoS designes into the Designer Combo-Box pull down list for Selection
            cb_PoSName.Items.Clear();
            if (POSList.Designs.Count < 1)
            {
                cb_PoSName.Text = "No Towers Created";
            }
            else
            {
                foreach (POS p in POSList.Designs)
                {
                    cb_PoSName.Items.Add(p.Name);
                }
            }
            BuildPOSListForMonitoring();
            UpdateAllTowerSovLevels();

            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);

            GetTowerItemListData();
            PopulateMonitoredPoSDisplay();
            cb_ItemType.SelectedIndex = 1;
            cb_Interval.SelectedIndex = 0;

            t_MonitorUpdate.Enabled = true;

            Mon_dg_indx = Config.data.MonSelIndex;
            Mon_dg_Pos = Config.data.SelPos;
            if (Mon_dg_Pos != "")
            {
                // Find index to selected POS
                for (int x = 0; x < dg_MonitoredTowers.Rows.Count; x++)
                {
                    if (Mon_dg_Pos == dg_MonitoredTowers.Rows[x].Cells[(int)MonDG.Name].Value.ToString())
                    {
                        Mon_dg_indx = x;
                        break;
                    }
                }
            }

            if (Mon_dg_indx >= dg_MonitoredTowers.RowCount)
                Mon_dg_indx = dg_MonitoredTowers.RowCount - 1;

            if (Mon_dg_indx >= 0)
            {
                dg_MonitoredTowers.CurrentCell = dg_MonitoredTowers.Rows[Mon_dg_indx].Cells[(int)MonDG.Name];
                Object o = new Object();
                EventArgs ea = new EventArgs();
                dg_MonitoredTowers_SelectionChanged(o, ea);
            }

            // Set default time period to FILL
            tscb_TimePeriod.SelectedIndex = (int)Config.data.maintTP;
            nud_PeriodValue.Value = Config.data.maintPV;

            load = false;
        }

        private void tb_PosManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selName == null)
                selName = "New POS";

            if (tb_PosManager.SelectedIndex == 1)
            {
                if (cb_PoSName.Items.Contains(selName))
                    cb_PoSName.SelectedItem = selName;
            }
            else if (tb_PosManager.SelectedIndex == 2)
            {
                PopulateTowerFillDG();
                if (dg_TowerFuelList.Rows.Count > Fil_dg_indx)
                {
                    dg_TowerFuelList.CurrentCell = dg_TowerFuelList.Rows[Fil_dg_indx].Cells[(int)fillDG.Name];
                    Object o = new Object();
                    EventArgs ea = new EventArgs();
                    dg_TowerFuelList_SelectionChanged(o, ea);
                }
            }
        }

        private void PoSManMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((PosChanged) && (Design.Name != "New POS") && (Design.Name != ""))
            {
                // Query user to save any changes
                DialogResult dr = MessageBox.Show("The currently Selected POS has Changed. Do you want to Save it?", "Save POS", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    foreach (POS pl in POSList.Designs)
                    {
                        if (pl.Name == Design.Name)
                        {
                            // Existing PoS being edited, need to overwrite it or something
                            POSList.Designs.Remove(pl);
                            break;
                        }
                    }
                    POSList.Designs.Add(new POS(Design));
                    POSList.SaveDesignListing();
                    PosChanged = false;
                }
                else
                {
                    PosChanged = false;
                }
            }
            Config.SaveConfiguration();
        }

        private void PopulateSystemList()
        {
            ArrayList syst = new ArrayList();

            cb_System.Items.Clear();

            foreach (string key in SL.Systems.Keys)
                syst.Add(key);

            if (syst.Count > 0)
                syst.Add("No Systems Found - Update API");

            syst.Sort();

            cb_System.Items.AddRange(syst.ToArray());

            cb_System.SelectedIndex = 0;
        }

#endregion


#region Conversions

        private string ConvertHoursToShortTextDisplay(decimal hours, int index)
        {
            string retVal = "", week = "", day = "", hour = "";
            int days = 0, weeks = 0;

            if (index == 2)
                hours = hours * (24 * 7);
            else if (index == 1)
                hours = hours * 24;

            if (index >= 2)
            {
                weeks = Convert.ToInt32(Math.Truncate(hours / (24 * 7)));
                if (weeks > 0)
                {
                    week = weeks + " Wk";
                    hours = hours - (weeks * (24 * 7));
                }
            }

            if (index >= 1)
            {
                days = Convert.ToInt32(Math.Truncate(hours / 24));
                if (days > 0)
                {
                    day = days + " Dy";
                    hours = hours - (days * 24);
                }
            }

            if (hours > 0)
                hour = hours + " Hr";

            if (weeks > 0)
                retVal = week;

            if (days > 0)
            {
                if (retVal.Length > 0)
                    retVal += ", " + day;
                else
                    retVal = day;
            }

            if (hours >= 1)
            {
                if (retVal.Length > 0)
                    retVal += ", " + hour;
                else
                    retVal = hour;
            }

            return retVal;
        }

        private string ConvertHoursToTextDisplay(decimal hours)
        {
            string retVal = "";
            int days;

            hours = Math.Floor(hours);

            days = Convert.ToInt32(Math.Truncate(hours / 24));

            if (days > 0)
            {
                retVal = String.Format("{0:0}", days) + " Days";
                hours = hours - (days * 24);
            }

            if ((days > 0) && (hours > 0))
                retVal += ", ";

            if (hours > 0)
                retVal += String.Format("{0:0}", hours) + " Hrs";

            if ((days <= 0) && (hours <= 0))
                retVal = "Zero - Shutdown";

            return retVal;
        }

        private string ConvertSecondsToTextDisplay(decimal secs)
        {
            string retVal = "";
            int hours, mins;

            hours = Convert.ToInt32(Math.Truncate(secs / 3600));

            if (hours > 0)
            {
                retVal = String.Format("{0:0}", hours) + " h";
                secs = secs - (hours * 3600);
            }

            mins = Convert.ToInt32(Math.Truncate(secs / 60));

            if (mins > 0)
            {
                if(hours > 0)
                    retVal += ", " + String.Format("{0:0}", mins) + " m";
                else
                    retVal = String.Format("{0:0}", mins) + " m";

                secs = secs - (mins * 60);
            }

            if (secs >= 1)
            {
                if ((hours > 0) || (mins > 0))
                    retVal += ", " + String.Format("{0:0}", secs) + " s";
                else
                    retVal = String.Format("{0:0}", secs) + " s";
            }

            return retVal;
        }

#endregion


#region Designer Methods

        private void DamageProfile_Change()
        {
            if (!update)
                CalculateAndDisplayEHPData();
        }

        private void nud_ThmRes_ValueChanged(object sender, EventArgs e)
        {
            if (DetermineTotalOverValue(100))
            {
                nud_ThmRes.Value -= 1;
                return;
            }
            DamageProfile_Change();
        }

        private void nud_EMRes_ValueChanged(object sender, EventArgs e)
        {
            if (DetermineTotalOverValue(100))
            {
                nud_EMRes.Value -= 1;
                return;
            }
            DamageProfile_Change();
        }

        private void nud_KinRes_ValueChanged(object sender, EventArgs e)
        {
            if (DetermineTotalOverValue(100))
            {
                nud_KinRes.Value -= 1;
                return;
            }
            DamageProfile_Change();
        }

        private void nud_ExpRes_ValueChanged(object sender, EventArgs e)
        {
            if (DetermineTotalOverValue(100))
            {
                nud_ExpRes.Value -= 1;
                return;
            }
            DamageProfile_Change();
        }

        private bool DetermineTotalOverValue(decimal val)
        {
            decimal emp, exp, kin, thm;

            emp = nud_EMRes.Value;
            exp = nud_ExpRes.Value;
            kin = nud_KinRes.Value;
            thm = nud_ThmRes.Value;

            if ((exp + emp + kin + thm) > val)
                return true;
            else
                return false;
        }

        private void cb_DamageProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            update = true;
            nud_EMRes.Value = 0;
            nud_ExpRes.Value = 0;
            nud_KinRes.Value = 0;
            nud_ThmRes.Value = 0;
            foreach (DamageProfile dl in DPLst.DPL)
            {
                if (cb_DamageProfile.Text == dl.Name)
                {
                    nud_EMRes.Value = dl.EMP;
                    nud_ExpRes.Value = dl.Exp;
                    nud_KinRes.Value = dl.Kin;
                    nud_ThmRes.Value = dl.Thm;
                    break;
                }
            }
            update = false;
            if(!load)
                CalculateAndDisplayEHPData();
        }

        private void PopulateDPList(string sel)
        {
            int ind = 0, newInd = 0;
            
            cb_DamageProfile.Items.Clear();
            foreach (DamageProfile dl in DPLst.DPL)
            {
                cb_DamageProfile.Items.Add(dl.Name);
                if (dl.Name == sel)
                    newInd = ind;
                ind++;
            }
            cb_DamageProfile.SelectedIndex = newInd;
        }

        private void b_SaveResProf_Click(object sender, EventArgs e)
        {
            DamageProfile dp = new DamageProfile();

            if ((cb_DamageProfile.Text == "Omni-Type") || (cb_DamageProfile.Text == "EM") ||
                (cb_DamageProfile.Text == "Explosive") || (cb_DamageProfile.Text == "Kinetic") ||
                (cb_DamageProfile.Text == "Thermal"))
            {
                MessageBox.Show("Sorry - You cannot Overwrite the Pre-Defined Selections.", "Save Error", MessageBoxButtons.OK);
                return;
            }

            dp.EMP = nud_EMRes.Value;
            dp.Exp = nud_ExpRes.Value;
            dp.Kin = nud_KinRes.Value;
            dp.Thm = nud_ThmRes.Value;
            dp.Name = cb_DamageProfile.Text;

            foreach (DamageProfile dl in DPLst.DPL)
            {
                if (dl.Name == dp.Name)
                {
                    DPLst.DPL.Remove(dl);
                    break;
                }
            }

            DPLst.DPL.Add(dp);
            DPLst.SaveDPListing();
            PopulateDPList(cb_DamageProfile.Text);
        }

        private decimal CalculatePosShieldRecharge()
        {
            decimal retVal = 0;

            if(Design.PosTower.Shield.Extra.Count > 0)
                retVal = (decimal)(Design.PosTower.Shield.Amount / Convert.ToDecimal(Design.PosTower.Shield.Extra[0].ToString()));

            return retVal;
        }

        private void CalculateAndDisplayEHPData()
        {
            decimal shield_ehp = 0, armor_ehp = 0, struct_ehp = 0, total_ehp = 0;
            decimal shield_hp, arm_hp, struct_hp, divis;
            decimal emp_def, exp_def, kin_def, thm_def;
            decimal emp_val, exp_val, kin_val, thm_val;

            emp_def = (nud_EMRes.Value / 100);
            exp_def = (nud_ExpRes.Value / 100);
            kin_def = (nud_KinRes.Value / 100);
            thm_def = (nud_ThmRes.Value / 100);

            emp_val = 100 - (decimal)(Design.PosTower.Shield.EMP + Design.PosTower.Shield.EMP_M);
            exp_val = 100 - (decimal)(Design.PosTower.Shield.Explosive + Design.PosTower.Shield.Explosive_M);
            kin_val = 100 - (decimal)(Design.PosTower.Shield.Kinetic + Design.PosTower.Shield.Kinetic_M);
            thm_val = 100 - (decimal)(Design.PosTower.Shield.Thermal + Design.PosTower.Shield.Thermal_M);

            shield_hp = Design.PosTower.Shield.Amount;
            divis = ((emp_def * emp_val) + (exp_def * exp_val) + (kin_def * kin_val) + (thm_def * thm_val));

            if(divis > 0)
                 shield_ehp = (shield_hp * 100) / divis;
            else
                shield_ehp = shield_hp;

            emp_val = 100 - (decimal)(Design.PosTower.Armor.EMP + Design.PosTower.Armor.EMP_M);
            exp_val = 100 - (decimal)(Design.PosTower.Armor.Explosive + Design.PosTower.Armor.Explosive_M);
            kin_val = 100 - (decimal)(Design.PosTower.Armor.Kinetic + Design.PosTower.Armor.Kinetic_M);
            thm_val = 100 - (decimal)(Design.PosTower.Armor.Thermal + Design.PosTower.Armor.Thermal_M);

            arm_hp = Design.PosTower.Armor.Amount;

            if ((emp_val != 100) || (exp_val != 100) || (kin_val != 100) || (thm_val != 00))
                divis = ((emp_def * emp_val) + (exp_def * exp_val) + (kin_def * kin_val) + (thm_def * thm_val));
            else
                divis = 0;

            if (divis > 0)
                armor_ehp = (arm_hp * 100) / divis;
            else
                armor_ehp = arm_hp;

            emp_val = 100 - (decimal)(Design.PosTower.Struct.EMP + Design.PosTower.Struct.EMP_M);
            exp_val = 100 - (decimal)(Design.PosTower.Struct.Explosive + Design.PosTower.Struct.Explosive_M);
            kin_val = 100 - (decimal)(Design.PosTower.Struct.Kinetic + Design.PosTower.Struct.Kinetic_M);
            thm_val = 100 - (decimal)(Design.PosTower.Struct.Thermal + Design.PosTower.Struct.Thermal_M);

            struct_hp = Design.PosTower.Struct.Amount;
            divis = ((emp_def * emp_val) + (exp_def * exp_val) + (kin_def * kin_val) + (thm_def * thm_val));

            if (divis > 0)
                struct_ehp = (struct_hp * 100) / divis;
            else
                struct_ehp = struct_hp;

            total_ehp = shield_ehp + armor_ehp + struct_ehp;

            l_PoS_EHP.Text = String.Format("{0:#,0.#}", total_ehp);
       }

        private void CalculatePOSData()
        {
            decimal cpu_used = 0, power_used = 0, dps = 0, rateofire = 0, dmg = 0;
            decimal total_damage = 0, total_dps = 0;
            ArrayList EM, EXP, KIN, THM;
            decimal[] topt = new decimal[2];
            EM = new ArrayList();
            EXP = new ArrayList();
            KIN = new ArrayList();
            THM = new ArrayList();
            int dg_ind;

            dg_PosMods.Rows.Clear();

            dg_ind = dg_PosMods.Rows.Add();
            if (Design.PosTower.typeID != 0)
            {
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Name].Value = Design.PosTower.Name;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Qty].Value = 1;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Value = Design.PosTower.State;

                if (Design.PosTower.State == "Online")
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.LawnGreen;
                else if (Design.PosTower.State == "Offline")
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.Gold;
                else if (Design.PosTower.State == "Reinforced")
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.LightCoral;

                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Opt].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.fOff].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dmg].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.rof].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dps].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.trk].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.prox].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.swDly].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Chg].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.cost].Value = String.Format("{0:#,0.#}", Design.PosTower.Cost) + " isk";
            }
            else
            {
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Name].Value = "No Tower";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Qty].Value = 0;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Opt].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.fOff].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dmg].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.rof].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dps].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.trk].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.prox].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.swDly].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Chg].Value = "NA";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.cost].Value = String.Format("{0:#,0.#}", 0) + " isk";
            }

            foreach (Module PMD in Design.Modules)
            {
                dg_ind = dg_PosMods.Rows.Add();
                dmg = CalculateDamage(PMD);
                rateofire = CalculateRateOfFire(PMD) / 1000;
                dps = CalculateDPS(dmg, rateofire);
                topt = GetTrackOptimalModBonus();

                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Name].Value = PMD.Name;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Qty].Value = PMD.Qty;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Value = PMD.State;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Opt].Value = String.Format("{0:#,0.#}", CalculateOptimalRange(PMD, topt) / 1000) + "km";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.fOff].Value = String.Format("{0:#,0.#}", CalculateFalloffRange(PMD) / 1000) + "km";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dmg].Value = String.Format("{0:#,0.#}", dmg);
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.rof].Value = String.Format("{0:#,0.#}", rateofire) + "s";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.dps].Value = String.Format("{0:#,0.#}", dps);
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.trk].Value = String.Format("{0:#,0.#####}", GetTracking(PMD, topt));
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.prox].Value = String.Format("{0:#,0.#}", GetProxRange(PMD));
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.swDly].Value = String.Format("{0:#,0.#}", GetSwitchDelayTime(PMD)) + "s";
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Chg].Value = PMD.Charge;
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.cost].Value = String.Format("{0:#,0.#}", PMD.Cost) + " isk";


                if (PMD.State == "Online")
                {
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.LawnGreen;
                    cpu_used += PMD.Qty * PMD.CPU_Used;
                    power_used += PMD.Qty * PMD.Power_Used;

                    for (int x = 0; x < PMD.Qty; x++)
                    {
                        if ((PMD.Bonuses.EmpDmgRes > 0) && (PMD.Bonuses.EmpDmgRes < 1))
                            EM.Add(1 - PMD.Bonuses.EmpDmgRes);
                        if ((PMD.Bonuses.ExplosiveDmgRes > 0) && (PMD.Bonuses.ExplosiveDmgRes < 1))
                            EXP.Add(1 - PMD.Bonuses.ExplosiveDmgRes);
                        if ((PMD.Bonuses.KineticDmgRes > 0) && (PMD.Bonuses.KineticDmgRes < 1))
                            KIN.Add(1 - PMD.Bonuses.KineticDmgRes);
                        if ((PMD.Bonuses.ThermalDmgRes > 0) && (PMD.Bonuses.ThermalDmgRes < 1))
                            THM.Add(1 - PMD.Bonuses.ThermalDmgRes);
                    }
                }
                else if (PMD.State == "Offline")
                {
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.Gold;
                }
                else if (PMD.State == "Reinforced")
                {
                    dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.State].Style.BackColor = Color.LightCoral;
                }

                total_damage += dmg;
                total_dps += dps;
            }

            Design.PosTower.CPU_Used = cpu_used;
            Design.PosTower.Power_Used = power_used;

            Design.PosTower.Shield.EMP_M = CalcResistanceBonus(Design.PosTower.Shield.EMP, EM);
            Design.PosTower.Shield.Kinetic_M = CalcResistanceBonus(Design.PosTower.Shield.Kinetic, KIN);
            Design.PosTower.Shield.Explosive_M = CalcResistanceBonus(Design.PosTower.Shield.Explosive, EXP);
            Design.PosTower.Shield.Thermal_M = CalcResistanceBonus(Design.PosTower.Shield.Thermal, THM);

            DisplayPOSData();
            l_PoS_DPS.Text = String.Format("{0:#,0.#}", total_damage) + "  /  " + String.Format("{0:#,0.#}", total_dps);
            CalculateAndDisplayEHPData();
            CalculateAndDisplayDesignFuelData();
        }

        private decimal[] GetTrackOptimalModBonus()
        {
            decimal[] topt = new decimal[2];
            topt[0] = 0;
            topt[1] = 0;

            foreach (Module PMD in Design.Modules)
            {
                if ((PMD.Category.Contains("Tracking")) && (PMD.State == "Online"))
                {
                    topt[0] += (decimal)PMD.Bonuses.TrtOptimal;
                    topt[1] += (decimal)PMD.Bonuses.TrtTrackSpeed;
                }
            }

            return topt;
        }

        private decimal GetProxRange(Module m)
        {
            decimal retVal = 0;

            if (m.Category.Contains("Missile"))
            {
                retVal = m.Proximity;
            }
            else if (m.Category.Contains("Hybrid"))
            {
                retVal = (m.Proximity * (decimal)((Design.PosTower.Bonuses.HybProxRange / 100) + 1));
            }
            else if (m.Category.Contains("Projectile"))
            {
                retVal = (m.Proximity * (decimal)((Design.PosTower.Bonuses.PrjProxRange / 100) + 1));
            }
            else if (m.Category.Contains("Laser"))
            {
                retVal = (m.Proximity * (decimal)((Design.PosTower.Bonuses.LaserProxRange / 100) + 1));
            }
            else
                retVal = m.Proximity;

            return retVal;
        }

        private decimal GetSwitchDelayTime(Module m)
        {
            decimal retVal = 0;
            decimal swDel;

            swDel = m.SwitchDelay;

            if (Design.PosTower.Bonuses.EWTargetSwitch != 0)
                retVal = swDel * (decimal)((Design.PosTower.Bonuses.EWTargetSwitch / 100) + 1);
            else
                retVal = swDel;

            return retVal;
        }

        private decimal GetTracking(Module m, decimal[] topt)
        {
            decimal retVal = 0;
            decimal track;

            track = (decimal)m.Tracking;

            track *= ((topt[1] / 100) + 1);

            retVal = track;

            return retVal;
        }

        private int GetChargeIndexForModule(Module m)
        {
            int chInd = -1;
            // Get Charge Data if Present
            if ((m.Charge != null) && (m.Charge != ""))
            {
                foreach (Charge ch in m.Charges)
                {
                    chInd++;
                    if (ch.Name == m.Charge)
                        break;
                }
            }

            return chInd;
        }

        private decimal CalculateOptimalRange(Module m, decimal[] topt)
        {
            decimal retVal = 0;
            decimal fltTime, vel;
            decimal ftMod, velMod;
            Charge ch;
            int cI;

            cI = GetChargeIndexForModule(m);
            if (cI < 0)
            {
                if (m.ChargeList.Count <= 0)
                {
                    retVal = (decimal)m.Optimal;
                }
                return retVal;
            }
            ch = (Charge)m.Charges[cI];

            if (m.Category.Contains("Missile"))
            {
                // Missile Battery - charges dictate range entirely, not module type
                fltTime = ch.FlightTime;
                vel = ch.Velocity;
                ftMod = (decimal)m.Bonuses.MslFlightTime;
                velMod = (decimal)m.Bonuses.MslVelocity;

                vel *= velMod;
                fltTime *= ftMod;

                velMod = (decimal)((Design.PosTower.Bonuses.MslVel / 100) + 1);
                vel *= velMod;

                retVal = vel * fltTime;
            }
            else if (m.Category.Contains("Hybrid"))
            {
                vel = m.Optimal;
                velMod = (decimal)((Design.PosTower.Bonuses.HybOpt / 100) + 1);
                retVal = vel * velMod;

                ftMod = ch.RangeMult;
                if (ftMod == 0)
                    ftMod = 1;

                fltTime = (topt[0] / 100) + 1;

                retVal = vel * velMod * ftMod * fltTime;
            }
            else if (m.Category.Contains("Projectile"))
            {
                vel = m.Optimal;
                velMod = (decimal)((Design.PosTower.Bonuses.PrjOpt / 100) + 1);
                
                ftMod = ch.RangeMult;
                if (ftMod == 0)
                    ftMod = 1;

                fltTime = (topt[0] / 100) + 1;

                retVal = vel * velMod * ftMod * fltTime;
            }
            else if (m.Category.Contains("Laser"))
            {
                vel = m.Optimal;
                velMod = (decimal)((Design.PosTower.Bonuses.LaserOpt / 100) + 1);
                retVal = vel * velMod;

                ftMod = ch.RangeMult;
                if (ftMod == 0)
                    ftMod = 1;

                fltTime = (topt[0] / 100) + 1;

                retVal = vel * velMod * ftMod * fltTime;
            }
            else
                retVal = m.Optimal;

            return retVal;
        }

        private decimal CalculateFalloffRange(Module m)
        {
            decimal retVal = 0;
            decimal vel;
            decimal ftMod, velMod;
            Charge ch;
            int cI;

            cI = GetChargeIndexForModule(m);
            if (cI < 0)
            {
                if (m.ChargeList.Count <=0)
                {
                    retVal = (decimal)m.FallOff;
                }
                return retVal;
            }
            ch = (Charge)m.Charges[cI];
      
            if (m.Category.Contains("Projectile"))
            {
                vel = m.FallOff;
                velMod = (decimal)((Design.PosTower.Bonuses.PrjFallOff / 100) + 1);

                ftMod = ch.FlyRangeMult;
                if (ftMod == 0)
                    ftMod = 1;

                retVal = vel * velMod * ftMod;
            }
            else if (m.Category.Contains("Hybrid"))
            {
                vel = m.FallOff;

                ftMod = ch.FlyRangeMult;
                if (ftMod == 0)
                    ftMod = 1;

                retVal = vel * ftMod;
            }
            else if (m.Category.Contains("Laser"))
            {
                vel = m.FallOff;

                ftMod = ch.FlyRangeMult;
                if (ftMod == 0)
                    ftMod = 1;

                retVal = vel * ftMod;
            }
            else
                retVal = m.FallOff;

            return retVal;
        }

        private decimal CalculateDamage(Module m)
        {
            decimal retVal = 0;
            decimal em, exp, kin, thm, arm, shd;
            decimal dmgMod, tDmgMod = 1;
            
            Charge ch;
            int cI;

            cI = GetChargeIndexForModule(m);
            if (cI < 0)
            {
                if (m.Category.Contains("Energy"))
                {
                    retVal = (decimal)m.EnergyNeut;
                }
               return retVal;
            }

            ch = (Charge)m.Charges[cI];

            dmgMod = (decimal)m.DamageMod;

            if (m.Category.Contains("Missile"))
            {
                dmgMod = (decimal)m.Bonuses.MslDamage;
                tDmgMod = 1;
            }
            else if (m.Category.Contains("Hybrid"))
            {
                tDmgMod = (decimal)((Design.PosTower.Bonuses.HybDmg / 100) + 1);
            }
            else if (m.Category.Contains("Projectile"))
            {
                tDmgMod = 1;
            }
            else if (m.Category.Contains("Laser"))
            {
                tDmgMod = (decimal)((Design.PosTower.Bonuses.LaserDmg / 100) +1);
            }
            if (dmgMod == 0)
                dmgMod = 1;

            em = ch.EM_Dmg;
            exp = ch.Exp_Dmg;
            kin = ch.Kin_Dmg;
            thm = ch.Thm_Dmg;
            arm = ch.Base_Armor;
            shd = ch.Base_Shield;

            em *= dmgMod * tDmgMod;
            exp *= dmgMod * tDmgMod;
            kin *= dmgMod * tDmgMod;
            thm *= dmgMod * tDmgMod;

            retVal = em + exp + kin + thm;
            
            return retVal;
        }

        private decimal CalculateRateOfFire(Module m)
        {
            decimal retVal = 0;
            decimal mROF, rofMod;

            mROF = (decimal)m.ROF;
            retVal = mROF;

            if (mROF <= 0)
                return retVal;

            if (m.Category.Contains("Missile"))
            {
                // Missile weapon
                rofMod = ((100 + (decimal)Design.PosTower.Bonuses.MslROF) / 100);
                retVal = mROF * rofMod;
            }
            else if (m.Category.Contains("Projectile"))
            {
                // Projectiles
                rofMod = ((100 + (decimal)Design.PosTower.Bonuses.PrjROF) / 100);
                retVal = mROF * rofMod;
            }
            else if (m.Category.Contains("Electronic Warfare"))
            {
                // E-War
                rofMod = ((100 + (decimal)Design.PosTower.Bonuses.EWRof) / 100);
                retVal = mROF * rofMod;
            }

            return retVal;
        }

        private decimal CalculateDPS(decimal dmg, decimal rof)
        {
            decimal retVal = 0;

            if (rof == 0)
                retVal = 0;
            else
                retVal = dmg / rof;

            return retVal;
        }

        private double CalcResistanceBonus(double res, ArrayList rl)
        {
            double pVal, cVal, bVal;

            if (rl.Count < 1)          // Sorry, No bonus to calculate
                return 0;

            bVal = res;
            pVal = res;
            for (int x = 0; x < rl.Count; x++)
            {
                cVal = ((100 - pVal) * (double)rl[x]);
                pVal += cVal;
            }

            return (pVal - bVal);
            //int n;
            //double retVal = 0;
            //double pVal, cVal, bVal;
            //double Xo;

            //n = rl.Count;
            //if (n < 1)          // Sorry, No bonus to calculate
            //    return 0;

            //Xo = (double)((100 - res)/100);
            //bVal = Xo;
            //for (int x = 0; x < n; x++)
            //{
            //    cVal = (1 + ((double)rl[x] * Math.Exp(-(Math.Pow(x, 2) / 7.1289))));
            //    pVal = (bVal * cVal) - bVal;
            //    retVal += pVal;
            //}

            //retVal *= 100;

            //return retVal;
        }

        private void DisplayPOSData()
        {
            decimal cpu, cpu_used, power, power_used, pbc, pbp;
            double calc;
            string Online;

            cpu = Design.PosTower.CPU;
            cpu_used = Design.PosTower.CPU_Used;
            power = Design.PosTower.Power;
            power_used = Design.PosTower.Power_Used;
            Online = Design.PosTower.State;

            if (Online == "Offline")
            {
                cpu = 0;
                power = 0;
            }

            l_CPU_Values.Text = cpu_used + " / " + cpu;
            l_Power_Val.Text = power_used + " / " + power;

            if (cpu_used > cpu)
                l_CPU_Values.ForeColor = Color.Red;
            else
                l_CPU_Values.ForeColor = Color.Green;

            if (power_used > power)
                l_Power_Val.ForeColor = Color.Red;
            else
                l_Power_Val.ForeColor = Color.Green;


            pbc = ComputeBayPercentage(cpu_used, cpu);
            pbp = ComputeBayPercentage(power_used, power);

            pb_CPU.Value = Convert.ToInt32(pbc);
            pb_Power.Value = Convert.ToInt32(pbp);

            l_TowerShield.Text = String.Format("{0:#,0.#}", Design.PosTower.Shield.Amount);
            calc = Design.PosTower.Shield.EMP;
            calc += Design.PosTower.Shield.EMP_M;
            l_TowerSEmRes.Text = String.Format("{0:#,0.#}", calc) + "%";

            calc = Design.PosTower.Shield.Explosive;
            calc += Design.PosTower.Shield.Explosive_M;
            l_TowerSExRes.Text = String.Format("{0:#,0.#}", calc) + "%";

            calc = Design.PosTower.Shield.Kinetic;
            calc += Design.PosTower.Shield.Kinetic_M;
            l_TowerSKRes.Text = String.Format("{0:#,0.#}", calc) + "%";

            calc = Design.PosTower.Shield.Thermal;
            calc += Design.PosTower.Shield.Thermal_M;
            l_TowerSTRes.Text = String.Format("{0:#,0.#}", calc) + "%";

            l_TowerArmor.Text = String.Format("{0:#,0.#}", Design.PosTower.Armor.Amount);
            l_TowerAEmRes.Text = String.Format("{0:#,0.#}", Design.PosTower.Armor.EMP) + "%";
            l_TowerAExRes.Text = String.Format("{0:#,0.#}", Design.PosTower.Armor.Explosive) + "%";
            l_TowerAKRes.Text = String.Format("{0:#,0.#}", Design.PosTower.Armor.Kinetic) + "%";
            l_TowerATRes.Text = String.Format("{0:#,0.#}", Design.PosTower.Armor.Thermal) + "%";

            l_TowerStruct.Text = String.Format("{0:#,0.#}", Design.PosTower.Struct.Amount);
            l_TowerStEmRes.Text = String.Format("{0:#,0.#}", Design.PosTower.Struct.EMP) + "%";
            l_TowerStExRes.Text = String.Format("{0:#,0.#}", Design.PosTower.Struct.Explosive) + "%";
            l_TowerStKRes.Text = String.Format("{0:#,0.#}", Design.PosTower.Struct.Kinetic) + "%";
            l_TowerStTRes.Text = String.Format("{0:#,0.#}", Design.PosTower.Struct.Thermal) + "%";

            l_PoS_Tank.Text = CalculatePosShieldRecharge() + " pt/sec";
        }

        private void ClearPosData()
        {
            l_CPU_Values.Text = "0 / 0";
            l_Power_Val.Text = "0 / 0";
            l_CPU_Values.ForeColor = Color.Green;
            l_Power_Val.ForeColor = Color.Green;
            pb_CPU.Value = 0;
            pb_Power.Value = 0;

            l_TowerShield.Text = String.Format("{0:#,0.#}", 0);
            l_TowerSEmRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerSExRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerSKRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerSTRes.Text = String.Format("{0:#,0.#}", 0) + "%";

            l_TowerArmor.Text = String.Format("{0:#,0.#}", 0);
            l_TowerAEmRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerAExRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerAKRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerATRes.Text = String.Format("{0:#,0.#}", 0) + "%";

            l_TowerStruct.Text = String.Format("{0:#,0.#}", 0);
            l_TowerStEmRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerStExRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerStKRes.Text = String.Format("{0:#,0.#}", 0) + "%";
            l_TowerStTRes.Text = String.Format("{0:#,0.#}", 0) + "%";

            l_AnchorTime.Text = "";
            l_OnlineTime.Text = "";
            l_Total_UnAnchor_T.Text = "";
            l_SetupTime.Text = "";
            l_PoS_EHP.Text = "";
            l_PoS_DPS.Text = "0 / 0";
            l_PoS_Tank.Text = "";
            l_Fuel_C.Text = "0 isk";
            l_POS_C.Text = "0 isk";
            l_FuelStront_C.Text = "0 isk";

        }

        private void ClearFuelData()
        {
            l_EnrUranium.Text = String.Format("{0:#,0.#}", 0);
            l_Oxygen.Text = String.Format("{0:#,0.#}", 0);
            l_MechParts.Text = String.Format("{0:#,0.#}", 0);
            l_Coolant.Text = String.Format("{0:#,0.#}", 0);
            l_Robotics.Text = String.Format("{0:#,0.#}", 0);
            l_HeavyWater.Text = String.Format("{0:#,0.#}", 0);
            l_LiquidOzone.Text = String.Format("{0:#,0.#}", 0);
            l_FactionCharters.Text = String.Format("{0:#,0.#}", 0);
            nud_StrontInterval.Value = 0;
            nud_StrontInterval.Increment = 0;
            l_Isotopes.Text = String.Format("{0:#,0.#}", 0);
            l_IsotopeType.Text = "Isotopes";
            pb_Fuel.Value = 0;
            pb_Stront.Value = 0;
            l_Fuel_Val.Text = "0 / 0";
            l_Stront_Val.Text = "0 / 0";
            l_Fuel_Val.ForeColor = Color.Green;
            l_Stront_Val.ForeColor = Color.Green;
        }

        private void ClearModListing()
        {
            dg_PosMods.Rows.Clear();
        }

        private void CalculateAndDisplayPosTimesAndCosts()
        {
            decimal totalCost = 0, onlineT = 0, anchorT = 0, unAnchorT = 0;

            totalCost += Design.PosTower.Cost;
            anchorT += Design.PosTower.Anchor_Time;

            if (Design.PosTower.State == "Online")
                onlineT += Design.PosTower.Online_Time;

            unAnchorT += Design.PosTower.UnAnchor_Time;

            foreach (Module m in Design.Modules)
            {
                totalCost += m.Cost;
                anchorT += m.Anchor_Time;
                if (m.State == "Online")
                    onlineT += m.Online_Time;

                unAnchorT += m.UnAnchor_Time;
            }

            l_POS_C.Text = String.Format("{0:#,0.#}", totalCost);
            l_AnchorTime.Text = ConvertSecondsToTextDisplay(anchorT);
            l_OnlineTime.Text = ConvertSecondsToTextDisplay(onlineT);
            l_SetupTime.Text = ConvertSecondsToTextDisplay(anchorT + onlineT);
            l_Total_UnAnchor_T.Text = ConvertSecondsToTextDisplay(unAnchorT);
        }

        private void CalculateAndDisplayDesignFuelData()
        {
            decimal fbu, sbu;
            string lin1, lin2;
            string run_time;

            Design.CalculatePOSDesignFuelValues(Config.data.FuelCosts);

            // Display Calculated Data
            l_EnrUranium.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.EnrUran.Qty);
            l_EnrUrn_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.EnrUran.CostForQty);

            l_Oxygen.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Oxygen.Qty);
            l_Oxygen_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Oxygen.CostForQty);

            l_MechParts.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.MechPart.Qty);
            l_McP_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.MechPart.CostForQty);

            l_Coolant.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Coolant.Qty);
            l_Cool_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Coolant.CostForQty);

            l_Robotics.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Robotics.Qty);
            l_Robt_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Robotics.CostForQty);

            l_HeavyWater.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.HvyWater.Qty);
            l_HvyW_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.HvyWater.CostForQty);

            l_LiquidOzone.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.LiqOzone.Qty);
            l_LiqO_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.LiqOzone.CostForQty);

            l_FactionCharters.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Charters.Qty);
            l_Chart_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Charters.CostForQty);

            l_Stront_D.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Strontium.Qty);
            l_Strnt_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.Strontium.CostForQty);

            if (Design.PosTower.D_Fuel.N2Iso.Qty > 0)
            {
                l_Isotopes.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.N2Iso.Qty);
                l_IsotopeType.Text = "N2 Iso";
                l_Iso_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.N2Iso.CostForQty);
            }
            else if (Design.PosTower.D_Fuel.H2Iso.Qty > 0)
            {
                l_Isotopes.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.H2Iso.Qty);
                l_IsotopeType.Text = "H2 Iso";
                l_Iso_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.H2Iso.CostForQty);
            }
            else if (Design.PosTower.D_Fuel.HeIso.Qty > 0)
            {
                l_Isotopes.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.HeIso.Qty);
                l_IsotopeType.Text = "He Iso";
                l_Iso_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.HeIso.CostForQty);
            }
            else if (Design.PosTower.D_Fuel.O2Iso.Qty > 0)
            {
                l_Isotopes.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.O2Iso.Qty);
                l_IsotopeType.Text = "O2 Iso";
                l_Iso_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.O2Iso.CostForQty);
            }
            else
            {
                l_Isotopes.Text = "0";
                l_IsotopeType.Text = "Unknown";
                l_Iso_C.Text = String.Format("{0:#,0.#}", 0);
            }

            l_Fuel_C.Text = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.FuelCost);
            l_FuelStront_C.Text = String.Format("{0:#,0.#}", (Design.PosTower.D_Fuel.FuelCost + Design.PosTower.D_Fuel.Strontium.CostForQty));

            fbu = ComputeBayPercentage(Design.PosTower.D_Fuel.FuelUsed, Design.PosTower.D_Fuel.FuelCap);
            sbu = ComputeBayPercentage(Design.PosTower.D_Fuel.StrontUsed, Design.PosTower.D_Fuel.StrontCap);

            pb_Fuel.Value = Convert.ToInt32(fbu);
            pb_Stront.Value = Convert.ToInt32(sbu);

            lin1 = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.FuelUsed);
            lin2 = String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.StrontUsed);
            l_Fuel_Val.Text = lin1 + " / " + String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.FuelCap);
            l_Stront_Val.Text = lin2 + " / " + String.Format("{0:#,0.#}", Design.PosTower.D_Fuel.StrontCap);

            if (Design.PosTower.D_Fuel.FuelUsed > Design.PosTower.D_Fuel.FuelCap)
                l_Fuel_Val.ForeColor = Color.Red;
            else
                l_Fuel_Val.ForeColor = Color.Green;

            if (Design.PosTower.D_Fuel.StrontUsed > Design.PosTower.D_Fuel.StrontCap)
                l_Stront_Val.ForeColor = Color.Red;
            else
                l_Stront_Val.ForeColor = Color.Green;

            if (cb_Interval.SelectedIndex != 4)
            {
                l_AmountForMax.Show();
                l_AmountForMax.BringToFront();
                run_time = ConvertHoursToShortTextDisplay(Design.PosTower.Design_Int_Qty, cb_Interval.SelectedIndex);
                l_AmountForMax.Text = run_time;
            }
            else
            {
                l_AmountForMax.Hide();
                l_AmountForMax.SendToBack();
            }

            CalculateAndDisplayPosTimesAndCosts();
        }

        private void GetTowerItemListData()
        {
            cb_ItemType.Items.Clear();

            foreach (CategoryItem ci in CL.Cats)
            {
                cb_ItemType.Items.Add(ci.CatName);
            }
            cb_ItemType.SelectedIndex = 1;
        }

        private void cb_ItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string imgLoc;
            CategoryItem ci;
            int selIndex;
            int selGroup;
            int imgIndx = 0;
            Bitmap bmp;
            ListViewItem lvi;

            selIndex = cb_ItemType.SelectedIndex;
            ci = (CategoryItem)CL.Cats[selIndex];
            selGroup = ci.groupID;

            lv_ItemSelList.Items.Clear();
            il_SelCat.Images.Clear();

            if (cb_ItemType.Text.Contains("Tower"))
            {
                foreach (Tower t in TL.Towers)
                {
                    imgLoc = EveHQ.Core.ImageHandler.GetImageLocation(t.typeID.ToString(), Convert.ToInt32(EveHQ.Core.ImageHandler.ImageType.Types));

                    try
                    {
                        bmp = new Bitmap(Image.FromFile(imgLoc));
                        il_SelCat.Images.Add(bmp);
                    }
                    catch
                    {
                        il_SelCat.Images.Add(il_system.Images[0]);
                    }

                    lvi = new ListViewItem(t.Name);
                    lvi.Name = cb_ItemType.Text;
                    lvi.ImageIndex = imgIndx;
                    lvi.SubItems.Add(t.typeID.ToString());
                    lvi.SubItems[1].Name = t.groupID.ToString();
                    lv_ItemSelList.Items.Add(lvi);
                    imgIndx++;
                }
            }
            else
            {
                foreach (Module m in ML.Modules)
                {
                    if (selGroup == m.groupID)
                    {
                        imgLoc = EveHQ.Core.ImageHandler.GetImageLocation(m.typeID.ToString(), Convert.ToInt32(EveHQ.Core.ImageHandler.ImageType.Types));

                        try
                        {
                            bmp = new Bitmap(Image.FromFile(imgLoc));
                            il_SelCat.Images.Add(bmp);
                        }
                        catch
                        {
                            il_SelCat.Images.Add(il_system.Images[0]);
                        }

                        lvi = new ListViewItem(m.Name);
                        lvi.Name = cb_ItemType.Text;
                        lvi.ImageIndex = imgIndx;
                        lvi.SubItems.Add(m.typeID.ToString());
                        lvi.SubItems[1].Name = m.groupID.ToString();
                        lv_ItemSelList.Items.Add(lvi);
                        imgIndx++;
                    }
                }
            }
        }

        private void lv_ItemSelList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DragDropEffects DDE;
            ListViewItem LVI;
            int typeID;
            string location;
            decimal st_int, mx_strnt;

            DDE = DoDragDrop(e.Item, DragDropEffects.Copy);

            if (DDE == DragDropEffects.Copy)
            {
                LVI = (ListViewItem)e.Item;
                typeID = Convert.ToInt32(LVI.SubItems[1].Text);
                location = LVI.SubItems[1].Name.ToString();

                if (LVI.Name.Contains("Tower"))
                {
                    foreach (Tower t in TL.Towers)
                    {
                        if (t.typeID == typeID)
                        {
                            Design.PosTower = new Tower(t);
                            Design.PosTower.Location = location;
                            Design.PosTower.Category = cb_ItemType.Text;
                            Design.PosTower.Design_Int_Qty = 1;
                            Design.PosTower.Design_Interval = cb_Interval.SelectedIndex;
                            mx_strnt = (Design.PosTower.D_Fuel.StrontCap / (Design.PosTower.D_Fuel.Strontium.QtyVol * Design.PosTower.D_Fuel.Strontium.BaseVol));
                            st_int = Math.Floor(mx_strnt / Design.PosTower.D_Fuel.Strontium.PeriodQty);
                            nud_StrontInterval.Maximum = st_int;
                            Design.PosTower.Design_Stront_Qty = 0;
                            if (!load)
                                PosChanged = true;
                        }
                    }
                }
                else
                {
                    foreach (Module m in ML.Modules)
                    {
                        if (m.typeID == typeID)
                        {
                            Module nw = new Module(m);
                            nw.Location = location;
                            nw.Category = cb_ItemType.Text;
                            Design.Modules.Add(nw);
                            if (!load)
                                PosChanged = true;
                        }
                    }
                }
                CalculatePOSData();
            }
        }

        private void GetItemData(ListViewItem LVI)
        {
            int typeID;

            typeID = Convert.ToInt32(LVI.SubItems[1].Text);

            GetItemData(typeID, cb_ItemType.Text);
        }

        private void GetItemData(int typeID, string cat)
        {
            string fuel = "";

            if (cat.Contains("Tower"))
            {
                foreach (Tower t in TL.Towers)
                {
                    if (t.typeID == typeID)
                    {
                        if (t.CPU > t.CPU_Used)
                        {
                            l_cpu.ForeColor = Color.Green;
                            l_cpu.Text = String.Format("{0:#,0.#}", t.CPU);
                        }
                        else
                        {
                            l_cpu.ForeColor = Color.Red;
                            l_cpu.Text = String.Format("{0:#,0.#}", t.CPU_Used);
                        }

                        if (t.Power > t.Power_Used)
                        {
                            l_power.ForeColor = Color.Green;
                            l_power.Text = String.Format("{0:#,0.#}", t.Power);
                        }
                        else
                        {
                            l_power.ForeColor = Color.Red;
                            l_power.Text = String.Format("{0:#,0.#}", t.Power_Used);
                        }

                        l_armor_hp.Text = String.Format("{0:#,0.#}", t.Armor.Amount);
                        l_shield_hp.Text = String.Format("{0:#,0.#}", t.Shield.Amount);
                        l_shield_em.Text = String.Format("{0:#,0.#}", t.Shield.EMP) + "%";
                        l_shield_exp.Text = String.Format("{0:#,0.#}", t.Shield.Explosive) + "%";
                        l_shield_kin.Text = String.Format("{0:#,0.#}", t.Shield.Kinetic) + "%";
                        l_shield_therm.Text = String.Format("{0:#,0.#}", t.Shield.Thermal) + "%";
                        l_struct_hp.Text = String.Format("{0:#,0.#}", t.Struct.Amount);
                        l_struct_emp.Text = String.Format("{0:#,0.#}", t.Struct.EMP) + "%";
                        l_struct_exp.Text = String.Format("{0:#,0.#}", t.Struct.Explosive) + "%";
                        l_struct_kin.Text = String.Format("{0:#,0.#}", t.Struct.Kinetic) + "%";
                        l_struct_therm.Text = String.Format("{0:#,0.#}", t.Struct.Thermal) + "%";
                        l_SI_Cost.Text = String.Format("{0:#,0.#}", t.Cost) + " isk";

                        fuel += "Fuel Usage -->\n";
                        fuel += "Enriched Uranium: " + t.Fuel.EnrUran.PeriodQty + "\n";
                        fuel += "Oxygen: " + t.Fuel.Oxygen.PeriodQty + "\n";
                        fuel += "Mechanical Parts: " + t.Fuel.MechPart.PeriodQty + "\n";
                        fuel += "Coolant: " + t.Fuel.Coolant.PeriodQty + "\n";
                        fuel += "Robotics: " + t.Fuel.Robotics.PeriodQty + "\n";
                        fuel += "Heavy Water: " + t.Fuel.HvyWater.PeriodQty + "\n";
                        fuel += "Liquid Ozone: " + t.Fuel.LiqOzone.PeriodQty + "\n";
                        fuel += "Charters: " + t.Fuel.Charters.PeriodQty + "\n";
                        fuel += "Strontium: " + t.Fuel.Strontium.PeriodQty + "\n";

                        if (t.Fuel.N2Iso.PeriodQty > 0)
                        {
                            fuel += "Nitrogen Isotopes: " + t.Fuel.N2Iso.PeriodQty + "\n";
                        }
                        else if (t.Fuel.HeIso.PeriodQty > 0)
                        {
                            fuel += "Helium Isotopes: " + t.Fuel.HeIso.PeriodQty + "\n";
                        }
                        else if (t.Fuel.H2Iso.PeriodQty > 0)
                        {
                            fuel += "Hydrogen Isotopes: " + t.Fuel.H2Iso.PeriodQty + "\n";
                        }
                        else if (t.Fuel.O2Iso.PeriodQty > 0)
                        {
                            fuel += "Oxygen Isotopes: " + t.Fuel.O2Iso.PeriodQty + "\n";
                        }

                        rtb_Other.Text = t.Desc + "\n\n" + fuel + "\n" + t.OtherInfo;
                    }
                }
            }
            else
            {
                foreach (Module m in ML.Modules)
                {
                    if (m.typeID == typeID)
                    {
                        if (m.CPU > m.CPU_Used)
                        {
                            l_cpu.ForeColor = Color.Green;
                            l_cpu.Text = String.Format("{0:#,0.#}", m.CPU);
                        }
                        else
                        {
                            l_cpu.ForeColor = Color.Red;
                            l_cpu.Text = String.Format("{0:#,0.#}", m.CPU_Used);
                        }

                        if (m.Power > m.Power_Used)
                        {
                            l_power.ForeColor = Color.Green;
                            l_power.Text = String.Format("{0:#,0.#}", m.Power);
                        }
                        else
                        {
                            l_power.ForeColor = Color.Red;
                            l_power.Text = String.Format("{0:#,0.#}", m.Power_Used);
                        }

                        l_armor_hp.Text = String.Format("{0:#,0.#}", m.Armor.Amount);
                        l_shield_hp.Text = String.Format("{0:#,0.#}", m.Shield.Amount);
                        l_shield_em.Text = String.Format("{0:#,0.#}", m.Shield.EMP) + "%";
                        l_shield_exp.Text = String.Format("{0:#,0.#}", m.Shield.Explosive) + "%";
                        l_shield_kin.Text = String.Format("{0:#,0.#}", m.Shield.Kinetic) + "%";
                        l_shield_therm.Text = String.Format("{0:#,0.#}", m.Shield.Thermal) + "%";
                        l_struct_hp.Text = String.Format("{0:#,0.#}", m.Struct.Amount);
                        l_struct_emp.Text = String.Format("{0:#,0.#}", m.Struct.EMP) + "%";
                        l_struct_exp.Text = String.Format("{0:#,0.#}", m.Struct.Explosive) + "%";
                        l_struct_kin.Text = String.Format("{0:#,0.#}", m.Struct.Kinetic) + "%";
                        l_struct_therm.Text = String.Format("{0:#,0.#}", m.Struct.Thermal) + "%";
                        l_SI_Cost.Text = String.Format("{0:#,0.#}", m.Cost) + " isk";

                        rtb_Other.Text = m.Desc + "\n\n" + fuel + "\n" + m.OtherInfo;
                    }
                }
            }
        }

        private void GetDesignItemData(int typeID, string cat)
        {
            string fuel = "";

            if (cat.Contains("Tower"))
            {
                foreach (Tower t in TL.Towers)
                {
                    if (t.typeID == typeID)
                    {
                        if (t.CPU > t.CPU_Used)
                        {
                            l_cpu.ForeColor = Color.Green;
                            l_cpu.Text = String.Format("{0:#,0.#}", t.CPU);
                        }
                        else
                        {
                            l_cpu.ForeColor = Color.Red;
                            l_cpu.Text = String.Format("{0:#,0.#}", t.CPU_Used);
                        }

                        if (t.Power > t.Power_Used)
                        {
                            l_power.ForeColor = Color.Green;
                            l_power.Text = String.Format("{0:#,0.#}", t.Power);
                        }
                        else
                        {
                            l_power.ForeColor = Color.Red;
                            l_power.Text = String.Format("{0:#,0.#}", t.Power_Used);
                        }

                        l_armor_hp.Text = String.Format("{0:#,0.#}", t.Armor.Amount);
                        l_shield_hp.Text = String.Format("{0:#,0.#}", t.Shield.Amount);
                        l_shield_em.Text = String.Format("{0:#,0.#}", t.Shield.EMP) + "%";
                        l_shield_exp.Text = String.Format("{0:#,0.#}", t.Shield.Explosive) + "%";
                        l_shield_kin.Text = String.Format("{0:#,0.#}", t.Shield.Kinetic) + "%";
                        l_shield_therm.Text = String.Format("{0:#,0.#}", t.Shield.Thermal) + "%";
                        l_struct_hp.Text = String.Format("{0:#,0.#}", t.Struct.Amount);
                        l_struct_emp.Text = String.Format("{0:#,0.#}", t.Struct.EMP) + "%";
                        l_struct_exp.Text = String.Format("{0:#,0.#}", t.Struct.Explosive) + "%";
                        l_struct_kin.Text = String.Format("{0:#,0.#}", t.Struct.Kinetic) + "%";
                        l_struct_therm.Text = String.Format("{0:#,0.#}", t.Struct.Thermal) + "%";
                        l_SI_Cost.Text = String.Format("{0:#,0.#}", t.Cost) + " isk";

                        fuel += "Fuel Usage -->\n";
                        fuel += "Enriched Uranium: " + t.Fuel.EnrUran.PeriodQty + "\n";
                        fuel += "Oxygen: " + t.Fuel.Oxygen.PeriodQty + "\n";
                        fuel += "Mechanical Parts: " + t.Fuel.MechPart.PeriodQty + "\n";
                        fuel += "Coolant: " + t.Fuel.Coolant.PeriodQty + "\n";
                        fuel += "Robotics: " + t.Fuel.Robotics.PeriodQty + "\n";
                        fuel += "Heavy Water: " + t.Fuel.HvyWater.PeriodQty + "\n";
                        fuel += "Liquid Ozone: " + t.Fuel.LiqOzone.PeriodQty + "\n";
                        fuel += "Charters: " + t.Fuel.Charters.PeriodQty + "\n";
                        fuel += "Strontium: " + t.Fuel.Strontium.PeriodQty + "\n";

                        if (t.Fuel.N2Iso.PeriodQty > 0)
                        {
                            fuel += "Nitrogen Isotopes: " + t.Fuel.N2Iso.PeriodQty + "\n";
                        }
                        else if (t.Fuel.HeIso.PeriodQty > 0)
                        {
                            fuel += "Helium Isotopes: " + t.Fuel.HeIso.PeriodQty + "\n";
                        }
                        else if (t.Fuel.H2Iso.PeriodQty > 0)
                        {
                            fuel += "Hydrogen Isotopes: " + t.Fuel.H2Iso.PeriodQty + "\n";
                        }
                        else if (t.Fuel.O2Iso.PeriodQty > 0)
                        {
                            fuel += "Oxygen Isotopes: " + t.Fuel.O2Iso.PeriodQty + "\n";
                        }

                        rtb_Other.Text = t.Desc + "\n\n" + fuel + "\n" + t.OtherInfo;
                    }
                }
            }
            else
            {
                foreach (Module m in ML.Modules)
                {
                    if (m.typeID == typeID)
                    {
                        if (m.CPU > m.CPU_Used)
                        {
                            l_cpu.ForeColor = Color.Green;
                            l_cpu.Text = String.Format("{0:#,0.#}", m.CPU);
                        }
                        else
                        {
                            l_cpu.ForeColor = Color.Red;
                            l_cpu.Text = String.Format("{0:#,0.#}", m.CPU_Used);
                        }

                        if (m.Power > m.Power_Used)
                        {
                            l_power.ForeColor = Color.Green;
                            l_power.Text = String.Format("{0:#,0.#}", m.Power);
                        }
                        else
                        {
                            l_power.ForeColor = Color.Red;
                            l_power.Text = String.Format("{0:#,0.#}", m.Power_Used);
                        }

                        l_armor_hp.Text = String.Format("{0:#,0.#}", m.Armor.Amount);
                        l_shield_hp.Text = String.Format("{0:#,0.#}", m.Shield.Amount);
                        l_shield_em.Text = String.Format("{0:#,0.#}", m.Shield.EMP) + "%";
                        l_shield_exp.Text = String.Format("{0:#,0.#}", m.Shield.Explosive) + "%";
                        l_shield_kin.Text = String.Format("{0:#,0.#}", m.Shield.Kinetic) + "%";
                        l_shield_therm.Text = String.Format("{0:#,0.#}", m.Shield.Thermal) + "%";
                        l_struct_hp.Text = String.Format("{0:#,0.#}", m.Struct.Amount);
                        l_struct_emp.Text = String.Format("{0:#,0.#}", m.Struct.EMP) + "%";
                        l_struct_exp.Text = String.Format("{0:#,0.#}", m.Struct.Explosive) + "%";
                        l_struct_kin.Text = String.Format("{0:#,0.#}", m.Struct.Kinetic) + "%";
                        l_struct_therm.Text = String.Format("{0:#,0.#}", m.Struct.Thermal) + "%";
                        l_SI_Cost.Text = String.Format("{0:#,0.#}", m.Cost) + " isk";

                        rtb_Other.Text = m.Desc + "\n\n" + fuel + "\n" + m.OtherInfo;
                    }
                }
            }
        }

        private void lv_ItemSelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView LV;
            ListViewItem LVI;

            LV = (ListView)sender;
            LVI = (ListViewItem)LV.FocusedItem;

            if (LVI != null)
            {
                GetItemData(LVI);
            }
        }

        private void PoS_Item_MouseDown(object sender, MouseEventArgs e)
        {
            DragDropEffects DDE;
            string srcName, location;
            int typeID;
            PoS_Item pi = (PoS_Item)sender;
            PoS_Item piOld;

            if (pi.typeID == 0)
                return;

            if (e.Button == MouseButtons.Right)
                return;

            srcName = pi.contName;
            DDE = DoDragDrop(pi, DragDropEffects.Copy);

            if (DDE == DragDropEffects.Copy)
            {
                typeID = pi.typeID;
                location = pi.contName;

                if (!pi.ModName.Contains("Tower"))
                {
                    if (pi.DragType == "Move")
                    {
                        foreach (Module m in Design.Modules)
                        {
                            if ((m.typeID == typeID) && (m.Location == srcName))
                            {
                                m.Location = location;
                                break;
                            }
                        }
                        piOld = (PoS_Item)p_Tower.Controls[srcName];
                        piOld.RemoveItemFromControl();
                    }
                    else   // Copy
                    {
                        foreach (Module m in Design.Modules)
                        {
                            if (m.typeID == typeID)
                            {
                                Module nw = new Module(m);
                                nw.Location = pi.dstName;
                                Design.Modules.Add(nw);
                                break;
                            }
                        }
                        foreach (Control c in p_Tower.Controls)
                        {
                            if (c.GetType().Name == "PoS_Item")
                            {
                                PoS_Item piC = (PoS_Item)c;
                                if (piC.typeID != 0)
                                {
                                    piC.DragType = "Move";
                                    piC.dstName = "";
                                }
                            }
                        }
                    }

                    if (!load)
                        PosChanged = true;
                }
                CalculatePOSData();
            }
            else if (e.Button == MouseButtons.Left)
            {
                GetDesignItemData(pi.typeID, pi.catName);
            }
        }

        private void cb_PoSName_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal mx_strnt, st_int;

            if ((PosChanged) && (Design.Name != "New POS") && (Design.Name != ""))
            {
                // Query user to save any changes
                DialogResult dr = MessageBox.Show("The currently Selected POS has Changed. Do you want to Save it?", "Save POS", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    foreach (POS pl in POSList.Designs)
                    {
                        if (pl.Name == Design.Name)
                        {
                            // Existing PoS being edited, need to overwrite it or something
                            POSList.Designs.Remove(pl);
                            break;
                        }
                    }

                    POSList.Designs.Add(new POS(Design));

                    POSList.SaveDesignListing();
                    BuildPOSListForMonitoring();
                    POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
                    PosChanged = false;
                }
                else
                {
                    POSList.LoadDesignListing();
                    PosChanged = false;
                }
            }

            load = true;

            CurrentName = cb_PoSName.Text;
            // 1. Get new PoS Data from List
            foreach (POS pl in POSList.Designs)
            {
                if (pl.Name == CurrentName)
                {
                    // 2. Clear Current PoS Display
                    foreach (Control c in p_Tower.Controls)
                    {
                        if (c.GetType().Name == "PoS_Item")
                        {
                            PoS_Item pi = (PoS_Item)c;
                            pi.RemoveItemFromControl();
                        }
                    }

                    // 3. Populate PoS Display with PoS Information
                    Design = new POS(pl);

                    // Populate POS
                    if (Design.PosTower.typeID != 0)
                    {
                        // First Populate the Tower itself - as there is only 1
                        pi_Tower.typeID = Design.PosTower.typeID;
                        pi_Tower.contName = "pi_Tower";
                        pi_Tower.number = 1;

                        if (Design.PosTower.State == "Online")
                            pi_Tower.onOff = Color.Green;
                        else
                            pi_Tower.onOff = Color.Red;

                        pi_Tower.catName = Design.PosTower.Category;
                        pi_Tower.ModName = Design.PosTower.Name;
                        pi_Tower.SetToolTipForItem(pl.PosTower.Name);
                        pi_Tower.UpdateItemState();

                        mx_strnt = (Design.PosTower.D_Fuel.StrontCap / (Design.PosTower.D_Fuel.Strontium.QtyVol * Design.PosTower.D_Fuel.Strontium.BaseVol));
                        st_int = Math.Floor(mx_strnt / Design.PosTower.D_Fuel.Strontium.PeriodQty);
                        nud_StrontInterval.Maximum = st_int;

                        foreach (Module mc in Design.Modules)
                        {
                            // Now Populate each Module in the POS design
                            PoS_Item pi;

                            pi = (PoS_Item)p_Tower.Controls[mc.Location];

                            pi.typeID = mc.typeID;
                            pi.contName = mc.Location;
                            pi.number = (int)mc.Qty;

                            if (mc.State == "Online")
                                pi.onOff = Color.Green;
                            else
                                pi.onOff = Color.Red;

                            pi.catName = mc.Category;
                            pi.ModName = mc.Name;
                            pi.SetToolTipForItem(mc.Name);
                            pi.UpdateItemState();

                        }
                    }
                }
            }

            if (Design.PosTower.typeID != 0)
            {
                cb_Interval.SelectedIndex = (int)Design.PosTower.Design_Interval;
                nud_StrontInterval.Value = Design.PosTower.Design_Stront_Qty;
            }
            else
            {
                cb_Interval.SelectedIndex = 1;
                nud_StrontInterval.Value = 0;
            }

            cb_SovLevel.SelectedIndex = Design.SovLevel;
            cb_System.Text = Design.System;
            cb_CorpName.Text = Design.CorpName;

            load = false;
            CalculatePOSData();
        }

        private void SetModuleQuantity(object sender, EventArgs e)
        {
            int newNum;

            if (api.typeID == 0)
                return;

            newNum = Convert.ToInt32(sender.ToString());

            if (api.TypeKey != EveHQ.PosManager.PoS_Item.TypeKeyEnum.Tower)
            {
                api.number = newNum;
                api.UpdateItemState();

                foreach (Module m in Design.Modules)
                {
                    if ((m.typeID == api.typeID) && (api.contName == m.Location))
                    {
                        m.Qty = api.number;
                    }
                }
                if (!load)
                    PosChanged = true;
                CalculatePOSData();
            }
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

        private void cb_SovLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Design.SovLevel = cb_SovLevel.SelectedIndex;

            if (!load)
                PosChanged = true;
            CalculatePOSData();
        }

        private void cb_System_SelectedIndexChanged(object sender, EventArgs e)
        {
            Design.System = cb_System.Text;

            if (!load)
                PosChanged = true;

            CalculatePOSData();
        }

        private void cb_CorpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Design.CorpName != cb_CorpName.Text)
            {
                Design.CorpName = cb_CorpName.Text;

                foreach (API_Data cn in API_D.apic)
                    if (cn.corpName == Design.CorpName)
                    {
                        if (Design.corpID != cn.corpID)
                        {
                            // Update the corpID for the POS and Save to Disk
                            Design.corpID = cn.corpID;
                            POSList.SaveDesignListing();
                        }
                        break;
                    }
            }

            UpdateLinkedTowerSovLevel(Design);

            if (!load)
                PosChanged = true;

            CalculatePOSData();
        }


#endregion


#region PoS Monitor Routines

        private void UpdateAllTowerSovLevels()
        {
            foreach (POS p in POSList.Designs)
            {
                UpdateLinkedTowerSovLevel(p);
            }
        }

        private void UpdateLinkedTowerSovLevel(POS p)
        {
            decimal corpID;
            Sov_Data sd;
            Alliance_Data ad;

            if (p.corpID != 0)
            {
                // This POS is Linked
                corpID = p.corpID;

                ad = (Alliance_Data)AL.alliances[corpID];
                if (ad != null)
                {
                    // Corp is in an alliance
                    // Find system in system list, update SOV level accordingly
                    sd = (Sov_Data)SL.Systems[p.System];
                    if (sd.allianceID == ad.allianceID)
                    {
                        // Found the correct system and alliance ID
                        p.SovLevel = (int)sd.sovLevel;
                    }
                }
            }
        }

        public POS GetPoSListingForPoS(string name)
        {
            foreach (POS pl in POSList.Designs)
            {
                if (name == pl.Name)
                    return pl;
            }
            return null;
        }
        
        private void BuildPOSListForMonitoring()
        {
            MonSel_L.Clear();
            foreach (POS pl in POSList.Designs)
                MonSel_L.Add(pl.Name);
        }
        
        private void PopulateMonitoredPoSDisplay()
        {
            API_Data apid;
            int dg_ind;
            string line, cache;
            DateTime now, cTim;

            dg_MonitoredTowers.Rows.Clear();

            foreach (POS p in POSList.Designs)
            {
                if (p.Monitored)
                {
                    // Add data to DG
                    dg_ind = dg_MonitoredTowers.Rows.Add();

                    line = p.Name + " < " + p.System + " >[" + p.SovLevel + "]";
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Name].Value = line;
                    line = ConvertHoursToTextDisplay(p.PosTower.F_RunTime);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.FuelR].Value = line;
                    line = ConvertHoursToTextDisplay(p.PosTower.Fuel.Strontium.RunTime);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.StrontR].Value = line;

                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.State].Value = p.PosTower.State;

                    if (p.itemID != 0)
                    {
                        apid = API_D.GetAPIDataMemberForTowerID(p.itemID);
                        line = "<" + apid.locName + "> ";
                        line += apid.towerName;

                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Link].Value = line;

                        cache = apid.cacheUntil;
                        cTim = Convert.ToDateTime(cache);
                        cTim = TimeZone.CurrentTimeZone.ToLocalTime(cTim);
                        now = DateTime.Now;

                        if(cTim.CompareTo(now) <= 0)
                        {
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cache].Value = cache;
                            // Cache can be updated, highlight red
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cache].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cache].Value = cache;
                            // Cache is up to date, highlight green
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cache].Style.ForeColor = Color.Green;
                        }
                    }
                    else
                    {
                        line = "<Not Linked>";
                        cache = "";
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Link].Value = line;
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cache].Value = cache;
                    }


                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.CPU].Value = String.Format("{0:#,0.#}", p.PosTower.CPU_Used) + " / " + String.Format("{0:#,0.#}", p.PosTower.CPU);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Power].Value = String.Format("{0:#,0.#}", p.PosTower.Power_Used) + " / " + String.Format("{0:#,0.#}", p.PosTower.Power);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.EnrUr].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.EnrUran.Qty);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Oxy].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.Oxygen.Qty);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.McP].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.MechPart.Qty);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cool].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.Coolant.Qty);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Rbt].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.Robotics.Qty);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.HvW].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.HvyWater.Qty);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.LqO].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.LiqOzone.Qty);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cht].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.Charters.Qty);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Strt].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.Strontium.Qty);

                    if (p.PosTower.Fuel.HeIso.PeriodQty > 0)
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.HeIso.Qty) + " He";
                    else if (p.PosTower.Fuel.H2Iso.PeriodQty > 0)
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.H2Iso.Qty) + " H2";
                    else if (p.PosTower.Fuel.N2Iso.PeriodQty > 0)
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.N2Iso.Qty) + " N2";
                    else if (p.PosTower.Fuel.O2Iso.PeriodQty > 0)
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.O2Iso.Qty) + " O2";
                    else
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Value = String.Format("{0:#,0.#}", 0) + " ??";

                    if (p.PosTower.State == "Online")
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.State].Style.BackColor = Color.LawnGreen;
                        if (p.PosTower.F_RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.FuelR].Style.BackColor = Color.Red;
                        if (p.PosTower.Fuel.EnrUran.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.EnrUr].Style.BackColor = Color.Red;
                        if (p.PosTower.Fuel.Oxygen.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Oxy].Style.BackColor = Color.Red;
                        if (p.PosTower.Fuel.MechPart.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.McP].Style.BackColor = Color.Red;
                        if (p.PosTower.Fuel.Coolant.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cool].Style.BackColor = Color.Red;
                        if (p.PosTower.Fuel.Robotics.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Rbt].Style.BackColor = Color.Red;
                        if (p.PosTower.Fuel.HvyWater.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.HvW].Style.BackColor = Color.Red;
                        if (p.PosTower.Fuel.LiqOzone.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.LqO].Style.BackColor = Color.Red;
                        if (p.UseChart)
                            if (p.PosTower.Fuel.Charters.RunTime < 24)
                                dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cht].Style.BackColor = Color.Red;
                        if (p.PosTower.Fuel.Strontium.RunTime < 4)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Strt].Style.BackColor = Color.Red;

                        if ((p.PosTower.Fuel.HeIso.PeriodQty > 0) && (p.PosTower.Fuel.HeIso.RunTime < 24))
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Style.BackColor = Color.Red;
                        else if ((p.PosTower.Fuel.N2Iso.PeriodQty > 0) && (p.PosTower.Fuel.N2Iso.RunTime < 24))
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Style.BackColor = Color.Red;
                        else if ((p.PosTower.Fuel.H2Iso.PeriodQty > 0) && (p.PosTower.Fuel.H2Iso.RunTime < 24))
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Style.BackColor = Color.Red;
                        else if ((p.PosTower.Fuel.O2Iso.PeriodQty > 0) && (p.PosTower.Fuel.O2Iso.RunTime < 24))
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Style.BackColor = Color.Red;
                    }
                    else if (p.PosTower.State == "Offline")
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.State].Style.BackColor = Color.Gold;
                    }
                    else
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.State].Style.BackColor = Color.LightCoral;
                        if (p.PosTower.S_RunTime < 4)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.StrontR].Style.BackColor = Color.Red;
                    }

                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.useC].Value = p.UseChart;

                }
            }

            if (Mon_dg_Pos != "")
            {
                // Find index to selected POS
                for (int x = 0; x < dg_MonitoredTowers.Rows.Count; x++)
                {
                    if (Mon_dg_Pos == dg_MonitoredTowers.Rows[x].Cells[(int)MonDG.Name].Value.ToString())
                    {
                        Mon_dg_indx = x;
                        break;
                    }
                }
            }

            if (Mon_dg_indx >= dg_MonitoredTowers.RowCount)
                Mon_dg_indx = dg_MonitoredTowers.RowCount - 1;

            if (Mon_dg_indx >= 0)
            {
                dg_MonitoredTowers.CurrentCell = dg_MonitoredTowers.Rows[Mon_dg_indx].Cells[(int)MonDG.Name];
                Object o = new Object();
                EventArgs ea = new EventArgs();
                dg_MonitoredTowers_SelectionChanged(o, ea);
            }

            if (Config.data.MonSortOrder == SortOrder.Ascending)
                dg_MonitoredTowers.Sort(dg_MonitoredTowers.Columns[Config.data.SortedColumnIndex], System.ComponentModel.ListSortDirection.Ascending);
            else if (Config.data.MonSortOrder == SortOrder.Descending)
                dg_MonitoredTowers.Sort(dg_MonitoredTowers.Columns[Config.data.SortedColumnIndex], System.ComponentModel.ListSortDirection.Descending);

        }

        private void dg_MonitoredTowers_SelectionChanged(object sender, EventArgs e)
        {
            decimal qty;
            string posItm, posName;

            if (dg_MonitoredTowers.CurrentRow.Cells[(int)MonDG.Name].Value != null)
            {
                selName = dg_MonitoredTowers.CurrentRow.Cells[(int)MonDG.Name].Value.ToString();
                posName = selName.Substring(0, selName.IndexOf(" <"));
                selName = posName;
                Mon_dg_Pos = posName;
                Mon_dg_indx = dg_MonitoredTowers.CurrentRow.Index;
               
                foreach (POS pl in POSList.Designs)
                {
                    if (selName == pl.Name)
                    {
                        // OK, this is the PoS that was just selected. Need to work the display.
                        // 1. Display current fuel levels for the POS & Run times for each type of fuel
                        // 2. Calculate and Display Bay usage (percentage on bars)
                        // 3. Display PoS module listing in listbox
                        // 4. Display PoS modules on picture screen section
                        lb_PoSModuleList.Items.Clear();

                        foreach (Module m in pl.Modules)
                        {
                            qty = m.Qty;
                            posItm = m.Name;
                            if (qty > 1)
                                posItm += "[" + qty + "]";
                            posItm += " <" + m.State + ">";
                            lb_PoSModuleList.Items.Add(posItm);
                        }
                        UpdateTowerMonitorDisplay();
                    }
                }
            }
            if (!load)
            {
                Config.data.MonSelIndex = Mon_dg_indx;
                Config.data.SelPos = Mon_dg_Pos;
                Config.SaveConfiguration();
            }
        }

        private void UpdateTowerMonitorDisplay()
        {
            FuelBay nud_fuel;
            decimal bay_p;
            decimal cpu_u, power_u;
            decimal sov_mod, increment;

            if (dg_MonitoredTowers.CurrentRow.Cells[(int)MonDG.Name].Value != null)
            {
                foreach (POS pl in POSList.Designs)
                {
                    if (selName == pl.Name)
                    {
                        nud_fuel = new FuelBay(pl.PosTower.Fuel);
                        nud_fuel.EnrUran.Qty = nud_EnrUran.Value;
                        nud_fuel.Oxygen.Qty = nud_Oxy.Value;
                        nud_fuel.MechPart.Qty = nud_MechPart.Value;
                        nud_fuel.Robotics.Qty = nud_Robotic.Value;
                        nud_fuel.Coolant.Qty = nud_Coolant.Value;
                        nud_fuel.HvyWater.Qty = nud_HvyWtr.Value;
                        nud_fuel.LiqOzone.Qty = nud_LiqOzn.Value;
                        nud_fuel.Charters.Qty = nud_Charter.Value;
                        nud_fuel.HeIso.Qty = nud_Isotope.Value;
                        nud_fuel.H2Iso.Qty = nud_Isotope.Value;
                        nud_fuel.N2Iso.Qty = nud_Isotope.Value;
                        nud_fuel.O2Iso.Qty = nud_Isotope.Value;
                        nud_fuel.Strontium.Qty = nud_Stront.Value;

                        pl.CalculatePOSAdjustRunTime(Config.data.FuelCosts, nud_fuel);

                        sov_mod = pl.GetSovMultiple();

                        power_u = pl.GetModifierTime(pl.PosTower.Power_Used, pl.PosTower.Power);
                        cpu_u = pl.GetModifierTime(pl.PosTower.CPU_Used, pl.PosTower.CPU);

                        // Enr Uranium
                        l_C_EnUr.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.EnrUran.Qty);
                        l_R_EnUr.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.EnrUran.RunTime);
                        increment = Math.Ceiling(pl.PosTower.Fuel.EnrUran.PeriodQty * sov_mod);
                        nud_EnrUran.Increment = Convert.ToDecimal(increment);
                        l_QH_EnUr.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_EnUr.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.EnrUran.RunTime);
                        l_AQ_EnUr.Text = String.Format("{0:#,0.#}", (pl.PosTower.A_Fuel.EnrUran.Qty));
                        if (pl.PosTower.A_Fuel.EnrUran.RunTime < pl.PosTower.Fuel.EnrUran.RunTime)
                            l_AR_EnUr.ForeColor = Color.Red;
                        else
                            l_AR_EnUr.ForeColor = Color.Green;
                        if (nud_EnrUran.Value >= 0)
                            nud_EnrUran.ForeColor = Color.Green;
                        else
                            nud_EnrUran.ForeColor = Color.Red;
                        if (nud_EnrUran.Value < 0)
                            l_AQ_EnUr.ForeColor = Color.Red;
                        else
                            l_AQ_EnUr.ForeColor = Color.Green;

                        // Oxygen
                        l_C_Oxyg.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.Oxygen.Qty);
                        l_R_Oxyg.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.Oxygen.RunTime);
                        increment = Math.Ceiling(pl.PosTower.Fuel.Oxygen.PeriodQty * sov_mod);
                        nud_Oxy.Increment = Convert.ToDecimal(increment);
                        l_QH_Oxyg.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_Oxyg.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.Oxygen.RunTime);
                        l_AQ_Oxyg.Text = String.Format("{0:#,0.#}", (pl.PosTower.A_Fuel.Oxygen.Qty));
                        if (pl.PosTower.A_Fuel.Oxygen.RunTime < pl.PosTower.Fuel.Oxygen.RunTime)
                            l_AR_Oxyg.ForeColor = Color.Red;
                        else
                            l_AR_Oxyg.ForeColor = Color.Green;
                        if (nud_Oxy.Value >= 0)
                            nud_Oxy.ForeColor = Color.Green;
                        else
                            nud_Oxy.ForeColor = Color.Red;
                        if (nud_Oxy.Value < 0)
                            l_AQ_Oxyg.ForeColor = Color.Red;
                        else
                            l_AQ_Oxyg.ForeColor = Color.Green;

                        // Mechanical Parts
                        l_C_McP.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.MechPart.Qty);
                        l_R_McP.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.MechPart.RunTime);
                        increment = Math.Ceiling(pl.PosTower.Fuel.MechPart.PeriodQty * sov_mod);
                        nud_MechPart.Increment = Convert.ToDecimal(increment);
                        l_QH_McP.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_McP.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.MechPart.RunTime);
                        l_AQ_McP.Text = String.Format("{0:#,0.#}", (pl.PosTower.A_Fuel.MechPart.Qty));
                        if (pl.PosTower.A_Fuel.MechPart.RunTime < pl.PosTower.Fuel.MechPart.RunTime)
                            l_AR_McP.ForeColor = Color.Red;
                        else
                            l_AR_McP.ForeColor = Color.Green;
                        if (nud_MechPart.Value >= 0)
                            nud_MechPart.ForeColor = Color.Green;
                        else
                            nud_MechPart.ForeColor = Color.Red;
                        if (nud_MechPart.Value < 0)
                            l_AQ_McP.ForeColor = Color.Red;
                        else
                            l_AQ_McP.ForeColor = Color.Green;

                        // Coolant
                        l_C_Cool.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.Coolant.Qty);
                        l_R_Cool.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.Coolant.RunTime);
                        increment = Math.Ceiling(pl.PosTower.Fuel.Coolant.PeriodQty * sov_mod);
                        nud_Coolant.Increment = Convert.ToDecimal(increment);
                        l_QH_Cool.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_Cool.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.Coolant.RunTime);
                        l_AQ_Cool.Text = String.Format("{0:#,0.#}", (pl.PosTower.A_Fuel.Coolant.Qty));
                        if (pl.PosTower.A_Fuel.Coolant.RunTime < pl.PosTower.Fuel.Coolant.RunTime)
                            l_AR_Cool.ForeColor = Color.Red;
                        else
                            l_AR_Cool.ForeColor = Color.Green;
                        if (nud_Coolant.Value >= 0)
                            nud_Coolant.ForeColor = Color.Green;
                        else
                            nud_Coolant.ForeColor = Color.Red;
                        if (nud_Coolant.Value < 0)
                            l_AQ_Cool.ForeColor = Color.Red;
                        else
                            l_AQ_Cool.ForeColor = Color.Green;

                        // Robotics
                        l_C_Robt.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.Robotics.Qty);
                        l_R_Robt.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.Robotics.RunTime);
                        increment = Math.Ceiling(pl.PosTower.Fuel.Robotics.PeriodQty * sov_mod);
                        nud_Robotic.Increment = Convert.ToDecimal(increment);
                        l_QH_Robt.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_Robt.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.Robotics.RunTime);
                        l_AQ_Robt.Text = String.Format("{0:#,0.#}", (pl.PosTower.A_Fuel.Robotics.Qty));
                        if (pl.PosTower.A_Fuel.Robotics.RunTime < pl.PosTower.Fuel.Robotics.RunTime)
                            l_AR_Robt.ForeColor = Color.Red;
                        else
                            l_AR_Robt.ForeColor = Color.Green;
                        if (nud_Robotic.Value >= 0)
                            nud_Robotic.ForeColor = Color.Green;
                        else
                            nud_Robotic.ForeColor = Color.Red;
                        if (nud_Robotic.Value < 0)
                            l_AQ_Robt.ForeColor = Color.Red;
                        else
                            l_AQ_Robt.ForeColor = Color.Green;

                        // Faction Charters
                        l_C_Chrt.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.Charters.Qty);
                        l_R_Chrt.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.Charters.RunTime);
                        increment = Math.Ceiling(pl.PosTower.Fuel.Charters.PeriodQty * sov_mod);
                        nud_Charter.Increment = Convert.ToDecimal(increment);
                        l_QH_Chrt.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_Chrt.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.Charters.RunTime);
                        l_AQ_Chrt.Text = String.Format("{0:#,0.#}", (pl.PosTower.A_Fuel.Charters.Qty));
                        if (pl.PosTower.A_Fuel.Charters.RunTime < pl.PosTower.Fuel.Charters.RunTime)
                            l_AR_Chrt.ForeColor = Color.Red;
                        else
                            l_AR_Chrt.ForeColor = Color.Green;
                        if (nud_Charter.Value >= 0)
                            nud_Charter.ForeColor = Color.Green;
                        else
                            nud_Charter.ForeColor = Color.Red;
                        if (nud_Charter.Value < 0)
                            l_AQ_Chrt.ForeColor = Color.Red;
                        else
                            l_AQ_Chrt.ForeColor = Color.Green;

                        // Strontium
                        l_C_Strn.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.Strontium.Qty);
                        l_R_Strn.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.Strontium.RunTime);
                        increment = Math.Ceiling(pl.PosTower.Fuel.Strontium.PeriodQty * sov_mod);
                        nud_Stront.Increment = Convert.ToDecimal(increment);
                        l_QH_Strn.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_Strn.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.Strontium.RunTime);
                        l_AQ_Strn.Text = String.Format("{0:#,0.#}", (pl.PosTower.A_Fuel.Strontium.Qty));
                        if (pl.PosTower.A_Fuel.Strontium.RunTime < pl.PosTower.Fuel.Strontium.RunTime)
                            l_AR_Strn.ForeColor = Color.Red;
                        else
                            l_AR_Strn.ForeColor = Color.Green;
                        if (nud_Stront.Value >= 0)
                            nud_Stront.ForeColor = Color.Green;
                        else
                            nud_Stront.ForeColor = Color.Red;
                        if (nud_Stront.Value < 0)
                            l_AQ_Strn.ForeColor = Color.Red;
                        else
                            l_AQ_Strn.ForeColor = Color.Green;

                        // Heavy Water
                        l_C_HvyW.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.HvyWater.Qty);
                        l_R_HvyW.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.HvyWater.RunTime);
                        increment = Math.Ceiling(pl.PosTower.Fuel.HvyWater.PeriodQty * sov_mod);
                        nud_HvyWtr.Increment = Convert.ToDecimal(Math.Ceiling(increment * cpu_u));
                        l_QH_HvyW.Text = String.Format("{0:#,0.#}", Math.Ceiling(increment * cpu_u));
                        l_AR_HvyW.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.HvyWater.RunTime);
                        l_AQ_HvyW.Text = String.Format("{0:#,0.#}", (pl.PosTower.A_Fuel.HvyWater.Qty));
                        if (pl.PosTower.A_Fuel.HvyWater.RunTime < pl.PosTower.Fuel.HvyWater.RunTime)
                            l_AR_HvyW.ForeColor = Color.Red;
                        else
                            l_AR_HvyW.ForeColor = Color.Green;
                        if (nud_HvyWtr.Value >= 0)
                            nud_HvyWtr.ForeColor = Color.Green;
                        else
                            nud_HvyWtr.ForeColor = Color.Red;
                        if (nud_HvyWtr.Value < 0)
                            l_AQ_HvyW.ForeColor = Color.Red;
                        else
                            l_AQ_HvyW.ForeColor = Color.Green;

                        // Liquid Ozone
                        l_C_LiqO.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.LiqOzone.Qty);
                        l_R_LiqO.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.LiqOzone.RunTime);
                        increment = Math.Ceiling(pl.PosTower.Fuel.LiqOzone.PeriodQty * sov_mod);
                        nud_LiqOzn.Increment = Convert.ToDecimal(Math.Ceiling(increment * power_u));
                        l_QH_LiqO.Text = String.Format("{0:#,0.#}", Math.Ceiling(increment * power_u));
                        l_AR_LiqO.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.LiqOzone.RunTime);
                        l_AQ_LiqO.Text = String.Format("{0:#,0.#}", (pl.PosTower.A_Fuel.LiqOzone.Qty));
                        if (pl.PosTower.A_Fuel.LiqOzone.RunTime < pl.PosTower.Fuel.LiqOzone.RunTime)
                            l_AR_LiqO.ForeColor = Color.Red;
                        else
                            l_AR_LiqO.ForeColor = Color.Green;
                        if (nud_LiqOzn.Value >= 0)
                            nud_LiqOzn.ForeColor = Color.Green;
                        else
                            nud_LiqOzn.ForeColor = Color.Red;
                        if (nud_LiqOzn.Value < 0)
                            l_AQ_LiqO.ForeColor = Color.Red;
                        else
                            l_AQ_LiqO.ForeColor = Color.Green;

                        // Isotopes
                        if (pl.PosTower.Fuel.N2Iso.PeriodQty > 0)
                        {
                            l_M_IsoType.Text = "N2";
                            l_C_Iso.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.N2Iso.Qty);
                            l_R_Iso.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.N2Iso.RunTime);
                            increment = Math.Ceiling(pl.PosTower.Fuel.N2Iso.PeriodQty * sov_mod);
                            nud_Isotope.Increment = Convert.ToDecimal(increment);
                            l_QH_Iso.Text = String.Format("{0:#,0.#}", increment);
                            l_AR_Iso.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.N2Iso.RunTime);
                            l_AQ_Iso.Text = String.Format("{0:#,0.#}", (pl.PosTower.A_Fuel.N2Iso.Qty));
                            if (pl.PosTower.A_Fuel.N2Iso.RunTime < pl.PosTower.Fuel.N2Iso.RunTime)
                                l_AR_Iso.ForeColor = Color.Red;
                            else
                                l_AR_Iso.ForeColor = Color.Green;
                        }
                        else if (pl.PosTower.Fuel.H2Iso.PeriodQty > 0)
                        {
                            l_M_IsoType.Text = "H2";
                            l_C_Iso.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.H2Iso.Qty);
                            l_R_Iso.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.H2Iso.RunTime);
                            increment = Math.Ceiling(pl.PosTower.Fuel.H2Iso.PeriodQty * sov_mod);
                            nud_Isotope.Increment = Convert.ToDecimal(increment);
                            l_QH_Iso.Text = String.Format("{0:#,0.#}", increment);
                            l_AR_Iso.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.H2Iso.RunTime);
                            l_AQ_Iso.Text = String.Format("{0:#,0.#}", (pl.PosTower.A_Fuel.H2Iso.Qty));
                            if (pl.PosTower.A_Fuel.H2Iso.RunTime < pl.PosTower.Fuel.H2Iso.RunTime)
                                l_AR_Iso.ForeColor = Color.Red;
                            else
                                l_AR_Iso.ForeColor = Color.Green;
                        }
                        else if (pl.PosTower.Fuel.O2Iso.PeriodQty > 0)
                        {
                            l_M_IsoType.Text = "O2";
                            l_C_Iso.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.O2Iso.Qty);
                            l_R_Iso.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.O2Iso.RunTime);
                            increment = Math.Ceiling(pl.PosTower.Fuel.O2Iso.PeriodQty * sov_mod);
                            nud_Isotope.Increment = Convert.ToDecimal(increment);
                            l_QH_Iso.Text = String.Format("{0:#,0.#}", increment);
                            l_AR_Iso.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.O2Iso.RunTime);
                            l_AQ_Iso.Text = String.Format("{0:#,0.#}", (pl.PosTower.A_Fuel.O2Iso.Qty));
                            if (pl.PosTower.A_Fuel.O2Iso.RunTime < pl.PosTower.Fuel.O2Iso.RunTime)
                                l_AR_Iso.ForeColor = Color.Red;
                            else
                                l_AR_Iso.ForeColor = Color.Green;
                        }
                        else if (pl.PosTower.Fuel.HeIso.PeriodQty > 0)
                        {
                            l_M_IsoType.Text = "He";
                            l_C_Iso.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.HeIso.Qty);
                            l_R_Iso.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.HeIso.RunTime);
                            increment = Math.Ceiling(pl.PosTower.Fuel.HeIso.PeriodQty * sov_mod);
                            nud_Isotope.Increment = Convert.ToDecimal(increment);
                            l_QH_Iso.Text = String.Format("{0:#,0.#}", increment);
                            l_AR_Iso.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.HeIso.RunTime);
                            l_AQ_Iso.Text = String.Format("{0:#,0.#}", (pl.PosTower.A_Fuel.HeIso.Qty));
                            if (pl.PosTower.A_Fuel.HeIso.RunTime < pl.PosTower.Fuel.HeIso.RunTime)
                                l_AR_Iso.ForeColor = Color.Red;
                            else
                                l_AR_Iso.ForeColor = Color.Green;
                        }
                        else
                        {
                            l_M_IsoType.Text = "";
                            l_C_Iso.Text = "0";
                            l_R_Iso.Text = ConvertHoursToTextDisplay(0);
                            l_QH_Iso.Text = "0";
                            l_AR_Iso.Text = ConvertHoursToTextDisplay(0);
                            l_AQ_Iso.Text = "0";
                            l_AR_Iso.ForeColor = Color.Green;
                            nud_Isotope.Increment = 0;
                        }

                        if (nud_Isotope.Value >= 0)
                            nud_Isotope.ForeColor = Color.Green;
                        else
                            nud_Isotope.ForeColor = Color.Red;
                        if (nud_Isotope.Value < 0)
                            l_AQ_Iso.ForeColor = Color.Red;
                        else
                            l_AQ_Iso.ForeColor = Color.Green;

                        bay_p = ComputeBayPercentage(pl.PosTower.A_Fuel.FuelUsed, pl.PosTower.A_Fuel.FuelCap);
                        pb_FuelBayFill.Value = Convert.ToInt32(bay_p);

                        bay_p = ComputeBayPercentage(pl.PosTower.A_Fuel.StrontUsed, pl.PosTower.A_Fuel.StrontCap);
                        pb_StrontBayFill.Value = Convert.ToInt32(bay_p);

                        break;
                    }
                }
            }
        }

        private void UdateMonitorInformation(object sender, EventArgs e)
        {
            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
        }

        private void UpdateMonitoredTowerState(string state)
        {
            update = true;

            if (dg_MonitoredTowers.CurrentRow.Cells[(int)MonDG.Name].Value != null)
            {
                foreach (POS pl in POSList.Designs)
                {
                    if (selName == pl.Name)
                    {
                        pl.PosTower.State = state;

                        if (state == "Reinforced")
                            pl.Stront_TS = DateTime.Now;
                        else
                            pl.Fuel_TS = DateTime.Now;
                    }
                }
            }

            update = false;
        }

        private void dg_MonitoredTowers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 18)
            {
                foreach (POS pl in POSList.Designs)
                {
                    if (selName == pl.Name)
                    {
                        if (!(bool)dg_MonitoredTowers.Rows[e.RowIndex].Cells[e.ColumnIndex].Value)
                        {
                            dg_MonitoredTowers.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                            pl.UseChart = true;
                        }
                        else
                        {
                            dg_MonitoredTowers.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                            pl.UseChart = false;
                        }
                        break;
                    }
                }
                POSList.SaveDesignListing();
            }
        }


#endregion


#region Button and Menu Controls

        private void cms_PosItem_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip Mnu = (ContextMenuStrip)sender;
            api = (PoS_Item)Mnu.SourceControl;
            ToolStripMenuItem tsmi;

            tsmi = (ToolStripMenuItem)cms_PosItem.Items[4];
            tsmi.DropDownItems.Clear();
            tsmi.Available = true;

            if (api.TypeKey != EveHQ.PosManager.PoS_Item.TypeKeyEnum.Tower)
            {
                foreach (Module m in Design.Modules)
                {
                    if ((m.typeID == api.typeID) && (api.contName == m.Location))
                    {
                        // found the module, populate the charge list
                        if (m.ChargeList == null)
                        {
                            tsmi.Available = false;
                            break;
                        }

                        foreach (string s in m.ChargeList)
                        {
                            tsmi.DropDownItems.Add(s);
                        }
                    }
                }
            }
        }

        private void tsm_SetModuleCharge_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (api.TypeKey != EveHQ.PosManager.PoS_Item.TypeKeyEnum.Tower)
            {
                foreach (Module m in Design.Modules)
                {
                    if ((m.typeID == api.typeID) && (api.contName == m.Location))
                    {
                        m.Charge = e.ClickedItem.Text.ToString();
                        break;
                    }
                }
                CalculatePOSData();
            }
        }

        private void b_SavePoS_Click(object sender, EventArgs e)
        {
            POS pli;

            if ((cb_PoSName.Text == "") && (NewName == ""))
            {
                // No name - request one from user
                POS_Name NewPos = new POS_Name();
                NewPos.myData = this;
                NewPos.NewPOS = true;
                NewPos.ShowDialog();
            }

            if ((cb_PoSName.Text == "") && (NewName == ""))
                return;

            if (cb_PoSName.Text == "")
            {
                POS_Name NewPos = new POS_Name();
                NewPos.myData = this;
                NewPos.NewPOS = true;
                NewPos.ShowDialog();

                CurrentName = NewName;
                NewName = "";

                if (CurrentName != "")
                    Design.Name = CurrentName;
                else
                {
                    // Still no name for the PoS, so just exit
                    return;
                }
            }

            foreach (POS pl in POSList.Designs)
            {
                if (pl.Name == cb_PoSName.Text)
                {
                    // Existing PoS being edited, need to overwrite it or something
                    POSList.Designs.Remove(pl);
                    break;
                }
            }

            pli = new POS(Design);
            pli.Name = CurrentName;

            POSList.Designs.Add(pli);

            cb_PoSName.Items.Clear();
            foreach (POS pl in POSList.Designs)
            {
                cb_PoSName.Items.Add(pl.Name);
            }

            POSList.SaveDesignListing();
            PosChanged = false;
            BuildPOSListForMonitoring();
            PopulateMonitoredPoSDisplay();
            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            cb_PoSName.Text = pli.Name;
        }

        private void tsm_Online_Click(object sender, EventArgs e)
        {
            if (api.typeID == 0)
                return;

            api.onOff = Color.Green;
            api.UpdateItemState();

            if (api.TypeKey == EveHQ.PosManager.PoS_Item.TypeKeyEnum.Tower)
            {
                Design.PosTower.State = "Online";
            }
            else
            {
                foreach (Module m in Design.Modules)
                {
                    if ((m.typeID == api.typeID) && (api.contName == m.Location))
                    {
                        m.State = "Online";
                    }
                }
            }
            if (!load)
                PosChanged = true;
            CalculatePOSData();
        }

        private void tsm_Offline_Click(object sender, EventArgs e)
        {
            if (api.typeID == 0)
                return;

            api.onOff = Color.Red;
            api.UpdateItemState();

            if (api.TypeKey == EveHQ.PosManager.PoS_Item.TypeKeyEnum.Tower)
            {
                Design.PosTower.State = "Offline";
            }
            else
            {
                foreach (Module m in Design.Modules)
                {
                    if ((m.typeID == api.typeID) && (api.contName == m.Location))
                    {
                        m.State = "Offline";
                    }
                }
            }
            if (!load)
                PosChanged = true;
            CalculatePOSData();
        }

        private void tsm_Remove_Click(object sender, EventArgs e)
        {
            int ind = 0;

            if (api.typeID == 0)
                return;

            if (api.TypeKey == EveHQ.PosManager.PoS_Item.TypeKeyEnum.Tower)
            {
                Design.RemoveTowerFromPOS();
            }
            else
            {
                foreach (Module m in Design.Modules)
                {
                    if ((m.typeID == api.typeID) && (api.contName == m.Location))
                    {
                        Design.RemoveModuleFromPOS(ind);
                        break;
                    }
                    ind++;
                }
            }

            api.RemoveItemFromControl();
            if (!load)
                PosChanged = true;
            CalculatePOSData();
        }

        private void b_NewPos_Click(object sender, EventArgs e)
        {
            if ((PosChanged) && (Design.Name != ""))
            {
                // Query user to save any changes
                DialogResult dr = MessageBox.Show("The currently Selected POS has Changed. Do you want to Save it?", "Save POS", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    foreach (POS pl in POSList.Designs)
                    {
                        if (pl.Name == Design.Name)
                        {
                            // Existing PoS has been edited, need to overwrite it or something
                            POSList.Designs.Remove(pl);
                            break;
                        }
                    }

                    POSList.AddDesignToList(new POS(Design));
                }
            }

            POS_Name NewPos = new POS_Name();
            NewPos.myData = this;
            NewPos.NewPOS = true;
            NewPos.ShowDialog();

            // The user did not enter a new name, just get out
            if (NewName == "")
                return;

            CurrentName = NewName;
            NewName = "";
            this.Focus();
            POSList.AddDesignToList(new POS(CurrentName));
            POSList.SaveDesignListing();
            BuildPOSListForMonitoring();
            cb_PoSName.Items.Add(CurrentName);
            PosChanged = false;
            cb_PoSName.SelectedItem = CurrentName;
            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();

            cb_System.Text = "Select POS System";
            cb_SovLevel.Text = "Select Sov Level";
            cb_CorpName.Text = "Select Corp Name";
        }

        private void b_RenamePos_Click(object sender, EventArgs e)
        {
            POS_Name RenPos = new POS_Name();
            RenPos.myData = this;
            RenPos.NewPOS = false;
            RenPos.ShowDialog();

            if (CurrentName.Equals(NewName))
                return;

            foreach (POS pl in POSList.Designs)
            {
                if (pl.Name == CurrentName)
                {
                    // Existing PoS has been edited, need to overwrite it or something
                    POSList.Designs.Remove(pl);
                    break;
                }
            }

            Design.Name = NewName;
            CurrentName = NewName;
            NewName = "";
            this.Focus();
            POSList.AddDesignToList(Design);
            POSList.SaveDesignListing();
            BuildPOSListForMonitoring();
            cb_PoSName.Items.Clear();
            foreach (POS pl in POSList.Designs)
            {
                cb_PoSName.Items.Add(pl.Name);
            }
            cb_PoSName.SelectedItem = CurrentName;
            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
            PosChanged = false;
        }

        private void b_CopyPOS_Click(object sender, EventArgs e)
        {
            POS_Name CpyPos = new POS_Name();
            CpyPos.myData = this;
            CpyPos.NewPOS = false;
            CpyPos.CopyPOS = true;
            CpyPos.ShowDialog();

            if (CurrentName.Equals(NewName))
                return;
            if (NewName.Length <= 0)
                return;

            Design.Name = NewName;
            CurrentName = NewName;
            Design.itemID = 0;
            Design.locID = 0;

            NewName = "";
            this.Focus();
            POSList.AddDesignToList(Design);
            POSList.SaveDesignListing();
            BuildPOSListForMonitoring();
            cb_PoSName.Items.Clear();
            foreach (POS pl in POSList.Designs)
            {
                cb_PoSName.Items.Add(pl.Name);
            }
            cb_PoSName.SelectedItem = CurrentName;
            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
            PosChanged = false;
        }

        private void b_ClearPoS_Click(object sender, EventArgs e)
        {
            // Needs Pop-Up confirmation
            DialogResult dr = MessageBox.Show("Are you sure that you want to Clear  the POS Modules from [" + CurrentName + "] ?", "Clear POS", MessageBoxButtons.YesNo);

            if (dr == DialogResult.No)
                return;

            // Clear Current PoS Display
            foreach (Control c in p_Tower.Controls)
            {
                if (c.GetType().Name == "PoS_Item")
                {
                    PoS_Item pi = (PoS_Item)c;
                    pi.RemoveItemFromControl();
                }
            }

            Design.ClearAllPOSData();

            b_SavePoS_Click(sender, e);

            // Needs to clear all Data (fuel, mod list, etc)
            ClearPosData();
            ClearFuelData();
            ClearModListing();
        }

        private void b_DelPoS_Click(object sender, EventArgs e)
        {
            // Needs Pop-Up confirmation
            DialogResult dr = MessageBox.Show("Are you sure that you want to Delete the POS [" + CurrentName + "] ?", "Delete POS", MessageBoxButtons.YesNo);

            if (dr == DialogResult.No)
                return;

            foreach (POS pl in POSList.Designs)
            {
                if (pl.Name == cb_PoSName.Text)
                {
                    // Existing PoS being edited, need to overwrite it or something
                    POSList.Designs.Remove(pl);
                    break;
                }
            }

            // 2. Clear Current PoS Display
            foreach (Control c in p_Tower.Controls)
            {
                if (c.GetType().Name == "PoS_Item")
                {
                    PoS_Item pi = (PoS_Item)c;
                    pi.RemoveItemFromControl();
                }
            }

            cb_PoSName.Items.Remove(cb_PoSName.Text);
            Design.ClearAllPOSData();

            cb_PoSName.Items.Clear();
            foreach (POS p in POSList.Designs)
            {
                cb_PoSName.Items.Add(p.Name);
            }
            if (cb_PoSName.Items.Count > 0)
                cb_PoSName.SelectedIndex = 0;

            CalculatePOSData();

            POSList.SaveDesignListing();

            // Needs to clear all Data (fuel, mod list, etc)
            ClearPosData();
            ClearFuelData();
            ClearModListing();
            PosChanged = false;
            BuildPOSListForMonitoring();
            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
        }

        private void b_ModifyList_Click(object sender, EventArgs e)
        {
            MonitorListSelect mlist_sel = new MonitorListSelect();
            mlist_sel.myData = this;
            mlist_sel.ShowDialog();

            POSList.SaveDesignListing();
            BuildPOSListForMonitoring();
            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
        }

        private void b_FuelUpdate_Click(object sender, EventArgs e)
        {
            update = true;

            if (dg_MonitoredTowers.CurrentRow.Cells[(int)MonDG.Name].Value != null)
            {
                foreach (POS pl in POSList.Designs)
                {
                    if (selName == pl.Name)
                    {
                        // Enr Uranium
                        pl.PosTower.Fuel.EnrUran.Qty = pl.PosTower.A_Fuel.EnrUran.Qty;
                        nud_EnrUran.Value = 0;

                        // Oxygen
                        pl.PosTower.Fuel.Oxygen.Qty = pl.PosTower.A_Fuel.Oxygen.Qty;
                        nud_Oxy.Value = 0;

                        // Mechanical Parts
                        pl.PosTower.Fuel.MechPart.Qty = pl.PosTower.A_Fuel.MechPart.Qty;
                        nud_MechPart.Value = 0;

                        // Coolant
                        pl.PosTower.Fuel.Coolant.Qty = pl.PosTower.A_Fuel.Coolant.Qty;
                        nud_Coolant.Value = 0;

                        // Robotics
                        pl.PosTower.Fuel.Robotics.Qty = pl.PosTower.A_Fuel.Robotics.Qty;
                        nud_Robotic.Value = 0;

                        // Faction Charters
                        pl.PosTower.Fuel.Charters.Qty = pl.PosTower.A_Fuel.Charters.Qty;
                        nud_Charter.Value = 0;

                        // Strontium
                        pl.PosTower.Fuel.Strontium.Qty = pl.PosTower.A_Fuel.Strontium.Qty;
                        nud_Stront.Value = 0;

                        // Heavy Water
                        pl.PosTower.Fuel.HvyWater.Qty = pl.PosTower.A_Fuel.HvyWater.Qty;
                        nud_HvyWtr.Value = 0;

                        // Liquid Ozone
                        pl.PosTower.Fuel.LiqOzone.Qty = pl.PosTower.A_Fuel.LiqOzone.Qty;
                        nud_LiqOzn.Value = 0;

                        // Isotope
                        if (pl.PosTower.Fuel.HeIso.PeriodQty > 0)
                            pl.PosTower.Fuel.HeIso.Qty = pl.PosTower.A_Fuel.HeIso.Qty;
                        if (pl.PosTower.Fuel.H2Iso.PeriodQty > 0)
                            pl.PosTower.Fuel.H2Iso.Qty = pl.PosTower.A_Fuel.H2Iso.Qty;
                        if (pl.PosTower.Fuel.N2Iso.PeriodQty > 0)
                            pl.PosTower.Fuel.N2Iso.Qty = pl.PosTower.A_Fuel.N2Iso.Qty;
                        if (pl.PosTower.Fuel.O2Iso.PeriodQty > 0)
                            pl.PosTower.Fuel.O2Iso.Qty = pl.PosTower.A_Fuel.O2Iso.Qty;
                        
                        nud_Isotope.Value = 0;
                    }
                }
            }

            update = false;

            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            POSList.LoadDesignListing();
            BuildPOSListForMonitoring();
            UpdateTowerMonitorDisplay();
            PopulateMonitoredPoSDisplay();
        }

        private void b_EnterReinf_Click(object sender, EventArgs e)
        {
            if (dg_MonitoredTowers.RowCount <= 0)
                return;

            UpdateMonitoredTowerState("Reinforced");
            BuildPOSListForMonitoring();
            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
        }

        private void b_offline_Click(object sender, EventArgs e)
        {
            if (dg_MonitoredTowers.RowCount <= 0)
                return;

            UpdateMonitoredTowerState("Offline");
            BuildPOSListForMonitoring();
            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
        }

        private void b_online_Click(object sender, EventArgs e)
        {
            if (dg_MonitoredTowers.RowCount <= 0)
                return;

            UpdateMonitoredTowerState("Online");
            BuildPOSListForMonitoring();
            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
        }


#endregion


#region POS Maintenance

        private void b_CopySelected_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(SelPosFillText);
        }

        private void b_CopyAllPos_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(AllPosFillText);
        }

        private void ApplyFillData(object sender, EventArgs e)
        {
            Config.data.maintTP = tscb_TimePeriod.SelectedIndex;
            Config.data.maintPV = nud_PeriodValue.Value;
            Config.SaveConfiguration();

            PopulateTowerFillDG();
            UpdateSelectedTowerList();
        }

        private void UpdateSelectedTowerList()
        {
            if (dg_TowerFuelList.Rows.Count > Fil_dg_indx)
            {
                dg_TowerFuelList.CurrentCell = dg_TowerFuelList.Rows[Fil_dg_indx].Cells[(int)fillDG.Name];
                Object o = new Object();
                EventArgs ea = new EventArgs();
                dg_TowerFuelList_SelectionChanged(o, ea);
            }

        }

        private void dg_TowerFuelList_SelectionChanged(object sender, EventArgs e)
        {
            string maintName, paddStr, padCnt, padVol, padIsk;
            int dgi;
            decimal totVol, totCost;
            string[,] fVal;
            FuelBay sfb;

            if (dg_TowerFuelList.CurrentRow.Cells[(int)fillDG.Name].Value != null)
            {
                SelPosFillText = "";
                Fil_dg_indx = dg_TowerFuelList.CurrentRow.Index;
                maintName = dg_TowerFuelList.CurrentRow.Cells[(int)fillDG.Name].Value.ToString();
                SelPosFillText = maintName + " < " + dg_TowerFuelList.CurrentRow.Cells[(int)fillDG.Loc].Value.ToString() + " >\n";
                foreach (POS pl in POSList.Designs)
                {
                    if (maintName == pl.Name)
                    {
                        sfb = new FuelBay(pl.PosTower.T_Fuel);
                        sfb.SetCurrentFuelVolumes();
                        sfb.SetCurrentFuelCosts(Config.data.FuelCosts);
                        fVal = sfb.GetFuelBayTotals();

                        dg_SelectedFuel.Rows.Clear();
                        totCost = 0;
                        totVol = 0;

                        for (int x = 0; x < 13; x++)
                        {
                            if ((!Config.data.maintChart) && (x == 11))
                                continue;
                            if ((!Config.data.maintStront) && (x == 12))
                                continue;

                            paddStr = fVal[x, 0].PadRight(20, ' ') + "|";
                            padCnt = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1])).PadLeft(11, ' ') + "|";
                            padVol = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])).PadLeft(13, ' ') + "m3|";
                            padIsk = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 3])).PadLeft(17, ' ') + "isk|\n";
                            SelPosFillText += paddStr + padCnt + padVol + padIsk;
                            dgi = dg_SelectedFuel.Rows.Add();
                            dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.type].Value = fVal[x, 0];
                            dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.amount].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1]));
                            totVol += Convert.ToDecimal(fVal[x, 2]);
                            dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.vol].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])) + " m3";
                            totCost += Convert.ToDecimal(fVal[x, 3]);
                            dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.cost].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 3])) + " isk";
                        }
                        dgi = dg_SelectedFuel.Rows.Add();
                        dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.type].Value = "Total Volume & Cost";
                        dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.vol].Value = String.Format("{0:#,0.#}", totVol) + " m3";
                        dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.cost].Value = String.Format("{0:#,0.#}", totCost) + " isk";
                        paddStr = ("Total Volume & Cost").PadRight(20, ' ') + "|";
                        padVol = String.Format("{0:#,0.#}", totVol).PadLeft(25, ' ') + "m3|";
                        padIsk = String.Format("{0:#,0.#}", totCost).PadLeft(17, ' ') + "isk|";
                        SelPosFillText += paddStr + padVol + padIsk;
                        break;
                    }
                }
            }
        }

        private void PopulateTotalsDG()
        {
            int dgi;
            decimal totVol, totCost;
            string[,] fVal;
            string paddStr, padCnt, padVol, padIsk;

            if (tt == null)
                return;

            tt.SetCurrentFuelVolumes();
            tt.SetCurrentFuelCosts(Config.data.FuelCosts);
            dg_TotalFuel.Rows.Clear();
            fVal = tt.GetFuelBayTotals();

            totVol = 0;
            totCost = 0;
            AllPosFillText = "Total Fill Values for < All POS's >\n";
            for (int x = 0; x < 13; x++)
            {
                if ((!Config.data.maintChart) && (x == 11))
                    continue;
                if ((!Config.data.maintStront) && (x == 12))
                    continue;

                paddStr = fVal[x, 0].PadRight(20, ' ') + "|";
                padCnt = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1])).PadLeft(11, ' ') + "|";
                padVol = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])).PadLeft(13, ' ') + "m3|";
                padIsk = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 3])).PadLeft(17, ' ') + "isk|\n";
                AllPosFillText += paddStr + padCnt + padVol + padIsk;
                dgi = dg_TotalFuel.Rows.Add();
                dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.type].Value = fVal[x, 0];
                dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.amount].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1]));
                totVol += Convert.ToDecimal(fVal[x, 2]);
                dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.vol].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])) + " m3";
                totCost += Convert.ToDecimal(fVal[x, 3]);
                dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.cost].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 3])) + " isk";
            }
            dgi = dg_TotalFuel.Rows.Add();
            dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.type].Value = "Total Volume & Cost";
            dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.vol].Value = String.Format("{0:#,0.#}", totVol) + " m3";
            dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.cost].Value = String.Format("{0:#,0.#}", totCost) + " isk";
            paddStr = ("Total Volume & Cost").PadRight(20, ' ') + "|";
            padVol = String.Format("{0:#,0.#}", totVol).PadLeft(25, ' ') + "m3|";
            padIsk = String.Format("{0:#,0.#}", totCost).PadLeft(17, ' ') + "isk|";
            AllPosFillText += paddStr + padVol + padIsk;
        }

        private void PopulateTowerFillDG()
        {
            int dgi;

            tt = null;
            dg_TowerFuelList.Rows.Clear();
            update = true;
            cb_FactChartTotal.Checked = Config.data.maintChart;
            cb_UseStrontTotals.Checked = Config.data.maintStront;
            update = false;

            foreach (POS p in POSList.Designs)
            {
                if (tt == null)
                    tt = new FuelBay(p.PosTower.Fuel);

                p.ComputePosFuelUsageForFillTracking((int)Config.data.maintTP, Config.data.maintPV, Config.data.FuelCosts);

                dgi = dg_TowerFuelList.Rows.Add();

                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Name].Value = p.Name;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Loc].Value = p.System;

                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.EnrUr].Value = String.Format("{0:#,0.#}",p.PosTower.T_Fuel.EnrUran.Qty);
                tt.EnrUran.Qty += p.PosTower.T_Fuel.EnrUran.Qty;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Oxy].Value = String.Format("{0:#,0.#}",p.PosTower.T_Fuel.Oxygen.Qty);
                tt.Oxygen.Qty += p.PosTower.T_Fuel.Oxygen.Qty;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.McP].Value = String.Format("{0:#,0.#}",p.PosTower.T_Fuel.MechPart.Qty);
                tt.MechPart.Qty += p.PosTower.T_Fuel.MechPart.Qty;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Cool].Value = String.Format("{0:#,0.#}",p.PosTower.T_Fuel.Coolant.Qty);
                tt.Coolant.Qty += p.PosTower.T_Fuel.Coolant.Qty;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Rbt].Value = String.Format("{0:#,0.#}",p.PosTower.T_Fuel.Robotics.Qty);
                tt.Robotics.Qty += p.PosTower.T_Fuel.Robotics.Qty;

                if (p.PosTower.T_Fuel.N2Iso.Name != "")
                {
                    dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Iso].Value = String.Format("{0:#,0.#}",p.PosTower.T_Fuel.N2Iso.Qty) + " N2";
                    tt.N2Iso.Qty += p.PosTower.T_Fuel.N2Iso.Qty;
                }
                else if (p.PosTower.T_Fuel.H2Iso.Name != "")
                {
                    dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Iso].Value = String.Format("{0:#,0.#}",p.PosTower.T_Fuel.H2Iso.Qty) + " H2";
                    tt.H2Iso.Qty += p.PosTower.T_Fuel.H2Iso.Qty;
                }
                else if (p.PosTower.T_Fuel.O2Iso.Name != "")
                {
                    dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Iso].Value = String.Format("{0:#,0.#}",p.PosTower.T_Fuel.O2Iso.Qty) + " O2";
                    tt.O2Iso.Qty += p.PosTower.T_Fuel.O2Iso.Qty;
                }
                else if (p.PosTower.T_Fuel.HeIso.Name != "")
                {
                    dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Iso].Value = String.Format("{0:#,0.#}",p.PosTower.T_Fuel.HeIso.Qty) + " He";
                    tt.HeIso.Qty += p.PosTower.T_Fuel.HeIso.Qty;
                }
                else
                {
                    dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Iso].Value = "No F'in Clue";
                }
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.HvW].Value = String.Format("{0:#,0.#}",p.PosTower.T_Fuel.HvyWater.Qty);
                tt.HvyWater.Qty += p.PosTower.T_Fuel.HvyWater.Qty;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.LqO].Value = String.Format("{0:#,0.#}",p.PosTower.T_Fuel.LiqOzone.Qty);
                tt.LiqOzone.Qty += p.PosTower.T_Fuel.LiqOzone.Qty;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Cht].Value = String.Format("{0:#,0.#}",p.PosTower.T_Fuel.Charters.Qty);
                tt.Charters.Qty += p.PosTower.T_Fuel.Charters.Qty;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Strt].Value = String.Format("{0:#,0.#}",p.PosTower.T_Fuel.Strontium.Qty);
                tt.Strontium.Qty += p.PosTower.T_Fuel.Strontium.Qty;

                if (p.PosTower.Fuel.H2Iso.Cost > tt.H2Iso.Cost)
                    tt.H2Iso.Cost = p.PosTower.Fuel.H2Iso.Cost;
                if (p.PosTower.Fuel.HeIso.Cost > tt.HeIso.Cost)
                    tt.HeIso.Cost = p.PosTower.Fuel.HeIso.Cost;
                if (p.PosTower.Fuel.N2Iso.Cost > tt.N2Iso.Cost)
                    tt.N2Iso.Cost = p.PosTower.Fuel.N2Iso.Cost;
                if (p.PosTower.Fuel.O2Iso.Cost > tt.O2Iso.Cost)
                    tt.O2Iso.Cost = p.PosTower.Fuel.O2Iso.Cost;

                if (p.PosTower.Fuel.H2Iso.BaseVol > tt.H2Iso.BaseVol)
                    tt.H2Iso.BaseVol = p.PosTower.Fuel.H2Iso.BaseVol;
                if (p.PosTower.Fuel.HeIso.BaseVol > tt.HeIso.BaseVol)
                    tt.HeIso.BaseVol = p.PosTower.Fuel.HeIso.BaseVol;
                if (p.PosTower.Fuel.N2Iso.BaseVol > tt.N2Iso.BaseVol)
                    tt.N2Iso.BaseVol = p.PosTower.Fuel.N2Iso.BaseVol;
                if (p.PosTower.Fuel.O2Iso.BaseVol > tt.O2Iso.BaseVol)
                    tt.O2Iso.BaseVol = p.PosTower.Fuel.O2Iso.BaseVol;

                if (p.PosTower.Fuel.H2Iso.QtyVol > tt.H2Iso.QtyVol)
                    tt.H2Iso.QtyVol = p.PosTower.Fuel.H2Iso.QtyVol;
                if (p.PosTower.Fuel.HeIso.QtyVol > tt.HeIso.QtyVol)
                    tt.HeIso.QtyVol = p.PosTower.Fuel.HeIso.QtyVol;
                if (p.PosTower.Fuel.N2Iso.QtyVol > tt.N2Iso.QtyVol)
                    tt.N2Iso.QtyVol = p.PosTower.Fuel.N2Iso.QtyVol;
                if (p.PosTower.Fuel.O2Iso.QtyVol > tt.O2Iso.QtyVol)
                    tt.O2Iso.QtyVol = p.PosTower.Fuel.O2Iso.QtyVol;
            }
            PopulateTotalsDG();
        }

        private void cb_FactChartTotal_CheckedChanged(object sender, EventArgs e)
        {
            if (update)
                return;
            Config.data.maintChart = cb_FactChartTotal.Checked;
            Config.SaveConfiguration();
            PopulateTotalsDG();
            UpdateSelectedTowerList();
        }

        private void cb_UseStrontTotals_CheckedChanged(object sender, EventArgs e)
        {
            if (update)
                return;
            Config.data.maintStront = cb_UseStrontTotals.Checked;
            Config.SaveConfiguration();
            PopulateTotalsDG();
            UpdateSelectedTowerList();
        }

#endregion


#region Fuel Amount Change Control

        private void cb_Interval_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If loading a POS and updating fields, do not pay attention.
            if (load)
                return;

            Design.PosTower.Design_Interval = cb_Interval.SelectedIndex;
            CalculateAndDisplayDesignFuelData();
        }

        private void nud_StrontInterval_ValueChanged(object sender, EventArgs e)
        {
            // If loading a POS and updating fields, do not pay attention.
            if (load)
                return;

            Design.PosTower.Design_Stront_Qty = nud_StrontInterval.Value;
            CalculateAndDisplayDesignFuelData();
        }

        private void MonitoredFuelChange(object sender, EventArgs e)
        {
            if (!update)
                UpdateTowerMonitorDisplay();
        }

        private void tsb_DisplayFuelFill_Click(object sender, EventArgs e)
        {
            // This functionality needs to (for each monitored POS):
            // 1. Determine the amount of fuel needed to Fill the fuel bay (from current amounts)
            // 2. Determine the volume and cost for this fuel
            // 3. Display an itemized listing of what is needed to fill each Tower
            //      a. Amount of each fuel type
            //      b. Total cost for the Tower
            //      c. Valume of Cargo needed to be Moved to that Tower

        }


#endregion


#region API Routines

        private void tsb_PopTowerFromAPI_Click(object sender, EventArgs e)
        {
            PopulateTowersFromAPI pti = new PopulateTowersFromAPI();
            pti.myData = this;
            pti.ShowDialog();

            cb_PoSName.Items.Clear();
            foreach (POS pl in POSList.Designs)
            {
                cb_PoSName.Items.Add(pl.Name);
            }

            POSList.SaveDesignListing();
            PosChanged = false;
            BuildPOSListForMonitoring();
            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
        }

        private void b_TowerAPILink_Click(object sender, EventArgs e)
        {
            Tower_API_Linker TowerLink = new Tower_API_Linker();
            TowerLink.myData = this;
            TowerLink.ShowDialog();
            API_D.SaveAPIListing();
            BuildPOSListForMonitoring();
            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
        }

        private void b_UpdateAPIData_Click(object sender, EventArgs e)
        {
            SystemSovList SSL = new SystemSovList();

            DownloadAndUpdateAPI();
            SSL.LoadSovListFromAPI(1);
            SL.LoadSystemListFromDisk();
            AL.LoadAllianceListFromAPI(1);
            API_D.LoadAPIListing(TL);
            BuildPOSListForMonitoring();
            UpdateAllTowerSovLevels();
            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
        }

        private void DownloadAndUpdateAPI()
        {
            apiWorker = new BackgroundWorker();

            this.Cursor = Cursors.WaitCursor;
            apiWorker.WorkerReportsProgress = true;
            apiWorker.DoWork += GetCorpAssets;
            apiWorker.DoWork += GetCorpSheet;
            apiWorker.DoWork += GetAllySovLists;
            apiWorker.RunWorkerCompleted += apiWorker_WorkComplete;

            apiWorker.RunWorkerAsync();
        }

        private void apiWorker_WorkComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void apiWorker_ProgChange(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void GetAllySovLists(object sender, DoWorkEventArgs e)
        {
            XmlDocument apiXML;
            SystemSovList SSL = new SystemSovList();

            AL = new AllianceList();

            apiXML = EveHQ.Core.EveAPI.GetAPIXML((int)EveHQ.Core.EveAPI.APIRequest.AllianceList, 0);
            apiXML = EveHQ.Core.EveAPI.GetAPIXML((int)EveHQ.Core.EveAPI.APIRequest.Sovereignty, 0);
            AL.LoadAllianceListFromAPI(1);
            SSL.LoadSovListFromAPI(1);

            e.Result = 123;
        }

        private void GetCorpAssets(object sender, DoWorkEventArgs e)
        {
            string acctName;
            int sel;
            EveHQ.Core.EveAccount pilotAccount;
            XmlDocument apiXML;
            foreach (EveHQ.Core.Pilot selPilot in EveHQ.Core.HQ.EveHQSettings.Pilots)
            {
                if (selPilot.Active)
                {
                    acctName = selPilot.Account;
                    if (EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(acctName))
                    {
                        pilotAccount = (Core.EveAccount)EveHQ.Core.HQ.EveHQSettings.Accounts[acctName];
                        sel = (int)EveHQ.Core.EveAPI.APIRequest.AssetsCorp;
                        apiXML = EveHQ.Core.EveAPI.GetAPIXML(sel, pilotAccount, selPilot.ID, 0);
                        if (!CheckXML(apiXML))
                        {
                            apiXML = EveHQ.Core.EveAPI.GetAPIXML(sel, pilotAccount, selPilot.ID, 0);
                            // Errors, try one more time. Data will not be used later if it contains errors
                        }
                    }
                }
            }
            e.Result = 123;
        }

        private void GetCorpSheet(object sender, DoWorkEventArgs e)
        {
            string acctName;
            int sel;
            EveHQ.Core.EveAccount pilotAccount;
            XmlDocument apiXML;
            foreach (EveHQ.Core.Pilot selPilot in EveHQ.Core.HQ.EveHQSettings.Pilots)
            {
                if (selPilot.Active)
                {
                    acctName = selPilot.Account;
                    if (EveHQ.Core.HQ.EveHQSettings.Accounts.Contains(acctName))
                    {
                        pilotAccount = (Core.EveAccount)EveHQ.Core.HQ.EveHQSettings.Accounts[acctName];
                        sel = (int)EveHQ.Core.EveAPI.APIRequest.CorpSheet;
                        apiXML = EveHQ.Core.EveAPI.GetAPIXML(sel, pilotAccount, selPilot.ID, 0);
                        if (!CheckXML(apiXML))
                        {
                            apiXML = EveHQ.Core.EveAPI.GetAPIXML(sel, pilotAccount, selPilot.ID, 0);
                            // Errors, try one more time. Data will not be used later if it contains errors
                        }
                    }
                }
            }
            e.Result = 123;
        }

        private bool CheckXML(XmlDocument apiXML)
        {
            if (apiXML != null)
            {
                XmlNodeList errlist = apiXML.SelectNodes("/eveapi/error");
                if (errlist.Count != 0)
                {
                    // Error - Now WTF do I do???
                    return false;
                }
            }
            return true;
        }

#endregion


#region Configuration Stuff

        private void tc_StatFuel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tc_StatFuel.SelectedIndex == (tc_StatFuel.TabPages.Count -1))
            {
                PopulateFuelConfigDisplay();
            }
        }

        private void PopulateFuelConfigDisplay()
        {
            nud_ER_FC.Value = Config.data.FuelCosts.EnrUran.Cost;
            nud_OX_FC.Value = Config.data.FuelCosts.Oxygen.Cost;
            nud_MP_FC.Value = Config.data.FuelCosts.MechPart.Cost;
            nud_CL_FC.Value = Config.data.FuelCosts.Coolant.Cost;
            nud_RB_FC.Value = Config.data.FuelCosts.Robotics.Cost;
            nud_HE_FC.Value = Config.data.FuelCosts.HeIso.Cost;
            nud_N2_FC.Value = Config.data.FuelCosts.N2Iso.Cost;
            nud_H2_FC.Value = Config.data.FuelCosts.H2Iso.Cost;
            nud_O2_FC.Value = Config.data.FuelCosts.O2Iso.Cost;
            nud_HW_FC.Value = Config.data.FuelCosts.HvyWater.Cost;
            nud_LO_FC.Value = Config.data.FuelCosts.LiqOzone.Cost;
            nud_CH_FC.Value = Config.data.FuelCosts.Charters.Cost;
            nud_ST_FC.Value = Config.data.FuelCosts.Strontium.Cost;
        }

        private void b_SaveFuelCosts_Click(object sender, EventArgs e)
        {
            Config.data.FuelCosts.EnrUran.Cost = nud_ER_FC.Value;
            Config.data.FuelCosts.Oxygen.Cost = nud_OX_FC.Value;
            Config.data.FuelCosts.MechPart.Cost = nud_MP_FC.Value;
            Config.data.FuelCosts.Coolant.Cost =  nud_CL_FC.Value;
            Config.data.FuelCosts.Robotics.Cost = nud_RB_FC.Value;
            Config.data.FuelCosts.HeIso.Cost = nud_HE_FC.Value;
            Config.data.FuelCosts.N2Iso.Cost = nud_N2_FC.Value;
            Config.data.FuelCosts.H2Iso.Cost = nud_H2_FC.Value;
            Config.data.FuelCosts.O2Iso.Cost = nud_O2_FC.Value;
            Config.data.FuelCosts.HvyWater.Cost = nud_HW_FC.Value;
            Config.data.FuelCosts.LiqOzone.Cost = nud_LO_FC.Value;
            Config.data.FuelCosts.Charters.Cost = nud_CH_FC.Value;
            Config.data.FuelCosts.Strontium.Cost = nud_ST_FC.Value;

            Config.SaveConfiguration();
            CalculateAndDisplayDesignFuelData();
        }

        private void dg_MonitoredTowers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv;

            dgv = (DataGridView)sender;

            Config.data.SortedColumnIndex = e.ColumnIndex;

            Config.data.MonSortOrder = dgv.SortOrder;

            Config.SaveConfiguration();
        }
        
#endregion


#region Set Module Charge - Handling Routines
 
        private void tsm_SetModuleCharge_Click(object sender, EventArgs e)
        {

        }


#endregion

    
     }


}
