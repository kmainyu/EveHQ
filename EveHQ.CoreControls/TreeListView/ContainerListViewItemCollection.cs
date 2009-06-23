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
using System.Collections;
using System.Windows.Forms;

namespace DotNetLib.Windows.Forms
{
	/// <summary>
	/// Implements a strongly typed collection of <see cref="ContainerListViewItem"/> elements.
	/// </summary>
	/// <remarks>
	/// <b>ContainerListViewItemCollection</b> provides an <see cref="ArrayList"/>
	/// that is strongly typed for <see cref="ContainerListViewItem"/> elements.
	/// </remarks>
	public sealed class ContainerListViewItemCollection : IList, ICollection
	{
		#region Variables

		private ContainerListView _listView;
		private ContainerListViewItem _owningItem;
		private ArrayList _data = new ArrayList();

		#endregion

		#region Constructors

		internal ContainerListViewItemCollection(ContainerListViewItem owningItem)
		{
			_owningItem = owningItem;
		}

		#endregion

		/// <summary>
		/// Indicates the <see cref="ContainerListViewItem"/> at the specified indexed
		/// location in the collection.  In C#, this property is the indexer for the
		/// <b>ContainerListViewItemCollection</b> class.
		/// </summary>
		public ContainerListViewItem this[int index]
		{
			get
			{
				return _data[index] as ContainerListViewItem;
			}
			set
			{
				if(value == null)
					throw new ArgumentNullException("value", "ContainerListView cannot contain null ContainerListViewItems");

				if(value != _data[index])
				{
					// remove the existing item
					ContainerListViewItem item = this[index];
					item.OwnerListView = null;

					// and add the new item in place
					_data[index] = value;
					value.OwnerListView = _listView;
				}
			}
		}

		#region Add

		/// <summary>
		/// Adds an existing <see cref="ContainerListViewItem"/> object to the collection.
		/// </summary>
		/// <param name="item">The <b>ContainerListViewItem</b> object to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add(ContainerListViewItem item)
		{
			int index = _data.Count;

			Insert(index, item);

			return index;
		}

		/// <summary>
		/// Adds an item to the collection with the specified text.
		/// </summary>
		/// <param name="text">The text to display.</param>
		/// <returns>The <see cref="ContainerListViewItem"/> that was added to the collection.</returns>
		public ContainerListViewItem Add(string text)
		{
			return Insert(_data.Count, text);
		}

		/// <summary>
		/// Adds an item to the collection with the specified text and image index.
		/// </summary>
		/// <param name="text">The text to display.</param>
		/// <param name="imageIndex">The index of the image to display.</param>
		/// <returns>The <see cref="ContainerListViewItem"/> that was added to the collection.</returns>
		public ContainerListViewItem Add(string text, int imageIndex)
		{
			return Insert(_data.Count, text, imageIndex);
		}

		/// <summary>
		/// Adds an item to the collection with the specified properties.
		/// </summary>
		/// <param name="text">The text to display.</param>
		/// <param name="imageIndex">The index of the image to display.</param>
		/// <param name="selectedImageIndex">The index of the image to display when the item is selected.</param>
		/// <returns>The <see cref="ContainerListViewItem"/> that was added to the collection.</returns>
		public ContainerListViewItem Add(string text, int imageIndex, int selectedImageIndex)
		{
			return Insert(_data.Count, text, imageIndex, selectedImageIndex);
		}

		#endregion

		#region Insert

		/// <summary>
		/// Inserts an existing <see cref="ContainerListViewItem"/> object to the collection at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index location where the item is inserted.</param>
		/// <param name="item">The <b>ContainerListViewItem</b> object to add to the collection.</param>
		public void Insert(int index, ContainerListViewItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (_data.Count != 0 && index > 0)
            {
                ContainerListViewItem previousItem = _data[index - 1] as ContainerListViewItem;
                item.InternalPreviousItem = previousItem;
                previousItem.InternalNextItem = item;
            }
            if (_data.Count - 1 >= index)
            {
                ContainerListViewItem nextItem = _data[index] as ContainerListViewItem;
                item.InternalNextItem = nextItem;
            }

            lock (_data.SyncRoot)
                _data.Insert(index, item);

            item.InternalParentItem = _owningItem;
            item.OwnerListView = _listView;
		}

		/// <summary>
		/// Inserts an item to the collection with the specified text at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index location where the item is inserted.</param>
		/// <param name="text">The text to display.</param>
		/// <returns>The <see cref="ContainerListViewItem"/> that was added to the collection.</returns>
		public ContainerListViewItem Insert(int index, string text)
		{
			ContainerListViewItem item = new ContainerListViewItem(text);

			Insert(index, item);

			return item;
		}

		/// <summary>
		/// Inserts an item to the collection with the specified text and image index at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index location where the item is inserted.</param>
		/// <param name="text">The text to display.</param>
		/// <param name="imageIndex">The index of the image to display.</param>
		/// <returns>The <see cref="ContainerListViewItem"/> that was added to the collection.</returns>
		public ContainerListViewItem Insert(int index, string text, int imageIndex)
		{
			ContainerListViewItem item = new ContainerListViewItem(text, imageIndex);

			Insert(index, item);

			return item;
		}

