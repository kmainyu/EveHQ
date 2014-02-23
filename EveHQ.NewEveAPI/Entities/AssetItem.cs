// ==============================================================================
// 
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2014  EveHQ Development Team
//   
// This file is part of EveHQ.
//  
// The source code for EveHQ is free and you may redistribute 
// it and/or modify it under the terms of the MIT License. 
// 
// Refer to the NOTICES file in the root folder of EVEHQ source
// project for details of 3rd party components that are covered
// under their own, separate licenses.
// 
// EveHQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
// license below for details.
// 
// ------------------------------------------------------------------------------
// 
// The MIT License (MIT)
// 
// Copyright © 2005-2014  EveHQ Development Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// ==============================================================================

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EveHQ.EveApi
{
    /// <summary>
    ///     The asset item.
    /// </summary>
    public sealed class AssetItem
    {
        /// <summary>
        ///     Gets the ID value of the Item. Only guaranteed unique at the time of the asset load.
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        ///     Gets the Location (sol system or star base) of the item. Not used with items that are inside a container item.
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        ///     Gets the item Type Id. This id can be used to get details from the inventory types Table of the EveDB.
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        ///     Gets how many of this item there are.
        /// </summary>
        public long Quantity { get; set; }

        public long RawQuantity { get; set; }

        /// <summary>
        ///     Gets the flag value for the item.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag",
            Justification = "It's the name of the field returned from Eve")]
        public int Flag { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this item is a singleton or not.
        /// </summary>
        public bool Singleton { get; set; }

        /// <summary>
        ///     Gets the contents of this item.
        /// </summary>
        public IEnumerable<AssetItem> Contents { get; set; }

        /// <summary>
        ///     Gets the containing item, if this item is inside a container.
        /// </summary>
        public long ParentItemId { get; set; }
    }
}