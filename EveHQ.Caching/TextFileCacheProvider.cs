// -----------------------------------------------------------------------
// <copyright file="TextFileCache.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EveHQ.Caching
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Newtonsoft.Json;

    /// <summary>
    /// A simple caching system that uses text files for persistence.
    /// </summary>
    public class TextFileCacheProvider:ICacheProvider
    {
         /// <summary>
        /// Cache file naming format.
        /// </summary>
        private const string CacheFileFormat = "{0}.json.txt";

        /// <summary>
        /// In memory copy of cache
        /// </summary>
        private readonly ConcurrentDictionary<string, object> _memCache = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// root path of the current cache instance.
        /// </summary>
        private readonly string _rootPath;

        /// <summary>
        /// Initializes a new instance of the SimpleTextFileCache class.
        /// </summary>
        /// <param name="rootPath">Root path of the cache instance.</param>
        public TextFileCacheProvider(string rootPath)
        {
            _rootPath = rootPath;
        }

        /// <summary>
        /// Adds an item to the cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheUntil"></param>
        public void Add<T>(string key, T value, DateTimeOffset cacheUntil)
        {
            var cacheitem = new CacheItem<T> { CacheUntil = cacheUntil, Data = value };

            _memCache[key] = cacheitem;
            SaveToFile(key, cacheitem); ;
        }

        /// <summary>
        /// Gets an item from cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets cached data from physical disk
        /// </summary>
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

        /// <summary>
        /// Saves the cache item to disk
        /// </summary>
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
    }
}
