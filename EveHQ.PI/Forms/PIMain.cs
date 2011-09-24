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
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

namespace EveHQ.PI
{
    public partial class PIMain : DevComponents.DotNetBar.Office2007Form
    {
        public PINFacility Working;
        public SortedList<string, Material> MatsList;
        public SortedList<string, Material> OverviewList;
        public SortedList<string, MaterialInfo> P1Info;
        public SortedList<string, MaterialInfo> P2Info;
        public SortedList<string, MaterialInfo> P3Info;
        public SortedList<string, MaterialInfo> P4Info;
        public SortedList<string, ArrayList> CompIn;
        public SortedList<string, ArrayList> P1RsrcLoc;
        public SortedList<string, ArrayList> P2RsrcLoc;
        public SortedList<string, string> P3RsrcLoc;
        public SortedList<string, string> P4RsrcLoc;
        SortedList<string, int> P0s = new SortedList<string, int>();
        SortedList<string, int> P1s = new SortedList<string, int>();
        SortedList<string, int> P2s = new SortedList<string, int>();
        SortedList<string, int> P3s = new SortedList<string, int>();
        SortedList<string, int> P4s = new SortedList<string, int>();
        SortedList<int, SortedList<string, Material>> OVMats;
        SortedList<string, decimal> BaseLinkLen;
        ArrayList Prod1LineItems, Prod2LineItems;
        enum dgP { Buy, Name, Qty, Cost };
        bool UpdatingSF = false;
        bool Loading = false;
        private double CompCost_1;
        public int SelectedMod;
        private object movingControl;
        private bool FacUpdated = false;
        private Point ObjOffset;

        enum UpdateType : int { All, Extractor, P1, P2, P3, P4, Numbers, Storage, Processor, Launchpad, ModSide };

        #region Startup and Setup

        public PIMain()
        {
            InitializeComponent();
            
            Loading = true;
            MatsList = new SortedList<string, Material>();
            OverviewList = new SortedList<string, Material>();
            P1Info = new SortedList<string, MaterialInfo>();
            P2Info = new SortedList<string, MaterialInfo>();
            P3Info = new SortedList<string, MaterialInfo>();
            P4Info = new SortedList<string, MaterialInfo>();
            CompIn = new SortedList<string, ArrayList>();
            P1RsrcLoc = new SortedList<string, ArrayList>();
            P2RsrcLoc = new SortedList<string, ArrayList>();
            P3RsrcLoc = new SortedList<string, string>();
            P4RsrcLoc = new SortedList<string, string>();
            BaseLinkLen = new SortedList<string, decimal>();
            Prod1LineItems = new ArrayList();
            Prod2LineItems = new ArrayList();

            BuildResourceLocations();
            SetP1ResourceLocations();
            SetP2ResourceLocations();
            DefineP3P4ResourceLocations();
            UpdateFacilityCB(false);
            GenerateAndFillP1Information();
            GenerateAndFillP2Information();
            BuildProductComboTrees();

            //PlugInData.ConvertToECUFacilityFormat();
            //PlugInData.SavePIFacilitiesToDisk();

            Loading = false;
        }

        public void SetFacilityBackground(string typ)
        {
            if (typ.Contains("Plasma"))
                pb_PlanetType.BackgroundImage = PI.Properties.Resources.Plasma;
            if (typ.Contains("Gas"))
                pb_PlanetType.BackgroundImage = PI.Properties.Resources.Gas;
            if (typ.Contains("Ice"))
                pb_PlanetType.BackgroundImage = PI.Properties.Resources.Ice;
            if (typ.Contains("Temperate"))
                pb_PlanetType.BackgroundImage = PI.Properties.Resources.Temperate;
            if (typ.Contains("Oceanic"))
                pb_PlanetType.BackgroundImage = PI.Properties.Resources.Oceanic;
            if (typ.Contains("Lava"))
                pb_PlanetType.BackgroundImage = PI.Properties.Resources.Lava;
            if (typ.Contains("Barren"))
                pb_PlanetType.BackgroundImage = PI.Properties.Resources.Barren;
            if (typ.Contains("Storm"))
                pb_PlanetType.BackgroundImage = PI.Properties.Resources.Storm;
        }

        public void DefineP3P4ResourceLocations()
        {
            BaseLinkLen.Add("Planet (Barren)", 25);
            BaseLinkLen.Add("Planet (Gas)", 250);
            BaseLinkLen.Add("Planet (Ice)", 30);
            BaseLinkLen.Add("Planet (Lava)", 25);
            BaseLinkLen.Add("Planet (Oceanic)", 35);
            BaseLinkLen.Add("Planet (Plasma)", 25);
            BaseLinkLen.Add("Planet (Storm)", 50);
            BaseLinkLen.Add("Planet (Temperate)", 25);

            P3RsrcLoc.Add("Biotech Research Reports", "{Barren} + {Lava | Plasma} + {Oceanic|Temperate}");
            P3RsrcLoc.Add("Camera Drones", "{Gas} + {Lava} + {Storm}");
            P3RsrcLoc.Add("Condensates", "Gas");
            P3RsrcLoc.Add("Cryoprotectant Solution", "{Gas | Storm} + {Oceanic|Temperate}");
            P3RsrcLoc.Add("Data Chips", "{Ice} + {Lava} + {Temperate}");
            P3RsrcLoc.Add("Gel-Matrix Biopaste", "{Barren} + {Gas} + {Storm}");
            P3RsrcLoc.Add("Guidance Systems", "{Barren | Gas Storm} + {Lava | Plasma}");
            P3RsrcLoc.Add("Hazmat Detection Systems", "{Lava | Plasma} + {Ice | Oceanic} + {Temperate}");
            P3RsrcLoc.Add("Hermetic Membranes", "{Gas} + {Oceanic} + {Temperate}");
            P3RsrcLoc.Add("High-Tech Transmitters", "{Gas} + {Lava | Plasma} + {Temperate}");
            P3RsrcLoc.Add("Industrial Explosives", "Temperate");
            P3RsrcLoc.Add("Neocoms", "{Barren} + {Gas} + {Lava}");
            P3RsrcLoc.Add("Nuclear Reactors", "{Lava} + {Plasma} + {Temperate}");
            P3RsrcLoc.Add("Planetary Vehicles", "{Barren | Plasma} + {Ice} + {Lava}");
            P3RsrcLoc.Add("Robotics", "Plasma");
            P3RsrcLoc.Add("Smartfab Units", "Lava");
            P3RsrcLoc.Add("Supercomputers", "{Gas | Storm} + {Lava | Plasma}");
            P3RsrcLoc.Add("Synthetic Synapses", "Ice");
            P3RsrcLoc.Add("Transcranial Microcontrollers", "Barren");
            P3RsrcLoc.Add("Ukomi Super Conductors", "Storm");
            P3RsrcLoc.Add("Vaccines", "Oceanic");

            P4RsrcLoc.Add("Broadcast Node", "{Barren} + {Gas} + {Ice | Oceanic} + {Lava} + {Temperate}");
            P4RsrcLoc.Add("Integrity Response Drones", "{Barren} + {Gas} + {Lava} + {Oceanic} + {Temperate}");
            P4RsrcLoc.Add("Nano-Factory", "{Storm} + {Temperate}");
            P4RsrcLoc.Add("Organic Mortar Applicators", "{Barren | Temperate | Oceanic | Ice} + {Gas} + {Plasma}");
            P4RsrcLoc.Add("Recursive Computing Module", "{Barren} + {Ice} + {Lava | Plasma}");
            P4RsrcLoc.Add("Self-Harmonizing Power Core", "{Barren} + {Gas} + {Ice | Oceanic} + {Lava} + {Temperate}");
            P4RsrcLoc.Add("Sterile Conduits", "{Barren | Gas | Storm} + {Lava} + {Oceanic}");
            P4RsrcLoc.Add("Wetware Mainframe", "{Gas} + {Lava | Plasma} + {Oceanic | Temperate}");
        }

        public void UpdateFacilityCB(bool newF)
        {
            int dg_index;

            Loading = true;

            cb_SelectFacility.Items.Clear();
            dgv_OverviewSelection.Rows.Clear();

            foreach (PINFacility pf in PlugInData.Facilities.Values)
            {
                dg_index = dgv_OverviewSelection.Rows.Add();
                cb_SelectFacility.Items.Add(pf.Name);

                if (pf.inOverview)
                    dgv_OverviewSelection.Rows[dg_index].Cells[0].Value = "Y";
                else
                    dgv_OverviewSelection.Rows[dg_index].Cells[0].Value = "N";
                
                dgv_OverviewSelection.Rows[dg_index].Cells[1].Value = pf.Name;
                if (pf.OVQty < 1)
                    pf.OVQty = 1;
                dgv_OverviewSelection.Rows[dg_index].Cells[2].Value = pf.OVQty;
            }
            Loading = false;

            if ((Working != null) && PlugInData.Facilities.ContainsKey(Working.Name))
            {
                cb_SelectFacility.SelectedItem = Working.Name;
            }

            UpdateWorkingDataDisplay();
            ComputeAndUpdatePIFacilityData(UpdateType.All);
            UpdateOverviewDisplay();
        }

        private void dgv_OverviewSelection_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string FacName;
            string cVal;
            int qty;

            if ((Loading) || (e.RowIndex < 0) || (e.ColumnIndex < 0))
                return;

            if (e.ColumnIndex == 0)
            {
                FacName = dgv_OverviewSelection.Rows[e.RowIndex].Cells[1].Value.ToString();
                cVal = dgv_OverviewSelection.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                if (cVal.Equals("Y"))
                    PlugInData.Facilities[FacName].inOverview = true;
                else
                    PlugInData.Facilities[FacName].inOverview = false;

                UpdateOverviewDisplay();
                GenerateAndFillP1Information();
                GenerateAndFillP2Information();

                if (!Loading)
                    PlugInData.SavePIFacilitiesToDisk();
            }
            else if (e.ColumnIndex == 2)
            {
                FacName = dgv_OverviewSelection.Rows[e.RowIndex].Cells[1].Value.ToString();
                qty = Convert.ToInt32(dgv_OverviewSelection.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());

                PlugInData.Facilities[FacName].OVQty = qty;

                UpdateOverviewDisplay();
                GenerateAndFillP1Information();
                GenerateAndFillP2Information();

                if (!Loading)
                    PlugInData.SavePIFacilitiesToDisk();
            }
        }

        private void UpdateOverviewDisplay()
        {
            int dg_index = 0;
            double days = Convert.ToDouble(nud_Days.Value);
            bool[] show = new bool[5];
            double delta, dHour;
            DataGridViewCellStyle redText = new DataGridViewCellStyle();
            DataGridViewCellStyle blueText = new DataGridViewCellStyle();
            DataGridViewCellStyle greenText = new DataGridViewCellStyle();
            redText.ForeColor = Color.Red;
            blueText.ForeColor = Color.Blue;
            greenText.ForeColor = Color.Green;

            show[0] = cb_P0.Checked;
            show[1] = cb_P1.Checked;
            show[2] = cb_P2.Checked;
            show[3] = cb_P3.Checked;
            show[4] = cb_P4.Checked;

            OVMats = new SortedList<int, SortedList<string, Material>>();

            // Handle all Extractors and Extraction First
            foreach (PINFacility pif in PlugInData.Facilities.Values)
            {
                if (pif.inOverview)
                {
                    // Build individual facility data
                    foreach (Extractor ext in pif.Extractors.Values)
                        OVMats = GetExtractorMatsValues(ext, OVMats, pif.Name, pif.OVQty);
                    foreach (ExtControlUnit ecu in pif.ECUs.Values)
                        OVMats = GetECUMatsValues(ecu, OVMats, pif.Name, pif.OVQty);
                    foreach (Processor p in pif.Processors.Values)
                        OVMats = GetProcessorMatsValues(p, OVMats, pif.Name, pif.OVQty);
                }
            }

            // Now do the processing for Partial Mode
            if (cbx_PartialOverview.Checked)
                OVMats = GetPartialProcessorMatsValues(OVMats);

            // Take the global Overview Facilities listing, and gimme a rundown
            dg_Overview.Rows.Clear();
            foreach (var ml in OVMats.Values)
            {
                foreach (var mt in ml.Values)
                {
                    if (show[mt.Rank])
                    {
                        dg_index = dg_Overview.Rows.Add();
                        dg_Overview.Rows[dg_index].Cells[0].Value = mt.Name;
                        dg_Overview.Rows[dg_index].Cells[1].Value = "P" + mt.Rank;
                        
                        dg_Overview.Rows[dg_index].Cells[2].Value = String.Format("{0:#,0.#}", mt.NeedHour);
                        dg_Overview.Rows[dg_index].Cells[3].Value = String.Format("{0:#,0.#}", mt.ProdHour);
                        dHour = mt.ProdHour - mt.NeedHour;
                        dg_Overview.Rows[dg_index].Cells[4].Value = String.Format("{0:#,0.#}", dHour);
                        delta = dHour * days;
                        dg_Overview.Rows[dg_index].Cells[5].Value = String.Format("{0:#,0.#}", delta);
                        dg_Overview.Rows[dg_index].Cells[6].Value = String.Format("{0:#,0.#}", (delta * mt.Value));

                        for (int x = 2; x < 7; x++)
                        {
                            if (dHour > 0)
                            {
                                dg_Overview.Rows[dg_index].Cells[x].Style = greenText;
                            }
                            else if (dHour < 0)
                            {
                                dg_Overview.Rows[dg_index].Cells[x].Style = redText;
                            }
                            else
                            {
                                dg_Overview.Rows[dg_index].Cells[x].Style = blueText;
                            }
                        }
                       
                        dg_Overview.Rows[dg_index].Cells[7].Value = mt.FacName;

                       
                    }
                }
            }
        }

