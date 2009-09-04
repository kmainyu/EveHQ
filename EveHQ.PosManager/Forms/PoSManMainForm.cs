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
        public PlayerList PL = new PlayerList();
        public NotificationList NL = new NotificationList();
        Configuration Config = new Configuration();
        public static BackgroundWorker apiWorker;
        PoS_Item api;
        FuelBay tt = null;
        bool load = false;
        bool update = false;
        bool timeCheck = false;
        bool PosChanged = false;
        bool UpdateTower = true;
        bool mailSendErr = false;
        private int Mon_dg_indx = 0;
        private int Fil_dg_indx = 0;
        private string Mon_dg_Pos = "";
        public string selName, AllPosFillText, SelPosFillText, SelReactPos;
        public string CurrentName = "", NewName = "";
        enum MonDG {Name, FuelR, StrontR, State, Link, Cache, CPU, Power, EnrUr, Oxy, McP, Cool, Rbt, Iso, HvW, LqO, Cht, Strt, useC, React, Owner, FTech, fHrs, sHrs, hCPU, hPow, hIso };
        enum dgPM {Name, Qty, State, Opt, fOff, dmg, rof, dps, trk, prox, swDly, Chg, cost, Cap };
        enum fillDG { Name, Loc, EnrUr, Oxy, McP, Cool, Rbt, Iso, HvW, LqO, Cht, Strt, RunT };
        enum fuelDG { type, amount, vol, cost };      
        decimal srcID, dstID;
        string srcNm, dstNm;
        // 20 max possible links for now
        private int MaxLink = 20;
        private Color[] LColor = new Color[] {Color.Blue, Color.Green, Color.Orange, Color.Violet, Color.LimeGreen,
                                              Color.LightBlue, Color.Red, Color.Yellow, Color.Coral, Color.DarkBlue,
                                              Color.DarkGreen, Color.DarkOrange, Color.DarkRed, Color.DarkViolet,
                                              Color.ForestGreen, Color.Gold, Color.Gray, Color.Lavender,
                                              Color.LightCyan, Color.Olive};
    


