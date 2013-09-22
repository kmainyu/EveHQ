// ===========================================================================
// <copyright file="CacheItem.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (CacheItem.cs), is part of EveHQ.
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 2 of the License, or
//  (at your option) any later version.
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with EveHQ.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// ============================================================================
namespace EveHQ.Caching
{
    using System;

    /// <summary>Data entity for an item stored with a file cache.</summary>
    /// <typeparam name="T">Type of the data</typeparam>
    public sealed class CacheItem<T>
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the cache expiry time.
        /// </summary>
        public DateTimeOffset CacheUntil { get; set; }

        /// <summary>
        ///     Gets or sets the data being cached.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this data is dirty and should be refreshed.
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return DateTimeOffset.Now > CacheUntil;
            }
        }

        #endregion
    }
}