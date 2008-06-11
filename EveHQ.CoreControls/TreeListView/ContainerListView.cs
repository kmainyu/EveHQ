/***************************************************************************\
|  Author:  Josh Carlson                                                    |
|                                                                           |
|  This work builds on code posted to CodeProject                           |
|  Jon Rista http://codeproject.com/cs/miscctrl/extendedlistviews.asp       |
|  and also updates by                                                      |
|  Bill Seddon http://codeproject.com/cs/miscctrl/Extended_List_View_2.asp  |
|                                                                           |
|  This code is provided "as is" and no warranty about                      |
|  it fitness for any specific task is expressed or                         |
|  implied.  If you choose to use this code, you do so                      |
|  at your own risk.                                                        |
\***************************************************************************/

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DotNetLib.Windows.Forms
{
	#region ContainerListView Delegates

	public delegate void ContextMenuEventHandler(object sender, MouseEventArgs e);
	public delegate void ItemMenuEventHandler(object sender, MouseEventArgs e);
	public delegate void HeaderMenuEventHandler(object sender, MouseEventArgs e);

	#endregion

	/// <summary>
	/// Provides a ListView control in detail mode that provides containers for each cell in a row/column.
	/// The container can hold almost any object that derives directly or indirectly from Control.
	/// </summary>
	[
	DefaultProperty("Items"),
	DesignerAttribute(typeof(Design.ContainerListViewDesigner)),
	ToolboxItem(true),
	ToolboxBitmap(typeof(ContainerListView), "Resources.listview.bmp"),
	DefaultEvent("SelectedItemsChanged")
	]
	public class ContainerListView : Control
	{
		#region Variables

		private ItemActivation _activationMethod = ItemActivation.Standard;

		private BorderStyle _borderStyle = BorderStyle.Fixed3D;
		private ColumnHeaderStyle _headerStyle = ColumnHeaderStyle.Clickable;
		private ContainerListViewColumnHeaderCollection _columns;
		private ArrayList _columnDisplayOrder;
		private ArrayList _columnsSorted;

		private int _defaultItemHeight;

		private int _userHeaderHeight = 20;
		private int _headerHeight = 20;
		private int _borderSize = 2;

		private ContainerListViewItem _rootItem;

		private bool _allowColumnReorder = false;
		private bool _allowColumnResize = true;
		private bool _allowMultipleColumnSort = false;
		private bool _allowMultipleSelect = false;

		private bool _showRootTreeLines = false;
		private bool _showPlusMinus = false;
		private bool _showTreeLines = false;
		private GridLines _gridLines = GridLines.None;

		private bool _hideSelection = true;
		private bool _hoverSelection = false;
		private bool _itemTracking = false;
		private bool _columnTracking = false;
		private bool _fullItemSelect = true;
		private bool _captureFocusClick = true;
		private bool _visualStyles = true;

		private ContainerListViewItemCollection _items;
		private ImageList _smallImageList, _selectedImageList;

		private ContainerListViewColumnHeader _hoveredColumnHeader;
		private ContainerListViewColumnHeader _pressedColumnHeader;
		private ContainerListViewColumnHeader _sizingColumnHeader;

		private int _sizingColumnDelta = 0;
		private int _sizingColumnOriginalWidth = 0;
		private bool _sizingColumn = false;

		private bool _reorderingColumn = false;
		private int _reorderingColumnDelta = 0;
		private int _reorderingColumnDropIndex = -1;

		private ContainerListViewSelectedItemCollection _selectedItems = null;
		private ContainerListViewItem _lastUserSelectedItem = null;
		/// <summary>Last item clicked by the user (without any control key down). Used in OnMouseUp.</summary>
		ContainerListViewItem _lastUserSingleClickedItem;

		private ContainerListViewItem _focusedItem;
		private ContainerListViewItem _hoveredItem;

		// various rectangles
		/// <summary>the visible portion of the header control</summary>
		private Rectangle _headerVisibleRect;
		/// <summary>the visible portion of the detail area</summary>
		internal Rectangle _detailVisibleRect;
		/// <summary>the rectangle representing the entire grid</summary>
		private Rectangle _detailRect;

		/// <summary>each column header and the filler column</summary>
		internal Rectangle[] _headerColumnRects;
		/// <summary>each area that will allow column resizing</summary>
		private Rectangle[] _headerColumnSizeRects;

		private ContextMenuStrip _headerMenu, _itemMenu, _contextMenu;
		private HScrollBar _hScrollBar;
		private VScrollBar _vScrollBar;
		/// <summary></summary>
		private bool _isScrollingUp;
		private ToolTip _toolTipControl;

		private Stack _updateTransactions = new Stack();

		private Color _itemSelectedColor = SystemColors.Highlight;

		private SolidBrush _columnTrackingBrush = new SolidBrush(Color.WhiteSmoke);
		private SolidBrush _itemTrackingBrush = new SolidBrush(Color.WhiteSmoke);
		private SolidBrush _columnSortBrush = new SolidBrush(Color.Gainsboro);
		private Pen _gridLinePen = new Pen(Color.WhiteSmoke);
		private TextureBrush _backgroundImageBrush = null;

		private Point _lastClickedPoint;

		private ArrayList _visibleItems = new ArrayList();
		#endregion

		#region Properties
		[
		Category("Appearance"),
		Description("The default background color of items in the list."),
		DefaultValue(typeof(Color), "Window")
		]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		[
		Category("Appearance"),
		Description("Specifies the height of the header."),
		DefaultValue(20)
		]
		public int HeaderHeight
		{
			get
			{
				return _userHeaderHeight;
			}
			set
			{
				if (value != _userHeaderHeight)
				{
					_userHeaderHeight = value;
					HeaderStyle = HeaderStyle;

					RecalculateLayout(true, false);
					Invalidate();
				}
			}
		}

		[
		Category("Appearance"),
		Description("Specifies whether the control will attempt to use Visual Styles when rendering the control."),
		DefaultValue(true)
		]
		public bool VisualStyles
		{
			get
			{
				return _visualStyles;
			}
			set
			{
				if (_visualStyles != value)
				{
					_visualStyles = value;
					BorderStyle = BorderStyle; // force this
				}
			}
		}

		[
		Category("Behavior"),
		Description("Specifies wether the control will capture the click used to focus the control and adjust the selection accordingly, or not."),
		DefaultValue(true)
		]
		public bool CaptureFocusClick
		{
			get
			{
				return _captureFocusClick;
			}
			set
			{
				_captureFocusClick = value;
			}
		}

		[
		Category("Behavior"),
		Description("The context menu displayed when the header is right-clicked."),
		DefaultValue(null)
		]
		public ContextMenuStrip HeaderContextMenu
		{
			get
			{
				return _headerMenu;
			}
			set
			{
				_headerMenu = value;
			}
		}

		[
		Category("Behavior"),
		Description("The context menu displayed when an item is right-clicked."),
		DefaultValue(null)
		]
		public ContextMenuStrip ItemContextMenu
		{
			get
			{
				return _itemMenu;
			}
			set
			{
				_itemMenu = value;
			}
		}

		[
		Category("Behavior"),
		Description("The context menu displayed when the control is right-clicked."),
		DefaultValue(null)
		]
		public override ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return _contextMenu;
			}
			set
			{
				_contextMenu = value;
			}
		}

		[
		Category("Behavior"),
		Description("The lists column headers."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Editor(typeof(CollectionEditor), typeof(UITypeEditor))
		]
		public ContainerListViewColumnHeaderCollection Columns
		{
			get
			{
				return _columns;
			}
		}

		[
		Category("Data"),
		Description("The items contained at the root of the list."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Editor(typeof(CollectionEditor), typeof(UITypeEditor))
		]
		public virtual ContainerListViewItemCollection Items
		{
			get { return _items; }
		}

		[
		Category("Behavior"),
		Description("Specifies what action activates and item."),
		DefaultValue(ItemActivation.Standard)
		]
		public ItemActivation Activation
		{
			get
			{
				return _activationMethod;
			}
			set
			{
				_activationMethod = value;
			}
		}

		[
		Category("Behavior"),
		Description("Specifies whether column headers may be reordered."),
		DefaultValue(false)
		]
		public bool AllowColumnReorder
		{
			get
			{
				return _allowColumnReorder;
			}
			set
			{
				_allowColumnReorder = value;
			}
		}

		[
		Category("Behavior"),
		Description("Specifies whether column headers may be resized."),
		DefaultValue(true)
		]
		public bool AllowColumnResize
		{
			get
			{
				return _allowColumnResize;
			}
			set
			{
				if (_allowColumnResize == value)
					return;

				_allowColumnResize = value;
				RecalculateLayout(false, false);
			}
		}

		[
		Category("Appearance"),
		Description("Specifies what style border the control has."),
		DefaultValue(BorderStyle.Fixed3D)
		]
		public BorderStyle BorderStyle
		{
			get
			{
				return _borderStyle;
			}
			set
			{
				if (_borderStyle == value)
					return;

				_borderStyle = value;

				if (UseVisualStyle || _borderStyle == BorderStyle.Fixed3D)
					_borderSize = 2;
				else if (_borderStyle == BorderStyle.FixedSingle)
					_borderSize = 1;
				else
					_borderSize = 0;

				RecalculateLayout(true, false);
				Invalidate();
			}
		}

		[
		Category("Appearance"),
		Description("Specifies wether to show column headers, and whether they respond to mouse clicks."),
		DefaultValue(ColumnHeaderStyle.Clickable)
		]
		public ColumnHeaderStyle HeaderStyle
		{
			get
			{
				return _headerStyle;
			}
			set
			{
				if (_headerStyle == value)
					return;

				_headerStyle = value;
				if (_headerStyle == ColumnHeaderStyle.None)
					_headerHeight = 0;
				else
					_headerHeight = _userHeaderHeight;

				RecalculateLayout(false, false);
				Invalidate();
			}
		}

		[
		Category("Behavior"),
		Description("Enables column tracking, highlighting the column when the mouse hovers over its header."),
		DefaultValue(false)
		]
		public bool ColumnTracking
		{
			get
			{
				return _columnTracking;
			}
			set
			{
				_columnTracking = value;
			}
		}

		[
		Category("Appearance"),
		Description("Specifies the color used for column hot-tracking."),
		DefaultValue(typeof(Color), "WhiteSmoke")
		]
		public Color ColumnTrackingColor
		{
			get
			{
				return _columnTrackingBrush.Color;
			}
			set
			{
				_columnTrackingBrush.Color = value;
			}
		}

		[
		Category("Appearance"),
		Description("Specifies the color used for the currently selected sorting column."),
		DefaultValue(typeof(Color), "Gainsboro")
		]
		public Color ColumnSortColor
		{
			get
			{
				return _columnSortBrush.Color;
			}
			set
			{
				_columnSortBrush.Color = value;
			}
		}

		[
		Category("Behavior"),
		Description("Enables item tracking, highlighting the item gray when the mouse hovers over it."),
		DefaultValue(false)
		]
		public bool ItemTracking
		{
			get
			{
				return _itemTracking;
			}
			set
			{
				_itemTracking = value;
			}
		}

		[
		Category("Appearance"),
		Description("Specifies the color used for item hot-tracking."),
		DefaultValue(typeof(Color), "WhiteSmoke")
		]
		public Color ItemTrackingColor
		{
			get
			{
				return _itemTrackingBrush.Color;
			}
			set
			{
				_itemTrackingBrush.Color = value;
			}
		}

		[
		Category("Appearance"),
		Description("Specifies the color used for selected items."),
		DefaultValue(typeof(Color), "Highlight")
		]
		public Color ItemSelectedColor
		{
			get
			{
				return _itemSelectedColor;
			}
			set
			{
				_itemSelectedColor = value;
			}
		}

		[
		Category("Behavior"),
		Description("Determines wether to highlight the full item or just the label of selected items."),
		DefaultValue(true)
		]
		public bool FullItemSelect
		{
			get
			{
				return _fullItemSelect;
			}
			set
			{
				_fullItemSelect = value;
			}
		}

		[
		Category("Appearance"),
		Description("Specifies the color used for grid lines."),
		DefaultValue(typeof(Color), "WhiteSmoke")
		]
		public Color GridLineColor
		{
			get
			{
				return _gridLinePen.Color;
			}
			set
			{
				_gridLinePen.Color = value;
			}
		}

		[
		Category("Behavior"),
		Description("Specifies wether to show grid lines."),
		DefaultValue(false)
		]
		public bool ShowPlusMinus
		{
			get
			{
				return _showPlusMinus;
			}
			set
			{
				if (_showPlusMinus == value)
					return;

				_showPlusMinus = value;
				RecalculateItemPositions(_rootItem);
				Invalidate();
			}
		}

		[
		Category("Behavior"),
		Description("Specifies wether to show grid lines."),
		DefaultValue(false)
		]
		public bool ShowRootTreeLines
		{
			get
			{
				return _showRootTreeLines;
			}
			set
			{
				if (_showRootTreeLines == value)
					return;

				_showRootTreeLines = value;
				RecalculateItemPositions(_rootItem);
				Invalidate();
			}
		}

		[
		Category("Appearance"),
		Description("The default item height for items."),
		DefaultValue(17)
		]
		public int DefaultItemHeight
		{
			get
			{
				return _defaultItemHeight;
			}
			set
			{
				if (_defaultItemHeight == value)
					return;

				_defaultItemHeight = value;
				_vScrollBar.SmallChange = value;
				RecalculateItemPositions(_rootItem);
				Invalidate();
			}
		}

		[
		Category("Behavior"),
		Description("Specifies whether to show grid lines."),
		DefaultValue(false)
		]
		public bool ShowTreeLines
		{
			get
			{
				return _showTreeLines;
			}
			set
			{
				if (_showTreeLines == value)
					return;

				_showTreeLines = value;
				RecalculateItemPositions(_rootItem);
				Invalidate();
			}
		}

		[
		Category("Behavior"),
		Description("Specifies whether and which grid lines to show."),
		DefaultValue(GridLines.None)
		]
		public GridLines GridLines
		{
			get
			{
				return _gridLines;
			}
			set
			{
				if (_gridLines == value)
					return;

				_gridLines = value;
				Invalidate();
			}
		}

		[
		Category("Behavior"),
		Description("The lists small image list (16x16)."),
		DefaultValue(null)
		]
		public ImageList SmallImageList
		{
			get
			{
				return _smallImageList;
			}
			set
			{
				if (_smallImageList == value)
					return;

				_smallImageList = value;
				Invalidate();
			}
		}

		[
		Category("Behavior"),
		Description("The lists custom state image list (16x16)."),
		DefaultValue(null)
		]
		public ImageList SelectedImageList
		{
			get
			{
				return _selectedImageList;
			}
			set
			{
				_selectedImageList = value;
			}
		}

		[
		Category("Behavior"),
		Description("Determines whether to hide the selected items when the control looses focus."),
		DefaultValue(true)
		]
		public bool HideSelection
		{
			get
			{
				return _hideSelection;
			}
			set
			{
				if (_hideSelection != value)
				{
					_hideSelection = value;
					Invalidate();
				}
			}
		}

		[
		Category("Behavior"),
		Description("Determines whether to automatically select a item when the mouse is hovered over it for a short time."),
		DefaultValue(false)
		]
		public bool HoverSelection
		{
			get
			{
				return _hoverSelection;
			}
			set
			{
				_hoverSelection = value;
			}
		}

		[
		Category("Behavior"),
		Description("Determines whether the control will allow multiple selections."),
		DefaultValue(false)
		]
		public bool AllowMultiSelect
		{
			get
			{
				return _allowMultipleSelect;
			}
			set
			{
				_allowMultipleSelect = value;

				// need to deselect all but the focused one (and if none focused, the top one)
				if (!_allowMultipleSelect && _selectedItems.Count > 1)
				{
					if (_focusedItem != null)
						SetItemSelected(_focusedItem, true, true, false);
					else
						SetItemSelected(_selectedItems[0], true, true, false);
				}
			}
		}

		[
		Category("Behavior"),
		Description("Determines whether the control will allow a multiple column sort."),
		DefaultValue(false)
		]
		public bool MultipleColumnSort
		{
			get
			{
				return _allowMultipleColumnSort;
			}
			set
			{
				_allowMultipleColumnSort = value;
			}
		}

		[Browsable(false)]
		public ContainerListViewItem TopItem
		{
			get
			{
				return GetItemAt(_vScrollBar.Value);
			}
		}

		[Browsable(false)]
		public ContainerListViewItem BottomItemCompletelyVisible
		{
			get
			{
				ContainerListViewItem item = BottomItemPartiallyVisible;

				if (item.Y + item.Height > _vScrollBar.Value + _detailVisibleRect.Height)
					item = item.PreviousItem;

				return item;
			}
		}

		[Browsable(false)]
		public ContainerListViewItem BottomItemPartiallyVisible
		{
			get
			{
				ContainerListViewItem item = GetItemAt(_vScrollBar.Value + _detailVisibleRect.Height);
				if (item == null)
					item = _rootItem.VeryLastItem;

				return item;
			}
		}

		[Browsable(false)]
		public ContainerListViewColumnHeader[] SortColumns
		{
			get
			{
				return (ContainerListViewColumnHeader[])_columnsSorted.ToArray(typeof(ContainerListViewColumnHeader));
			}
		}

		[Browsable(false)]
		public ContainerListViewSelectedItemCollection SelectedItems
		{
			get
			{
				return _selectedItems;
			}
		}

		protected internal bool InUpdateTransaction
		{
			get
			{
				return _updateTransactions.Count > 0;
			}
		}

		internal bool UseVisualStyle
		{
			get
			{
				// Aulofee customization - start. Remove ref to C++ lib and directly access .NET theming classes.
				bool val = VisualStyles;

				val &= !DesignMode && Application.RenderWithVisualStyles;
				return val;
				// Aulofee customization - end.
			}
		}

		internal ContainerListViewItem RootItem
		{
			get
			{
				return _rootItem;
			}
		}

		#endregion

		#region Constructor
		public ContainerListView()
		{
			Construct();
		}

		private void Construct()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint |
				ControlStyles.ResizeRedraw |
				ControlStyles.Opaque |
				ControlStyles.UserPaint |
				ControlStyles.DoubleBuffer |
				ControlStyles.Selectable |
				ControlStyles.UserMouse |
				ControlStyles.ContainerControl,
				true);

			this.BackColor = SystemColors.Window;

			_columns = new ContainerListViewColumnHeaderCollection(this);
			_columnDisplayOrder = new ArrayList();
			_columnsSorted = new ArrayList();

			_rootItem = new ContainerListViewItem();
			_rootItem.OwnerListView = this;
			_rootItem.Height = 0;
			_rootItem.Expanded = true;

			_items = _rootItem.Items;
			_selectedItems = new ContainerListViewSelectedItemCollection(this);

			_headerHeight = 20;

			_defaultItemHeight = 20;

			_hScrollBar = new HScrollBar();
			_hScrollBar.Parent = this;
			_hScrollBar.Minimum = 0;
			_hScrollBar.Maximum = 0;
			_hScrollBar.SmallChange = 10;
			_hScrollBar.Hide();

			_vScrollBar = new VScrollBar();
			_vScrollBar.Parent = this;
			_vScrollBar.Minimum = 0;
			_vScrollBar.Maximum = 0;
			_vScrollBar.SmallChange = this.DefaultItemHeight;
			_vScrollBar.Hide();

			_hScrollBar.ValueChanged += new EventHandler(OnScroll);
			_vScrollBar.ValueChanged += new EventHandler(OnScroll);
			_vScrollBar.Scroll += new ScrollEventHandler(_vScrollBar_Scroll);
			RecalculateLayout(false, false);

			_toolTipControl = new ToolTip();
			_toolTipControl.InitialDelay = 2000;
			_toolTipControl.AutoPopDelay = 5000;
			_toolTipControl.ReshowDelay = 1000;
		}


		#endregion

		#region Events

		private EventHandler _selectedItemsChanged;
		private ContainerListViewEventHandler _columnClick;
		private ContainerListViewEventHandler _columnReordered;
		private ContainerListViewEventHandler _popColumnContextMenu;
		private ContainerListViewEventHandler _popItemContextMenu;
		private ContainerListViewEventHandler _popContextMenu;

		private ContainerListViewEventHandler _itemExpanded;
		private ContainerListViewEventHandler _itemCollapsed;

		private ContainerListViewCancelEventHandler _itemExpanding;
		private ContainerListViewCancelEventHandler _itemCollapsing;

		private ItemDragEventHandler _itemDrag;

		public event EventHandler SelectedItemsChanged
		{
			add
			{
				_selectedItemsChanged += value;
			}
			remove
			{
				_selectedItemsChanged -= value;
			}
		}

		public event ContainerListViewEventHandler ColumnClick
		{
			add
			{
				_columnClick += value;
			}
			remove
			{
				_columnClick -= value;
			}
		}

		public event ContainerListViewEventHandler PopColumnHeaderContextMenu
		{
			add
			{
				_popColumnContextMenu += value;
			}
			remove
			{
				_popColumnContextMenu -= value;
			}
		}


		public event ContainerListViewEventHandler ColumnReordered
		{
			add
			{
				_columnReordered += value;
			}
			remove
			{
				_columnReordered -= value;
			}
		}


		public event ContainerListViewEventHandler PopItemContextMenu
		{
			add
			{
				_popItemContextMenu += value;
			}
			remove
			{
				_popItemContextMenu -= value;
			}
		}

		public event ContainerListViewEventHandler PopContextMenu
		{
			add
			{
				_popContextMenu += value;
			}
			remove
			{
				_popContextMenu -= value;
			}
		}

		public event ContainerListViewCancelEventHandler ItemExpanding
		{
			add
			{
				_itemExpanding += value;
			}
			remove
			{
				_itemExpanding -= value;
			}
		}

		public event ContainerListViewEventHandler ItemExpanded
		{
			add
			{
				_itemExpanded += value;
			}
			remove
			{
				_itemExpanded -= value;
			}
		}

		public event ContainerListViewCancelEventHandler ItemCollapsing
		{
			add
			{
				_itemCollapsing += value;
			}
			remove
			{
				_itemCollapsing -= value;
			}
		}

		public event ContainerListViewEventHandler ItemCollapsed
		{
			add
			{
				_itemCollapsed += value;
			}
			remove
			{
				_itemCollapsed -= value;
			}
		}

		public event ItemDragEventHandler ItemDrag
		{
			add
			{
				_itemDrag += value;
			}
			remove
			{
				_itemDrag -= value;
			}
		}

		#region On* event fire-ers

		protected void OnColumnClick(ContainerListViewEventArgs e)
		{
			if (_columnClick != null)
				_columnClick(this, e);

			Sort(e.ColumnHeader.Index, true, !((ModifierKeys & Keys.Control) == Keys.Control));
		}

		protected void OnColumnReorder(ContainerListViewEventArgs e)
		{
			if (_columnReordered != null)
				_columnReordered(this, e);
		}

		protected void OnSelectedItemsChanged()
		{
			if (_selectedItemsChanged != null)
				_selectedItemsChanged(this, EventArgs.Empty);
		}

		protected void OnPopContextMenu(ContainerListViewEventArgs e)
		{
			if (_popContextMenu != null)
				_popContextMenu(this, e);

			PopMenu(_contextMenu, e);
		}

		protected void OnPopItemContextMenu(ContainerListViewEventArgs e)
		{
			if (_popItemContextMenu != null)
				_popItemContextMenu(this, e);

			if (_itemMenu != null)
				PopMenu(_itemMenu, e);
		}

		protected void OnPopColumnContextMenu(ContainerListViewEventArgs e)
		{
			if (_popColumnContextMenu != null)
				_popColumnContextMenu(this, e);

			if (_headerMenu == null)
				PopMenu(_contextMenu, e);
			else
				PopMenu(_headerMenu, e);
		}

		protected internal void OnItemExpanding(ContainerListViewCancelEventArgs e)
		{
			if (_itemExpanding != null)
				_itemExpanding(this, e);
		}

		protected internal void OnItemExpanded(ContainerListViewEventArgs e)
		{
			if (_itemExpanded != null)
				_itemExpanded(this, e);
		}

		protected internal void OnItemCollapsing(ContainerListViewCancelEventArgs e)
		{
			if (_itemCollapsing != null)
				_itemCollapsing(this, e);
		}

		protected internal void OnItemCollapsed(ContainerListViewEventArgs e)
		{
			if (_itemCollapsed != null)
				_itemCollapsed(this, e);
		}

		protected void OnItemDrag(ItemDragEventArgs e)
		{
			if (_itemDrag != null)
				_itemDrag(this, e);
		}

		#endregion

		#endregion

		protected internal virtual void SubItemItemControlMouseDown(ContainerListViewSubItem subItem)
		{
			if (!ContainsFocus && _captureFocusClick)
				Focus();

			MouseSelection(subItem.Item, MouseButtons.Left);
		}

		// Handler for vertical scrollbar scroll
		void _vScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			//this Scroll event is raised before the ValueChanged event
			_isScrollingUp = e.NewValue < e.OldValue;
			//Debug.WriteLine(e.OldValue + " " + e.NewValue);
		}

		// Handler for scrollbars scroll
		protected void OnScroll(object sender, EventArgs e)
		{
			if (!ContainsFocus)
				Focus();

			int vScrollValue = _vScrollBar.Value;

			if (_vScrollBar.Visible)
			{
				ContainerListViewItem firstVisibleItem = TopItem;

				if (firstVisibleItem == null)
					return;

                //if (vScrollValue != firstVisibleItem.Y)
                //{
                //    vScrollValue = firstVisibleItem.Y;
                //    if (_isScrollingUp)
                //    {
                //        //do nothing
                //    }
                //    else
                //    {
                //        //scrolling down
                //        firstVisibleItem = firstVisibleItem.NextVisibleItem;
                //    }
                //    if (firstVisibleItem != null)
                //        vScrollValue = firstVisibleItem.Y;
                //}

                //// if by chance we have "space" at the bottom, then remove it as best as possible
                //if (vScrollValue > _vScrollBar.Maximum - _vScrollBar.LargeChange)
                //{
                //    vScrollValue = (_vScrollBar.Maximum - _vScrollBar.LargeChange);

                //    firstVisibleItem = GetItemAt(vScrollValue);

                //    if (firstVisibleItem != null)
                //    {
                //        firstVisibleItem = firstVisibleItem.NextVisibleItem;

                //        if (firstVisibleItem != null)
                //        {
                //            if (firstVisibleItem.Y != vScrollValue)
                //                vScrollValue = firstVisibleItem.Y;
                //        }
                //    }
                //}

				// make sure we want to do something
				if (vScrollValue != _vScrollBar.Value)
				{
					_vScrollBar.Value = Math.Max(vScrollValue, 0);
					return;
				}
			}

			Invalidate(_detailVisibleRect);

			// only redraw the headers if we're scrolling horizontally
			if (sender is HScrollBar)
			{
				Invalidate(_headerVisibleRect);
				RecalculateLayout(false, false);
			}

		}

		internal void ColumnInvalidated(ContainerListViewColumnHeader column, bool recalculateLayout, bool redrawItems)
		{
			if (recalculateLayout)
				RecalculateLayout(true, false);

			if (redrawItems)
				Invalidate();
			else
				Invalidate(_headerVisibleRect);
		}

		#region Methods
		/// <summary>
		/// Halts drawing, sorting, filtering, and some internal functions
		/// </summary>
		public void BeginUpdate()
		{
			_updateTransactions.Push(this);
		}

		/// <summary>
		/// Resumes everything that was halted when BeginUpdate was called
		/// </summary>
		public void EndUpdate()
		{
			if (_updateTransactions.Count > 0)
				_updateTransactions.Pop();

			if (!InUpdateTransaction)
			{
				RecalculateLayout(true, true);

				Invalidate();
			}
		}

		/// <summary>
		/// Ensures that the currently focused item is visible, will scroll as necessary
		/// </summary>
		public void EnsureVisible()
		{
			EnsureVisible(_focusedItem);
		}

		/// <summary>
		/// Ensures that the item is visible, will scroll as necessary
		/// </summary>
		/// <param name="item">The item you want to ensure visibility of</param>
		public void EnsureVisible(ContainerListViewItem item)
		{
			if (item != null && _vScrollBar.Visible)
			{
				int top = item.Y;
				int bottom = top + item.Height;

				if (bottom >= _vScrollBar.Value + _vScrollBar.Height) // if it's too low, then scroll down
					_vScrollBar.Value += bottom - (_vScrollBar.Value + _vScrollBar.Height);
				else if (top < _vScrollBar.Value) // if it's too high, then scroll up
					_vScrollBar.Value = top;
			}
		}

		public bool IsVisible(ContainerListViewItem item)
		{
			if (item == null || _vScrollBar == null)
				return false;
			if (!_vScrollBar.Visible)
				return true;

			int top = item.Y;
			int bottom = top + item.Height;

			if (top >= _vScrollBar.Value + _vScrollBar.Height) // it's too low
				return false;
			else if (bottom < _vScrollBar.Value) // it's too high
				return false;
			else
				return true;
		}

		/// <summary>
		/// Refreshes the sort, uses the current sorting information
		/// </summary>
		/// <param name="recursive"><b>True</b> if you want to sort child items as well, <b>false</b> if you want to only sort the root items.</param>
		public void Sort(bool recursive)
		{
			Cursor = Cursors.WaitCursor;
			_items.Sort(new ContainerListViewComparer(this), recursive);
			RecalculateItemPositions(_rootItem, false);
			Cursor = Cursors.Default;
		}

		/// <summary>
		/// Adds a column to the sort
		/// </summary>
		/// <param name="columnIndex">The zero-based index of the column to add to the sort</param>
		/// <param name="autoSortOrder">If true will determine the sort order automatically, if false will use the columns default sort order (or ascending if that isn't set)</param>
		/// <param name="clearCurrentSort">Whether to clear the current sort before adding this one</param>
		public void Sort(int columnIndex, bool autoSortOrder, bool clearCurrentSort)
		{
			if (columnIndex < 0 || columnIndex >= _columns.Count)
				columnIndex = 0;

			ContainerListViewColumnHeader hdr = _columns[columnIndex];

			if (hdr.SortDataType == SortDataType.None)
				return;

			SortOrder newSortOrder = hdr.SortOrder;

			if (autoSortOrder || newSortOrder == SortOrder.None)
			{
				if (hdr.SortOrder == SortOrder.Ascending)
					newSortOrder = SortOrder.Descending;
				else if (hdr.SortOrder == SortOrder.Descending)
					newSortOrder = SortOrder.Ascending;
			}

			if (newSortOrder == SortOrder.None)
				newSortOrder = (hdr.DefaultSortOrder == SortOrder.None ? SortOrder.Ascending : hdr.DefaultSortOrder);

			Sort(columnIndex, newSortOrder, clearCurrentSort);
		}

		/// <summary>
		/// Adds a column to the sort
		/// </summary>
		/// <param name="columnIndex">The zero-based index of the column to add to the sort</param>
		/// <param name="sortOrder">The order to sort the column by</param>
		/// <param name="clearCurrentSort">Whether to clear the current sort before adding this one</param>
		public void Sort(int columnIndex, SortOrder sortOrder, bool clearCurrentSort)
		{
			if (columnIndex < 0 || columnIndex >= _columns.Count)
				columnIndex = 0;

			if (clearCurrentSort || !_allowMultipleColumnSort)
			{
				foreach (ContainerListViewColumnHeader column in _columnsSorted)
					column.InternalSortOrder = SortOrder.None;
				_columnsSorted.Clear();
			}

			ContainerListViewColumnHeader hdr = _columns[columnIndex];

			SetColumnSort(hdr, sortOrder);

			Sort(true);

			Invalidate();
		}

		/// <summary>
		/// Recursively sets a simple text (case insensitive) filter to all items
		/// </summary>
		/// <param name="columnIndex">The zero-based index of the column you want this text filter to filter on</param>
		/// <param name="filterText">The text you would like to filter on. <value>null</value> to reset the filter</param>
		public void SetFilter(int columnIndex, string filterText)
		{
			if (filterText == null || filterText.Length == 0)
				ResetFilter();
			else
				SetFilter(new ContainerListViewItemFilter(columnIndex, filterText));
		}

		/// <summary>
		/// Recursively sets a custom filter to all items
		/// </summary>
		/// <param name="filter">The custom <see cref="IFilter"/></param>
		public void SetFilter(IFilter filter)
		{
			BeginUpdate();

			_rootItem.SetFilter(filter, true);

			EndUpdate();
		}

		/// <summary>
		/// Recursively removes any custom applied filters to all items
		/// </summary>
		public void ResetFilter()
		{
			SetFilter(null);
		}

		/// <summary>
		/// Resizes all of the columns to fit their text
		/// </summary>
		/// <param name="includeItemWidths"><b>True</b> if you want it to include the texts of the sub-items below it</param>
		public void AutoSizeColumnWidths(bool includeItemWidths)
		{
			BeginUpdate();

			foreach (ContainerListViewColumnHeader column in _columns)
				column.AutoSizeWidth(includeItemWidths);

			EndUpdate();
		}

		public new void Refresh()
		{
			base.Refresh();
		}

		public void Refresh(ContainerListViewItem item)
		{
			if (item == null)
				return;

			if (_vScrollBar == null) return;
			Rectangle invalidateRect = new Rectangle(_detailVisibleRect.Left, _detailVisibleRect.Top + item.Y - _vScrollBar.Value, _detailRect.Width, item.Height);
			this.Invalidate(invalidateRect);
		}
		public void Refresh(ContainerListViewSubItem subItem)
		{
			if (subItem == null)
				return;

			Rectangle subRect = _headerColumnRects[subItem.ColumnIndex];
			Rectangle invalidateRect = new Rectangle(subRect.Left, _detailVisibleRect.Top + subItem.Item.Y - _vScrollBar.Value, subRect.Width, subItem.Item.Height);
			this.Invalidate(invalidateRect);
		}

		#endregion

		#region Overrides
		protected override void OnPaintBackground(PaintEventArgs pevent) { }

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			// draw the border of this control
			Rectangle clientRectangle = ClientRectangle;
			clientRectangle.Inflate(-_borderSize, -_borderSize);
			if (!clientRectangle.Contains(e.ClipRectangle))
			{
				// make sure the method only draws on the border
				g.Clip = new Region(ClientRectangle);
				g.ExcludeClip(clientRectangle);

				DrawBorder(g);
			}

			// draw the header and then exclude it from the client area then update the clip don't waste time

			clientRectangle = Rectangle.Intersect(_headerVisibleRect, e.ClipRectangle);
			if (clientRectangle.Width != 0 && clientRectangle.Height != 0)
				DrawHeaders(g, clientRectangle);

			// draw various background-y things
			clientRectangle = Rectangle.Intersect(_detailVisibleRect, e.ClipRectangle);
			if (clientRectangle.Width != 0 && clientRectangle.Height != 0)
			{
				// make sure the method only draws where it should
				g.Clip = new Region(clientRectangle);

				DrawBackground(g, clientRectangle);

				DrawItems(g, clientRectangle);

				DrawGridLines(g, clientRectangle);
			}

			if (_vScrollBar.Visible && _hScrollBar.Visible)
			{
				Rectangle extraRect = new Rectangle(_vScrollBar.Left, _hScrollBar.Top, _vScrollBar.Width, _hScrollBar.Height);
				extraRect.Intersect(e.ClipRectangle);

				if (extraRect.Width != 0 && extraRect.Height != 0)
					DrawExtra(g, extraRect);
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			RecalculateLayout(true, false);

			if (_vScrollBar.Visible)
				if (_vScrollBar.Value > _vScrollBar.Maximum - _vScrollBar.LargeChange)
					_vScrollBar.Value = _vScrollBar.Maximum - _vScrollBar.LargeChange;

			if (_hScrollBar.Visible)
				if (_hScrollBar.Value > _hScrollBar.Maximum - _hScrollBar.LargeChange)
					_hScrollBar.Value = _hScrollBar.Maximum - _hScrollBar.LargeChange;

			Invalidate(); // TODO: figure out what has been made visible and only invalidate that
		}

		[System.Security.Permissions.SecurityPermission(
			System.Security.Permissions.SecurityAction.LinkDemand,
			Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (m.Msg == Win32Constants.WM_GETDLGCODE)
			{
				// This line makes Arrow and Tab key events cause OnKeyXXX events to fire
				m.Result = new IntPtr(Win32Constants.DLGC_WANTCHARS | Win32Constants.DLGC_WANTARROWS | m.Result.ToInt32());
			}
		}

		protected override void OnBackgroundImageChanged(EventArgs e)
		{
			base.OnBackgroundImageChanged(e);

			if (BackgroundImage != null)
				_backgroundImageBrush = new TextureBrush(BackgroundImage);
			else
				_backgroundImageBrush = null;

			Invalidate(_detailVisibleRect);
		}

		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackgroundImageChanged(e);

			Invalidate(_detailVisibleRect);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			SetItemHovered(_hoveredItem, false);
			SetColumnHovered(_hoveredColumnHeader, false);
			SetColumnPressed(_pressedColumnHeader, false);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			// if the mouse button is currently pressed down on a header column,
			// moving will attempt to move the position of that column
			if (_pressedColumnHeader != null && _allowColumnReorder)
			{
				if (_reorderingColumn || Math.Abs(e.X - _lastClickedPoint.X) > 3)
				{
					_reorderingColumn = true;
					_reorderingColumnDelta = e.X - _lastClickedPoint.X;

					int displayIndex;
					for (displayIndex = 0; displayIndex < _columnDisplayOrder.Count; ++displayIndex)
					{
						_reorderingColumnDropIndex = ((ContainerListViewColumnHeader)_columnDisplayOrder[displayIndex]).Index;

						Rectangle columnRect = _headerColumnRects[_reorderingColumnDropIndex];
						int middle = columnRect.Left + (columnRect.Width / 2);

						if (e.X < middle)
							break;
					}

					if (displayIndex == _columnDisplayOrder.Count)
						_reorderingColumnDropIndex = _columnDisplayOrder.Count;

					Invalidate(_headerVisibleRect);
				}
			}
			else if (_sizingColumn)
			{
				Cursor = Cursors.VSplit;

				_sizingColumnDelta = e.X - _lastClickedPoint.X;
				_sizingColumnHeader.Width = Math.Max(_sizingColumnDelta + _sizingColumnOriginalWidth, 0);
			}
			else if (e.Button != MouseButtons.None // Button is clicked
				&& _selectedItems.Count > 0  // Is there items selected
				&& Math.Abs(Math.Sqrt(Math.Pow(e.X - _lastClickedPoint.X, 2) + Math.Pow(e.Y - _lastClickedPoint.Y, 2))) > 3  // Mesure distance
				&& MouseInRectangle(e, _detailRect)) // Are we over the items
			{
				OnItemDrag(new ItemDragEventArgs(e.Button, _selectedItems[0]));
			}
			else
			{
				// if we care about doing something in the header
				if (_headerStyle != ColumnHeaderStyle.None)
				{
					Cursor = Cursors.Default;

					// if we're in the header
					if (MouseInRectangle(e, _headerVisibleRect))
					{
						SetItemHovered(_hoveredItem, false);

						ContainerListViewColumnHeader newHoveredColumn = null;

						//TODO: tooltip? string toolTip = string.Empty;
						for (int index = 0; index < _columns.Count; ++index)
						{
							if (MouseInRectangle(e, _headerColumnSizeRects[index]))
							{
								Cursor = Cursors.VSplit;
								break;
							}
							else
							{
								if (_pressedColumnHeader == null)
								{
									if (MouseInRectangle(e, _headerColumnRects[index]))
									{
										newHoveredColumn = _columns[index];
										if (_toolTipControl.GetToolTip(this) != _columns[index].ToolTip)
											_toolTipControl.SetToolTip(this, _columns[index].ToolTip);
									}
								}
							}
						}

						if (newHoveredColumn == null)
							SetColumnHovered(_hoveredColumnHeader, false);
						else if (_hoveredColumnHeader != newHoveredColumn)
							SetColumnHovered(newHoveredColumn, true);

						return;
					}

					if (_toolTipControl.GetToolTip(this).Length != 0)
						_toolTipControl.SetToolTip(this, string.Empty);
				}

				SetColumnHovered(_hoveredColumnHeader, false);

				if (MouseInRectangle(e, _detailVisibleRect) && _pressedColumnHeader == null)
				{
					ContainerListViewItem item = GetItemAt(e.Y - _detailVisibleRect.Top + _vScrollBar.Value);

					if (item != null)
						SetItemHovered(item, true);
					else
						SetItemHovered(_hoveredItem, false);
				}
				else
					SetItemHovered(_hoveredItem, false);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (!Focused)
			{
				this.Focus();

				if (!_captureFocusClick)
					return;
			}

			_lastClickedPoint = new Point(e.X, e.Y);

			// determine if a header was pressed
			if (_headerStyle == ColumnHeaderStyle.Clickable)
			{
				if (MouseInRectangle(e, _headerVisibleRect) && e.Button == MouseButtons.Left)
				{
					for (int index = 0; index < _columns.Count; ++index)
					{
						if (MouseInRectangle(e, _headerColumnSizeRects[index]))
						{
							// autosize column
							if (e.Clicks == 2 && e.Button == MouseButtons.Left && _items.Count > 0)
								_columns[index].AutoSizeWidth(true);
							else // scale column
							{
								_sizingColumn = true;
								_sizingColumnOriginalWidth = _headerColumnRects[index].Width;
								_sizingColumnHeader = _columns[index];
							}
						}
						else if (MouseInRectangle(e, _headerColumnRects[index]))
							SetColumnPressed(_columns[index], true);
					}
					Invalidate(_headerVisibleRect);
					return;
				}
			}

			// determine if a item was pressed			
			if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
			{
				if (MouseInRectangle(e, _detailVisibleRect) && _items.Count > 0)
				{
					ContainerListViewItem item = GetItemAt(e.Y - _detailVisibleRect.Top + _vScrollBar.Value);
					if (item != null)
					{
						if (item.HasChildren && (MouseInRectangle(e, item.Glyph) || e.Clicks == 2))
							item.Expanded = !item.Expanded;
						else
							MouseSelection(item, e.Button);
					}
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			_lastClickedPoint = Point.Empty;

			if (_sizingColumn) // if we're actively sizing a column
			{
				_sizingColumn = false;

				_sizingColumnDelta = 0;
				_sizingColumnOriginalWidth = 0;
				_sizingColumnHeader = null;
			}

			if (_reorderingColumn) // if we're actively changing the orders of the columns
			{
				SetColumnDisplayIndex(_pressedColumnHeader, _reorderingColumnDropIndex);

				_reorderingColumn = false;
				_reorderingColumnDelta = 0;
				_reorderingColumnDropIndex = -1;
				SetColumnPressed(_pressedColumnHeader, false);

				this.OnColumnReorder(new ContainerListViewEventArgs(_pressedColumnHeader, null));
			}

			// if we just pressed a column
			if (_pressedColumnHeader != null && !_reorderingColumn)
			{
				int index = _pressedColumnHeader.Index;
				if (MouseInRectangle(e, _headerColumnRects[index]) && !MouseInRectangle(e, _headerColumnSizeRects[index]) && e.Button == MouseButtons.Left)
					OnColumnClick(new ContainerListViewEventArgs(_pressedColumnHeader, null, e));
			}
			SetColumnPressed(_pressedColumnHeader, false);

			// Check for context click
			if (e.Button == MouseButtons.Right)
			{
				if (MouseInRectangle(e, _headerVisibleRect))
					OnPopColumnContextMenu(new ContainerListViewEventArgs(_hoveredColumnHeader, null, e));
				else if (MouseInRectangle(e, _detailVisibleRect))
				{
					ContainerListViewItem item = GetItemAt(e.Y - _detailVisibleRect.Top + _vScrollBar.Value);

					if (item != null)
						OnPopItemContextMenu(new ContainerListViewEventArgs(_hoveredColumnHeader, item, e));
					else
						OnPopContextMenu(new ContainerListViewEventArgs(_hoveredColumnHeader, item, e));
				}
				else
					OnPopContextMenu(new ContainerListViewEventArgs(_hoveredColumnHeader, null, e));
			}

			if (_lastUserSingleClickedItem != null && ((ModifierKeys & Keys.Shift) == 0) && ((ModifierKeys & Keys.Control) == 0))
			{
				//single click mouse up, with no modifiers keys: select only _lastItemClicked
				ClearSelectedItems(false);
				SetItemSelected(_lastUserSingleClickedItem, true, true, true);
				_lastUserSingleClickedItem = null;
			}
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			if (this.PointToClient(MousePosition).Y > this._headerHeight)
				base.OnDoubleClick(e);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			try
			{
				if (e.Delta > 0)
				{
					if (_vScrollBar.Visible)
						//for some reasons it didn't work
						//										was _vScrollBar.SmallChange									was _vScrollBar.SmallChange
						_vScrollBar.Value = (_vScrollBar.Value - _defaultItemHeight * (e.Delta / 100) < 0 ? 0 : _vScrollBar.Value - _defaultItemHeight * (e.Delta / 100));
					else if (_hScrollBar.Visible)
						//for some reasons it didn't work
						//										was _vScrollBar.SmallChange									was _vScrollBar.SmallChange
						_hScrollBar.Value = (_hScrollBar.Value - _defaultItemHeight * (e.Delta / 100) < 0 ? 0 : _hScrollBar.Value - _defaultItemHeight * (e.Delta / 100));
				}
				else if (e.Delta < 0)
				{
					if (_vScrollBar.Visible)
						_vScrollBar.Value = (_vScrollBar.Value - _vScrollBar.SmallChange * (e.Delta / 100) > _vScrollBar.Maximum - _vScrollBar.LargeChange ? _vScrollBar.Maximum - _vScrollBar.LargeChange : _vScrollBar.Value - _vScrollBar.SmallChange * (e.Delta / 100));
					else if (_hScrollBar.Visible)
						_hScrollBar.Value = (_hScrollBar.Value - _hScrollBar.SmallChange * (e.Delta / 100) > _hScrollBar.Maximum - _hScrollBar.LargeChange ? _hScrollBar.Maximum - _hScrollBar.LargeChange : _hScrollBar.Value - _hScrollBar.SmallChange * (e.Delta / 100));
				}
			}
			catch (ArgumentException ex)
			{
				System.Diagnostics.Debug.Assert(false, "Problem setting scrollbar value:" + ex.Message);
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Add:
					{
						if ((ModifierKeys & Keys.Control) == Keys.Control && _allowColumnResize)
						{
							AutoSizeColumnWidths(true);
							e.Handled = true;
							return;
						}
						else
							OnSelectionKeys(e);
					}
					break;

				case Keys.Space:
				case Keys.Home:
				case Keys.End:
				case Keys.PageUp:
				case Keys.PageDown:
				case Keys.Up:
				case Keys.Down:
				case Keys.Right:
				case Keys.Left:
				case Keys.Subtract:
				case Keys.Multiply:
				case Keys.Divide:
				case Keys.A:
					OnSelectionKeys(e);
					break;
				default:
					base.OnKeyDown(e);
					break;
			}
		}

		protected virtual void OnSelectionKeys(KeyEventArgs e)
		{
			if (_items.Count == 0)
				return;

			if (_focusedItem == null)
				_focusedItem = _items[0];

			ContainerListViewItem item = _focusedItem;

			switch (e.KeyCode)
			{
				case Keys.Space:
					if (_allowMultipleSelect)
						SetItemSelected(_focusedItem, !_focusedItem.Selected, true, true);
					else
						SetItemSelected(_focusedItem, true, true, true);

					e.Handled = true;
					return;

				case Keys.Home:
					item = _rootItem.FirstItem;
					break;

				case Keys.End:
					item = _rootItem.VeryLastItem;
					break;

				case Keys.PageUp:
					ContainerListViewItem topItem = TopItem;

					if (topItem == item)
						item = GetItemAt(Math.Max(topItem.Y + topItem.Height - _vScrollBar.Height, 0));
					else
						item = topItem;

					break;

				case Keys.PageDown:
					if (_vScrollBar.Visible && _vScrollBar.Value + _vScrollBar.LargeChange < _vScrollBar.Maximum)
					{
						ContainerListViewItem bottomItem = BottomItemCompletelyVisible;

						if (bottomItem == item)
							item = GetItemAt(bottomItem.Y + _vScrollBar.Height) ?? _items[_items.Count - 1];
						else
							item = bottomItem;
					}
					else
						item = _rootItem.VeryLastItem;
					break;

				case Keys.Down:
					item = item.NextVisibleItem;
					break;

				case Keys.Up:
					item = item.PreviousVisibleItem;
					break;

				case Keys.Right:
					if (!item.Expanded && item.HasChildren)
						item.Expanded = true;
					else
						item = item.FirstItem;
					break;

				case Keys.Left:
					if (item.Expanded && item.HasChildren)
						item.Expanded = false;
					else
						item = item.ParentItem;
					break;

				case Keys.Add:
					item.Expanded = true;
					break;

				case Keys.Subtract:
					item.Expanded = false;
					break;

				case Keys.Multiply:
					item.Expanded = true; // TODO: recursively do this
					break;

				case Keys.Divide:
					item.Expanded = false; // TODO: recursively do this
					break;

				case Keys.A:
					if (((ModifierKeys & Keys.Control) != Keys.Control) || !_allowMultipleSelect)
						return;

					ClearSelectedItems(false);

					for (item = _rootItem.FirstItem; item != _rootItem.VeryLastItem; item = item.NextVisibleItem)
						SetItemSelected(item, true, false, false);

					SetItemSelected(_rootItem.LastItem, true, true, false);
					e.Handled = true;
					return;
			}

			KeyboardSelection(item);
			e.Handled = true;
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);

			Invalidate();
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);

			Invalidate();
		}
		#endregion

		#region Helper Functions

		#region Column and Item Properties Getters/Setters
		internal void SetColumnSort(ContainerListViewColumnHeader column, SortOrder sortOrder)
		{
			if (column == null)
				return;

			if (sortOrder == SortOrder.None && _columnsSorted.Contains(column))
				_columnsSorted.Remove(column);
			else
			{
				if (!_columnsSorted.Contains(column))
					_columnsSorted.Add(column);

				column.InternalSortOrder = sortOrder;
			}
		}

		internal bool GetColumnHovered(ContainerListViewColumnHeader column)
		{
			return (_hoveredColumnHeader != null && _hoveredColumnHeader.Equals(column));
		}

		internal void SetColumnHovered(ContainerListViewColumnHeader column, bool isHovered)
		{
			if (isHovered)
			{
				if (_hoveredColumnHeader == null || !_hoveredColumnHeader.Equals(column))
				{
					ContainerListViewColumnHeader old = _hoveredColumnHeader;
					_hoveredColumnHeader = column;

					if (_columnTracking)
					{
						InvalidateColumnHeader(true, old);
						InvalidateColumnHeader(true, column);
					}
				}
			}
			else if (_hoveredColumnHeader != null && _hoveredColumnHeader.Equals(column))
			{
				ContainerListViewColumnHeader old = _hoveredColumnHeader;
				_hoveredColumnHeader = null;

				if (_columnTracking)
					InvalidateColumnHeader(true, old);
			}
		}

		internal bool GetColumnPressed(ContainerListViewColumnHeader column)
		{
			return (_pressedColumnHeader != null && _pressedColumnHeader.Equals(column));
		}

		internal void SetColumnPressed(ContainerListViewColumnHeader column, bool isPressed)
		{
			if (isPressed)
			{
				if (_pressedColumnHeader == null || !_pressedColumnHeader.Equals(column))
				{
					ContainerListViewColumnHeader old = _pressedColumnHeader;
					_pressedColumnHeader = column;

					SetColumnHovered(_hoveredColumnHeader, false);
					SetItemHovered(_hoveredItem, false);

					InvalidateColumnHeader(false, old);
					InvalidateColumnHeader(false, column);
				}
			}
			else if (_pressedColumnHeader != null && _pressedColumnHeader.Equals(column))
			{
				ContainerListViewColumnHeader old = _pressedColumnHeader;
				_pressedColumnHeader = null;

				InvalidateColumnHeader(false, old);
			}
		}

		internal int GetColumnDisplayIndex(ContainerListViewColumnHeader column)
		{
			if (_columnDisplayOrder.Count != _columns.Count)
				RecalculateLayout(false, false);

			return (column == null ? -1 : _columnDisplayOrder.IndexOf(column));
		}

		internal void SetColumnDisplayIndex(ContainerListViewColumnHeader column, int dropBeforeIndex)
		{
			int curDisplayIndex = column.DisplayIndex;
			int newDisplayIndex = (dropBeforeIndex == _columns.Count ? _columns.Count : _columns[dropBeforeIndex].DisplayIndex);

			if (curDisplayIndex != newDisplayIndex && curDisplayIndex != newDisplayIndex - 1)
			{
				if (newDisplayIndex > curDisplayIndex) // moving right
				{
					_columnDisplayOrder.Insert(newDisplayIndex, column);
					_columnDisplayOrder.RemoveAt(curDisplayIndex);
					column.DisplayIndex = newDisplayIndex - 1;
				}
				else // moving left
				{
					_columnDisplayOrder.RemoveAt(curDisplayIndex);
					_columnDisplayOrder.Insert(newDisplayIndex, column);
					column.DisplayIndex = newDisplayIndex;
				}

				RecalculateLayout(false, false);
			}

			Invalidate();
		}

		internal int GetColumnIndex(ContainerListViewColumnHeader column)
		{
			return _columns.IndexOf(column);
		}

		internal bool GetItemFocused(ContainerListViewItem item)
		{
			return (_focusedItem != null && _focusedItem.Equals(item));
		}

		internal void SetItemFocused(ContainerListViewItem item, bool isFocused)
		{
			if (isFocused) // setting it to be focused
			{
				if (_focusedItem == null || !_focusedItem.Equals(item))
				{
					ContainerListViewItem old = _focusedItem;
					_focusedItem = item;

					InvalidateDetailItem(old);
					InvalidateDetailItem(item);
				}
			}
			else if (_focusedItem != null && _focusedItem.Equals(item))
			{
				ContainerListViewItem old = _focusedItem;
				_focusedItem = null;

				InvalidateDetailItem(old);
			}
		}

		internal bool GetItemHovered(ContainerListViewItem item)
		{
			return (_hoveredItem != null && _hoveredItem.Equals(item));
		}

		internal void SetItemHovered(ContainerListViewItem item, bool isHovered)
		{
			if (isHovered)
			{
				if (_hoveredItem == null || !_hoveredItem.Equals(item))
				{
					ContainerListViewItem old = _hoveredItem;
					_hoveredItem = item;

					InvalidateDetailItem(old);
					InvalidateDetailItem(item);
				}
			}
			else if (_hoveredItem != null && _hoveredItem.Equals(item))
			{
				ContainerListViewItem old = _hoveredItem;
				_hoveredItem = null;

				InvalidateDetailItem(old);
			}
		}

		internal bool GetItemSelected(ContainerListViewItem item)
		{
			return _selectedItems.Contains(item);
		}

		internal void ClearSelectedItems(bool sendEvent)
		{
			if (_selectedItems.Count > 0)
			{
				_selectedItems.InternalClear();

				if (sendEvent)
					OnSelectedItemsChanged();
			}
		}


		/// <summary>
		/// should be internal, don't use
		/// </summary>
		public void SetItemSelected(ContainerListViewItem item, bool isSelected, bool sendEvent, bool userFocused)
		{
			SetItemSelected(item, isSelected, sendEvent, userFocused, false);
		}

		/// <summary>
		/// should be internal, don't use
		/// </summary>
		public void SetItemSelected(ContainerListViewItem item, bool isSelected, bool sendEvent, bool userFocused, bool selectChildren)
		{
			if (item == null)
				return;

			if (userFocused && isSelected)
				_lastUserSelectedItem = item;

			if (_allowMultipleSelect)
			{
				if (isSelected)
				{
					if (!_selectedItems.Contains(item))
						_selectedItems.Add(item);
					else
						sendEvent = false; // didn't actually do anything

					if (selectChildren)
						foreach (ContainerListViewItem child in item.Items)
							SetItemSelected(child, isSelected, false, userFocused, true);
				}
				else
				{
					if (_selectedItems.Contains(item))
						_selectedItems.Remove(item);
					else
						sendEvent = false; // didn't actually do anything

					if (selectChildren)
						foreach (ContainerListViewItem child in item.Items)
							SetItemSelected(child, isSelected, false, userFocused, true);
				}
			}
			else
			{
				if (isSelected)
				{
					if (_selectedItems.Count == 1 && _selectedItems.Contains(item)) // not actually doing anything
						sendEvent = false;
					else
					{
						ClearSelectedItems(false);

						_selectedItems.Add(item);
					}
				}
				else
				{
					if (_selectedItems.Contains(item))
						ClearSelectedItems(false);
				}
			}

			Invalidate(_detailVisibleRect);

			if (sendEvent)
				OnSelectedItemsChanged();
		}
		#endregion

		private void PopMenu(ContextMenuStrip theMenu, ContainerListViewEventArgs e)
		{
			if (theMenu != null)
				theMenu.Show(this, e.MousePosition);
		}

		public ContainerListViewItem GetItemAt(int y)
		{
			return _rootItem.GetItemAt(y);
		}

		/// <summary>
		/// Calculates the virtual rectangle of the header rectangle and it's specific columns
		/// </summary>
		protected void RecalculateLayout(bool recalculateScrollbars, bool recalculateItems)
		{
			if (InUpdateTransaction || _hScrollBar == null)
				return;

			_headerColumnRects = new Rectangle[_columns.Count + 1];
			_headerColumnSizeRects = new Rectangle[_columns.Count + 1];

			// clean up the display index ArrayList, remove those columns that are no longer part of the control
			for (int index = 0; index < _columnDisplayOrder.Count; ++index)
			{
				ContainerListViewColumnHeader columnHeader = (ContainerListViewColumnHeader)_columnDisplayOrder[index];
				if (!_columns.Contains(columnHeader))
				{
					_columnDisplayOrder.Remove(columnHeader);
					--index; // we just messed up the indexing, this fixes it
				}
			}

			// now add in the display indexes for those 
			for (int index = 0; index < _columns.Count; ++index)
			{
				ContainerListViewColumnHeader columnHeader = _columns[index];

				if (!_columnDisplayOrder.Contains(columnHeader))
					_columnDisplayOrder.Add(_columns[index]);
			}

			int initialLeft = _borderSize - _hScrollBar.Value;
			int left = initialLeft;
			int headerWidth = 0;
			int top = _borderSize;

			for (int displayIndex = 0; displayIndex < _columnDisplayOrder.Count; ++displayIndex)
			{
				int index = ((ContainerListViewColumnHeader)_columnDisplayOrder[displayIndex]).Index;

				if (_columns[index].Visible)
				{
					_headerColumnRects[index] = new Rectangle(left, top, _columns[index].Width, _headerHeight);
					left += _columns[index].Width;
					headerWidth += _columns[index].Width;

					if (_allowColumnResize)
						_headerColumnSizeRects[index] = new Rectangle(left - 4, top, 4, _headerHeight);
					else
						_headerColumnSizeRects[index] = Rectangle.Empty;
				}
				else
				{
					_headerColumnRects[index] = Rectangle.Empty;
					_headerColumnSizeRects[index] = Rectangle.Empty;
				}
			}

			// add filler column
			int fillerColumnWidth = Math.Max((ClientRectangle.Right - _borderSize) - left, 0);
			_headerColumnRects[_headerColumnRects.Length - 1] = new Rectangle(left, top, fillerColumnWidth, _headerHeight);
			_headerColumnSizeRects[_headerColumnRects.Length - 1] = Rectangle.Empty;

			_headerVisibleRect = new Rectangle(_borderSize, _borderSize, ClientRectangle.Width - (_borderSize * 2), _headerHeight);

			if (recalculateItems)
				RecalculateItemPositions(_rootItem, false);

			_detailVisibleRect = Rectangle.FromLTRB(ClientRectangle.Left + _borderSize,
				ClientRectangle.Top + _borderSize + _headerHeight,
				ClientRectangle.Right - _borderSize - (_vScrollBar.Visible ? _vScrollBar.Width : 0),
				ClientRectangle.Bottom - _borderSize - (_hScrollBar.Visible ? _hScrollBar.Height : 0));

			if (_rootItem.VeryLastItem == null)
				_detailRect = new Rectangle(initialLeft, top + _headerHeight, headerWidth, 0);
			else
				_detailRect = new Rectangle(initialLeft, top + _headerHeight, headerWidth, _rootItem.VeryLastItem.Y + _rootItem.VeryLastItem.Height);

			if (recalculateScrollbars)
				UpdateScrollbars();
		}

		private void RecalculateItemPositions(ContainerListViewItem startAt, bool updateScrollBars)
		{
			if (InUpdateTransaction)
				return;

			// Aulofee customization - start. Added the following checks.
			if (startAt == null && _rootItem != null)
				startAt = _rootItem;

			if (startAt == null)
				return;
			// Aulofee customization - end

			ContainerListViewItem item = startAt;

			ContainerListViewItem parentItem = item.ParentItem;
			while (parentItem != null)
			{
				// the item we want to start at isn't even being drawn, start at the parent
				if (!parentItem.Expanded)
				{
					RecalculateItemPositions(parentItem, updateScrollBars);
					return;
				}

				parentItem = parentItem.ParentItem;
			}

			int startY = item.Y;

			while (item != null)
			{
				item.Y = startY;
				startY += item.Height;

				item = item.NextVisibleItem;
			}

			if (updateScrollBars)
				RecalculateLayout(true, false);

			Invalidate();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="startAt"><value>null</value> to recalculate all items</param>
		internal void RecalculateItemPositions(ContainerListViewItem startAt)
		{
			// Aulofee customization - start. Moved the next checks into the called method
			//			if (startAt == null && _rootItem != null)
			//				startAt = _rootItem;
			//
			//			if (startAt == null)
			//				return;
			// Aulofee customization - end.

			RecalculateItemPositions(startAt, true);
		}

		/// <summary>
		/// Updates the visibility and page sizes of the scroll bars
		/// </summary>
		protected virtual void UpdateScrollbars()
		{
			if (_detailRect.IsEmpty)
				return;

			bool hScrollBarVisWithVertical = (_detailRect.Width > (Width - (_borderSize * 2) - _vScrollBar.Width));
			bool hScrollBarVisWOutVertical = (_detailRect.Width > (Width - (_borderSize * 2)));
			bool vScrollBarVisWithHorizont = (_detailRect.Height > (Height - (_borderSize * 2) - _headerHeight - _hScrollBar.Height));
			bool vScrollBarVisWOutHorizont = (_detailRect.Height > (Height - (_borderSize * 2) - _headerHeight));

			bool hScrollVisible = false, vScrollVisible = false;
			if (hScrollBarVisWOutVertical || vScrollBarVisWOutHorizont)
			{
				hScrollVisible = hScrollBarVisWithVertical || hScrollBarVisWOutVertical;
				vScrollVisible = vScrollBarVisWithHorizont || vScrollBarVisWOutHorizont;
			}

			_hScrollBar.Left = _borderSize;
			_hScrollBar.Top = Height - _borderSize - _hScrollBar.Height;
			_hScrollBar.Width = Width - (_borderSize * 2) - (vScrollVisible ? _vScrollBar.Width : 0);
			_hScrollBar.Maximum = _detailRect.Width;
			_hScrollBar.LargeChange = Math.Max(_hScrollBar.Width, 0);

			_vScrollBar.Left = Width - _borderSize - _vScrollBar.Width;
			_vScrollBar.Top = _borderSize + _headerHeight;
			_vScrollBar.Height = Height - _headerHeight - (_borderSize * 2) - (hScrollVisible ? _hScrollBar.Height : 0);
			_vScrollBar.Maximum = _detailRect.Height;
			_vScrollBar.LargeChange = Math.Max(_vScrollBar.Height, 0);

			if (hScrollVisible != _hScrollBar.Visible)
			{
				_hScrollBar.Value = 0;
				_hScrollBar.Visible = hScrollVisible;
				RecalculateLayout(false, false);
			}

			if (vScrollVisible != _vScrollBar.Visible)
			{
				_vScrollBar.Value = 0;
				_vScrollBar.Visible = vScrollVisible;
				RecalculateLayout(false, false);
			}
		}


		private void InvalidateColumnHeader(bool invalidateEntireColumn, ContainerListViewColumnHeader column)
		{
			if (column == null)
				return;

			Rectangle columnRect = _headerColumnRects[column.Index];

			if (invalidateEntireColumn)
				columnRect.Height += _detailVisibleRect.Height;

			Invalidate(columnRect);
		}

		private void InvalidateDetailItem(ContainerListViewItem item)
		{
			if (item == null)
				return;

			Rectangle itemRect = new Rectangle(_detailVisibleRect.Left,
				_detailVisibleRect.Top + item.Y - _vScrollBar.Value,
				_detailVisibleRect.Width,
				item.Height + 1);

			Invalidate(itemRect);
		}

		public static bool MouseInRectangle(MouseEventArgs mouseArgs, Rectangle r)
		{
			return r.Contains(mouseArgs.X, mouseArgs.Y);
		}

		public static string TruncatedString(string s, int width, int height, int offset, Font font, Graphics g)
		{
			int i = s.Length;
			int swid;
			int allowed = width - offset;
			int numLines = height / font.Height;

			if (numLines == 0)
				return string.Empty;

			// check to see if string fits all by itself
			if ((int)g.MeasureString(s, font).Width <= allowed)
				return s;

			// string doesn't fit and we're allowed more than one line, find where we should wrap
			if (numLines > 1)
			{
				int index = 0;
				for (; index < s.Length; ++index)
				{
					if ((int)g.MeasureString(s.Substring(0, index + 1), font).Width > allowed)
						break; // doesn't fit, need to wrap before this point
				}

				int breakIndex = s.LastIndexOfAny(new char[] { ' ', '-' }, index);
				if (breakIndex == -1)
					breakIndex = index;

				string nextLine = TruncatedString(s.Substring(breakIndex), width, height - font.Height, offset, font, g);
				if (nextLine.Length == 0)
					return TruncatedString(s, width, height - font.Height, offset, font, g);
				else
					return s.Substring(0, breakIndex) + "\n" + nextLine;
			}
			else // just one line left, trim with ellipses
			{
				// string doesn't fit, drop last three characters
				i -= 3;

				while (true)
				{
					if (i <= 1)
						return s.Substring(0, 1) + "...";

					string toTest = s.Substring(0, i) + "...";

					swid = (int)g.MeasureString(toTest, font).Width;

					if (swid <= allowed) // it fits!
						return toTest;

					--i;
				}
			}
		}

		private void KeyboardSelection(ContainerListViewItem item)
		{
			if (item == null || item == _rootItem)
				return;

			if (_allowMultipleSelect)
			{
				if ((ModifierKeys & Keys.Shift) == Keys.Shift) // we have the shift key, select a range if possible
				{
					ContainerListViewItem startRange = _lastUserSelectedItem;
					if (startRange == null)
						startRange = _focusedItem;

					if (startRange != null)
					{
						ClearSelectedItems(false);

						selectRange(startRange, item);
					}
					else
						SetItemSelected(item, true, true, true);

					SetItemFocused(item, true);
				}
				else if ((ModifierKeys & Keys.Control) == Keys.Control) // we have the control key, set focus
				{
					SetItemFocused(item, true);
				}
				else
				{
					ClearSelectedItems(false);

					SetItemFocused(item, true);
					SetItemSelected(item, true, true, true);
				}
			}
			else
			{
				SetItemFocused(item, true);
				SetItemSelected(item, true, true, true);
			}

			EnsureVisible();
		}

		private void MouseSelection(ContainerListViewItem item, MouseButtons mouseButton)
		{
			if (!_allowMultipleSelect)
			{
				KeyboardSelection(item);
				return;
			}

			if (item == null || item == _rootItem)
				return;

			if (((ModifierKeys & Keys.Shift) == Keys.Shift) && ((ModifierKeys & Keys.Control) == Keys.Control)) // we have both keys, add a range
			{
				ContainerListViewItem startRange = _lastUserSelectedItem;
				if (startRange == null)
					startRange = _focusedItem;

				if (startRange != null)
				{
					// System.Diagnostics.Debug.Assert(startRange.Selected, "startRange not Selected");
					selectRange(startRange, item);
				}
				else
					SetItemSelected(item, true, true, true);
			}
			else if ((ModifierKeys & Keys.Shift) == Keys.Shift) // we have the shift key, select a range if possible
			{
				ContainerListViewItem startRange = _lastUserSelectedItem;
				if (startRange == null)
					startRange = _focusedItem;

				if (startRange != null)
				{
					ClearSelectedItems(false);

					selectRange(startRange, item);
				}
				else
					SetItemSelected(item, true, true, true);
			}
			else if ((ModifierKeys & Keys.Control) == Keys.Control) // we have the control key, swap select value
			{
				SetItemSelected(item, !item.Selected, true, true);
			}
			else // just a click, just select it exclusively
			{
				if (!item.Selected)
				{
					//do not clear selection when the user selects a selected item: maybe he wants to drag it 
					// (selection will be cleared in MouseUp event)
					ClearSelectedItems(false);
				}
				else
					_lastUserSingleClickedItem = item;
				SetItemSelected(item, true, true, true);
			}

			SetItemFocused(item, true);

			EnsureVisible();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="itemFrom">initial item</param>
		/// <param name="itemTo">item selected by the user</param>
		private void selectRange(ContainerListViewItem itemFrom, ContainerListViewItem itemTo)
		{
			//check for null arguments
			if (itemFrom == null || itemTo == null)
			{
				//true only the non null item
				if (itemFrom != null)
					SetItemSelected(itemFrom, true, false, false, true);
				else if (itemTo != null)
					SetItemSelected(itemTo, true, false, false, true);
				return;
			}

			if (itemFrom.Depth == itemTo.Depth && itemFrom.ParentItem == itemTo.ParentItem) //same depth
			{
				ContainerListViewItemCollection items = itemFrom.ParentItem.Items;
				//get the indexes
				int indexFrom = items.IndexOf(itemFrom);
				int indexTo = items.IndexOf(itemTo);

				if (indexFrom > indexTo)
				{
					//swap the items
					Helpers.Swap(ref itemFrom, ref itemTo);
					Helpers.Swap(ref indexFrom, ref indexTo);
				}
				//now indexFrom < indexTo

				if (indexFrom == -1)
				{
					//itemFrom does not belong to items collection
					// this can happen when _lastUserSelectedItem is not up-to-date
					indexFrom = indexTo;
				}

				for (int index = indexFrom; index < indexTo; ++index)
					SetItemSelected(items[index], true, false, false, true);

				SetItemSelected(itemTo, true, true, false, false); //do not true itemTo's children
			}
			else
			{
				if (itemFrom.Depth < itemTo.Depth)
				{
					//swap itemFrom with itemTo
					Helpers.Swap(ref itemFrom, ref itemTo);
				}
				//now itemTo.Depth < itemFrom.Depth

				//discover if the selected itemTo is before or after itemFrom
				ContainerListViewItem parent = itemFrom;
				for (int i = itemFrom.Depth; i > itemTo.Depth; i--)
					parent = parent.ParentItem;
				//TODO: bug   //now parent.ParentItem == itemTo.ParentItem
				//   System.Diagnostics.Debug.Assert(parent.ParentItem == itemTo.ParentItem);

				ContainerListViewItemCollection items = parent.ParentItem.Items;

				if (items.IndexOf(itemTo) <= items.IndexOf(parent))
				{ //going up
					//true all the items on the highest dept
					selectRange(itemFrom.ParentItem.Items[0], itemFrom);

					//true all the items on lower depts
					selectRange(itemTo, itemFrom.ParentItem);
				}
				else
				{ //going down
					//true all the items on the highest dept
					selectRange(itemFrom.ParentItem.Items[itemFrom.ParentItem.Items.Count - 1], itemFrom);
					SetItemSelected(itemFrom.ParentItem.Items[itemFrom.ParentItem.Items.Count - 1], true, true, true, true);

					//true all the items on lower depts
					ContainerListViewItem nextStart = itemFrom;
					do
					{
						nextStart = nextStart.ParentItem;
					} while (nextStart.NextItem == null);
					selectRange(itemTo, nextStart.NextItem);
				}
			}
		}

		#region Drawing Methods
		/// <summary>
		/// Called when the border of the control needs to be painted
		/// </summary>
		/// <param name="g">The graphics object to draw the border on</param>
		protected virtual void DrawBorder(Graphics g)
		{
            if (UseVisualStyle)
                drawBorderWithStyles(g);
            else
                drawBorderWithoutStyles(g);
		}

        /// <summary>
        /// Tries to draw the border with Visual Styles but falls back to normal drawing in case of error.
        /// </summary>
        /// <param name="g">The graphics object to draw the border on</param>
        private void drawBorderWithStyles(Graphics g)
        {
            VisualStyleElement vse = null;
            if (VisualStyleRenderer.IsElementDefined(VisualStyleElement.ListView.Detail.Normal))
                vse = VisualStyleElement.ListView.Detail.Normal; //this style does not seem always present
            else if (VisualStyleRenderer.IsElementDefined(VisualStyleElement.TextBox.TextEdit.Normal))
                vse = VisualStyleElement.TextBox.TextEdit.Normal;

            if (vse != null)
            {
                VisualStyleRenderer renderer = new VisualStyleRenderer(vse);
                renderer.DrawBackground(g, ClientRectangle);
            }
            else
            {
                //no suitable VisualStyleElement was found: draw normally
                drawBorderWithoutStyles(g);
            }
        }

        /// <param name="g">The graphics object to draw the border on</param>
        private void drawBorderWithoutStyles(Graphics g)
        {
            //draw without visual styles
            if (_borderStyle == BorderStyle.FixedSingle)
                g.DrawRectangle(SystemPens.ControlDarkDark, ClientRectangle);
            else if (_borderStyle == BorderStyle.Fixed3D)
                ControlPaint.DrawBorder3D(g, ClientRectangle, Border3DStyle.Sunken);
        }

		/// <summary>
		/// Called when the background of the detail area needs to be painted
		/// </summary>
		/// <param name="g">The graphics object to draw the background on</param>
		/// <param name="rect">The rectangle representing the portion of the control that needs to be painted</param>
		protected virtual void DrawBackground(Graphics g, Rectangle rectangle)
		{
			Brush backBrush, columnTrackingBrush, columnSortBrush;

			// throw a background on the control
			if (BackgroundImage != null)
			{
				_backgroundImageBrush.ResetTransform();
				System.Drawing.Drawing2D.Matrix mx = _backgroundImageBrush.Transform;
				mx.Translate(-_hScrollBar.Value, -_vScrollBar.Value);
				_backgroundImageBrush.Transform = mx;
				backBrush = _backgroundImageBrush;

				Color curColor = _columnTrackingBrush.Color;
				columnTrackingBrush = new SolidBrush(Color.FromArgb(69, (int)Math.Min(255, curColor.R * 0.75f), (int)Math.Min(255, curColor.G * 0.75f), (int)Math.Min(255, curColor.B * 0.75f)));

				curColor = _columnSortBrush.Color;
				columnSortBrush = new SolidBrush(Color.FromArgb(69, (int)Math.Min(255, curColor.R * 0.75f), (int)Math.Min(255, curColor.G * 0.75f), (int)Math.Min(255, curColor.B * 0.75f)));
			}
			else
			{
				backBrush = new SolidBrush(BackColor);
				columnTrackingBrush = _columnTrackingBrush;
				columnSortBrush = _columnSortBrush;
			}

			g.FillRectangle(backBrush, rectangle);

			try
			{
				for (int index = 0; index < _columns.Count; ++index)
				{
					ContainerListViewColumnHeader column = _columns[index];

					if (column.Visible)
					{
						Brush br = null;

						if (_columnsSorted.Contains(column))
							br = columnSortBrush;
						else if (_columnTracking && _hoveredColumnHeader == column)
							br = columnTrackingBrush;

						if (br != null)
						{
							Rectangle columnRect = _headerColumnRects[column.Index];

							g.FillRectangle(br,
								columnRect.Left,
								columnRect.Bottom,
								columnRect.Width,
								ClientRectangle.Height);
						}
					}
				}
			}
			catch (IndexOutOfRangeException)
			{
				//I believe it might happen while removing a column and repainting the background at the same time.
				//Just ignore it
			}
		}

		/// <summary>
		/// Called when grid lines need to be painted
		/// </summary>
		/// <param name="g">The graphics object to draw the grid lines on</param>
		/// <param name="rect">The rectangle representing the portion of the control that needs to be painted</param>
		private void DrawGridLines(Graphics g, Rectangle rect)
		{
			if (_gridLines == GridLines.None)
				return;

			if ((_gridLines & GridLines.Vertical) == GridLines.Vertical)
			{
				for (int index = 0; index < _columns.Count; ++index)
				{
					if (_columns[index].Visible)
					{
						// don't draw if it's outside of our clip
						if (_headerColumnRects[index].Right < rect.Left || _headerColumnRects[index].Right > rect.Right)
							continue;

						g.DrawLine(_gridLinePen, new Point(_headerColumnRects[index].Right, rect.Top), new Point(_headerColumnRects[index].Right, rect.Bottom));
					}
				}
			}

			if ((_gridLines & GridLines.Horizontal) == GridLines.Horizontal)
			{
				ContainerListViewItem bottomItemPartiallyVisible = BottomItemPartiallyVisible;
				for (ContainerListViewItem item = TopItem; item != bottomItemPartiallyVisible; item = item.NextVisibleItem)
					g.DrawLine(_gridLinePen, new Point(rect.Left, _detailVisibleRect.Top + item.Y + item.Height - _vScrollBar.Value), new Point(rect.Right, _detailVisibleRect.Top + item.Y + item.Height - _vScrollBar.Value));
			}
		}

		/// <summary>
		/// Draws the header control
		/// </summary>
		/// <param name="g">The graphics object to draw the header control on</param>
		/// <param name="actRect">The actual position of the header control</param>
		/// <param name="clip">The portion of the header control to redraw</param>
		protected virtual void DrawHeaders(Graphics g, Rectangle clip)
		{
			for (int index = 0; index < _headerColumnRects.Length; ++index)
			{
				Rectangle columnRect = _headerColumnRects[index];
				Rectangle columnClipRect = Rectangle.Intersect(columnRect, clip);

				// don't waste time on this one
				if (columnClipRect.Height == 0 || columnClipRect.Width == 0)
					continue;

				DrawColumnHeader(g, index, columnClipRect, _headerColumnRects[index]);
			}

			g.ResetClip();

			// if we're currently reordering the columns, we need to draw the "moving" column
			if (_reorderingColumn)
			{
				int dropMark = _headerColumnRects[_reorderingColumnDropIndex].Left - 1;
				g.FillRectangle(SystemBrushes.HotTrack, dropMark, _headerVisibleRect.Top, 2, _headerVisibleRect.Height);

				int index = _pressedColumnHeader.Index;
				Rectangle columnRect = _headerColumnRects[index];
				columnRect.Offset(_reorderingColumnDelta, 0);

				Bitmap tmp = new Bitmap(_headerVisibleRect.Right, _headerVisibleRect.Bottom);
				Graphics gfx = Graphics.FromImage(tmp);

				ImageAttributes imageAttributes = new ImageAttributes();

				float[][] colorMatrixElements = { 
															  new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
															  new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
															  new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
															  new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},
															  new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
														  };

				ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
				imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

				DrawColumnHeader(gfx, index, _headerVisibleRect, columnRect);

				g.DrawImage(tmp, _headerVisibleRect, _headerVisibleRect.Left, _headerVisibleRect.Top, _headerVisibleRect.Width, _headerVisibleRect.Height, GraphicsUnit.Pixel, imageAttributes);
			}
		}

		protected virtual void DrawColumnHeader(Graphics g, int columnIndex, Rectangle clip, Rectangle headerBounds)
		{
			g.Clip = new Region(clip);

			ContainerListViewColumnHeader column = (columnIndex == _columns.Count ? null : _columns[columnIndex]);

			VisualStyleElement vse = null;
			if (UseVisualStyle)
			{
				// Aulofee customization - start. Remove ref to C++ lib and directly access .NET theming classes.
				if (columnIndex == _columns.Count)
				{
					if (_headerStyle == ColumnHeaderStyle.Clickable && column != null && column.Pressed)
						vse = VisualStyleElement.Header.ItemRight.Pressed;
					else if (_headerStyle != ColumnHeaderStyle.None && column != null && column.Hovered)
						vse = VisualStyleElement.Header.ItemRight.Hot;
					else
						//vse = VisualStyleElement.Header.ItemRight.Normal;	// Should logically be ItemRight but it's not defined. So we use Item.
						vse = VisualStyleElement.Header.Item.Normal;
				}
				else
				{
					if (_headerStyle == ColumnHeaderStyle.Clickable && column != null && column.Pressed)
						vse = VisualStyleElement.Header.Item.Pressed;
					else if (_headerStyle != ColumnHeaderStyle.None && column != null && column.Hovered)
						vse = VisualStyleElement.Header.Item.Hot;
					else
						vse = VisualStyleElement.Header.Item.Normal;
				}

				if (!VisualStyleRenderer.IsElementDefined(vse))
					vse = null;
			}

			if (vse != null)
			{
				VisualStyleRenderer renderer = new VisualStyleRenderer(vse);
				renderer.DrawBackground(g, headerBounds, clip);
			}
			// Aulofee customization - end
			else
			{
				g.FillRectangle(new SolidBrush(SystemColors.Control), headerBounds);

				ButtonState state;
				if (_headerStyle == ColumnHeaderStyle.Clickable && column != null && column.Pressed)
					state = ButtonState.Pushed;
				else
					state = ButtonState.Normal;

				if (headerBounds.Width < 0)
					return;

				ControlPaint.DrawButton(g, headerBounds, state);
			}

			if (column == null)
				return;

			// offset all drawing from the left and exclude the right two pixels (for the separator)
			headerBounds.Offset(6, 0);
			headerBounds.Width -= 8;
			headerBounds.Height -= 4;
			clip.Intersect(headerBounds);

			if (headerBounds.Width <= 0 || headerBounds.Height <= 0)
				return;

			int sortTriangleWidth = (column.SortOrder == SortOrder.None ? 0 : 16);
			int imageWidth = (column.Image == null ? 0 : 20);

			string sp = TruncatedString(column.Text,
				headerBounds.Width,
				headerBounds.Height,
				sortTriangleWidth + imageWidth,
				column.Font,
				g);
			StringFormat sf = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);
			sf.LineAlignment = StringAlignment.Center;
			sf.Trimming = StringTrimming.None;

			int textWidth = (int)g.MeasureString(sp, column.Font, headerBounds.Width, sf).Width;

			int startLeft;
			if (column.ContentAlign == System.Drawing.ContentAlignment.MiddleRight || column.ContentAlign == System.Drawing.ContentAlignment.TopRight || column.ContentAlign == System.Drawing.ContentAlignment.BottomRight)
			{
				startLeft = headerBounds.Width - (sortTriangleWidth + imageWidth + textWidth) - 2;
				sf.Alignment = StringAlignment.Far;
			}
			else if (column.ContentAlign == System.Drawing.ContentAlignment.MiddleCenter || column.ContentAlign == System.Drawing.ContentAlignment.TopCenter || column.ContentAlign == System.Drawing.ContentAlignment.BottomCenter)
			{
				startLeft = (headerBounds.Width - (sortTriangleWidth + imageWidth + textWidth)) / 2;
				sf.Alignment = StringAlignment.Center;
			}
			else
			{
				startLeft = 0;
				sf.Alignment = StringAlignment.Near;
			}

			if (startLeft < 0 || sp != column.Text)
				startLeft = 0;

			startLeft += headerBounds.Left;

			if (column.SortOrder != SortOrder.None)
			{
				int topLeftX = (startLeft + textWidth + imageWidth + sortTriangleWidth) - 11;

				if (sp != column.Text || topLeftX > headerBounds.Right - 11)
					topLeftX = headerBounds.Right - 13;

				if (topLeftX < 0)
					topLeftX = 0;

				DrawSortTriangle(column, g, new Rectangle(topLeftX, headerBounds.Top + 1, 13, headerBounds.Height));

				g.ExcludeClip(new Rectangle(headerBounds.Width - sortTriangleWidth, 0, sortTriangleWidth, headerBounds.Height));
			}
			else
				g.ExcludeClip(new Rectangle(headerBounds.Width - 3, 0, 3, headerBounds.Height));

			if (column.Image != null)
			{
				int top = (headerBounds.Height - Math.Min(column.Image.Height, 16)) / 2;
				top = Math.Max(top, 1);

				g.DrawImage(column.Image, new Rectangle(startLeft, top, 16, 16));
			}

			Rectangle textRect = new Rectangle(startLeft + imageWidth, headerBounds.Top, textWidth, headerBounds.Height);
			g.DrawString(sp, column.Font, SystemBrushes.ControlText, textRect, sf);
		}

		protected virtual void DrawSortTriangle(ContainerListViewColumnHeader col, Graphics g, Rectangle rectangle)
		{
			int height = (_columnsSorted.Count > 1 ? 3 : 4);

			Bitmap bm = new Bitmap(9, height + 1);
			Graphics gfx = Graphics.FromImage(bm);

			Pen lightPen = SystemPens.ControlLightLight;
			Pen shadowPen = SystemPens.ControlDark;

			bm.SetPixel(4, 0, shadowPen.Color);
			for (int index = 1; index <= height; ++index)
				gfx.DrawLine(shadowPen, 4 - index, index, 4 + index, index);

			gfx.Dispose();

			if (_columnsSorted.Count > 1)
			{
				Font newFont = new Font(col.Font.FontFamily, 5, col.Font.Style, col.Font.Unit, col.Font.GdiCharSet, col.Font.GdiVerticalFont);
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Center;

				g.DrawString((_columnsSorted.IndexOf(col) + 1).ToString(CultureInfo.CurrentCulture), newFont, SystemBrushes.ControlText, rectangle.Left + 4, rectangle.Top, sf);
			}

			if (col.SortOrder == SortOrder.Descending)
				bm.RotateFlip(RotateFlipType.RotateNoneFlipY);

			if (_columnsSorted.Count > 1)
				g.DrawImage(bm, rectangle.Left, rectangle.Top + 3 + ((rectangle.Height - 5) / 2));
			else
				g.DrawImage(bm, rectangle.Left, rectangle.Top + ((rectangle.Height - 5) / 2));
		}

		protected virtual void DrawItems(Graphics g, Rectangle clip)
		{
			// Don't paint if in transaction
			if (InUpdateTransaction)
				return;

			ArrayList newVisibleItems = new ArrayList();

			for (int index = 0; index < _visibleItems.Count; ++index)
			{
				ContainerListViewItem item = _visibleItems[index] as ContainerListViewItem;

				if (item.Y >= _vScrollBar.Value && item.Y <= _vScrollBar.Value + _detailVisibleRect.Height && item.InternalIsVisible)
				{
					newVisibleItems.Add(item);
					continue;
				}

				// not visible, hide it
				for (int iidx = 0; iidx < item.SubItems.Count; ++iidx)
					if (item.SubItems[iidx].ItemControl != null)
						item.SubItems[iidx].ItemControl.Visible = false;
			}
			_visibleItems = newVisibleItems;

			int yTopPos = clip.Top + _vScrollBar.Value - _detailVisibleRect.Top;
			int yBotPos = clip.Bottom + _vScrollBar.Value - _detailVisibleRect.Top;

			ContainerListViewItem endItem = GetItemAt(yBotPos);

			if (endItem == null)
				endItem = _rootItem.VeryLastItem;

			if (endItem == null)
				return;

			endItem = endItem.NextVisibleItem;
			for (ContainerListViewItem item = GetItemAt(yTopPos); item != endItem; item = item.NextVisibleItem)
			{
				if (item == null)
				{
					break;
				}
				Rectangle itemRect = new Rectangle(_detailRect.Left, _detailVisibleRect.Top + item.Y - _vScrollBar.Value, _detailRect.Width, item.Height);

				g.Clip = new Region(_detailVisibleRect);

				if (!_visibleItems.Contains(item))
					_visibleItems.Add(item);

				DrawItem(g, item, clip, itemRect);
			}

			g.ResetClip();
		}

		private void DrawItem(Graphics g, ContainerListViewItem item, Rectangle clip, Rectangle itemBounds)
		{
			int itemSelWidth = _detailRect.Width;
			if (!_fullItemSelect)
				itemSelWidth = _columns[0].Width;

			int indent = 16 * (item.Depth - 1);
			int extraIndent = (_showRootTreeLines && (_showPlusMinus || _showTreeLines) ? 16 : 0) + 3;

			itemSelWidth -= (indent + extraIndent);

			Rectangle selectedRect = new Rectangle(itemBounds.Left + indent + extraIndent, itemBounds.Top, itemSelWidth, itemBounds.Height);

			// hot-tracked item
			if (_itemTracking && item.Hovered)
				g.FillRectangle(_itemTrackingBrush, clip.Left, _detailVisibleRect.Top + item.Y - _vScrollBar.Value, clip.Right, item.Height);
			else if (item.BackColor != Color.Empty)
				g.FillRectangle(new SolidBrush(item.BackColor), clip.Left, _detailVisibleRect.Top + item.Y - _vScrollBar.Value, clip.Right, item.Height);

			Color backcolor = item.BackColor;
			if (backcolor == Color.Empty)
				backcolor = this.BackColor;

			//only initialize foreground: it will be set specifically later
			Color foreground = this.ForeColor;

			// render selected item highlights
			if (item.Selected)
			{
				ContainerListViewItem nextItem = item.NextVisibleItem;
				ContainerListViewItem prevItem = item.PreviousVisibleItem;

				bool isNextSelected = (nextItem == null ? false : nextItem.Selected);
				bool isPrevSelected = (prevItem == null ? false : prevItem.Selected);

				Color drawColor;
				if (ContainsFocus)
					drawColor = _itemSelectedColor;
				else
					drawColor = Color.FromArgb(120, _itemSelectedColor);

				DrawSelectionBox(g, selectedRect, drawColor, !isPrevSelected, !isNextSelected);
			}

			if (item.Focused && _allowMultipleSelect && ContainsFocus)
				ControlPaint.DrawFocusRectangle(g, selectedRect, foreground, backcolor);

			Font fnt = item.Font;
			if (fnt == null)
				fnt = Font;

			//draw subitem
			for (int colIndex = 0; colIndex < _columns.Count; ++colIndex)
			{
				DrawSubitem(g, item, itemBounds, indent, fnt, colIndex);
			}
		}

		private void DrawSubitem(Graphics g, ContainerListViewItem item, Rectangle itemBounds, int indent, Font fnt, int colIndex)
		{
			ContainerListViewColumnHeader column = _columns[colIndex];
			Rectangle columnRect = _headerColumnRects[colIndex];

			columnRect.Offset(5, 0);
			columnRect.Width -= 5;

			if (column.Visible && columnRect.Right > _detailVisibleRect.Left && columnRect.Left < _detailVisibleRect.Right)
			{
				string drawText = null;
				int imageWidth = 0;

				g.Clip = new Region(new Rectangle(columnRect.Left, itemBounds.Top, columnRect.Width, itemBounds.Height));
				g.IntersectClip(_detailVisibleRect);

				if (column.DisplayIndex == 0) // first visible column, draw tree stuff if appropriate
				{
					if ((item.Depth == 1 && _showRootTreeLines && (_showPlusMinus || _showTreeLines)) || (item.Depth != 1)) // drawing root glyph or we're a child item
					{
						indent = 0;

						if (item.Depth != 1) // drawing lines or plus/minus for root entry and this isn't a root item
						{
							for (int d = (_showRootTreeLines && (_showPlusMinus || _showTreeLines) ? 1 : 2); d < item.Depth; ++d)
							{
								ContainerListViewItem parent = item;
								for (int p = 0; p < item.Depth - d; ++p)
									parent = parent.ParentItem;

								if (_showTreeLines && parent.NextItem != null)
								{
									Point top = new Point(columnRect.Left + 4, itemBounds.Top);
									Point bottom = new Point(columnRect.Left + 4, itemBounds.Bottom);

									if (top.Y % 2 != 0)
										++top.Y;
									if (bottom.Y % 2 != 0)
										++bottom.Y;

									if (item.PreviousVisibleItem != null)
										top.Y = itemBounds.Top + (itemBounds.Top % 2 == 0 ? 0 : 1);

									if (item.NextVisibleItem != null)
										bottom.Y = itemBounds.Bottom + (itemBounds.Bottom % 2 == 0 ? 0 : 1);

									Pen p = new Pen(SystemColors.GrayText, 1);
									p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

									g.DrawLine(p, top, bottom);
								}

								columnRect.Offset(16, 0);
								columnRect.Width -= 16;
							}
						}

						columnRect.Offset(indent, 0);
						columnRect.Width -= indent;

						if (_showTreeLines)
						{
							Point center = new Point(columnRect.Left + 4, itemBounds.Top + (itemBounds.Height / 2));
							if (center.Y % 2 != 0 && !_showPlusMinus)
								--center.Y;

							Point right = new Point(columnRect.Left + 16, center.Y);
							Point top = center;
							Point bottom = top;

							if (item.PreviousVisibleItem != null)
								top.Y = itemBounds.Top + (itemBounds.Top % 2 == 0 ? 0 : 1);

							if (item.NextItem != null)
								bottom.Y = itemBounds.Bottom + (itemBounds.Bottom % 2 == 0 ? 0 : 1);

							Pen p = new Pen(SystemColors.GrayText, 1);
							p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

							g.DrawLine(p, top, bottom);
							g.DrawLine(p, center, right);
						}

						if (item.HasChildren && _showPlusMinus)
						{
							Rectangle rect = new Rectangle(columnRect.Left, itemBounds.Top + ((itemBounds.Height - 9) / 2), 9, 9);
							DrawTreeExpander(g, rect, item.Expanded);
							item.Glyph = rect;
						}
						else
							item.Glyph = Rectangle.Empty;

						columnRect.Offset(16, 0);
						columnRect.Width -= 16;
					}

					// render the image
					if (_smallImageList != null || _selectedImageList != null)
					{
						ImageList imageLst = _smallImageList;
						int imageIndex = item.ImageIndex;
						if (imageLst == null || (item.Selected && _selectedImageList != null))
							imageLst = _selectedImageList;
						if (imageIndex == -1 || (item.Selected && item.SelectedImageIndex != -1))
							imageIndex = item.SelectedImageIndex;

						if (imageIndex != -1)
						{
							g.DrawImage(imageLst.Images[imageIndex], columnRect.Left + 2, itemBounds.Top + ((itemBounds.Height - 16) / 2), 16, 16);
							imageWidth = 20;
						}
					}
				}

				ContainerListViewSubItem subItem = item.SubItems[colIndex];

				Control c = subItem.ItemControl;
				if (c != null)
				{
					Size initialSize = subItem.ItemControlInitialSize;
					int cLeft, cTop, cWidth, cHeight;

					cWidth = initialSize.Width;
					cHeight = initialSize.Height;

					if (subItem.ControlResizeBehavior == ControlResizeBehavior.HeightFitMaintainRatio ||
							subItem.ControlResizeBehavior == ControlResizeBehavior.HeightFit ||
							subItem.ControlResizeBehavior == ControlResizeBehavior.BothFit)
						cHeight = item.Height - c.Margin.Top - c.Margin.Bottom;

					if (subItem.ControlResizeBehavior == ControlResizeBehavior.WidthFitMaintainRatio ||
						subItem.ControlResizeBehavior == ControlResizeBehavior.WidthFit ||
						subItem.ControlResizeBehavior == ControlResizeBehavior.BothFit)
						cWidth = columnRect.Width - c.Margin.Left - c.Margin.Right;

					if (subItem.ControlResizeBehavior == ControlResizeBehavior.HeightFitMaintainRatio)
					{
						cWidth = (cHeight * initialSize.Width) / initialSize.Height;
						if (cWidth > columnRect.Width)
						{
							cWidth = columnRect.Width;
							cHeight = (cWidth * initialSize.Height) / initialSize.Width;
						}
					}
					else if (subItem.ControlResizeBehavior == ControlResizeBehavior.WidthFitMaintainRatio)
					{
						cHeight = (cWidth * initialSize.Height) / initialSize.Width;
						if (cHeight > itemBounds.Height)
						{
							cHeight = itemBounds.Height;
							cWidth = (cHeight * initialSize.Width) / initialSize.Height;
						}
					}

					if (column.ContentAlign == System.Drawing.ContentAlignment.MiddleRight ||
							column.ContentAlign == System.Drawing.ContentAlignment.TopRight ||
							column.ContentAlign == System.Drawing.ContentAlignment.BottomRight)
						cLeft = columnRect.Right - 3 - cWidth;
					else if (column.ContentAlign == System.Drawing.ContentAlignment.MiddleCenter ||
							column.ContentAlign == System.Drawing.ContentAlignment.TopCenter ||
							column.ContentAlign == System.Drawing.ContentAlignment.BottomCenter)
						cLeft = columnRect.Left + ((columnRect.Width - cWidth) / 2);
					else
						cLeft = columnRect.Left + c.Margin.Left;

					cTop = itemBounds.Top + ((itemBounds.Height - cHeight) / 2);

					c.SetBounds(cLeft, cTop, cWidth, cHeight, BoundsSpecified.All);
					c.Visible = true;

					if (_detailVisibleRect.Contains(c.Bounds))
						c.Region = null;
					else
					{
						Rectangle vis = Rectangle.Intersect(c.Bounds, _detailVisibleRect);
						vis.Location = new Point(0, 0);
						c.Region = new Region(vis);
					}

					c.Parent = this;
				}
				else
					drawText = subItem.Text;

				//sets the appropriate ForeColor for each subitem
				Color foreground = subItem.ForeColor;
				if (foreground == Color.Empty)
				{
					foreground = item.ForeColor;
					if (foreground == Color.Empty)
						foreground = this.ForeColor;
				}

				if (drawText != null && drawText.Length != 0)
				{
					string sp = TruncatedString(drawText,
						columnRect.Width,
						itemBounds.Height,
						imageWidth,
						fnt,
						g);
					StringFormat sf = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);

					Size textSize = g.MeasureString(sp, fnt).ToSize();

					int startLeft;
					if (column.ContentAlign == System.Drawing.ContentAlignment.MiddleRight || column.ContentAlign == System.Drawing.ContentAlignment.TopRight || column.ContentAlign == System.Drawing.ContentAlignment.BottomRight)
						startLeft = columnRect.Right - (imageWidth + textSize.Width + 4);
					else if (column.ContentAlign == System.Drawing.ContentAlignment.MiddleCenter || column.ContentAlign == System.Drawing.ContentAlignment.TopCenter || column.ContentAlign == System.Drawing.ContentAlignment.BottomCenter)
						startLeft = columnRect.Left + (columnRect.Width - (imageWidth + textSize.Width)) / 2;
					else
						startLeft = columnRect.Left;

					if (startLeft < columnRect.Left || sp != drawText)
						startLeft = columnRect.Left;

					g.DrawString(sp, fnt, new SolidBrush(foreground), (float)(startLeft + imageWidth), (float)(itemBounds.Top + ((itemBounds.Height - textSize.Height) / 2)), sf);
				}
			}
			else
			{
				Control c = item.SubItems[colIndex].ItemControl;
				if (c != null)
					c.Visible = false;
			}
		}

		private void DrawTreeExpander(Graphics g, Rectangle rectangle, bool expanded)
		{
			// Aulofee customization - start. Remove ref to C++ lib and directly access .NET theming classes.
			VisualStyleElement vse = null;
			if (UseVisualStyle)
			{
				vse = expanded ?
					VisualStyleElement.TreeView.Glyph.Opened : VisualStyleElement.TreeView.Glyph.Closed;

				if (!VisualStyleRenderer.IsElementDefined(vse))
					vse = null;
			}

			if (vse != null)
			{
				VisualStyleRenderer renderer = new VisualStyleRenderer(vse);
				renderer.DrawBackground(g, rectangle);
			}
			// Aulofee customization - end
			else
			{
				Image img = Image.FromStream(System.Reflection.Assembly.GetExecutingAssembly()
					.GetManifestResourceStream("DotNetLib.Windows.Forms.Resources.tv_" + (expanded ? "minus" : "plus") + ".bmp"));

				g.DrawImage(img, rectangle);
			}
		}

		private void DrawSelectionBox(Graphics g, Rectangle r, Color border, bool drawTop, bool drawBottom)
		{
			Pen borderPen = new Pen(border);

			g.FillRectangle(new SolidBrush(Color.FromArgb((border.A * 27) / 100, _itemSelectedColor)), r.Left + 1, r.Top, r.Width - 2, r.Height);

			g.DrawLine(borderPen, r.Left, r.Top, r.Left, r.Bottom - 1); // left
			g.DrawLine(borderPen, r.Right - 1, r.Top, r.Right - 1, r.Bottom - 1); // right

			if (drawTop)
				g.DrawLine(borderPen, r.Left + 1, r.Top, r.Right - 2, r.Top); // top

			if (drawBottom)
				g.DrawLine(borderPen, r.Left + 1, r.Bottom - 1, r.Right - 2, r.Bottom - 1); // bottom
		}

		protected virtual void DrawExtra(Graphics g, Rectangle clip)
		{
			g.ResetClip();

			g.FillRectangle(SystemBrushes.Control, clip);
		}
		#endregion

		#endregion
	}

	#region Event Handlers and Arguments
	public delegate void ContainerListViewEventHandler(object sender, ContainerListViewEventArgs e);
	public delegate void ContainerListViewCancelEventHandler(object sender, ContainerListViewCancelEventArgs e);

	public class ContainerListViewEventArgs : EventArgs
	{
		private ContainerListViewColumnHeader _column;
		private ContainerListViewItem _item;

		private Point _mousePosition;
		private MouseButtons _mouseButton;

		public ContainerListViewColumnHeader ColumnHeader
		{
			get
			{
				return _column;
			}
		}

		public ContainerListViewItem Item
		{
			get
			{
				return _item;
			}
		}

		public Point MousePosition
		{
			get
			{
				return _mousePosition;
			}
		}

		public MouseButtons MouseButton
		{
			get
			{
				return _mouseButton;
			}
		}

		internal ContainerListViewEventArgs(ContainerListViewColumnHeader column, ContainerListViewItem item)
		{
			_column = column;
			_item = item;
		}

		internal ContainerListViewEventArgs(ContainerListViewColumnHeader column, ContainerListViewItem item, MouseEventArgs e)
		{
			_column = column;
			_item = item;

			_mousePosition = new Point(e.X, e.Y);
			_mouseButton = e.Button;
		}

		internal ContainerListViewEventArgs(ContainerListViewColumnHeader column, ContainerListViewItem item, int mouseX, int mouseY, MouseButtons mouseButton)
		{
			_column = column;
			_item = item;

			_mousePosition = new Point(mouseX, mouseY);
			_mouseButton = mouseButton;
		}
	}

	public class ContainerListViewCancelEventArgs : CancelEventArgs
	{
		private ContainerListViewColumnHeader _column;
		private ContainerListViewItem _item;

		private Point _mousePosition;
		private MouseButtons _mouseButton;

		public ContainerListViewColumnHeader ColumnHeader
		{
			get
			{
				return _column;
			}
		}

		public ContainerListViewItem Item
		{
			get
			{
				return _item;
			}
		}

		public Point MousePosition
		{
			get
			{
				return _mousePosition;
			}
		}

		public MouseButtons MouseButton
		{
			get
			{
				return _mouseButton;
			}
		}

		internal ContainerListViewCancelEventArgs(ContainerListViewColumnHeader column, ContainerListViewItem item)
		{
			_column = column;
			_item = item;
		}

		internal ContainerListViewCancelEventArgs(ContainerListViewColumnHeader column, ContainerListViewItem item, MouseEventArgs e)
		{
			_column = column;
			_item = item;

			_mousePosition = new Point(e.X, e.Y);
			_mouseButton = e.Button;
		}

		internal ContainerListViewCancelEventArgs(ContainerListViewColumnHeader column, ContainerListViewItem item, int mouseX, int mouseY, MouseButtons mouseButton)
		{
			_column = column;
			_item = item;

			_mousePosition = new Point(mouseX, mouseY);
			_mouseButton = mouseButton;
		}
	}
	#endregion

	#region Type Converters
	public class ContainerListViewColumnHeaderConverter : TypeConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			ContainerListViewColumnHeader lvi = value as ContainerListViewColumnHeader;
			if (lvi != null && destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo ci = typeof(ContainerListViewColumnHeader).GetConstructor(Type.EmptyTypes);
				if (ci != null)
				{
					return new InstanceDescriptor(ci, null, false);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

	public class ContainerListViewItemConverter : TypeConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			ContainerListViewItem lvi = value as ContainerListViewItem;
			if (lvi != null && destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo ci = typeof(ContainerListViewItem).GetConstructor(Type.EmptyTypes);
				if (ci != null)
				{
					return new InstanceDescriptor(ci, null, false);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

	public class ContainerListViewSubItemConverter : TypeConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			ContainerListViewSubItem lvi = value as ContainerListViewSubItem;
			if (lvi != null && destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo ci = typeof(ContainerListViewSubItem).GetConstructor(Type.EmptyTypes);
				if (ci != null)
				{
					return new InstanceDescriptor(ci, null, false);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
	#endregion
}