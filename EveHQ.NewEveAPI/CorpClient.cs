// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (CorpClient.cs), is part of EveHQ.
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
    using System.Diagnostics.CodeAnalysis;

    using EveHQ.Caching;
    using EveHQ.Common;

    /// <summary>The corp client.</summary>
    public sealed class CorpClient : CorpCharBaseClient
    {
        /// <summary>The request prefix.</summary>
        private const string RequestPrefix = "/corp";

        /// <summary>Initializes a new instance of the <see cref="CorpClient"/> class. Initializes a new instance of the CharacterClient class.</summary>
        /// <param name="eveServiceLocation">location of the eve API web service</param>
        /// <param name="cacheProvider">root folder used for caching.</param>
        /// <param name="requestProvider">Request provider to use for this instance.</param>
        internal CorpClient(string eveServiceLocation, ICacheProvider cacheProvider, IHttpRequestProvider requestProvider)
            : base(eveServiceLocation, cacheProvider, requestProvider, RequestPrefix)
        {
        }
    }
}