#region Main Form Handlers

        public PoSManMainForm()
        {
            InitializeComponent();
        }
        
        private void frmMainForm_Load(object sender, EventArgs e)
        {
            int dgCol;
            load = true;

            Config.LoadConfiguration();     // Load PoS Manager Configuration Information

            CL.LoadCategoryList();          // Load Category Listing from Disk
            TL.LoadTowerListing();          // Load Tower Listing from Disk
            ML.LoadModuleListing();         // Load Tower Modules Listing from Disk
            POSList.LoadDesignListing();    // Loads Desgined POS's from Disk
            API_D.LoadAPIListing();       // Load Tower API Data from Disk
            DPLst.LoadDPListing();          // Load Damage Profile List
            SL.LoadSystemListFromDisk();
            AL.LoadAllianceListFromDisk();
            PL.LoadPlayerList();
            NL.LoadNotificationList();

            PopulateDPList("Omni-Type");
            PopulateSystemList();
            PopulateCorpList();

            // Add PoS designes into the Designer Combo-Box pull down list for Selection
            cb_PoSName.Items.Clear();
            if (POSList.Designs.Count < 1)
            {
                cb_PoSName.Text = "No Towers Created";
            }

            // Fill in default values for New Tower Data
            foreach (POS pl in POSList.Designs)
            {
                cb_PoSName.Items.Add(pl.Name);

                if (pl.ReactionLinks == null)
                {
                    pl.ReactionLinks = new ArrayList();
                    pl.React_TS = DateTime.Now;
                }

                if (pl.Owner == null)
                {
                    pl.Owner = "";
                    pl.FuelTech = "";
                    pl.ownerID = 0;
                    pl.fuelTechID = 0;
                }

                if (pl.Owner == "")
                    pl.Owner = pl.CorpName;

                // Actual testing reveals that the DB values are NOT correct for 
                // structure resistances, they are in reality ZERO
                pl.PosTower.Struct.EMP = 0;
                pl.PosTower.Struct.Explosive = 0;
                pl.PosTower.Struct.Kinetic = 0;
                pl.PosTower.Struct.Thermal = 0;

                foreach (Module m in pl.Modules)
                {
                    if (m.ReactList == null)
                        CopyMissingReactData(m);
                    else
                    {
                        // Copy the Cache load data for Reactions, etc... Just in case it changed
                        // or was updated.
                        foreach (Module bm in ML.Modules)
                        {
                            if (bm.typeID == m.typeID)
                            {
                                m.ReactList = new ArrayList(bm.ReactList);
                                m.MSRList = new ArrayList(bm.MSRList);
                                m.InputList = new ArrayList(bm.InputList);
                                m.OutputList = new ArrayList(bm.OutputList);
                            }
                        }
                    }
                }
            }

            BuildPOSListForMonitoring();
            UpdateAllTowerSovLevels();

            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            POSList.CalculatePOSReactions();

            GetTowerItemListData();
            SetReactionValuesAndUpdateDisplay();
            
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
                    if (dg_MonitoredTowers.Rows[x].Cells[(int)MonDG.Name].Value.ToString().Contains(Mon_dg_Pos))
                    {
                        Mon_dg_indx = x;
                        break;
                    }
                }
            }

            if (Mon_dg_indx >= dg_MonitoredTowers.RowCount)
                Mon_dg_indx = dg_MonitoredTowers.RowCount - 1;

            // Set default time period to FILL
            tscb_TimePeriod.SelectedIndex = (int)Config.data.maintTP;
            nud_PeriodValue.Value = Config.data.maintPV;

            if ((Config.data.dgMonBool == null) || (Config.data.dgMonBool.Count < 22))
            {
                if (Config.data.dgMonBool == null)
                {
                    Config.data.dgMonBool = new ArrayList();

                    for (int x = 0; x < 22; x++)
                        Config.data.dgMonBool.Add(true);
                }
                else
                {
                    for (int x = Config.data.dgMonBool.Count; x<22; x++)
                        Config.data.dgMonBool.Add(true);
                }
                Config.SaveConfiguration();
            }
            else
            {
                dgCol = 0;
                foreach (bool dgcs in Config.data.dgMonBool)
                {
                    if (dgCol <= dg_MonitoredTowers.Columns.Count)
                    {
                        dg_MonitoredTowers.Columns[dgCol].Visible = dgcs;
                        dgCol++;
                    }
                }
            }

            if ((Config.data.dgDesBool == null) || (Config.data.dgDesBool.Count < 14))
            {
                Config.data.dgDesBool = new ArrayList();

                for (int x = 0; x < 14; x++)
                    Config.data.dgDesBool.Add(true);

                Config.SaveConfiguration();
            }
            else
            {
                dgCol = 0;
                foreach (bool dgcs in Config.data.dgDesBool)
                {
                    if (dgCol <= dg_PosMods.Columns.Count)
                    {
                        dg_PosMods.Columns[dgCol].Visible = dgcs;
                        dgCol++;
                    }
                }
            }

            SortMonitorDataGridByColumn(dg_MonitoredTowers, Config.data.SortedColumnIndex);

            if (Mon_dg_indx >= 0)
            {
                dg_MonitoredTowers.CurrentCell = dg_MonitoredTowers.Rows[Mon_dg_indx].Cells[(int)MonDG.Name];
                Object o = new Object();
                EventArgs ea = new EventArgs();
                load = false;
                Mon_dg_indx = -1;
                dg_MonitoredTowers_SelectionChanged(o, ea);
                load = true;
            }

            load = false;

            tsl_APIState.Text = "Updating API Data and Fuel Calculations";
            bgw_APIUpdate.RunWorkerAsync();
        }

        private void tb_PosManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            int colCt = 1;
            bool colSt = false;
            DataGridViewColumn dgvc;
            string cName;

            if (selName == null)
                selName = "New POS";


            if (tb_PosManager.SelectedIndex == 1) // POS Designer
            {
                if (cb_PoSName.Items.Contains(selName))
                    cb_PoSName.SelectedItem = selName;
                SaveConfiguration();
            }
            else if (tb_PosManager.SelectedIndex == 0) // PoS Monitor
            {
                SaveConfiguration();
                PopulateMonitoredPoSDisplay();
            }
            else if (tb_PosManager.SelectedIndex == 2)  // POS Maintenence
            {
                PopulateTowerFillDG();
                if (dg_TowerFuelList.Rows.Count > Fil_dg_indx)
                {
                    dg_TowerFuelList.CurrentCell = dg_TowerFuelList.Rows[Fil_dg_indx].Cells[(int)fillDG.Name];
                    Object o = new Object();
                    EventArgs ea = new EventArgs();
                    dg_TowerFuelList_SelectionChanged(o, ea);
                }
                SaveConfiguration();
            }
            else if (tb_PosManager.SelectedIndex == 5) // Configuration
            {
                // Config Page - Fill in Checkbox States vs. Datagrid states
                foreach (CheckBox cb in gb_MonPosCol.Controls)
                {
                    cName = cb.Name;
                    colCt = Convert.ToInt32(cName.Replace("checkBox", ""));
                    if (colCt < 21)
                    {
                        dgvc = dg_MonitoredTowers.Columns[colCt - 1];
                        colSt = dgvc.Visible;
                        cb.Checked = colSt;
                    }
                }

                foreach (CheckBox cb in gb_PosDesignShow.Controls)
                {
                    cName = cb.Name;
                    colCt = Convert.ToInt32(cName.Replace("checkBox", ""));
                    colCt -= 24;
                    if (colCt < 15)
                    {
                        dgvc = dg_PosMods.Columns[colCt - 1];
                        colSt = dgvc.Visible;
                        cb.Checked = colSt;
                    }
                }

                PopulateFuelConfigDisplay();
            }
            else if (tb_PosManager.SelectedIndex == 3)  // Reaction Manager
            {
                PopulateTowerModuleDisplay();
            }
            else if (tb_PosManager.SelectedIndex == 4)  // Notifications
            {
                PopulatePlayerList();
                PopulateNotificationList();
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
                    POSList.UpdateListDesign(Design);
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

        private void PopulateCorpList()
        {
            APITowerData atd;
            // Build corp selection listing
            cb_CorpName.Items.Clear();
            cb_CorpName.Items.Add("Undefined");
            for (int x = 0; x < API_D.apiTower.Count; x++ )
            {
                atd = (APITowerData)API_D.apiTower.GetByIndex(x);
                if (!cb_CorpName.Items.Contains(atd.corpName))
                    cb_CorpName.Items.Add(atd.corpName);
            }
        }

#endregion


#region Conversions

        private string ConvertHoursToShortTextDisplay(decimal hours, int index)
        {
            string retVal = "", week = "", day = "", hour = "";
            int days = 0, weeks = 0;

            if (hours >= 999999)
            {
                retVal = "Infinite";
            }
            else
            {
                if (index == 2)
                    hours = hours * (24 * 7);
                else if (index == 1)
                    hours = hours * 24;

                if (index >= 2)
                {
                    weeks = Convert.ToInt32(Math.Truncate(hours / (24 * 7)));
                    if (weeks > 0)
                    {
                        week = weeks + " Wks";
                        hours = hours - (weeks * (24 * 7));
                    }
                }

                if (index >= 1)
                {
                    days = Convert.ToInt32(Math.Truncate(hours / 24));
                    if (days > 0)
                    {
                        day = days + " Dys";
                        hours = hours - (days * 24);
                    }
                }

                if (hours > 0)
                    hour = hours + " Hrs";

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
            }
            return retVal;
        }

        private string ConvertHoursToTextDisplay(decimal hours)
        {
            string retVal = "";
            int days;

            hours = Math.Floor(hours);

            if (hours >= 999999)
            {
                retVal = "Infinite";
            }
            else
            {
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
            }
            return retVal;
        }

        private string ConvertSecondsToTextDisplay(decimal secs)
        {
            string retVal = "";
            int hours, mins;

            hours = Convert.ToInt32(Math.Truncate(secs / 3600));
            if (hours >= 999999)
            {
                retVal = "Infinite";
            }
            else
            {

                if (hours > 0)
                {
                    retVal = String.Format("{0:0}", hours) + " h";
                    secs = secs - (hours * 3600);
                }

                mins = Convert.ToInt32(Math.Truncate(secs / 60));

                if (mins > 0)
                {
                    if (hours > 0)
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
            }
            return retVal;
        }

        public string ConvertReactionHoursToTextDisplay(decimal hours)
        {
            string retVal = "";
            int days;

            hours = Math.Floor(hours);

            if (hours >= 999999)
            {
                retVal = "";
            }
            else if (hours == 0)
            {
                retVal = "Unknown";
            }
            else
            {
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
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Cap].Value = String.Format("{0:#,0.#}", Design.PosTower.Capacity) + " m3";
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
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Cap].Value = String.Format("{0:#,0.#}", 0) + " m3";
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
                dg_PosMods.Rows[dg_ind].Cells[(int)dgPM.Cap].Value = String.Format("{0:#,0.#}", PMD.Capacity) + " m3";

               
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

                    if (PMD.Name.Contains("Energy Neut"))
                    {
                    }
                    else
                    {
                        total_damage += dmg;
                        total_dps += dps;
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

            retVal *= m.Qty;

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

        private decimal CalculateSiloCapacity(Module m, POS p)
        {
            decimal mult;

            if (m.Category.Contains("Silo"))
            {
                mult = 1 + (decimal)(p.PosTower.Bonuses.SiloCap / 100);

                return m.Capacity * mult;
            }
            else
                return m.Capacity;
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

            pbc = ComputeBayPercentage(cpu_used, cpu);
            pbp = ComputeBayPercentage(power_used, power);

            pb_CPU.TextOverlay = cpu_used + " / " + cpu;
            pb_CPU.Value = Convert.ToInt32(pbc);
            pb_Power.TextOverlay = power_used + " / " + power;
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
            pb_CPU.TextOverlay = "";
            pb_Power.TextOverlay = "";
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

                            nud_StrontInterval.Maximum = Design.ComputeMaxPosStrontTime();
                            nud_DesFuelPeriod.Maximum = Design.ComputeMaxPosRunTimeForLoad();
                            
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
                        rtb_Other.Text += "Volume   <" + t.Volume + " m3>\n";
                        rtb_Other.Text += "Capacity <" + t.Capacity + " m3>\n";
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
                        rtb_Other.Text += "Volume   <" + m.Volume + " m3>\n";
                        rtb_Other.Text += "Capacity <" + m.Capacity + " m3>\n";
                    }
                }
            }
        }

        private void GetDesignItemData(int typeID, string cat)
        {
            string fuel = "", tName;
            decimal cap;

            tName = cb_PoSName.Text;

            foreach (POS p in POSList.Designs)
            {
                if (p.Name == tName)
                {
                    if (cat.Contains("Tower"))
                    {
                        if (p.PosTower.CPU > p.PosTower.CPU_Used)
                        {
                            l_cpu.ForeColor = Color.Green;
                            l_cpu.Text = String.Format("{0:#,0.#}", p.PosTower.CPU);
                        }
                        else
                        {
                            l_cpu.ForeColor = Color.Red;
                            l_cpu.Text = String.Format("{0:#,0.#}", p.PosTower.CPU_Used);
                        }

                        if (p.PosTower.Power > p.PosTower.Power_Used)
                        {
                            l_power.ForeColor = Color.Green;
                            l_power.Text = String.Format("{0:#,0.#}", p.PosTower.Power);
                        }
                        else
                        {
                            l_power.ForeColor = Color.Red;
                            l_power.Text = String.Format("{0:#,0.#}", p.PosTower.Power_Used);
                        }

                        l_armor_hp.Text = String.Format("{0:#,0.#}", p.PosTower.Armor.Amount);
                        l_shield_hp.Text = String.Format("{0:#,0.#}", p.PosTower.Shield.Amount);
                        l_shield_em.Text = String.Format("{0:#,0.#}", p.PosTower.Shield.EMP) + "%";
                        l_shield_exp.Text = String.Format("{0:#,0.#}", p.PosTower.Shield.Explosive) + "%";
                        l_shield_kin.Text = String.Format("{0:#,0.#}", p.PosTower.Shield.Kinetic) + "%";
                        l_shield_therm.Text = String.Format("{0:#,0.#}", p.PosTower.Shield.Thermal) + "%";
                        l_struct_hp.Text = String.Format("{0:#,0.#}", p.PosTower.Struct.Amount);
                        l_struct_emp.Text = String.Format("{0:#,0.#}", p.PosTower.Struct.EMP) + "%";
                        l_struct_exp.Text = String.Format("{0:#,0.#}", p.PosTower.Struct.Explosive) + "%";
                        l_struct_kin.Text = String.Format("{0:#,0.#}", p.PosTower.Struct.Kinetic) + "%";
                        l_struct_therm.Text = String.Format("{0:#,0.#}", p.PosTower.Struct.Thermal) + "%";
                        l_SI_Cost.Text = String.Format("{0:#,0.#}", p.PosTower.Cost) + " isk";

                        fuel += "Fuel Usage -->\n";
                        fuel += "Enriched Uranium: " + p.PosTower.Fuel.EnrUran.PeriodQty + "\n";
                        fuel += "Oxygen: " + p.PosTower.Fuel.Oxygen.PeriodQty + "\n";
                        fuel += "Mechanical Parts: " + p.PosTower.Fuel.MechPart.PeriodQty + "\n";
                        fuel += "Coolant: " + p.PosTower.Fuel.Coolant.PeriodQty + "\n";
                        fuel += "Robotics: " + p.PosTower.Fuel.Robotics.PeriodQty + "\n";
                        fuel += "Heavy Water: " + p.PosTower.Fuel.HvyWater.PeriodQty + "\n";
                        fuel += "Liquid Ozone: " + p.PosTower.Fuel.LiqOzone.PeriodQty + "\n";
                        fuel += "Charters: " + p.PosTower.Fuel.Charters.PeriodQty + "\n";
                        fuel += "Strontium: " + p.PosTower.Fuel.Strontium.PeriodQty + "\n";

                        if (p.PosTower.Fuel.N2Iso.PeriodQty > 0)
                        {
                            fuel += "Nitrogen Isotopes: " + p.PosTower.Fuel.N2Iso.PeriodQty + "\n";
                        }
                        else if (p.PosTower.Fuel.HeIso.PeriodQty > 0)
                        {
                            fuel += "Helium Isotopes: " + p.PosTower.Fuel.HeIso.PeriodQty + "\n";
                        }
                        else if (p.PosTower.Fuel.H2Iso.PeriodQty > 0)
                        {
                            fuel += "Hydrogen Isotopes: " + p.PosTower.Fuel.H2Iso.PeriodQty + "\n";
                        }
                        else if (p.PosTower.Fuel.O2Iso.PeriodQty > 0)
                        {
                            fuel += "Oxygen Isotopes: " + p.PosTower.Fuel.O2Iso.PeriodQty + "\n";
                        }

                        rtb_Other.Text = p.PosTower.Desc + "\n\n" + fuel + "\n" + p.PosTower.OtherInfo;
                        rtb_Other.Text += "Volume   <" + p.PosTower.Volume + " m3>\n";
                        rtb_Other.Text += "Capacity <" + p.PosTower.Capacity + " m3>\n";
                    }
                    else
                    {
                        foreach (Module m in p.Modules)
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
                                rtb_Other.Text += "Volume   <" + m.Volume + " m3>\n";
                                cap = CalculateSiloCapacity(m, p);
                                rtb_Other.Text += "Capacity <" + cap + " m3>\n";

                                break;
                            }
                        }
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

            if(e.Button == MouseButtons.Middle)
                Mouse_Online_Offline_Click(sender, e);

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
            if ((PosChanged) && (Design.Name != "New POS") && (Design.Name != ""))
            {
                // Query user to save any changes
                DialogResult dr = MessageBox.Show("The currently Selected POS has Changed. Do you want to Save it?", "Save POS", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    POSList.UpdateListDesign(Design);
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

                        nud_StrontInterval.Maximum = Design.ComputeMaxPosStrontTime();

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

            nud_DesFuelPeriod.Maximum = Design.ComputeMaxPosRunTimeForLoad();
            if (Design.PosTower.typeID != 0)
            {
                cb_Interval.SelectedIndex = (int)Design.PosTower.Design_Interval;
                if (Design.PosTower.Design_Int_Qty > 0)
                    nud_DesFuelPeriod.Value = Design.PosTower.Design_Int_Qty;
                else
                    nud_DesFuelPeriod.Value = 1;
                nud_StrontInterval.Value = Design.PosTower.Design_Stront_Qty;
            }
            else
            {
                cb_Interval.SelectedIndex = 1;
                nud_DesFuelPeriod.Minimum = 0;
                nud_DesFuelPeriod.Maximum = 1;
                nud_DesFuelPeriod.Value = 0;
                nud_StrontInterval.Minimum = 0;
                nud_StrontInterval.Maximum = 1;
                nud_StrontInterval.Value = 0;
            }

            cb_SovLevel.SelectedIndex = Design.SovLevel;
            cb_System.Text = Design.System;
            cb_CorpName.Text = Design.CorpName;
            cb_systemMoon.Text = Design.Moon;

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

        private void cb_systemMoon_SelectedIndexChanged(object sender, EventArgs e)
        {
            Design.Moon = cb_systemMoon.Text;

            if (!load)
                PosChanged = true;
        }

        private void cb_System_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sov_Data syst;
            int index;

            Design.System = cb_System.Text;

            if (!load)
                PosChanged = true;

            FindAndGetSystemMoons();

            index = SL.Systems.IndexOfKey(Design.System);
            syst = (Sov_Data)SL.Systems.GetByIndex(index);

            if (Decimal.Compare(syst.secLevel, Convert.ToDecimal(0.45)) > 0)
                Design.UseChart = true;
            else
                Design.UseChart = false;
            
            CalculatePOSData();
        }

        private void FindAndGetSystemMoons()
        {
            string strSQL;
            decimal sysID;
            Sov_Data sd;
            DataSet ml;
            
            sd = (Sov_Data)SL.Systems[Design.System];
            sysID = sd.systemID;
            
            strSQL = "SELECT mapDenormalize.itemName FROM mapDenormalize WHERE (mapDenormalize.solarSystemID=" + sysID + " AND mapDenormalize.groupID=8);";
            ml = EveHQ.Core.DataFunctions.GetData(strSQL);

            cb_systemMoon.Items.Clear();
            if (ml.Tables.Count > 0)
            {
                foreach (DataRow dr in ml.Tables[0].Rows)
                {
                    cb_systemMoon.Items.Add(dr[0].ToString());
                }
            }
        }

        private void cb_CorpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            APITowerData apid;

            if (Design.itemID != 0)
            {
                cb_CorpName.Text = Design.CorpName;
                return;
            }
            if (Design.CorpName != cb_CorpName.Text)
            {
                Design.CorpName = cb_CorpName.Text;

                for (int x = 0; x < API_D.apiTower.Count; x++)
                {
                    apid = (APITowerData)API_D.apiTower.GetByIndex(x);
                    if (apid.corpName == Design.CorpName)
                    {
                        if (Design.corpID != apid.corpID)
                        {
                            // Update the corpID for the POS and Save to Disk
                            Design.corpID = apid.corpID;
                            POSList.SaveDesignListing();
                        }
                        break;
                    }
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

            POSList.SaveDesignListing();
        }

        private void UpdateLinkedTowerSovLevel(POS p)
        {
            decimal corpID;
            string strSQL;
            Sov_Data sd;
            Alliance_Data ad;
            APITowerData td;
            DataSet ml;

            if (p.corpID != 0)
            {
                // This POS is Linked to a Corp
                corpID = p.corpID;

                ad = (Alliance_Data)AL.alliances[corpID];
                if (ad != null)
                {
                    // Corp is in an alliance
                    // Find system in system list, update SOV level accordingly
                    sd = (Sov_Data)SL.Systems[p.System];
                    if (sd != null)
                    {
                        if (sd.allianceID == ad.allianceID)
                        {
                            // Found the correct system and alliance ID
                            p.SovLevel = (int)sd.sovLevel;
                        }
                    }
                }
            }

            if (p.itemID != 0)
            {
                // Tower is linked to CORP Data
                td = API_D.GetAPIDataMemberForTowerID(p.itemID);

                if (td != null)
                {
                    // Update Tower Moon from API Data
                    strSQL = "SELECT mapDenormalize.itemName FROM mapDenormalize WHERE mapDenormalize.itemID=" + td.moonID + ";";
                    ml = EveHQ.Core.DataFunctions.GetData(strSQL);

                    p.Moon = ml.Tables[0].Rows[0].ItemArray[0].ToString();

                    switch (td.stateV)
                    {
                        case 0:
                            p.PosTower.State = "Unanchored";
                            break;
                        case 1:
                            p.PosTower.State = "Offline";
                            break;
                        case 2:
                            p.PosTower.State = "Onlining";
                            break;
                        case 3:
                            p.PosTower.State = "Reinforced";
                            break;
                        case 4:
                            p.PosTower.State = "Online";
                            break;
                    }
                }
                else
                {
                    // Tower Item ID no longer exists in API Data, so clear it as link is no longer valid
                    p.itemID = 0;
                }
            }

            // Update Tower Charter Requirements
            if(SL.Systems.Contains(p.System))
            {
                sd = (Sov_Data)SL.Systems[p.System];
                if (Decimal.Compare(sd.secLevel, Convert.ToDecimal(0.45)) > 0)
                    p.UseChart = true;
                else
                    p.UseChart = false;
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

        private ArrayList GetLongestSiloRunTime(POS p)
        {
            decimal runT = 9999999999999999;
            string rText, rName = "";
            ArrayList retVal = new ArrayList();

            foreach (Module m in p.Modules)
            {
                switch (Convert.ToInt64(m.ModType))
                {
                    case 2:
                    case 8:
                    case 9:
                    case 11:
                    case 12:
                    case 13:
                        if ((m.FillEmptyTime < runT) && (m.State == "Online"))
                        {
                            rName = m.selMineral.name;
                            runT = m.FillEmptyTime;
                        }
                        break;
                    default:
                        break;
                }
            }

            rText = ConvertReactionHoursToTextDisplay(runT);
            if (rText.Length > 0)
                rText += " <" + rName + ">";

            retVal.Add(runT);
            retVal.Add(rText);

            return retVal;
        }

        private void PopulateMonitoredPoSDisplay()
        {
            APITowerData apid;
            int dg_ind, ind_ct;
            string line, cache, strSQL, loc;
            DateTime now, cTim, ref_TM;
            TimeSpan diffT;
            decimal ReactTime = 0;
            DataSet locData;
            ArrayList ReactRet;

            foreach (POS p in POSList.Designs)
            {
                if (p.Monitored)
                {
                    dg_ind = -1;
                    ind_ct = 0;
                    foreach(DataGridViewRow dr in dg_MonitoredTowers.Rows)
                    {
                        if (dr.Cells[(int)MonDG.Name].Value.ToString().Contains(p.Name))
                        {
                            dg_ind = ind_ct;
                            break;
                        }

                        ind_ct++;
                    }

                    if (dg_ind < 0)
                    {
                        // Did not find the tower in the existing list, so Add new row data to DG
                        dg_ind = dg_MonitoredTowers.Rows.Add();
                    }

                    if(p.Moon != "")
                        line = p.Name + " < " + p.Moon + " >[Sov" + p.SovLevel + "]";
                    else
                        line = p.Name + " < " + p.System + " >[Sov" + p.SovLevel + "]";

                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Name].Value = line;
                    line = ConvertHoursToTextDisplay(p.PosTower.F_RunTime);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.FuelR].Value = line;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.fHrs].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.fHrs].Value = p.PosTower.F_RunTime;
                    line = ConvertHoursToTextDisplay(p.PosTower.Fuel.Strontium.RunTime);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.StrontR].Value = line;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.sHrs].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.sHrs].Value = p.PosTower.Fuel.Strontium.RunTime;

                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.State].Value = p.PosTower.State;

                    ref_TM = DateTime.Now;
                    if (p.itemID != 0)
                    {
                        apid = API_D.GetAPIDataMemberForTowerID(p.itemID);
                        // Get Table With Tower or Tower Item Information
                        if (apid.locName == "Unknown")
                        {
                            strSQL = "SELECT itemName FROM mapDenormalize WHERE mapDenormalize.itemID=" + apid.locID + ";";
                            locData = EveHQ.Core.DataFunctions.GetData(strSQL);
                            loc = locData.Tables[0].Rows[0].ItemArray[0].ToString();
                            apid.locName = loc;
                            API_D.SaveAPIListing();
                        }

                        line = "<" + apid.locName + "> ";
                        line += apid.towerName;

                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Link].Value = line;

                        cache = apid.cacheUntil;
                        cTim = Convert.ToDateTime(cache);
                        cTim = TimeZone.CurrentTimeZone.ToLocalTime(cTim);
                        now = DateTime.Now;
                        diffT = now.Subtract(cTim);

                        switch (apid.stateV)
                        {
                            case 0:
                                p.PosTower.State = "Unanchored";
                                break;
                            case 1:
                                p.PosTower.State = "Offline";
                                break;
                            case 2:
                                p.PosTower.State = "Onlining";
                                break;
                            case 3:
                                p.PosTower.State = "Reinforced";
                                ref_TM = Convert.ToDateTime(apid.stateTS);
                                break;
                            case 4:
                                p.PosTower.State = "Online";
                                break;
                        }

                        if(diffT.Hours > 1)
                        {
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cache].Value = cTim.ToString();
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
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hCPU].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hCPU].Value = p.PosTower.CPU_Used;

                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Power].Value = String.Format("{0:#,0.#}", p.PosTower.Power_Used) + " / " + String.Format("{0:#,0.#}", p.PosTower.Power);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hPow].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hPow].Value = p.PosTower.Power_Used;

                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.EnrUr].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Oxy].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.McP].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cool].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Rbt].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.HvW].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.LqO].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cht].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Strt].ValueType = typeof(decimal);
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].ValueType = typeof(decimal);

                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.EnrUr].Value = p.PosTower.Fuel.EnrUran.Qty;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Oxy].Value = p.PosTower.Fuel.Oxygen.Qty;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.McP].Value = p.PosTower.Fuel.MechPart.Qty;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cool].Value = p.PosTower.Fuel.Coolant.Qty;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Rbt].Value = p.PosTower.Fuel.Robotics.Qty;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.HvW].Value = p.PosTower.Fuel.HvyWater.Qty;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.LqO].Value = p.PosTower.Fuel.LiqOzone.Qty;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cht].Value = p.PosTower.Fuel.Charters.Qty;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Strt].Value = p.PosTower.Fuel.Strontium.Qty;

                    if (p.PosTower.Fuel.HeIso.PeriodQty > 0)
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.HeIso.Qty) + " He";
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].ValueType = typeof(decimal);
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].Value = p.PosTower.Fuel.HeIso.Qty;
                    }
                    else if (p.PosTower.Fuel.H2Iso.PeriodQty > 0)
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.H2Iso.Qty) + " H2";
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].ValueType = typeof(decimal);
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].Value = p.PosTower.Fuel.H2Iso.Qty;
                    }
                    else if (p.PosTower.Fuel.N2Iso.PeriodQty > 0)
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.N2Iso.Qty) + " N2";
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].ValueType = typeof(decimal);
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].Value = p.PosTower.Fuel.N2Iso.Qty;
                    }
                    else if (p.PosTower.Fuel.O2Iso.PeriodQty > 0)
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.Fuel.O2Iso.Qty) + " O2";
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].ValueType = typeof(decimal);
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].Value = p.PosTower.Fuel.O2Iso.Qty;
                    }
                    else
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Value = String.Format("{0:#,0.#}", 0) + " ??";
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].ValueType = typeof(decimal);
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.hIso].Value = 0;
                    }

                    if (p.PosTower.State == "Online")
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.State].Style.BackColor = Color.LawnGreen;
                        if (p.PosTower.F_RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.FuelR].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.FuelR].Style.BackColor = Color.Gainsboro;

                        if (p.PosTower.Fuel.EnrUran.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.EnrUr].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.EnrUr].Style.BackColor = Color.Gainsboro;

                        if (p.PosTower.Fuel.Oxygen.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Oxy].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Oxy].Style.BackColor = Color.Gainsboro;

                        if (p.PosTower.Fuel.MechPart.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.McP].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.McP].Style.BackColor = Color.Gainsboro;

                        if (p.PosTower.Fuel.Coolant.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cool].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cool].Style.BackColor = Color.Gainsboro;

                        if (p.PosTower.Fuel.Robotics.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Rbt].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Rbt].Style.BackColor = Color.Gainsboro;
                            
                        if (p.PosTower.Fuel.HvyWater.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.HvW].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.HvW].Style.BackColor = Color.Gainsboro;

                        if (p.PosTower.Fuel.LiqOzone.RunTime < 24)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.LqO].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.LqO].Style.BackColor = Color.Gainsboro;

                        if (p.UseChart)
                            if (p.PosTower.Fuel.Charters.RunTime < 24)
                                dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cht].Style.BackColor = Color.Red;
                            else
                                dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Cht].Style.BackColor = Color.Gainsboro;

                        if (p.PosTower.Fuel.Strontium.RunTime < 4)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Strt].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Strt].Style.BackColor = Color.Gainsboro;

                        if ((p.PosTower.Fuel.HeIso.PeriodQty > 0) && (p.PosTower.Fuel.HeIso.RunTime < 24))
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Style.BackColor = Color.Red;
                        else if ((p.PosTower.Fuel.N2Iso.PeriodQty > 0) && (p.PosTower.Fuel.N2Iso.RunTime < 24))
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Style.BackColor = Color.Red;
                        else if ((p.PosTower.Fuel.H2Iso.PeriodQty > 0) && (p.PosTower.Fuel.H2Iso.RunTime < 24))
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Style.BackColor = Color.Red;
                        else if ((p.PosTower.Fuel.O2Iso.PeriodQty > 0) && (p.PosTower.Fuel.O2Iso.RunTime < 24))
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Style.BackColor = Color.Red;
                        else
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Iso].Style.BackColor = Color.Gainsboro;

                    }
                    else if (p.PosTower.State == "Offline")
                    {
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.State].Style.BackColor = Color.Gold;
                    }
                    else
                    {
                        // Tower is Reinforced !
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.State].Value = ref_TM.ToString();
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.State].Style.BackColor = Color.LightCoral;
                        if (p.PosTower.S_RunTime < 4)
                            dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.StrontR].Style.BackColor = Color.Red;
                    }

                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.useC].Value = p.UseChart;

                    ReactRet = GetLongestSiloRunTime(p);
                    ReactTime = (decimal)ReactRet[0];
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.React].Value = (string)ReactRet[1];
                    if(ReactTime < 6)
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.React].Style.BackColor = Color.Red;
                    else if (ReactTime < 24)
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.React].Style.BackColor = Color.Gold;
                    else
                        dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.React].Style.BackColor = Color.Gainsboro;

                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.Owner].Value = p.Owner;
                    dg_MonitoredTowers.Rows[dg_ind].Cells[(int)MonDG.FTech].Value = p.FuelTech;
                }
            }

            if (Mon_dg_Pos != "")
            {
                // Find index to selected POS
                for (int x = 0; x < dg_MonitoredTowers.Rows.Count; x++)
                {
                    if (dg_MonitoredTowers.Rows[x].Cells[(int)MonDG.Name].Value.ToString().Contains(Mon_dg_Pos))
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
        }

        private void SortMonitorDataGridByColumn(DataGridView dgv, int ColIndex)
        {
            if (load)
            {
                 if (Config.data.MonSortOrder == SortOrder.Descending)
                     Config.data.MonSortOrder = SortOrder.Ascending;
                 else
                     Config.data.MonSortOrder = SortOrder.Descending;
            }

            if ((ColIndex == (int)MonDG.FuelR) && (dgv.Columns[(int)MonDG.FuelR].SortMode == DataGridViewColumnSortMode.Programmatic))
            {
                // Sort on hidden fuel hours column
                if (Config.data.MonSortOrder == SortOrder.Descending)
                {
                    Config.data.MonSortOrder = SortOrder.Ascending;
                    dgv.Sort(dgv.Columns[(int)MonDG.fHrs], ListSortDirection.Ascending);
                }
                else
                {
                    Config.data.MonSortOrder = SortOrder.Descending;
                    dgv.Sort(dgv.Columns[(int)MonDG.fHrs], ListSortDirection.Descending);
                }
            }
            else if ((ColIndex == (int)MonDG.CPU) && (dgv.Columns[(int)MonDG.CPU].SortMode == DataGridViewColumnSortMode.Programmatic))
            {
                if (Config.data.MonSortOrder == SortOrder.Descending)
                {
                    Config.data.MonSortOrder = SortOrder.Ascending;
                    dgv.Sort(dgv.Columns[(int)MonDG.hCPU], ListSortDirection.Ascending);
                }
                else
                {
                    Config.data.MonSortOrder = SortOrder.Descending;
                    dgv.Sort(dgv.Columns[(int)MonDG.hCPU], ListSortDirection.Descending);
                }
            }
            else if ((ColIndex == (int)MonDG.Power) && (dgv.Columns[(int)MonDG.Power].SortMode == DataGridViewColumnSortMode.Programmatic))
            {
                if (Config.data.MonSortOrder == SortOrder.Descending)
                {
                    Config.data.MonSortOrder = SortOrder.Ascending;
                    dgv.Sort(dgv.Columns[(int)MonDG.hPow], ListSortDirection.Ascending);
                }
                else
                {
                    Config.data.MonSortOrder = SortOrder.Descending;
                    dgv.Sort(dgv.Columns[(int)MonDG.hPow], ListSortDirection.Descending);
                }
            }
            else if ((ColIndex == (int)MonDG.StrontR) && (dgv.Columns[(int)MonDG.StrontR].SortMode == DataGridViewColumnSortMode.Programmatic))
            {
                // Sort on hidden stront hours column
                if (Config.data.MonSortOrder == SortOrder.Descending)
                {
                    Config.data.MonSortOrder = SortOrder.Ascending;
                    dgv.Sort(dgv.Columns[(int)MonDG.sHrs], ListSortDirection.Ascending);
                }
                else
                {
                    Config.data.MonSortOrder = SortOrder.Descending;
                    dgv.Sort(dgv.Columns[(int)MonDG.sHrs], ListSortDirection.Descending);
                }
            }
            else if ((ColIndex == (int)MonDG.Iso) && (dgv.Columns[(int)MonDG.Iso].SortMode == DataGridViewColumnSortMode.Programmatic))
            {
                // Sort on hidden stront hours column
                if (Config.data.MonSortOrder == SortOrder.Descending)
                {
                    Config.data.MonSortOrder = SortOrder.Ascending;
                    dgv.Sort(dgv.Columns[(int)MonDG.hIso], ListSortDirection.Ascending);
                }
                else
                {
                    Config.data.MonSortOrder = SortOrder.Descending;
                    dgv.Sort(dgv.Columns[(int)MonDG.hIso], ListSortDirection.Descending);
                }
            }
            else
            {
                Config.data.MonSortOrder = dgv.SortOrder;
            }
        }

        private void dg_MonitoredTowers_SelectionChanged(object sender, EventArgs e)
        {
            decimal qty;
            string posItm, posName, line;
            APITowerData td;

            if (dg_MonitoredTowers.CurrentRow == null)
                return;

            if (dg_MonitoredTowers.CurrentRow.Cells[(int)MonDG.Name].Value != null)
            {
                selName = dg_MonitoredTowers.CurrentRow.Cells[(int)MonDG.Name].Value.ToString();
                posName = selName.Substring(0, selName.IndexOf(" <"));
                selName = posName;
                Mon_dg_Pos = posName;
                
                if (Mon_dg_indx == dg_MonitoredTowers.CurrentRow.Index)
                    return;

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
                        rtb_POSMods.Clear();
                        UpdateTower = false;

                        rtb_POSMods.AppendText("Modules at POS\n");
                        rtb_POSMods.AppendText("---------------------------------\n");
                        foreach (Module m in pl.Modules)
                        {
                            qty = m.Qty;
                            posItm = m.Name;
                            if (qty > 1)
                                posItm += "[" + qty + "]";
                            posItm += " <" + m.State + ">\n";
                            rtb_POSMods.AppendText(posItm);
                        }
                        // Set up Default NUD Fuel Values to make calculations work correctly
                        nud_EnrUran.Value = pl.PosTower.Fuel.EnrUran.Qty;
                        nud_Oxy.Value = pl.PosTower.Fuel.Oxygen.Qty;
                        nud_MechPart.Value = pl.PosTower.Fuel.MechPart.Qty;
                        nud_Robotic.Value = pl.PosTower.Fuel.Robotics.Qty;
                        nud_Coolant.Value = pl.PosTower.Fuel.Coolant.Qty;
                        nud_HvyWtr.Value = pl.PosTower.Fuel.HvyWater.Qty;
                        nud_LiqOzn.Value = pl.PosTower.Fuel.LiqOzone.Qty;
                        nud_Charter.Value = pl.PosTower.Fuel.Charters.Qty;
                        nud_Stront.Value = pl.PosTower.Fuel.Strontium.Qty;
                        if (pl.PosTower.Fuel.HeIso.PeriodQty > 0)
                            nud_Isotope.Value = pl.PosTower.Fuel.HeIso.Qty;
                        if (pl.PosTower.Fuel.H2Iso.PeriodQty > 0)
                            nud_Isotope.Value = pl.PosTower.Fuel.H2Iso.Qty;
                        if (pl.PosTower.Fuel.O2Iso.PeriodQty > 0)
                            nud_Isotope.Value = pl.PosTower.Fuel.O2Iso.Qty;
                        if (pl.PosTower.Fuel.N2Iso.PeriodQty > 0)
                            nud_Isotope.Value = pl.PosTower.Fuel.N2Iso.Qty;

                        // Tower is linked to CORP Data
                        td = API_D.GetAPIDataMemberForTowerID(pl.itemID);
                        if (td != null)
                        {
                            rtb_POSMods.AppendText("\n---------------------------------\n");
                            rtb_POSMods.AppendText("General Tower Settings:\n");
                            rtb_POSMods.AppendText("---------------------------------\n");
                            rtb_POSMods.AppendText("Usage Flags : " + td.useFlag + "\n");
                            rtb_POSMods.AppendText("Deploy Flags : " + td.depFlag + "\n");
                            line = "Allow Corp Members : ";
                            if (td.allowCorp)
                                line += "[X]\n";
                            else
                                line += "[ ]\n";
                            rtb_POSMods.AppendText(line);
                            line = "Allow Alliance Members : ";
                            if (td.allowAlliance)
                                line += "[X]\n";
                            else
                                line += "[ ]\n";
                            rtb_POSMods.AppendText(line);
                            line = "Claim Sovereignty : ";
                            if (td.claimSov)
                                line += "[X]\n";
                            else
                                line += "[ ]\n";
                            rtb_POSMods.AppendText(line);

                            rtb_POSMods.AppendText("\n---------------------------------\n");
                            rtb_POSMods.AppendText("Tower Combat Settings (Attack):\n");
                            rtb_POSMods.AppendText("---------------------------------\n");
                            line = "On Standing Drop : < " + td.standDrop + " >\n";
                            rtb_POSMods.AppendText(line);
                            line = "On Status Drop : ";
                            if (td.onStatusDrop)
                                line += "[X] < " + td.statusDrop + " >\n";
                            else
                                line += "[ ] < " + td.statusDrop + " >\n";
                            rtb_POSMods.AppendText(line);
                            line = "On Agression : ";
                            if (td.onAgression)
                                line += "[X]\n";
                            else
                                line += "[ ]\n";
                            rtb_POSMods.AppendText(line);
                            line = "On War Declared : ";
                            if (td.onWar)
                                line += "[X]\n";
                            else
                                line += "[ ]\n";
                            rtb_POSMods.AppendText(line);
                        }

                        UpdateTower = true;
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
            decimal sov_mod, increment;

            if ((load) || (timeCheck))
                return;

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

                        // Enr Uranium
                        l_C_EnUr.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.EnrUran.Qty);
                        l_R_EnUr.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.EnrUran.RunTime);
                        increment = pl.PosTower.Fuel.EnrUran.GetFuelQtyForPeriod(sov_mod, 1, 1);
                        nud_EnrUran.Increment = Convert.ToDecimal(increment);
                        l_QH_EnUr.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_EnUr.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.EnrUran.RunTime);
                        if (pl.PosTower.A_Fuel.EnrUran.RunTime < pl.PosTower.Fuel.EnrUran.RunTime)
                            l_AR_EnUr.ForeColor = Color.Red;
                        else
                            l_AR_EnUr.ForeColor = Color.Green;
                        if (nud_EnrUran.Value < pl.PosTower.Fuel.EnrUran.Qty)
                            nud_EnrUran.ForeColor = Color.Red;
                        else if (nud_EnrUran.Value > pl.PosTower.Fuel.EnrUran.Qty)
                            nud_EnrUran.ForeColor = Color.Green;
                        else
                            nud_EnrUran.ForeColor = Color.Blue;

                        // Oxygen
                        l_C_Oxyg.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.Oxygen.Qty);
                        l_R_Oxyg.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.Oxygen.RunTime);
                        increment = pl.PosTower.Fuel.Oxygen.GetFuelQtyForPeriod(sov_mod, 1, 1);
                        nud_Oxy.Increment = Convert.ToDecimal(increment);
                        l_QH_Oxyg.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_Oxyg.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.Oxygen.RunTime);
                        if (pl.PosTower.A_Fuel.Oxygen.RunTime < pl.PosTower.Fuel.Oxygen.RunTime)
                            l_AR_Oxyg.ForeColor = Color.Red;
                        else
                            l_AR_Oxyg.ForeColor = Color.Green;
                        if (nud_Oxy.Value > pl.PosTower.Fuel.Oxygen.Qty)
                            nud_Oxy.ForeColor = Color.Green;
                        else if (nud_Oxy.Value < pl.PosTower.Fuel.Oxygen.Qty)
                            nud_Oxy.ForeColor = Color.Red;
                        else
                            nud_Oxy.ForeColor = Color.Blue;

                        // Mechanical Parts
                        l_C_McP.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.MechPart.Qty);
                        l_R_McP.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.MechPart.RunTime);
                        increment = pl.PosTower.Fuel.MechPart.GetFuelQtyForPeriod(sov_mod, 1, 1);
                        nud_MechPart.Increment = Convert.ToDecimal(increment);
                        l_QH_McP.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_McP.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.MechPart.RunTime);
                        if (pl.PosTower.A_Fuel.MechPart.RunTime < pl.PosTower.Fuel.MechPart.RunTime)
                            l_AR_McP.ForeColor = Color.Red;
                        else
                            l_AR_McP.ForeColor = Color.Green;
                        if (nud_MechPart.Value > pl.PosTower.Fuel.MechPart.Qty)
                            nud_MechPart.ForeColor = Color.Green;
                        else if (nud_MechPart.Value < pl.PosTower.Fuel.MechPart.Qty)
                            nud_MechPart.ForeColor = Color.Red;
                        else
                            nud_MechPart.ForeColor = Color.Blue;

                        // Coolant
                        l_C_Cool.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.Coolant.Qty);
                        l_R_Cool.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.Coolant.RunTime);
                        increment = pl.PosTower.Fuel.Coolant.GetFuelQtyForPeriod(sov_mod, 1, 1);
                        nud_Coolant.Increment = Convert.ToDecimal(increment);
                        l_QH_Cool.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_Cool.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.Coolant.RunTime);
                        if (pl.PosTower.A_Fuel.Coolant.RunTime < pl.PosTower.Fuel.Coolant.RunTime)
                            l_AR_Cool.ForeColor = Color.Red;
                        else
                            l_AR_Cool.ForeColor = Color.Green;
                        if (nud_Coolant.Value > pl.PosTower.Fuel.Coolant.Qty)
                            nud_Coolant.ForeColor = Color.Green;
                        else if (nud_Coolant.Value < pl.PosTower.Fuel.Coolant.Qty)
                            nud_Coolant.ForeColor = Color.Red;
                        else
                            nud_Coolant.ForeColor = Color.Blue;

                        // Robotics
                        l_C_Robt.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.Robotics.Qty);
                        l_R_Robt.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.Robotics.RunTime);
                        increment = pl.PosTower.Fuel.Robotics.GetFuelQtyForPeriod(sov_mod, 1, 1);
                        nud_Robotic.Increment = Convert.ToDecimal(increment);
                        l_QH_Robt.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_Robt.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.Robotics.RunTime);
                        if (pl.PosTower.A_Fuel.Robotics.RunTime < pl.PosTower.Fuel.Robotics.RunTime)
                            l_AR_Robt.ForeColor = Color.Red;
                        else
                            l_AR_Robt.ForeColor = Color.Green;
                        if (nud_Robotic.Value > pl.PosTower.Fuel.Robotics.Qty)
                            nud_Robotic.ForeColor = Color.Green;
                        else if (nud_Robotic.Value < pl.PosTower.Fuel.Robotics.Qty)
                            nud_Robotic.ForeColor = Color.Red;
                        else
                            nud_Robotic.ForeColor = Color.Blue;

                        // Faction Charters
                        l_C_Chrt.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.Charters.Qty);
                        l_R_Chrt.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.Charters.RunTime);
                        increment = pl.PosTower.Fuel.Charters.GetFuelQtyForPeriod(sov_mod, 1, 1);
                        nud_Charter.Increment = Convert.ToDecimal(increment);
                        l_QH_Chrt.Text = String.Format("{0:#,0.#}", increment);
                        if (!pl.UseChart)
                        {
                            l_AR_Chrt.ForeColor = Color.Blue;
                            l_AR_Chrt.Text = "NA";
                        }
                        else
                        {
                            l_AR_Chrt.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.Charters.RunTime);
                            if (pl.PosTower.A_Fuel.Charters.RunTime < pl.PosTower.Fuel.Charters.RunTime)
                                l_AR_Chrt.ForeColor = Color.Red;
                            else
                                l_AR_Chrt.ForeColor = Color.Green;
                        }
                        if (nud_Charter.Value > pl.PosTower.Fuel.Charters.Qty)
                            nud_Charter.ForeColor = Color.Green;
                        if (nud_Charter.Value < pl.PosTower.Fuel.Charters.Qty)
                            nud_Charter.ForeColor = Color.Red;
                        else
                            nud_Charter.ForeColor = Color.Blue;

                        // Strontium
                        l_C_Strn.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.Strontium.Qty);
                        l_R_Strn.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.Strontium.RunTime);
                        increment = pl.PosTower.Fuel.Strontium.GetFuelQtyForPeriod(sov_mod, 1, 1);
                        nud_Stront.Increment = Convert.ToDecimal(increment);
                        l_QH_Strn.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_Strn.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.Strontium.RunTime);
                        if (pl.PosTower.A_Fuel.Strontium.RunTime < pl.PosTower.Fuel.Strontium.RunTime)
                            l_AR_Strn.ForeColor = Color.Red;
                        else
                            l_AR_Strn.ForeColor = Color.Green;
                        if (nud_Stront.Value > pl.PosTower.Fuel.Strontium.Qty)
                            nud_Stront.ForeColor = Color.Green;
                        else if (nud_Stront.Value < pl.PosTower.Fuel.Strontium.Qty)
                            nud_Stront.ForeColor = Color.Red;
                        else
                            nud_Stront.ForeColor = Color.Blue;

                        // Heavy Water
                        l_C_HvyW.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.HvyWater.Qty);
                        l_R_HvyW.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.HvyWater.RunTime);
                        increment = pl.PosTower.Fuel.HvyWater.GetFuelQtyForPeriod(sov_mod, pl.PosTower.CPU, pl.PosTower.CPU_Used);
                        nud_HvyWtr.Increment = increment;
                        l_QH_HvyW.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_HvyW.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.HvyWater.RunTime);
                        if (pl.PosTower.A_Fuel.HvyWater.RunTime < pl.PosTower.Fuel.HvyWater.RunTime)
                            l_AR_HvyW.ForeColor = Color.Red;
                        else
                            l_AR_HvyW.ForeColor = Color.Green;
                        if (nud_HvyWtr.Value > pl.PosTower.Fuel.HvyWater.Qty)
                            nud_HvyWtr.ForeColor = Color.Green;
                        else if (nud_HvyWtr.Value < pl.PosTower.Fuel.HvyWater.Qty)
                            nud_HvyWtr.ForeColor = Color.Red;
                        else
                            nud_HvyWtr.ForeColor = Color.Blue;

                        // Liquid Ozone
                        l_C_LiqO.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.LiqOzone.Qty);
                        l_R_LiqO.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.LiqOzone.RunTime);
                        increment = pl.PosTower.Fuel.LiqOzone.GetFuelQtyForPeriod(sov_mod, pl.PosTower.Power, pl.PosTower.Power_Used);
                        nud_LiqOzn.Increment = increment;
                        l_QH_LiqO.Text = String.Format("{0:#,0.#}", increment);
                        l_AR_LiqO.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.LiqOzone.RunTime);
                        if (pl.PosTower.A_Fuel.LiqOzone.RunTime < pl.PosTower.Fuel.LiqOzone.RunTime)
                            l_AR_LiqO.ForeColor = Color.Red;
                        else
                            l_AR_LiqO.ForeColor = Color.Green;
                        if (nud_LiqOzn.Value > pl.PosTower.Fuel.LiqOzone.Qty)
                            nud_LiqOzn.ForeColor = Color.Green;
                        else if (nud_LiqOzn.Value < pl.PosTower.Fuel.LiqOzone.Qty)
                            nud_LiqOzn.ForeColor = Color.Red;
                        else
                            nud_LiqOzn.ForeColor = Color.Blue;

                        // Isotopes
                        if (pl.PosTower.Fuel.N2Iso.PeriodQty > 0)
                        {
                            l_M_IsoType.Text = "N2";
                            l_C_Iso.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.N2Iso.Qty);
                            l_R_Iso.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.N2Iso.RunTime);
                            increment = pl.PosTower.Fuel.N2Iso.GetFuelQtyForPeriod(sov_mod, 1, 1);
                            nud_Isotope.Increment = Convert.ToDecimal(increment);
                            l_QH_Iso.Text = String.Format("{0:#,0.#}", increment);
                            l_AR_Iso.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.N2Iso.RunTime);
                            if (pl.PosTower.A_Fuel.N2Iso.RunTime < pl.PosTower.Fuel.N2Iso.RunTime)
                                l_AR_Iso.ForeColor = Color.Red;
                            else
                                l_AR_Iso.ForeColor = Color.Green;
                            if (nud_Isotope.Value > pl.PosTower.Fuel.N2Iso.Qty)
                                nud_Isotope.ForeColor = Color.Green;
                            else if (nud_Isotope.Value < pl.PosTower.Fuel.N2Iso.Qty)
                                nud_Isotope.ForeColor = Color.Red;
                            else
                                nud_Isotope.ForeColor = Color.Blue;
                        }
                        else if (pl.PosTower.Fuel.H2Iso.PeriodQty > 0)
                        {
                            l_M_IsoType.Text = "H2";
                            l_C_Iso.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.H2Iso.Qty);
                            l_R_Iso.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.H2Iso.RunTime);
                            increment = pl.PosTower.Fuel.H2Iso.GetFuelQtyForPeriod(sov_mod, 1, 1);
                            nud_Isotope.Increment = Convert.ToDecimal(increment);
                            l_QH_Iso.Text = String.Format("{0:#,0.#}", increment);
                            l_AR_Iso.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.H2Iso.RunTime);
                            if (pl.PosTower.A_Fuel.H2Iso.RunTime < pl.PosTower.Fuel.H2Iso.RunTime)
                                l_AR_Iso.ForeColor = Color.Red;
                            else
                                l_AR_Iso.ForeColor = Color.Green;
                            if (nud_Isotope.Value > pl.PosTower.Fuel.H2Iso.Qty)
                                nud_Isotope.ForeColor = Color.Green;
                            else if (nud_Isotope.Value < pl.PosTower.Fuel.H2Iso.Qty)
                                nud_Isotope.ForeColor = Color.Red;
                            else
                                nud_Isotope.ForeColor = Color.Blue;
                        }
                        else if (pl.PosTower.Fuel.O2Iso.PeriodQty > 0)
                        {
                            l_M_IsoType.Text = "O2";
                            l_C_Iso.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.O2Iso.Qty);
                            l_R_Iso.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.O2Iso.RunTime);
                            increment = pl.PosTower.Fuel.O2Iso.GetFuelQtyForPeriod(sov_mod, 1, 1);
                            nud_Isotope.Increment = Convert.ToDecimal(increment);
                            l_QH_Iso.Text = String.Format("{0:#,0.#}", increment);
                            l_AR_Iso.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.O2Iso.RunTime);
                            if (pl.PosTower.A_Fuel.O2Iso.RunTime < pl.PosTower.Fuel.O2Iso.RunTime)
                                l_AR_Iso.ForeColor = Color.Red;
                            else
                                l_AR_Iso.ForeColor = Color.Green;
                            if (nud_Isotope.Value > pl.PosTower.Fuel.O2Iso.Qty)
                                nud_Isotope.ForeColor = Color.Green;
                            else if (nud_Isotope.Value < pl.PosTower.Fuel.O2Iso.Qty)
                                nud_Isotope.ForeColor = Color.Red;
                            else
                                nud_Isotope.ForeColor = Color.Blue;
                        }
                        else if (pl.PosTower.Fuel.HeIso.PeriodQty > 0)
                        {
                            l_M_IsoType.Text = "He";
                            l_C_Iso.Text = String.Format("{0:#,0.#}", pl.PosTower.Fuel.HeIso.Qty);
                            l_R_Iso.Text = ConvertHoursToTextDisplay(pl.PosTower.Fuel.HeIso.RunTime);
                            increment = pl.PosTower.Fuel.HeIso.GetFuelQtyForPeriod(sov_mod, 1, 1);
                            nud_Isotope.Increment = Convert.ToDecimal(increment);
                            l_QH_Iso.Text = String.Format("{0:#,0.#}", increment);
                            l_AR_Iso.Text = ConvertHoursToTextDisplay(pl.PosTower.A_Fuel.HeIso.RunTime);
                            if (pl.PosTower.A_Fuel.HeIso.RunTime < pl.PosTower.Fuel.HeIso.RunTime)
                                l_AR_Iso.ForeColor = Color.Red;
                            else
                                l_AR_Iso.ForeColor = Color.Green;
                            if (nud_Isotope.Value > pl.PosTower.Fuel.HeIso.Qty)
                                nud_Isotope.ForeColor = Color.Green;
                            else if (nud_Isotope.Value < pl.PosTower.Fuel.HeIso.Qty)
                                nud_Isotope.ForeColor = Color.Red;
                            else
                                nud_Isotope.ForeColor = Color.Blue;
                        }
                        else
                        {
                            l_M_IsoType.Text = "";
                            l_C_Iso.Text = "0";
                            l_R_Iso.Text = ConvertHoursToTextDisplay(0);
                            l_QH_Iso.Text = "0";
                            l_AR_Iso.Text = ConvertHoursToTextDisplay(0);
                            l_AR_Iso.ForeColor = Color.Green;
                            nud_Isotope.Increment = 0;
                            nud_Isotope.ForeColor = Color.Blue;
                        }

                        bay_p = ComputeBayPercentage(pl.PosTower.A_Fuel.FuelUsed, pl.PosTower.A_Fuel.FuelCap);
                        pb_FuelBayFill.Value = Convert.ToInt32(bay_p);
                        pb_FuelBayFill.TextOverlay = pl.PosTower.A_Fuel.FuelUsed + " / " + pl.PosTower.A_Fuel.FuelCap;

                        bay_p = ComputeBayPercentage(pl.PosTower.A_Fuel.StrontUsed, pl.PosTower.A_Fuel.StrontCap);
                        pb_StrontBayFill.Value = Convert.ToInt32(bay_p);
                        pb_StrontBayFill.TextOverlay = pl.PosTower.A_Fuel.StrontUsed + " / " + pl.PosTower.A_Fuel.StrontCap;

                        break;
                    }
                }
            }
        }

        private void UdateMonitorInformation(object sender, EventArgs e)
        {
            tsl_APIState.Text = "Updating API Data and Fuel Calculations";
            bgw_APIUpdate.RunWorkerAsync();
        }

        private void RunCalculationsWithUpdatedInformation()
        {
            timeCheck = true;

            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);

            if (POSList.CalculatePOSReactions())
                SetReactionValuesAndUpdateDisplay();

            PopulateMonitoredPoSDisplay();
            if (Mon_dg_indx >= 0)
            {
                dg_MonitoredTowers.CurrentCell = dg_MonitoredTowers.Rows[Mon_dg_indx].Cells[(int)MonDG.Name];
                Object o = new Object();
                EventArgs ea = new EventArgs();
                dg_MonitoredTowers_SelectionChanged(o, ea);
            }

            bgw_SendNotify.RunWorkerAsync();

            timeCheck = false;
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
            //if (e.ColumnIndex == 18)
            //{
            //    foreach (POS pl in POSList.Designs)
            //    {
            //        if (selName == pl.Name)
            //        {
            //            if (!(bool)dg_MonitoredTowers.Rows[e.RowIndex].Cells[e.ColumnIndex].Value)
            //            {
            //                dg_MonitoredTowers.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
            //                pl.UseChart = true;
            //            }
            //            else
            //            {
            //                dg_MonitoredTowers.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
            //                pl.UseChart = false;
            //            }
            //            break;
            //        }
            //    }
            //    POSList.SaveDesignListing();
            //}
        }

        private void CheckAndSendNotificationIfActive()
        {
            decimal ntfyHours, freqHrs;
            decimal ReactTime = 0;
            ArrayList ReactRet;
            DateTime cts;
            TimeSpan diff;

            if (mailSendErr)
                return;

            cts = DateTime.Now;

            foreach (POS p in POSList.Designs)
            {
                foreach (PosNotify pn in NL.NotifyList)
                {
                    if (mailSendErr)
                        return;

                    if (p.Name == pn.Tower)
                    {
                        if (pn.Frequency == "Days")
                        {
                            freqHrs = pn.FreqQty * 24;
                        }
                        else
                        {
                            freqHrs = pn.FreqQty;
                        }
                        if (pn.Initial == "Days")
                        {
                            ntfyHours = pn.InitQty * 24;
                        }
                        else
                        {
                            ntfyHours = pn.InitQty;
                        }

                        if (pn.Type == "Fuel Level")
                        {
                            if (pn.Notify_Active)
                            {
                                if (ntfyHours > p.PosTower.F_RunTime)
                                {
                                    diff = cts.Subtract(pn.Notify_Sent);
                                    if (diff.Hours >= freqHrs)
                                    {
                                        if(SendNotification(pn, p))
                                            pn.Notify_Sent = DateTime.Now;
                                    }
                                }
                                else
                                {
                                    pn.Notify_Active = false;
                                }
                            }
                            else
                            {
                                // Check to see if tower fuel run time is less than notification time
                                if (ntfyHours >= p.PosTower.F_RunTime)
                                {
                                    pn.Notify_Active = true;
                                    if (SendNotification(pn, p))
                                        pn.Notify_Sent = DateTime.Now;
                                }
                                else
                                {
                                    pn.Notify_Active = false;
                                }
                            }
                        }
                        else if (pn.Type == "Silo Level")
                        {
                            ReactRet = GetLongestSiloRunTime(p);
                            ReactTime = (decimal)ReactRet[0];

                            if (pn.Notify_Active)
                            {
                                if (ntfyHours > ReactTime)
                                {
                                    diff = cts.Subtract(pn.Notify_Sent);
                                    if (diff.Hours >= freqHrs)
                                    {
                                        if (SendNotification(pn, p))
                                            pn.Notify_Sent = DateTime.Now;
                                    }
                                }
                                else
                                {
                                    pn.Notify_Active = false;
                                }
                            }
                            else
                            {
                                // Check to see if tower fuel run time is less than notification time
                                if (ntfyHours >= ReactTime)
                                {
                                    pn.Notify_Active = true;
                                    if (SendNotification(pn, p))
                                        pn.Notify_Sent = DateTime.Now;
                                }
                                else
                                {
                                    pn.Notify_Active = false;
                                }
                            }
                        }
                    }
                }
            }
        }

