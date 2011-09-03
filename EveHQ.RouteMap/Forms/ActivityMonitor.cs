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
using ZedGraph;

namespace EveHQ.RouteMap
{
    public partial class ActivityMonitor : UserControl
    {
        public int SelActID = 0;
        public bool doingInit = false;

        public ActivityMonitor()
        {
            InitializeComponent();
        }

        private void PopulateJumpKillsGraph(int sID, string sysName, long clas)
        {
            PointPairList jd, sd, pd, fd;
            LineItem myCurve;
            DateTime timStamp, jTS = new DateTime(0001, 1, 1), kTS = new DateTime(0001, 1, 1);
            string lastAPI = "Unknown";
            int ov = 2;

            // get a reference to the GraphPane
            GraphPane myPane = zg_SysMon.GraphPane;

            SelActID = sID;

            myPane.CurveList.Clear();
            // Set the Titles
            if (clas >= 0)
                myPane.Title.Text = "System Activity for " + sysName + " Class - " + clas;
            else
                myPane.Title.Text = "System Activity for " + sysName;

            myPane.XAxis.Type = AxisType.Date;
            myPane.XAxis.Scale.MajorUnit = DateUnit.Hour;
            myPane.XAxis.Scale.Format = "T";
            myPane.YAxis.Title.Text = "Quantity";
            //myPane.YAxis.Type = AxisType.Linear;
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorGrid.Color = Color.DarkGray;
            myPane.YAxis.MajorGrid.Color = Color.DarkGray;
            myPane.YAxis.Type = AxisType.Log;
            myPane.Fill = new Fill(Color.Black);
            myPane.Chart.Fill = new Fill(Color.Black);
            myPane.Border.Color = Color.White;
            myPane.Legend.Fill = new Fill(Color.Black);
            myPane.Legend.FontSpec.FontColor = Color.DodgerBlue;
            myPane.Chart.Border.Color = Color.Transparent;
            myPane.Title.FontSpec.FontColor = Color.DodgerBlue;
            myPane.XAxis.Title.FontSpec.FontColor = Color.DodgerBlue;
            myPane.YAxis.Title.FontSpec.FontColor = Color.DodgerBlue;
            myPane.XAxis.Scale.FontSpec.FontColor = Color.DodgerBlue;
            myPane.YAxis.Scale.FontSpec.FontColor = Color.DodgerBlue;
            myPane.XAxis.Color = Color.DodgerBlue;
            myPane.YAxis.Color = Color.DodgerBlue;

            jd = new PointPairList();
            sd = new PointPairList();
            pd = new PointPairList();
            fd = new PointPairList();

            foreach (var skTS in PlugInData.JKList.Jumps)
            {
                timStamp = skTS.Key;
                if (skTS.Value.ContainsKey(sID))
                {
                    jd.Add(timStamp.ToOADate(), skTS.Value[sID] + ov);
                }
                else
                {
                    jd.Add(timStamp.ToOADate(), ov);
                }

                jTS = timStamp;
            }

            foreach (var skTS in PlugInData.JKList.Kills)
            {
                timStamp = skTS.Key;
                if (skTS.Value.ContainsKey(sID))
                {
                    sd.Add(timStamp.ToOADate(), Convert.ToInt32(skTS.Value[sID][0]) + ov);
                    pd.Add(timStamp.ToOADate(), Convert.ToInt32(skTS.Value[sID][1]) + ov);
                    fd.Add(timStamp.ToOADate(), Convert.ToInt32(skTS.Value[sID][2]) + ov);
                }
                else
                {
                    sd.Add(timStamp.ToOADate(), ov);
                    pd.Add(timStamp.ToOADate(), ov);
                    fd.Add(timStamp.ToOADate(), ov);
                }

                kTS = timStamp;
            }

            myCurve = myPane.AddCurve("System Jumps", jd, Color.Yellow, SymbolType.None);
            myCurve.Line.IsSmooth = true;
            myCurve.Line.SmoothTension = 0.1f;
            myCurve.Line.IsOptimizedDraw = true;

            myCurve = myPane.AddCurve("Ship Kills", sd, Color.Violet, SymbolType.None);
            myCurve.Line.IsSmooth = true;
            myCurve.Line.SmoothTension = 0.1f;
            myCurve.Line.IsOptimizedDraw = true;

            myCurve = myPane.AddCurve("Pod Kills", pd, Color.Red, SymbolType.None);
            myCurve.Line.IsSmooth = true;
            myCurve.Line.SmoothTension = 0.1f;
            myCurve.Line.IsOptimizedDraw = true;

            myCurve = myPane.AddCurve("NPC Kills", fd, Color.PaleGreen, SymbolType.None);
            myCurve.Line.IsSmooth = true;
            myCurve.Line.SmoothTension = 0.1f;
            myCurve.Line.IsOptimizedDraw = true;

            if (jTS.CompareTo(kTS) > 0)
                lastAPI = "J - " + jTS.ToString();
            else
                lastAPI = "K - " + kTS.ToString();

            myPane.XAxis.Title.Text = "Time in Hours (Last API - " + lastAPI + ")";

            zg_SysMon.AxisChange();
            zg_SysMon.Refresh();
        }

