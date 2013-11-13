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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using EveHQ.Caching;
    using EveHQ.Common;
    using EveHQ.Common.Extensions;

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


        public EveServiceResponse<CorporateData> CorporationSheet(string keyId, string vCode, int corpId = 0)
        {
            return RunAsyncMethod(CorporationSheetAsync, keyId, vCode, corpId);
        }

        public Task<EveServiceResponse<CorporateData>> CorporationSheetAsync(string keyId, string vCode, int corpId = 0)
        {
            System.Diagnostics.Contracts.Contract.Requires(!keyId.IsNullOrWhiteSpace());
            System.Diagnostics.Contracts.Contract.Requires(!vCode.IsNullOrWhiteSpace());
            System.Diagnostics.Contracts.Contract.Requires(corpId > 0);

            const string MethodPath = "{0}/CorporationSheet.xml.aspx";
            const string CacheKeyFormat = "CorporationSheet{0}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, corpId > 0 ? "_{0}".FormatInvariant(corpId) : string.Empty);

              IDictionary<string, string> apiParams = new Dictionary<string, string>();
            if (corpId > 0)
            {
                apiParams["corporationID"] = corpId.ToInvariantString();
            }

            return GetServiceResponseAsync(keyId, vCode, 0, MethodPath.FormatInvariant(PathPrefix), apiParams, cacheKey, ApiConstants.SixtyMinuteCache, ProcessCorporationSheetResponse);
        }


        private static CorporateData ProcessCorporationSheetResponse(XElement results)
        {
            if (results == null)
            {
                return null; // return null... no data.
            }

            CorporateData data = new CorporateData();


            data.CorporationId = results.Element("corporationID").Value.ToInt32();
            data.CorporationName = results.Element("corporationName").Value;
            data.Ticker = results.Element("ticker").Value;
            data.CeoId = results.Element("ceoID").Value.ToInt32();
            data.CeoName = results.Element("ceoName").Value;
            data.StationId = results.Element("stationID").Value.ToInt32();
            data.StationName = results.Element("stationName").Value;
            data.Description = results.Element("description").Value;
            data.Url = results.Element("url").Value;
            data.AllianceId = results.Element("allianceID").Value.ToInt32();
            data.AllianceName = results.Element("allianceName").Value;
            data.TaxRate = results.Element("taxRate").Value;
            data.MemberCount = results.Element("memberCount").Value.ToInt32();
            data.MemberLimit = results.Element("memberLimit").Value.ToInt32();
            data.Shares = results.Element("shares").Value.ToInt32();
            data.Divisions = from rowsets in results.Elements("rowset")
                             from rows in rowsets.Descendants()
                             where rowsets.Attribute("name").Value == "divisons"
                             select new CorporateDivision() { AccountKey = rows.Attribute("accountKey").Value.ToInt32(), Description = rows.Attribute("description").Value };
            data.WalletDivisions = from rowsets in results.Elements("rowset")
                             from rows in rowsets.Descendants()
                                   where rowsets.Attribute("name").Value == "walletDivisions"
                             select new CorporateDivision() { AccountKey = rows.Attribute("accountKey").Value.ToInt32(), Description = rows.Attribute("description").Value };

            var logoXml = results.Element("logo");
            var logo = new CorporateLogo()
                           {
                               GraphicId = logoXml.Element("graphicID").Value.ToInt32(),
                               Color1 = logoXml.Element("color1").Value.ToInt32(),
                               Color2 = logoXml.Element("color2").Value.ToInt32(),
                               Color3 = logoXml.Element("color3").Value.ToInt32(),
                               Shape1 = logoXml.Element("color1").Value.ToInt32(),
                               Shape2 = logoXml.Element("shape2").Value.ToInt32(),
                               Shape3 = logoXml.Element("shape").Value.ToInt32()
                           };
            data.Logo = logo;


            return data;
        }
    }
}