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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using EveHQ.Caching;
using EveHQ.Common;
using EveHQ.Common.Extensions;

namespace EveHQ.EveApi
{
    /// <summary>
    ///     Client class for interacting with the Account API methods of the EVE Web Service.
    /// </summary>
    public sealed class AccountClient : BaseApiClient
    {
        /// <summary>
        ///     Account status cache key
        /// </summary>
        private const string AccountStatusCacheKeyFormat = "AccountStatus_{0}";

        /// <summary>
        ///     Path to Account status method.
        /// </summary>
        private const string AccountStatusPath = "/account/AccountStatus.xml.aspx";

        /// <summary>
        ///     Format for cache file name .
        /// </summary>
        private const string ApiKeyInfoCacheKeyFormat = "ApiKeyInfo_{0}";

        /// <summary>
        ///     Path to ApiKeyInfo method
        /// </summary>
        private const string ApiKeyInfoPath = "/account/APIKeyInfo.xml.aspx";

        /// <summary>
        ///     Format for the cache key used for the characters data.
        /// </summary>
        private const string CharacterCacheKeyFormat = "Characters_{0}";

        /// <summary>
        ///     Path to the Characters method on the web service
        /// </summary>
        private const string CharactersPath = "/account/Characters.xml.aspx";

        /// <summary>
        ///     Initializes a new instance of the AccountClient class.
        /// </summary>
        /// <param name="eveServiceLocation">location of the eve API web service</param>
        /// <param name="cacheProvider">root folder used for caching.</param>
        /// <param name="requestProvider">Request provider to use for this instance.</param>
        internal AccountClient(string eveServiceLocation, ICacheProvider cacheProvider,
            IHttpRequestProvider requestProvider)
            : base(eveServiceLocation, cacheProvider, requestProvider)
        {
        }


        public EveServiceResponse<Account> AccountStatus(string keyId, string vCode,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());

            return RunAsyncMethod(AccountStatusAsync, keyId, vCode, responseMode);
        }

        /// <summary>
        ///     Gets the status of an account user.
        /// </summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="responseMode"></param>
        /// <returns>An account object</returns>
        public Task<EveServiceResponse<Account>> AccountStatusAsync(string keyId, string vCode,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());

            string cacheKey = AccountStatusCacheKeyFormat.FormatInvariant(keyId);

            return GetServiceResponseAsync(keyId, vCode, 0, AccountStatusPath, null, cacheKey,
                ApiConstants.SixtyMinuteCache, responseMode, ParseAccountXml);
        }

        public EveServiceResponse<ApiKeyInfo> ApiKeyInfo(string keyId, string vCode,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            return RunAsyncMethod(ApiKeyInfoAsync, keyId, vCode, responseMode);
        }

        public Task<EveServiceResponse<ApiKeyInfo>> ApiKeyInfoAsync(string keyId, string vCode,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());

            string cacheKey = ApiKeyInfoCacheKeyFormat.FormatInvariant(keyId);

            return GetServiceResponseAsync(keyId, vCode, 0, ApiKeyInfoPath, null, cacheKey, ApiConstants.FiveMinuteCache,
                responseMode, ParseApiKeyInfoXml);
        }

        /// <summary>
        ///     Gets the list of characters on the given account.
        /// </summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="responseMode"></param>
        /// <returns>A Service Response object, containing the collection of Characters.</returns>
        public EveServiceResponse<IEnumerable<AccountCharacter>> Characters(string keyId, string vCode,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            return RunAsyncMethod(CharactersAsync, keyId, vCode, responseMode);
        }

        /// <summary>
        ///     Gets the list of characters on the given account.
        /// </summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="responseMode"></param>
        /// <returns>A Service Response object, containing the collection of Characters.</returns>
        public Task<EveServiceResponse<IEnumerable<AccountCharacter>>> CharactersAsync(string keyId, string vCode,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());

            string cacheKey = CharacterCacheKeyFormat.FormatInvariant(keyId);

            return GetServiceResponseAsync(keyId, vCode, 0, CharactersPath, null, cacheKey,
                ApiConstants.SixtyMinuteCache, responseMode, ParseCharactersXml);
        }

        /// <summary>
        ///     Gets a character collection from the rowset.
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
                    return new AccountCharacter
                    {
                        CharacterId = characterId,
                        CorporationId = corpid,
                        CorporationName = corpName,
                        Name = name
                    };
                });
        }

        /// <summary>
        ///     Parses the account xml
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
                        expiry = DateTimeOffset.TryParse(children.Value, out tempDate)
                            ? new DateTimeOffset(tempDate.DateTime, TimeSpan.FromHours(0))
                            : default(DateTimeOffset);
                        break;
                    case ApiConstants.CreateDate:
                        created = DateTimeOffset.TryParse(children.Value, out tempDate)
                            ? new DateTimeOffset(tempDate.DateTime, TimeSpan.FromHours(0))
                            : default(DateTimeOffset);
                        break;
                    case ApiConstants.LogonCount:
                        logonCount = int.TryParse(children.Value, out tempNumber) ? tempNumber : 0;
                        break;
                    case ApiConstants.LogonMinutes:
                        loggedinMinutes = int.TryParse(children.Value, out tempNumber)
                            ? TimeSpan.FromMinutes(tempNumber)
                            : TimeSpan.Zero;
                        break;
                }
            }

            return new Account
            {
                UserId = userId,
                ExpiryDate = expiry,
                CreateDate = created,
                LogOnCount = logonCount,
                LoggedInTime = loggedinMinutes
            };
        }

        /// <summary>
        ///     Parses the api key information from xml
        /// </summary>
        /// <param name="result">xml data node</param>
        /// <returns>api key information</returns>
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults",
            MessageId = "System.Enum.TryParse<EveHQ.EveApi.ApiKeyType>(System.String,EveHQ.EveApi.ApiKeyType@)",
            Justification = "the return of the method isn't important in this usage.")]
        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults",
            MessageId = "System.Int32.TryParse(System.String,System.Int32@)",
            Justification = "the return of the method isn't important in this usage."
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
                            expiry = DateTime.TryParse(attribute.Value, out temp)
                                ? new DateTimeOffset(temp, TimeSpan.Zero)
                                : default(DateTimeOffset);
                            break;
                    }
                }

                // get the character rowset that might be present in the result
                XElement characterSet = keyElement.Element(ApiConstants.Rowset);
                if (characterSet != null)
                {
                    chars = GetCharactersFromRowSet(characterSet);
                }

                keyInfo = new ApiKeyInfo {AccessMask = accessMask, ApiType = type, Expires = expiry, Characters = chars};
            }

            return keyInfo;
        }

        /// <summary>
        ///     Parses the character data set from the web service.
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