        private string zg_SysMon_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {
            string timeStp = "", Jumps = "", SKills = "", PKills = "", NKills = "", retString = "Unknown";
            DateTime jTm = new DateTime(0001, 1, 1);
            DateTime kTm = new DateTime(0001, 1, 1);

            // New Jump / Kills data has been processed
            // Need to update displays if a system is selected!
            if (!SelActID.Equals(0))
            {
                if (PlugInData.JKList.Jumps.Count > iPt)
                {
                    jTm = PlugInData.JKList.Jumps.ElementAt(iPt).Key;
                    if (PlugInData.JKList.Jumps.ElementAt(iPt).Value.ContainsKey(SelActID))
                    {
                        Jumps = "Jumps: " + PlugInData.JKList.Jumps.ElementAt(iPt).Value[SelActID];
                    }
                    else
                        Jumps = "Jumps: 0";
                }
                if (PlugInData.JKList.Kills.Count > iPt)
                {
                    kTm = PlugInData.JKList.Kills.ElementAt(iPt).Key;
                    if (PlugInData.JKList.Kills.ElementAt(iPt).Value.ContainsKey(SelActID))
                    {
                        SKills = "Ship Kills: " + PlugInData.JKList.Kills.ElementAt(iPt).Value[SelActID][0];
                        PKills = "Pod Kills: " + PlugInData.JKList.Kills.ElementAt(iPt).Value[SelActID][1];
                        NKills = "NPC Kills: " + PlugInData.JKList.Kills.ElementAt(iPt).Value[SelActID][2];
                    }
                    else
                    {
                        SKills = "Ship Kills: 0";
                        PKills = "Pod Kills: 0";
                        NKills = "NPC Kills: 0";
                    }
                }

                if (jTm.CompareTo(kTm) > 0)
                    timeStp = "J - " + jTm.ToString();
                else
                    timeStp = "K - " + kTm.ToString();

                retString = timeStp + "\n";
                if (!Jumps.Equals(""))
                    retString += Jumps + "\n";

                if (!SKills.Equals(""))
                {
                    retString += SKills + "\n";
                    retString += PKills + "\n";
                    retString += NKills + "\n";
                }
            }

            return retString;
        }

        public void UpdateJKGraph()
        {
            string sysNm;
            SolarSystem ss;

            if (doingInit)
                return;

            if (cb_SystemSelect.Text != "")
            {
                sysNm = cb_SystemSelect.Text;
                ss = PlugInData.GalMap.GetSystemByName(sysNm);

                if (ss != null)
                {
                    if (PlugInData.Misc.WHRegnToClass.ContainsKey(ss.RegionID))
                        PopulateJumpKillsGraph(ss.ID, sysNm, PlugInData.Misc.WHRegnToClass[ss.RegionID]);
                    else
                        PopulateJumpKillsGraph(ss.ID, sysNm, -1);
                }
            }
            else
                zg_SysMon.Enabled = false;
        }

