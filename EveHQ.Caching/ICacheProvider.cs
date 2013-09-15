// ===========================================================================
// <copyright file="ICacheProvider.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (ICacheProvider.cs), is part of EveHQ.
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

    /// <summary>The CacheProvider interface.</summary>
    public interface ICacheProvider
    {
        #region Public Methods and Operators

        /// <summary>The add.</summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="cacheUntil">The cache until.</param>
        /// <typeparam name="T">Type to cache</typeparam>
        void Add<T>(string key, T value, DateTimeOffset cacheUntil);

        /// <summary>The get.</summary>
        /// <param name="key">The key.</param>
        /// <typeparam name="T">Type to retrieve</typeparam>
        /// <returns>An instance of the target Type</returns>
        CacheItem<T> Get<T>(string key);

        #endregion
    }
}