// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (EveAPI.cs), is part of EveHQ.
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

namespace EveHQ.EveApi
{
    using System;

    using EveHQ.Caching;
    using EveHQ.Common;

    /// <summary>The eve api.</summary>
    public sealed class EveAPI : IDisposable
    {
        /// <summary>The _cache provider.</summary>
        private readonly ICacheProvider _cacheProvider;

        /// <summary>The _request provider.</summary>
        private readonly IHttpRequestProvider _requestProvider;

        /// <summary>The _service location.</summary>
        private readonly string _serviceLocation;

        /// <summary>The _account client.</summary>
        private AccountClient _accountClient;

        /// <summary>The _character client.</summary>
        private CharacterClient _characterClient;

        /// <summary>The _corp client.</summary>
        private CorpClient _corpClient;

        private EveClient _eveClient;

        /// <summary>Initializes a new instance of the <see cref="EveAPI"/> class.</summary>
        /// <param name="dataCacheFolder">The data cache folder.</param>
        /// <param name="requestProvider"></param>
        public EveAPI(string dataCacheFolder, IHttpRequestProvider requestProvider)
            : this(BaseApiClient.DefaultEveWebServiceLocation, new TextFileCacheProvider(dataCacheFolder), requestProvider)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="EveAPI"/> class.</summary>
        /// <param name="apiServiceLocation"></param>
        /// <param name="dataCacheFolder">The data cache folder.</param>
        /// <param name="requestProvider"></param>
        public EveAPI(string apiServiceLocation, string dataCacheFolder, IHttpRequestProvider requestProvider)
            : this(apiServiceLocation, new TextFileCacheProvider(dataCacheFolder), requestProvider)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="EveAPI"/> class.</summary>
        /// <param name="eveWebServiceLocation">The eve web service location.</param>
        /// <param name="cacheProvider">The cache provider.</param>
        /// <param name="requestProvider">The request provider.</param>
        public EveAPI(string eveWebServiceLocation, ICacheProvider cacheProvider, IHttpRequestProvider requestProvider)
        {
            _serviceLocation = eveWebServiceLocation;
            _cacheProvider = cacheProvider;
            _requestProvider = requestProvider;
        }

        /// <summary>Gets a client instance for interacting with the Account related service methods</summary>
        public AccountClient Account
        {
            get
            {
                return _accountClient ?? (_accountClient = new AccountClient(_serviceLocation, _cacheProvider, _requestProvider));
            }
        }

        /// <summary>Gets a client instance for interacting with the Character related service methods</summary>
        public CharacterClient Character
        {
            get
            {
                return _characterClient ?? (_characterClient = new CharacterClient(_serviceLocation, _cacheProvider, _requestProvider));
            }
        }

        /// <summary>Gets a client instance for interacting with the Corporation related service methods</summary>
        public CorpClient Corporation
        {
            get
            {
                return _corpClient ?? (_corpClient = new CorpClient(_serviceLocation, _cacheProvider, _requestProvider));
            }
        }

        public EveClient Eve
        {
            get
            {
                return _eveClient ?? (_eveClient = new EveClient(_serviceLocation, _cacheProvider, _requestProvider));
            }
        }

        public void Dispose()
        {
            if (_accountClient != null)
            {
                _accountClient.Dispose();
            }

            if (_characterClient != null)
            {
                _characterClient.Dispose();
            }

            if (_corpClient != null)
            {
                _corpClient.Dispose();
            }

            if (_eveClient != null)
            {
                _eveClient.Dispose();
            }
        }
    }
}