#endregion


#region Button and Menu Controls

        private void tsb_ExportPOS_Click(object sender, EventArgs e)
        {
            string TowerExport, paddStr;
            string[,] fVal;
            decimal totalCost = 0, totalVol = 0, anchorTime = 0, onlineTime = 0;

            TowerExport = Design.Name + " < " + Design.System + " >[" + Design.SovLevel + "]{" + Design.Moon + "}\n";
            TowerExport += "--------------------------------------------------------\n";
            // Do Tower Itself and The Modules w/ Qty and Charges
            TowerExport += Design.PosTower.Name + "\n";
            totalVol += Design.PosTower.Volume;
            totalCost += Design.PosTower.Cost;
            anchorTime += Design.PosTower.Anchor_Time;
            onlineTime += Design.PosTower.Online_Time;

            foreach (Module m in Design.Modules)
            {
                TowerExport += m.Name + " [ " + m.Qty + " ]";
                if (m.Charge.Length > 0)
                    TowerExport += "< " + m.Charge + " >\n";
                else
                    TowerExport += "\n";

                totalVol += m.Volume;
                totalCost += m.Cost;
                anchorTime += m.Anchor_Time;
                onlineTime += m.Online_Time;
            }

            // Do Fuel For Design Period
            fVal = Design.PosTower.D_Fuel.GetFuelBayTotals();
            TowerExport += "--------------------------------------------------------\n";
            for (int x = 0; x < 13; x++)
            {
                if ((!Design.UseChart) && (x == 11))
                    continue;

                if (Convert.ToDecimal(fVal[x, 1]) > 0)
                {
                    paddStr = fVal[x, 0].PadRight(20, ' ').Substring(0,18) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1]));
                    paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])) + "m3 ]\n";
                    TowerExport += paddStr;
                    totalVol += Convert.ToDecimal(fVal[x, 2]);
                    totalCost += Convert.ToDecimal(fVal[x, 3]);
                }
            }

            // Do Total Volume, Cost, Anchor, and Online Time
            TowerExport += "--------------------------------------------------------\n";
            TowerExport += "Transport Volume: " + String.Format("{0:#,0.#}", totalVol) + " m3\n";
            TowerExport += "Total Cost       : " + String.Format("{0:#,0.#}", totalCost) + " isk\n";
            TowerExport += "Anchor Time     : " + ConvertSecondsToTextDisplay(anchorTime) + "\n";
            TowerExport += "Online Time      : " + ConvertSecondsToTextDisplay(onlineTime) + "\n";
            TowerExport += "Total Assembly   : " + ConvertSecondsToTextDisplay(anchorTime + onlineTime) + "\n";

            Clipboard.SetText(TowerExport);
        }

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
            DateTime cur;

            tsl_Status.Text = "Saving POS Listing";

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

            pli = new POS(Design);
            pli.Name = CurrentName;

            POSList.UpdateListDesign(pli);

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
            cb_PoSName.Text = pli.Name;

            cur = DateTime.Now;
            tsl_Status.Text = "POS Listing Saved (" + cur.ToString() + ")";
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

        private void Mouse_Online_Offline_Click(object sender, MouseEventArgs e)
        {
            bool online = false;

            api = (PoS_Item)sender;

            if (api.typeID == 0)
                return;

            if (api.TypeKey == EveHQ.PosManager.PoS_Item.TypeKeyEnum.Tower)
            {
                if (Design.PosTower.State == "Online")
                    Design.PosTower.State = "Offline";
                else
                {
                    online = true;
                    Design.PosTower.State = "Online";
                }
            }
            else
            {
                foreach (Module m in Design.Modules)
                {
                    if ((m.typeID == api.typeID) && (api.contName == m.Location))
                    {
                        if (m.State == "Online")
                            m.State = "Offline";
                        else
                        {
                            online = true;
                            m.State = "Online";
                        }
                    }
                }
            }

            if (online)
                api.onOff = Color.Green;
            else
                api.onOff = Color.Red;

            api.UpdateItemState();

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
                    POSList.UpdateListDesign(Design);
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

            POSList.RemoveDesignFromList(CurrentName);
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
            dg_MonitoredTowers.Rows.Clear();
            PopulateMonitoredPoSDisplay();
            PosChanged = false;
        }

        private void b_CopyPOS_Click(object sender, EventArgs e)
        {
            POS newCopy;
            POS_Name CpyPos = new POS_Name();
            CpyPos.myData = this;
            CpyPos.NewPOS = false;
            CpyPos.CopyPOS = true;
            CpyPos.ShowDialog();

            if (CurrentName.Equals(NewName))
                return;
            if (NewName.Length <= 0)
                return;

            newCopy = new POS(NewName, Design);
            newCopy.itemID = 0;
            newCopy.locID = 0;
            newCopy.ReactionLinks.Clear();
            foreach (Module m in newCopy.Modules)
                CopyMissingReactData(m);

            POSList.AddDesignToList(newCopy);
            CurrentName = NewName;
            NewName = "";
            this.Focus();
            POSList.SaveDesignListing();

            BuildPOSListForMonitoring();
            cb_PoSName.Items.Clear();
            foreach (POS pl in POSList.Designs)
            {
                cb_PoSName.Items.Add(pl.Name);
            }
            cb_PoSName.SelectedItem = CurrentName;
            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            dg_MonitoredTowers.Rows.Clear();
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
                    // Existing PoS being removed
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
            dg_MonitoredTowers.Rows.Clear();
            PopulateMonitoredPoSDisplay();
        }

        private void b_ModifyList_Click(object sender, EventArgs e)
        {
            MonitorListSelect mlist_sel = new MonitorListSelect();
            mlist_sel.myData = this;
            mlist_sel.ShowDialog();

            dg_MonitoredTowers.Rows.Clear();
            BuildPOSListForMonitoring();
            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            PopulateMonitoredPoSDisplay();
            POSList.SaveDesignListing();
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

                        // Oxygen
                        pl.PosTower.Fuel.Oxygen.Qty = pl.PosTower.A_Fuel.Oxygen.Qty;

                        // Mechanical Parts
                        pl.PosTower.Fuel.MechPart.Qty = pl.PosTower.A_Fuel.MechPart.Qty;

                        // Coolant
                        pl.PosTower.Fuel.Coolant.Qty = pl.PosTower.A_Fuel.Coolant.Qty;

                        // Robotics
                        pl.PosTower.Fuel.Robotics.Qty = pl.PosTower.A_Fuel.Robotics.Qty;

                        // Faction Charters
                        pl.PosTower.Fuel.Charters.Qty = pl.PosTower.A_Fuel.Charters.Qty;

                        // Strontium
                        pl.PosTower.Fuel.Strontium.Qty = pl.PosTower.A_Fuel.Strontium.Qty;

                        // Heavy Water
                        pl.PosTower.Fuel.HvyWater.Qty = pl.PosTower.A_Fuel.HvyWater.Qty;

                        // Liquid Ozone
                        pl.PosTower.Fuel.LiqOzone.Qty = pl.PosTower.A_Fuel.LiqOzone.Qty;

                        // Isotope
                        if (pl.PosTower.Fuel.HeIso.PeriodQty > 0)
                            pl.PosTower.Fuel.HeIso.Qty = pl.PosTower.A_Fuel.HeIso.Qty;
                        if (pl.PosTower.Fuel.H2Iso.PeriodQty > 0)
                            pl.PosTower.Fuel.H2Iso.Qty = pl.PosTower.A_Fuel.H2Iso.Qty;
                        if (pl.PosTower.Fuel.N2Iso.PeriodQty > 0)
                            pl.PosTower.Fuel.N2Iso.Qty = pl.PosTower.A_Fuel.N2Iso.Qty;
                        if (pl.PosTower.Fuel.O2Iso.PeriodQty > 0)
                            pl.PosTower.Fuel.O2Iso.Qty = pl.PosTower.A_Fuel.O2Iso.Qty;
                        
                    }
                }
            }

            update = false;

            POSList.CalculatePOSFuelRunTimes(API_D, Config.data.FuelCosts);
            POSList.SaveDesignListing();
            BuildPOSListForMonitoring();
            PopulateMonitoredPoSDisplay();
            UpdateTowerMonitorDisplay();
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

        private void tsb_SetOwner_Click(object sender, EventArgs e)
        {
            // Setting of the Tower Owner and Fuel Tech
            // Need to have Entry / Selection for:
            //  Owner (corp / personal) type
            //  Owner Name (corp name or person)
            //  Fuel Tech Name
            //  Player ID's for players if known, Corp ID's if corp
            OwnerFuelTech ownFT;

            ownFT = new OwnerFuelTech(this);
            ownFT.ShowDialog();
            POSList.SaveDesignListing();
            PopulateMonitoredPoSDisplay();
        }


