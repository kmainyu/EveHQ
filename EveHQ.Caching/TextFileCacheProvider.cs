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

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using EveHQ.Common.Extensions;
using Newtonsoft.Json;

namespace EveHQ.Caching
{
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
            var cacheitem = new CacheItem<T> {CacheUntil = cacheUntil, Data = value};

            _memCache[key] = cacheitem;
            SaveToFile(key, cacheitem);
        }

        /// <summary>Gets an item from cache.</summary>
        /// <param name="key">The key.</param>
        /// <typeparam name="T">Tye to get from cache</typeparam>
        /// <returns>The <see cref="CacheItem" />.</returns>
        public CacheItem<T> Get<T>(string key)
        {
            CacheItem<T> result = null;

            object item;

            if (!_memCache.TryGetValue(key, out item))
            {
                // not in memory, check disk
                item = GetFromDisk<T>(key);

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
            Justification =
                "The disposal may appear to be happening twice, but it doesn't. Without the 2 using clauses, FXcop also complains about not disposing an object before losing scope."
            )]
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

            try
            {
                using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(stringData);
                    stream.Write(dataBytes, 0, dataBytes.Length);
                    stream.Flush();
                }
            }
            catch (Exception e)
            {
                // supress the exception since we have the data in memory, but log the occurance
                Trace.TraceWarning(e.FormatException());
            }
        }

        #endregion
    }
}