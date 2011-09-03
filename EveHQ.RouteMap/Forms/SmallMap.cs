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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace EveHQ.RouteMap
{
    public partial class SmallMap : UserControl
    {
        private Bitmap miniMapBitmap;
        private Rectangle GlobalFlatArea;
        private SizeF MMScale;
        private Rectangle mmOutlineRect;
        private bool mmOutlineDragging;
        private Point mmOutlineDragOffset;
        //private Point mmDragOffset;
        private RouteMapMainForm RMMF;
        private bool Initialized = false;
        private bool whMode = false;
      
        public SmallMap()
        {
            InitializeComponent();
        }

        public void InitializeSmallMap(RouteMapMainForm frm)
        {
            RMMF = frm;
            GlobalFlatArea = new Rectangle();
            GetGlobalFlatArea();
            this.DoubleBuffered = true;
            Initialized = true;
        }

        private void SmallMap_Resize(object sender, EventArgs e)
        {
            if (!Initialized)
                return;

            //GetGlobalFlatArea();
            GenerateMiniMapBitmap();
        }

        public void SmallMap_Refresh(bool whMd)
        {
            if (!Initialized)
                return;

            whMode = whMd;

            GetGlobalFlatArea();
            GenerateMiniMapBitmap();
        }

        // Routines to Draw map data
        private void GenerateMiniMapBitmap()
        {
            float mmAvgScale, alphaMult;
            int sysAlpha, gateAlpha;
            Pen JPNormal, JPConst, JPRegion, sysPen;
            StarGate gate;
            SolarSystem To, From;

            if ((this == null) || (this.Width <= 0) || (this.Height <= 0))
                return;

            try
            {
                miniMapBitmap = new Bitmap(this.Width, this.Height);
            }
            catch
            {
                return;
            }

            Graphics g = Graphics.FromImage(miniMapBitmap);
            g.SmoothingMode = SmoothingMode.HighQuality;

            g.Clear(PlugInData.Config.MapColors["Background"]);

            SizeF mapSize = new SizeF(GlobalFlatArea.Width + 2, GlobalFlatArea.Height + 2);
            MMScale = new SizeF((this.Width - 2) / mapSize.Width, (this.Height - 2) / mapSize.Height);

            mmAvgScale = (MMScale.Width + MMScale.Height) / 2;
            alphaMult = mmAvgScale * 65;

            sysAlpha = (int)(0x30 * alphaMult);
            gateAlpha = (int)(0x60 * alphaMult);

            if (sysAlpha > 0xFF)
                sysAlpha = 0xFF;
            if (gateAlpha > 0xFF)
                gateAlpha = 0xFF;

            JPNormal = new Pen(Color.FromArgb(Math.Abs(gateAlpha), PlugInData.Config.MapColors["Normal Gate"]), 1);
            JPConst = new Pen(PlugInData.Config.MapColors["Constellation Gate"], 1);
            JPRegion = new Pen(PlugInData.Config.MapColors["Region Gate"], 1);

            if (!whMode)
            {
                for (int i = 0; i < PlugInData.GalMap.GalData.StarGates.Count; i++)
                {
                    gate = PlugInData.GalMap.GalData.StarGates[i];
                    To = gate.To;
                    From = gate.From;

                    float dx = From.OrgCoord.X - To.OrgCoord.X;
                    float dy = From.OrgCoord.Y - To.OrgCoord.Y;
                    float length = (float)Math.Sqrt(dx * dx + dy * dy);

                    if (length * mmAvgScale > 1)
                    {
                        Pen gPen = null;
                        switch (gate.Type)
                        {
                            case GateType.Normal:
                                gPen = JPNormal;
                                break;
                            case GateType.InterConst:
                                gPen = JPConst;
                                break;
                            case GateType.InterRegion:
                                gPen = JPRegion;
                                break;
                        }

                        float dpx1 = (From.OrgCoord.X - GlobalFlatArea.Left) * MMScale.Width;
                        float dpy1 = (From.OrgCoord.Y - GlobalFlatArea.Top) * MMScale.Height;
                        float dpx2 = (To.OrgCoord.X - GlobalFlatArea.Left) * MMScale.Width;
                        float dpy2 = (To.OrgCoord.Y - GlobalFlatArea.Top) * MMScale.Height;

                        if (gPen != null)
                            g.DrawLine(gPen, dpx1, dpy1, dpx2, dpy2);
                    }
                }
            }

            foreach (Region r in PlugInData.GalMap.GalData.Regions.Values)
            {
                if (PlugInData.WHMapSelected)
                {
                    if (!PlugInData.WHRegions.Contains(r.ID))
                        continue;
                }
                else
                {
                    if (PlugInData.SkipRegions.Contains(r.ID))
                        continue;
                }

                sysPen = new Pen(Color.FromArgb(Math.Abs(sysAlpha), r.RColor));
                foreach (Constellation c in r.Constellations.Values)
                {
                    foreach (SolarSystem s in c.Systems.Values)
                    {
                        int dpx = (int)((s.OrgCoord.X - GlobalFlatArea.Left) * MMScale.Width);
                        int dpy = (int)((s.OrgCoord.Y - GlobalFlatArea.Top) * MMScale.Height);

                        g.DrawEllipse(sysPen, dpx, dpy, 1, 1);
                    }
                }
            }

            JPConst.Dispose();
            JPNormal.Dispose();
            JPRegion.Dispose();
            g.Dispose();
            //miniMapBitmap = (Bitmap)drawBitmap.Clone();
            this.BackgroundImage = miniMapBitmap;

            this.Invalidate();
        }

        private void GetGlobalFlatArea()
        {
            Point FlatMin = new Point(int.MaxValue, int.MaxValue);
            Point FlatMax = new Point(int.MinValue, int.MinValue);

            foreach (var curRegion in PlugInData.GalMap.GalData.Regions)
            {
                if (PlugInData.WHMapSelected)
                {
                    if (!PlugInData.WHRegions.Contains(curRegion.Value.ID))
                        continue;
                }
                else
                {
                    if (PlugInData.SkipRegions.Contains(curRegion.Value.ID))
                        continue;
                }

                if (curRegion.Value.Bounds.Left < FlatMin.X) FlatMin.X = curRegion.Value.Bounds.Left;
                if (curRegion.Value.Bounds.Right > FlatMax.X) FlatMax.X = curRegion.Value.Bounds.Right;
                if (curRegion.Value.Bounds.Top < FlatMin.Y) FlatMin.Y = curRegion.Value.Bounds.Top;
                if (curRegion.Value.Bounds.Bottom > FlatMax.Y) FlatMax.Y = curRegion.Value.Bounds.Bottom;
            }

            GlobalFlatArea = Rectangle.FromLTRB(FlatMin.X, FlatMin.Y, FlatMax.X, FlatMax.Y);
        }

        // Routines to handle mouse movement for selection of map display on main map

        // Routines to show what area of the Main map are visible on the Mini-map
        // Routines to accept an updated Main-map position to update correct area on mini-map
        public void UpdateSmallMapHighlight(int sbH, int sbHM, int sbV, int sbVM, Size mSize, float mpS)
        {
            Point origin;
            Size size;

            origin = new Point((int)(sbH * MMScale.Width), (int)(sbV * MMScale.Height));
            size = new Size((int)(mSize.Width / (sbHM * mpS) * (this.Width - 3)),
                             (int)(mSize.Height / (sbVM * mpS) * (this.Height - 3)));

            if (size.Width > this.Width - 3)
                size.Width = this.Width - 3;
            if (size.Height > this.Height - 3)
                size.Height = this.Height - 3;

            mmOutlineRect = new Rectangle(origin, size);

            this.Invalidate();
        }

        private void SmallMap_Paint(object sender, PaintEventArgs e)
        {
            if (!Initialized)
                return;

            e.Graphics.DrawRectangle(new Pen(Color.Red, 1), mmOutlineRect);
        }

        private void SmallMap_MouseDown(object sender, MouseEventArgs e)
        {
            mmOutlineDragging = true;
            if (mmOutlineRect.Contains(e.Location))
            {
                // Mouse / rectangle contains mouse pointer location
                mmOutlineDragOffset.X = e.Location.X - mmOutlineRect.Left;
                mmOutlineDragOffset.Y = e.Location.Y - mmOutlineRect.Top;
            }
            else
            {
                // Mouse point does not contain OutlineRect, so move OutlineRect to new point
                mmOutlineDragOffset.X = mmOutlineRect.Width / 2;
                mmOutlineDragOffset.Y = mmOutlineRect.Height / 2;

                Point newScroll = new Point(
                    (int)((e.Location.X - mmOutlineDragOffset.X) / MMScale.Width),
                    (int)((e.Location.Y - mmOutlineDragOffset.Y) / MMScale.Height));

                RMMF.MinimapMovedMap(newScroll);
            }
        }

        private void SmallMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (mmOutlineDragging)
            {
                Cursor = Cursors.Hand;

                Point newScroll = new Point(
                    (int)((e.Location.X  - mmOutlineDragOffset.X) / MMScale.Width),
                    (int)((e.Location.Y  - mmOutlineDragOffset.Y) / MMScale.Height));

                RMMF.MinimapMovedMap(newScroll);
            }
        }

        private void SmallMap_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
            if (mmOutlineDragging)
            {
                Point newScroll = new Point(
                    (int)((e.Location.X - mmOutlineDragOffset.X) / MMScale.Width),
                    (int)((e.Location.Y - mmOutlineDragOffset.Y) / MMScale.Height));

                RMMF.MinimapMovedMap(newScroll);
                mmOutlineDragging = false;
            }

        }
    }
}
