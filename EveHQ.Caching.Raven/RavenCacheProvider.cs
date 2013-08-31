// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (RavenCacheProvider.cs), is part of EveHQ.
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

namespace EveHQ.Caching.Raven
{
    using System;
    using System.Collections.Concurrent;

    using global::Raven.Client;
    using global::Raven.Client.Embedded;

    /// <summary>The raven cache provider.</summary>
    public sealed class RavenCacheProvider : ICacheProvider, IDisposable
    {
        /// <summary>The _db.</summary>
        private readonly EmbeddableDocumentStore _db;

        private readonly ConcurrentDictionary<string, object> _memCache = new ConcurrentDictionary<string, object>();

        private readonly object _lockObject = new object();

        /// <summary>Initializes a new instance of the <see cref="RavenCacheProvider"/> class.</summary>
        /// <param name="dataFolder">The data folder.</param>
        public RavenCacheProvider(string dataFolder)
        {
            _db = new EmbeddableDocumentStore { DataDirectory = dataFolder, Conventions = { AllowMultipuleAsyncOperations = true } };
            _db.Initialize();

        }

        /// <summary>Adds an item to the document cache</summary>
        /// <param name="key">The key to identify the cache item</param>
        /// <param name="value">The data value to cache</param>
        /// <param name="cacheUntil">The time when this data should be considered dirty.</param>
        /// <typeparam name="T">data type to cache</typeparam>
        public void Add<T>(string key, T value, DateTimeOffset cacheUntil)
        {
            var cacheItem = new CacheItem<T> { Data = value, CacheUntil = cacheUntil };
            using (IDocumentSession session = _db.OpenSession())
            {
                session.Store(cacheItem, key);
                session.SaveChanges();
            }
        }

        /// <summary>Gets an item from caches</summary>
        /// <param name="key">The key to retrieve</param>
        /// <typeparam name="T">data type of the data object</typeparam>
        /// <returns>The returned cache item.</returns>
        public CacheItem<T> Get<T>(string key)
        {
            CacheItem<T> data = null;
            object temp;
            if (_memCache.TryGetValue(key, out temp) && (data = temp as CacheItem<T>) != null)
            {
                return data;
            }

            lock (_lockObject)
            {
                // double check that the memory cache wasn't populated while waiting.
                if (_memCache.TryGetValue(key, out temp) && (data = temp as CacheItem<T>) != null)
                {
                    return data;
                }

                using (IDocumentSession session = _db.OpenSession())
                {
                    var result = session.Load<CacheItem<T>>(key);

                    if (result != null)
                    {
                        _memCache.TryAdd(key, result);
                        data = result;
                    }
                }
            }
            return data;
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}