        private void cb_SystemSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sysNm;
            SolarSystem ss;

            if (doingInit)
                return;

            sysNm = cb_SystemSelect.Text;
            zg_SysMon.Enabled = true;
            ss = PlugInData.GalMap.GetSystemByName(sysNm);

            if (PlugInData.Misc.WHRegnToClass.ContainsKey(ss.RegionID))
                PopulateJumpKillsGraph(ss.ID, sysNm, PlugInData.Misc.WHRegnToClass[ss.RegionID]);
            else
                PopulateJumpKillsGraph(ss.ID, sysNm, -1);

            if (PlugInData.Config.StrSet.ContainsKey(this.Name))
                PlugInData.Config.StrSet[this.Name] = sysNm;
            else
                PlugInData.Config.StrSet.Add(this.Name, sysNm);

            PlugInData.SaveConfigToDisk();
        }

        private void cb_SystemSelect_TextChanged(object sender, EventArgs e)
        {
            string sysNm;
            SolarSystem ss;

            if (doingInit)
                return;

            if (cb_SystemSelect.Text != "")
            {
                sysNm = cb_SystemSelect.Text;
                zg_SysMon.Enabled = true;
                ss = PlugInData.GalMap.GetSystemByName(sysNm);

                if (ss.ID != 0)
                {
                    if (PlugInData.Misc.WHRegnToClass.ContainsKey(ss.RegionID))
                        PopulateJumpKillsGraph(ss.ID, sysNm, PlugInData.Misc.WHRegnToClass[ss.RegionID]);
                    else
                        PopulateJumpKillsGraph(ss.ID, sysNm, -1);

                    if (PlugInData.Config.StrSet.ContainsKey(this.Name))
                        PlugInData.Config.StrSet[this.Name] = sysNm;
                    else
                        PlugInData.Config.StrSet.Add(this.Name, sysNm);

                    PlugInData.SaveConfigToDisk();
                }
            }
            else
                zg_SysMon.Enabled = false;
        }

        public void InitializeMonitor()
        {
            string amCS;

            doingInit = true;

            cb_SystemSelect.Items.AddRange(PlugInData.SystemList.ToArray());
            amCS = this.Name;

            if (PlugInData.Config.StrSet.ContainsKey(amCS))
                cb_SystemSelect.Text = PlugInData.Config.StrSet[amCS];
            
            // get a reference to the GraphPane
            GraphPane myPane = zg_SysMon.GraphPane;

            // Set the Titles
            myPane.Title.Text = "System Activity for ";
            myPane.XAxis.Scale.Format = "T";
            myPane.YAxis.Title.Text = "Quantity";
            //myPane.YAxis.Type = AxisType.Linear;
            myPane.Fill = new Fill(Color.Black);
            myPane.Chart.Fill = new Fill(Color.Black);
            myPane.Border.Color = Color.White;
            myPane.Legend.Fill = new Fill(Color.Black);
            myPane.Legend.FontSpec.FontColor = Color.DodgerBlue;
            myPane.Chart.Border.Color = Color.Transparent;
            myPane.Title.FontSpec.FontColor = Color.DodgerBlue;
            myPane.XAxis.Title.FontSpec.FontColor = Color.DodgerBlue;
            myPane.YAxis.Title.FontSpec.FontColor = Color.DodgerBlue;
            myPane.XAxis.Scale.FontSpec.FontColor = Color.DodgerBlue;
            myPane.YAxis.Scale.FontSpec.FontColor = Color.DodgerBlue;
            myPane.XAxis.Color = Color.DodgerBlue;
            myPane.YAxis.Color = Color.DodgerBlue;

            if (cb_SystemSelect.Text.Equals(""))
                zg_SysMon.Enabled = false;

            doingInit = false;

            UpdateJKGraph();
        }

        private void ActivityMonitor_Load(object sender, EventArgs e)
        {
        }
    }
}
