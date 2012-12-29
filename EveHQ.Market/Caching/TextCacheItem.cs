//  ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (TextCacheItem.cs), is part of EveHQ.
// 
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 2 of the License, or
//  (at your option) any later version.
// 
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
// =========================================================================

namespace EveHQ.Market
{
    using System;

    /// <summary>
    /// Data entity for an item stored with a text file cache.
    /// </summary>
    /// <typeparam name="T">Type of the data</typeparam>
    public class TextCacheItem<T>
    {
        /// <summary>
        /// Gets or sets the cache expiry time.
        /// </summary>
        public DateTimeOffset CacheUntil { get; set; }

        /// <summary>
        /// Gets or sets the data being cached.
        /// </summary>
        public T Data { get; set; }
    }
}