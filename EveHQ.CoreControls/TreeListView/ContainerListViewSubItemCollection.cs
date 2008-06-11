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

namespace DotNetLib.Windows.Forms
{
	/// <summary>
	/// Implements a strongly typed collection of <see cref="ContainerListViewSubItem"/> elements.
	/// </summary>
	/// <remarks>
	/// <b>ContainerListViewSubItemCollection</b> provides an <see cref="ArrayList"/> that
	/// is strongly typed for <see cref="ContainerListViewSubItem"/> elements.
	/// </remarks>
	public sealed class ContainerListViewSubItemCollection : IList, ICollection
	{
		#region Variables

		private ContainerListViewItem _owningItem;
		private ArrayList _data;

		#endregion

		#region Constructors

		internal ContainerListViewSubItemCollection(ContainerListViewItem item)
		{
			_owningItem = item;

			if(_owningItem.ListView != null)
				_data = new ArrayList(_owningItem.ListView.Columns.Count);
			else
				_data = new ArrayList();
		}

		#endregion

		/// <summary>
		/// Indicates the <see cref="ContainerListViewSubItem"/> at the specified indexed
		/// location in the collection.  In C#, this property is the indexer for the
		/// <b>ContainerListViewSubItemCollection</b> class.
		/// </summary>
		public ContainerListViewSubItem this[int index]
		{
			get
			{
				return _data[index] as ContainerListViewSubItem;
			}
			set
			{
				if(value == null)
					throw new ArgumentNullException("value", "ContainerListViewItem cannot contain null sub items");

				if(value != _data[index])
				{
					// remove the existing sub item
					this[index].Collection = null;

					// add the new sub item in place
					_data[index] = value;
					value.Collection = this;
				}
			}
		}
		
		/// <summary>
		/// Gets the number of <see cref="ContainerListViewSubItem"/> elements in this collection.
		/// </summary>
		public int Count
		{
			get
			{
				return _data.Count;
			}
		}

		/// <summary>
		/// Gets the <see cref="ContainerListViewItem"/> that this collection is attached to.
		/// </summary>
		public ContainerListViewItem OwningItem
		{
			get
			{
				return _owningItem;
			}
		}

		/// <summary>
		/// Adds an existing <see cref="ContainerListViewSubItem"/> object to the collection.
		/// </summary>
		/// <param name="subItem">The <b>ContainerListViewSubItem</b> object to add to the collection.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add(ContainerListViewSubItem subItem)
		{
			if(_owningItem.ListView != null)
				throw new NotSupportedException("Cannot modify sub item collection while the item is attached to a list.");

			int index = _data.Count;

			InternalInsert(index, subItem);

			return index;
		}

		/// <summary>
		/// Inserts an existing <see cref="ContainerListViewSubItem"/> object to the collection at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index location where the item is inserted.</param>
		/// <param name="subItem">The <b>ContainerListViewSubItem</b> object to add to the collection.</param>
		public void Insert(int index, ContainerListViewSubItem subItem)
		{
			if(_owningItem.ListView != null)
				throw new NotSupportedException("Cannot modify sub item collection while the item is attached to a list.");

			InternalInsert(index, subItem);
		}

		/// <summary>
		/// Removes the specified <see cref="ContainerListViewItem"/> from the collection.
		/// </summary>
		/// <param name="subItem">A <see cref="ContainerListViewSubItem"/> to remove from the collection.</param>
		public void Remove(ContainerListViewSubItem subItem)
		{
            if (subItem == null)
                return;

			if(_owningItem.ListView != null)
				throw new NotSupportedException("Cannot modify sub item collection while the item is attached to a list.");

			lock(_data.SyncRoot)
			{
				_data.Remove(subItem);
                subItem.Collection = null;
                if (subItem.ItemControl != null)
                    subItem.ItemControl.Visible = false;
			}
		}