        private void at_PCost1_MouseClick(object sender, MouseEventArgs e)
        {
            string name;

            if (e.Button == MouseButtons.Right)
            {
                if (at_PCost1.Nodes.Count < 1)
                    return;

                name = at_PCost1.SelectedNode.Cells[0].Text;
                SetPriceForComponent(name);
            }
        }

        private void at_PCost2_MouseClick(object sender, MouseEventArgs e)
        {
            string name;

            if (e.Button == MouseButtons.Right)
            {
                if (at_PCost2.Nodes.Count < 1)
                    return;

                name = at_PCost2.SelectedNode.Cells[0].Text;
                SetPriceForComponent(name);
            }
        }

        private void dg_Overview_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string name, cltxt;

            if (e.Button == MouseButtons.Right)
            {
                name = dg_Overview.Columns[e.ColumnIndex].HeaderText;

                if (name.Contains("Value"))
                {
                    cltxt = dg_Overview.Rows[e.RowIndex].Cells[0].Value.ToString();
                    SetPriceForComponent(cltxt);
                }
            }

        }

        private void SetPriceForComponent(string comp)
        {
            foreach (Reaction r in PlugInData.Reactions.Values)
            {
                foreach (Component c in r.inputs.Values)
                {
                    if (c.Name.Equals(comp))
                    {
                        EveHQ.Core.frmModifyPrice newPrice = new EveHQ.Core.frmModifyPrice(c.ID.ToString(), 0);
                        newPrice.ShowDialog();

                        UpdateOverviewDisplay();
                        BuildProductCostTree1(ct_SelectProduct1.Text, true);
                        BuildProductCostTree2(ct_SelectProduct2.Text, true);
                        return;
                    }
                }
                foreach (Component c in r.outputs.Values)
                {
                    if (c.Name.Equals(comp))
                    {
                        EveHQ.Core.frmModifyPrice newPrice = new EveHQ.Core.frmModifyPrice(c.ID.ToString(), 0);
                        newPrice.ShowDialog();
 
                        UpdateOverviewDisplay();
                        BuildProductCostTree1(ct_SelectProduct1.Text, true);
                        BuildProductCostTree2(ct_SelectProduct2.Text, true);
                        return;
                    }
                }
            }
        }

        private void nud_Days_ValueChanged(object sender, EventArgs e)
        {
            UpdateOverviewDisplay();
        }

        private void cbx_PartialOverview_CheckedChanged(object sender, EventArgs e)
        {
            UpdateOverviewDisplay();
        }

        private SortedList<int, SortedList<string, Material>> GetProcessorMatsValues(Processor p, SortedList<int, SortedList<string, Material>> MatL, string fName, int qty)
        {
            int mult;
            int qtyNeed;
            int qtyProd;
            int rank = 0;
            double val;

            if (qty < 1)
                qty = 1;

            if (p.Qty < 1)
                p.Qty = 1;

            if (p.Reacting.reactName != "")
            {
                mult = 60 / (p.Reacting.cycleTime / 60);
                foreach (Component c in p.Reacting.inputs.Values)
                {
                    qtyNeed = c.Qty * mult * qty * 24 * p.Qty;
                    if (c.Qty == 3000)
                        rank = 0;
                    else if (c.Qty == 40)
                        rank = 1;
                    else if (c.Qty == 10)
                        rank = 2;
                    else if (c.Qty == 6)
                    {
                        if (c.Name.Equals("Reactive Metals") || c.Name.Equals("Bacteria") || c.Name.Equals("Water"))
                            rank = 1;
                        else
                            rank = 3;
                    }

                    val = EveHQ.Core.DataFunctions.GetPrice(c.ID.ToString());

                    if (!MatL.ContainsKey(rank))
                    {
                        MatL.Add(rank, new SortedList<string, Material>()); 
                        MatL[rank].Add(c.Name, new Material(c.Name, c.ID.ToString(), qtyNeed, 0, (qtyNeed * -1), rank, "", 0, val));
                    }
                    else
                    {
                        if (MatL[rank].ContainsKey(c.Name))
                        {
                            MatL[rank][c.Name].NeedHour += qtyNeed;
                            MatL[rank][c.Name].Value = val;
                        }
                        else
                            MatL[rank].Add(c.Name, new Material(c.Name, c.ID.ToString(), qtyNeed, 0, (qtyNeed * -1), rank, "", 0, val));
                    }
                }
                foreach (Component c in p.Reacting.outputs.Values)
                {
                    qtyProd = c.Qty * mult * qty * 24 * p.Qty;
                    if (c.Qty == 20)
                        rank = 1;
                    else if (c.Qty == 5)
                        rank = 2;
                    else if (c.Qty == 3)
                        rank = 3;
                    else if (c.Qty == 1)
                        rank = 4;

                    val = EveHQ.Core.DataFunctions.GetPrice(c.ID.ToString());

                    if (!MatL.ContainsKey(rank))
                    {
                        MatL.Add(rank, new SortedList<string, Material>());
                        MatL[rank].Add(c.Name, new Material(c.Name, c.ID.ToString(), 0, qtyProd, 0, rank, fName, 0, val));
                    }
                    else
                    {
                        if (MatL[rank].ContainsKey(c.Name))
                        {
                            MatL[rank][c.Name].ProdHour += qtyProd;
                            MatL[rank][c.Name].Value = val;

                            if (!MatL[rank][c.Name].FacName.Contains(fName))
                                if (MatL[rank][c.Name].FacName.Length > 0)
                                    MatL[rank][c.Name].FacName += ", " + fName;
                                else
                                    MatL[rank][c.Name].FacName += fName;
                        }
                        else
                        {
                            MatL[rank].Add(c.Name, new Material(c.Name, c.ID.ToString(), 0, qtyProd, 0, rank, fName, 0, val));
                        }
                    }
                }
            }

            return MatL;
        }

        private SortedList<int, SortedList<string, Material>> GetPartialProcessorMatsValues(SortedList<int, SortedList<string, Material>> MatL)
        {
            int qtyProd, rank = 0;
            double inpMult = 1;
            double scnMult = 1;

            for (int x = 1; x < 5; x++)
            {
                foreach (Reaction r in PlugInData.Reactions.Values)
                {
                    if (r.level != x)
                        continue;

                    inpMult = 1;
                    foreach (Component c in r.inputs.Values)
                    {
                        if (c.Qty == 3000)
                            rank = 0;
                        else if (c.Qty == 40)
                            rank = 1;
                        else if (c.Qty == 10)
                            rank = 2;
                        else if (c.Qty == 6)
                        {
                            if (c.Name.Equals("Reactive Metals") || c.Name.Equals("Bacteria") || c.Name.Equals("Water"))
                                rank = 1;
                            else
                                rank = 3;
                        }

                        if (!MatL.ContainsKey(rank))
                        {
                            scnMult = 1;
                        }
                        else if (!MatL[rank].ContainsKey(c.Name))
                        {
                            scnMult = 1;
                        }
                        else
                        {
                            scnMult = MatL[rank][c.Name].ProdHour / MatL[rank][c.Name].NeedHour;

                            if (scnMult > 1)
                                scnMult = 1;
                        }
                        if ((scnMult < inpMult) && (scnMult > 0))
                            inpMult = scnMult;
                    }

                    foreach (Component c in r.outputs.Values)
                    {                      
                        if (!MatL.ContainsKey(x))
                            continue;
                        else if (!MatL[x].ContainsKey(c.Name))
                            continue;
                        else
                        {
                            qtyProd = Convert.ToInt32(inpMult * MatL[x][c.Name].ProdHour);
                            MatL[x][c.Name].ProdHour = qtyProd;
                        }
                    }
                }
            }

            return MatL;
        }

        private SortedList<int, SortedList<string, Material>> GetECUMatsValues(ExtControlUnit ecu, SortedList<int, SortedList<string, Material>> MatL, string fName, int qty)
        {
            string mat;
            int qh;
            int tMult;
            double val=0;

            mat = ecu.Extracting;

            if (ecu.RunTime < 25)
                tMult = ecu.RunTime;
            else
                tMult = 24;

            qh = ecu.ExtRate;
            qh = qh * tMult * qty;

            if (!MatL.ContainsKey(0))
                MatL.Add(0, new SortedList<string, Material>());

            if (MatL[0].ContainsKey(mat))
            {
                MatL[0][mat].ProdHour += qh;
                if (!MatL[0][mat].FacName.Contains(fName))
                    MatL[0][mat].FacName += ", " + fName;
            }
            else
            {
                MatL[0].Add(mat, new Material(mat, "", 0, qh, 0, 0, fName, 0, val));
            }

            return MatL;
        }

        private SortedList<int, SortedList<string, Material>> GetExtractorMatsValues(Extractor ext, SortedList<int, SortedList<string, Material>> MatL, string fName, int qty)
        {
            string mat;
            int qh;
            int tMult;
            double val = 0;

            if (ext.RunTime < 25)
                tMult = ext.RunTime;
            else
                tMult = 24;

            mat = ext.Extracting;

            qh = (int)(ext.ExtRate * (60 / ext.CycleTime));
            qh = qh * tMult * qty;

            if (!MatL.ContainsKey(0))
                MatL.Add(0, new SortedList<string, Material>());

            if (MatL[0].ContainsKey(mat))
            {
                MatL[0][mat].ProdHour += qh;
                if (!MatL[0][mat].FacName.Contains(fName))
                    MatL[0][mat].FacName += ", " + fName;
            }
            else
            {
                MatL[0].Add(mat, new Material(mat, "", 0, qh, 0, 0, fName, 0, val));
            }

            return MatL;
        }

        private void UpdateWorkingDataDisplay()
        {
            Planet p;

            if (Working == null)
            {
                cb_SelectFacility.Text = "Select a Facility or click New Above";
            }
            else if (!PlugInData.Facilities.ContainsKey(Working.Name))
            {
                cb_SelectFacility.Text = "Select a Facility or click New Above";
                p = PlugInData.GetPlanetData(Working.PlanetType);
            }
        }

        public List<string> SetAvailListByLevel(List<string> fullList, string Level)
        {
            List<string> retVal = new List<string>();

            foreach (string rs in fullList)
            {
                foreach (Component c in PlugInData.Reactions[rs].inputs.Values)
                {
                    if ((c.Qty > 10) && (Level.Equals("P2")))
                    {
                        retVal.Add(rs);
                        break;
                    }
                    else if ((c.Qty <= 10) && (Level.Equals("P3")))
                    {
                        retVal.Add(rs);
                        break;
                    }
                }
            }

            return retVal;
        }

        public string GetRecipesUsingItem(string ext)
        {
            string retVal = "";

            foreach (Reaction r in PlugInData.Reactions.Values)
            {
                foreach (Component c in r.inputs.Values)
                {
                    if (c.Name.Contains(ext) || ext.Contains(c.Name))
                    {
                        retVal += r.reactName + ", ";
                    }
                }
            }

            retVal = retVal.Trim();
            retVal = retVal.TrimEnd(',');

            return retVal;
        }

        public string GetInputsForRecipe(string rcp)
        {
            string retVal = "";

            if (PlugInData.Reactions.ContainsKey(rcp))
            {
                foreach (Component c in PlugInData.Reactions[rcp].inputs.Values)
                {
                    retVal += c.Name + ", ";
                }
            }

            retVal = retVal.Trim();
            retVal = retVal.TrimEnd(',');

            return retVal;
        }

         private void SetProcessorMatsValues()
        {
            int mult;
            double inpMult = 1;
            double scnMult = 1;
            SortedList<string, int> InpList = new SortedList<string, int>();
            int qtyNeed;
            double qtyProd;
            int rank = 0;
            int tMult, hdMult;
            bool partial = cb_AdjustNumbersForPartial.Checked;

            if (rb_PerDay.Checked)
                tMult = 1440;
            else
                tMult = 60;

            hdMult = ii_NumberOfHoursDays.Value;
            tMult *= hdMult;

            // If we are doing Partial calculations, then compute the list of total required inputs for ALL processors
            if (partial)
            {
                foreach (Processor p in Working.Processors.Values)
                {
                    foreach (Component c in p.Reacting.inputs.Values)
                    {
                        mult = tMult / (p.Reacting.cycleTime / 60);
                        qtyNeed = c.Qty * mult * p.Qty;
                        if (InpList.ContainsKey(c.Name))
                            InpList[c.Name] += qtyNeed;
                        else
                            InpList.Add(c.Name, qtyNeed);
                    }
                }
            }

            for (int x = 1; x < 5; x++)
            {
                foreach (Processor p in Working.Processors.Values)
                {
                    if (!x.Equals(p.ProcLevel))
                        continue;

                    if (p.Qty < 1)
                        p.Qty = 1;

                    inpMult = 1;

                    if (p.Reacting.reactName != "")
                    {
                        mult = tMult / (p.Reacting.cycleTime / 60);

                        // If doing Partial Input calculations, Compute input Multiplier from Needs vs. Available values
                        if (partial)
                        {
                            foreach (Component c in p.Reacting.inputs.Values)
                            {
                                if (!InpList.ContainsKey(c.Name))
                                    continue;

                                qtyNeed = InpList[c.Name];
                                if (!MatsList.ContainsKey(c.Name))
                                {
                                    scnMult = 1;
                                }
                                else
                                {
                                    if (MatsList[c.Name].ProdHour >= qtyNeed)
                                        scnMult = 1;
                                    else
                                        scnMult = MatsList[c.Name].ProdHour / qtyNeed;
                                }
                                if ((scnMult < inpMult) && (scnMult > 0))
                                    inpMult = scnMult;
                            }
                        }

                        foreach (Component c in p.Reacting.inputs.Values)
                        {
                            qtyNeed = c.Qty * mult * p.Qty;
                            if (c.Qty == 3000)
                                rank = 0;
                            else if (c.Qty == 40)
                                rank = 1;
                            else if (c.Qty == 10)
                                rank = 2;
                            else if (c.Qty == 6)
                                rank = 3;

                            if (!MatsList.ContainsKey(c.Name))
                            {
                                MatsList.Add(c.Name, new Material(c.Name, c.ID.ToString(), qtyNeed, 0, 0, rank, "", qtyNeed * inpMult, 0));
                            }
                            else
                            {
                                MatsList[c.Name].Rank = rank;
                                MatsList[c.Name].NeedHour += qtyNeed;
                                MatsList[c.Name].UseHour = MatsList[c.Name].NeedHour * inpMult;
                            }
                        }

                        foreach (Component c in p.Reacting.outputs.Values)
                        {
                            qtyProd = c.Qty * mult * p.Qty;
                            if (partial)
                                qtyProd *= inpMult;

                            if (c.Qty == 20)
                                rank = 1;
                            else if (c.Qty == 5)
                                rank = 2;
                            else if (c.Qty == 3)
                                rank = 3;
                            else if (c.Qty == 1)
                                rank = 4;
                            
                            if (!MatsList.ContainsKey(c.Name))
                            {
                                MatsList.Add(c.Name, new Material(c.Name, c.ID.ToString(), 0, qtyProd, 0, rank, "", 0, 0));
                            }
                            else
                            {
                                MatsList[c.Name].ProdHour += qtyProd;
                            }
                        }
                    }
                }
            }

            foreach (Material m in MatsList.Values)
            {
                if (m.Name.Equals(""))
                    continue;

                m.NeedVol = (m.NeedHour * (PlugInData.Components[m.Name].Volume));
                m.ProdVol = (m.ProdHour * (PlugInData.Components[m.Name].Volume));
                m.DeltaVol = (m.ProdVol - m.NeedVol);
            }
        }

        private void SetExtractorMatsValues(Extractor ext)
        {
            string mat, pType;
            int qh;
            int tMult, hdMult;

            if (rb_PerDay.Checked)
            {
                tMult = Convert.ToInt32(ext.RunTime);
            }
            else
                tMult = 1;

            hdMult = ii_NumberOfHoursDays.Value;
            tMult *= hdMult;

            if (Working == null)
                return;

            pType = Working.PlanetType.Replace("Planet (", "");
            pType = pType.Replace(')', ' ');
            pType = pType.Trim();

            mat = ext.Extracting;

            if (ext.CycleTime > 60)
            {
                ext.ExtRate = 1000;
                ext.CycleTime = 30;
            }

            qh = (int)(ext.ExtRate * (60 / ext.CycleTime));
            qh = qh * tMult;

            if (!MatsList.ContainsKey(mat))
            {
                MatsList.Add(mat, new Material(mat, "", 0, qh, 0, 0, "", 0, 0));
            }
            else
            {
                MatsList[mat].ProdHour += qh;
            }
        }

        private void SetECUMatsValues(ExtControlUnit ecu)
        {
            string mat;
            int qh, tMult, hdMult;

            if (Working == null)
                return;

            if (rb_PerDay.Checked)
            {
                if (ecu.RunTime < 25)
                    tMult = ecu.RunTime;
                else
                    tMult = 24;
            }
            else
                tMult = 1;

            hdMult = ii_NumberOfHoursDays.Value;
            tMult *= hdMult;
            
            mat = ecu.Extracting;
            qh = (int)(ecu.ExtRate);
            qh = qh * tMult;

            if (!MatsList.ContainsKey(mat))
            {
                MatsList.Add(mat, new Material(mat, "", 0, qh, 0, 0, "", 0, 0));
            }
            else
            {
                MatsList[mat].ProdHour += qh;
            }
        }

        private void ComputeAndDisplayMaterialTree()
        {
            MatsList.Clear();

            if (Working != null)
            {
                if (Working.Extractors != null)
                    foreach (Extractor ext in Working.Extractors.Values)
                        SetExtractorMatsValues(ext);

                foreach (ExtControlUnit ecu in Working.ECUs.Values)
                    SetECUMatsValues(ecu);

                SetProcessorMatsValues();
                BuildProductionTree(true);
            }
        }

        private void BuildProductionTree(bool update)
        {
            ArrayList Nodes = new ArrayList();
            DevComponents.AdvTree.Node atn;
            DevComponents.AdvTree.Cell atc;
           
            at_ProduceView.Nodes.Clear();
            for (int x = 4; x >= 0; x--)
            {
                foreach (Material m in MatsList.Values)
                {
                    if (m.Rank != x)
                        continue;

                    m.DeltaHour = m.ProdHour - m.UseHour;
                   
                    // Root or Name
                    atn = new DevComponents.AdvTree.Node();
                    atn.Text = "P" + m.Rank + " - " + m.Name;
                    atn.CheckBoxVisible = false;

                    // Needed
                    atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", m.NeedHour));                 
                    if (m.DeltaHour > 0)
                        atc.StyleNormal = at_ProduceView.Styles["OverProd"];
                    else if (m.DeltaHour < 0)
                        atc.StyleNormal = at_ProduceView.Styles["UnderProd"];
                    else
                        atc.StyleNormal = at_ProduceView.Styles["NormalProd"];
                    atn.Cells.Add(atc);

                    // Producing
                    atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", m.ProdHour));
                    if (m.DeltaHour > 0)
                        atc.StyleNormal = at_ProduceView.Styles["OverProd"];
                    else if (m.DeltaHour < 0)
                        atc.StyleNormal = at_ProduceView.Styles["UnderProd"];
                    else
                        atc.StyleNormal = at_ProduceView.Styles["NormalProd"];
                    atn.Cells.Add(atc);

                    // Using
                    atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", m.UseHour));
                    if (m.DeltaHour > 0)
                        atc.StyleNormal = at_ProduceView.Styles["OverProd"];
                    else if (m.DeltaHour < 0)
                        atc.StyleNormal = at_ProduceView.Styles["UnderProd"];
                    else
                        atc.StyleNormal = at_ProduceView.Styles["NormalProd"];
                    atn.Cells.Add(atc);

                    // Delta
                    atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", m.DeltaHour));
                    if (m.DeltaHour > 0)
                        atc.StyleNormal = at_ProduceView.Styles["OverProd"];
                    else if (m.DeltaHour < 0)
                        atc.StyleNormal = at_ProduceView.Styles["UnderProd"];
                    else
                        atc.StyleNormal = at_ProduceView.Styles["NormalProd"];
                    atn.Cells.Add(atc);

                    // Need Volumne
                    atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", m.NeedVol));
                    if (m.DeltaHour > 0)
                        atc.StyleNormal = at_ProduceView.Styles["OverProd"];
                    else if (m.DeltaHour < 0)
                        atc.StyleNormal = at_ProduceView.Styles["UnderProd"];
                    else
                        atc.StyleNormal = at_ProduceView.Styles["NormalProd"];
                    atn.Cells.Add(atc);

                    // Produce Volumne
                    atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", m.ProdVol));
                    if (m.DeltaHour > 0)
                        atc.StyleNormal = at_ProduceView.Styles["OverProd"];
                    else if (m.DeltaHour < 0)
                        atc.StyleNormal = at_ProduceView.Styles["UnderProd"];
                    else
                        atc.StyleNormal = at_ProduceView.Styles["NormalProd"];
                    atn.Cells.Add(atc);

                    // Delta Volumne
                    atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", m.DeltaVol));
                    if (m.DeltaHour > 0)
                        atc.StyleNormal = at_ProduceView.Styles["OverProd"];
                    else if (m.DeltaHour < 0)
                        atc.StyleNormal = at_ProduceView.Styles["UnderProd"];
                    else
                        atc.StyleNormal = at_ProduceView.Styles["NormalProd"];
                    atn.Cells.Add(atc);

                    // Isk Cost / Value
                    double iskVal = EveHQ.Core.DataFunctions.GetPrice(m.ID);
                    atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", (m.DeltaHour * iskVal)));
                    if (m.DeltaHour > 0)
                        atc.StyleNormal = at_ProduceView.Styles["OverProd"];
                    else if (m.DeltaHour < 0)
                        atc.StyleNormal = at_ProduceView.Styles["UnderProd"];
                    else
                        atc.StyleNormal = at_ProduceView.Styles["NormalProd"];
                    atn.Cells.Add(atc);

                    Nodes = GetComponentsForPrdItem(m.Name);

                    // Now I have my components, time to populate the Data Grid
                    foreach (LineItem li in Nodes)
                    {
                        BuildProduceNode(li, atn, update);
                    }

                    if ((at_ProduceView.Nodes.Count > 0) && update && (at_ProduceView.Nodes[0].Expanded))
                        atn.Expand();

                    at_ProduceView.Nodes.Add(atn);
                }            
            }
        }

        private void BuildProduceNode(LineItem li, DevComponents.AdvTree.Node rn, bool update)
        {
            DevComponents.AdvTree.Node atn;
            DevComponents.AdvTree.Cell atc;

            atn = new DevComponents.AdvTree.Node();
            atn.Text = li.Name;

            // Needed
            atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", li.Need));
            atc.StyleNormal = at_PCost1.Styles["es_CenterAlign"];
            atn.Cells.Add(atc);

            // Producing
            atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", li.Prod));
            atc.StyleNormal = at_PCost1.Styles["es_CenterAlign"];
            atn.Cells.Add(atc);

            // Using
            atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", li.Use));
            atc.StyleNormal = at_PCost1.Styles["es_CenterAlign"];
            atn.Cells.Add(atc);

            // Delta
            atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}",li.Delta));
            atc.StyleNormal = at_PCost1.Styles["es_CenterAlign"];
            atn.Cells.Add(atc);
            
            foreach (LineItem it in li.MadeFrom.Values)
            {
                BuildProduceNode(it, atn, update);
            }

            if ((at_ProduceView.Nodes.Count > 0) && IsCurrentNodeExpanded(at_ProduceView.Nodes[0], li.Name))
                atn.Expand();

            rn.Nodes.Add(atn);
        }

        private ArrayList GetComponentsForPrdItem(string item)
        {
            ArrayList RootNodes = new ArrayList();
            LineItem Comp = new LineItem();
            Reaction r;
            Material mat;
            string snm;

            if (!PlugInData.Reactions.ContainsKey(item))
                return RootNodes;
            else
                r = PlugInData.Reactions[item];

            // For Each Root Component that Makes up the Item
            foreach (Component c in r.inputs.Values)
            {
                // Read in direct inputs here
                Comp = new LineItem();
                Comp.ID = c.ID;
                Comp.Name = c.Name;
                Comp.Qty = c.Qty;
                Comp.BuyIt = false;

                if (MatsList.ContainsKey(c.Name))
                {
                    mat = MatsList[c.Name];
                }
                else
                {
                    mat = new Material();
                    mat.NeedHour = c.Qty;
                    mat.ProdHour = 0;
                    mat.UseHour = c.Qty;
                    mat.DeltaHour = (0 - c.Qty);
                }

                Comp.Need = mat.NeedHour;
                Comp.Prod = mat.ProdHour;
                Comp.Use = mat.UseHour;
                Comp.Delta = (mat.ProdHour - mat.UseHour);

                if (PlugInData.Reactions.ContainsKey(c.Name))
                {
                    // This component has Sub-Components, Go Get EM!
                    GetSubComponentsForPrdItem(c.Name, Comp);
                }
                else
                {
                    snm = c.Name.TrimEnd('s');
                    if (PlugInData.Reactions.ContainsKey(snm))
                        GetSubComponentsForPrdItem(snm, Comp);
                }

                RootNodes.Add(Comp);
            }

            return RootNodes;
        }

        private LineItem GetSubComponentsForPrdItem(string sItem, LineItem item)
        {
            Reaction r;
            LineItem SubComp = new LineItem();
            string snm;
            int mult = 1;
            Material mat;

            r = PlugInData.Reactions[sItem];

            // For Each Root Component that Makes up the Item
            foreach (Component c in r.inputs.Values)
            {
                // Read in direct inputs here
                mult = item.Qty / r.outputs.Values[0].Qty;

                SubComp = new LineItem();
                SubComp.ID = c.ID;
                SubComp.Name = c.Name;
                SubComp.Qty = c.Qty;
                SubComp.BuyIt = false;
                if (MatsList.ContainsKey(c.Name))
                {
                    mat = MatsList[c.Name];
                }
                else
                {
                    mat = new Material();
                    mat.NeedHour = c.Qty;
                    mat.ProdHour = 0;
                    mat.UseHour = c.Qty;
                    mat.DeltaHour = (0 - c.Qty);
                }

                SubComp.Need = mat.NeedHour;
                SubComp.Prod = mat.ProdHour;
                SubComp.Use = mat.UseHour;
                SubComp.Delta = mat.ProdHour - mat.UseHour;

                if (PlugInData.Reactions.ContainsKey(c.Name))
                {
                    // This component has Sub-Components, Go Get EM!
                    GetSubComponentsForPrdItem(c.Name, SubComp);
                }
                else
                {
                    snm = c.Name.TrimEnd('s');
                    if (PlugInData.Reactions.ContainsKey(snm))
                        GetSubComponentsForPrdItem(snm, SubComp);
                }

                item.MadeFrom.Add(c.Name, SubComp);
            }

            return item;
        }

        private void ComputeAndUpdatePIFacilityData(UpdateType UT)
        {
            int TotalCPU = 0, TotalPower = 0, StoreSpace = 0;
            double LinkCPU = 0, LinkPower = 0;
            double totLinks, linkLen;
            decimal numMod = 0;

            if (Working == null)
                return;

            if (Working.PlanetType.Contains("Planet"))
            {
                Working.PlanetType = Working.PlanetType.Replace("Planet (", "");
                Working.PlanetType = Working.PlanetType.Replace(")", "");
                Working.PlanetType = Working.PlanetType.Trim();
            }

            totLinks = Convert.ToDouble(nud_NumLinks.Value);
            linkLen = Convert.ToDouble(nud_LinkLength.Value);

            LinkCPU = (totLinks * ((linkLen * PlugInData.BaseLink.CPUPerKM) + PlugInData.BaseLink.CPU));
            LinkPower = (totLinks * ((linkLen * PlugInData.BaseLink.MWPerKM) + PlugInData.BaseLink.Power));

            if (Working.Command.CPU <= 0)
            {
                pb_CPU.Maximum = 1675;
                pb_Power.Maximum = 6000;
                StoreSpace = 500;
            }
            else
            {
                pb_CPU.Maximum = Working.Command.CPU;
                pb_Power.Maximum = Working.Command.Power;
                StoreSpace += Working.Command.Capacity;
            }

            foreach (Launchpad lp in Working.LaunchPad.Values)
            {
                TotalCPU += lp.CPU;
                TotalPower += lp.Power;
                StoreSpace += lp.Capacity;
                numMod++;
            }

            foreach (Extractor ex in Working.Extractors.Values)
            {
                TotalCPU += ex.CPU;
                TotalPower += ex.Power;
                StoreSpace += ex.Capacity;
                numMod++;
            }

            foreach (ExtControlUnit ecu in Working.ECUs.Values)
            {
                TotalCPU += ecu.CPU;
                TotalPower += ecu.Power;
                StoreSpace += ecu.Capacity;

                TotalCPU += (ecu.Heads * ecu.Head_CPU);
                TotalPower += (ecu.Heads * ecu.Head_Power);
                numMod++;
            }

            foreach (Processor p1 in Working.Processors.Values)
            {
                if (p1.ProcLevel == 4)
                {
                    foreach (Processor c in PlugInData.Planets[Working.PlanetType].Processors.Values)
                    {
                        if (c.Name.Contains("High"))
                        {
                            p1.CPU = c.CPU;
                            p1.Power = c.Power;
                            p1.Name = c.Name;
                            break;
                        }
                    }
                }
                if (p1.ProcLevel == 1)
                {
                    foreach (Processor c in PlugInData.Planets[Working.PlanetType].Processors.Values)
                    {
                        if (c.Name.Contains("Basic"))
                        {
                            p1.CPU = c.CPU;
                            p1.Power = c.Power;
                            p1.Name = c.Name;
                            break;
                        }
                    }
                }
                else if (p1.ProcLevel > 1)
                {
                    foreach (Processor c in PlugInData.Planets[Working.PlanetType].Processors.Values)
                    {
                        if (c.Name.Contains("Advanced"))
                        {
                            p1.CPU = c.CPU;
                            p1.Power = c.Power;
                            p1.Name = c.Name;
                            break;
                        }
                    }
                }

                TotalCPU += p1.CPU * p1.Qty;
                TotalPower += p1.Power * p1.Qty;
                StoreSpace += p1.Capacity * p1.Qty;
                numMod++;
            }

            foreach (StorageFacility sf in Working.Storage.Values)
            {
                TotalCPU += sf.CPU;
                TotalPower += sf.Power;
                StoreSpace += sf.Capacity;
                numMod++;
            }

            TotalCPU += Convert.ToInt32(LinkCPU);
            TotalPower += Convert.ToInt32(LinkPower);

            if (TotalCPU > pb_CPU.Maximum)
                pb_CPU.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Error;
            else
                pb_CPU.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Normal;

            if (TotalPower > pb_Power.Maximum)
                pb_Power.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Error;
            else
                pb_Power.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Normal;

            pb_CPU.Value = TotalCPU;
            pb_CPU.Text = TotalCPU + " / " + Working.Command.CPU;
            pb_Power.Value = TotalPower;
            pb_Power.Text = TotalPower + " / " + Working.Command.Power;

            Working.Command.CPU_Used = TotalCPU;
            Working.Command.Power_Used = TotalPower;

            if (PlugInData.Facilities.ContainsKey(Working.Name))
            {
                PlugInData.Facilities.Remove(Working.Name);
                PlugInData.Facilities.Add(Working.Name, Working);
            }
            else
            {
                PlugInData.Facilities.Add(Working.Name, Working);
            }

            if (numMod < 1)
                nud_NumLinks.Value = numMod;

            ComputeAndDisplayMaterialTree();

            if (UT != UpdateType.ModSide)
            {
                foreach (Control cn in gp_Planner.Controls)
                {
                    if (cn.GetType().Name == "Command_Mod")
                    {
                        Command_Mod cm = (Command_Mod)cn;
                        cm.Command_Mod_Update();
                    }
                }
            }

            // My PI Design has been updated, SAVE it to Disk.
            if (!UpdatingSF)
                PlugInData.SavePIFacilitiesToDisk();

            // Update the Overview
            UpdateOverviewDisplay();
        }

        private void rb_TimeDurationChange(object sender, EventArgs e)
        {
            if (rb_RunPerHour.Checked)
                lbx_HoursDays.Text = "# of Hours";
            else
                lbx_HoursDays.Text = "# of Days";

            ComputeAndDisplayMaterialTree();
        }

        private void ii_NumberOfHoursDays_ValueChanged(object sender, EventArgs e)
        {
            ComputeAndDisplayMaterialTree();
        }

        private void nud_ExtHoursPerDay_ValueChanged(object sender, EventArgs e)
        {
            ComputeAndDisplayMaterialTree();
            UpdateOverviewDisplay();
        }

        private void cb_AdjustForPartial_CheckedChanged(object sender, EventArgs e)
        {
            ComputeAndDisplayMaterialTree();
        }

        private void PIMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FacUpdated)
            {
                PlugInData.SavePIFacilitiesToDisk();
                FacUpdated = false;
            }
        }


        #endregion

        #region Button Handling

        private void b_NewFacility_Click(object sender, EventArgs e)
        {
            Planet p;
            NewPIFacility NPIF = new NewPIFacility();
            NPIF.ShowDialog();

            if (PlugInData.NewFacName == "")
                return;

            Working = new PINFacility();
            Working.Name = PlugInData.NewFacName;
            Working.PlanetType = PlugInData.NewFacPlanet;

            PlugInData.Facilities.Add(Working.Name, new PINFacility(Working));
            SetFacilityBackground(Working.PlanetType);

            UpdateFacilityCB(true);

            p = PlugInData.GetPlanetData(Working.PlanetType);
            foreach (CommandCenter cc in p.CmdCenter.Values)
            {
                if (cc.Name.Contains("Advanced"))
                {
                    Working.Command = new CommandCenter(cc);
                    break;
                }
            }

            gp_Planner.Controls.Clear();
            Command_Mod CM = new Command_Mod(this);
            CM.Location = Working.Command.CLoc;
            CM.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CommandCenter_OnMouseDown);

            gp_Planner.Controls.Add(CM);

            FacUpdated = true;
        }

        private void b_Copy_Click(object sender, EventArgs e)
        {
            CopyRename CR = new CopyRename();
            PlugInData.CurFacName = Working.Name;
            PINFacility newFacil;

            CR.ShowDialog();

            if (PlugInData.NewFacName != "")
            {
                newFacil = new PINFacility(Working);
                newFacil.Name = PlugInData.NewFacName;
                PlugInData.Facilities.Add(newFacil.Name, new PINFacility(newFacil));

                PlugInData.Facilities[newFacil.Name].inOverview = false;

                Working = new PINFacility(PlugInData.Facilities[newFacil.Name]);

                PlugInData.SavePIFacilitiesToDisk();

                UpdateFacilityCB(true);
           }
        }

        private void b_Rename_Click(object sender, EventArgs e)
        {
            CopyRename CR = new CopyRename();
            PlugInData.CurFacName = Working.Name;

            CR.ShowDialog();

            if (PlugInData.NewFacName != "")
            {
                PlugInData.Facilities.Remove(PlugInData.CurFacName);

                Working.Name = PlugInData.NewFacName;
                PlugInData.Facilities.Add(Working.Name, new PINFacility(Working));

                PlugInData.Facilities[Working.Name].inOverview = false;

                PlugInData.SavePIFacilitiesToDisk();

                UpdateFacilityCB(true);
            }
        }

        private void b_Delete_Click(object sender, EventArgs e)
        {
            int sI = cb_SelectFacility.SelectedIndex;

            if (PlugInData.Facilities.ContainsKey(cb_SelectFacility.Text))
            {
                PlugInData.Facilities.Remove(cb_SelectFacility.Text);
                Working = null;
                pb_CPU.Value = 0;
                pb_CPU.Maximum = 100;
                pb_Power.Value = 0;
                pb_Power.Maximum = 100;
                pb_CPU.Text = "0 / 0";
                pb_Power.Text = "0 / 0";

                if (cb_SelectFacility.Items.Count > 0 && sI == 0)
                    cb_SelectFacility.SelectedIndex = 1;
                else
                    cb_SelectFacility.SelectedIndex = 0;

                rtb_PlanetDescription.Text = "";

                foreach (Control c in gp_Planner.Controls)
                    c.Dispose();

                gp_Planner.Controls.Clear();

                PlugInData.SavePIFacilitiesToDisk();

                UpdateFacilityCB(true);
            }
        }

        private void b_Import_Click(object sender, EventArgs e)
        {

        }

        private void b_ExportToFile_Click(object sender, EventArgs e)
        {

        }

        private void b_ExportToClipboard_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Module and Display Handling

        public Bitmap GetImageForItem(long typeID)
        {
            string imgKey;
            Bitmap bmp;
            Image img;

            img = EveHQ.Core.ImageHandler.GetImage(typeID.ToString());

            if (img == null)
            {
                imgKey = typeID.ToString() + ".png";
                if (il_PI_Images.Images.ContainsKey(imgKey))
                    bmp = new Bitmap(il_PI_Images.Images[imgKey]);
                else
                    bmp = new Bitmap(PI.Properties.Resources.noitem);
            }
            else
            {
                bmp = new Bitmap(img);
            }

            return bmp;
        }

        public Bitmap GetIconForItem(string icon)
        {
            string imgKey;
            Bitmap bmp;
            Image img;

            img = EveHQ.Core.ImageHandler.GetImage(icon);

            if (img == null)
            {
                imgKey = icon + ".png";
                if (il_PI_Images.Images.ContainsKey(imgKey))
                    bmp = new Bitmap(il_PI_Images.Images[imgKey]);
                else
                    bmp = new Bitmap(PI.Properties.Resources.noitem);
            }
            else
            {
                bmp = new Bitmap(img);
            }

            return bmp;
        }

        private void cb_Facility_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CurrentName;
            Planet p;
            decimal totalMods;
            Command_Mod CM;

            CurrentName = cb_SelectFacility.Text;
            totalMods = 0;

            if (FacUpdated)
            {
                PlugInData.SavePIFacilitiesToDisk();
                FacUpdated = false;
            }

            // 1. Get new PoS Data from List
            if (PlugInData.Facilities.ContainsKey(CurrentName))
            {
                UpdatingSF = true;
                gp_Planner.Controls.Clear();

                // 3. Populate PoS Display with PoS Information
                //Working = new PINFacility(PlugInData.Facilities[CurrentName]);
                Working = PlugInData.Facilities[CurrentName];
                p = PlugInData.GetPlanetData(Working.PlanetType);

                if (Working.AvgLinkLength != 0)
                {
                    nud_LinkLength.Value = Working.AvgLinkLength;
                }
                else
                {
                    if (BaseLinkLen.ContainsKey(Working.PlanetType))
                        nud_LinkLength.Value = BaseLinkLen[Working.PlanetType];
                    else
                        nud_LinkLength.Value = 25;
                }

                rtb_PlanetDescription.Text = p.Description;
                SetFacilityBackground(Working.PlanetType);
                // Populate Planet
                if (Working.Command.Name != "")
                {
                    CM = new Command_Mod(this);
                    CM.Location = Working.Command.CLoc;
                    CM.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CommandCenter_OnMouseDown);

                    gp_Planner.Controls.Add(CM);

                    // Next Populate the Launch Pad(s) - if it has been installed
                    totalMods += Working.LaunchPad.Count;
                    foreach (Launchpad lp in Working.LaunchPad.Values)
                    {
                        Launch_Pad MD = new Launch_Pad(this, lp.Location);
                        MD.Location = lp.CLoc;
                        MD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CommandCenter_OnMouseDown);
                        
                        gp_Planner.Controls.Add(MD);
                    }

                    // Populate: Extractors, Processors, Storage Facilities
                    totalMods += Working.Extractors.Count;
                    foreach (Extractor ex in Working.Extractors.Values)
                    {
                        Extractor_Mod MD = new Extractor_Mod(this, ex.Location);
                        MD.Location = ex.CLoc;
                        MD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CommandCenter_OnMouseDown);
                        gp_Planner.Controls.Add(MD);
                    }

                    totalMods += Working.ECUs.Count;
                    foreach (ExtControlUnit ex in Working.ECUs.Values)
                    {
                        // Since I now have DB available, need to fill in any missing information
                        ex.ECU_FillInMissing(p.ECU);
                        ECU_Mod MD = new ECU_Mod(this, ex.Location);
                        MD.Location = ex.CLoc;
                        MD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CommandCenter_OnMouseDown);
                        gp_Planner.Controls.Add(MD);
                    }

                    foreach (Processor pr in Working.Processors.Values)
                    {
                        Processor_Mod MD = new Processor_Mod(this, pr.Location);
                        MD.Location = pr.CLoc;
                        MD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CommandCenter_OnMouseDown);
                        gp_Planner.Controls.Add(MD);
                        totalMods += pr.Qty;
                    }

                    totalMods += Working.Storage.Count;
                    foreach (StorageFacility sf in Working.Storage.Values)
                    {
                        Storage_Mod MD = new Storage_Mod(this, sf.Location);
                        MD.Location = sf.CLoc;
                        MD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CommandCenter_OnMouseDown);
                        gp_Planner.Controls.Add(MD);
                    }

                }
            }
            else
                return;

            nud_NumLinks.Value = totalMods;

            ComputeAndUpdatePIFacilityData(UpdateType.All);
            UpdatingSF = false;
        }

        public void PIM_RemoveModule(string contName, string type)
        {
            switch (type)
            {
                case "Processor":
                    if (Working.Processors != null)
                        if (Working.Processors.ContainsKey(contName))
                            Working.Processors.Remove(contName);
                    break;
                case "Extractor":
                    if (Working.Extractors != null)
                        if (Working.Extractors.ContainsKey(contName))
                            Working.Extractors.Remove(contName);
                    break;
                case "Launchpad":
                    if (Working.LaunchPad != null)
                        if (Working.LaunchPad.ContainsKey(contName))
                            Working.LaunchPad.Remove(contName);
                    break;
                case "Storage":
                    if (Working.Storage != null)
                        if (Working.Storage.ContainsKey(contName))
                            Working.Storage.Remove(contName);
                    break;
                case "ECU":
                    if (Working.ECUs != null)
                        if (Working.ECUs.ContainsKey(contName))
                            Working.ECUs.Remove(contName);
                    break;
            }

            FacUpdated = true;
            ComputeAndUpdatePIFacilityData(UpdateType.All);
        }

        private void Overview_Selections_Changed(object sender, EventArgs e)
        {
            UpdateOverviewDisplay();
        }

        private void nud_LinkLength_ValueChanged(object sender, EventArgs e)
        {
            if (UpdatingSF)
                return;

            Working.AvgLinkLength = nud_LinkLength.Value;
            ComputeAndUpdatePIFacilityData(UpdateType.Numbers);
        }

        private void nud_NumLinks_ValueChanged(object sender, EventArgs e)
        {
            if (UpdatingSF)
                return;

            Working.NumLinks = nud_NumLinks.Value;
            ComputeAndUpdatePIFacilityData(UpdateType.Numbers);
        }


        #endregion

        #region Commodities Display Tracking

        private void BuildResourceLocations()
        {
            string rslt;

            foreach (Reaction r in PlugInData.Reactions.Values)
            {
                rslt = r.outputs.Values[0].Name;

                foreach (Reaction ui in PlugInData.Reactions.Values)
                {
                    foreach (Component c in ui.inputs.Values)
                    {
                        if (c.Name.Equals(rslt))
                        {
                            if (CompIn.ContainsKey(rslt))
                            {
                                CompIn[rslt].Add(ui.reactName);
                            }
                            else
                            {
                                CompIn.Add(rslt, new ArrayList());
                                CompIn[rslt].Add(ui.reactName);
                            }
                        }
                    }
                }
            }
        }

        private void SetP1ResourceLocations()
        {
            foreach (Reaction r in PlugInData.Reactions.Values)
            {
                if (r.inputs.Count > 1)
                    continue;

                if (!P1RsrcLoc.ContainsKey(r.outputs.Values[0].Name))
                {
                    // Resource is Not in the list
                    foreach (var v in PlugInData.Resources)
                    {
                        if (r.inputs.Values[0].Name.Contains(v.Key))
                        {
                            P1RsrcLoc.Add(r.outputs.Values[0].Name, new ArrayList(v.Value.Planets));
                        }
                    }
                }
            }
        }

        private void SetP2ResourceLocations()
        {
            string sMake = "", mMake = "";
            ArrayList pl;

            foreach (Reaction r in PlugInData.Reactions.Values)
            {
                if ((r.inputs.Count == 2) && (r.inputs.Values[0].Qty > 10))
                {

                    pl = new ArrayList();
                    // Now I have P2's, which are made up from P1's. So for each P2 I need to determine the P1's
                    // and then the planet(s) that can make said P1's.
                    if (!P2RsrcLoc.ContainsKey(r.outputs.Values[0].Name))
                    {
                        // We have 2 inputs. For each one, get the list of Planets that can make the item
                        // Compare the 2 lists
                        // If a planet is in both lists, add the planet to the P2RsrsLoc list
                        sMake = "";
                        if (P1RsrcLoc.ContainsKey(r.inputs.Values[0].Name))
                        {
                            foreach (string s1 in P1RsrcLoc[r.inputs.Values[0].Name])
                            {
                                if (P1RsrcLoc[r.inputs.Values[1].Name].Contains(s1))
                                {
                                    if (sMake.Length == 0)
                                    {
                                        sMake = s1;
                                    }
                                    else
                                    {
                                        sMake += " | " + s1;
                                    }
                                }
                            }
                        }
                        pl.Add(sMake);

                        // The above code catches the cases where 1 planet can make the item.
                        // Now to catch the cases where 2 or more planets are required
                        for (int x = 0; x < r.inputs.Count; x++)
                        {
                            mMake = "";
                            foreach (string s2 in P1RsrcLoc[r.inputs.Values[x].Name])
                            {
                                // Componenet 1 can be made at each of these planets
                                if (mMake.Length == 0)
                                    mMake = s2;
                                else
                                    mMake += " | " + s2;
                            }
                            pl.Add(mMake);
                        }

                        P2RsrcLoc.Add(r.outputs.Values[0].Name, new ArrayList(pl));

                    }
                }
            }
        }

        private void ConsolidateResourceListings(SortedList<string, ArrayList> rsrc)
        {
            foreach (var rs in rsrc)
            {
                if (rsrc.Values[0].ToString() != "")
                    continue;   // We have a solo for this component, so no reason to bother consolidating it.

                foreach (string r1 in rs.Value)
                {
                    // OK, this one needs to be made up of all the requirements for all of the elements
                    if ((rs.Value.Count > 2) && (rs.Value[0].ToString() != ""))
                        rsrc[rs.Key] = new ArrayList(ConsolidateItem(rs.Value));
                }
            }
        }

        private ArrayList ConsolidateItem(ArrayList itm)
        {
            string[] p2r, p3r;
            ArrayList retVal = new ArrayList();
            bool Need3 = true, Need2 = true;

            p2r = itm[2].ToString().Split('|');

            if (itm.Count > 3)
            {
                p3r = itm[3].ToString().Split('|');
                for (int x = 0; x < p3r.Length; x++)
                {
                    if (itm[1].ToString().Contains(p3r[x].Trim()))
                    {
                        // Item/Comp 3 contains a planet that is also in the list for Item/Comp 1
                        // Therefore we do not Need 3
                        Need3 = false;
                    }
                }
            }

            for (int x = 0; x < p2r.Length; x++)
            {
                if (itm[1].ToString().Contains(p2r[x].Trim()))
                    Need2 = false;
            }

            for (int x = 0; x < itm.Count; x++)
            {
                if ((x < 2) || ((x == 2) && (Need2)) || ((x == 3) && (Need3)))
                    retVal.Add(itm[x]);
            }

            return retVal;
        }

        private void cb_HLCommodities_CheckedChanged(object sender, EventArgs e)
        {
            GenerateAndFillP1Information();
            GenerateAndFillP2Information();
        }

        private void GenerateAndFillP1Information()
        {
            MaterialInfo mi;
            ListViewItem lvi;
            string line;

            foreach (Reaction r in PlugInData.Reactions.Values)
            {
                if (r.inputs.Count > 1)
                    continue;

                if (!P1Info.ContainsKey(r.outputs.Values[0].Name))
                {
                    mi = new MaterialInfo();
                    mi.inp1 = r.inputs.Values[0].Name;
                    mi.result = r.outputs.Values[0].Name;
                    if (PlugInData.Resources.ContainsKey(mi.inp1))
                        mi.foundAt = new ArrayList(PlugInData.Resources[mi.inp1].Planets);
                    else
                    {
                        foreach (var v in PlugInData.Resources)
                        {
                            if (mi.inp1.Contains(v.Key))
                                mi.foundAt = new ArrayList(v.Value.Planets);
                        }
                    }
                    if (CompIn.ContainsKey(mi.result))
                        mi.compIn = new ArrayList(CompIn[mi.result]);
                    else
                        mi.compIn.Add("POS / SoV Structures");

                    P1Info.Add(mi.result, mi);
                }
            }

            lv_P1.Items.Clear();

            foreach (MaterialInfo mt in P1Info.Values)
            {
                lvi = lv_P1.Items.Add(mt.result);
                lvi.SubItems.Add(mt.inp1);
                line = "";
                foreach (string s in mt.foundAt)
                {
                    if (line.Length > 0)
                    {
                        line += " | " + s;
                    }
                    else
                    {
                        line = s;
                    }
                }
                lvi.SubItems.Add(line);
                line = "";
                foreach (string s in mt.compIn)
                {
                    if (line.Length > 0)
                    {
                        line += ", " + s;
                    }
                    else
                    {
                        line = s;
                    }
                }
                lvi.SubItems.Add(line);

                if (cb_HLCommodities.Checked)
                {
                    if (CheckOVMatsHasItem(mt.result))
                        lvi.BackColor = Color.Yellow;
                }
                else
                    lvi.BackColor = Color.White;
            }
        }

        private void GenerateAndFillP2Information()
        {
            MaterialInfo mi;
            ListViewItem lvi;
            ListViewItem.ListViewSubItem lvsi;
            string line;

            foreach (Reaction r in PlugInData.Reactions.Values)
            {
                // P2 Page

                if ((r.inputs.Count == 2) && (r.inputs.Values[0].Qty > 10))
                {
                    if (!P2Info.ContainsKey(r.outputs.Values[0].Name))
                    {
                        mi = new MaterialInfo();
                        mi.inp1 = r.inputs.Values[0].Name;
                        mi.inp2 = r.inputs.Values[1].Name;
                        mi.result = r.outputs.Values[0].Name;

                        if (CompIn.ContainsKey(mi.result))
                            mi.compIn = new ArrayList(CompIn[mi.result]);
                        else
                            mi.compIn.Add("POS / SoV Structures");
                        P2Info.Add(mi.result, mi);
                    }

                    lv_P2.Items.Clear();

                    foreach (MaterialInfo mt in P2Info.Values)
                    {
                        lvi = lv_P2.Items.Add(mt.result);
                        lvsi = lvi.SubItems.Add(mt.inp1);
                        if (cb_HLCommodities.Checked)
                        {
                            if (CheckOVMatsHasItem(mt.inp1))
                                lvsi.BackColor = Color.Yellow;
                        }
                        else
                            lvsi.BackColor = Color.White;

                        lvsi = lvi.SubItems.Add(mt.inp2);
                        if (cb_HLCommodities.Checked)
                        {
                            if (CheckOVMatsHasItem(mt.inp2))
                                lvsi.BackColor = Color.Yellow;
                        }
                        else
                            lvsi.BackColor = Color.White;

                        if (P2RsrcLoc.ContainsKey(mt.result))
                        {
                            line = (string)P2RsrcLoc[mt.result][0];
                            if (line.Equals(""))
                            {
                                line = "{" + (string)P2RsrcLoc[mt.result][1] + "}";
                                line += " + ";
                                line += "{" + (string)P2RsrcLoc[mt.result][2] + "}";
                            }

                            lvi.SubItems.Add(line);
                        }

                        line = "";
                        foreach (string s in mt.compIn)
                        {
                            if (line.Length > 0)
                            {
                                line += ", " + s;
                            }
                            else
                            {
                                line = s;
                            }
                        }

                        if (cb_HLCommodities.Checked)
                        {
                            if (CheckOVMatsHasItem(mt.result))
                                lvi.BackColor = Color.LightBlue;
                        }
                        else
                            lvi.BackColor = Color.White;
                        
                        lvi.SubItems.Add(line);
                    }
                }
                else if ((r.inputs.Count >= 2) && (r.inputs.Values[0].Qty == 10))
                {
                    // P3 Page

                    if (!P3Info.ContainsKey(r.outputs.Values[0].Name))
                    {
                        mi = new MaterialInfo();
                        mi.inp1 = r.inputs.Values[0].Name;
                        mi.inp2 = r.inputs.Values[1].Name;

                        if (r.inputs.Count > 2)
                            mi.inp3 = r.inputs.Values[2].Name;

                        mi.result = r.outputs.Values[0].Name;

                        if (CompIn.ContainsKey(mi.result))
                            mi.compIn = new ArrayList(CompIn[mi.result]);
                        else
                            mi.compIn.Add("POS / SoV Structures");
                        P3Info.Add(mi.result, mi);
                    }

                    lv_P3.Items.Clear();

                    foreach (MaterialInfo mt in P3Info.Values)
                    {
                        lvi = lv_P3.Items.Add(mt.result);
                        lvsi = lvi.SubItems.Add(mt.inp1);
                        if (cb_HLCommodities.Checked)
                        {
                            if (CheckOVMatsHasItem(mt.inp1))
                                lvsi.BackColor = Color.LightBlue;
                        }
                        else
                            lvsi.BackColor = Color.White;

                        lvsi = lvi.SubItems.Add(mt.inp2);
                        if (cb_HLCommodities.Checked)
                        {
                            if (CheckOVMatsHasItem(mt.inp2))
                                lvsi.BackColor = Color.LightBlue;
                        }
                        else
                            lvsi.BackColor = Color.White;

                        lvsi = lvi.SubItems.Add(mt.inp3);
                        if (cb_HLCommodities.Checked)
                        {
                            if (CheckOVMatsHasItem(mt.inp3))
                                lvsi.BackColor = Color.LightBlue;
                        }
                        else
                            lvsi.BackColor = Color.White;
                        

                        if (P3RsrcLoc.ContainsKey(mt.result))
                        {
                            line = P3RsrcLoc[mt.result];

                            lvi.SubItems.Add(line);
                        }
                        line = "";
                        foreach (string s in mt.compIn)
                        {
                            if (line.Length > 0)
                            {
                                line += ", " + s;
                            }
                            else
                            {
                                line = s;
                            }
                        }

                        if (cb_HLCommodities.Checked)
                        {
                            if (CheckOVMatsHasItem(mt.result))
                                lvi.BackColor = Color.LimeGreen;
                        }
                        else
                            lvi.BackColor = Color.White;

                        lvi.SubItems.Add(line);
                    }
                }
                else if (r.outputs.Values[0].Qty == 1)
                {
                    // P4 's

                    if (!P4Info.ContainsKey(r.outputs.Values[0].Name))
                    {
                        mi = new MaterialInfo();
                        mi.inp1 = r.inputs.Values[0].Name;
                        mi.inp2 = r.inputs.Values[1].Name;

                        if (r.inputs.Count > 2)
                            mi.inp3 = r.inputs.Values[2].Name;

                        mi.result = r.outputs.Values[0].Name;

                        if (CompIn.ContainsKey(mi.result))
                            mi.compIn = new ArrayList(CompIn[mi.result]);
                        else
                            mi.compIn.Add("POS / SoV Structures");
                        P4Info.Add(mi.result, mi);
                    }

                    lv_P4.Items.Clear();

                    foreach (MaterialInfo mt in P4Info.Values)
                    {
                        lvi = lv_P4.Items.Add(mt.result);
                        lvsi = lvi.SubItems.Add(mt.inp1);
                        if (cb_HLCommodities.Checked)
                        {
                            if (CheckOVMatsHasItem(mt.inp1))
                                lvsi.BackColor = Color.LimeGreen;
                        }
                        else
                            lvsi.BackColor = Color.White;

                        lvsi = lvi.SubItems.Add(mt.inp2);
                        if (cb_HLCommodities.Checked)
                        {
                            if (CheckOVMatsHasItem(mt.inp2))
                                if (mt.inp2.Equals("Reactive Metals") || mt.inp2.Equals("Bacteria") || mt.inp2.Equals("Water"))
                                    lvsi.BackColor = Color.Yellow;
                                else
                                    lvsi.BackColor = Color.LimeGreen;

                        }
                        else
                            lvsi.BackColor = Color.White;

                        lvsi = lvi.SubItems.Add(mt.inp3);
                        if (cb_HLCommodities.Checked)
                        {
                            if (CheckOVMatsHasItem(mt.inp3))
                                if (mt.inp3.Equals("Reactive Metals") || mt.inp3.Equals("Bacteria") || mt.inp3.Equals("Water"))
                                    lvsi.BackColor = Color.Yellow;
                                else
                                    lvsi.BackColor = Color.LimeGreen;
                        }
                        else
                            lvsi.BackColor = Color.White;


                        if (P4RsrcLoc.ContainsKey(mt.result))
                        {
                            line = P4RsrcLoc[mt.result];
                            lvi.SubItems.Add(line);
                        }
                        line = "";
                        foreach (string s in mt.compIn)
                        {
                            if (line.Length > 0)
                            {
                                line += ", " + s;
                            }
                            else
                            {
                                line = s;
                            }
                        }

                        if (cb_HLCommodities.Checked)
                        {
                            if (CheckOVMatsHasItem(mt.result))
                                lvi.BackColor = Color.Orange;
                        }
                        else
                            lvi.BackColor = Color.White;

                        lvi.SubItems.Add(line);
                    }
                }
            }
        }

        public bool CheckOVMatsHasItem(string name)
        {
            foreach (var ml in OVMats.Values)
            {
                if (ml.ContainsKey(name))
                    return true;
            }

            return false;
        }

        #endregion

        #region Product Cost Matrix

        private void BuildPNumLists()
        {
            foreach (Reaction r in PlugInData.Reactions.Values)
            {
                if ((r.inputs.Count == 1) && (r.inputs.Values[0].Qty > 40))
                {
                    // P0 & P1
                    P0s.Add(r.reactName, r.inputs.Values[0].Qty);
                    P1s.Add(r.reactName, r.outputs.Values[0].Qty);
                }
                else if ((r.inputs.Count == 2) && (r.outputs.Values[0].Qty == 5))
                {
                    // P2
                    P2s.Add(r.reactName, r.outputs.Values[0].Qty);
                }
                else if ((r.inputs.Count >= 2) && (r.outputs.Values[0].Qty == 3))
                {
                    // P3
                    P3s.Add(r.reactName, r.outputs.Values[0].Qty);
                }
                else if ((r.inputs.Count > 2) && (r.outputs.Values[0].Qty == 1))
                {
                    // P4
                    P4s.Add(r.reactName, r.outputs.Values[0].Qty);
                }
            }
        }

        private void BuildComboTree(DevComponents.DotNetBar.Controls.ComboTree Tree)
        {
            DevComponents.AdvTree.Node P1n, P2n, P3n, P4n, Sn;

            Tree.Nodes.Clear();

            P1n = new DevComponents.AdvTree.Node();
            P2n = new DevComponents.AdvTree.Node();
            P3n = new DevComponents.AdvTree.Node();
            P4n = new DevComponents.AdvTree.Node();

            P4n.Text = "P4 Products";
            P4n.Selectable = false;
            P4n.Expanded = false;
            P3n.Text = "P3 Products";
            P3n.Selectable = false;
            P3n.Expanded = false;
            P2n.Text = "P2 Products";
            P2n.Selectable = false;
            P2n.Expanded = false;
            P1n.Text = "P1 Products";
            P1n.Selectable = false;
            P1n.Expanded = false;

            foreach (var p in P4s)
            {
                Sn = new DevComponents.AdvTree.Node();
                Sn.Text = p.Key;
                P4n.Nodes.Add(Sn);
            }
            Tree.Nodes.Add(P4n);

            foreach (var p in P3s)
            {
                Sn = new DevComponents.AdvTree.Node();
                Sn.Text = p.Key;
                P3n.Nodes.Add(Sn);
            }
            Tree.Nodes.Add(P3n);

            foreach (var p in P2s)
            {
                Sn = new DevComponents.AdvTree.Node();
                Sn.Text = p.Key;
                P2n.Nodes.Add(Sn);
            }
            Tree.Nodes.Add(P2n);

            foreach (var p in P1s)
            {
                Sn = new DevComponents.AdvTree.Node();
                Sn.Text = p.Key;
                P1n.Nodes.Add(Sn);
            }
            Tree.Nodes.Add(P1n);
        }

        private void BuildProductComboTrees()
        {
            BuildPNumLists();
            BuildComboTree(ct_SelectProduct1);
            BuildComboTree(ct_SelectProduct2);
        }

        private void ct_SelectProduct1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildProductCostTree1(ct_SelectProduct1.Text, false);
        }

        private void ct_SelectProduct2_SelectedIndexChanged(object sender, EventArgs e)
        {
            BuildProductCostTree2(ct_SelectProduct2.Text, false);
        }

        private void BuildProductCostTree1(string prod, bool update)
        {
            double Qty;
            ArrayList Nodes = new ArrayList();
            DevComponents.AdvTree.Node atn;
            DevComponents.AdvTree.Cell atc;

            Qty = Convert.ToDouble(nud_NumProduct1.Value);

            if (!PlugInData.Reactions.ContainsKey(prod))
                return;

            Reaction r = PlugInData.Reactions[prod];

            atn = new DevComponents.AdvTree.Node();
            atn.Text = r.outputs.Values[0].Name;
            atn.CheckBoxVisible = false;
            atn.Checked = false;
            atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", r.outputs.Values[0].Qty * Qty));
            atc.StyleNormal = at_PCost1.Styles["es_CenterAlign"];
            atn.Cells.Add(atc);
            atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#} isk", (EveHQ.Core.DataFunctions.GetPrice(r.outputs.Values[0].ID.ToString())))); // * r.outputs.Values[0].Qty * Qty
            atc.StyleNormal = at_PCost1.Styles["es_RightAlign"];
            atn.Cells.Add(atc);

            if (r.inputs.Count > 0)
            {
                Nodes = GetComponentsForTVItem(r.reactName);
            }
            // Now I have my components, time to populate the Data Grid
            foreach (LineItem li in Nodes)
            {
                BuildProduct1CostNode(li, atn, Qty, update);
            }

            if ((at_PCost1.Nodes.Count > 0) && update && (at_PCost1.Nodes[0].Expanded))
                atn.Expand();

            at_PCost1.Nodes.Clear();
            at_PCost1.Nodes.Add(atn);

            if (!update)
                at_PCost1.ExpandAll();

            UpdatePC1ItemTotals();
        }
        
        private void BuildProductCostTree2(string prod, bool update)
        {
            double Qty;
            ArrayList Nodes = new ArrayList();
            DevComponents.AdvTree.Node atn;
            DevComponents.AdvTree.Cell atc;

            Qty = Convert.ToDouble(nud_NumProduct2.Value);

            if (!PlugInData.Reactions.ContainsKey(prod))
                return;

            Reaction r = PlugInData.Reactions[prod];

            atn = new DevComponents.AdvTree.Node();
            atn.Text = r.outputs.Values[0].Name;
            atn.CheckBoxVisible = false;
            atn.Checked = false;
            atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", r.outputs.Values[0].Qty * Qty));
            atc.StyleNormal = at_PCost2.Styles["es_CenterAlign"];
            atn.Cells.Add(atc);
            atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#} isk", (EveHQ.Core.DataFunctions.GetPrice(r.outputs.Values[0].ID.ToString())))); // * r.outputs.Values[0].Qty * Qty
            atc.StyleNormal = at_PCost2.Styles["es_RightAlign"];
            atn.Cells.Add(atc);

            if (r.inputs.Count > 0)
            {
                Nodes = GetComponentsForTVItem(r.reactName);
            }
            // Now I have my components, time to populate the Data Grid
            foreach (LineItem li in Nodes)
            {
                BuildProduct2CostNode(li, atn, Qty, update);
            }

            if ((at_PCost2.Nodes.Count > 0) && update && (at_PCost2.Nodes[0].Expanded))
                atn.Expand();

            at_PCost2.Nodes.Clear();
            at_PCost2.Nodes.Add(atn);

            if (!update)
                at_PCost2.ExpandAll();

            UpdatePC2ItemTotals();
        }

        private void BuildProduct1CostNode(LineItem li, DevComponents.AdvTree.Node rn, double Qty, bool update)
        {
            DevComponents.AdvTree.Node atn;
            DevComponents.AdvTree.Cell atc;

            atn = new DevComponents.AdvTree.Node();
            atn.Text = li.Name;
            atn.CheckBoxVisible = true;

            if (update && IsCurrentNodeChecked(at_PCost1.Nodes[0], li.Name))
                atn.Checked = true;
            else
                atn.Checked = false;

            atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", li.Qty * Qty));
            atc.StyleNormal = at_PCost1.Styles["es_CenterAlign"];
            atn.Cells.Add(atc);
            atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#} isk", (li.BuyCost))); // * li.Qty * Qty
            atc.StyleNormal = at_PCost1.Styles["es_RightAlign"];
            atn.Cells.Add(atc);

            foreach (LineItem it in li.MadeFrom.Values)
            {
                BuildProduct1CostNode(it, atn, Qty, update);
            }

            if ((at_PCost1.Nodes.Count>0) &&  IsCurrentNodeExpanded(at_PCost1.Nodes[0], li.Name))
                atn.Expand();

            rn.Nodes.Add(atn);
        }

        private void BuildProduct2CostNode(LineItem li, DevComponents.AdvTree.Node rn, double Qty, bool update)
        {
            DevComponents.AdvTree.Node atn;
            DevComponents.AdvTree.Cell atc;

            atn = new DevComponents.AdvTree.Node();
            atn.Text = li.Name;
            atn.CheckBoxVisible = true;

            if (update && IsCurrentNodeChecked(at_PCost2.Nodes[0], li.Name))
                atn.Checked = true;
            else
                atn.Checked = false;

            atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#}", li.Qty * Qty));
            atc.StyleNormal = at_PCost2.Styles["es_CenterAlign"];
            atn.Cells.Add(atc);
            atc = new DevComponents.AdvTree.Cell(String.Format("{0:#,0.#} isk", (li.BuyCost))); // * li.Qty * Qty
            atc.StyleNormal = at_PCost2.Styles["es_RightAlign"];
            atn.Cells.Add(atc);

            foreach (LineItem it in li.MadeFrom.Values)
            {
                BuildProduct2CostNode(it, atn, Qty, update);
            }

            if ((at_PCost2.Nodes.Count > 0) && IsCurrentNodeExpanded(at_PCost2.Nodes[0], li.Name))
                atn.Expand();

            rn.Nodes.Add(atn);
        }

        private bool IsCurrentNodeChecked(DevComponents.AdvTree.Node RN, string nm)
        {
            foreach (DevComponents.AdvTree.Node nd in RN.Nodes)
            {
                if ((nd.Checked == true) && (nd.Text == nm))
                {
                    return true;
                }
                else if (nd.Nodes.Count > 0)
                {
                    if (IsCurrentNodeChecked(nd, nm))
                        return true;
                }
            }

            return false;
        }

        private bool IsCurrentNodeExpanded(DevComponents.AdvTree.Node RN, string nm)
        {
            foreach (DevComponents.AdvTree.Node nd in RN.Nodes)
            {
                if ((nd.Expanded) && (nd.Text == nm))
                {
                    return true;
                }
                else if (nd.Nodes.Count > 0)
                {
                    if (IsCurrentNodeExpanded(nd, nm))
                        return true;
                }
            }

            return false;
        }

        private void GetComponentsForItem(string item, bool Prod1, int lvl)
        {
            LineItem Comp = new LineItem();
            Reaction r;
            string hdr = "";

            item = item.TrimStart('-');

            if (!PlugInData.Reactions.ContainsKey(item))
                return;

            for (int x = 0; x < lvl; x++)
                hdr += "  ";
            hdr += "-";

            r = PlugInData.Reactions[item];

            foreach (Component c in r.inputs.Values)
            {
                // Read in direct inputs here
                Comp = new LineItem();
                Comp.ID = c.ID;
                Comp.Name = hdr + c.Name;
                Comp.Qty = c.Qty;
                Comp.BuyIt = false;
                Comp.BuyCost = EveHQ.Core.DataFunctions.GetPrice(Comp.ID.ToString());

                if (Prod1)
                    Prod1LineItems.Add(Comp);
                else
                    Prod2LineItems.Add(Comp);

                if (PlugInData.Reactions.ContainsKey(c.Name))
                {
                    GetComponentsForItem(c.Name, Prod1, lvl+1);
                }
            }
        }

        private void nud_NumProduct1_ValueChanged(object sender, EventArgs e)
        {
            BuildProductCostTree1(ct_SelectProduct1.Text, true);
        }

        private void nud_NumProduct2_ValueChanged(object sender, EventArgs e)
        {
            BuildProductCostTree2(ct_SelectProduct2.Text, true);
        }

        private void at_PCost1_AfterCheck(object sender, DevComponents.AdvTree.AdvTreeCellEventArgs e)
        {
            UpdatePC1ItemTotals();
        }

        private void at_PCost2_AfterCheck(object sender, DevComponents.AdvTree.AdvTreeCellEventArgs e)
        {
            UpdatePC2ItemTotals();
        }

        private void UpdatePC1ItemTotals()
        {
            double ItmCost = 0, ItmQty = 0, ItemCost = 0, Profit = 0;

            ItmCost = Convert.ToDouble(at_PCost1.Nodes[0].Cells[2].Text.Replace(" isk", ""));
            ItmQty = Convert.ToDouble(at_PCost1.Nodes[0].Cells[1].Text);

            ItemCost = ItmCost * ItmQty;

            ScanAndUpdatePC1Totals(at_PCost1.Nodes[0]);

            Profit = ItemCost - CompCost_1;
            lb_Profit1.Text = String.Format("{0:#,0.#} isk", (Profit));
            lb_MarketCost1.Text = String.Format("{0:#,0.#} isk", (ItemCost));
            lb_BuildCost1.Text = String.Format("{0:#,0.#} isk", (CompCost_1));
            lb_CompBuy1.Text = String.Format("{0:#,0.#} isk", (CompCost_1));
        }

        private void UpdatePC2ItemTotals()
        {
            double ItmCost = 0, ItmQty = 0, ItemCost = 0, Profit = 0;

            ItmCost = Convert.ToDouble(at_PCost2.Nodes[0].Cells[2].Text.Replace(" isk", ""));
            ItmQty = Convert.ToDouble(at_PCost2.Nodes[0].Cells[1].Text);

            ItemCost = ItmCost * ItmQty;

            ScanAndUpdatePC1Totals(at_PCost2.Nodes[0]);

            Profit = ItemCost - CompCost_1;
            lb_Profit2.Text = String.Format("{0:#,0.#} isk", (Profit));
            lb_MarketCost2.Text = String.Format("{0:#,0.#} isk", (ItemCost));
            lb_BuildCost2.Text = String.Format("{0:#,0.#} isk", (CompCost_1));
            lb_CompBuy2.Text = String.Format("{0:#,0.#} isk", (CompCost_1));
        }

        private void ScanAndUpdatePC1Totals(DevComponents.AdvTree.Node RNode)
        {
            double ItmCost = 0, ItmQty = 0;
            
            CompCost_1 = 0;

            foreach (DevComponents.AdvTree.Node nd in RNode.Nodes)
            {
                // calculate the tree totals based upon node checkboxes for Costs
                ItmCost = Convert.ToDouble(nd.Cells[2].Text.Replace(" isk", ""));
                ItmQty = Convert.ToDouble(nd.Cells[1].Text);
                if (nd.Checked)
                    CompCost_1 += (ItmCost * ItmQty);

                // Scan each node and sub-node in the tree
                ScanAndUpdatePC1NodeTotals(nd);
            }
        }

        private void ScanAndUpdatePC1NodeTotals(DevComponents.AdvTree.Node RNode)
        {
            double ItmCost = 0, ItmQty = 0;

            foreach (DevComponents.AdvTree.Node nd in RNode.Nodes)
            {
                // calculate the tree totals based upon node checkboxes for Costs
                ItmCost = Convert.ToDouble(nd.Cells[2].Text.Replace(" isk", ""));
                ItmQty = Convert.ToDouble(nd.Cells[1].Text);
                if (nd.Checked)
                    CompCost_1 += (ItmCost * ItmQty);

                // Scan each node and sub-node in the tree
                if (nd.Nodes.Count > 0)
                {
                    foreach (DevComponents.AdvTree.Node sn in nd.Nodes)
                        ScanAndUpdatePC1NodeTotals(sn);
                }
            }
        }

        private ArrayList GetComponentsForTVItem(string item)
        {
            ArrayList RootNodes = new ArrayList();
            LineItem Comp = new LineItem();
            Reaction r;

            if (!PlugInData.Reactions.ContainsKey(item))
                return RootNodes;
            else
                r = PlugInData.Reactions[item];

            // For Each Root Component that Makes up the Item
            foreach (Component c in r.inputs.Values)
            {
                // Read in direct inputs here
                Comp = new LineItem();
                Comp.ID = c.ID;
                Comp.Name = c.Name;
                Comp.Qty = c.Qty;
                Comp.BuyIt = false;
                Comp.BuyCost = EveHQ.Core.DataFunctions.GetPrice(Comp.ID.ToString());

                if (PlugInData.Reactions.ContainsKey(c.Name))
                {
                    // This component has Sub-Components, Go Get EM!
                    GetSubComponentsForTVItem(c.Name, Comp);
                }
                RootNodes.Add(Comp);
            }

            return RootNodes;
        }

        private LineItem GetSubComponentsForTVItem(string sItem, LineItem item)
        {
            Reaction r;
            LineItem SubComp = new LineItem();
            int mult = 1;

            r = PlugInData.Reactions[sItem];

            // For Each Root Component that Makes up the Item
            foreach (Component c in r.inputs.Values)
            {
                // Read in direct inputs here
                mult = item.Qty / r.outputs.Values[0].Qty;

                SubComp = new LineItem();
                SubComp.ID = c.ID;
                SubComp.Name = c.Name;
                SubComp.Qty = c.Qty * mult;
                SubComp.BuyIt = false;
                SubComp.BuyCost = EveHQ.Core.DataFunctions.GetPrice(SubComp.ID.ToString());

                if (PlugInData.Reactions.ContainsKey(c.Name))
                {
                    // This component has Sub-Components, Go Get EM!
                    GetSubComponentsForTVItem(c.Name, SubComp);
                }
                item.MadeFrom.Add(c.Name, SubComp);
            }

            return item;
        }

        #endregion

        #region Planet Facility Mk2

        private void UpdateWorkingModuleCount()
        {
            // Check to see if it needs updating
            if (Working.numMods <= 0)
            {
                Working.numMods = 0;
                Working.numMods += Working.Extractors.Count;
                Working.numMods += Working.Storage.Count;
                Working.numMods += Working.LaunchPad.Count;
            }
            foreach (Processor p in Working.Processors.Values)
                Working.numMods += p.Qty;
        }

        private void gp_Planner_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Bitmap p, l, s, ex;
                Planet pl;
                ArrayList ToolTips = new ArrayList();
                string tip;
                AddModule AM;

                if (Working == null)
                    return;

                pl = PlugInData.GetPlanetData(Working.PlanetType);

                SelectedMod = 0;
                // Case for adding a new Facility to the Plan
                // I need : Processor, Launch Pad, Storage, and Extractor Images
                ex = GetImageForItem(pl.Extractors.Values[0].typeID);
                tip = "CPU: " + pl.ECU.CPU;
                tip += "\n PG: " + pl.ECU.Power;
                tip += "\nHead CPU: " + pl.ECU.Head_CPU;
                tip += "\nHead  PG: " + pl.ECU.Head_Power;
                ToolTips.Add(tip);
                //ex = GetImageForItem(pl.ECU.typeID);
                p = GetImageForItem(pl.Processors.Values[0].typeID);
                tip = "";
                foreach (Processor pr in pl.Processors.Values)
                {
                    if (pr.Name.Contains("Basic"))
                    {
                        if (tip.Contains("Bse"))
                            continue;

                        if (tip.Length == 0)
                            tip = "Bse CPU: " + pr.CPU;
                        else
                            tip += "\nBse CPU: " + pr.CPU;
                        tip += "\nBse  PG: " + pr.Power;
                    }
                    else if (pr.Name.Contains("Advanced"))
                    {
                        if (tip.Contains("Adv"))
                            continue;

                        if (tip.Length == 0)
                            tip = "Adv CPU: " + pr.CPU;
                        else
                            tip += "\nAdv CPU: " + pr.CPU;
                        tip += "\nAdv  PG: " + pr.Power;
                    }
                    else
                    {
                        if (tip.Length == 0)
                            tip = "HT  CPU: " + pr.CPU;
                        else
                            tip += "\nHT  CPU: " + pr.CPU;
                        tip += "\nHT   PG: " + pr.Power;
                    }
                }
                ToolTips.Add(tip);
                s = GetImageForItem(pl.Storage.Values[0].typeID);
                tip = "CPU: " + pl.Storage.Values[0].CPU;
                tip += "\n PG: " + pl.Storage.Values[0].Power;
                ToolTips.Add(tip);
                l = GetImageForItem(pl.LaunchPads.Values[0].typeID);
                tip = "CPU: " + pl.LaunchPads.Values[0].CPU;
                tip += "\n PG: " + pl.LaunchPads.Values[0].Power;
                ToolTips.Add(tip);

                AM = new AddModule(this, ex, p, s, l, ToolTips);
                Point ptoc = new Point(e.X, e.Y);
                Point clientp = this.gp_Planner.PointToScreen(new Point(e.X, e.Y));
                AM.Location = clientp;
                AM.ShowDialog();

                if (SelectedMod > 0)
                    AddSelectedModuleTypeToFacility(ptoc);
            }

            if (movingControl != null)
            {
                FacUpdated = true;
                PlugInData.SavePIFacilitiesToDisk();
                movingControl = null;
            }
        }

        private void AddSelectedModuleTypeToFacility(Point posP)
        {
            Planet pl;
            UpdateWorkingModuleCount();
            pl = PlugInData.GetPlanetData(Working.PlanetType);

            UpdatingSF = true;
            switch (SelectedMod)
            {
                case 1:     // Extractor
                    if (pl.Extractors.Count > 0)
                    {
                        //ExtControlUnit ecu = new ExtControlUnit();
                        Extractor ext = new Extractor(pl.Extractors.Values[0]);
                        Working.numMods++;
                        ext.Location = "Extractor_" + Working.numMods;
                        ext.ID = Working.numMods;
                        ext.CLoc = new Point(posP.X, posP.Y);
                        Working.Extractors.Add(ext.Location, ext);
                        nud_NumLinks.Value++;
                        UpdatingSF = false;
                        ComputeAndUpdatePIFacilityData(UpdateType.Extractor);

                        Extractor_Mod MD = new Extractor_Mod(this, ext.Location);
                        MD.Location = new Point(posP.X, posP.Y);
                        MD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CommandCenter_OnMouseDown);

                        gp_Planner.Controls.Add(MD);
                    }
                    break;
                case 5:     // ECU
                    if (pl.Extractors.Count > 0)
                    {
                        ExtControlUnit ecu = new ExtControlUnit(PlugInData.Planets[Working.PlanetType].ECU);
                        
                        Working.numMods++;
                        ecu.Location = "Extractor_" + Working.numMods;
                        ecu.ID = Working.numMods;
                        ecu.CLoc = new Point(posP.X, posP.Y);
                        Working.ECUs.Add(ecu.Location, ecu);
                        nud_NumLinks.Value++;
                        UpdatingSF = false;
                        ComputeAndUpdatePIFacilityData(UpdateType.Extractor);

                        ECU_Mod MD = new ECU_Mod(this, ecu.Location);
                        MD.Location = new Point(posP.X, posP.Y);
                        MD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CommandCenter_OnMouseDown);

                        gp_Planner.Controls.Add(MD);
                    }
                    break;
                case 2:     // Processor
                    // Set appropriate Available Reactions Here
                    foreach (Processor c in pl.Processors.Values)
                    {
                        if (c.Name.Contains("Basic"))
                        {
                            Processor pr = new Processor(c);
                            Working.numMods++;
                            pr.Location = "Processor_" + Working.numMods;
                            pr.ID = Working.numMods;
                            pr.ProcLevel = 1;
                            pr.CLoc = new Point(posP.X, posP.Y);
                            Working.Processors.Add(pr.Location, pr);
                            nud_NumLinks.Value++;
                            UpdatingSF = false;
                            ComputeAndUpdatePIFacilityData(UpdateType.Processor);

                            Processor_Mod MD = new Processor_Mod(this, pr.Location);
                            MD.Location = new Point(posP.X, posP.Y);
                            MD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CommandCenter_OnMouseDown);

                            gp_Planner.Controls.Add(MD);
                        }
                    }
                    break;
                case 3:     // Storage
                    if (pl.Storage.Count > 0)
                    {
                        StorageFacility sf = new StorageFacility(pl.Storage.Values[0]);
                        Working.numMods++;
                        sf.Location = "Storage_" + Working.numMods;
                        sf.ID = Working.numMods;
                        sf.CLoc = new Point(posP.X, posP.Y);
                        Working.Storage.Add(sf.Location, sf);
                        nud_NumLinks.Value++;
                        UpdatingSF = false;
                        ComputeAndUpdatePIFacilityData(UpdateType.Storage);

                        Storage_Mod MD = new Storage_Mod(this, sf.Location);
                        MD.Location = new Point(posP.X, posP.Y);
                        MD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CommandCenter_OnMouseDown);

                        gp_Planner.Controls.Add(MD);
                    }
                    break;
                case 4:     // Launching Pad
                    if (pl.LaunchPads.Count > 0)
                    {
                        Launchpad lp = new Launchpad(pl.LaunchPads.Values[0]);
                        Working.numMods++;
                        lp.Location = "Launchpad_" + Working.numMods;
                        lp.ID = Working.numMods;
                        lp.CLoc = new Point(posP.X, posP.Y);
                        Working.LaunchPad.Add(lp.Location, lp);
                        nud_NumLinks.Value++;
                        UpdatingSF = false;
                        ComputeAndUpdatePIFacilityData(UpdateType.Launchpad);

                        Launch_Pad MD = new Launch_Pad(this, lp.Location);
                        MD.Location = new Point(posP.X, posP.Y);
                        MD.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CommandCenter_OnMouseDown);

                        gp_Planner.Controls.Add(MD);
                    }
                    break;
                default:
                    break;
            }
        }

        private void CommandCenter_OnMouseDown(object sender, MouseEventArgs e)
        {
            movingControl = sender;
            ObjOffset = new Point(e.X, e.Y);
            DoDragDrop(sender, DragDropEffects.Move);
        }

        private void gp_Planner_DragOver(object sender, DragEventArgs e)
        {
            Point clientp = this.gp_Planner.PointToClient(new Point(e.X, e.Y));
            clientp.X -= ObjOffset.X;
            clientp.Y -= ObjOffset.Y;

            switch (movingControl.GetType().Name)
            {
                case "Command_Mod":
                    Command_Mod CMD = (Command_Mod)movingControl;
                    Working.Command.CLoc = clientp;
                    CMD.Location = clientp;
                    CMD.BringToFront();
                    break;
                case "Extractor_Mod":
                    Extractor_Mod EMD = (Extractor_Mod)movingControl;
                    if (Working.Extractors.ContainsKey(EMD.ExtName))
                        Working.Extractors[EMD.ExtName].CLoc = clientp;
                    EMD.Location = clientp;
                    EMD.BringToFront();
                    break;
                case "ECU_Mod":
                    ECU_Mod ECMD = (ECU_Mod)movingControl;
                    if (Working.ECUs.ContainsKey(ECMD.ExtName))
                        Working.ECUs[ECMD.ExtName].CLoc = clientp;
                    ECMD.Location = clientp;
                    ECMD.BringToFront();
                    break;
                case "Processor_Mod":
                    Processor_Mod PMD = (Processor_Mod)movingControl;
                    if (Working.Processors.ContainsKey(PMD.ProcName))
                        Working.Processors[PMD.ProcName].CLoc = clientp;
                    PMD.Location = clientp;
                    PMD.BringToFront();
                    break;
                case "Storage_Mod":
                    Storage_Mod SMD = (Storage_Mod)movingControl;
                    if (Working.Storage.ContainsKey(SMD.SName))
                        Working.Storage[SMD.SName].CLoc = clientp;
                    SMD.Location = clientp;
                    SMD.BringToFront();
                    break;
                case "Launch_Pad":
                    Launch_Pad LMD = (Launch_Pad)movingControl;
                    if (Working.LaunchPad.ContainsKey(LMD.LPName))
                        Working.LaunchPad[LMD.LPName].CLoc = clientp;
                    LMD.Location = clientp;
                    LMD.BringToFront();
                    break;
            }
            gp_Planner.Update();
        }

        public void ProcessorModuleChanged()
        {
            FacUpdated = true;
            ComputeAndUpdatePIFacilityData(UpdateType.All);
        }

        public void CommandCenterTypeChanged()
        {
            FacUpdated = true;
            ComputeAndUpdatePIFacilityData(UpdateType.ModSide);
        }

        public void ExtractorModuleChanged()
        {
            FacUpdated = true;
            ComputeAndUpdatePIFacilityData(UpdateType.ModSide);
        }

        public void ECUModuleChanged()
        {
            FacUpdated = true;
            ComputeAndUpdatePIFacilityData(UpdateType.All);
        }

        #endregion

      }
}
