// ========================================================================
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2011  EveHQ Development Team
// 
// This file is part of the "EveHQ RouteMap" plug-in
//
// EveHQ RouteMap derives from copyrighted works licensed under the GNU 
// General Public License originally created by Lhyda Souljacker.
//
// This version has been modified pursuant to the GNU General Public 
// License as of 4 September 2011, and as distributed, it includes or is 
// derivative of works licensed under the GNU General Public License.
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
    public partial class MapView : UserControl
    {
        private const float DEFAULT_CONST_FONT_SIZE = 32;
        private const float DEFAULT_REGION_FONT_SIZE = 92;
        private const float DEFAULT_SYSTEM_DETAIL_SIZE = 8;
        private const float DEFAULT_SYSTEM_FONT_SIZE = 8;
        private const float MIN_ZOOM = 0.02f;
        private const float MAX_ZOOM = 2.0f;
        private const float DEF_ZOOM_INC = 0.01f;
        private const float FAST_ZOOM_INC = 0.1f;
        private const float SLOW_ZOOM_INC = 0.005f;
        private static NumberFormatInfo nfi;
        public Dictionary<int, SolarSystem> UnfilteredSystems;
        public List<Vertex> Route { get; set; }

        private bool DoingPrintImage = false;
        public bool ScrollEnable = true;
        public bool ResizeRedrawEn = false;
        private bool InitMapSettings = false;
        public bool FlatDataChanged = true;
        public float MapScale = 0.035f;
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
        private bool LocalUpdate = false;
        private Point _sysDrag;
        private bool _sysDragging;
        private Point _sysDragOffset;
        private Point _sysStart;

        private SolarSystem _hoverSystem;
        private SolarSystem _selectedSystem;
        private SolarSystem _startSystem;
        private bool _zoomEnabled;

        public bool HLJBRange;
        public bool HLShipJumpRange;
        public bool HLShipJumpToRange;
        public bool HLSearchResult;
        public bool HLGateRange;
        public bool RouteChanged;

        public delegate void SystemSelectedHandler();
        private EveGalaxy GalMap = new EveGalaxy();
        private ConfigData2 Config = new ConfigData2();
        public bool MouseZoom = false;
        private RouteMapMainForm RMA;
        private Point MWCenter;
        SmallMap SMap;
        private Bitmap imgJB, imgJ, imgG, imgCS, imgCyno;
        private ImageAttributes attr = new ImageAttributes();


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

        public MapView()
        {
        }

        public void UpdateConfig(ConfigData2 cfg)
        {
            Config = cfg;
            Invalidate();
        }

        public void UpdateGalaxy(EveGalaxy gmap)
        {
            GalMap = gmap;
            Invalidate();
        }

        public void UpdateMapForChange()
        {
            Invalidate();
        }

        public void SetupMapView(ArrayList sl, EveGalaxy gmap, ConfigData2 cfg, SmallMap sm, RouteMapMainForm rm)
        {
            SMap = sm;
            Config = cfg;
            GalMap = gmap;
            RMA = rm;

            InitializeComponent();
            InitializeMapVeiw();

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
        }

        protected virtual void OnSystemSelected()
        {
            SystemSelected();
        }

        public void InitializeMapVeiw()
        {

            imgJB = new Bitmap(MV_Images.Images[0]);
            attr.SetColorKey(imgJB.GetPixel(1, 1), imgJB.GetPixel(1, 1));
            imgJ = new Bitmap(MV_Images.Images[4]);
            attr.SetColorKey(imgJ.GetPixel(1, 1), imgJ.GetPixel(1, 1));
            imgG = new Bitmap(MV_Images.Images[2]);
            attr.SetColorKey(imgG.GetPixel(1, 1), imgG.GetPixel(1, 1));
            imgCyno = new Bitmap(MV_Images.Images[3]);
            attr.SetColorKey(imgCyno.GetPixel(1, 1), imgCyno.GetPixel(1, 1));
            imgCS = new Bitmap(MV_Images.Images[1]);
            attr.SetColorKey(imgCS.GetPixel(1, 1), imgCS.GetPixel(1, 1));
          
            InitMapSettings = true;
            FillAreaParams();
            MapScale = 0.035f;
            MapSize = Size.Subtract(ClientSize, new Size(sb_VerticalScroll.Width, sb_HorizontalScroll.Height));
            _hoverSystem = new SolarSystem();
            SetGraphicsToScale(MapScale, false);
        }

        public void SystemSelected()
        {
            // A solar system was selected, display the information!
            if (RMA == null)
                return;

            RMA.NewSystemSelected(_selectedSystem);
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

        public string SelectedSystemName
        {

            set
            {
                SolarSystem solar;
                if (GalMap != null)
                    solar = GalMap.GetSystemByName(value);
                else
                    solar = new SolarSystem();

                if (solar != null)
                    SelectedSystem = solar;
            }
            get
            {
                if (_selectedSystem != null)
                    return _selectedSystem.Name;
                return String.Empty;
            }
        }

        public SolarSystem SelectedSystem
        {
            get { return _selectedSystem; }
            set
            {
                if (value == null)
                    return;

                _selectedSystem = value;
                OnSystemSelected();

                if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(_selectedSystem.ID)))
                    CenterOnPoint(PlugInData.SystemCoords[Convert.ToInt64(_selectedSystem.ID)].OrgCoord);
                else
                    CenterOnPoint( _selectedSystem.OrgCoord);

                Application.DoEvents();
            }
        }

        public SolarSystem HoverSystem
        {
            get { return _hoverSystem; }
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

            _systemFont = new Font(MapView.DefaultFont.FontFamily, systemFontSize);
            _systemBoldFont = new Font(MapView.DefaultFont.FontFamily, systemBoldFontSize, FontStyle.Bold);
            _systemDetailFont = new Font(MapView.DefaultFont.FontFamily, systemDetailFontSize);
            _regionFont = new Font(MapView.DefaultFont.FontFamily, regionFontSize);
            _constFont = new Font(MapView.DefaultFont.FontFamily, constFontSize);

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
            Point FlatMin = new Point(int.MaxValue, int.MaxValue);
            Point FlatMax = new Point(int.MinValue, int.MinValue);

            foreach (var curRegion in GalMap.GalData.Regions)
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

                if (GlobalFlatArea.Width < MapSize.Width / MapScale)
                {
                    sb_HorizontalScroll.Enabled = false;
                    sb_HorizontalScroll.Value = 0;
                }
                else
                {
                    sb_HorizontalScroll.Enabled = true;

                    if ((int)(MapSize.Width / MapScale) > 0)
                        sb_HorizontalScroll.LargeChange = (int)(MapSize.Width / MapScale);
                    if ((int)(MapSize.Width / 5 / MapScale) > 0)
                        sb_HorizontalScroll.SmallChange = (int)(MapSize.Width / 5 / MapScale);

                }

                if (GlobalFlatArea.Height < MapSize.Height / MapScale)
                {
                    sb_VerticalScroll.Enabled = false;
                    sb_VerticalScroll.Value = 0;
                }
                else
                {
                    sb_VerticalScroll.Enabled = true;
                    if ((int)(MapSize.Height / MapScale) > 0)
                        sb_VerticalScroll.LargeChange = (int)(MapSize.Height / MapScale);
                    if ((int)(MapSize.Height / 5 / MapScale) > 0)
                        sb_VerticalScroll.SmallChange = (int)(MapSize.Height / 5 / MapScale);
                }
            }
            else
                MapSize = Size.Subtract(ClientSize, new Size(sb_VerticalScroll.Width, sb_HorizontalScroll.Height));
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

        public static bool IsAreaVisible(RectangleF testArea, RectangleF viewRect, Point adjust, float mapScale,
                                         bool prescaled)
        {
            if (!prescaled)
            {
                PointF topLeft = ScalePoint(new PointF(testArea.Left, testArea.Top), adjust, mapScale);
                PointF bottomRight = ScalePoint(new PointF(testArea.Right, testArea.Bottom), adjust, mapScale);
                testArea = RectangleF.FromLTRB(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
            }

            return viewRect.IntersectsWith(testArea);
        }

        public void CenterOnPoint(PointF point)
        {
            float newScrollX = point.X - GlobalFlatArea.Left - MapSize.Width / 2 / MapScale;
            float newScrollY = point.Y - GlobalFlatArea.Top - MapSize.Height / 2 / MapScale;

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

        public void ZoomAndCenterOnRegionOrConstellation(string rgn, string cnst)
        {
            Rectangle bounds;
            PointF Center = new PointF();
            float hView, vView;
            float nhScale, nvScale;

            hView = MapSize.Width;
            vView = MapSize.Height;

            if (!cnst.Equals(""))
            {
                // We after a constellation
                // Find the center map point for the Center
                // Get boundary of the Constellation
                bounds = PlugInData.GalMap.GalData.Regions[Convert.ToInt32(PlugInData.RCSelList[rgn].ID)].Constellations[Convert.ToInt32(PlugInData.RCSelList[rgn].Constellations[cnst].ID)].GetCurrentBounds();
                // Compute Center point for that boundary
                Center.X = bounds.X + (bounds.Width / 2);
                Center.Y = bounds.Y + (bounds.Height / 2);

                nhScale = hView / bounds.Width;
                nvScale = vView / bounds.Height;
                // Offset a bit to get the entire systems images into the view
                nhScale -= (float)0.15;
                nvScale -= (float)0.15;

                if (nhScale < nvScale)
                {
                    if (nhScale > MAX_ZOOM)
                        SetMapScale = MAX_ZOOM;
                    else if (nhScale < MIN_ZOOM)
                        SetMapScale = MIN_ZOOM;
                    else
                        SetMapScale = nhScale;
                }
                else
                {
                    if (nvScale > MAX_ZOOM)
                        SetMapScale = MAX_ZOOM;
                    else if (nvScale < MIN_ZOOM)
                        SetMapScale = MIN_ZOOM;
                    else
                        SetMapScale = nvScale;
                }
                // Call CenterOnPoint for this location
                CenterOnPoint(Center);
            }
            else
            {
                // We after a region
                // Find the center map point for the Center
                bounds = PlugInData.GalMap.GalData.Regions[Convert.ToInt32(PlugInData.RCSelList[rgn].ID)].GetCurrentBounds();
                // Compute Center point for that boundary
                Center.X = bounds.X + (bounds.Width / 2);
                Center.Y = bounds.Y + (bounds.Height / 2);

                nhScale = hView / bounds.Width;
                nvScale = vView / bounds.Height;
                // Offset a bit to get the entire systems images into the view
                nhScale -= (float)0.04;
                nvScale -= (float)0.04;

                if (nhScale < nvScale)
                {
                    if (nhScale > MAX_ZOOM)
                        SetMapScale = MAX_ZOOM;
                    else if (nhScale < MIN_ZOOM)
                        SetMapScale = MIN_ZOOM;
                    else
                        SetMapScale = nhScale;
                }
                else
                {
                    if (nvScale > MAX_ZOOM)
                        SetMapScale = MAX_ZOOM;
                    else if (nvScale < MIN_ZOOM)
                        SetMapScale = MIN_ZOOM;
                    else
                        SetMapScale = nvScale;
                }
                // Call CenterOnPoint for this location
                CenterOnPoint(Center);
            }
        }

        public void ZoomAndCenterOnRoute()
        {
            Rectangle bounds;
            PointF Center = new PointF();
            float hView, vView;
            float nhScale, nvScale;

            hView = MapSize.Width;
            vView = MapSize.Height;

            if (Route.Count > 0)
            {
                // We after a constellation
                // Find the center map point for the Center
                // Get boundary of the Constellation
                bounds = GetRouteBounds();
                // Compute Center point for that boundary
                Center.X = bounds.X + (bounds.Width / 2);
                Center.Y = bounds.Y + (bounds.Height / 2);

                nhScale = hView / bounds.Width;
                nvScale = vView / bounds.Height;
                // Offset a bit to get the entire systems images into the view
                nhScale *= (float)0.97;
                nvScale *= (float)0.97;

                if (nhScale < nvScale)
                {
                    if (nhScale > MAX_ZOOM)
                        SetMapScale = MAX_ZOOM;
                    else if (nhScale < MIN_ZOOM)
                        SetMapScale = MIN_ZOOM;
                    else
                        SetMapScale = nhScale;
                }
                else
                {
                    if (nvScale > MAX_ZOOM)
                        SetMapScale = MAX_ZOOM;
                    else if (nvScale < MIN_ZOOM)
                        SetMapScale = MIN_ZOOM;
                    else
                        SetMapScale = nvScale;
                }
                // Call CenterOnPoint for this location
                CenterOnPoint(Center);
            }
        }

        #endregion

        #region Graphics_And_Drawing

        private void DrawSystems(Graphics g, Rectangle clipRect)
        {
            bool regionDrawn = false;
            bool filterSystems = IsFilterActive();
            int clipInflateSize = (int)(75 * MapScale);
            // compensates for regional and constellational edges with labels sticking out
            Rectangle clipInflate = Rectangle.Inflate(clipRect, clipInflateSize, clipInflateSize);

            foreach (Region curRegion in GalMap.GalData.Regions.Values)
            {
                if (RMA.cbx_SelRegions.Checked)
                    if (PlugInData.RCSelList.ContainsKey(curRegion.Name))
                        if (!PlugInData.RCSelList[curRegion.Name].Selected)
                            continue;

                if (PlugInData.WHMapSelected)
                {
                    if (!PlugInData.WHRegions.Contains(curRegion.ID))
                        continue;
                }
                else
                {
                    if (PlugInData.SkipRegions.Contains(curRegion.ID))
                        continue;
                }

                regionDrawn = false;
                foreach (var curConst in curRegion.Constellations)
                {
                    if (IsAreaVisible(curConst.Value.Bounds, clipInflate, Adjust, MapScale, false))
                    {
                        if (RMA.cbx_SelConst.Checked)
                            if (!PlugInData.RCSelList[curRegion.Name].Constellations[curConst.Value.Name].Selected)
                                continue;

                        if (!regionDrawn)
                        {
                            if (Config.CFlags["Show Region Names"])
                            {
                                PointF drawPos = ScalePoint(new PointF(
                                                                        curRegion.Bounds.Left + curRegion.Bounds.Width / 2,
                                                                        curRegion.Bounds.Top + curRegion.Bounds.Height / 2),
                                                                    Adjust, MapScale);
                                DrawTerritoryLabel(g, drawPos, curRegion.RColor, _regionFont, curRegion.Name);
                            }
                            regionDrawn = true;
                        }

                        if (Config.CFlags["Show Const Names"])
                        {
                            PointF drawPos = ScalePoint(new PointF(
                                                                    curConst.Value.Bounds.Left + curConst.Value.Bounds.Width / 2,
                                                                    curConst.Value.Bounds.Top + curConst.Value.Bounds.Height / 2),
                                                                Adjust, MapScale);
                            DrawTerritoryLabel(g, drawPos, curConst.Value.CColor, _constFont,
                                                curConst.Value.Name);
                        }

                        foreach (var curSystem in curConst.Value.Systems)
                            DrawSystemElements(g, curSystem.Value, filterSystems, false);
                    }
                }
            }
        }

        private void DrawSystemElements(Graphics g, SolarSystem system, bool filtering, bool linkSys)
        {
            PointF sysLoc;

            if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(system.ID)))
                sysLoc = ScalePoint(PlugInData.SystemCoords[Convert.ToInt64(system.ID)].OrgCoord, Adjust, MapScale);
            else
                sysLoc = ScalePoint(system.OrgCoord, Adjust, MapScale);

            bool filterpass;
            if (filtering)
                filterpass = SystemIsUnfiltered(system);
            else 
                filterpass = true;

            if (Config.CFlags["Show Sovreignty"])
                DrawInfoCircle(g, system, sysLoc, linkSys);

            DrawSystemMarker(g, system, sysLoc, filterpass, linkSys);

            if (Config.CFlags["Show System Names"] && (MapScale >= 0.2f || (Config.CFlags["Show Cyno Beacons"] && PlugInData.CynoGenJamList.ContainsKey(system.ID))))
                DrawSystemLabel(g, system, sysLoc, filterpass, linkSys);
            if ((_startSystem != null) && HLJBRange)
                CheckAndHighlightJBRangeForSystem(g, system, sysLoc);
            if ((_startSystem != null) && HLShipJumpRange && (Config.SelShip != null))
                CheckAndHighlightShipRangeForSystem(g, system, sysLoc);
            if ((_startSystem != null) && HLShipJumpToRange && (Config.SelShip != null))
                CheckAndHighlightShipToRangeForSystem(g, system, sysLoc);
            if (HLSearchResult)
                CheckAndHighlightSearchResultsForSystem(g, system, sysLoc);
        }

        private bool GetConqStationForSystemIfAny(int sysID)
        {
            if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(sysID))
                if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems[sysID].ConqStations.Count > 0)
                    return true;

            return false;
        }

        private int GetStationTypeIndexIfAny(SolarSystem ss)
        {
            int c = -1, l = 0, r = 0, m = 0;

            foreach (Station st in ss.Stations.Values)
            {
                if (st.Cloning)
                    c = 1;
                if ((st.Factory) || (st.Services.Contains("Factory")))
                    m = 8;
                if (st.Refine > 0)
                    r = 4;
                if (st.Services.Contains("Laboratory"))
                    l = 2;
            }

            if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(ss.ID))
            {
                foreach (ConqStation cq in PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems[ss.ID].ConqStations.Values)
                {
                    if (cq.Cloning)
                        c = 1;
                    if (cq.Factory)
                        m = 8;
                    if (cq.Refine > 0)
                        r = 4;
                    if (cq.Labs)
                        l = 2;
                }
            }

            return (c + l + r + m);
        }

        private void DrawStationMarker(Graphics g, PointF drawPos, float drawSize, bool linkSys, int imgIndx)
        {
            float xP, yP, Dim;
            PointF Cent;
            bool M = false;
            bool R = false;
            bool C = false;
            bool L = false;
            PointF[] rRegion = new PointF[3];
            PointF[] fRegion = new PointF[3];
            PointF[] lRegion = new PointF[3];
            PointF[] cRegion = new PointF[3];
            Pen noPen;

            if (imgIndx >= 8)
            {
                M = true;
                imgIndx -= 8;
            }
            if (imgIndx >= 4)
            {
                R = true;
                imgIndx -= 4;
            }
            if (imgIndx >= 2)
            {
                L = true;
                imgIndx -= 2;
            }
            if (imgIndx >= 1)
            {
                C = true;
                imgIndx -= 1;
            }
            // 1 - Draw Square
            // 2 - Draw Cross from Corners
            // 3 - Fill in with Approp Colors in correct locations
            Pen borderPen = new Pen(Color.FromArgb((linkSys) ? 0x20 : 0xFF, Color.Silver), (Config.Sizes["Sys Marker Thickness"] * MapScale));
            Pen refinePen = new Pen(Color.FromArgb((linkSys) ? 0x20 : 0xFF, Color.Red), (Config.Sizes["Sys Marker Thickness"] * MapScale));
            Pen factoryPen = new Pen(Color.FromArgb((linkSys) ? 0x20 : 0xFF, Color.Gold), (Config.Sizes["Sys Marker Thickness"] * MapScale));
            Pen clonePen = new Pen(Color.FromArgb((linkSys) ? 0x20 : 0xFF, Color.LimeGreen), (Config.Sizes["Sys Marker Thickness"] * MapScale));
            Pen labPen = new Pen(Color.FromArgb((linkSys) ? 0x20 : 0xFF, Color.Blue), (Config.Sizes["Sys Marker Thickness"] * MapScale));
            
            if (DoingPrintImage)
                noPen = new Pen(Color.FromArgb((linkSys) ? 0x20 : 0xFF, Color.White), (Config.Sizes["Sys Marker Thickness"] * MapScale));
            else
                noPen = new Pen(Color.FromArgb((linkSys) ? 0x20 : 0xFF, Color.Black), (Config.Sizes["Sys Marker Thickness"] * MapScale));

            xP = (drawPos.X - drawSize / 2);
            yP = (drawPos.Y - drawSize / 2);
            Dim = drawSize;
            Cent = new PointF((xP + (Dim / 2)), (yP + (Dim / 2)));

            g.DrawRectangle(borderPen, xP, yP, Dim, Dim);

            rRegion[0] = new PointF(xP, yP);
            rRegion[1] = new PointF(xP + Dim, yP);
            rRegion[2] = new PointF(Cent.X, Cent.Y);

            cRegion[0] = new PointF(xP + Dim, yP);
            cRegion[1] = new PointF(xP + Dim, yP + Dim);
            cRegion[2] = new PointF(Cent.X, Cent.Y);

            fRegion[0] = new PointF(xP, yP + Dim);
            fRegion[1] = new PointF(xP + Dim, yP + Dim);
            fRegion[2] = new PointF(Cent.X, Cent.Y);

            lRegion[0] = new PointF(xP, yP);
            lRegion[1] = new PointF(xP, yP + Dim);
            lRegion[2] = new PointF(Cent.X, Cent.Y);

            if (R)
                g.FillPolygon(refinePen.Brush, rRegion);
            else
                g.FillPolygon(noPen.Brush, rRegion);

            if (M)
                g.FillPolygon(factoryPen.Brush, fRegion);
            else
                g.FillPolygon(noPen.Brush, fRegion);

            if (C)
                g.FillPolygon(clonePen.Brush, cRegion);
            else
                g.FillPolygon(noPen.Brush, cRegion);

            if (L)
                g.FillPolygon(labPen.Brush, lRegion);
            else
                g.FillPolygon(noPen.Brush, lRegion);
        }

        private void DrawSystemMarker(Graphics g, SolarSystem system, PointF drawPos, bool passesFilter, bool linkSys)
        {
            float drawSize; // right now all system markers have to have square proportions.
            int imgIndx;

            // If a route exists, update co-ordinates in case system has been manually relocated
            if (Route != null)
            {
                for (int i = 0; i < Route.Count - 1; i++)
                {
                    if (Route[i].SolarSystem.ID == system.ID)
                    {
                        if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(system.ID)))
                            Route[i].SolarSystem.OrgCoord = PlugInData.SystemCoords[Convert.ToInt64(system.ID)].OrgCoord;
                        else
                            Route[i].SolarSystem.OrgCoord = system.OrgCoord;
                    }
                }
            }

            if (!linkSys)
            {
                using (
                    Pen systemPen =
                        new Pen(
                            Color.FromArgb((passesFilter) ? 0xFF : 0x40, system.GetSystemColor()), (int)(Config.Sizes["Sys Marker Thickness"] * MapScale)))
                {
                    if (system == _selectedSystem)
                    {
                        drawSize = (Config.Sizes["Sys Marker Size"] - 2) * MapScale;
                        imgIndx = GetStationTypeIndexIfAny(system);

                        if (imgIndx >= 0)
                        {
                            drawSize = (Config.Sizes["Sys Marker Size"] + 5) * MapScale;
                            DrawStationMarker(g, drawPos, drawSize, linkSys, imgIndx);
                        }
                        else
                        {
                            g.DrawEllipse(systemPen, drawPos.X - drawSize / 2, drawPos.Y - drawSize / 2, drawSize,
                                          drawSize);
                        }
                    }
                    else
                    {
                        drawSize = Config.Sizes["Sys Marker Diameter"] * MapScale;
                        imgIndx = GetStationTypeIndexIfAny(system);

                        if ((imgIndx > 0) && (MapScale < 0.8f))
                        {
                            g.FillRectangle(systemPen.Brush, drawPos.X - drawSize / 2 - MapScale,
                                            drawPos.Y - drawSize / 2 - MapScale, drawSize + 2 * MapScale,
                                            drawSize + 2 * MapScale);
                        }
                        else if (imgIndx >= 0)
                        {
                            drawSize = (Config.Sizes["Sys Marker Size"] + 2) * MapScale;
                            DrawStationMarker(g, drawPos, drawSize, linkSys, imgIndx);
                        }
                        else
                        {
                            g.DrawEllipse(systemPen, drawPos.X - drawSize / 2, drawPos.Y - drawSize / 2, drawSize,
                                          drawSize);
                        }
                    }
                }
            }
            else
            {
                using (
                    Pen systemPen =
                        new Pen(
                            Color.FromArgb(0x20, system.GetSystemColor()), (int)(Config.Sizes["Sys Marker Thickness"] * MapScale)))
                {
                    if (system == _selectedSystem)
                    {
                        drawSize = (Config.Sizes["Sys Marker Size"] - 2) * MapScale;
                        imgIndx = GetStationTypeIndexIfAny(system);

                        if (imgIndx >= 0)
                        {
                            drawSize = (Config.Sizes["Sys Marker Size"] + 5) * MapScale;
                            DrawStationMarker(g, drawPos, drawSize, linkSys, imgIndx);
                        }
                        else
                        {
                            g.DrawEllipse(systemPen, drawPos.X - drawSize / 2, drawPos.Y - drawSize / 2, drawSize,
                                          drawSize);
                        }
                    }
                    else
                    {
                        drawSize = Config.Sizes["Sys Marker Diameter"] * MapScale;
                        imgIndx = GetStationTypeIndexIfAny(system);

                        if ((imgIndx > 0) && (MapScale < 0.8f))
                        {
                            g.FillRectangle(systemPen.Brush, drawPos.X - drawSize / 2 - MapScale,
                                            drawPos.Y - drawSize / 2 - MapScale, drawSize + 2 * MapScale,
                                            drawSize + 2 * MapScale);
                        }
                        else if (imgIndx >= 0)
                        {
                            drawSize = (Config.Sizes["Sys Marker Size"] + 2) * MapScale;
                            DrawStationMarker(g, drawPos, drawSize, linkSys, imgIndx);
                        }
                        else
                        {
                            g.DrawEllipse(systemPen, drawPos.X - drawSize / 2, drawPos.Y - drawSize / 2, drawSize,
                                          drawSize);
                        }
                    }
                }
            }
        }

        private void DrawInfoCircle(Graphics g, SolarSystem system, PointF drawPos, bool linkSys)
        {
            if (PlugInData.GalAPI.Galaxy_API.SovAPI.SovList.ContainsKey(system.ID))
            {
                float radius = Config.Sizes["Sys Marker Size"] * MapScale;
                RectangleF visibilityRect = new RectangleF(drawPos.X - radius, drawPos.Y - radius, radius * 2, radius * 2);

                if (PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances.ContainsKey(PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[system.ID].allianceID))
                {
                    if (!linkSys)
                    {
                        using (
                            SolidBrush sovBrush =
                                new SolidBrush(PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[system.ID].allianceID].AColor))
                            g.FillEllipse(sovBrush, visibilityRect);
                    }
                    else
                    {
                        using (
                            SolidBrush sovBrush =
                                new SolidBrush(Color.FromArgb(0x20,PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[system.ID].allianceID].AColor)))
                            g.FillEllipse(sovBrush, visibilityRect);
                    }
                }
            }
        }

        private void DrawSystemLabel(Graphics g, SolarSystem system, PointF drawPos, bool passesFilter, bool linkSys)
        {
            bool doDetails = Config.CFlags["Show Detail Text"] && MapScale > 0.80f;
            Font systemTextFont = system == _selectedSystem ? _systemBoldFont : _systemFont;

            string systemTitle = system.Name;
            string systemDetails;


            if (doDetails)
                systemDetails = GetDetailString(system, Config.Settings["Detail Text"]);
            else
                systemDetails = string.Empty;

            SizeF titleSize = g.MeasureString(systemTitle, systemTextFont);
            SizeF detailSize = g.MeasureString(systemDetails, _systemDetailFont);

            SizeF totalSize = new SizeF(titleSize.Width, titleSize.Height + detailSize.Height + 2 * MapScale);

            bool detailLarger = false;
            if (detailSize.Width > titleSize.Width)
            {
                totalSize.Width = detailSize.Width;
                detailLarger = true;
            }

            float offset = Config.Sizes["Sys Marker Size"] * MapScale / 2;

            //if (MapScale < 0.50f && Config.ShowCynoBeacons && GalMap.GalData.SystemHasCynoGen(system.ID))
            //    offset = 3;

            switch ((RelativePosition)system.LabelLocation)
            {
                case RelativePosition.Top:
                    drawPos.X -= totalSize.Width / 2;
                    drawPos.Y -= totalSize.Height;
                    break;
                case RelativePosition.TopLeft:
                    drawPos.X -= totalSize.Width + offset - 2;
                    drawPos.Y -= totalSize.Height;
                    break;
                case RelativePosition.TopRight:
                    drawPos.X += offset + 1;
                    drawPos.Y -= totalSize.Height;
                    break;
                case RelativePosition.CenterLeft:
                    drawPos.X -= totalSize.Width + offset - 2;
                    drawPos.Y -= totalSize.Height / 2;
                    break;
                case RelativePosition.CenterRight:
                    drawPos.X += offset + 1;
                    drawPos.Y -= totalSize.Height / 2;
                    break;
                case RelativePosition.Bottom:
                    drawPos.X -= totalSize.Width / 2;
                    drawPos.Y += offset;
                    break;
                case RelativePosition.BottomLeft:
                    drawPos.X -= totalSize.Width + offset - 2;
                    drawPos.Y += offset;
                    break;
                case RelativePosition.BottomRight:
                    drawPos.X += offset + 1;
                    drawPos.Y += offset;
                    break;
            }

            PointF titlePos = drawPos, detailPos = drawPos;
            if (detailLarger)
            {
                titlePos.X = drawPos.X + (detailSize.Width - titleSize.Width) / 2;
                detailPos.Y = drawPos.Y + titleSize.Height - 2 * MapScale;
            }
            else if (Config.CFlags["Show Detail Text"])
            {
                detailPos.X = drawPos.X + (titleSize.Width - detailSize.Width) / 2;
                detailPos.Y = drawPos.Y + titleSize.Height - 2 * MapScale;
            }

            if (!linkSys)
            {
                if (!DoingPrintImage)
                {
                    using (
                        SolidBrush systemFontBrush =
                            new SolidBrush(Color.FromArgb((passesFilter) ? 0xFF : 0x40, Config.MapColors["System"])))
                        g.DrawString(systemTitle, systemTextFont, systemFontBrush, titlePos);
                }
                else
                {
                    using (
                        SolidBrush systemFontBrush =
                            new SolidBrush(Color.FromArgb((passesFilter) ? 0xFF : 0x40, Color.Black)))
                        g.DrawString(systemTitle, systemTextFont, systemFontBrush, titlePos);
                }
            }
            else
            {
                using (
                    SolidBrush systemFontBrush =
                        new SolidBrush(Color.FromArgb(0x20, Config.MapColors["System"])))
                    g.DrawString(systemTitle, systemTextFont, systemFontBrush, titlePos);
            }

            if (doDetails)
            {
                Color secColor;

                if 
                    (system.Security < 0) secColor = Config.MapColors["Null Sec Details"];
                else if 
                    (system.Security < 0.45) secColor = Config.MapColors["Low Sec Details"];
                else 
                    secColor = Config.MapColors["High Sec Details"];

                if 
                    (linkSys) secColor = Color.FromArgb(0x20, secColor);
                else if 
                    (!passesFilter) secColor = Color.FromArgb(0x40, secColor);

                using (SolidBrush secBrush = new SolidBrush(secColor))
                    g.DrawString(systemDetails, _systemDetailFont, secBrush, detailPos);
            }
        }

        private string GetDetailString(SolarSystem srcSystem, int detailType)
        {
            string systemDetails = "(" + Math.Round(srcSystem.Security, 2).ToString();
            ArrayList s_jk = new ArrayList();

            int count = 0, cnt2 = 0;

            switch (detailType)
            {
                case 0:
                    if (srcSystem.OreBelts > 0)
                        systemDetails += ") (" + srcSystem.OreBelts; // +")";

                    if (srcSystem.IceBelts > 0)
                        systemDetails += ") [" + srcSystem.IceBelts + "]";
                    else
                        systemDetails += ")";

                    break;
                case 4:
                    systemDetails += ") (" + srcSystem.SecClass + ")";
                    break;
                case 8:
                    if (srcSystem.Stations != null)
                        count = srcSystem.Stations.Count;
                    if (PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems.ContainsKey(srcSystem.ID))
                        cnt2 += PlugInData.GalAPI.Galaxy_API.ConqStationAPI.Systems[srcSystem.ID].ConqStations.Count;

                    if (count > 0)
                        systemDetails += ") (S:" + count + ")";
                    else if (cnt2 > 0)
                        systemDetails += ") (CS:" + cnt2 + ")";
                    else
                        systemDetails += ") (0)";

                    break;
                case 2:
                    if (srcSystem.Moons > 0)
                        systemDetails += ") (" + srcSystem.Moons + ")";
                    break;
                case 7:
                    if (PlugInData.GalAPI.Galaxy_API.SovAPI.SovList.ContainsKey(srcSystem.ID))
                    {
                        if (PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[srcSystem.ID].allianceID == 0)
                            break;

                        systemDetails += ")[" + PlugInData.GalAPI.Galaxy_API.AllianceAPI.Alliances[PlugInData.GalAPI.Galaxy_API.SovAPI.SovList[srcSystem.ID].allianceID].ticker + "]";
                    }
                    break;
                case 6:
                    s_jk = PlugInData.JKList.GetLatestJumpKillForSID(srcSystem.ID);
                    int totalKills = Convert.ToInt32(s_jk[1]) + Convert.ToInt32(s_jk[2]) + Convert.ToInt32(s_jk[3]);
                    systemDetails += ") (T:" + totalKills + ") (P:" + s_jk[2] + "  S:" +
                                        s_jk[1] + "  N:" + s_jk[3] + ")";
                    break;
                case 1:
                    s_jk = PlugInData.JKList.GetLatestJumpKillForSID(srcSystem.ID);
                    systemDetails += ")( " + s_jk[0] + " )";
                    break;
                case 5:
                    count = srcSystem.OreBelts + srcSystem.IceBelts;
                    systemDetails += ") (" + count + ")";

                    string currentRatType = GalMap.GalData.Regions[srcSystem.RegionID].GetRatType();
                    if (currentRatType != "None")
                        systemDetails += " " + currentRatType;

                    break;
                case 9:
                    systemDetails += ") " + GalMap.GalData.Regions[srcSystem.RegionID].Name;
                    systemDetails += "\n   - " + GalMap.GalData.Regions[srcSystem.RegionID].Constellations[srcSystem.ConstID].Name;
                    break;
                default:
                    systemDetails += ")";
                    break;
            }
            return systemDetails;
        }

        private void DrawTerritoryLabel(Graphics g, PointF position, Color color, Font font, string text)
        {
            
            SizeF nameSize = g.MeasureString(text, font);
            PointF drawOrigin = new PointF(position.X - nameSize.Width / 2, position.Y - nameSize.Height / 2);

            SolidBrush labelBrush = new SolidBrush(Color.FromArgb(_labelAlpha, color));
            g.DrawString(text, font, labelBrush, drawOrigin);
            labelBrush.Dispose();
        }

        private void DrawConnections(Graphics g, Rectangle clipRect)
        {
            if (PlugInData.WHMapSelected)
                return;

            DrawGateJumps(clipRect, g);

            if (Route != null)
                DrawRoute(g);

            if (Config.CFlags["Show Jump Bridges"])
                DrawJumpBridges(g, clipRect);

            if (Config.CFlags["Show Cyno Beacons"])
                DrawCynoGens(g);
        }

        private void DrawCynoGens(Graphics g)
        {
            bool filter = IsFilterActive();
            SolarSystem ss;
            Color cynoColor;
            RectangleF dstRect;
            int scMult;
            PointF cynoSystem;

            scMult = (int)(Config.Sizes["Sys Marker Size"] * 1.2 * MapScale);

            foreach (var cynoArray in PlugInData.CynoGenJamList.Values)
            {
                float drawSizeInternal = (Config.Sizes["Sys Marker Diameter"]) * ((MapScale < 0.60f) ? 0.60f : MapScale);
                float drawSizeExternal = (Config.Sizes["Sys Marker Size"] + Config.Sizes["Sys Marker Thickness"]) *
                                         ((MapScale < 0.60f) ? 0.60f : MapScale);

                ss = PlugInData.GalMap.GetSystemByID(cynoArray.systemID);

                if (RMA.cbx_SelRegions.Checked)
                    if (!PlugInData.RCSelList[GalMap.GalData.Regions[ss.RegionID].Name].Selected)
                        continue;

                if (RMA.cbx_SelConst.Checked)
                    if (!PlugInData.RCSelList[GalMap.GalData.Regions[ss.RegionID].Name].Constellations[GalMap.GalData.Regions[ss.RegionID].Constellations[ss.ConstID].Name].Selected)
                        continue;

                if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(ss.ID)))
                    cynoSystem = ScalePoint(PlugInData.SystemCoords[Convert.ToInt64(ss.ID)].OrgCoord, Adjust, MapScale);
                else
                    cynoSystem = ScalePoint(ss.OrgCoord, Adjust, MapScale);
                
                if(cynoArray.IsJammer)
                    cynoColor = Color.Red;
                else
                    cynoColor = Color.FromArgb(0xFF, Config.MapColors["Cyno Beacon"]);

                if (cynoArray.IsJammer && (MapScale > 0.5f))
                {
                    dstRect = new RectangleF(cynoSystem.X + 10 * MapScale, cynoSystem.Y + 10 * MapScale, scMult, scMult);
                    DrawCynoImage(g, dstRect, true);
                    //g.DrawImage(imgJ, dstRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                }
                else if (MapScale > 0.5f)
                {
                    dstRect = new RectangleF(cynoSystem.X + 10 * MapScale, cynoSystem.Y + 10 * MapScale, scMult, scMult);
                    DrawCynoImage(g, dstRect, false);
                    //g.DrawImage(imgG, dstRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                }
                else
                {
                    using (SolidBrush internalBrush = new SolidBrush(cynoColor))
                        g.FillEllipse(internalBrush, cynoSystem.X - drawSizeInternal / 2, cynoSystem.Y - drawSizeInternal / 2,
                                      drawSizeInternal, drawSizeInternal);

                    using (
                        Pen externalPen = new Pen(cynoColor, Config.Sizes["Sys Marker Thickness"] * MapScale - 1))
                        g.DrawEllipse(externalPen, cynoSystem.X - drawSizeExternal / 2, cynoSystem.Y - drawSizeExternal / 2,
                                      drawSizeExternal, drawSizeExternal);

                }

            }
        }

        private void DrawJumpBridges(Graphics g, Rectangle viewRect)
        {
            RectangleF dstRect, srcRect;
            ArrayList SrcDest;
            SortedList<string, JBMoon> SysBL = new SortedList<string, JBMoon>();
            int scMult;
            PointF fromSystem, toSystem;
            PointF fromSysT, toSysT;
            string fromDetails, toDetails;

            scMult = (int)(Config.Sizes["Sys Marker Size"] * 1.2 * MapScale);

            float scaledNoDraw = (MapScale < 0.5f) ? 0 : Config.Sizes["Sys Marker Size"] * MapScale;
            float circleRadius = Config.Sizes["Sys Marker Size"] * MapScale;

            foreach (var jbl in PlugInData.JumpBridgeList.Values)
            {
                foreach (var curJump in jbl.Values)
                {
                    if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(curJump.From.ID)))
                        fromSystem = ScalePoint(PlugInData.SystemCoords[Convert.ToInt64(curJump.From.ID)].OrgCoord, Adjust, MapScale);
                    else
                        fromSystem = ScalePoint(PlugInData.GalMap.GetSystemByID(curJump.From.ID).OrgCoord, Adjust, MapScale);

                    if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(curJump.To.ID)))
                        toSystem = ScalePoint(PlugInData.SystemCoords[Convert.ToInt64(curJump.To.ID)].OrgCoord, Adjust, MapScale);
                    else
                        toSystem = ScalePoint(PlugInData.GalMap.GetSystemByID(curJump.To.ID).OrgCoord, Adjust, MapScale);

                    if (!IsLineVisible(fromSystem, toSystem, viewRect, Adjust, MapScale))
                        continue;

                    Pen bridgePen;
                    if (IsFilterActive())
                        bridgePen = new Pen(Color.FromArgb(40, PlugInData.Config.MapColors["Jump Bridge Link"]), _lineThickness) { DashStyle = DashStyle.Dash };
                    else
                        bridgePen = new Pen(Color.FromArgb(170, PlugInData.Config.MapColors["Jump Bridge Link"]), _lineThickness) { DashStyle = DashStyle.Dash };

                    if (RMA.cbx_SelRegions.Checked)
                    {
                        if (PlugInData.RCSelList[GalMap.GalData.Regions[curJump.From.RegionID].Name].Selected || PlugInData.RCSelList[GalMap.GalData.Regions[curJump.To.RegionID].Name].Selected)
                        {
                            if (MapScale > 0.5f)
                            {
                                srcRect = new RectangleF((int)(fromSystem.X - 10 * MapScale), (int)(fromSystem.Y - (22 * MapScale)), scMult, scMult);
                                DrawJBImage(g, srcRect);
                                //g.DrawImage(imgJB, srcRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                                dstRect = new RectangleF((int)(toSystem.X - 10 * MapScale), (int)(toSystem.Y - (22 * MapScale)), scMult, scMult);
                                DrawJBImage(g, dstRect);
                                //g.DrawImage(imgJB, dstRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);

                                fromDetails = GetFromJB(curJump.FromMoon, curJump.From) + "> " + curJump.To.Name;
                                toDetails = GetFromJB(curJump.ToMoon, curJump.To) + "> " + curJump.From.Name;

                                if (SysBL.ContainsKey(curJump.From.Name))
                                {
                                    SysBL[curJump.From.Name].jb2 = fromDetails;
                                }
                                else
                                {
                                    SysBL.Add(curJump.From.Name, new JBMoon(new PointF(srcRect.X + srcRect.Width, srcRect.Y), fromDetails, "", false));
                                }

                                if (SysBL.ContainsKey(curJump.To.Name))
                                {
                                    SysBL[curJump.To.Name].jb2 = toDetails;
                                }
                                else
                                {
                                    SysBL.Add(curJump.To.Name, new JBMoon(new PointF(dstRect.X + dstRect.Width, dstRect.Y), toDetails, "", false));
                                }
                            }

                            SrcDest = DrawBezier(g, bridgePen, fromSystem, toSystem, scaledNoDraw, 3, Math.PI / 12);

                            g.DrawEllipse(bridgePen, fromSystem.X - circleRadius, fromSystem.Y - circleRadius, circleRadius * 2,
                                          circleRadius * 2);
                            g.DrawEllipse(bridgePen, toSystem.X - circleRadius, toSystem.Y - circleRadius, circleRadius * 2,
                                          circleRadius * 2);

                            if (!PlugInData.RCSelList[GalMap.GalData.Regions[curJump.From.RegionID].Name].Selected)
                            {
                                DrawSystemElements(g, curJump.From, false, true);
                                if (SysBL.ContainsKey(curJump.From.Name))
                                    SysBL[curJump.From.Name].dimT = true;
                            }
                            else if (!PlugInData.RCSelList[GalMap.GalData.Regions[curJump.To.RegionID].Name].Selected)
                            {
                                DrawSystemElements(g, curJump.To, false, true);
                                if (SysBL.ContainsKey(curJump.To.Name))
                                    SysBL[curJump.To.Name].dimT = true;
                            }
                        }
                    }
                    else if (RMA.cbx_SelConst.Checked)
                    {
                        if (PlugInData.RCSelList[GalMap.GalData.Regions[curJump.From.RegionID].Name].Constellations[GalMap.GalData.Regions[curJump.From.RegionID].Constellations[curJump.From.ConstID].Name].Selected ||
                            PlugInData.RCSelList[GalMap.GalData.Regions[curJump.To.RegionID].Name].Constellations[GalMap.GalData.Regions[curJump.To.RegionID].Constellations[curJump.To.ConstID].Name].Selected)
                        {
                            if (MapScale > 0.5f)
                            {
                                srcRect = new Rectangle((int)(fromSystem.X - 10 * MapScale), (int)(fromSystem.Y - (22 * MapScale)), scMult, scMult);
                                DrawJBImage(g, srcRect);
                                //g.DrawImage(imgJB, srcRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                                dstRect = new Rectangle((int)(toSystem.X - 10 * MapScale), (int)(toSystem.Y - (22 * MapScale)), scMult, scMult);
                                DrawJBImage(g, dstRect);
                                //g.DrawImage(imgJB, dstRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);

                                fromDetails = GetFromJB(curJump.FromMoon, curJump.From) + "> " + curJump.To.Name;
                                toDetails = GetFromJB(curJump.ToMoon, curJump.To) + "> " + curJump.From.Name;

                                if (SysBL.ContainsKey(curJump.From.Name))
                                {
                                    SysBL[curJump.From.Name].jb2 = fromDetails;
                                }
                                else
                                {
                                    SysBL.Add(curJump.From.Name, new JBMoon(new PointF(srcRect.X + srcRect.Width, srcRect.Y), fromDetails, "", false));
                                }

                                if (SysBL.ContainsKey(curJump.To.Name))
                                {
                                    SysBL[curJump.To.Name].jb2 = toDetails;
                                }
                                else
                                {
                                    SysBL.Add(curJump.To.Name, new JBMoon(new PointF(dstRect.X + dstRect.Width, dstRect.Y), toDetails, "", false));
                                }
                            }

                            SrcDest = DrawBezier(g, bridgePen, fromSystem, toSystem, scaledNoDraw, 3, Math.PI / 12);

                            g.DrawEllipse(bridgePen, fromSystem.X - circleRadius, fromSystem.Y - circleRadius, circleRadius * 2,
                                          circleRadius * 2);
                            g.DrawEllipse(bridgePen, toSystem.X - circleRadius, toSystem.Y - circleRadius, circleRadius * 2,
                                          circleRadius * 2);

                            if (!PlugInData.RCSelList[GalMap.GalData.Regions[curJump.From.RegionID].Name].Constellations[GalMap.GalData.Regions[curJump.From.RegionID].Constellations[curJump.From.ConstID].Name].Selected)
                            {
                                DrawSystemElements(g, curJump.From, false, true);
                                if (SysBL.ContainsKey(curJump.From.Name))
                                    SysBL[curJump.From.Name].dimT = true;
                            }
                            else if (!PlugInData.RCSelList[GalMap.GalData.Regions[curJump.To.RegionID].Name].Constellations[GalMap.GalData.Regions[curJump.To.RegionID].Constellations[curJump.To.ConstID].Name].Selected)
                            {
                                DrawSystemElements(g, curJump.To, false, true);
                                if (SysBL.ContainsKey(curJump.To.Name))
                                    SysBL[curJump.To.Name].dimT = true;
                            }
                        }
                    }
                    else if (RMA.cbx_SelGalaxy.Checked)
                    {
                        if (MapScale > 0.5f)
                        {
                            srcRect = new Rectangle((int)(fromSystem.X - 10 * MapScale), (int)(fromSystem.Y - (22 * MapScale)), scMult, scMult);
                            DrawJBImage(g, srcRect);
                            //g.DrawImage(imgJB, srcRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                            dstRect = new Rectangle((int)(toSystem.X - 10 * MapScale), (int)(toSystem.Y - (22 * MapScale)), scMult, scMult);
                            DrawJBImage(g, dstRect);
                            //g.DrawImage(imgJB, dstRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);

                            fromDetails = GetFromJB(curJump.FromMoon, curJump.From) + "> " + curJump.To.Name;
                            toDetails = GetFromJB(curJump.ToMoon, curJump.To) + "> " + curJump.From.Name;

                            if (SysBL.ContainsKey(curJump.From.Name))
                            {
                                SysBL[curJump.From.Name].jb2 = fromDetails;
                            }
                            else
                            {
                                SysBL.Add(curJump.From.Name, new JBMoon(new PointF(srcRect.X + srcRect.Width, srcRect.Y), fromDetails, "", false));
                            }

                            if (SysBL.ContainsKey(curJump.To.Name))
                            {
                                SysBL[curJump.To.Name].jb2 = toDetails;
                            }
                            else
                            {
                                SysBL.Add(curJump.To.Name, new JBMoon(new PointF(dstRect.X + dstRect.Width, dstRect.Y), toDetails, "", false));
                            }
                        }

                        /* Line running from --> to */
                        SrcDest = DrawBezier(g, bridgePen, fromSystem, toSystem, scaledNoDraw, 3, Math.PI / 12);

                        /* From System Circle */
                        g.DrawEllipse(bridgePen, fromSystem.X - circleRadius, fromSystem.Y - circleRadius, circleRadius * 2,
                                        circleRadius * 2);

                        /* To System Circle */
                        g.DrawEllipse(bridgePen, toSystem.X - circleRadius, toSystem.Y - circleRadius, circleRadius * 2,
                                        circleRadius * 2);
                    }
                }
            }

            if ((MapScale > 0.5f) && PlugInData.Config.CFlags["Show Bridge Details"])
            {
                foreach (JBMoon jbs in SysBL.Values)
                {
                    fromSysT = jbs.SysLoc;
                    fromSysT.X -= 3;
                    fromSysT.Y -= 3;
                    
                    SizeF detailSize = g.MeasureString(jbs.jb1, _systemDetailFont);

                    if (DoingPrintImage)
                    {
                        using (
                            SolidBrush systemFontBrush =
                                    new SolidBrush(Color.FromArgb(0xFF, Color.Black)))
                            g.DrawString(jbs.jb1, _systemDetailFont, systemFontBrush, fromSysT);
                    }
                    else if (jbs.dimT)
                    {
                        using (
                            SolidBrush systemFontBrush =
                                    new SolidBrush(Color.FromArgb(0x20, Config.MapColors["System"])))
                            g.DrawString(jbs.jb1, _systemDetailFont, systemFontBrush, fromSysT);
                    }
                    else
                    {
                        using (
                            SolidBrush systemFontBrush =
                                    new SolidBrush(Color.FromArgb(0x80, Config.MapColors["System"])))
                            g.DrawString(jbs.jb1, _systemDetailFont, systemFontBrush, fromSysT);
                    }

                    if (!jbs.jb2.Equals(""))
                    {
                        toSysT = jbs.SysLoc;
                        toSysT.X -= 3;
                        toSysT.Y -= detailSize.Height + 3;

                        if (DoingPrintImage)
                        {
                            using (
                                SolidBrush systemFontBrush =
                                        new SolidBrush(Color.FromArgb(0xFF, Color.Black)))
                                g.DrawString(jbs.jb1, _systemDetailFont, systemFontBrush, fromSysT);
                        }
                        else if (jbs.dimT)
                        {
                            using (
                                 SolidBrush systemFontBrush =
                                         new SolidBrush(Color.FromArgb(0x20, Config.MapColors["System"])))
                                g.DrawString(jbs.jb2, _systemDetailFont, systemFontBrush, toSysT);
                        }
                        else
                        {
                            using (
                                 SolidBrush systemFontBrush =
                                         new SolidBrush(Color.FromArgb(0x80, Config.MapColors["System"])))
                                g.DrawString(jbs.jb2, _systemDetailFont, systemFontBrush, toSysT);
                        }
                    }
                }
            }
        }

        private void DrawJBImage(Graphics g, RectangleF Center)
        {
            float circleRadius = Center.Width / 2;
            Pen bridgePen;

            if (DoingPrintImage)
                bridgePen = new Pen(Color.FromArgb(250, Color.Black), _lineThickness);
            else
                bridgePen = new Pen(Color.FromArgb(250, Color.White), _lineThickness);

            g.DrawPie(bridgePen, Center.X - circleRadius, Center.Y - circleRadius, Center.Width, Center.Height, 210, 300);
        }

        private void DrawCynoImage(Graphics g, RectangleF Center, bool Jammer)
        {
            Pen bridgePen, fillPen;
            RectangleF smCent;
            float circleRadius = Center.Width / 2;

            if (Jammer)
                bridgePen = new Pen(Color.FromArgb(250, Color.Red), _lineThickness);
            else
                bridgePen = new Pen(Color.FromArgb(250, Color.Green), _lineThickness);

            fillPen = new Pen(Color.FromArgb(255, Config.MapColors["Background"]));
            if (Jammer)
            {
                g.DrawLine(bridgePen, new PointF((Center.Width / 2 + Center.X), Center.Y - 3), new PointF((Center.Width / 2 + Center.X), (Center.Y + Center.Height + 3)));
                g.DrawLine(bridgePen, new PointF(Center.X - 3, (Center.Height / 2 + Center.Y)), new PointF((Center.Width + Center.X + 3), (Center.Height / 2 + Center.Y)));
            }

            g.FillEllipse(fillPen.Brush, Center);
            g.DrawEllipse(bridgePen, Center);
            smCent = new RectangleF();
            smCent.X = Center.X + (0.3f * Center.Width);
            smCent.Y = Center.Y + (0.3f * Center.Height);
            smCent.Width = Center.Width * 0.4f;
            smCent.Height = Center.Height * 0.4f;
            g.FillEllipse(bridgePen.Brush, smCent);
        }

        private string GetFromJB(string mn, SolarSystem ss)
        {
            string sb, it;
            string[] rti;

            sb = mn.Replace(ss.Name, "p");
            sb = sb.Replace("- Moon", "m");
            sb = sb.Replace(" ", "");

            rti = sb.Split('m');
            rti[0] = rti[0].Replace("p", "");

            it = RomanToInt(rti[0]);

            sb = sb.Replace(rti[0], it);

            return sb;
        }

        private string RomanToInt(string rn)
        {
            switch (rn)
            {
                case "I":
                    return "1";
                case "II":
                    return "2";
                case "III":
                    return "3";
                case "IV":
                    return "4";
                case "V":
                    return "5";
                case "VI":
                    return "6";
                case "VII":
                    return "7";
                case "VIII":
                    return "8";
                case "IX":
                    return "9";
                case "X":
                    return "10";
                case "XI":
                    return "11";
                case "XII":
                    return "12";
                case "XIII":
                    return "13";
                case "XIV":
                    return "14";
                case "XV":
                    return "15";
                case "XVI":
                    return "16";
                case "XVII":
                    return "17";
                case "XVIII":
                    return "18";
                case "XIX":
                    return "19";
                case "XX":
                    return "20";
                case "XXI":
                    return "21";
                case "XXII":
                    return "22";
                case "XXIII":
                    return "23";
                case "XXIV":
                    return "24";
                case "XXV":
                    return "25";
                case "XXVI":
                    return "26";
                case "XXVII":
                    return "27";
                case "XXVIII":
                    return "28";
                case "XXIX":
                    return "29";
                case "XXX":
                    return "30";
            }

            return "0";
        }

        private void DrawGateJumps(Rectangle clipRect, Graphics g)
        {
            float scaledNoDraw = (MapScale < 0.5f) ? 0 : Config.Sizes["Sys Marker Size"] * MapScale;
            bool filter = IsFilterActive();

            Color normalGateColor = Color.FromArgb(Config.Settings["Normal Gate Alpha"], Config.MapColors["Normal Gate"]);
            Color constGateColor = Color.FromArgb(Config.Settings["Const Gate Alpha"], Config.MapColors["Constellation Gate"]);
            Color regionGateColor = Color.FromArgb(Config.Settings["Region Gate Alpha"], Config.MapColors["Region Gate"]);

            Pen normalPen = new Pen(normalGateColor, _lineThickness);
            Pen constellationPen = new Pen(constGateColor, _lineThickness);
            Pen regionPen = new Pen(regionGateColor, _lineThickness);
            Pen emptyPen = new Pen(Color.Empty, _lineThickness);

            PointF ScaledFromGate, ScaledToGate;
            Pen gatePen;

            for (int i = 0; i < GalMap.GalData.StarGates.Count; i++)
            {
                StarGate gate = GalMap.GalData.StarGates[i];

                if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(gate.From.ID)))
                    ScaledFromGate = ScalePoint(PlugInData.SystemCoords[Convert.ToInt64(gate.From.ID)].OrgCoord, Adjust, MapScale);
                else
                    ScaledFromGate = ScalePoint(gate.From.OrgCoord, Adjust, MapScale);

                if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(gate.To.ID)))
                    ScaledToGate = ScalePoint(PlugInData.SystemCoords[Convert.ToInt64(gate.To.ID)].OrgCoord, Adjust, MapScale);
                else
                    ScaledToGate = ScalePoint(gate.To.OrgCoord, Adjust, MapScale);

                switch (gate.Type)
                {
                    case GateType.Normal:
                        gatePen = normalPen;
                        break;
                    case GateType.InterConst:
                        gatePen = constellationPen;
                        break;
                    case GateType.InterRegion:
                        gatePen = regionPen;
                        break;
                    default:
                        gatePen = emptyPen;
                        break;
                }

                if (IsLineVisible(ScaledFromGate, ScaledToGate, clipRect, Adjust, MapScale))
                {
                    if (RMA.cbx_SelRegions.Checked)
                    {
                        if (PlugInData.RCSelList[GalMap.GalData.Regions[gate.From.RegionID].Name].Selected || PlugInData.RCSelList[GalMap.GalData.Regions[gate.To.RegionID].Name].Selected)
                        {
                            DrawGateLine(g, gatePen, gate, scaledNoDraw);
                            if (!PlugInData.RCSelList[GalMap.GalData.Regions[gate.From.RegionID].Name].Selected)
                            {
                                DrawSystemElements(g, gate.From, false, true);
                            }
                            else if (!PlugInData.RCSelList[GalMap.GalData.Regions[gate.To.RegionID].Name].Selected)
                            {
                                DrawSystemElements(g, gate.To, false, true);
                            }
                        }
                    }
                    else if (RMA.cbx_SelConst.Checked)
                    {
                         if (PlugInData.RCSelList[GalMap.GalData.Regions[gate.From.RegionID].Name].Constellations[GalMap.GalData.Regions[gate.From.RegionID].Constellations[gate.From.ConstID].Name].Selected ||
                            PlugInData.RCSelList[GalMap.GalData.Regions[gate.To.RegionID].Name].Constellations[GalMap.GalData.Regions[gate.To.RegionID].Constellations[gate.To.ConstID].Name].Selected)
                        {
                            DrawGateLine(g, gatePen, gate, scaledNoDraw);
                            if (!PlugInData.RCSelList[GalMap.GalData.Regions[gate.From.RegionID].Name].Constellations[GalMap.GalData.Regions[gate.From.RegionID].Constellations[gate.From.ConstID].Name].Selected)
                            {
                                DrawSystemElements(g, gate.From, false, true);
                            }
                            else if (!PlugInData.RCSelList[GalMap.GalData.Regions[gate.To.RegionID].Name].Constellations[GalMap.GalData.Regions[gate.To.RegionID].Constellations[gate.To.ConstID].Name].Selected)
                            {
                                DrawSystemElements(g, gate.To, false, true);
                            }
                        }
                    }
                    else if (RMA.cbx_SelGalaxy.Checked)
                        DrawGateLine(g, gatePen, gate, scaledNoDraw);
                }
            }
        }

        private void DrawGateLine(Graphics g, Pen pen, StarGate srcGate, float nodrawRadius)
        {
            PointF drawPos1;
            PointF drawPos2;

            if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(srcGate.From.ID)))
                drawPos1 = ScalePoint(PlugInData.SystemCoords[Convert.ToInt64(srcGate.From.ID)].OrgCoord, Adjust, MapScale);
            else
                drawPos1 = ScalePoint(srcGate.From.OrgCoord, Adjust, MapScale);

            if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(srcGate.To.ID)))
                drawPos2 = ScalePoint(PlugInData.SystemCoords[Convert.ToInt64(srcGate.To.ID)].OrgCoord, Adjust, MapScale);
            else
                drawPos2 = ScalePoint(srcGate.To.OrgCoord, Adjust, MapScale);

            if (nodrawRadius > 0) // shorten the line on both ends
            {
                float dx = drawPos2.X - drawPos1.X;
                float dy = drawPos2.Y - drawPos1.Y;
                float length = (float)(Math.Sqrt(dx * dx + dy * dy));

                float xDisplacement = dx / length;
                float yDisplacement = (float)Math.Sqrt(1 - xDisplacement * xDisplacement);

                int xOffset = (int)(nodrawRadius * xDisplacement);
                int yOffset = (int)(nodrawRadius * yDisplacement);

                drawPos1.X += xOffset;
                drawPos2.X -= xOffset;

                drawPos1.Y += (dy > 0) ? yOffset : -yOffset;
                drawPos2.Y -= (dy > 0) ? yOffset : -yOffset;
            }
            g.DrawLine(pen, drawPos1.X, drawPos1.Y, drawPos2.X, drawPos2.Y);
        }

        private void DrawRoute(Graphics g)
        {
            Rectangle dstRect;
            int scMult;
            Pen routePen = new Pen(Config.MapColors["Gate Route"], _lineThickness);
            Pen routeJumpPen = new Pen(Color.FromArgb(170, Config.MapColors["Jump Route"]), _lineThickness);
            Pen routeBridgePen = new Pen(Color.FromArgb(170, Config.MapColors["Jump Route"]), _lineThickness);

            float scaledNoDraw = (MapScale < 0.5f) ? 0 : Config.Sizes["Sys Marker Size"] * MapScale;
            float drawSizeExternal = (Config.Sizes["Sys Marker Size"] +
                                      Config.Sizes["Sys Marker Thickness"] + 6) *
                                     ((MapScale < 0.60f) ? 0.60f : MapScale);

            PointF start;
            PointF end;

            scMult = (int)(Config.Sizes["Sys Marker Size"] * 3 * MapScale);

            for (int i = 0; i < Route.Count - 1; i++)
            {
                if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(Route[i].SolarSystem.ID)))
                    start = ScalePoint(PlugInData.SystemCoords[Convert.ToInt64(Route[i].SolarSystem.ID)].OrgCoord, Adjust, MapScale);
                else
                    start = ScalePoint(Route[i].SolarSystem.OrgCoord, Adjust, MapScale);

                if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(Route[i + 1].SolarSystem.ID)))
                    end = ScalePoint(PlugInData.SystemCoords[Convert.ToInt64(Route[i + 1].SolarSystem.ID)].OrgCoord, Adjust, MapScale);
                else
                    end = ScalePoint(Route[i + 1].SolarSystem.OrgCoord, Adjust, MapScale);
                
                switch (Route[i + 1].JumpTyp)
                {
                    case PlugInData.JumpType.Undefined:
                    case PlugInData.JumpType.Gate:
                        g.DrawLine(routePen, start, end);
                        break;
                    case PlugInData.JumpType.Bridge:
                        DrawBezier(g, routeBridgePen, start, end, scaledNoDraw, 3, Math.PI / 8);

                        dstRect = new Rectangle((int)(end.X + 10), (int)(end.Y - (5 + scMult)), scMult, scMult);
                        g.DrawImage(imgJB, dstRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                        break;

                    case PlugInData.JumpType.Beacon:
                        g.DrawEllipse(routeJumpPen, start.X - drawSizeExternal / 2, start.Y - drawSizeExternal / 2,
                                      drawSizeExternal, drawSizeExternal);
                        DrawBezier(g, routeJumpPen, start, end, scaledNoDraw, 3, Math.PI / 8);
                        g.DrawEllipse(routeJumpPen, end.X - drawSizeExternal / 2, end.Y - drawSizeExternal / 2,
                                      drawSizeExternal, drawSizeExternal);

                        dstRect = new Rectangle((int)(end.X + 10), (int)(end.Y + 5), scMult, scMult);
                        g.DrawImage(imgG, dstRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                        break;

                    case PlugInData.JumpType.Cyno:
                        g.DrawEllipse(routeJumpPen, start.X - drawSizeExternal / 2, start.Y - drawSizeExternal / 2,
                                      drawSizeExternal, drawSizeExternal);
                        DrawBezier(g, routeJumpPen, start, end, scaledNoDraw, 3, Math.PI / 8);
                        g.DrawEllipse(routeJumpPen, end.X - drawSizeExternal / 2, end.Y - drawSizeExternal / 2,
                                      drawSizeExternal, drawSizeExternal);

                        dstRect = new Rectangle((int)(end.X + 10), (int)(end.Y + 5), scMult, scMult);
                        g.DrawImage(imgCyno, dstRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                        break;

                    case PlugInData.JumpType.CynoSafe:
                        g.DrawEllipse(routeJumpPen, start.X - drawSizeExternal / 2, start.Y - drawSizeExternal / 2,
                                      drawSizeExternal, drawSizeExternal);
                        DrawBezier(g, routeJumpPen, start, end, scaledNoDraw, 3, Math.PI / 8);
                        g.DrawEllipse(routeJumpPen, end.X - drawSizeExternal / 2, end.Y - drawSizeExternal / 2,
                                      drawSizeExternal, drawSizeExternal);

                        dstRect = new Rectangle((int)(end.X + 10), (int)(end.Y + 5), scMult, scMult);
                        g.DrawImage(imgCS, dstRect, 0, 0, 64, 64, GraphicsUnit.Pixel, attr);
                        break;

                    default:
                        throw new Exception("I broke the gate renderer.  I don't know how....");
                }
            }
        }

        public ArrayList DrawBezier(Graphics g, Pen pen, PointF start, PointF end, float nodrawRadius,
                              float lengthfraction, double angle)
        {
            ArrayList retPt = new ArrayList();
            float drawPosX1 = start.X, drawPosY1 = -start.Y, drawPosX2 = end.X, drawPosY2 = -end.Y;
            float dx = drawPosX2 - drawPosX1;
            float dy = drawPosY2 - drawPosY1;
            if (dx < 0)
            {
                drawPosX1 = end.X;
                drawPosY1 = -end.Y;
                drawPosX2 = start.X;
                drawPosY2 = -start.Y;
                dx = drawPosX2 - drawPosX1;
                dy = drawPosY2 - drawPosY1;
            }
            float length = (float)(Math.Sqrt(dx * dx + dy * dy));
            double alpha = (float)(Math.Acos(dx / length));
            if (dy < 0) // adjust for the right quadrant
            {
                if (dx > 0) alpha = -alpha;
                else alpha = Math.PI + Math.PI - alpha;
            }
            float bx1 = drawPosX1 + length / lengthfraction * (float)Math.Cos(alpha + angle);
            float by1 = drawPosY1 + length / lengthfraction * (float)Math.Sin(alpha + angle);
            float bx2 = drawPosX2 - length / lengthfraction * (float)Math.Cos(alpha - angle);
            float by2 = drawPosY2 - length / lengthfraction * (float)Math.Sin(alpha - angle);
            drawPosX1 += nodrawRadius * (float)Math.Cos(alpha);
            drawPosY1 += nodrawRadius * (float)Math.Sin(alpha);
            drawPosX2 -= nodrawRadius * (float)Math.Cos(alpha);
            drawPosY2 -= nodrawRadius * (float)Math.Sin(alpha);
            PointF src = new PointF(drawPosX1, -drawPosY1);
            PointF bezier1 = new PointF(bx1, -by1);
            PointF bezier2 = new PointF(bx2, -by2);
            PointF dest = new PointF(drawPosX2, -drawPosY2);
            g.DrawBezier(pen, src, bezier1, bezier2, dest);

            retPt.Add(src);
            retPt.Add(dest);

            return retPt;
        }

        private bool IsSystemInTable(DataTable dt, string sys)
        {
            foreach (DataRow r in dt.Rows)
            {
                if (r.ItemArray.Contains(sys))
                    return true;
            }

            return false;
        }

        private void CheckAndHighlightSearchResultsForSystem(Graphics g, SolarSystem system, PointF drawPos)
        {
            if (RMA.ActiveSearch.Equals(""))
                return;

            if (!IsSystemInTable(RMA.ds_SearchResult.Tables[RMA.ActiveSearch], system.Name))
                return;

            float circleRadius = Config.Sizes["Sys Marker Size"] * MapScale + 9;

            Pen bridgePen;
            bridgePen = new Pen(Color.FromArgb(170, PlugInData.Config.MapColors["Search Result Highlight"]), _lineThickness) { DashStyle = DashStyle.Dash };

            g.DrawEllipse(bridgePen, drawPos.X - circleRadius, drawPos.Y - circleRadius, circleRadius * 2,
                                  circleRadius * 2);
        }

        private void CheckAndHighlightJBRangeForSystem(Graphics g, SolarSystem system, PointF drawPos)
        {
            if ((_startSystem.GetJumpDistance(system) > 5.0) || (system.Security > 0))
                return;

            float circleRadius = Config.Sizes["Sys Marker Size"] * MapScale + 6;

            Pen bridgePen;
            bridgePen = new Pen(Color.FromArgb(170, PlugInData.Config.MapColors["Bridge Range Highlight"]), _lineThickness) { DashStyle = DashStyle.Dash };

            g.DrawEllipse(bridgePen, drawPos.X - circleRadius, drawPos.Y - circleRadius, circleRadius * 2,
                                  circleRadius * 2);
        }

        private void CheckAndHighlightShipRangeForSystem(Graphics g, SolarSystem system, PointF drawPos)
        {
            if (Config.SelPilot == null)
                return;

            //int JDC = Convert.ToInt32(Config.SelPilot.KeySkills[(int)EveHQ.Core.Pilot.KeySkill.JumpDriveCalibration]);

            if (_startSystem.GetJumpDistance(system) > RMA.CurShipJumpRange)
                return;

            float circleRadius = Config.Sizes["Sys Marker Size"] * MapScale + 3;

            Pen bridgePen;
            bridgePen = new Pen(Color.FromArgb(170, PlugInData.Config.MapColors["Ship Jump Range Highlight"]), _lineThickness) { DashStyle = DashStyle.Dash };

            g.DrawEllipse(bridgePen, drawPos.X - circleRadius, drawPos.Y - circleRadius, circleRadius * 2,
                                  circleRadius * 2);
        }

        private void CheckAndHighlightShipToRangeForSystem(Graphics g, SolarSystem system, PointF drawPos)
        {
            if (Config.SelPilot == null)
                return;

            //int JDC = Convert.ToInt32(Config.SelPilot.KeySkills[(int)EveHQ.Core.Pilot.KeySkill.JumpDriveCalibration]);

            if (_startSystem.GetJumpDistance(system) > RMA.CurShipJumpRange)
                return;

            if (system.Security >= 0.45)
                return;

            float circleRadius = (float)(Config.Sizes["Sys Marker Size"] * MapScale + 1);

            Pen bridgePen;
            bridgePen = new Pen(Color.FromArgb(170, PlugInData.Config.MapColors["Ship Jump To Range Highlight"]), _lineThickness) { DashStyle = DashStyle.Dash };

            g.DrawEllipse(bridgePen, drawPos.X - circleRadius, drawPos.Y - circleRadius, circleRadius * 2,
                                  circleRadius * 2);
        }


        #endregion

        #region Filtering

        private bool IsFilterActive()
        {
            //switch (FilterType)
            //{
            //    case SystemFilterType.Route:
            //        if (Route != null)
            //            return true;
            //        return false;
            //    case SystemFilterType.JumpRange:
            //        return true;
            //    case SystemFilterType.UserFilter:
            //        if (UnfilteredSystems != null)
            //            return true;
            //        return false;
            //    default:
            //        return false;
            //}
            return false;
        }

        private bool SystemIsUnfiltered(SolarSystem system)
        {
            if (UnfilteredSystems != null && UnfilteredSystems.ContainsKey(system.ID))
                return true;

            if (Route == null)
                return false;

            foreach (var route in Route)
            {
                if (route.SolarSystem == system)
                    return true;
            }

            return false;
        }

        #endregion

        #region Events and Movements

        public void SetSelectedStartSystem(string selSys)
        {
            SolarSystem ss = GalMap.GetSystemByName(selSys);
            _startSystem = ss;
            Invalidate();
        }

        private void MapViewControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;

            if (!InitMapSettings)
                return;

            if (PlugInData.rebuild)
                return;

            e.Graphics.Clear(Config.MapColors["Background"]);
            //FlatDataChanged = true;
            FillTerritoryParameters();

            // we extend it slightly to avoid clipping off AAed edges of things, for example gate lines
            e.ClipRectangle.Inflate(2, 2);

            DrawConnections(e.Graphics, e.ClipRectangle);
            DrawSystems(e.Graphics, e.ClipRectangle);
            UpdateSmallMapPosition();
        }

        private void MapViewControl_MouseDown(object sender, MouseEventArgs e)
        {
            double radius;
            SolarSystem selSys;

            if (e.Button == MouseButtons.Left)
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    // New mode - system location movement
                    radius = Config.Sizes["Sys Marker Size"] * ((MapScale < 0.8f) ? 0.8f : MapScale);
                    selSys = FindSystemOnScreen(e.Location, radius);

                    if (selSys != null)
                    {
                        _selectedSystem = selSys;
                        _sysDragging = true;
                        if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(_selectedSystem.ID)))
                            _sysDrag = PlugInData.SystemCoords[Convert.ToInt64(_selectedSystem.ID)].OrgCoord;
                        else
                            _sysDrag = _selectedSystem.OrgCoord;

                        _sysStart = e.Location;
                        _sysDragOffset.X = sb_HorizontalScroll.Value;
                        _sysDragOffset.Y = sb_VerticalScroll.Value;
                    }
                }
                else if (_scrollEnabled)
                {
                    // left_click + drag == drag map around
                    _mapDragging = true;
                    _mapDrag = e.Location;
                    _mapDragOffset.X = sb_HorizontalScroll.Value;
                    _mapDragOffset.Y = sb_VerticalScroll.Value;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Check for System Selection
            }
        }

        private void MapViewControl_MouseWheel(object sender, MouseEventArgs e)
        {
            MWCenter.X = e.X;
            MWCenter.Y = e.Y;

            if (e.Delta < 0)
            {
                MouseZoom = true;
                b_ZoomIn_Click(sender, new EventArgs());
                //RMA.tsb_ZoomIn_Click(sender, new EventArgs());
            }
            else
            {
                MouseZoom = true;
                b_ZoomOut_Click(sender, new EventArgs());
                //RMA.tsb_ZoomOut_Click(sender, new EventArgs());
            }
        }

        private void MapViewControl_MouseMove(object sender, MouseEventArgs e)
        {
            PointF loc, strt;
            
            if (_mapDragging || _sysDragging)
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
            else if (_sysDragging)
            {
                Cursor = Cursors.Hand;

                strt = ScalePoint(_sysDrag, Adjust, MapScale);
                Point newScroll = new Point((int)(strt.X - (_sysStart.X - e.X)),
                                            (int)(strt.Y - (_sysStart.Y - e.Y)));

                loc = UnscalePoint(new PointF(newScroll.X, newScroll.Y), Adjust, MapScale);

                if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(_selectedSystem.ID)))
                    PlugInData.SystemCoords[Convert.ToInt64(_selectedSystem.ID)].OrgCoord = new Point(Convert.ToInt32(loc.X), Convert.ToInt32(loc.Y));
                else
                    _selectedSystem.OrgCoord = new Point(Convert.ToInt32(loc.X), Convert.ToInt32(loc.Y));

                Invalidate();
                //ScrollToPosition(newScroll);
            }
        }

        private void MapViewControl_MouseUp(object sender, MouseEventArgs e)
        {
            double radius;
            SolarSystem selSys;
            PointF loc, strt;

            Cursor = Cursors.Default;

            if (_mapDragging)
            {
                Point newScroll = new Point(
                    (int)(2 * (_mapDrag.X - e.X) / MapScale + _mapDragOffset.X),
                    (int)(2 * (_mapDrag.Y - e.Y) / MapScale + _mapDragOffset.Y));

                ScrollToPosition(newScroll);
                _mapDragging = false;
            }

            if (_sysDragging)
            {
                strt = ScalePoint(_sysDrag, Adjust, MapScale);
                Point newScroll = new Point((int)(strt.X - (_sysStart.X - e.X)),
                                            (int)(strt.Y - (_sysStart.Y - e.Y)));

                loc = UnscalePoint(new PointF(newScroll.X, newScroll.Y), Adjust, MapScale);
                if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(_selectedSystem.ID)))
                    PlugInData.SystemCoords[Convert.ToInt64(_selectedSystem.ID)].OrgCoord = new Point(Convert.ToInt32(loc.X), Convert.ToInt32(loc.Y));
                else
                    _selectedSystem.OrgCoord = new Point(Convert.ToInt32(loc.X), Convert.ToInt32(loc.Y));
                //ScrollToPosition(newScroll);
                Invalidate();
                _sysDragging = false;

                RMA.MapDataChanged = true;
            }

            radius = Config.Sizes["Sys Marker Size"] * ((MapScale < 0.8f) ? 0.8f : MapScale);
            selSys = FindSystemOnScreen(e.Location, radius);

            if (selSys != null)
            {
                _selectedSystem = selSys;
                OnSystemSelected();
                Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                cms_MapRClick.Hide();
            }

        }

         public void SearchSelectedSystem(string selSys)
        {
            SolarSystem ss = GalMap.GetSystemByName(selSys);
            _selectedSystem = ss;
            OnSystemSelected();

            if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(ss.ID)))
                CenterOnPoint(PlugInData.SystemCoords[Convert.ToInt64(ss.ID)].OrgCoord);
            else
                CenterOnPoint(ss.OrgCoord);
    
             Invalidate();

            // Update local TB if needed
            LocalUpdate = true;
            cb_FindSystem.Text = ss.Name;
            LocalUpdate = false;
        }

        private void sb_VerticalScroll_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }

        private void sb_HorizontalScroll_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }

        private void MapView_Resize(object sender, EventArgs e)
        {
            FlatDataChanged = true;
        }

        private SolarSystem FindSystemOnScreen(PointF location, double radius)
        {
            PointF sysLoc;

            foreach (var pReg in GalMap.GalData.Regions)
            {
                if (PlugInData.WHMapSelected)
                {
                    if (!PlugInData.WHRegions.Contains(pReg.Value.ID))
                        continue;
                }
                else
                {
                    if (PlugInData.SkipRegions.Contains(pReg.Value.ID))
                        continue;
                }

                foreach (var cnst in pReg.Value.Constellations)
                {
                    foreach (var pair in cnst.Value.Systems)
                    {
                        if (IsSystemSelectable(pair.Value))
                        {
                            if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(pair.Value.ID)))
                                sysLoc = ScalePoint(PlugInData.SystemCoords[Convert.ToInt64(pair.Value.ID)].OrgCoord, Adjust, MapScale);
                            else
                                sysLoc = ScalePoint(pair.Value.OrgCoord, Adjust, MapScale);

                            float xDiff = sysLoc.X - location.X;
                            float yDiff = sysLoc.Y - location.Y;
                            float curRadius = xDiff * xDiff + yDiff * yDiff;
                            if (curRadius < radius * radius) return pair.Value;
                        }
                    }
                }
            }

            return null;
        }

        private bool IsSystemSelectable(SolarSystem system)
        {
            //if (_mapScale < 0.50f)
            //    return DynamicStore.Instance.CynoBeacons.ContainsKey(system);
            return true;
        }

        private void tsmi_SetAsStart_Click(object sender, EventArgs e)
        {
            RMA.MapViewSetStart(_selectedSystem);
            _startSystem = new SolarSystem(_selectedSystem);
        }

        private void tsmi_SetAsDestination_Click(object sender, EventArgs e)
        {
            RMA.MapViewSelectEnd(_selectedSystem);
        }

        private void tsmi_AddAsWaypoint_Click(object sender, EventArgs e)
        {
            RMA.MapViewSelectWaypoint(_selectedSystem);
        }

        private void tsmi_AddToAvoid_Click(object sender, EventArgs e)
        {
            RMA.MapViewSelectAvoid(_selectedSystem);
        }

        private void tsmi_ShowInfo_Click(object sender, EventArgs e)
        {
            RMA.MapViewShowSystemInformation();
        }

        private void tsmi_ViewSystemMap_Click(object sender, EventArgs e)
        {
            RMA.MapViewSelectSystemView(_selectedSystem);
        }

        public void ToggleJBRangeHL()
        {
            if (HLJBRange)
            {
                HLJBRange = false;
                tsmi_HL_JB_Range.Text = "Enable Bridge Range Highlight";
            }
            else
            {
                HLJBRange = true;
                tsmi_HL_JB_Range.Text = "Disable Bridge Range Highlight";
            }

            Invalidate();
        }

        private void tsmi_HL_JB_Range_Click(object sender, EventArgs e)
        {
            ToggleJBRangeHL();
        }

        public void ToggleShipJumpRangeHL()
        {
            if (HLShipJumpRange)
            {
                HLShipJumpRange = false;
                tsmi_HL_ShipJumpRange.Text = "Enable Ship Jump From Range Highlight";
            }
            else
            {
                HLShipJumpRange = true;
                tsmi_HL_ShipJumpRange.Text = "Disable Ship Jump From Range Highlight";
            }

            Invalidate();
        }

        private void tsmi_HL_ShipJumpRange_Click(object sender, EventArgs e)
        {
            ToggleShipJumpRangeHL();
        }

        public void ToggleShipJumpToRangeHL()
        {
            if (HLShipJumpToRange)
            {
                HLShipJumpToRange = false;
                tsmi_ShipJumpToRange.Text = "Enable Ship Jump To Range Highlight";
            }
            else
            {
                HLShipJumpToRange = true;
                tsmi_ShipJumpToRange.Text = "Disable Ship Jump To Range Highlight";
            }

            Invalidate();
        }

        private void tsmi_ShipJumpToRange_Click(object sender, EventArgs e)
        {
            ToggleShipJumpToRangeHL();
        }

        public void ToggleSearchResultHL()
        {
            if (HLSearchResult)
                HLSearchResult = false;
            else
                HLSearchResult = true;

            Invalidate();
        }

        private void cb_FindSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sSys;

            if (!LocalUpdate)
            {
                sSys = cb_FindSystem.Text;

                SetMapScale = Config.Zooms["Zoom On Search"];
                SearchSelectedSystem(sSys);
                this.SelectNextControl(cb_FindSystem, true, true, true, true);
            }
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

         #endregion

        #region Small Map Updating

        private void UpdateSmallMapPosition()
        {
            SMap.UpdateSmallMapHighlight(sb_HorizontalScroll.Value, sb_HorizontalScroll.Maximum, sb_VerticalScroll.Value, sb_VerticalScroll.Maximum, MapSize, MapScale);
        }

        #endregion

        #region Map Settings Handler

        public Rectangle GetRouteBounds()
        {
            SystemCoordinates sys;
            Rectangle _bounds;
            SolarSystem ss;

            Point Min = new Point(int.MaxValue, int.MaxValue);
            Point Max = new Point(int.MinValue, int.MinValue);

            foreach (Vertex vt in Route)
            {
                ss = vt.SolarSystem;
                if (PlugInData.SystemCoords.ContainsKey(Convert.ToInt64(ss.ID)))
                {
                    sys = PlugInData.SystemCoords[Convert.ToInt64(ss.ID)];

                    if (sys.OrgCoord.X < Min.X)
                        Min.X = sys.OrgCoord.X;
                    if (sys.OrgCoord.Y < Min.Y)
                        Min.Y = sys.OrgCoord.Y;
                    if (sys.OrgCoord.X > Max.X)
                        Max.X = sys.OrgCoord.X;
                    if (sys.OrgCoord.Y > Max.Y)
                        Max.Y = sys.OrgCoord.Y;
                }
                else
                {
                    if (ss.OrgCoord.X < Min.X)
                        Min.X = ss.OrgCoord.X;
                    if (ss.OrgCoord.Y < Min.Y)
                        Min.Y = ss.OrgCoord.Y;
                    if (ss.OrgCoord.X > Max.X)
                        Max.X = ss.OrgCoord.X;
                    if (ss.OrgCoord.Y > Max.Y)
                        Max.Y = ss.OrgCoord.Y;
                }
            }
            _bounds = new Rectangle(Min.X, Min.Y, Max.X - Min.X, Max.Y - Min.Y);

            return _bounds;
        }

        public void MapViewSettingChanged()
        {
            Invalidate();
        }

        #endregion

        public void PrintMapView()
        {
            ppd_MapPreview.Width = this.Width;
            ppd_MapPreview.Height = this.Height;
            ppd_MapPreview.Document = pd_MainMap;
            ppd_MapPreview.Document.DefaultPageSettings.Landscape = true;

            if (pdlg_MapDialog.ShowDialog() == DialogResult.OK)
            {
                ppd_MapPreview.Document.DefaultPageSettings = pdlg_MapDialog.PrinterSettings.DefaultPageSettings;

                if (!pdlg_MapDialog.PrinterSettings.SupportsColor)
                    DoingPrintImage = true;

                ppd_MapPreview.ShowDialog();
            }
        }

        private void pd_MainMap_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics gr;
            Bitmap printBMP;
            Rectangle ClipRect;

            printBMP = new Bitmap(this.Width, this.Height);

            gr = Graphics.FromImage(printBMP);

            gr.SmoothingMode = SmoothingMode.HighQuality;
            gr.TextRenderingHint = TextRenderingHint.SystemDefault;

            gr.Clear(Color.White);

            FillTerritoryParameters();

            // we extend it slightly to avoid clipping off AAed edges of things, for example gate lines
            gr.ClipBounds.Inflate(50000, 50000);

            ClipRect = new Rectangle(Convert.ToInt32(gr.ClipBounds.X), Convert.ToInt32(gr.ClipBounds.Y), Convert.ToInt32(gr.ClipBounds.Width), Convert.ToInt32(gr.ClipBounds.Height));
            
            DrawConnections(gr, ClipRect);
            DrawSystems(gr, ClipRect);

            DoingPrintImage = false;
            //Image img = new Bitmap(this.);
            e.Graphics.DrawImage(printBMP, 0, 0);
        }

        #region Print Handler

        // Would want to get the visible section of the current map view window
        // Get all details for said map window
        // Copy print section of bitmap to new BMP
        // Change background color to white
        // Modify other colors as needed (for B&W image - maybe)
        // Do print preview, and then send to printer if accepted

        // Later
        // 1. Allow for print of a larger area over multiple pages if desired.
        //    This would require re-drawing onto a now custom canvas

        #endregion
    }
}
