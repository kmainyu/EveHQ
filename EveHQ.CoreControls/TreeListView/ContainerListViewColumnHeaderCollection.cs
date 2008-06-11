/***************************************************************************\
|  Author:  Josh Carlson                                                    |
|                                                                           |
|  This work builds on code posted to CodeProject                           |
|  Jon Rista http://codeproject.com/cs/miscctrl/extendedlistviews.asp       |
|  and also updates by                                                      |
|  Bill Seddon http://codeproject.com/cs/miscctrl/Extended_List_View_2.asp  |
|                                                                           |
|  This code is provided "as is" and no warranty about its fitness for any  |
|  specific task is expressed or implied.  If you choose to use this code,  |
|  you do so at your own risk.                                              |
\***************************************************************************/

using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace DotNetLib.Windows.Forms
{
	/// <summary>
	/// Implements a strongly typed collection of <see cref="ContainerListViewColumnHeader"/> elements.
	/// </summary>
	/// <remarks>
	/// <b>ContainerListViewColumnHeaderCollection</b> provides an <see cref="ArrayList"/> 
	/// that is strongly typed for <see cref="ContainerListViewColumnHeader"/> elements.
	/// </remarks>    
	public sealed class ContainerListViewColumnHeaderCollection : IList, ICollection
	{
		#region Variables

		private ContainerListView _listView;
		private ArrayList _physicalData = new ArrayList(); // actual ordering of columns
		private ArrayList _logicalData = new ArrayList(); // display ordering of columns

		#endregion

		#region Constructors

		internal ContainerListViewColumnHeaderCollection(ContainerListView listView)
		{
			_listView = listView;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the <see cref="ContainerListViewColumnHeader"/> at the specified index.
		/// In C#, this property is the indexer for the <b>ContainerListViewColumnHeaderCollection</b> class.
		/// </summary>
		/// <value>The <see cref="ContainerListViewColumnHeader"/> at the specified index.</value>
		public ContainerListViewColumnHeader this[int index]
		{
			get
			{ 
				return _physicalData[index] as ContainerListViewColumnHeader;
			}
			set
			{
				if(value == null)
					throw new ArgumentNullException("value", "ContainerListView cannot contain null ContainerListViewColumnHeaders");

				if(value == _physicalData[index])
					return;

				ContainerListViewColumnHeader currentColumn = this[index];

				if(value.Collection == this) // incoming column is part of this collection already
				{
					// just swap them, display index will stay the same
					_physicalData[value.Index] = currentColumn;
					_physicalData[index] = value;
				}
				else
				{
					// let the current column know they they don't belong to us anymore
					currentColumn.Collection = null;

					if(value.Collection != null) // incoming column is part of a different collection, remove it from that collection
						value.Collection.Remove(value);

					_physicalData[index] = value;
					_logicalData[index] = value;

					value.Collection = this;
				}
			}
		}

		/// <summary>
		/// Gets the number of <see cref="ContainerListViewColumnHeader"/> in this collection.
		/// </summary>
		public int Count
		{
			get
			{
				return _physicalData.Count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="ContainerListViewColumnHeaderCollection"/> has a fixed size.
		/// </summary>
		/// <value>This property is always <b>false</b>.</value>
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="ContainerListViewColumnHeaderCollection"/> is read-only.
		/// </summary>
		/// <value>This property is always <b>false</b>.</value>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="ContainerListViewColumnHeaderCollection"/>.
		/// </summary>
		/// <value>An object that can be used to synchronize access to the <see cref="ContainerListViewColumnHeaderCollection"/>.</value>
		public object SyncRoot
		{
			get
			{
				return _physicalData.SyncRoot;
			}
		}

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="ContainerListViewColumnHeaderCollection"/> is synchronized.
		/// </summary>
		/// <value>This property is always <b>false</b>.</value>
		public bool IsSynchronized
		{
			get
			{
				return _physicalData.IsSynchronized;
			}
		}

		/// <summary>
		/// Gets the <see cref="ContainerListView"/> that this collection is attached to.
		/// </summary>
		public ContainerListView ListView
		{
			get
			{
				return _listView;
			}
		}

		#endregion

		#region Add/AddRange

		/// <summary>
		/// Adds an existing <see cref="ContainerListViewColumnHeader"/> object to the collection.
		/// </summary>
		/// <param name="column">The <b>ContainerListViewColumnHeader</b> object to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add(ContainerListViewColumnHeader column)
		{
			int index = _physicalData.Count;

			Insert(index, column);

			return index;
		}

		/// <summary>
		/// Adds a column to the collection with the specified text.
		/// </summary>
		/// <param name="text">The text to display.</param>
		/// <returns>The <see cref="ContainerListViewColumnHeader"/> that was added to the collection.</returns>
		public ContainerListViewColumnHeader Add(string text)
		{
			return Insert(_physicalData.Count, text);
		}

		/// <summary>
		/// Adds a column to the collection with the specified text and width.
		/// </summary>
		/// <param name="text">The text to display.</param>
		/// <param name="width">The starting width.</param>
		/// <returns>The <see cref="ContainerListViewColumnHeader"/> that was added to the collection.</returns>
		public ContainerListViewColumnHeader Add(string text, int width)
		{
			return Insert(_physicalData.Count, text, width);
		}

		// Aulofee customization - start. Method added (can directly specify the sort type)
		/// <summary>
		/// Adds a column to the collection with the specified text and width.
		/// </summary>
		/// <param name="text">The text to display.</param>
		/// <param name="width">The starting width.</param>
		/// <param name="sortDataType">The data type to use to sort the column.</param>
		/// <returns>The <see cref="ContainerListViewColumnHeader"/> that was added to the collection.</returns>
		public ContainerListViewColumnHeader Add(string text, int width, SortDataType sortDataType)
		{
			return Insert(_physicalData.Count, text, width, sortDataType);
		}
		// Aulofee customization - end
		
		/// <summary>
		/// Adds a column to the collection with the specified properties.
		/// </summary>
		/// <param name="text">The text to display.</param>
		/// <param name="width">The starting width.</param>
		/// <param name="horizontalAlign">The horizontal alignment, will default vertical alignment to middle.</param>
		/// <returns>The <see cref="ContainerListViewColumnHeader"/> that was added to the collection.</returns>
		public ContainerListViewColumnHeader Add(string text, int width, HorizontalAlignment horizontalAlign)
		{
			return Insert(_physicalData.Count, text, width, horizontalAlign);
		}

		/// <summary>
		/// Adds a column to the collection with the specified properties.
		/// </summary>
		/// <param name="text">The text to display.</param>
		/// <param name="width">The starting width.</param>
		/// <param name="contentAlign">The content alignment.</param>
		/// <returns>The <see cref="ContainerListViewColumnHeader"/> that was added to the collection.</returns>
		public ContainerListViewColumnHeader Add(string text, int width, ContentAlignment contentAlign)
		{
			return Insert(_physicalData.Count, text, width, contentAlign);
		}

		/// <summary>
		/// Adds an array of <see cref="ContainerListViewColumnHeader"/> objects to the collection.
		/// </summary>
		/// <param name="columns">An array of <see cref="ContainerListViewColumnHeader"/> objects to add to the collection.</param>
		public void AddRange(ContainerListViewColumnHeader[] columns)
		{
            if (columns == null)
                return;

			_listView.BeginUpdate();

			lock(_physicalData.SyncRoot)
				for(int index = 0; index < columns.Length; ++index)
					Add(columns[index]);

			_listView.EndUpdate();
		}

		#endregion

		#region Insert

		/// <summary>
		/// Inserts an existing <see cref="ContainerListViewColumnHeader"/> into the collection at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index location where the column is inserted.</param>
		/// <param name="column">The <see cref="ContainerListViewColumnHeader"/> that represents the column to insert.</param>
		public void Insert(int index, ContainerListViewColumnHeader column)
		{
            if (column == null)
                throw new ArgumentNullException("column");

			lock(_physicalData.SyncRoot)
			{
				_physicalData.Insert(index, column);
				_logicalData.Add(column);
				column.Collection = this;
			}

            if (ListView != null)
            {
                //create a new subitem
                foreach (ContainerListViewItem item in ListView.Items)
                    item.SubItems.InternalInsert(index, new ContainerListViewSubItem(index));
                //recalculate layout and redraw
                ListView.ColumnInvalidated(column, true, true);
            }
		}

		/// <summary>
		/// Creates a new header and inserts it into the collection at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index location where the column is inserted.</param>
		/// <param name="text">The text to display.</param>
		/// <returns>The <see cref="ContainerListViewColumnHeader"/> that was inserted into the collection.</returns>
		public ContainerListViewColumnHeader Insert(int index, string text)
		{
			ContainerListViewColumnHeader column = new ContainerListViewColumnHeader(text);

			Insert(index, column);

			return column;
		}

		/// <summary>
		/// Creates a new header and inserts it into the collection at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index location where the column is inserted.</param>
		/// <param name="text">The text to display.</param>
		/// <param name="width">The starting width.</param>
		/// <returns>The <see cref="ContainerListViewColumnHeader"/> that was inserted into the collection.</returns>
		public ContainerListViewColumnHeader Insert(int index, string text, int width)
		{
			ContainerListViewColumnHeader column = new ContainerListViewColumnHeader(text, width);

			Insert(index, column);

			return column;
		}

		// Aulofee customization - start. Method added (can directly specify the sort type)
		/// <summary>
		/// Creates a new header and inserts it into the collection at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index location where the column is inserted.</param>
		/// <param name="text">The text to display.</param>
		/// <param name="width">The starting width.</param>
		/// <param name="sortDataType">The data type to use to sort the column.</param>
		/// <returns>The <see cref="ContainerListViewColumnHeader"/> that was inserted into the collection.</returns>
		public ContainerListViewColumnHeader Insert(int index, string text, int width, SortDataType sortDataType)
		{
			ContainerListViewColumnHeader column = new ContainerListViewColumnHeader(text, width);
			column.SortDataType = sortDataType;

			Insert(index, column);

			return column;
		}
		// Aulofee customization - end

		/// <summary>
		/// Creates a new header and inserts it into the collection at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index location where the column is inserted.</param>
		/// <param name="text">The text to display.</param>
		/// <param name="width">The starting width.</param>
		/// <param name="horizontalAlign">The horizontal alignment of the text, will default vertical alignment to middle.</param>
		/// <returns>The <see cref="ContainerListViewColumnHeader"/> that was inserted into the collection.</returns>
		public ContainerListViewColumnHeader Insert(int index, string text, int width, HorizontalAlignment horizontalAlign)
		{
			ContainerListViewColumnHeader column = new ContainerListViewColumnHeader(text, width, horizontalAlign);

			Insert(index, column);

			return column;
		}

		/// <summary>
		/// Creates a new header and inserts it into the collection at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index location where the column is inserted.</param>
		/// <param name="text">The text to display.</param>
		/// <param name="width">The starting width.</param>
		/// <param name="contentAlign">The content alignment.</param>
		/// <returns>The <see cref="ContainerListViewColumnHeader"/> that was inserted into the collection.</returns>
		public ContainerListViewColumnHeader Insert(int index, string text, int width, ContentAlignment contentAlign)
		{
			ContainerListViewColumnHeader column = new ContainerListViewColumnHeader(text, width, contentAlign);

			Insert(index, column);

			return column;
		}

		#endregion

		#region Remove/RemoveAt

		/// <summary>
		/// Removes the specified <see cref="ContainerListViewColumnHeader"/> from the collection.
		/// </summary>
		/// <param name="column">A <see cref="ContainerListViewColumnHeader"/> to remove from the collection.</param>
		public void Remove(ContainerListViewColumnHeader column)
		{
			Remove(column, false);
		}

		/// <summary>
		/// Removes the specified <see cref="ContainerListViewColumnHeader"/> from the collection.
		/// </summary>
		/// <param name="column">A <see cref="ContainerListViewColumnHeader"/> to remove from the collection.</param>
		/// <param name="removeSubItems"><b>True</b> if you want to remove the subitems, <b>false</b> otherwise.</param>
		public void Remove(ContainerListViewColumnHeader column, bool removeSubItems)
		{
            if (column == null)
                return;

			lock(_physicalData.SyncRoot)
			{
                if (_listView != null)
                {
                    if (removeSubItems)
                    {
                        //remove subitems
                        foreach (ContainerListViewItem item in _listView.Items)
                            item.RemoveSubItem(column.Index);
                    }
                    else
                    {
                        //hide subitem controls
                        foreach (ContainerListViewItem item in _listView.Items)
                        {
                            Control subItemControl = item.SubItems[column.Index].ItemControl;
                            if (subItemControl != null)
                                subItemControl.Visible = false;
                        }
                    }
                }
				_physicalData.Remove(column);
				_logicalData.Remove(column);

				column.Collection = null;
			}

            if (ListView != null)
                ListView.ColumnInvalidated(column, true, true);
		}

		/// <summary>
		/// Removes the column at the specified location.
		/// </summary>
		/// <param name="index">The zero-based index of the column you want to remove.</param>
		public void RemoveAt(int index)
		{
			RemoveAt(index, false);
		}

		/// <summary>
		/// Removes the column at the specified location.
		/// </summary>
		/// <param name="index">The zero-based index of the column you want to remove.</param>
		/// <param name="removeSubItems"><b>True</b> if you want to remove the subitems, <b>false</b> otherwise.</param>
		public void RemoveAt(int index, bool removeSubItems)
		{
			Remove(this[index], removeSubItems);
		}

		#endregion

		#region Others

		/// <summary>
		/// Returns the index within the collection of the specified column.
		/// </summary>
		/// <param name="column">A <see cref="ContainerListViewColumnHeader"/> representing the column to locate in the collection.</param>
		/// <returns>The zero-based index of the column's location in the collection.  If the column is not located in the collection the return value is negative one (-1).</returns>
		public int IndexOf(ContainerListViewColumnHeader column)
		{
			return _physicalData.IndexOf(column);
		}

		/// <summary>
		/// Returns the display index of the specified column.
		/// </summary>
		/// <param name="column">A <see cref="ContainerListViewHeader"/> representing the column to locate.</param>
		/// <returns>The zero-based display index of the column's location in the collection.  If the column is not located in the collection the return value is negative one (-1).</returns>
		public int DisplayIndexOf(ContainerListViewColumnHeader column)
		{
			return _logicalData.IndexOf(column);
		}

		/// <summary>
		/// Determines whether the specified column is located in the collection.
		/// </summary>
		/// <param name="column">A <see cref="ContainerListViewColumnHeader"/> representing the column to locate in the collection.</param>
		/// <returns><b>true</b> if the column is contained in the collection; otherwise, <b>false</b>.</returns>
		public bool Contains(ContainerListViewColumnHeader column)
		{
			return _physicalData.Contains(column);
		}

		/// <summary>
		/// Removes all columns from the collection.
		/// </summary>
		public void Clear()
		{
			_listView.BeginUpdate();

			lock(_physicalData.SyncRoot)
			{
				for(int index = 0; index < _physicalData.Count; ++index)
					this[index].Collection = null;

				_physicalData.Clear();
				_logicalData.Clear();
			}

			_listView.EndUpdate();
		}

		/// <summary>
		/// Copies the entire collection into an existing array at a specified location within the array.
		/// </summary>
		/// <param name="array">The destination array.</param>
		/// <param name="arrayIndex">The zero-based relative index in <em>array</em> at which copying begins.</param>
		public void CopyTo(ContainerListViewItem[] array, int arrayIndex)
		{
			_physicalData.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Returns an <see cref="IEnumerator"/> for the collection.
		/// </summary>
		/// <returns>An <see cref="IEnumerator"/> for the collection.</returns>
		public IEnumerator GetEnumerator()
		{
			return _physicalData.GetEnumerator();
		}

		/// <summary>
		/// Returns an <see cref="IEnumerator"/> for the collection ordered by the display index.
		/// </summary>
		/// <returns>An <see cref="IEnumerator"/> for the collection ordered by the display index.</returns>
		public IEnumerable GetDisplayOrderEnumerator()
		{
			return _logicalData;
		}

		internal void SetDisplayIndex(ContainerListViewColumnHeader column, int newDisplayIndex)
		{
			if(!Contains(column))
				return;

			if(newDisplayIndex >= Count)
				newDisplayIndex = Count - 1;

			int curDisplayIndex = column.DisplayIndex;

			if(curDisplayIndex == newDisplayIndex)
				return;

			_logicalData.RemoveAt(curDisplayIndex);
			_logicalData.Insert(newDisplayIndex, column);

			if(_listView != null)
				_listView.ColumnInvalidated(null, false, true);
		}

		#endregion

		#region Explicit interface implementations

		int IList.Add(object value)
		{
			return this.Add(value as ContainerListViewColumnHeader);
		}

		bool IList.Contains(object value)
		{
			return this.Contains(value as ContainerListViewColumnHeader);
		}

		int IList.IndexOf(object value)
		{
			return this.IndexOf(value as ContainerListViewColumnHeader);
		}

		void IList.Insert(int index, object value)
		{
			this.Insert(index, value as ContainerListViewColumnHeader);
		}

		void IList.Remove(object value)
		{
			this.Remove(value as ContainerListViewColumnHeader);
		}

		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = value as ContainerListViewColumnHeader;
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
			this.CopyTo((ContainerListViewItem[])array, index);
		}

		#endregion
	}
}
