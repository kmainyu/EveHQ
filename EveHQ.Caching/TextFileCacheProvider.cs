// ===========================================================================
// <copyright file="TextFileCacheProvider.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (TextFileCacheProvider.cs), is part of EveHQ.
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
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Text;

    using Newtonsoft.Json;

    /// <summary>
    ///     A simple caching system that uses text files for persistence.
    /// </summary>
    public class TextFileCacheProvider : ICacheProvider
    {
        #region Constants

        /// <summary>
        ///     Cache file naming format.
        /// </summary>
        private const string CacheFileFormat = "{0}.json.txt";

        #endregion

        #region Fields

        /// <summary>
        ///     In memory copy of cache
        /// </summary>
        private readonly ConcurrentDictionary<string, object> _memCache = new ConcurrentDictionary<string, object>();

        /// <summary>
        ///     root path of the current cache instance.
        /// </summary>
        private readonly string _rootPath;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the TextFileCacheProvider class.</summary>
        /// <param name="rootPath">Root path of the cache instance.</param>
        public TextFileCacheProvider(string rootPath)
        {
            _rootPath = rootPath;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Adds an item to the cache.</summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="cacheUntil">The cache Until.</param>
        /// <typeparam name="T">Type to Cache</typeparam>
        public void Add<T>(string key, T value, DateTimeOffset cacheUntil)
        {
            var cacheitem = new CacheItem<T> { CacheUntil = cacheUntil, Data = value };

            _memCache[key] = cacheitem;
            SaveToFile(key, cacheitem);
        }

        /// <summary>Gets an item from cache.</summary>
        /// <param name="key">The key.</param>
        /// <typeparam name="T">Tye to get from cache</typeparam>
        /// <returns>The <see cref="CacheItem"/>.</returns>
        public CacheItem<T> Get<T>(string key)
        {
            CacheItem<T> result = null;

            object item;

            if (!_memCache.TryGetValue(key, out item))
            {
                // not in memory, check disk
                item = this.GetFromDisk<T>(key);

                if (item == null)
                {
                    return result; // data didn't exist in cache.
                }

                _memCache[key] = item;
            }

            var cacheItem = item as CacheItem<T>;

            result = cacheItem;

            return result;
        }

        #endregion

        #region Methods

        /// <summary>Gets cached data from physical disk</summary>
        /// <typeparam name="T">Data type to return</typeparam>
        /// <param name="key">Key to look for on disk</param>
        /// <returns>The cache item</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", 
            Justification = "The disposal may appear to be happening twice, but it doesn't. Without the 2 using clauses, FXcop also complains about not disposing an object before losing scope.")]
        private CacheItem<T> GetFromDisk<T>(string key)
        {
            string fileName = string.Format(CultureInfo.InvariantCulture, CacheFileFormat, key);

            string fullPath = Path.Combine(_rootPath, fileName);

            if (!File.Exists(fullPath))
            {
                return null; // the file doesn't exist, therefore there's no cache.
            }

            CacheItem<T> result;
            using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var streamReader = new StreamReader(stream))
            {
                string data = streamReader.ReadToEnd();

                result = JsonConvert.DeserializeObject<CacheItem<T>>(data);
            }

            return result;
        }

        /// <summary>Saves the cache item to disk</summary>
        /// <typeparam name="T">Type of data</typeparam>
        /// <param name="key">name of the cache item</param>
        /// <param name="cacheitem">data to cache</param>
        private void SaveToFile<T>(string key, CacheItem<T> cacheitem)
        {
            string stringData = JsonConvert.SerializeObject(cacheitem);

            string fileName = string.Format(CultureInfo.InvariantCulture, CacheFileFormat, key);

            if (!Directory.Exists(_rootPath))
            {
                Directory.CreateDirectory(_rootPath);
            }

            string fullPath = Path.Combine(_rootPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(stringData);
                stream.Write(dataBytes, 0, dataBytes.Length);
                stream.Flush();
            }
        }

        #endregion
    }
}