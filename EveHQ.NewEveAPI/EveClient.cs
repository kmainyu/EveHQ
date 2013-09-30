// ===========================================================================
// <copyright file="EveClient.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (EveClient.cs), is part of EveHQ.
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
namespace EveHQ.EveApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using EveHQ.Caching;
    using EveHQ.Common;
    using EveHQ.Common.Extensions;

    /// <summary>
    ///     Client for the general Eve information APIs
    /// </summary>
    public sealed class EveClient : BaseApiClient
    {
        #region Constants

        /// <summary>The request prefix.</summary>
        private const string RequestPrefix = "/eve";

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the EveClient class.</summary>
        /// <param name="eveServiceLocation">location of the eve API web service</param>
        /// <param name="cacheProvider">root folder used for caching.</param>
        /// <param name="requestProvider">Request provider to use for this instance.</param>
        internal EveClient(string eveServiceLocation, ICacheProvider cacheProvider, IHttpRequestProvider requestProvider)
            : base(eveServiceLocation, cacheProvider, requestProvider)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Converts an array of character Ids into names</summary>
        /// <param name="ids"></param>
        /// <returns>The response object.</returns>
        public EveServiceResponse<IEnumerable<CharacterName>> CharacterName(IEnumerable<long> ids)
        {
            Task<EveServiceResponse<IEnumerable<CharacterName>>> task = CharacterNameAsync(ids);
            task.Wait();
            return task.Result;
        }

        /// <summary>Converts an array of character Ids into names</summary>
        /// <param name="ids"></param>
        /// <returns>An async task reference.</returns>
        public Task<EveServiceResponse<IEnumerable<CharacterName>>> CharacterNameAsync(IEnumerable<long> ids)
        {
            System.Diagnostics.Contracts.Contract.Requires(ids != null);

            List<long> checkedIds = ids.Distinct().ToList(); // remove duplicates

            if (checkedIds.Count > 250 || checkedIds.Count < 1)
            {
                throw new ArgumentException("ids must have a length of 250 or less and greater than 0.");
            }

            const string MethodPath = "{0}/CharacterName.xml.aspx";
            const string CacheKeyFormat = "Eve_CharacterName{0}";

            string cacheKey = CacheKeyFormat.FormatInvariant(checkedIds.GetHashCode());

            IDictionary<string, string> apiParams = new Dictionary<string, string>();
            apiParams.Add(ApiConstants.Ids, string.Join(",", checkedIds.Select(id => id.ToInvariantString())));

            return this.GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey, ApiConstants.SixtyMinuteCache, ParseCharacterNameResult);
        }


        public EveServiceResponse<IEnumerable<CharacterName>> CharacterId(IEnumerable<string> names)
        {
            Task<EveServiceResponse<IEnumerable<CharacterName>>> task = CharacterIdAsync(names);
            task.Wait();
            return task.Result;
        }

        public Task<EveServiceResponse<IEnumerable<CharacterName>>> CharacterIdAsync(IEnumerable<string> names)
        {
            System.Diagnostics.Contracts.Contract.Requires(names != null);

            List<string> checkedNames = names.Distinct().ToList(); // remove duplicates

            if (checkedNames.Count > 250 || checkedNames.Count < 1)
            {
                throw new ArgumentException("names must have a length of 250 or less and greater than 0.");
            }

            const string MethodPath = "{0}/CharacterID.xml.aspx";
            const string CacheKeyFormat = "Eve_CharacterID{0}";
            const string paramName = "names";
            string cacheKey = CacheKeyFormat.FormatInvariant(checkedNames.GetHashCode());

            IDictionary<string, string> apiParams = new Dictionary<string, string>();
            apiParams.Add(paramName, string.Join(",", checkedNames.Select(name => name)));

            return this.GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey, ApiConstants.SixtyMinuteCache, ParseCharacterNameResult);
        }



        #endregion

        #region Methods

        /// <summary>Parses the xml results for Character names.</summary>
        /// <param name="results"></param>
        /// <returns>A collection of character name objects.</returns>
        private static IEnumerable<CharacterName> ParseCharacterNameResult(XElement results)
        {
            if (results == null)
            {
                return null; // return null... no data.
            }

            return from rowset in results.Elements(ApiConstants.Rowset)
                    from row in rowset.Elements(ApiConstants.Row)
                    let name = row.Attribute("name").Value
                    let id = row.Attribute("characterID").Value.ToInt64()
                    select new CharacterName() { Id = id, Name = name };
        }

        #endregion
    }
}