		/// <summary>
		/// Inserts an item to the collection with the specified properties at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index location where the item is inserted.</param>
		/// <param name="text">The text to display.</param>
		/// <param name="imageIndex">The index of the image to display.</param>
		/// <param name="selectedImageIndex">The index of the image to display when the item is selected.</param>
		/// <returns>The <see cref="ContainerListViewItem"/> that was added to the collection.</returns>
		public ContainerListViewItem Insert(int index, string text, int imageIndex, int selectedImageIndex)
		{
			ContainerListViewItem item = new ContainerListViewItem(text, imageIndex, selectedImageIndex);

			Insert(index, item);

			return item;
		}

		#endregion

		/// <summary>
		/// Removes the specified <see cref="ContainerListViewItem"/> from the collection.
		/// </summary>
		/// <param name="item">A <see cref="ContainerListViewItem"/> to remove from the collection.</param>
		/// <custom type="modified">Fixed bugs (the method was throwing exceptions) and improved the method.</custom>
        public void Remove(ContainerListViewItem item)
        {
			// Aulofee customization - start. Bug fixed + improvments (initial method is at the end)
			if (item == null)
				return;

        	// Aulofee customization - start. Line below added.
        	_listView.BeginUpdate();

            ContainerListViewItem recalculateFrom = RemoveInternal(item);

            _listView.RecalculateItemPositions(recalculateFrom);

            // Aulofee customization - start. Line below added.
        	_listView.EndUpdate();
        }

        /// <summary>
        /// Removes the specified collection of items from the collection.
        /// </summary>
        /// <param name="item">A <see cref="ContainerListViewItemCollection"/> to remove from the collection.</param>
        public void RemoveRange(ContainerListViewItemCollection items)
        {
            if (items == null)
                return;

        	// Aulofee customization - start. Line below added.
        	_listView.BeginUpdate();

            ContainerListViewItem recalculateFrom = null;
            for (int i = items.Count - 1; i >= 0; i--)
            {
                ContainerListViewItem newRecalculateFrom = RemoveInternal(items[i]);
                if (recalculateFrom == null || newRecalculateFrom.Y < recalculateFrom.Y)
                    recalculateFrom = newRecalculateFrom;
            }

            _listView.RecalculateItemPositions(recalculateFrom);

        	// Aulofee customization - start. Line below added.
        	_listView.EndUpdate();
        }

        /// <summary>
        /// Removes the specified collection of items from the collection.
        /// </summary>
        /// <param name="item">A list of items to remove from the collection.</param>
        public void RemoveRange(System.Collections.Generic.IList<ContainerListViewItem> items)
        {
            if (items == null)
                return;

        	// Aulofee customization - start. Line below added.
        	_listView.BeginUpdate();

            ContainerListViewItem recalculateFrom = null;
            for (int i = items.Count - 1; i >= 0; i--)
            {
                ContainerListViewItem newRecalculateFrom = RemoveInternal(items[i]);
                if (recalculateFrom == null || newRecalculateFrom.Y < recalculateFrom.Y)
                    recalculateFrom = newRecalculateFrom;
            }

            _listView.RecalculateItemPositions(recalculateFrom);

        	// Aulofee customization - start. Line below added.
        	_listView.EndUpdate();
        }

        internal ContainerListViewItem RemoveInternal(ContainerListViewItem item)
        {
            ContainerListViewItem recalculateFrom = null;
            if (item.InternalPreviousItem != null && item.InternalNextItem != null)
            {
                item.InternalPreviousItem.InternalNextItem = item.InternalNextItem;
                item.InternalNextItem.InternalPreviousItem = item.InternalPreviousItem;
                recalculateFrom = item.InternalPreviousItem;
            }
            else if (item.InternalPreviousItem != null)
            {
                item.InternalPreviousItem.InternalNextItem = null;
                recalculateFrom = item.InternalPreviousItem;
            }
            else if (item.InternalNextItem != null)
            {
                item.InternalNextItem.InternalPreviousItem = null;
            }

            lock (_data.SyncRoot)
                _data.Remove(item);

            item.Selected = false;
            item.Focused = false;
            item.OwnerListView = null;
            item.InternalParentItem = null;

            foreach (ContainerListViewSubItem si in item.SubItems)
                if (si.ItemControl != null)
                    si.ItemControl.Visible = false;

            return recalculateFrom;
        }

		/// <summary>
		/// Adds an array of <see cref="ContainerListViewItem"/> objects to the collection.
		/// </summary>
		/// <param name="items">An array of <see cref="ContainerListViewItem"/> objects to add to the collection.</param>
		public void AddRange(ContainerListViewItem[] items)
		{
            if (items == null)
                return;

			_listView.BeginUpdate();

			lock(_data.SyncRoot)
				for(int index = 0; index < items.Length; ++index)
					Add(items[index]);

			_listView.EndUpdate();
		}