#endregion


#region POS Maintenance

        private void cb_ShowFuelNeed_CheckedChanged(object sender, EventArgs e)
        {
            PopulateTowerFillDG();
            UpdateSelectedTowerList();
        }

        private void b_CopySelected_Click(object sender, EventArgs e)
        {
            string maintName = "", paddStr;
            int dgi, count = 0;
            FuelBay sfb = new FuelBay();
            decimal totVol, totCost;
            decimal[,] tval = new decimal[13, 3];
            string[,] fVal;

            for (int x = 0; x < 13; x++)
            {
                for (int y = 0; y < 3; y++)
                    tval[x, y] = 0;
            }
            SelPosFillText = "";
            dgi = cb_SelectedDataForCopy.SelectedIndex;

            foreach (DataGridViewRow dr in dg_TowerFuelList.SelectedRows)
            {
                if (dr.Cells[0].Value == null)
                    continue;

                maintName = dr.Cells[(int)fillDG.Name].Value.ToString();
                SelPosFillText += maintName + " < " + dr.Cells[(int)fillDG.Loc].Value.ToString() + " >\n";
                foreach (POS pl in POSList.Designs)
                {
                    if (maintName == pl.Name)
                    {
                        totVol = 0;
                        totCost = 0;

                        if (count < 1)
                            sfb = new FuelBay(pl.PosTower.T_Fuel);
                        else
                            sfb.AddFuelQty(pl.PosTower.T_Fuel);

                        pl.PosTower.T_Fuel.SetCurrentFuelVolumes();
                        pl.PosTower.T_Fuel.SetCurrentFuelCosts(Config.data.FuelCosts);
                        fVal = pl.PosTower.T_Fuel.GetFuelBayTotals();

                        for (int x = 0; x < 13; x++)
                        {
                            if ((!Config.data.maintChart) && (x == 11))
                                continue;
                            if ((!Config.data.maintStront) && (x == 12))
                                continue;

                            if (Convert.ToDecimal(fVal[x, 1]) > 0)
                            {
                                switch (dgi)
                                {
                                    case 0:
                                        paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0,10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1])) + " ]\n";
                                        totVol += Convert.ToDecimal(fVal[x, 2]);
                                        totCost += Convert.ToDecimal(fVal[x, 3]);
                                        break;
                                    case 1:
                                        paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1]));
                                        paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])) + "m3 ]\n";
                                        totVol += Convert.ToDecimal(fVal[x, 2]);
                                        totCost += Convert.ToDecimal(fVal[x, 3]);
                                        break;
                                    case 2:
                                        paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1]));
                                        paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])) + "m3";
                                        paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 3])) + "isk ]\n";
                                        totVol += Convert.ToDecimal(fVal[x, 2]);
                                        totCost += Convert.ToDecimal(fVal[x, 3]);
                                        break;
                                    default:
                                        paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1])) + " ]\n";
                                        totVol += Convert.ToDecimal(fVal[x, 2]);
                                        totCost += Convert.ToDecimal(fVal[x, 3]);
                                        break;
                                }
                                tval[x, 0] += Convert.ToDecimal(fVal[x, 1]);
                                tval[x, 1] += Convert.ToDecimal(fVal[x, 2]);
                                tval[x, 2] += Convert.ToDecimal(fVal[x, 3]);
                                SelPosFillText += paddStr;
                            }
                        }
                        count++;
                        SelPosFillText += "Tport Vol  [ " + String.Format("{0:#,0.#}", totVol) + "m3 ]\n";
                        if (dgi == 2)
                            SelPosFillText += "Fuel Cost  [ " + String.Format("{0:#,0.#}", totCost) + "isk ]\n";

                        SelPosFillText += "----\n";
                        break;
                    }
                }
            }

            if (dg_TowerFuelList.SelectedRows.Count > 1)
            {
                sfb.SetCurrentFuelVolumes();
                sfb.SetCurrentFuelCosts(Config.data.FuelCosts);
                fVal = sfb.GetFuelBayTotals();
                totCost = 0;
                totVol = 0;

                SelPosFillText += " < Total for Selected Towers >\n";
                for (int x = 0; x < 13; x++)
                {
                    if ((!Config.data.maintChart) && (x == 11))
                        continue;
                    if ((!Config.data.maintStront) && (x == 12))
                        continue;

                    if (Convert.ToDecimal(fVal[x, 1]) > 0)
                    {
                        switch (dgi)
                        {
                            case 0:
                                paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 0])) + " ]\n";
                                totVol += Convert.ToDecimal(tval[x, 1]);
                                totCost += Convert.ToDecimal(tval[x, 2]);
                                break;
                            case 1:
                                paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 0]));
                                paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 1])) + "m3 ]\n";
                                totVol += Convert.ToDecimal(tval[x, 1]);
                                totCost += Convert.ToDecimal(tval[x, 2]);
                                break;
                            case 2:
                                paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 0]));
                                paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 1])) + "m3";
                                paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 2])) + "isk ]\n";
                                totVol += Convert.ToDecimal(tval[x, 1]);
                                totCost += Convert.ToDecimal(tval[x, 2]);
                                break;
                            default:
                                paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 0])) + " ]\n";
                                totVol += Convert.ToDecimal(tval[x, 1]);
                                totCost += Convert.ToDecimal(tval[x, 2]);
                                break;
                        }
                        SelPosFillText += paddStr;
                    }
                }
                SelPosFillText += "Tport Vol  [ " + String.Format("{0:#,0.#}", totVol) + "m3 ]\n";
                if (dgi == 2)
                    SelPosFillText += "Fuel Cost  [ " + String.Format("{0:#,0.#}", totCost) + "isk ]\n";
            }
            Clipboard.SetText(SelPosFillText);
        }

        private void b_CopyAllPos_Click(object sender, EventArgs e)
        {
            string maintName = "", paddStr;
            int dgi, count = 0;
            FuelBay sfb = new FuelBay();
            decimal totVol, totCost;
            decimal[,] tval = new decimal[13, 3];
            string[,] fVal;

            for (int x = 0; x < 13; x++)
            {
                for (int y = 0; y < 3; y++)
                    tval[x, y] = 0;
            }
            AllPosFillText = "";
            dgi = cb_AllDataForCopy.SelectedIndex;

            foreach (DataGridViewRow dr in dg_TowerFuelList.Rows)
            {
                if (dr.Cells[0].Value == null)
                    continue;

                maintName = dr.Cells[(int)fillDG.Name].Value.ToString();
                AllPosFillText += maintName + " < " + dr.Cells[(int)fillDG.Loc].Value.ToString() + " >\n";
                foreach (POS pl in POSList.Designs)
                {
                    if (maintName == pl.Name)
                    {
                        totVol = 0;
                        totCost = 0;

                        if (count < 1)
                            sfb = new FuelBay(pl.PosTower.T_Fuel);
                        else
                            sfb.AddFuelQty(pl.PosTower.T_Fuel);

                        pl.PosTower.T_Fuel.SetCurrentFuelVolumes();
                        pl.PosTower.T_Fuel.SetCurrentFuelCosts(Config.data.FuelCosts);
                        fVal = pl.PosTower.T_Fuel.GetFuelBayTotals();

                        for (int x = 0; x < 13; x++)
                        {
                            if ((!Config.data.maintChart) && (x == 11))
                                continue;
                            if ((!Config.data.maintStront) && (x == 12))
                                continue;

                            if (Convert.ToDecimal(fVal[x, 1]) > 0)
                            {
                                switch (dgi)
                                {
                                    case 0:
                                        paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1])) + " ]\n";
                                        totVol += Convert.ToDecimal(fVal[x, 2]);
                                        totCost += Convert.ToDecimal(fVal[x, 3]);
                                        break;
                                    case 1:
                                        paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1]));
                                        paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])) + "m3 ]\n";
                                        totVol += Convert.ToDecimal(fVal[x, 2]);
                                        totCost += Convert.ToDecimal(fVal[x, 3]);
                                        break;
                                    case 2:
                                        paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1]));
                                        paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])) + "m3";
                                        paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 3])) + "isk ]\n";
                                        totVol += Convert.ToDecimal(fVal[x, 2]);
                                        totCost += Convert.ToDecimal(fVal[x, 3]);
                                        break;
                                    default:
                                        paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1])) + " ]\n";
                                        totVol += Convert.ToDecimal(fVal[x, 2]);
                                        totCost += Convert.ToDecimal(fVal[x, 3]);
                                        break;
                                }
                                tval[x, 0] += Convert.ToDecimal(fVal[x, 1]);
                                tval[x, 1] += Convert.ToDecimal(fVal[x, 2]);
                                tval[x, 2] += Convert.ToDecimal(fVal[x, 3]);
                                AllPosFillText += paddStr;
                            }
                        }
                        count++;
                        AllPosFillText += "Tport Vol  [ " + String.Format("{0:#,0.#}", totVol) + "m3 ]\n";
                        if (dgi == 2)
                            AllPosFillText += "Fuel Cost  [ " + String.Format("{0:#,0.#}", totCost) + "isk ]\n";

                        AllPosFillText += "-------------------------------------------------\n";
                        break;
                    }
                }
            }

            sfb.SetCurrentFuelVolumes();
            sfb.SetCurrentFuelCosts(Config.data.FuelCosts);
            fVal = sfb.GetFuelBayTotals();
            totCost = 0;
            totVol = 0;

            AllPosFillText += " < Total for All Towers >\n";
            for (int x = 0; x < 13; x++)
            {
                if ((!Config.data.maintChart) && (x == 11))
                    continue;
                if ((!Config.data.maintStront) && (x == 12))
                    continue;

                if (Convert.ToDecimal(fVal[x, 1]) > 0)
                {
                    switch (dgi)
                    {
                        case 0:
                            paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 0])) + " ]\n";
                            totVol += Convert.ToDecimal(tval[x, 1]);
                            totCost += Convert.ToDecimal(tval[x, 2]);
                            break;
                        case 1:
                            paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 0]));
                            paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 1])) + "m3 ]\n";
                            totVol += Convert.ToDecimal(tval[x, 1]);
                            totCost += Convert.ToDecimal(tval[x, 2]);
                            break;
                        case 2:
                            paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 0]));
                            paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 1])) + "m3";
                            paddStr += " ][ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 2])) + "isk ]\n";
                            totVol += Convert.ToDecimal(tval[x, 1]);
                            totCost += Convert.ToDecimal(tval[x, 2]);
                            break;
                        default:
                            paddStr = fVal[x, 0].PadRight(12, ' ').Substring(0, 10) + " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(tval[x, 0])) + " ]\n";
                            totVol += Convert.ToDecimal(tval[x, 1]);
                            totCost += Convert.ToDecimal(tval[x, 2]);
                            break;
                    }
                    AllPosFillText += paddStr;
                }
            }
            AllPosFillText += "Tport Vol  [ " + String.Format("{0:#,0.#}", totVol) + "m3 ]\n";
            if (dgi == 2)
                AllPosFillText += "Fuel Cost  [ " + String.Format("{0:#,0.#}", totCost) + "isk ]\n";

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
            string maintName = "";
            int dgi, count = 0;
            FuelBay sfb = new FuelBay();
            decimal totVol, totCost;
            string[,] fVal;

            foreach (DataGridViewRow dr in dg_TowerFuelList.SelectedRows)
            {
                if (dr.Cells[0].Value == null)
                    continue;

                Fil_dg_indx = dg_TowerFuelList.CurrentRow.Index;
                maintName = dr.Cells[(int)fillDG.Name].Value.ToString();
                foreach (POS pl in POSList.Designs)
                {
                    if (maintName == pl.Name)
                    {
                        if (count < 1)
                            sfb = new FuelBay(pl.PosTower.T_Fuel);
                        else
                            sfb.AddFuelQty(pl.PosTower.T_Fuel);

                        count++;
                        break;
                    }
                }
            }

            dg_SelectedFuel.Rows.Clear();
            sfb.SetCurrentFuelVolumes();
            sfb.SetCurrentFuelCosts(Config.data.FuelCosts);
            fVal = sfb.GetFuelBayTotals();

            totCost = 0;
            totVol = 0;

            for (int x = 0; x < 13; x++)
            {
                if ((!Config.data.maintChart) && (x == 11))
                    continue;
                if ((!Config.data.maintStront) && (x == 12))
                    continue;

                dgi = dg_SelectedFuel.Rows.Add();
                dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.type].Value = fVal[x, 0];
                dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.amount].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1]));
                if (Convert.ToDecimal(fVal[x, 2]) > 0)
                    totVol += Convert.ToDecimal(fVal[x, 2]);
                dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.vol].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])) + " m3";
                totCost += Convert.ToDecimal(fVal[x, 3]);
                dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.cost].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 3])) + " isk";
            }
            dgi = dg_SelectedFuel.Rows.Add();
            dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.type].Value = "Total Volume & Cost";
            dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.vol].Value = String.Format("{0:#,0.#}", totVol) + " m3";
            dg_SelectedFuel.Rows[dgi].Cells[(int)fuelDG.cost].Value = String.Format("{0:#,0.#}", totCost) + " isk";
        }

        private void PopulateTotalsDG()
        {
            int dgi;
            decimal totVol, totCost;
            string[,] fVal;

            if (tt == null)
                return;

            tt.SetCurrentFuelVolumes();
            tt.SetCurrentFuelCosts(Config.data.FuelCosts);
            dg_TotalFuel.Rows.Clear();
            fVal = tt.GetFuelBayTotals();

            totVol = 0;
            totCost = 0;
            for (int x = 0; x < 13; x++)
            {
                if ((!Config.data.maintChart) && (x == 11))
                    continue;
                if ((!Config.data.maintStront) && (x == 12))
                    continue;

                dgi = dg_TotalFuel.Rows.Add();
                dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.type].Value = fVal[x, 0];
                dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.amount].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1]));
                if (Convert.ToDecimal(fVal[x, 2]) > 0)
                    totVol += Convert.ToDecimal(fVal[x, 2]);
                dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.vol].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])) + " m3";
                totCost += Convert.ToDecimal(fVal[x, 3]);
                dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.cost].Value = String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 3])) + " isk";
            }
            dgi = dg_TotalFuel.Rows.Add();
            dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.type].Value = "Total Volume & Cost";
            dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.vol].Value = String.Format("{0:#,0.#}", totVol) + " m3";
            dg_TotalFuel.Rows[dgi].Cells[(int)fuelDG.cost].Value = String.Format("{0:#,0.#}", totCost) + " isk";
        }

        private void PopulateTowerFillDG()
        {
            int dgi;
            string line;
            decimal period;

            tt = null;
            dg_TowerFuelList.Rows.Clear();
            update = true;
            cb_FactChartTotal.Checked = Config.data.maintChart;
            cb_UseStrontTotals.Checked = Config.data.maintStront;
            update = false;

            foreach (POS p in POSList.Designs)
            {
                if (!p.Monitored)
                    continue;

                if (tt == null)
                    tt = new FuelBay(p.PosTower.Fuel);

                if(!cb_ShowFuelNeed.Checked)
                    period = p.ComputePosFuelUsageForFillTracking((int)Config.data.maintTP, Config.data.maintPV, Config.data.FuelCosts);
                else
                    period = p.ComputePosFuelNeedForFillTracking((int)Config.data.maintTP, Config.data.maintPV, Config.data.FuelCosts);

                dgi = dg_TowerFuelList.Rows.Add();

                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Name].Value = p.Name;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Loc].Value = p.Moon;

                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.EnrUr].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.EnrUran.Qty);
                tt.EnrUran.Qty += p.PosTower.T_Fuel.EnrUran.Qty;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Oxy].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.Oxygen.Qty);
                tt.Oxygen.Qty += p.PosTower.T_Fuel.Oxygen.Qty;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.McP].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.MechPart.Qty);
                tt.MechPart.Qty += p.PosTower.T_Fuel.MechPart.Qty;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Cool].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.Coolant.Qty);
                tt.Coolant.Qty += p.PosTower.T_Fuel.Coolant.Qty;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Rbt].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.Robotics.Qty);
                tt.Robotics.Qty += p.PosTower.T_Fuel.Robotics.Qty;

                if (p.PosTower.T_Fuel.N2Iso.Name != "")
                {
                    dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.N2Iso.Qty) + " N2";
                    tt.N2Iso.Qty += p.PosTower.T_Fuel.N2Iso.Qty;
                }
                else if (p.PosTower.T_Fuel.H2Iso.Name != "")
                {
                    dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.H2Iso.Qty) + " H2";
                    tt.H2Iso.Qty += p.PosTower.T_Fuel.H2Iso.Qty;
                }
                else if (p.PosTower.T_Fuel.O2Iso.Name != "")
                {
                    dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.O2Iso.Qty) + " O2";
                    tt.O2Iso.Qty += p.PosTower.T_Fuel.O2Iso.Qty;
                }
                else if (p.PosTower.T_Fuel.HeIso.Name != "")
                {
                    dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Iso].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.HeIso.Qty) + " He";
                    tt.HeIso.Qty += p.PosTower.T_Fuel.HeIso.Qty;
                }
                else
                {
                    dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Iso].Value = "No F'in Clue";
                }
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.HvW].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.HvyWater.Qty);
                tt.HvyWater.Qty += p.PosTower.T_Fuel.HvyWater.Qty;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.LqO].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.LiqOzone.Qty);
                tt.LiqOzone.Qty += p.PosTower.T_Fuel.LiqOzone.Qty;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Cht].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.Charters.Qty);
                tt.Charters.Qty += p.PosTower.T_Fuel.Charters.Qty;
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.Strt].Value = String.Format("{0:#,0.#}", p.PosTower.T_Fuel.Strontium.Qty);
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

                line = ConvertHoursToTextDisplay(period);
                dg_TowerFuelList.Rows[dgi].Cells[(int)fillDG.RunT].Value = line;
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

        private void b_SetSelectedFull_Click(object sender, EventArgs e)
        {
            string maintName;

            if (dg_TowerFuelList.SelectedRows.Count > 1)
            {
                // Post a Pop-Up error and exit
                MessageBox.Show("Sorry - You can Only have ONE tower selected to use this feature.", "Set Selected Tower Full", MessageBoxButtons.OK);
                return;
            }

            if (dg_TowerFuelList.CurrentRow.Cells[(int)fillDG.Name].Value != null)
            {
                maintName = dg_TowerFuelList.CurrentRow.Cells[(int)fillDG.Name].Value.ToString();
                foreach (POS pl in POSList.Designs)
                {
                    if (maintName == pl.Name)
                    {
                        pl.PosTower.Fuel.AddFuelQty(pl.PosTower.T_Fuel);
                        break;
                    }
                }
            }
            PopulateTowerFillDG();
            dg_TowerFuelList_SelectionChanged(sender, e);
            PopulateTotalsDG();
            UpdateSelectedTowerList();
            PopulateMonitoredPoSDisplay();
        }