		/// <summary>
		/// Removes an item from the collection at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index position of the item that is to be removed.</param>
		public void RemoveAt(int index)
		{
			if(_owningItem.ListView != null)
				throw new NotSupportedException("Cannot modify a sub item collection while the item is attached to a list.");

			ContainerListViewSubItem subItem = this[index];

			lock(_data.SyncRoot)
			{
				_data.RemoveAt(index);
				subItem.Collection = null;
                if (subItem.ItemControl != null)
                    subItem.ItemControl.Visible = false;
			}
		}

		/// <summary>
		/// Adds an array of <see cref="ContainerListViewSubItem"/> objects to the collection.
		/// </summary>
		/// <param name="subItems">An array of <see cref="ContainerListViewSubItem"/> objects to add to the collection.</param>
		public void AddRange(ContainerListViewSubItem[] subItems)
		{
            if (subItems == null)
                return;

			if(_owningItem.ListView != null)
				throw new NotSupportedException("Cannot modify sub item collection while the item is attached to a list.");

            _owningItem.ListView.BeginUpdate();

			lock(_data.SyncRoot)
				foreach(ContainerListViewSubItem subItem in subItems)
                    Add(subItem);

            _owningItem.ListView.EndUpdate();
		}

		/// <summary>
		/// Returns the index within the collection of the specified sub item.
		/// </summary>
		/// <param name="item">A <see cref="ContainerListViewSubItem"/> representing the item to locate in the collection.</param>
		/// <returns>The zero-based index of the item's location in the collection.  If the item is not located in the collection the return value is negative one (-1).</returns>
		public int IndexOf(ContainerListViewSubItem item)
		{
			return _data.IndexOf(item);
		}

		/// <summary>
		/// Determines whether the specified item is located in the collection.
		/// </summary>
		/// <param name="item">A <see cref="ContainerListViewSubItem"/> representing the item to locate in the collection.</param>
		/// <returns><b>true</b> if the column is contained in the collection; otherwise, <b>false</b>.</returns>
		public bool Contains(ContainerListViewSubItem item)
		{
			return _data.Contains(item);
		}

		/// <summary>
		/// Removes all sub items from the collection.
		/// </summary>
		public void Clear()
		{
			if(_owningItem.ListView != null)
				throw new NotSupportedException("Cannot modify sub item collection while the item is attached to a list.");

			lock(_data.SyncRoot)
				_data.Clear();
		}

		/// <summary>
		/// Makes a shallow copy of the entire <see cref="ContainerListViewSubItemCollection"/> to a one-dimensional array, starting at the specified index of the target array.
		/// </summary>
		/// <param name="array">The one-dimensional array that is the destination of the elements.</param>
		/// <param name="arrayIndex">The zero-based index in <em>array</em> at which copying begins.</param>
		public void CopyTo(ContainerListViewSubItem[] array, int arrayIndex)
		{
			_data.CopyTo(array, arrayIndex);
		}

		internal void InternalInsert(int index, ContainerListViewSubItem subItem)
		{
			lock(_data.SyncRoot)
			{
				_data.Insert(index, subItem);
				subItem.Collection = this;
			}
		}

		internal void AdjustSize(int newSize)
		{
			// if we don't have enough, add them
			for(int index = Count; index < newSize; ++index)
				InternalInsert(index, new ContainerListViewSubItem(index));

			// if we have too many, remove them from the end
			for(int index = Count - 1; index >= newSize; --index)
				_data.RemoveAt(index);
		}

		#region IList

		int IList.Add(object value)
		{
			return this.Add(value as ContainerListViewSubItem);
		}

		bool IList.Contains(object value)
		{
			return this.Contains(value as ContainerListViewSubItem);
		}

		int IList.IndexOf(object value)
		{
			return this.IndexOf(value as ContainerListViewSubItem);
		}

		void IList.Insert(int index, object value)
		{
			this.Insert(index, value as ContainerListViewSubItem);
		}

		void IList.Remove(object value)
		{
			this.Remove(value as ContainerListViewSubItem);
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
				this[index] = value as ContainerListViewSubItem;
			}
		}

		#endregion

		#region ICollection

		void ICollection.CopyTo(Array array, int index)
		{
			_data.CopyTo(array, index);
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
	}
}