		/// <summary>
		/// Returns the index within the collection of the specified item.
		/// </summary>
		/// <param name="item">A <see cref="ContainerListViewItem"/> representing the item to locate in the collection.</param>
		/// <returns>The zero-based index of the item's location in the collection.  If the item is not located in the collection the return value is negative one (-1).</returns>
		public int IndexOf(ContainerListViewItem item)
		{
			return _data.IndexOf(item);
		}

		/// <summary>
		/// Determines whether the specified item is located in the collection.
		/// </summary>
		/// <param name="item">A <see cref="ContainerListViewItem"/> representing the item to locate in the collection.</param>
		/// <returns><b>true</b> if the column is contained in the collection; otherwise, <b>false</b>.</returns>
		public bool Contains(ContainerListViewItem item)
		{
			return _data.Contains(item);
		}

		/// <summary>
		/// Removes all items from the collection.
		/// </summary>
		public void Clear()
		{
			_listView.BeginUpdate();

            //avoid firing one SelectedItemsChanged event for every selected item
            if (_listView.SelectedItems.Count > 0)
                _listView.SelectedItems.Clear();

			for(int index = 0; index < _data.Count; ++index)
			{
				ContainerListViewItem item = this[index];

				item.Selected = false;
				item.Focused = false;
				item.OwnerListView = null;

                for (int childIndex = 0; childIndex < item.Items.Count; ++childIndex)
                {
                    for (int subIndex = 0; subIndex < item.Items[childIndex].SubItems.Count; ++subIndex)
                    {
                        if (item.Items[childIndex].SubItems[subIndex].ItemControl != null)
                        {
                            item.Items[childIndex].SubItems[subIndex].ItemControl.Parent = null;
                            item.Items[childIndex].SubItems[subIndex].ItemControl.Visible = false;
                            item.Items[childIndex].SubItems[subIndex].ItemControl = null;
                        }
                    }
                }

				for(int subIndex = 0; subIndex < item.SubItems.Count; ++subIndex)
				{
					if (item.SubItems[subIndex].ItemControl != null)
					{
						item.SubItems[subIndex].ItemControl.Parent = null;
						item.SubItems[subIndex].ItemControl.Visible = false;
						item.SubItems[subIndex].ItemControl = null;
					}
				}
			}
			_data.Clear();

			_listView.EndUpdate();
		}

		/// <summary>
		/// Sorts the elements using the specified comparer.
		/// </summary>
		/// <param name="comparer">The <see cref="IComparer"/> to use when comparing elements.</param>
		/// <param name="recursiveSort">Whether to sort these items child items as well.</param>
		public void Sort(IComparer comparer, bool recursiveSort)
		{
			try
			{
				_data.Sort(comparer);

				ContainerListViewItem lastItem = null;
				ContainerListViewItem curItem = null;

				for(int index = 0; index < _data.Count; ++index)
				{
					curItem = this[index];

					curItem.InternalPreviousItem = lastItem;
					if(lastItem != null)
						lastItem.InternalNextItem = curItem;

					lastItem = curItem;

					if(recursiveSort && curItem.HasChildren)
						curItem.Items.Sort(comparer, recursiveSort);
				}

				if(curItem != null)
					curItem.InternalNextItem = null;
			}
			catch
			{
				// TODO: should likely refine this and determine the cause of the error and handle appropriately.
			}
		}

		/// <summary>
		/// Copies the entire collection into an existing array at a specified location within the array.
		/// </summary>
		/// <param name="array">The destination array.</param>
		/// <param name="arrayIndex">The zero-based relative index in <em>array</em> at which copying begins.</param>
		public void CopyTo(ContainerListViewItem[] array, int arrayIndex)
		{
			_data.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets the number of items in this collection
		/// </summary>
		public int Count
		{
			get
			{
				return _data.Count;
			}
		}

		#region IList

		int IList.Add(object value)
		{
			return this.Add(value as ContainerListViewItem);
		}

		bool IList.Contains(object value)
		{
			return this.Contains(value as ContainerListViewItem);
		}

		int IList.IndexOf(object value)
		{
			return this.IndexOf(value as ContainerListViewItem);
		}

		void IList.Insert(int index, object value)
		{
			this.Insert(index, value as ContainerListViewItem);
		}

		void IList.Remove(object value)
		{
			this.Remove(value as ContainerListViewItem);
		}

		void IList.RemoveAt(int index)
		{
			_data.RemoveAt(index);
		}

		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = value as ContainerListViewItem;
			}
		}

		#endregion

		#region ICollection

		void ICollection.CopyTo(Array array, int index)
		{
			this.CopyTo((ContainerListViewItem[])array, index);
		}

		object ICollection.SyncRoot
		{
			get
			{
				return _data.SyncRoot;
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				return _data.IsSynchronized;
			}
		}

		#endregion

		#region IEnumerable

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _data.GetEnumerator();
		}

		#endregion

		internal ContainerListView InternalListView
		{
			set
			{
				_listView = value;
			}
		}
	}
}
