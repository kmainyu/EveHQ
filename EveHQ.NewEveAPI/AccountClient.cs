//  ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (AccountClient.cs), is part of EveHQ.
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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using EveHQ.Caching;
    using EveHQ.Common;
    using EveHQ.Common.Extensions;

    /// <summary>
    /// Client class for interacting with the Account API methods of the EVE Web Service.
    /// </summary>
    public sealed class AccountClient : BaseApiClient
    {
        /// <summary>
        /// Account status cache key
        /// </summary>
        private const string AccountStatusCacheKeyFormat = "AccountStatus_{0}";

        /// <summary>
        /// Path to Account status method.
        /// </summary>
        private const string AccountStatusPath = "/account/AccountStatus.xml.aspx";

        /// <summary>
        /// Format for cache file name .
        /// </summary>
        private const string ApiKeyInfoCacheKeyFormat = "ApiKeyInfo_{0}";

        /// <summary>
        /// Path to ApiKeyInfo method
        /// </summary>
        private const string ApiKeyInfoPath = "/account/APIKeyInfo.xml.aspx";

        /// <summary>
        /// Format for the cache key used for the characters data.
        /// </summary>
        private const string CharacterCacheKeyFormat = "Characters_{0}";

        /// <summary>
        /// Path to the Characters method on the web service
        /// </summary>
        private const string CharactersPath = "/account/Characters.xml.aspx";

        /// <summary>
        /// Initializes a new instance of the AccountClient class.
        /// </summary>
        /// <param name="eveServiceLocation">location of the eve API web service</param>
        /// <param name="cacheProvider">root folder used for caching.</param>
        /// <param name="requestProvider">Request provider to use for this instance.</param>
        internal AccountClient(string eveServiceLocation, ICacheProvider cacheProvider, IHttpRequestProvider requestProvider)
            : base(eveServiceLocation, cacheProvider, requestProvider)
        {
        }


        public EveServiceResponse<Account> AccountStatus(string keyId, string vCode)
        {
            System.Diagnostics.Contracts.Contract.Requires(!keyId.IsNullOrWhiteSpace());
            System.Diagnostics.Contracts.Contract.Requires(!vCode.IsNullOrWhiteSpace());
            
            var task = AccountStatusAsync(keyId, vCode);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Gets the status of an account user. 
        /// </summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <returns>An account object</returns>
        public Task<EveServiceResponse<Account>> AccountStatusAsync(string keyId, string vCode)
        {
            System.Diagnostics.Contracts.Contract.Requires(!keyId.IsNullOrWhiteSpace());
            System.Diagnostics.Contracts.Contract.Requires(!vCode.IsNullOrWhiteSpace());

            string cacheKey = AccountStatusCacheKeyFormat.FormatInvariant(keyId);

            return this.GetServiceResponseAsync(keyId, vCode, 0, AccountStatusPath, null, cacheKey, ApiConstants.SixtyMinuteCache, ParseAccountXml);
        }

        public EveServiceResponse<ApiKeyInfo> ApiKeyInfo(string keyId, string vCode)
        {
            var task = ApiKeyInfoAsync(keyId, vCode);
            task.Wait();
            return task.Result;
        }

        public Task<EveServiceResponse<ApiKeyInfo>> ApiKeyInfoAsync(string keyId, string vCode)
        {
            System.Diagnostics.Contracts.Contract.Requires(!keyId.IsNullOrWhiteSpace());
            System.Diagnostics.Contracts.Contract.Requires(!vCode.IsNullOrWhiteSpace());

            string cacheKey = ApiKeyInfoCacheKeyFormat.FormatInvariant(keyId);

            return this.GetServiceResponseAsync(keyId, vCode, 0, ApiKeyInfoPath, null, cacheKey, ApiConstants.FiveMinuteCache, ParseApiKeyInfoXml);
        }

        /// <summary>
        /// Gets the list of characters on the given account.
        /// </summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <returns>A Service Response object, containing the collection of Characters.</returns>
        public EveServiceResponse<IEnumerable<AccountCharacter>> Characters(string keyId, string vCode)
        {
            var task = CharactersAsync(keyId, vCode);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Gets the list of characters on the given account.
        /// </summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <returns>A Service Response object, containing the collection of Characters.</returns>
        public Task<EveServiceResponse<IEnumerable<AccountCharacter>>> CharactersAsync(string keyId, string vCode)
        {
            System.Diagnostics.Contracts.Contract.Requires(!keyId.IsNullOrWhiteSpace());
            System.Diagnostics.Contracts.Contract.Requires(!vCode.IsNullOrWhiteSpace());

            string cacheKey = CharacterCacheKeyFormat.FormatInvariant(keyId);

            return this.GetServiceResponseAsync(keyId, vCode, 0, CharactersPath, null, cacheKey, ApiConstants.SixtyMinuteCache, ParseCharactersXml);
        }

        /// <summary>
        /// Gets a character collection from the rowset.
        /// </summary>
        /// <param name="rowset"></param>
        /// <returns></returns>
        private static IEnumerable<AccountCharacter> GetCharactersFromRowSet(XElement rowset)
        {
            return rowset.Elements().Select(
                row =>
                {
                    string name = (row.Attribute(ApiConstants.Name) ?? row.Attribute(ApiConstants.CharacterName)).Value;
                    // because there are more than 1 way to define a character name in a result set.
                    int characterId = row.Attribute(ApiConstants.CharacterId).Value.ToInt32();
                    string corpName = row.Attribute(ApiConstants.CorporationName).Value;
                    int corpid = row.Attribute(ApiConstants.CorporationId).Value.ToInt32();
                    return new AccountCharacter() { CharacterId = characterId, CorporationId = corpid, CorporationName = corpName, Name = name };

                });
        }

        /// <summary>
        /// Parses the account xml
        /// </summary>
        /// <param name="results">root node of the results</param>
        /// <returns>An account object</returns>
        private static Account ParseAccountXml(XElement results)
        {
            DateTimeOffset expiry = default(DateTimeOffset);
            DateTimeOffset created = default(DateTimeOffset), tempDate;
            TimeSpan loggedinMinutes = TimeSpan.Zero;
            int userId = 0, logonCount = 0, tempNumber;

            // the order of the children is not guaranteed, so we need to detect the ordering.
            foreach (XElement children in results.Elements())
            {
                switch (children.Name.LocalName)
                {
                    case ApiConstants.UserId:
                        userId = int.TryParse(children.Value, out tempNumber) ? tempNumber : 0;
                        break;
                    case ApiConstants.PaidUntil:
                        expiry = DateTimeOffset.TryParse(children.Value, out tempDate) ? new DateTimeOffset(tempDate.DateTime, TimeSpan.FromHours(0)) : default(DateTimeOffset);
                        break;
                    case ApiConstants.CreateDate:
                        created = DateTimeOffset.TryParse(children.Value, out tempDate) ? new DateTimeOffset(tempDate.DateTime, TimeSpan.FromHours(0)) : default(DateTimeOffset);
                        break;
                    case ApiConstants.LogonCount:
                        logonCount = int.TryParse(children.Value, out tempNumber) ? tempNumber : 0;
                        break;
                    case ApiConstants.LogonMinutes:
                        loggedinMinutes = int.TryParse(children.Value, out tempNumber) ? TimeSpan.FromMinutes(tempNumber) : TimeSpan.Zero;
                        break;
                }
            }

            return new Account() { UserId = userId, ExpiryDate = expiry, CreateDate = created, LogOnCount = logonCount, LoggedInTime = loggedinMinutes };
        }

        /// <summary>
        /// Parses the api key information from xml
        /// </summary>
        /// <param name="result">xml data node</param>
        /// <returns>api key information</returns>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "System.Enum.TryParse<EveHQ.EveApi.ApiKeyType>(System.String,EveHQ.EveApi.ApiKeyType@)",
            Justification = "the return of the method isn't important in this usage.")]
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "System.Int32.TryParse(System.String,System.Int32@)", Justification = "the return of the method isn't important in this usage."
            )]
        private static ApiKeyInfo ParseApiKeyInfoXml(XElement result)
        {
            DateTimeOffset expiry = default(DateTimeOffset);
            int accessMask = 0;
            var type = ApiKeyType.Invalid;
            ApiKeyInfo keyInfo = null;
            IEnumerable<AccountCharacter> chars = new AccountCharacter[0];

            // get the key element in the result
            XElement keyElement = result.Element(ApiConstants.Key);
            if (keyElement != null)
            {
                // get details from the attributes
                foreach (XAttribute attribute in keyElement.Attributes())
                {
                    switch (attribute.Name.LocalName)
                    {
                        case "accessMask":
                            int.TryParse(attribute.Value, out accessMask);
                            break;
                        case "type":
                            Enum.TryParse(attribute.Value, out type);
                            break;
                        case "expires":
                            DateTime temp;
                            expiry = DateTime.TryParse(attribute.Value, out temp) ? new DateTimeOffset(temp, TimeSpan.Zero) : default(DateTimeOffset);
                            break;
                    }
                }

                // get the character rowset that might be present in the result
                XElement characterSet = keyElement.Element(ApiConstants.Rowset);
                if (characterSet != null)
                {
                    chars = GetCharactersFromRowSet(characterSet);
                }

                keyInfo = new ApiKeyInfo() { AccessMask = accessMask, ApiType = type, Expires = expiry, Characters = chars };
            }

            return keyInfo;
        }

        /// <summary>
        /// Parses the character data set from the web service.
        /// </summary>
        /// <param name="results">root of the results xml.</param>
        /// <returns>A collection of Characters.</returns>
        private static IEnumerable<AccountCharacter> ParseCharactersXml(XElement results)
        {
            XElement rowset = results.Element(ApiConstants.Rowset);

            if (rowset == null)
            {
                return new AccountCharacter[0]; // return empty collection
            }

            return GetCharactersFromRowSet(rowset);
        }
    }
}