#endregion


#region Fuel Amount Change Control

        private void nud_DesFuelPeriod_ValueChanged(object sender, EventArgs e)
        {
            // If loading a POS and updating fields, do not pay attention.
            if (load)
                return;

            if (Design.PosTower.typeID == 0)
                return;

            Design.PosTower.Design_Int_Qty = nud_DesFuelPeriod.Value;
            CalculateAndDisplayDesignFuelData();
            nud_DesFuelPeriod.Value = Design.PosTower.Design_Int_Qty;
        }

        private void cb_Interval_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If loading a POS and updating fields, do not pay attention.
            if (load)
                return;

            if (Design.PosTower.typeID == 0)
                return;

            Design.PosTower.Design_Interval = cb_Interval.SelectedIndex;
            CalculateAndDisplayDesignFuelData();
            nud_DesFuelPeriod.Value = Design.PosTower.Design_Int_Qty;
        }

        private void nud_StrontInterval_ValueChanged(object sender, EventArgs e)
        {
            // If loading a POS and updating fields, do not pay attention.
            if (load)
                return;

            if (Design.PosTower.typeID == 0)
                return;

            Design.PosTower.Design_Stront_Qty = nud_StrontInterval.Value;
            CalculateAndDisplayDesignFuelData();
        }

        private void MonitoredFuelChange(object sender, EventArgs e)
        {
            if ((!update) && (UpdateTower))
                UpdateTowerMonitorDisplay();
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
            RunCalculationsWithUpdatedInformation();
        }

        private void b_TowerAPILink_Click(object sender, EventArgs e)
        {
            Tower_API_Linker TowerLink = new Tower_API_Linker();
            TowerLink.myData = this;
            TowerLink.ShowDialog();
            API_D.SaveAPIListing();
            BuildPOSListForMonitoring();
            RunCalculationsWithUpdatedInformation();
        }

        private void b_UpdateAPIData_Click(object sender, EventArgs e)
        {
            tsl_APIState.Text = "Updating API Data and Fuel Calculations";
            bgw_APIUpdate.RunWorkerAsync();
        }

        private void bgw_APIUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SystemSovList SSL = new SystemSovList();

            SSL.LoadSovListFromAPI(1);
            SL.LoadSystemListFromDisk();
            AL.LoadAllianceListFromAPI(1);
            API_D.LoadAPIListing();
            BuildPOSListForMonitoring();
            UpdateAllTowerSovLevels();
            GetTowerItemListData();
            RunCalculationsWithUpdatedInformation();
            POSList.SaveDesignListing();
            tsl_APIState.Text = "";
        }

        private void bgw_APIUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            GetCorpAssets();
            GetCorpSheet();
            GetAllySovLists();
            API_D.LoadPOSDataFromAPI(TL);
            //API_D.LoadCorpTowerDataFromAPI(TL);
            API_D.SaveAPIListing();
            PopulateCorpList();
        }

        private void GetAllySovLists()
        {
            XmlDocument apiXML;
            SystemSovList SSL = new SystemSovList();

            AL = new AllianceList();

            apiXML = EveHQ.Core.EveAPI.GetAPIXML((int)EveHQ.Core.EveAPI.APIRequest.AllianceList, 0);
            apiXML = EveHQ.Core.EveAPI.GetAPIXML((int)EveHQ.Core.EveAPI.APIRequest.Sovereignty, 0);
            AL.LoadAllianceListFromAPI(1);
            SSL.LoadSovListFromAPI(1);

        }

        private void GetCorpAssets()
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
        }

        private void GetCorpSheet()
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

        private void DGColumnCBStateChange(object sender, EventArgs e)
        {
            int colCt = 1;
            DataGridViewColumn dgvc;
            CheckBox cb;
            string cName;

            // Configuration Change for Combo-Box
            cb = (CheckBox)sender;

            cName = cb.Name;
            colCt = Convert.ToInt32(cName.Replace("checkBox", ""));

            if (colCt < 23)
            {
                dgvc = dg_MonitoredTowers.Columns[colCt - 1];
                dgvc.Visible = cb.Checked;
                Config.data.dgMonBool[colCt-1] = cb.Checked;
            }
            else
            {
                colCt -= 24;
                if (colCt < 15)
                {
                    dgvc = dg_PosMods.Columns[colCt - 1];
                    dgvc.Visible = cb.Checked;
                    Config.data.dgDesBool[colCt-1] = cb.Checked;
                }
            }

            SaveConfiguration();
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

        private void SaveConfiguration()
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

            SortMonitorDataGridByColumn(dgv, Config.data.SortedColumnIndex);

            Config.SaveConfiguration();
        }
        

