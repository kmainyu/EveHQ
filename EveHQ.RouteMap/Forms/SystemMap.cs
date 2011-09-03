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
    public partial class SystemMap : UserControl
    {
        private const float DEFAULT_CONST_FONT_SIZE = 32;
        private const float DEFAULT_REGION_FONT_SIZE = 92;
        private const float DEFAULT_SYSTEM_DETAIL_SIZE = 8;
        private const float DEFAULT_SYSTEM_FONT_SIZE = 8;
        private const double CUBE = 1.0 / 3.0;
        private const float MIN_ZOOM = 0.01f;
        private const float MAX_ZOOM = 10.0f;
        private const float DEF_ZOOM_INC = 0.01f;
        private const float FAST_ZOOM_INC = 1.0f;
        private const float SLOW_ZOOM_INC = 0.01f;
        private const double CEL_DIV = 10000000;
        private const float AU = 149597870691; // In meters
        //private const float AU_DIV = 3; // To get approx 600 pixels at max zoom w/ 200AU system
        private float AU_MULT = 100.0f;
        private Int32 SystemTime;
        private static NumberFormatInfo nfi;

        public bool ScrollEnable = true;
        public bool ResizeRedrawEn = false;
        public bool InitMapSettings = false;
        public bool FlatDataChanged = true;
        public bool FirstLoad = false;
        public float MapScale = 1.0f;
        public Size MapSize;
        private Point Adjust;
        private Rectangle GlobalFlatArea;
        private float _lineThickness;
        private Font _regionFont;
        private Font _systemBoldFont, _systemDetailFont;
        private Font _systemFont;
        private Font _constFont;
        private int _labelAlpha = 0x40;
        private int _lineAlpha = 0xFF;
        private bool _scrollEnabled = true;
        private Point _mapDrag;
        private bool _mapDragging;
        private Point _mapDragOffset;
        private bool _zoomEnabled;
        private bool _doingDistance = false;
        private ArrayList DistFrom = null;
        private ArrayList DistTo = null;
        private ArrayList SelectInfo;

        public delegate void SystemSelectedHandler();
        public SystemCelestials SysDat;
        private ConfigData2 Config = new ConfigData2();
        public bool MouseZoom = false;
        private RouteMapMainForm RMA;
        private Point MWCenter;
        private SolarSystem sSys;
        public bool GotData = false;
        public bool LocalUpdate = false;
        public SortedList<string, PointF> CelestialLocsByName;

        #region Init and Enums

        public enum RelativePosition
        {
            Top,
            TopLeft,
            TopRight,
            CenterLeft,
            CenterRight,
            Bottom,
            BottomLeft,
            BottomRight,
            Center,
            None
        };

        public SystemMap()
        {
            CelestialLocsByName = new SortedList<string, PointF>();
        }

        public void UpdateConfig(ConfigData2 cfg)
        {
            Config = cfg;
            Invalidate();
        }

        public void UpdateMapForChange()
        {
            Invalidate();
        }

        public void SetupSystemMap(ArrayList sl, ConfigData2 cfg, SolarSystem s, RouteMapMainForm rm)
        {
            Config = cfg;
            RMA = rm;
            sSys = s;
            SysDat = new SystemCelestials();
            SysDat.GetSystemCelestial(s);

            InitializeComponent();
            InitializeMapVeiw();

            LocalUpdate = true;
            cb_FindSystem.Text = sSys.Name;
            LocalUpdate = false;

            sb_HorizontalScroll.Visible = ScrollEnable;
            sb_VerticalScroll.Visible = ScrollEnable;

            tb_Zoom.Text = String.Format("{0:#,0.##}", MapScale);

            cb_FindSystem.Items.Clear();
            cb_FindSystem.Items.AddRange(sl.ToArray());

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            ResizeRedraw = false;
            MWCenter = new Point(0, 0);

            // Force string/double conversions to use "." as decimal point
            // InstalledUICulture is read-only so it needs to be copied and changed
            CultureInfo ci =
                CultureInfo.InstalledUICulture;
            nfi = (NumberFormatInfo)ci.NumberFormat.Clone();
            nfi.NumberDecimalSeparator = ".";

            Invalidate();
            GotData = true;
        }

        public void UpdateSystemMap(SolarSystem s)
        {
            if (s == null)
                return;

            sSys = s;
            SysDat = new SystemCelestials();
            SysDat.GetSystemCelestial(s);

            LocalUpdate = true;
            MapScale = 1.0f;
            SystemTime = -70900;
            if (cb_FindSystem != null)
                cb_FindSystem.Text = sSys.Name;

            LocalUpdate = false;

            //tb_Zoom.Text = String.Format("{0:#,0.##}", MapScale);

            ResizeRedraw = true;
            MWCenter = new Point(0, 0);

            Invalidate();
            CenterOnPoint(new PointF(0, 0));
        }


        public void InitializeMapVeiw()
        {
            InitMapSettings = true;
            FillAreaParams();
            MapScale = 1.0f;
            MapSize = ClientSize;
            SetGraphicsToScale(MapScale, false);
            CenterOnPoint(new PointF(0, 0));
        }

        public void SysMap_OnPaint(object sender, EventArgs e)
        {
            //if(GotData)
            //    DrawSystem();
        }

        #endregion

        #region Get_Set

        public bool ZoomEnabled
        {
            get { return _zoomEnabled; }
            set { _zoomEnabled = value; }
        }

        public bool ScrollEnabled
        {
            get { return ScrollEnable; }
            set
            {
                ScrollEnable = value;
                AdjustScrollBars();
                Invalidate();
            }
        }

        public float SetMapScale
        {
            get { return MapScale; }
            set
            {
                float oldScale = MapScale;
                MapScale = value;
                SetGraphicsToScale(oldScale, true);
                if(tb_Zoom != null)
                    tb_Zoom.Text = String.Format("{0:0.00}", Convert.ToDouble(MapScale));
            }
        }

        #endregion

        #region Scaling, Centering, Generic routines for Map View

        private void SetGraphicsToScale(float oldScale, bool recenter)
        {
            if (!InitMapSettings)
                return;

            float systemFontSize = DEFAULT_SYSTEM_FONT_SIZE,
                  systemDetailFontSize = DEFAULT_SYSTEM_DETAIL_SIZE,
                  systemBoldFontSize = DEFAULT_SYSTEM_FONT_SIZE,
                  regionFontSize = DEFAULT_REGION_FONT_SIZE,
                  constFontSize = DEFAULT_CONST_FONT_SIZE;

            _lineThickness = ((MapScale < 0.8f) ? 1.5f : (MapScale * 2));

            _lineAlpha = 0xFF;
            _labelAlpha = 0x40;

            if (MapScale < 0.5f) // dim out gates, display labels prominently
            {
                if (MapScale >= 0.1f)
                {
                    _lineAlpha = 0xFF - (int)((0.5f - MapScale) * 25 * 16);
                    _labelAlpha = 0x40 + (int)((0.5f - MapScale) * 25 * 16);
                }
                else
                {
                    _lineAlpha = 0xFF - (int)((0.25f - MapScale) * 25 * 16);
                    _labelAlpha = 0x40 + (int)((0.25f - MapScale) * 25 * 16);
                }
            }

            if (MapScale < 0.50)
            {
                // details are hidden at these scales
                systemFontSize = 7;
                systemBoldFontSize = 7;
                systemDetailFontSize = 7;
                regionFontSize = DEFAULT_REGION_FONT_SIZE * (MapScale + 0.08f);
                constFontSize = DEFAULT_CONST_FONT_SIZE * (MapScale + 0.08f);
            }
            if (MapScale >= 0.50 && MapScale < 0.80)
            {
                systemFontSize = 7;
                systemBoldFontSize = 7;
                systemDetailFontSize = 7;
                regionFontSize = DEFAULT_REGION_FONT_SIZE * (MapScale + 0.08f);
                constFontSize = DEFAULT_CONST_FONT_SIZE * (MapScale + 0.08f);
            }
            else if (MapScale >= 0.80 && MapScale < 0.90)
            {
                systemFontSize = 8;
                systemBoldFontSize = 8;
                systemDetailFontSize = 7;
                regionFontSize = DEFAULT_REGION_FONT_SIZE * (MapScale + 0.08f);
                constFontSize = DEFAULT_CONST_FONT_SIZE * (MapScale + 0.08f);
            }
            else if (MapScale >= 0.90 && MapScale < 1.10)
            {
                systemFontSize = 9;
                systemBoldFontSize = 9;
                systemDetailFontSize = 8;
            }
            else if (MapScale >= 1.10 && MapScale < 1.20)
            {
                systemFontSize = 10;
                systemBoldFontSize = 10;
                systemDetailFontSize = 9;
            }
            else if (MapScale >= 1.20 && MapScale <= 1.50)
            {
                systemFontSize = 11;
                systemBoldFontSize = 11;
                systemDetailFontSize = 10;
            }

            _systemFont = new Font(SystemMap.DefaultFont.FontFamily, systemFontSize);
            _systemBoldFont = new Font(SystemMap.DefaultFont.FontFamily, systemBoldFontSize, FontStyle.Bold);
            _systemDetailFont = new Font(SystemMap.DefaultFont.FontFamily, systemDetailFontSize);
            _regionFont = new Font(SystemMap.DefaultFont.FontFamily, regionFontSize);
            _constFont = new Font(SystemMap.DefaultFont.FontFamily, constFontSize);

            AdjustScrollBars();

            if (recenter)
            {
                if (MouseZoom)
                {
                    //PointF oldCenter =
                    //    UnscalePoint(new PointF(MWCenter.X, MWCenter.Y), Adjust, oldScale);
                    PointF oldCenter =
                        UnscalePoint(new PointF(MapSize.Width / 2f, MapSize.Height / 2f), Adjust, oldScale);

                    CenterOnPoint(oldCenter);
                    MouseZoom = false;
                }
                else
                {
                    PointF oldCenter =
                        UnscalePoint(new PointF(MapSize.Width / 2f, MapSize.Height / 2f), Adjust, oldScale);

                    CenterOnPoint(oldCenter);
                }
            }
        }

        private void FillAreaParams()
        {
            if (!InitMapSettings)
                return;

            if (FlatDataChanged)
            {
                GetGlobalFlatArea();
                FlatDataChanged = false;
                AdjustScrollBars();
            }

            Adjust.X = sb_HorizontalScroll.Value + GlobalFlatArea.Left;// -UserSettings.Instance.ActiveViewportStyle.Margin.Width;
            Adjust.Y = sb_VerticalScroll.Value + GlobalFlatArea.Top;// -UserSettings.Instance.ActiveViewportStyle.Margin.Height;
        }

        private void GetGlobalFlatArea()
        {
            float sysRad;

            //AU_MULT = (float)Math.Sqrt(((float)sSys.Radius / AU)) * 5;// / 5;

            sysRad = (((float)sSys.Radius / AU) * (float)(AU_MULT * Math.PI));

            GlobalFlatArea = Rectangle.FromLTRB((int)(sysRad * -1), (int)(sysRad * -1), (int)(sysRad), (int)(sysRad));
        }

        private void AdjustScrollBars()
        {
            if (!InitMapSettings)
                return;

            sb_HorizontalScroll.Visible = ScrollEnable;
            sb_VerticalScroll.Visible = ScrollEnable;

            sb_HorizontalScroll.Minimum = 0;
            sb_HorizontalScroll.Maximum = GlobalFlatArea.Width + 20;// +2 * UserSettings.Instance.ActiveViewportStyle.Margin.Width;

            sb_VerticalScroll.Minimum = 0;
            sb_VerticalScroll.Maximum = GlobalFlatArea.Height + 20;// +2 * UserSettings.Instance.ActiveViewportStyle.Margin.Height;

            if (ScrollEnable)
            {
                MapSize = Size.Subtract(ClientSize, new Size(sb_VerticalScroll.Width, sb_HorizontalScroll.Height));

                //if (GlobalFlatArea.Width + UserSettings.Instance.ActiveViewportStyle.Margin.Width * 2 <
                //    _mapEffectiveSize.Width / _mapScale)
                if (GlobalFlatArea.Width < MapSize.Width / MapScale)
                {
                    sb_HorizontalScroll.Enabled = false;
                    sb_HorizontalScroll.Value = 0;
                }
                else
                {
                    sb_HorizontalScroll.Enabled = true;
                    sb_HorizontalScroll.LargeChange = (int)(MapSize.Width / MapScale);
                    sb_HorizontalScroll.SmallChange = (int)(MapSize.Width / 5 / MapScale);
                }

                //if (GlobalFlatArea.Height + UserSettings.Instance.ActiveViewportStyle.Margin.Height * 2 <
                //    _mapEffectiveSize.Height / _mapScale)


                if (GlobalFlatArea.Height < MapSize.Height / MapScale)
                {
                    sb_VerticalScroll.Enabled = false;
                    sb_VerticalScroll.Value = 0;
                }
                else
                {
                    sb_VerticalScroll.Enabled = true;
                    sb_VerticalScroll.LargeChange = (int)(MapSize.Height / MapScale);
                    sb_VerticalScroll.SmallChange = (int)(MapSize.Height / 5 / MapScale);
                }
            }
            else MapSize = ClientSize;
        }

        public static PointF ScalePoint(PointF point, Point adjust, float scale)
        {
            point.X = (point.X - adjust.X) * scale;
            point.Y = (point.Y - adjust.Y) * scale;
            //return Point.Round(point);
            return point;
        }

        public static PointF UnscalePoint(PointF point, Point adjust, float scale)
        {
            point.X = point.X / scale + adjust.X;
            point.Y = point.Y / scale + adjust.Y;
            //return Point.Round(point);
            return point;
        }

        public static bool IsLineVisible(PointF p1, PointF p2, RectangleF viewRect, Point adjust, float mapScale)
        {
            // first, we check whether either of the points are inside the viewing rectangle.  if not,
            // we check whether the line intersects any of the sides of the viewing rectangle.  if so,
            // it is visible inside the rectangle

            if (viewRect.Contains(p1))
                return true;
            if (viewRect.Contains(p2))
                return true;

            if (LinesIntersect(p1, p2, new PointF(viewRect.Left, viewRect.Top), new PointF(viewRect.Right, viewRect.Top)))
                return true;
            if (LinesIntersect(p1, p2, new PointF(viewRect.Left, viewRect.Top),
                               new PointF(viewRect.Left, viewRect.Bottom))) return true;
            if (LinesIntersect(p1, p2, new PointF(viewRect.Right, viewRect.Bottom),
                               new PointF(viewRect.Left, viewRect.Bottom))) return true;
            if (LinesIntersect(p1, p2, new PointF(viewRect.Right, viewRect.Bottom),
                               new PointF(viewRect.Right, viewRect.Top))) return true;
            return false;
        }

        private static bool LinesIntersect(PointF a1, PointF a2, PointF b1, PointF b2)
        {
            // to accurately test whether two line segments AB and CD intersect, we test whether point A and B are on opposite
            // sides of line CD, and whether point C and D are on opposite sides of AB.  we do this by testing whether
            // you turn in the same direction when turning between the points (thanks, "Numerical Recipes in C"!)

            return (TurnDirection(a1, a2, b1) * TurnDirection(a1, a2, b2) <= 0) &&
                   (TurnDirection(b1, b2, a1) * TurnDirection(b1, b2, a2) <= 0);
        }

        private static int TurnDirection(PointF a, PointF b, PointF c)
        {
            // return code: 1 counter-clockwise, -1 clockwise, 0 collinear with no turn
            float dABX = (b.X - a.X),
                  dABY = (b.Y - a.Y),
                  dACX = (c.X - a.X),
                  dACY = (c.Y - a.Y);

            if (dABY * dACX < dACY * dABX) return 1;
            if (dABY * dACX > dACY * dABX) return -1;

            // special cases with collinear points AKA line right at edge of view rectangle
            if (dABX * dACX < 0 || dABY * dACY < 0) return -1;
            if (dABX * dABX + dABY * dABY >= dACX * dACX + dACY * dACY) return 0;
            return 1;
        }

        public void CenterOnPoint(PointF point)
        {
            float newScrollX = point.X - GlobalFlatArea.Left - MapSize.Width / 2 / MapScale;
            float newScrollY = point.Y - GlobalFlatArea.Top - MapSize.Height / 2 / MapScale;
            //float newScrollX = point.X - (MapSize.Width / 2);// / MapScale;
            //float newScrollY = point.Y - (MapSize.Height / 2);// / MapScale;

            ScrollToPosition(new Point((int)newScrollX, (int)newScrollY));
        }

        public void ScrollToPosition(PointF scrollValues)
        {
            Point safeScroll = SnapToScrollBoundaries(scrollValues);
            sb_HorizontalScroll.Value = safeScroll.X;
            sb_VerticalScroll.Value = safeScroll.Y;

            Invalidate();
        }

        private Point SnapToScrollBoundaries(PointF newScrollPos)
        {
            if (newScrollPos.X > sb_HorizontalScroll.Maximum - MapSize.Width / MapScale)
                newScrollPos.X = (int)(sb_HorizontalScroll.Maximum - MapSize.Width / MapScale);
            if (newScrollPos.Y > sb_VerticalScroll.Maximum - MapSize.Height / MapScale)
                newScrollPos.Y = (int)(sb_VerticalScroll.Maximum - MapSize.Height / MapScale);

            if (newScrollPos.X < 0) newScrollPos.X = 0;
            if (newScrollPos.Y < 0) newScrollPos.Y = 0;

            return Point.Round(newScrollPos);
        }

        private void FillTerritoryParameters()
        {
            if (!InitMapSettings)
                return;

            if (FlatDataChanged)
            {
                GetGlobalFlatArea();
                FlatDataChanged = false;
                AdjustScrollBars();
            }

            Adjust.X = sb_HorizontalScroll.Value + GlobalFlatArea.Left;// -UserSettings.Instance.ActiveViewportStyle.Margin.Width;
            Adjust.Y = sb_VerticalScroll.Value + GlobalFlatArea.Top;// -UserSettings.Instance.ActiveViewportStyle.Margin.Height;
        }

        #endregion

        #region Graphics_And_Drawing

        private float GetObjectDiameter(double rad)
        {
            float retV;

            if (MapScale > 1.5)
                retV = (float)((Math.Sqrt(2 * (rad / 1000)) / 10) * MapScale);
            else
                retV = (float)((Math.Sqrt(2 * (rad / 1000)) / 10) * 1.5);

            return retV;
        }

        private float GetOrbitDiameter(double rad)
        {
            float retV, div;

            div = (float)(rad / AU);
            retV = div * AU_MULT * MapScale;

            return retV;
        }

        private PointF GetOrbitHtWd(double rad, double ecc)
        {
            float ratio, div;
            PointF retV = new PointF();

            div = (float)rad;

            retV.X = div * 2;
            ratio = (float)Math.Sqrt(1 - (ecc * ecc));
            retV.Y = (float)(ratio * retV.X);

            retV.X = (retV.X / AU) * 10 * MapScale;
            retV.Y = (retV.Y / AU) * 10 * MapScale;
            //retV.X = (div / AU) * 10 * MapScale;
            //retV.Y = (div / AU) * 10 * MapScale;


            return retV;
        }

        private PointF GetPlanetCenterPostition(Planet p, PointF orbitCenter, long tm, float diamS)
        {
            PointF rVal = new PointF();
            double pDay, travDeg, degDay, angle, xyRel;
            float oRad;

            xyRel = p.Y / p.Z;
            angle = Math.Atan(xyRel);
            angle = angle * 360 / Math.PI;

            pDay = p.orbitP / 86400;
            degDay = 360 / pDay;

            travDeg = degDay * tm;
            oRad = (float)((p.orbitR / AU) * AU_MULT * MapScale) + diamS / 2;  

            angle = (angle + travDeg) / 360;

            rVal.X = (float)(orbitCenter.X + (Math.Cos(angle * 2 * Math.PI) * oRad) * -1);
            rVal.Y = (float)(orbitCenter.Y + (Math.Sin(angle * 2 * Math.PI) * oRad) * -1);

            return rVal;
        }

        private PointF GetMoonCenterPostition(Moon p, PointF orbitCenter, long tm, float diamS, float diam)
        {
            PointF rVal = new PointF();
            double pDay, travDeg, degDay, angle, xyRel;
            float oRad;

            xyRel = p.Y / p.Z;
            angle = Math.Atan(xyRel);
            angle = angle * 360 / Math.PI;

            pDay = p.orbitP / 86400;
            degDay = 360 / pDay;

            travDeg = degDay * tm;
            oRad = (float)(p.OrbitIndex * (MapScale / 2) + diamS / 2 + diam);// / 2;

            angle = (angle + travDeg) / 360;

            rVal.X = (float)(orbitCenter.X + (Math.Cos(angle * 2 * Math.PI) * oRad) * -1);
            rVal.Y = (float)(orbitCenter.Y + (Math.Sin(angle * 2 * Math.PI) * oRad) * -1);

            return rVal;
        }

        private PointF GetGateCenterPostition(StarGate g, PointF orbitCenter, long tm, float diam)
        {
            PointF rVal = new PointF();
            double travDeg, degDay, angle, xyRel;
            float oRad, orbitR;

            xyRel = g.Y / g.Z;
            angle = Math.Atan(xyRel);
            angle = angle * 360 / Math.PI;

            degDay = 0;
            travDeg = degDay * tm;

            orbitR = (float)Math.Sqrt((g.X * g.X) + (g.Y * g.Y) + (g.Z * g.Z));

            oRad = (float)((orbitR / AU) * AU_MULT * MapScale) + diam / 2;

            angle = (angle + travDeg) / 360;
            rVal.X = (float)(orbitCenter.X + (Math.Cos(angle * 2 * Math.PI) * oRad) * -1);
            rVal.Y = (float)(orbitCenter.Y + (Math.Sin(angle * 2 * Math.PI) * oRad) * -1);

            return rVal;
        }

        private void DrawSystem(Graphics g, Rectangle clipRect)
        {
            SolidBrush BSun, BPlanet, BMoon, BGate;
            Pen PPlanet;
            int clipInflateSize = (int)(250 * MapScale);
            // compensates for regional and constellational edges with labels sticking out
            Rectangle clipInflate = Rectangle.Inflate(clipRect, clipInflateSize, clipInflateSize);
            PointF celLoc, oCent, pLoc, sCent;
            float diamS, diamP;
            Rectangle dstRect, pdRect;
            RectangleF poRect;
            Bitmap imgM;
            ImageAttributes attr = new ImageAttributes();
            double mjrL, mnrL, ecc, mjrKM, mnrKM, mjrAU, mnrAU, loc1, loc2;

            BSun = new SolidBrush(Color.Gold);
            BPlanet = new SolidBrush(Color.Cyan);
            BMoon = new SolidBrush(Color.White);
            BGate = new SolidBrush(Color.Violet);
            PPlanet = new Pen(Color.LimeGreen);

            CelestialLocsByName.Clear();

            // Draw Sun
            diamS = GetObjectDiameter(SysDat.SunR);
            diamS = (diamS / 30) * MapScale;
            celLoc = ScalePoint(new PointF(0,0), Adjust, MapScale);
            sCent = new PointF();
            sCent.X = celLoc.X;
            sCent.Y = celLoc.Y;

            if (diamS < 10)
                diamS = 10;

            // Try using the graphic Icon for Suns here
            //img = new Bitmap(GetSunImage(SysDat.graphicID));
            //attr.SetColorKey(img.GetPixel(1, 1), img.GetPixel(1, 1));
            dstRect = new Rectangle((int)(sCent.X - diamS / 2), (int)(sCent.Y - diamS / 2), (int)(diamS), (int)(diamS));
            g.FillEllipse(Brushes.Gold, dstRect);
            //g.DrawImage(img, dstRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);

            // Get Graphic Icon to use for the Moon
            imgM = new Bitmap(SpatialIconList.Images[3]);
            attr.SetColorKey(imgM.GetPixel(0, 0), imgM.GetPixel(0, 0));
            
            CelestialLocsByName.Add("Sun", new PointF(0, 0));

            // Draw Planets
            foreach (Planet p in SysDat.Planets.Values)
            {
                // Now I need to place the planets on screen, in their orbits
                // The main problem being how the hell do I find the correct location with respect to
                // the actual data, and the in-game representation of the same
                diamP = GetObjectDiameter(p.radius);
                pLoc = GetPlanetCenterPostition(p, sCent, SystemTime, diamS);

                diamP = diamP * MapScale;

                mjrL = p.orbitR * 2;
                ecc = p.orbitE;

                mnrL = mjrL * (Math.Sqrt(1 - (ecc * ecc)));

                mjrKM = mjrL / 1000;
                mnrKM = mnrL / 1000;

                mjrAU = (mjrL / AU);
                mnrAU = (mnrL / AU);

                // Compute the Ellipse for Orbit based upon planet location (so elipse goes through the planet itself)
                loc1 = Math.Sqrt(((mjrAU / 2) * (mjrAU / 2)) - ((mnrAU / 2) * (mnrAU / 2)));
                loc2 = loc1 * -1;

                // Plot the Planet Orbit
                oCent = new PointF(((float)mjrAU * 100 * MapScale) / 2, ((float)mnrAU * 100 * MapScale) / 2);
                poRect = new RectangleF(oCent, new SizeF(((float)mjrAU * 100 * MapScale), ((float)mnrAU * 100 * MapScale)));
                g.DrawEllipse(Pens.White, (float)(sCent.X - poRect.X + loc1), sCent.Y - poRect.Y, poRect.Width, poRect.Height);

                if (diamP < 5)
                    diamP = 5;

                // Plot the Planet itself
                pLoc.X = (float)((p.Z / AU) * 100 * MapScale + sCent.X);
                pLoc.Y = (float)((p.X / AU) * 100 * MapScale + sCent.Y);
                pdRect = new Rectangle((int)(pLoc.X - diamP / 2), (int)(pLoc.Y - diamP / 2), (int)(diamP), (int)(diamP));
                BPlanet = new SolidBrush(GetPlanetColor(p.graphicID));
                g.FillEllipse(BPlanet, pdRect);

                //g.DrawImage(img, pdRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                //CelestialLocsByName.Add(p.Name, new PointF((pLoc.X - diamP / 2), (pLoc.Y - diamP / 2)));

                // Do Moons for Planetary Orbit
                //foreach (Moon m in SysDat.Moons.Values)
                //{
                //    if (m.CelIndex == p.CelIndex)
                //    {
                //        // This moon belongs to the planet, so calculate some numbers
                //        diam = GetObjectDiameter(Convert.ToDouble(m.radius));
                //        mCent = GetMoonCenterPostition(m, new PointF(pLoc.X - 2, pLoc.Y - 2), SystemTime, diamP, diam);

                //        mdRect = new Rectangle((int)(mCent.X - diam / 2), (int)(mCent.Y - diam / 2), (int)(diam), (int)(diam));
                //        g.DrawImage(imgM, mdRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                //        CelestialLocsByName.Add(m.Name, new PointF((mCent.X - diam / 2), (mCent.Y - diam / 2)));

                //        // Check to see if any stations are here at this moon
                //        GetAndDrawStationIfHere(m.Name, true, mCent, diam, g);
                //        // Check for modified configurable station at this moon
                //        GetAndDrawCStationIfHere(g, pLoc, mCent, diamP, diam, p.ID, m.ID, 1);
                //        // Check for any Towers, Cyno, JB at this moon
                //        GetAndDrawTowerCynoJBIfHere(g, mCent, diam, p.ID, m.ID, m.Name);
                //    }

                //}

                //// Check to see if any stations are here at this planet
                //GetAndDrawStationIfHere(p.Name, false, pLoc, diamP, g);
            }

            // Check to see if any Non-Modified conquerable stations are here in this System that are not located elsewhere
            //GetAndDrawCStationIfHere(g, new PointF(), new PointF(), 0, 0, 0, 0, 0);

            //img = new Bitmap(SpatialIconList.Images[2]);
            //attr.SetColorKey(img.GetPixel(0, 0), img.GetPixel(0, 0));
            //// Draw Gates
            //foreach (StarGate sg in SysDat.Gates.Values)
            //{
            //    diam = (float)(5 * MapScale);
            //    gCent = GetGateCenterPostition(sg, oCent, SystemTime, diam);

            //    dstRect = new Rectangle((int)(gCent.X - (diam*3)/2), (int)(gCent.Y - diam/2), (int)(diam *3), (int)(diam));
            //    g.DrawImage(img, dstRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
            //    CelestialLocsByName.Add(sg.destSys, new PointF((gCent.X - (diam * 3) / 2), (gCent.Y - diam / 2)));
            //}

            // Draw Belts - if realistic
            // Draw Cyno Gens
            // Draw Jump Bridges
            // Draw Towers - if practical (at current res, not very)

        }

        private void GetAndDrawTowerCynoJBIfHere(Graphics g, PointF mLoc, float dM, int p_id, int m_id, string mnName)
        {
            Rectangle stnRect, jbRect, cgRect;
            Bitmap img, jbI, cgjI, cImg;
            ImageAttributes attr = new ImageAttributes();
            int sSize;
            bool knownTower = false;
            bool gotOne = false;
            bool tJB = false;
            bool tCy = false;
            Graphics gI;
            DataRow MD = null;
            DataRow[] dr;

            sSize = (int)(MapScale * 4);

            cImg = new Bitmap(64, 64);
            gI = Graphics.FromImage(cImg);

            // Check for a Cyno or JB in system details
            dr = PlugInData.SysD.MoonTable.Select("SID=" + sSys.ID + " AND MID=" + m_id);
            if (dr.Length > 0)
                MD = dr[0];

            if (MD != null)
            {
                // JB
                if (Convert.ToBoolean(MD["Bridge"]))
                {
                    jbI = new Bitmap(SpatialIconList.Images[1]);
                    tJB = true;
                }
                else
                    jbI = null;

                // Only draw one, Jammer overrides
                if (MD["Cyno"].Equals("Jammer"))
                {
                    cgjI = new Bitmap(SpatialIconList.Images[24]);
                    tCy = true;
                }
                else if (MD["Cyno"].Equals("Generator"))
                {
                    cgjI = new Bitmap(SpatialIconList.Images[0]);
                    tCy = true;
                }
                else
                    cgjI = null;

                if (MD["TType"].ToString().Contains("Amarr"))
                    img = new Bitmap(SpatialIconList.Images[21]);
                else if (MD["TType"].ToString().Contains("Caldari"))
                    img = new Bitmap(SpatialIconList.Images[22]);
                else if (MD["TType"].ToString().Contains("Minmatar"))
                    img = new Bitmap(SpatialIconList.Images[20]);
                else if (MD["TType"].ToString().Contains("Gallente"))
                    img = new Bitmap(SpatialIconList.Images[23]);
                else
                {
                    if ((jbI != null) || (cgjI != null))
                        img = new Bitmap(SpatialIconList.Images[21]);
                    else
                        img = null; // No station type - so even if a jammer, etc is present we do not know where to put it so run away !
                }

                if (img != null)
                {
                    gotOne = true;
                    // Put in station image
                    stnRect = new Rectangle(0, 0, 64, 64);
                    attr.SetColorKey(img.GetPixel(1, 1), img.GetPixel(1, 1));
                    gI.DrawImage(img, stnRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);

                    if (jbI != null)
                    {
                        jbRect = new Rectangle(0, 32, 32, 32);
                        attr.SetColorKey(jbI.GetPixel(1, 1), jbI.GetPixel(1, 1));
                        gI.DrawImage(jbI, jbRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                    }

                    if (cgjI != null)
                    {
                        cgRect = new Rectangle(32, 32, 32, 32);
                        attr.SetColorKey(cgjI.GetPixel(1, 1), cgjI.GetPixel(1, 1));
                        gI.DrawImage(cgjI, cgRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                    }
                }
                //if (!CelestialLocsByName.ContainsKey(sName))
                //    CelestialLocsByName.Add(sName, new PointF((mLoc.X - dM / 2), (mLoc.Y - dM / 2)));
            }

            // Now - to be safe, check for one in the Cyno / Jump Bridge lists as well
            if (PlugInData.CynoGenJamList.ContainsKey(sSys.ID))
            {
                CynoGenJam cg = PlugInData.CynoGenJamList[sSys.ID];
                if (cg.moon.Equals(mnName))
                {
                    if (tCy)
                    {
                        knownTower = true;
                    }

                    if (!knownTower)
                    {
                        img = new Bitmap(SpatialIconList.Images[21]);
                        stnRect = new Rectangle(0, 0, 64, 64);
                        attr.SetColorKey(img.GetPixel(1, 1), img.GetPixel(1, 1));
                        gI.DrawImage(img, stnRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                    }

                    if (cg.IsJammer)
                        cgjI = new Bitmap(SpatialIconList.Images[24]);
                    else
                        cgjI = new Bitmap(SpatialIconList.Images[0]);

                    gotOne = true;
                    cgRect = new Rectangle(32, 32, 32, 32);
                    attr.SetColorKey(cgjI.GetPixel(1, 1), cgjI.GetPixel(1, 1));
                    gI.DrawImage(cgjI, cgRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                }
            }

            knownTower = false;
            foreach (var jbl in PlugInData.JumpBridgeList.Values)
            {
                foreach (JumpBridge jb in jbl.Values)
                {
                    // More complex check as JB has 2 systems where it is present, and key is only on source system
                    if ((jb.From.ID == sSys.ID) || (jb.To.ID == sSys.ID))
                    {
                        if (jb.ToMoon.Equals(mnName) || jb.FromMoon.Equals(mnName))
                        {
                            // We have a JB in this system at this moon
                            if (tJB)
                            {
                                knownTower = true;
                            }

                            if (!knownTower)
                            {
                                img = new Bitmap(SpatialIconList.Images[21]);
                                stnRect = new Rectangle(0, 0, 64, 64);
                                attr.SetColorKey(img.GetPixel(1, 1), img.GetPixel(1, 1));
                                gI.DrawImage(img, stnRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                            }

                            gotOne = true;
                            jbI = new Bitmap(SpatialIconList.Images[1]);
                            jbRect = new Rectangle(0, 32, 32, 32);
                            attr.SetColorKey(jbI.GetPixel(1, 1), jbI.GetPixel(1, 1));
                            gI.DrawImage(jbI, jbRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                        }
                    }
                }
            }

            if (gotOne)
            {
                stnRect = new Rectangle((int)(mLoc.X - 5), (int)(mLoc.Y + dM / 2 - 10), sSize, sSize);
                attr.SetColorKey(cImg.GetPixel(1, 1), cImg.GetPixel(1, 1));
                g.DrawImage(cImg, stnRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
            }
        }

        private Color GetPlanetColor(int grID)
        {
            switch (grID)
            {
                case 3941:
                    return Color.Purple;
                case 3832:
                    return Color.Green;
                case 3834:
                    return Color.LightBlue;
                case 3833:
                    return Color.LightYellow;
                case 3835:
                    return Color.Blue;
                case 3836:
                    return Color.Red;
                case 3837:
                    return Color.Tan;
                case 3935:
                    return Color.SteelBlue;
                case 202:
                    return Color.DimGray;
            }
            return Color.Green;
        }

        private Bitmap GetSunImage(int grID)
        {
            switch (grID)
            {
                case 1245:
                case 1243:
                case 1012:
                    return new Bitmap(SpatialIconList.Images[14]);
                case 1013:
                    return new Bitmap(SpatialIconList.Images[16]);
                case 1242:
                case 1244:
                    return new Bitmap(SpatialIconList.Images[15]);
                case 1247:
                case 1015:
                    return new Bitmap(SpatialIconList.Images[18]);
                case 1011:
                case 1241:
                case 1246:
                    return new Bitmap(SpatialIconList.Images[13]);
                case 1014:
                case 1248:
                    return new Bitmap(SpatialIconList.Images[17]);
            }
            return new Bitmap(SpatialIconList.Images[18]);
        }

        private void GetAndDrawStationIfHere(string nm, bool mn, PointF loc, float diam, Graphics g)
        {
            Image stnI = null;
            Rectangle dstRect;
            Bitmap img;
            ImageAttributes attr = new ImageAttributes();
            string sName = "";

            // Draw Stations
            if (sSys.Stations.Count > 0)
            {
                foreach (Station st in sSys.Stations.Values)
                {
                    stnI = null;

                    if (mn)
                    {
                        if (st.Name.Contains(nm))
                        {
                            stnI = GetImage(st.stationTypeID, true);
                            sName = st.Name;
                        }
                    }
                    else
                    {
                        if ((st.Name.Contains(nm)) && (!st.Name.Contains("Moon")))
                        {
                            stnI = GetImage(st.stationTypeID, true);
                            sName = st.Name;
                        }
                    }

                    if (stnI != null)
                    {
                        img = new Bitmap(stnI);

                        attr.SetColorKey(img.GetPixel(1, 1), img.GetPixel(1, 1));
                        dstRect = new Rectangle((int)(loc.X + diam / 2), (int)(loc.Y), (int)(MapScale * 5), (int)(MapScale * 5));
                        g.DrawImage(img, dstRect, 0, 0, 256, 256, GraphicsUnit.Pixel, attr);
                        if (!CelestialLocsByName.ContainsKey(sName))
                            CelestialLocsByName.Add(sName, new PointF((loc.X - diam / 2), (loc.Y - diam / 2)));
                    }
                }
            }

        }

        private void GetAndDrawCStationIfHere(Graphics g, PointF pLoc, PointF mLoc, float dP, float dM, int p_id, int m_id, int typ)
        {
            Image stnI = null;
            Rectangle dstRect;
            Bitmap img;
            ImageAttributes attr = new ImageAttributes();
            string sName = "";
            int sLoc = 0;
            DataRow[] dr;
            
            if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(sSys.ID))
            {
                ConqStation st = PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems[sSys.ID].ConqStations[0];
                stnI = GetImage(st.StnTypeID, true);

                dr = PlugInData.SysD.PlanetTable.Select("PID=" + p_id + " AND STID>0");
                if (dr.Length > 0)
                    sLoc = p_id;
                else
                {
                    dr = PlugInData.SysD.MoonTable.Select("MID=" + m_id + " AND STID>0");
                    if (dr.Length > 0)
                        sLoc = m_id;
                }

                sName = st.Name;
            }

            dstRect = new Rectangle(0, 25, 75, 75);
            if ((sLoc > 0) && ((p_id != 0) || (m_id != 0)))
            {
                // We have system details, and the station actually has a location entered for it.
                if (sLoc == p_id)
                {
                    dstRect = new Rectangle((int)(pLoc.X + dP / 2), (int)(pLoc.Y), (int)(MapScale * 5), (int)(MapScale * 5));
                    if ((stnI != null) && (!CelestialLocsByName.ContainsKey(sName)))
                    {
                        img = new Bitmap(stnI);

                        attr.SetColorKey(img.GetPixel(1, 1), img.GetPixel(1, 1));
                        g.DrawImage(img, dstRect, 0, 0, 256, 256, GraphicsUnit.Pixel, attr);
                        CelestialLocsByName.Add(sName, new PointF((pLoc.X + dP / 2), (pLoc.Y)));
                    }
                }
                else if (sLoc == m_id)
                {
                    dstRect = new Rectangle((int)(mLoc.X + dM / 2), (int)(mLoc.Y), (int)(MapScale * 5), (int)(MapScale * 5));
                    if ((stnI != null) && (!CelestialLocsByName.ContainsKey(sName)))
                    {
                        img = new Bitmap(stnI);

                        attr.SetColorKey(img.GetPixel(1, 1), img.GetPixel(1, 1));
                        g.DrawImage(img, dstRect, 0, 0, 256, 256, GraphicsUnit.Pixel, attr);
                        CelestialLocsByName.Add(sName, new PointF((mLoc.X + dM / 2), (pLoc.Y)));
                    }
                }
            }
            else
            {
                if ((stnI != null) && (!CelestialLocsByName.ContainsKey(sName)))
                {
                    img = new Bitmap(stnI);

                    attr.SetColorKey(img.GetPixel(1, 1), img.GetPixel(1, 1));
                    g.DrawImage(img, dstRect, 0, 0, 256, 256, GraphicsUnit.Pixel, attr);
                    CelestialLocsByName.Add(sName, new PointF(0, 25));
                }
            }
        }

         public Image GetImage(int ID, bool typ)
        {
            Image bmp;
            string imgId;

            imgId = ID.ToString();

            bmp = EveHQ.Core.ImageHandler.GetImage(imgId);

            return bmp;
        }

        #endregion

        #region Events and Movements

        public void FindSystemCelestialScreen(string CelName)
        {
            PointF centerPoint;
            SelectInfo = new ArrayList();

            if (CelName.Contains("{"))
                CelName = CelName.Substring(0, CelName.IndexOf('{'));
            CelName.TrimEnd(' ');

            if (!CelestialLocsByName.ContainsKey(CelName))
                return;

            //Invalidate();

            centerPoint = CelestialLocsByName[CelName];

            //celLoc = ScalePoint(new PointF(0, 0), Adjust, MapScale);
            //centerPoint.X -= celLoc.X;
            //centerPoint.Y -= celLoc.Y;
            CenterOnPoint(UnscalePoint(centerPoint, Adjust, MapScale));

            SelectInfo = FindCelestialByName(CelName);
            SystemCelestialSelected();
        }

        private void SystemMapControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;

            if (!InitMapSettings)
                return;

            e.Graphics.Clear(Config.MapColors["Background"]);
            FlatDataChanged = true;
            FillTerritoryParameters();

            // we extend it slightly to avoid clipping off AAed edges of things, for example gate lines
            e.ClipRectangle.Inflate(2, 2);

            DrawSystem(e.Graphics, e.ClipRectangle);

            if (!FirstLoad)
            {
                FirstLoad = true;
                CenterOnPoint(new PointF(0, 0));
            }
        }

        private void SystemMapControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _scrollEnabled)
            {
                // right_click + drage == drag map around
                _mapDragging = true;
                _mapDrag = e.Location;
                _mapDragOffset.X = sb_HorizontalScroll.Value;
                _mapDragOffset.Y = sb_VerticalScroll.Value;
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Check for System Selection
            }
        }

        private void SystemMapControl_MouseWheel(object sender, MouseEventArgs e)
        {
            MWCenter.X = e.X;
            MWCenter.Y = e.Y;

            if (e.Delta < 0)
            {
                MouseZoom = true;
                b_ZoomIn_Click(sender, new EventArgs());
            }
            else
            {
                MouseZoom = true;
                b_ZoomOut_Click(sender, new EventArgs());
            }
        }

        private void SystemMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mapDragging)
                Cursor = Cursors.Hand;
            else
                Cursor = Cursors.Default;

            if (_mapDragging)
            {
                Cursor = Cursors.Hand;

                Point newScroll = new Point(
                    (int)(2 * (_mapDrag.X - e.X) / MapScale + _mapDragOffset.X),
                    (int)(2 * (_mapDrag.Y - e.Y) / MapScale + _mapDragOffset.Y));

                ScrollToPosition(newScroll);
            }
        }

        private void SystemMapControl_MouseUp(object sender, MouseEventArgs e)
        {
            SelectInfo = new ArrayList();
            Cursor = Cursors.Default;
            ArrayList distData = new ArrayList();

            if (_mapDragging)
            {
                Point newScroll = new Point(
                    (int)(2 * (_mapDrag.X - e.X) / MapScale + _mapDragOffset.X),
                    (int)(2 * (_mapDrag.Y - e.Y) / MapScale + _mapDragOffset.Y));

                ScrollToPosition(newScroll);
                _mapDragging = false;
            }

            DistTo = null;
            SelectInfo = null;
            if (_doingDistance && (e.Button == MouseButtons.Left))
            {
                DistTo = FindCelestialOnScreen(e.Location);
            }
            else
                SelectInfo = FindCelestialOnScreen(e.Location);

            if ((SelectInfo != null) || (DistTo != null))
            {
                if (_doingDistance && (e.Button == MouseButtons.Left) && (DistTo != null))
                {
                    _doingDistance = false;
                    distData = CalculateCelestialDistances(DistFrom, DistTo);
                    RMA.ShowDistanceValues((string)distData[0], (string)distData[1], Convert.ToDouble(distData[2]), 2);
                }
                else
                    SystemCelestialSelected();
            }
            else if (e.Button == MouseButtons.Right)
            {
                cms_GetDistance.Hide();
            }
        }

        private void SystemCelestialSelected()
        {
            Planet p;
            Moon m;
            Station s;
            ConqStation cs;
            StarGate sg;
            JumpBridge jb;
            int type;

            type = Convert.ToInt32(SelectInfo[0]);

            switch (type)
            {
                case 1:         // Planet
                    p = (Planet)SelectInfo[1];
                    RMA.ShowPlanetDetails(p, sSys);
                    break;
                case 2:         // Moon
                    m = (Moon)SelectInfo[1];
                    RMA.ShowMoonDetails(m, sSys);
                    break;
                case 3:         // Station
                    s = (Station)SelectInfo[1];
                    RMA.ShowStationDetails(s, sSys);
                    break;
                case 4:         // Conq Station
                    cs = (ConqStation)SelectInfo[1];
                    RMA.ShowConqStationDetails(cs, sSys);
                    break;
                case 5:         // StarGate
                    sg = (StarGate)SelectInfo[1];
                    RMA.ShowGateDetails(sg, sSys);
                    break;
                case 6:         // Jump Bridge
                    jb = (JumpBridge)SelectInfo[1];
                    break;
                case 7:         // Pos Tower
                    //sd = (SystemDetails)SelectInfo[1];
                    break;
                default:        // No F-in clue
                    break;
            }
        }

        private ArrayList CalculateCelestialDistances(ArrayList FC, ArrayList TC)
        {
            ArrayList retVal = new ArrayList();

            Planet p;
            Moon m;
            Station s;
            ConqStation cs;
            StarGate sg;
            JumpBridge jb;
            DataRow[] SD;
            int type;
            double dist;
            PointF_3D src = new PointF_3D();
            PointF_3D dst = new PointF_3D();

            type = Convert.ToInt32(FC[0]);

            switch (type)
            {
                case 1:         // Planet
                    p = (Planet)FC[1];
                    retVal.Add(p.Name);
                    src = new PointF_3D(p.X, p.Y, p.Z);
                    break;
                case 2:         // Moon
                    m = (Moon)FC[1];
                    retVal.Add(m.Name);
                    src = new PointF_3D(m.X, m.Y, m.Z);
                    break;
                case 3:         // Station
                    s = (Station)FC[1];
                    retVal.Add(s.Name);
                    src = new PointF_3D(s.X, s.Y, s.Z);
                    break;
                case 4:         // Conq Station
                    cs = (ConqStation)FC[1];
                    retVal.Add(cs.Name);
                    // use planet or moon coords
                    src = (PointF_3D)FC[2];
                    break;
                case 5:         // StarGate
                    sg = (StarGate)FC[1];
                    retVal.Add(sg.destSys);
                    src = new PointF_3D(sg.X, sg.Y, sg.Z);
                    break;
                case 6:         // Jump Bridge
                    jb = (JumpBridge)FC[1];
                    retVal.Add("JB, " + jb.FromMoon + " --> " + jb.ToMoon);
                    // Use moon coords
                    break;
                case 7:         // Pos Tower
                    SD = (DataRow[])FC[1];
                    retVal.Add(SD[0]["TName"].ToString());
                    // Use moon coords
                    break;
                default:        // No F-in clue
                    retVal.Add("No Clue");
                    break;
            }

            type = Convert.ToInt32(TC[0]);

            switch (type)
            {
                case 1:         // Planet
                    p = (Planet)TC[1];
                    retVal.Add(p.Name);
                    dst = new PointF_3D(p.X, p.Y, p.Z);
                    break;
                case 2:         // Moon
                    m = (Moon)TC[1];
                    retVal.Add(m.Name);
                    dst = new PointF_3D(m.X, m.Y, m.Z);
                    break;
                case 3:         // Station
                    s = (Station)TC[1];
                    retVal.Add(s.Name);
                    dst = new PointF_3D(s.X, s.Y, s.Z);
                    break;
                case 4:         // Conq Station
                    cs = (ConqStation)TC[1];
                    retVal.Add(cs.Name);
                    // use planet or moon coords
                    dst = (PointF_3D)TC[2];
                    break;
                case 5:         // StarGate
                    sg = (StarGate)TC[1];
                    retVal.Add(sg.destSys);
                    dst = new PointF_3D(sg.X, sg.Y, sg.Z);
                    break;
                case 6:         // Jump Bridge
                    jb = (JumpBridge)TC[1];
                    retVal.Add("JB, " + jb.FromMoon + " --> " + jb.ToMoon);
                    // Use moon coords
                    break;
                case 7:         // Pos Tower
                     SD = (DataRow[])TC[1];
                    retVal.Add(SD[0]["TName"].ToString());
                    // Use moon coords
                    break;
                default:        // No F-in clue
                    retVal.Add("No Clue");
                    break;
            }

            dist = src.DistanceMeters(dst);
            retVal.Add(dist);

            return retVal;
        }

        private string GetCelestialName(ArrayList SC)
        {
            Planet p;
            Moon m;
            Station s;
            ConqStation cs;
            StarGate sg;
            JumpBridge jb;
            DataRow[] SD;
            int type;

            type = Convert.ToInt32(SC[0]);

            switch (type)
            {
                case 1:         // Planet
                    p = (Planet)SelectInfo[1];
                    return p.Name;
                case 2:         // Moon
                    m = (Moon)SelectInfo[1];
                    return m.Name;
                case 3:         // Station
                    s = (Station)SelectInfo[1];
                    return s.Name;
                case 4:         // Conq Station
                    cs = (ConqStation)SelectInfo[1];
                    return cs.Name;
                case 5:         // StarGate
                    sg = (StarGate)SelectInfo[1];
                    return sg.destSys;
                case 6:         // Jump Bridge
                    jb = (JumpBridge)SelectInfo[1];
                    return "JB, " + jb.FromMoon + " --> " + jb.ToMoon;
                case 7:         // Pos Tower
                    SD = (DataRow[])SelectInfo[1];
                    return SD[0]["TName"].ToString();
                default:        // No F-in clue
                    return "No Clue";
            }
        }

        private void sb_VerticalScroll_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }

        private void sb_HorizontalScroll_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }

        private void SystemMap_Resize(object sender, EventArgs e)
        {
            FlatDataChanged = true;
        }

        private void cb_FindSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ss;

            if (!LocalUpdate)
            {
                ss = cb_FindSystem.Text;
                sSys = PlugInData.GalMap.GetSystemByName(ss);
                SysDat = new SystemCelestials();
                SysDat.GetSystemCelestial(sSys);
                this.SelectNextControl(cb_FindSystem, true, true, true, true);
                SystemTime = -70900;
                Invalidate();
                RMA.ShowSystemDetails(sSys);
            }
        }

        #endregion

        #region Celestial Object Calculations
 
        public ArrayList FindCelestialByName(string name)
        {
            ArrayList retVal = null;

            //   Get object center & diameter
            //   Get an area to check against (diam)
            //   Determine if selecting the given object
            //   Generate info for display and return it

            // Planets
            foreach (Planet p in SysDat.Planets.Values)
            {
                if (p.Name.Equals(name))
                {
                    // Found planet
                    retVal = new ArrayList();
                    retVal.Add(1);
                    retVal.Add(p);
                    return retVal;
                }
                else
                {
                    // Do Moons for Planetary Orbit
                    foreach (Moon m in SysDat.Moons.Values)
                    {
                        if (m.CelIndex == p.CelIndex)
                        {
                            if (m.Name.Equals(name))
                            {
                                // Found Moon
                                retVal = new ArrayList();
                                retVal.Add(2);
                                retVal.Add(m);
                                return retVal;
                            }
                        }
                    }
                }
            }

            // Stations
            foreach (Station st in sSys.Stations.Values)
            {
                if (st.Name.Equals(name))
                {
                    retVal = new ArrayList();
                    retVal.Add(3);
                    retVal.Add(st);
                    return retVal;
                }
            }

            // Conq Stations
            if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(sSys.ID))
            {
                foreach (ConqStation st in PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems[sSys.ID].ConqStations.Values)
                {
                    if (st.Name.Equals(name))
                    {
                        retVal = new ArrayList();
                        retVal.Add(4);
                        retVal.Add(st);
                    }
                }
            }

            // Gates
            foreach (StarGate sg in SysDat.Gates.Values)
            {
                if (sg.destSys.Equals(name))
                {
                    retVal = new ArrayList();
                    retVal.Add(5);
                    retVal.Add(sg);
                }
            }

            return retVal;
        }


        public ArrayList FindCelestialOnScreen(PointF location)
        {
            ArrayList retVal = null;
            // compensates for regional and constellational edges with labels sticking out
            PointF celLoc, conv, oCent, pLoc, mCent, gCent;
            Rectangle gate;
            float diam, diamS, diamP;

            float xDiff;
            float yDiff;
            float curRadius;

            //   Get object center & diameter
            //   Get an area to check against (diam)
            //   Determine if selecting the given object
            //   Generate info for display and return it
            diamS = MapScale * 60;
            celLoc = ScalePoint(new PointF(0, 0), Adjust, MapScale);
            oCent = new PointF();
            oCent.X = celLoc.X - 3;
            oCent.Y = celLoc.Y - 3;

            // Planets
            foreach (Planet p in SysDat.Planets.Values)
            {
                conv = GetOrbitHtWd(Convert.ToDouble(p.orbitR), p.orbitE);

                // This draws a fair planetary Orbit

                // Now I need to place the planets on screen, in their orbits
                // The main problem being how the hell do I find the correct location with respect to
                // the actual data, and the in-game representation of the same
                diamP = GetObjectDiameter(p.radius);
                pLoc = GetPlanetCenterPostition(p, oCent, SystemTime, diamS);
                xDiff = pLoc.X - location.X;
                yDiff = pLoc.Y - location.Y;
                curRadius = (float)Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
                if (curRadius < (diamP / 2.1))
                {
                    // Found planet
                    retVal = new ArrayList();
                    retVal.Add(1);
                    retVal.Add(p);
                    return retVal;
                }
                else
                {
                    // Do Moons for Planetary Orbit
                    foreach (Moon m in SysDat.Moons.Values)
                    {
                        if (m.CelIndex == p.CelIndex)
                        {
                            // This moon belongs to the planet, so calculate some numbers
                            diam = GetObjectDiameter(Convert.ToDouble(m.radius));
                            mCent = GetMoonCenterPostition(m, new PointF(pLoc.X - 2, pLoc.Y - 2), SystemTime, diamP, diam);

                            xDiff = mCent.X - location.X;
                            yDiff = mCent.Y - location.Y;
                            curRadius = (float)Math.Sqrt(xDiff * xDiff + yDiff * yDiff);

                            if (curRadius < (diam / 2.1))
                            {
                                // Found Moon
                                retVal = new ArrayList();
                                retVal.Add(2);
                                retVal.Add(m);
                                return retVal;
                            }
                            else
                            {
                                // Stations
                                retVal = CheckForStationClick(m.Name, true, mCent, diam, location, m.ID);
                                if (retVal != null)
                                    break;
                            }
                        }
                    }
                    // Stations
                    if (retVal != null)
                        break;
                    retVal = CheckForStationClick(p.Name, false, pLoc, diamP, location, p.ID);
                    if (retVal != null)
                        break;
                }
            }

            // Gates
            foreach (StarGate sg in SysDat.Gates.Values)
            {
                diam = (float)(5 * MapScale);
                gCent = GetGateCenterPostition(sg, oCent, SystemTime, diam);
                gate = new Rectangle((int)(gCent.X - (diam * 3) / 2), (int)(gCent.Y - diam / 2), (int)(diam * 3), (int)(diam));

                if (gate.Contains((int)location.X, (int)location.Y))
                {
                    retVal = new ArrayList();
                    retVal.Add(5);
                    retVal.Add(sg);
                }
            }
            
            return retVal;
        }

        private ArrayList CheckForStationClick(string nm, bool mn, PointF loc, float diam, PointF mLoc, int ID)
        {
            ArrayList retVal = null;
            Rectangle stn;
            PointF_3D stnLoc = new PointF_3D();
            int sLoc = 0;
            DataRow[] dr;

            stn = new Rectangle((int)(loc.X + diam/2), (int)(loc.Y), (int)(MapScale * 2.35), (int)(MapScale * 2.35));

            // Draw Stations
            if (sSys.Stations.Count > 0)
            {
                foreach (Station st in sSys.Stations.Values)
                {
                    if (mn)
                    {
                        if (st.Name.Contains(nm))
                        {
                            if (stn.Contains((int)mLoc.X, (int)mLoc.Y))
                            {
                                retVal = new ArrayList();
                                retVal.Add(3);
                                retVal.Add(st);
                                return retVal;
                            }
                        }
                    }
                    else
                    {
                        if ((st.Name.Contains(nm)) && (!st.Name.Contains("Moon")))
                        {
                            if (stn.Contains((int)mLoc.X, (int)mLoc.Y))
                            {
                                retVal = new ArrayList();
                                retVal.Add(3);
                                retVal.Add(st);
                                return retVal;
                            }
                        }
                    }
                }
            }

            if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(sSys.ID))
            {
                foreach (ConqStation st in PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems[sSys.ID].ConqStations.Values)
                {
                    sLoc = 0;
                    dr = PlugInData.SysD.PlanetTable.Select("PID=" + ID + " AND STID>0");
                    if (dr.Length > 0)
                        sLoc = ID;
                    else
                    {
                        dr = PlugInData.SysD.MoonTable.Select("MID=" + ID + " AND STID>0");
                        if (dr.Length > 0)
                            sLoc = ID;
                    }

                    if (sLoc < 1)
                        stn = new Rectangle(0, 25, 75, 75);
                    else
                    {
                        if (mn)
                        {
                            // At a moon
                            stn = new Rectangle((int)(loc.X + diam / 2), (int)(loc.Y), (int)(MapScale * 2.35), (int)(MapScale * 2.35));
                            stnLoc = new PointF_3D(SysDat.Moons[ID].X, SysDat.Moons[ID].Y, SysDat.Moons[ID].Z);
                        }
                        else
                        {
                            // At a planet
                            stn = new Rectangle((int)(loc.X + diam / 2), (int)(loc.Y), (int)(MapScale * 2.35), (int)(MapScale * 2.35));
                            stnLoc = new PointF_3D(SysDat.Planets[ID].X, SysDat.Planets[ID].Y, SysDat.Planets[ID].Z);
                        }
                    }

                    if (stn.Contains((int)mLoc.X, (int)mLoc.Y))
                    {
                        retVal = new ArrayList();
                        retVal.Add(4);
                        retVal.Add(st);
                        retVal.Add(stnLoc);
                    }
                }
            }

            
            return retVal;
        }

        private void tsmi_CalculateDistance_Click(object sender, EventArgs e)
        {
            _doingDistance = true;
            DistFrom = new ArrayList(SelectInfo);

            RMA.ShowDistanceValues(GetCelestialName(DistFrom), "", 0, 1);
        }


        #endregion

        #region Zoom Handling

        //private const float MIN_ZOOM = 0.02f;
        //private const float MAX_ZOOM = 2.0f;
        //private const float DEF_ZOOM_INC = 0.01f;
        //private const float FAST_ZOOM_INC = 0.1f;
        //private const float SLOW_ZOOM_INC = 0.005f;
        
        private void b_ZoomIn_Click(object sender, EventArgs e)
        {
            float dScale = MapScale;

            if (Control.ModifierKeys == Keys.Control)
                dScale += FAST_ZOOM_INC;
            else if (Control.ModifierKeys == Keys.Alt)
                dScale += SLOW_ZOOM_INC;
            else
                dScale += DEF_ZOOM_INC;

            if (dScale > MAX_ZOOM)
                dScale = MAX_ZOOM;

            SetMapScale = dScale;
            Invalidate();
        }

        private void b_ZoomOut_Click(object sender, EventArgs e)
        {
            float dScale = MapScale;

            if (Control.ModifierKeys == Keys.Control)
                dScale -= FAST_ZOOM_INC;
            else if (Control.ModifierKeys == Keys.Alt)
                dScale -= SLOW_ZOOM_INC;
            else
                dScale -= DEF_ZOOM_INC;

            if (dScale < MIN_ZOOM)
                dScale = MIN_ZOOM;

            SetMapScale = dScale;
            Invalidate();
        }

        private void b_RotateClockwise_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
                SystemTime += 100;
            else if (Control.ModifierKeys == Keys.Alt)
                SystemTime += 10;
            else
                SystemTime += 1;

            Invalidate();
        }

        private void b_RotateCClockwise_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
                SystemTime -= 100;
            else if (Control.ModifierKeys == Keys.Alt)
                SystemTime -= 10;
            else
                SystemTime -= 1;

            Invalidate();
        }

        #endregion

     }
}