#endregion


#region Set Module Charge - Handling Routines
 
        private void tsm_SetModuleCharge_Click(object sender, EventArgs e)
        {

        }


#endregion


#region Reaction Monitor

        public void SetModuleWarnOnValueAndTime()
        {
            bool fInp, fOutp;
            decimal xfIn, xfOut;

            foreach (POS p in POSList.Designs)
            {
                foreach (Module m in p.Modules)
                {
                    switch (Convert.ToInt64(m.ModType))
                    {
                        case 2:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                            // Silo type module here
                            fInp = false;
                            fOutp = false;
                            xfIn = 0;
                            xfOut = 0;
                            foreach (ReactionLink rl in p.ReactionLinks)
                            {
                                if (rl.InpID == m.ModuleID)
                                {
                                    fInp = true;
                                    xfIn = rl.XferQty;
                                }

                                if (rl.OutID == m.ModuleID)
                                {
                                    fOutp = true;
                                    xfOut = rl.XferQty;
                                }
                            }

                            if ((fInp) && (!fOutp))
                            {
                                // Silo is an input to another module only - WarnOn Empty (2)
                                m.WarnOn = 2;
                                m.FillEmptyTime = Math.Floor(m.CapQty / xfIn);
                            }
                            else if ((!fInp) && (fOutp))
                            {
                                // Silo is an output from another module only - WarnOn Full (1)
                                m.WarnOn = 1;
                                m.FillEmptyTime = Math.Floor((m.MaxQty - m.CapQty) / xfOut);
                            }
                            else if (fInp && fOutp)
                            {
                                if (xfIn >= xfOut)
                                {
                                    // Silo is both an input and an output - WarnOn Full (1)
                                    m.WarnOn = 1;
                                    if (xfIn > xfOut)
                                        m.FillEmptyTime = Math.Floor((m.MaxQty - m.CapQty) / (xfIn - xfOut));
                                    else
                                        m.FillEmptyTime = 9999999999999;
                                }
                                else if (xfOut > xfIn)
                                {
                                    // Silo is both an input and an output - WarnOn Empty (2)
                                    m.WarnOn = 2;
                                    m.FillEmptyTime = Math.Floor(m.CapQty / (xfOut - xfIn));
                                }
                            }
                            else
                            {
                                // Silo is not connected to anything - disable
                                m.WarnOn = 0;
                                m.FillEmptyTime = 0;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void SetReactionValuesAndUpdateDisplay()
        {
            // OK, new fill numbers, etc have been set for towers. Now I need to update the display
            // accordingly
            TowerReactMod tr;
            TreeNode pos;
            decimal sMult;
            string posName;

            SetModuleWarnOnValueAndTime();

            pos = tv_ReactManage.SelectedNode;
            if (pos == null)
                return;

            if (pos.Parent != null)
                pos = pos.Parent;

            // Now - Get / find this POS in the list of towers
            foreach (POS pl in POSList.Designs)
            {
                posName = pl.Name + "  < " + pl.Moon + " >";
                if (posName == pos.Text)
                {
                    // Found Selected Tower - Update all Modules
                    sMult = 1 + (decimal)(pl.PosTower.Bonuses.SiloCap / 100);

                    foreach (Control c in p_PosMods.Controls)
                    {
                        tr = (TowerReactMod)c;

                        foreach (Module m in pl.Modules)
                        {
                            if (m.ModuleID == tr.ReactMod.ModuleID)
                            {
                                tr.SetModuleData(m, sMult);
                                break;
                            }
                        }
                    }
                }
            }
        }
         
        public void PopulateTowerModuleDisplay()
        {
            // Need to populate the tree view with Monitored Towers and Thier applicable modules
            // IE: Silos, Juntions, Reactors, Harvesters
            TreeNode mtn, tn;
            string posName, line;
            bool hasMods = false;
            int towerNum = 1;

            tv_ReactManage.Nodes.Clear();

            foreach (POS pl in POSList.Designs)
            {
                if (!pl.Monitored)
                    continue;

                posName = pl.Name;

                posName += "  < " + pl.Moon + " >";

                mtn = tv_ReactManage.Nodes.Add(posName);
                mtn.Name = "Tower" + towerNum;
                towerNum++;
                hasMods = false;

                foreach (Module m in pl.Modules)
                {
                    if ((m.Category == "Mobile Reactor") || (m.Category == "Moon Mining") ||
                        (m.Category == "Silo"))
                    {
                        if (m.Category == "Silo")
                            line = m.Name + " < " + m.CapQty + " / " + m.MaxQty + ">";
                        else
                            line = m.Name;

                        tn = mtn.Nodes.Add(line);
                        hasMods = true;
                    }
                }

                if (!hasMods)
                    tv_ReactManage.Nodes.Remove(mtn);
            }

        }

        private void tv_ReactManage_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Populate Module list on RH side with correct controls, etc... for this POS
            TreeNode pos;

            pos = tv_ReactManage.SelectedNode;

            if (pos.Parent != null)
                pos = pos.Parent;

            SelReactPos = pos.Text;

            POSList.SaveDesignListing();
            DisplayReactionModules();
        }

        private void DisplayReactionModules()
        {
            TowerReactMod trm;
            int num = 0;
            decimal sMult;
            string posName;

            // Clear current module display
            p_PosMods.Controls.Clear();
            p_PosMods.Refresh();
            SetModuleWarnOnValueAndTime();

            // Now - Get / find this POS in the list of towers
            foreach (POS pl in POSList.Designs)
            {
                posName = pl.Name + "  < " + pl.Moon + " >";

                if (SelReactPos == posName)
                {
                    SetReactionModuleID(pl);

                    foreach (Module m in pl.Modules)
                    {
                        if (((m.Category == "Mobile Reactor") || (m.Category == "Moon Mining") ||
                            (m.Category == "Silo")) && (m.State == "Online"))
                        {
                            trm = new TowerReactMod(m.Name);
                            trm.myData = this;
                            trm.Location = new Point(0, (num * 86));
                            p_PosMods.Controls.Add(trm);
                            sMult = 1 + (decimal)(pl.PosTower.Bonuses.SiloCap / 100);
                            trm.SetModuleData(m, sMult);

                            num++;
                        }
                    }
                    SetActiveLinkColorBG(pl);
                }
            }
            POSList.SaveDesignListing();
        }

        private void SetActiveLinkColorBG(POS pl)
        {
            TowerReactMod tr;
            int lnkCnt = 0;

            // Now populate links
            foreach (ReactionLink rl in pl.ReactionLinks)
            {
                foreach (Control c in p_PosMods.Controls)
                {
                    tr = (TowerReactMod)c;

                    if (tr.ReactMod.ModuleID == rl.InpID)
                        tr.SetLinkColor(LColor[lnkCnt], rl.srcNm);
                    if (tr.ReactMod.ModuleID == rl.OutID)
                        tr.SetLinkColor(LColor[lnkCnt], rl.dstNm);
                }
                lnkCnt++;
            }
        }

        public void CopyMissingReactData(Module m)
        {
            foreach (Module bm in ML.Modules)
            {
                if (bm.typeID == m.typeID)
                {
                    m.ReactList = new ArrayList(bm.ReactList);
                    m.MSRList = new ArrayList(bm.MSRList);
                    m.InputList = new ArrayList(bm.InputList);
                    m.OutputList = new ArrayList(bm.OutputList);
                    m.selReact = new Reaction();
                    m.selMineral = new MoonSiloReactMineral();
                    m.ModuleID = 0;
                    m.ModType = 0;
                    m.CapVol = 0;
                    m.CapQty = 0;
                    m.MaxQty = 0;
                    m.FillEmptyTime = 0;
                    m.WarnOn = 0;
                    break;
                }
            }
        }

        public void SetReactionModuleID(POS p)
        {
            decimal cnt = 0;
            SortedList SLs = new SortedList();
            // Need to give each and every tower module a different unique ID number as it comes in.
            //      Note: ID only needs to be Unique per the tower, not overall
            // BUT - only want to do this if the module does not already have such an ID - do not want
            // to change or modify an existing ID

            // 1. Scan for existing ID's - Add to list
            foreach (Module m in p.Modules)
            {
                if ((m.Category == "Mobile Reactor") || (m.Category == "Moon Mining") ||
                    (m.Category == "Silo"))
                {
                    // I only care about Reaction Modules with IDs
                    if (m.ModuleID != 0)
                    {
                        SLs.Add(m.ModuleID, m.Name);
                    }

                    if (m.Category == "Mobile Reactor")
                    {
                        if (m.Name.Contains("Complex"))
                            m.ModType = 4;
                        else if (m.Name.Contains("Medium Biochemical"))
                            m.ModType = 5;
                        else if (m.Name.Contains("Biochemical"))
                            m.ModType = 6;
                        else if (m.Name.Contains("Polymer"))
                            m.ModType = 7;
                        else
                            m.ModType = 3;  // Simple
                    }
                    else if (m.Category == "Moon Mining")
                    {
                        m.ModType = 1;
                    }
                    else if (m.Category == "Silo")
                    {
                        if (m.Name == "Silo")
                            m.ModType = 2;
                        else if (m.Name.Contains("Biochemical"))
                            m.ModType = 8;
                        else if (m.Name.Contains("Catalyst"))
                            m.ModType = 9;
                        else if (m.Name.Contains("General Storage"))
                            m.ModType = 11;
                        else if (m.Name.Contains("Hazardous Chemical"))
                            m.ModType = 12;
                        else if (m.Name.Contains("Hybrid Polymer"))
                            m.ModType = 13;
                        else
                            m.ModType = 10; // Junction
                    }
                }
            }

            // 2. Scan for un-assigned Modules : ID = 0
            // 3. Assign Unique ID to modules with existing ID of 0
            foreach (Module m in p.Modules)
            {
                cnt = 0;
                if ((m.Category == "Mobile Reactor") || (m.Category == "Moon Mining") ||
                    (m.Category == "Silo"))
                {
                    // I only care about Reaction Modules w/o IDs
                    if (m.ModuleID == 0)
                    {
                        cnt++;
                        // Find an unused Key value
                        while (SLs.ContainsKey(cnt))
                            cnt++;

                        m.ModuleID = cnt;
                        SLs.Add(cnt, m.Name);
                    }
                }
            }
        }

        public void TowerReactModuleUpdated(Module m, int cTyp, int linkID, string pbNm)
        {
            string posName;

            // Get corret POS Object
            foreach (POS pl in POSList.Designs)
            {
                posName = pl.Name + "  < " + pl.Moon + " >";
                if (posName == SelReactPos)
                {
                    // A module reaction, or Link has been set
                    switch(cTyp)
                    {
                        case 1:
                        case 11:
                            // Mineral has been set
                            foreach (Module pm in pl.Modules)
                            {
                                if (pm.ModuleID == m.ModuleID)
                                {
                                    pm.selMineral = new MoonSiloReactMineral(m.selMineral);
                                    pm.MaxQty = m.MaxQty;
                                    if (cTyp == 11)
                                        ClearReactionLink(pm);
                                    break;
                                }
                            }
                            break;
                        case 2:
                        case 12:
                            // Reaction has been set
                            foreach (Module pm in pl.Modules)
                            {
                                if (pm.ModuleID == m.ModuleID)
                                {
                                    pm.selReact = new Reaction(m.selReact);
                                    if (cTyp == 12)
                                        ClearReactionLink(pm);
                                    break;
                                }
                            }
                            break;
                        case 3:
                            if (srcID != 0)
                            {
                                // selection of 2nd input has taken place.
                                ClearSelectData();
                            }
                            // Source has been selected
                            srcID = m.ModuleID;
                            srcNm = pbNm;

                            if (dstID != 0)
                                CheckAndSetModuleLinkIfCompatible();
                            break;
                        case 4:
                            if (dstID != 0)
                            {
                                // selection of 2nd output has taken place.
                                ClearSelectData();
                            }
                            // Destination has been selected
                            dstID = m.ModuleID;
                            dstNm = pbNm;

                            if (srcID != 0)
                                CheckAndSetModuleLinkIfCompatible();
                            break;
                        case 5:
                            // We Have a Silo Fill Level Change
                            foreach (Module pm in pl.Modules)
                            {
                                if (pm.ModuleID == m.ModuleID)
                                {
                                    pm.CapQty = m.CapQty;
                                    pm.CapVol = m.CapVol;
                                }
                            }
                            // Values updated, change tower timestamp
                            pl.React_TS = DateTime.Now;
                            break;
                        case 6:
                            // User wants to clear this reaction link
                            foreach (ReactionLink rl in pl.ReactionLinks)
                            {
                                if (((rl.InpID == m.ModuleID) && (rl.srcNm == pbNm)) || ((rl.OutID == m.ModuleID) && (rl.dstNm == pbNm)))
                                {
                                    pl.ReactionLinks.Remove(rl);
                                    DisplayReactionModules();
                                    break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public bool CheckAndSetModuleLinkIfCompatible()
        {
            Module src = new Module();
            Module dst = new Module();
            int numFound = 0;
            int srcNum = 0, dstNum = 0;
            InOutData dio, sio;
            string posName;

            if (srcNm.Length > 0)
            {
                srcNum = Convert.ToInt32(srcNm.Substring(6,1)) - 1;
            }

            if (dstNm.Length > 0)
            {
                dstNum = Convert.ToInt32(dstNm.Substring(5,1)) - 1;
            }

            // Reaction has been set
            foreach (POS pl in POSList.Designs)
            {
                posName = pl.Name + "  < " + pl.Moon + " >";
                if (posName == SelReactPos)
                {
                    if (pl.ReactionLinks.Count >= MaxLink)
                    {
                        MessageBox.Show("Sorry, Maximum [20] Links has been Reached. Pester Sherk to allow more!");
                        ClearSelectData();
                        return false;
                    }
                    // Found tower - now find modules
                    foreach (Module pm in pl.Modules)
                    {
                        if (pm.ModuleID == srcID)
                        {
                            src = new Module(pm);
                            numFound++;
                        }
                        if (pm.ModuleID == dstID)
                        {
                            dst = new Module(pm);
                            numFound++;
                        }

                        if (numFound == 2)
                            break;
                    }

                    if (src.ModuleID == dst.ModuleID)
                    {
                        MessageBox.Show("Invalid Link - Source and Destination of Link cannot be the Same Module.");
                        ClearSelectData();
                        return false;
                    }

                    // Verify location is not already linked
                    foreach (ReactionLink rl in pl.ReactionLinks)
                    {
                        if (((rl.srcNm == srcNm) && (rl.InpID == src.ModuleID)) || ((rl.dstNm == dstNm) && (rl.OutID == dst.ModuleID)))
                        {
                            MessageBox.Show("Invalid Link - Input or Output is Already Linked.");
                            ClearSelectData();
                            return false;
                        }
                    }

                    // src == Module providing link source material
                    // dst == Module providing link desctination for material
                    if (src.Category == "Mobile Reactor")
                    {
                        // Source is a Mobile Reactor, Dest Can be: A Reactor or A Silo/Junction
                        // Source Has different Outputs, make sure we check correct one
                        sio = new InOutData((InOutData)src.selReact.outputs[srcNum]);

                        if (dst.Category == "Silo")
                        {
                            if (sio.typeID == dst.selMineral.typeID)
                            { 
                                // Valid - complete the linkup
                                SetModuleReactionLink(pl);
                            }
                            else
                            {
                                MessageBox.Show("Invalid Link - Mineral Output Type and Target Module Type are not compatible.");
                                ClearSelectData();
                                return false;
                                // Not valid
                            }
                        }
                        else if (dst.Category == "Mobile Reactor")
                        {
                            dio = new InOutData((InOutData)dst.selReact.inputs[dstNum]);
                            if (dio.typeID == sio.typeID)
                            {
                                // Valid but not ideal - warn
                                // This is allowed, but more comes in than the reaction will use, so
                                // it will cost to do it - not ideal use of resources
                                DialogResult dr = MessageBox.Show("You will lose mineral / reaction production with this link, do you want to continue?", "Make Link", MessageBoxButtons.YesNo);

                                if (dr == DialogResult.Yes)
                                {
                                    // Well, stupid things do happen
                                    SetModuleReactionLink(pl);
                                }
                                else
                                {
                                    ClearSelectData();
                                    return false;
                                }
                            }
                            else
                            {
                                // Not valid
                                MessageBox.Show("Invalid Link - Mineral Output Type and Reaction Input Types are not compatible.");
                                ClearSelectData();
                                return false;
                            }
                        }
                        else
                        {
                            // Invalid fitting
                            MessageBox.Show("Invalid Link - Module Types are Not Compatible.");
                            ClearSelectData();
                            return false;
                        }
                    }
                    else if (src.Category == "Moon Mining")
                    {
                        // Source is a Moon miner, Dest can be: A Mobile Reactor or a Silo / Junction
                        // Only one possible output
                        if(dst.Category == "Silo")
                        {
                            if(src.selMineral.typeID == dst.selMineral.typeID)
                            {
                                // Valid Link
                                SetModuleReactionLink(pl);
                            }
                            else
                            {
                                // Not valid
                                MessageBox.Show("Invalid Link - Mineral Output Type and Silo Types are not compatible.");
                                ClearSelectData();
                                return false;
                            }
                        }
                        else if(dst.Category == "Mobile Reactor")
                        {
                            dio = new InOutData((InOutData)dst.selReact.inputs[dstNum]);
                            if (src.selMineral.typeID == dio.typeID)
                            {
                                // Valid Link
                                SetModuleReactionLink(pl);
                            }
                            else
                            {
                                // Not valid
                                MessageBox.Show("Invalid Link - Mineral Output Type and Reaction Input Types are not compatible.");
                                ClearSelectData();
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Link - Moon miner input is always the same as Selected Mineral. No link possible here.");
                            ClearSelectData();
                            return false;
                        }
                    }
                    else if (src.Category == "Silo")
                    {
                        // Input is a Silo type - ensure compatibility with output of the other module in the link
                        // Silo Outputs can be: Reaction, Silo / Junction
                        if (dst.Category == "Mobile Reactor")
                        {
                            dio = new InOutData((InOutData)dst.selReact.inputs[dstNum]);
                            if (src.selMineral.typeID == dio.typeID)
                            {
                                // Valid Link
                                SetModuleReactionLink(pl);
                            }
                            else
                            {
                                // Not valid
                                MessageBox.Show("Invalid Link - Mineral Output Type and Reaction Input Types are not compatible.");
                                ClearSelectData();
                                return false;
                            }
                        }
                        else if(dst.Category == "Silo")
                        {
                            // Silo or Moon Mining type - check mineral compabitility
                            if (src.selMineral.typeID == dst.selMineral.typeID)
                            {
                                // Valid
                                SetModuleReactionLink(pl);
                            }
                            else
                            {
                                // Not valid
                                MessageBox.Show("Invalid Link - Mineral Output Type and Mineral Input Types are not compatible.");
                                ClearSelectData();
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Link - Silo output cannot be used as input for this Module.");
                            ClearSelectData();
                            return false;
                        }
                    }
                    break;
                }
            }
            ClearSelectData();
            return false;
        }

        private decimal[] GetQtyVolForLink()
        {
            decimal mult, qty, vol, bPrice, rQty;
            decimal[] retV = new decimal[2];
            retV[0] = 0;
            retV[1] = 100;
            Module SrcMod = new Module();
            Module DstMod = new Module();
            string posName;

            foreach (POS pl in POSList.Designs)
            {
                posName = pl.Name + "  < " + pl.Moon + " >";
                if (posName == SelReactPos)
                {
                    foreach (Module m in pl.Modules)
                    {
                        if (m.ModuleID == srcID)
                        {
                            SrcMod = new Module(m);
                            break;
                        }
                    }
                    foreach (Module m in pl.Modules)
                    {
                        if (m.ModuleID == dstID)
                        {
                            DstMod = new Module(m);
                            break;
                        }
                    }
                    break;
                }
            }

            if ((SrcMod.Category == "Silo") || (SrcMod.Category == "Moon Mining"))
            {
                // Plain mineral incomming
                qty = SrcMod.selMineral.reactQty;
                mult = SrcMod.selMineral.portionSize;
                vol = SrcMod.selMineral.volume;
                bPrice = SrcMod.selMineral.basePrice;

                retV[0] = qty;
                retV[1] = retV[0] * vol * mult;
            }
            else if (SrcMod.Category == "Mobile Reactor")
            {
                // Reaction mineral incomming. Use IO mod Qty as multiplier
                foreach (InOutData iod in SrcMod.selReact.outputs)
                {
                    if (iod.typeID == DstMod.selMineral.typeID)
                    {
                        qty = iod.qty;
                        mult = DstMod.selMineral.portionSize;
                        vol = DstMod.selMineral.volume;
                        bPrice = DstMod.selMineral.basePrice;
                        rQty = DstMod.selMineral.reactQty;

                        retV[0] = qty * rQty;
                        retV[1] = retV[0] * vol * mult;
                    }
                }
            }
            else
            {
            }

            return retV;
        }

        private void SetModuleReactionLink(POS pl)
        {
            ReactionLink nrl;
            decimal[] qv;

            qv = GetQtyVolForLink();

            nrl = new ReactionLink(1, srcID, dstID, qv[0], qv[1], Color.Green, srcNm, dstNm);
            
            pl.ReactionLinks.Add(nrl);
            
            SetLinkColorBG(pl);

            ClearSelectData();
        }

        private void tsb_React_Save_Click(object sender, EventArgs e)
        {
            POSList.SaveDesignListing();
            SetReactionValuesAndUpdateDisplay();
            PopulateTowerModuleDisplay();
        }

        private void tsb_ReactClearLink_Click(object sender, EventArgs e)
        {
            string posName;

            // Get corret POS Object
            foreach (POS pl in POSList.Designs)
            {
                posName = pl.Name + "  < " + pl.Moon + " >";
                if (posName == SelReactPos)
                {
                    pl.ReactionLinks.Clear();
                    DisplayReactionModules();
                }
            }
        }

        private void ClearReactionLink(Module m)
        {
            ArrayList tmpLst;
            string posName;

            foreach (POS pl in POSList.Designs)
            {
                posName = pl.Name + "  < " + pl.Moon + " >";
                if (posName == SelReactPos)
                {
                    tmpLst = new ArrayList();
                    foreach (ReactionLink rl in pl.ReactionLinks)
                    {
                        if ((rl.InpID != m.ModuleID) && (rl.OutID != m.ModuleID))
                        {
                            tmpLst.Add(rl);
                        }
                    }
                    pl.ReactionLinks = new ArrayList(tmpLst);
                    break;
                }
            }
            DisplayReactionModules();
        }

        private void ClearSelectData()
        {
            TowerReactMod tr;

            foreach (Control c in p_PosMods.Controls)
            {
                tr = (TowerReactMod)c;

                if (tr.ReactMod.ModuleID == srcID)
                    tr.ClearLinkSel(srcNm);
                if (tr.ReactMod.ModuleID == dstID)
                    tr.ClearLinkSel(dstNm);
            }

            srcID = 0;
            dstID = 0;
            srcNm = "";
            dstNm = "";
        }

        private void SetLinkColorBG(POS pl)
        {
            TowerReactMod tr;

            foreach (Control c in p_PosMods.Controls)
            {
                tr = (TowerReactMod)c;

                if (tr.ReactMod.ModuleID == srcID)
                    tr.SetLinkColor(LColor[(pl.ReactionLinks.Count-1)], srcNm);
                if (tr.ReactMod.ModuleID == dstID)
                    tr.SetLinkColor(LColor[(pl.ReactionLinks.Count-1)], dstNm);
            }
        }

        public Bitmap GetIcon(string icon)
        {
            Bitmap bmp;
            string imgLoc;

            imgLoc = EveHQ.Core.ImageHandler.GetImageLocation(icon, Convert.ToInt32(EveHQ.Core.ImageHandler.ImageType.Icons));

            try
            {
                bmp = new Bitmap(Image.FromFile(imgLoc));
            }
            catch
            {
                bmp = new Bitmap(il_system.Images[0]);
            }

            return bmp;
        }

#endregion


#region Tower Notifications

        private TreeNode FindTreeNode(TreeNodeCollection tnc, string node)
        {
            foreach (TreeNode tn in tnc)
            {
                if (tn.Text == node)
                    return tn;
            }

            return null;
        }

        private void PopulatePlayerList()
        {
            TreeNode mtn, tn;

            tv_Players.Nodes.Clear();

            foreach (Player p in PL.Players)
            {
                mtn = tv_Players.Nodes.Add(p.Name);
                mtn.Name = p.Name;
                if(p.Email1 != "")
                    tn = mtn.Nodes.Add(p.Email1);
                if (p.Email2 != "")
                    tn = mtn.Nodes.Add(p.Email2);
                if (p.Email3 != "")
                    tn = mtn.Nodes.Add(p.Email3);
            }
        }

        private void PopulateNotificationList()
        {
            TreeNode mtn, tn, pn;
            string line;

            tv_Notifications.Nodes.Clear();

            foreach (PosNotify p in NL.NotifyList)
            {
                mtn = FindTreeNode(tv_Notifications.Nodes, p.Tower);
                if (mtn == null)
                {
                    mtn = tv_Notifications.Nodes.Add(p.Tower);
                    mtn.Name = p.Tower;
                }
                line = p.Type + " [ Start at " + p.InitQty + " " + p.Initial + " | Every " + p.FreqQty + " " + p.Frequency + " after.]";
                tn = mtn.Nodes.Add(line);

                foreach (Player pl in p.PList.Players)
                {
                    pn = tn.Nodes.Add(pl.Name);
                }
            }
        }

        private void tsb_NewPlayer_Click(object sender, EventArgs e)
        {
            AddPlayer apf = new AddPlayer();
            apf.myData = this;
            apf.SetupData("Add a New Player", "");

            apf.ShowDialog();
            PopulatePlayerList();
        }

        private void tsb_NewNotification_Click(object sender, EventArgs e)
        {
            Notification not = new Notification();
            not.myData = this;
            not.SetupData("Add a New Notification", "", "");

            not.ShowDialog();
            PopulateNotificationList();
        }

        private void tsb_Test_Click(object sender, EventArgs e)
        {
            mailSendErr = false;
            bgw_ManualSend.RunWorkerAsync();
        }

        private bool SendNotification(PosNotify pn, POS p)
        {
            string eServe, eAddy, eUser, ePass, paddStr = "", mailMsg = "", subject;
            string paddLine = "";
            bool useSMTP;
            int ePort;
            decimal timeP;
            decimal totVol=0, totCost=0;
            decimal[,] tval = new decimal[13, 3];
            string[,] fVal;
            decimal ReactTime = 0;
            ArrayList ReactRet;
            System.Net.Mail.SmtpClient Notify = new System.Net.Mail.SmtpClient();
            System.Net.Mail.MailMessage NtfMsg;

            for (int x = 0; x < 13; x++)
            {
                for (int y = 0; y < 3; y++)
                    tval[x, y] = 0;
            }

            eServe = EveHQ.Core.HQ.EveHQSettings.EMailServer;
            ePort = EveHQ.Core.HQ.EveHQSettings.EMailPort;
            eAddy = EveHQ.Core.HQ.EveHQSettings.EMailAddress;
            useSMTP = EveHQ.Core.HQ.EveHQSettings.UseSMTPAuth;
            eUser = EveHQ.Core.HQ.EveHQSettings.EMailUsername;
            ePass = EveHQ.Core.HQ.EveHQSettings.EMailPassword;

            Notify.Port = ePort;
            Notify.Host = eServe;

            if (useSMTP)
            {
                System.Net.NetworkCredential netCred = new System.Net.NetworkCredential();
                netCred.Password = ePass;
                netCred.UserName = eUser;
                Notify.Credentials = netCred;
            }

            if (pn.Type == "Fuel Level")
            {
                timeP = p.ComputePosFuelUsageForFillTracking(4, 0, Config.data.FuelCosts);
                p.PosTower.T_Fuel.SetCurrentFuelVolumes();
                p.PosTower.T_Fuel.SetCurrentFuelCosts(Config.data.FuelCosts);
                fVal = p.PosTower.T_Fuel.GetFuelBayTotals();

                mailMsg += "Tower   : " + p.Name + "\n";
                mailMsg += "Location: " + p.Moon + "\n";
                mailMsg += "Fuel Run Time Is: " + ConvertHoursToTextDisplay(p.PosTower.F_RunTime) + "\n";
                mailMsg += "\nThe Tower Needs the Following Fuel for MAX Run Time:\n";
                mailMsg += "----------------------------------------------------\n\n";

                for (int x = 0; x < 13; x++)
                {
                    if ((!p.UseChart) && (x == 11))
                        continue;

                    if (Convert.ToDecimal(fVal[x, 1]) > 0)
                    {
                        paddStr = fVal[x, 0].PadRight(17, ' ');
                        paddLine = " [ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 1])).PadLeft(7) + " ]";
                        paddStr += paddLine;
                        paddLine = "[ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 2])).PadLeft(7) + "m3 ]";
                        paddStr += paddLine;
                        paddLine = "[ " + String.Format("{0:#,0.#}", Convert.ToDecimal(fVal[x, 3])).PadLeft(11) + "isk ]\n";
                        paddStr += paddLine;
                        totVol += Convert.ToDecimal(fVal[x, 2]);
                        totCost += Convert.ToDecimal(fVal[x, 3]);
                        tval[x, 0] += Convert.ToDecimal(fVal[x, 1]);
                        tval[x, 1] += Convert.ToDecimal(fVal[x, 2]);
                        tval[x, 2] += Convert.ToDecimal(fVal[x, 3]);
                        mailMsg += paddStr;
                    }
                }
                mailMsg += "Transport Volume  [ " + String.Format("{0:#,0.#}", totVol).PadLeft(12) + "m3 ]\n";
                mailMsg += "Total Fuel Cost   [ " + String.Format("{0:#,0.#}", totCost).PadLeft(11) + "isk ]\n";
                //Clipboard.SetText(mailMsg);
            }
            else if (pn.Type == "Silo Level")
            {
                ReactRet = GetLongestSiloRunTime(p);
                ReactTime = (decimal)ReactRet[0];

                mailMsg += "Tower   : " + p.Name + "\n";
                mailMsg += "Location: " + p.Moon + "\n";
                mailMsg += "\nThe Tower Needs the Silo's Dealt With!\n";
                mailMsg += "----------------------------------------------------\n\n";

                foreach (Module m in p.Modules)
                {
                    paddLine = "";
                    switch (Convert.ToInt64(m.ModType))
                    {
                        case 2:
                        case 8:
                        case 9:
                        case 11:
                        case 12:
                        case 13:
                            paddLine = m.selMineral.name + " Silo Needs attention in [ " + ConvertHoursToTextDisplay(m.FillEmptyTime) + " ]\n\n";
                            break;
                        default:
                            break;
                    }
                    mailMsg += paddLine;
                }
            }
            else
                return false;  // Unknown Type

            NtfMsg = new System.Net.Mail.MailMessage();
            NtfMsg.From = new System.Net.Mail.MailAddress(eAddy);
            foreach (Player pl in pn.PList.Players)
            {
                foreach (Player ply in PL.Players)
                {
                    if (pl.Name == ply.Name)
                    {
                        if (ply.Email1 != "")
                        {
                            NtfMsg.To.Add(ply.Email1);
                        }
                        if (ply.Email2 != "")
                        {
                            NtfMsg.To.Add(ply.Email2);
                        }
                        if (ply.Email3 != "")
                        {
                            NtfMsg.To.Add(ply.Email3);
                        }
                    }
                }
            }
            subject = "EveHQ PoSManager: " + pn.Type + " for '" + p.Name + "'";
            NtfMsg.Subject = subject;
            NtfMsg.Body = mailMsg;
            try 
            {
                Notify.Send(NtfMsg);
            }
            catch
            {
                MessageBox.Show("Error during attempt to Send Mail\nCheck to make sure the Mail configuration for EveHQ\nis correct and that you have a good connection.\n\nYou will need to do a Manual Send (button)\nOr restart manager to re-enable Notification Mails.", "Mail Send Error");
                mailSendErr = true;
                return false;
            }
            return true;
        }

        private void tsm_EditNotify_Click(object sender, EventArgs e)
        {
            TreeNode sel;
            string root, rule;

            sel = tv_Notifications.SelectedNode;
            if (sel == null)
                return;

            if (sel.Parent == null)
            {
                if (sel.Nodes.Count > 1)
                {
                    DialogResult dr = MessageBox.Show("You Must Select the Rule you wish to Edit, Not the Tower.", "Edit Notification", MessageBoxButtons.OK);
                    return; // Request they select a rule for the tower
                }

                root = sel.Text;
                rule = sel.FirstNode.Text;
            }
            else
            {
                if (sel.Parent.Parent != null)
                    sel = sel.Parent;

                rule = sel.Text;
                root = sel.Parent.Text;
            }

            Notification not = new Notification();
            not.myData = this;
            not.SetupData("Edit a Notification", root, rule);

            not.ShowDialog();
            PopulateNotificationList();
        }

        private void tsm_RemoveNotify_Click(object sender, EventArgs e)
        {
            TreeNode sel;
            ArrayList tLst = new ArrayList();
            string root, rule, line;

            sel = tv_Notifications.SelectedNode;
            if (sel == null)
                return;

            if (sel.Parent == null)
            {
                // Query user to save any changes
                DialogResult dr = MessageBox.Show("Are you sure you want to Delete All Tower Rules for :" + sel.Text, "Delete All Rules", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    root = sel.Text;
                    foreach (PosNotify pn in NL.NotifyList)
                    {
                        if (pn.Tower != root)
                            tLst.Add(pn);
                    }
                    NL.NotifyList = new ArrayList(tLst);
                }
                else
                    return;
            }
            else
            {
                if (sel.Parent.Parent != null)
                    sel = sel.Parent;

                rule = sel.Text;
                root = sel.Parent.Text;

                foreach (PosNotify pn in NL.NotifyList)
                {
                    line = pn.Type + " [ Start at " + pn.InitQty + " " + pn.Initial + " | Every " + pn.FreqQty + " " + pn.Frequency + " after.]";
                    if ((pn.Tower == root) && (line == rule))
                    {
                        DialogResult dr = MessageBox.Show("Are you sure you want to Delete the Rule for :" + root + "\n Rule: " + rule, "Delete Rule", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            NL.NotifyList.Remove(pn);
                        }
                        break;
                    }
                }
            }

            NL.SaveNotificationList();
            PopulateNotificationList();
        }

        private void tsm_EditPlayer_Click(object sender, EventArgs e)
        {
            TreeNode plyr;
            AddPlayer apf = new AddPlayer();
            apf.myData = this;

            plyr = tv_Players.SelectedNode;
            if (plyr != null)
            {
                if (plyr.Parent != null)
                    plyr = plyr.Parent;

                apf.SetupData("Edit Player", plyr.Text);

                apf.ShowDialog();
                PopulatePlayerList();
            }
        }

        private void tsm_RemovePlayer_Click(object sender, EventArgs e)
        {
            TreeNode plyr;
            bool delYes = false;

            foreach (Player p in PL.Players)
            {
                plyr = tv_Players.SelectedNode;
                if (plyr != null)
                {
                    if (plyr.Parent != null)
                        plyr = plyr.Parent;

                    if (p.Name == plyr.Text)
                    {
                        // Query user to save any changes
                        DialogResult dr = MessageBox.Show("Are you sure you want to Delete :" + p.Name, "Delete Player", MessageBoxButtons.YesNo);

                        if (dr == DialogResult.Yes)
                        {
                            PL.Players.Remove(p);
                            delYes = true;
                            break;
                        }
                    }
                }
            }
            if (delYes)
            {
                PL.SavePlayerList();
                PopulatePlayerList();
            }
        }

        private void bgw_SendNotify_DoWork(object sender, DoWorkEventArgs e)
        {
            CheckAndSendNotificationIfActive();
        }

        private void bgw_ManualSend_DoWork(object sender, DoWorkEventArgs e)
        {
            TreeNode sel;
            string root, rule, line;

            sel = tv_Notifications.SelectedNode;
            if (sel == null)
                return;

            if (sel.Parent == null)
            {
                if (sel.Nodes.Count > 1)
                {
                    DialogResult dr = MessageBox.Show("You Must Select the Rule you wish to Test, Not the Tower.", "Send Notification", MessageBoxButtons.OK);
                    return; // Request they select a rule for the tower
                }

                root = sel.Text;
                rule = sel.FirstNode.Text;
            }
            else
            {
                if (sel.Parent.Parent != null)
                    sel = sel.Parent;

                rule = sel.Text;
                root = sel.Parent.Text;
            }

            foreach (PosNotify pn in NL.NotifyList)
            {
                line = pn.Type + " [ Start at " + pn.InitQty + " " + pn.Initial + " | Every " + pn.FreqQty + " " + pn.Frequency + " after.]";
                if ((pn.Tower == root) && (rule == line))
                {
                    foreach (POS p in POSList.Designs)
                    {
                        if (p.Name == root)
                        {
                            SendNotification(pn, p);
                            break;
                        }
                    }
                    break;
                }
            }
        }



#endregion


    }


}
