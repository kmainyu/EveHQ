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
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using EveHQ.Caching;
using EveHQ.Common;
using EveHQ.Common.Extensions;

namespace EveHQ.EveApi
{
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
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The response object.</returns>
        public EveServiceResponse<IEnumerable<CharacterName>> CharacterName(IEnumerable<long> ids,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            Task<EveServiceResponse<IEnumerable<CharacterName>>> task = CharacterNameAsync(ids, responseMode);
            task.Wait();
            return task.Result;
        }

        /// <summary>Converts an array of character Ids into names</summary>
        /// <param name="ids"></param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>An async task reference.</returns>
        public Task<EveServiceResponse<IEnumerable<CharacterName>>> CharacterNameAsync(IEnumerable<long> ids,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(ids != null);

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

            return GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey,
                ApiConstants.SixtyMinuteCache, responseMode, ParseCharacterNameResult);
        }

        /// <summary>The character id.</summary>
        /// <param name="names">The names.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns></returns>
        public EveServiceResponse<IEnumerable<CharacterName>> CharacterId(IEnumerable<string> names,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            Task<EveServiceResponse<IEnumerable<CharacterName>>> task = CharacterIdAsync(names, responseMode);
            task.Wait();
            return task.Result;
        }

        /// <summary>The character id async.</summary>
        /// <param name="names">The names.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The <see cref="Task" />.</returns>
        /// <exception cref="ArgumentException"></exception>
        public Task<EveServiceResponse<IEnumerable<CharacterName>>> CharacterIdAsync(IEnumerable<string> names,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(names != null);

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

            return GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey,
                ApiConstants.SixtyMinuteCache, responseMode, ParseCharacterNameResult);
        }

        /// <summary>The character info async.</summary>
        /// <param name="characterId">The character id.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The <see cref="Task" />.</returns>
        public Task<EveServiceResponse<CharacterInfo>> CharacterInfoAsync(int characterId,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(characterId > 0);
            const string MethodPath = "{0}/CharacterInfo.xml.aspx";
            const string CacheKeyFormat = "Eve_CharacterInfo{0}";

            string cacheKey = CacheKeyFormat.FormatInvariant(characterId);
            IDictionary<string, string> apiParams = new Dictionary<string, string>();
            apiParams.Add(ApiConstants.CharacterId, characterId.ToInvariantString());

            return GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey,
                ApiConstants.SixtyMinuteCache, responseMode, ParseCharacterInfoResult);
        }

        /// <summary>The character info.</summary>
        /// <param name="characterId">The character id.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns></returns>
        public EveServiceResponse<CharacterInfo> CharacterInfo(int characterId,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            Task<EveServiceResponse<CharacterInfo>> task = CharacterInfoAsync(characterId, responseMode);
            task.Wait();
            return task.Result;
        }

        /// <summary>The alliance list.</summary>
        /// <param name="responseMode">The response mode.</param>
        /// <returns></returns>
        public EveServiceResponse<IEnumerable<AllianceData>> AllianceList(
            ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(AllianceListAsync, responseMode);
        }

        /// <summary>The alliance list async.</summary>
        /// <param name="responseMode">The response mode.</param>
        /// <returns>The <see cref="Task" />.</returns>
        public Task<EveServiceResponse<IEnumerable<AllianceData>>> AllianceListAsync(
            ResponseMode responseMode = ResponseMode.Normal)
        {
            const string MethodPath = "{0}/AllianceList.xml.aspx";
            const string CacheKeyFormat = "AllianceList";

            string cacheKey = CacheKeyFormat.FormatInvariant();

            IDictionary<string, string> apiParams = new Dictionary<string, string>();

            return GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey,
                ApiConstants.SixtyMinuteCache, responseMode, ParseAllianceListResponse);
        }

        /// <summary>The ref types.</summary>
        /// <param name="responseMode">The response mode.</param>
        /// <returns></returns>
        public EveServiceResponse<IEnumerable<RefType>> RefTypes(ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(RefTypesAsync, responseMode);
        }

        /// <summary>The ref types async.</summary>
        /// <param name="responseMode">The response mode.</param>
        /// <returns>The <see cref="Task" />.</returns>
        public Task<EveServiceResponse<IEnumerable<RefType>>> RefTypesAsync(
            ResponseMode responseMode = ResponseMode.Normal)
        {
            const string MethodPath = "{0}/RefTypes.xml.aspx";
            const string CacheKeyFormat = "RefTypes";

            string cacheKey = CacheKeyFormat.FormatInvariant();

            IDictionary<string, string> apiParams = new Dictionary<string, string>();

            return GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey,
                ApiConstants.SixtyMinuteCache, responseMode, ParseReferenceTypesResponse);
        }

        /// <summary>The skill tree.</summary>
        /// <param name="responseMode">The response mode.</param>
        /// <returns></returns>
        public EveServiceResponse<IEnumerable<SkillGroup>> SkillTree(ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(SkillTreeAsync, responseMode);
        }

        /// <summary>The skill tree async.</summary>
        /// <param name="responseMode">The response mode.</param>
        /// <returns>The <see cref="Task" />.</returns>
        public Task<EveServiceResponse<IEnumerable<SkillGroup>>> SkillTreeAsync(
            ResponseMode responseMode = ResponseMode.Normal)
        {
            const string MethodPath = "{0}/SkillTree.xml.aspx";
            const string CacheKeyFormat = "SkillTree";

            string cacheKey = CacheKeyFormat.FormatInvariant();

            IDictionary<string, string> apiParams = new Dictionary<string, string>();

            return GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey,
                ApiConstants.SixtyMinuteCache, responseMode, ParseSkillTreeResponse);
        }

        /// <summary>The conquerable station list.</summary>
        /// <param name="responseMode">The response mode.</param>
        /// <returns></returns>
        public EveServiceResponse<IEnumerable<ConquerableStation>> ConquerableStationList(
            ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(ConquerableStationListAsync, responseMode);
        }

        /// <summary>The conquerable station list async.</summary>
        /// <param name="responseMode">The response mode.</param>
        /// <returns>The <see cref="Task" />.</returns>
        public Task<EveServiceResponse<IEnumerable<ConquerableStation>>> ConquerableStationListAsync(
            ResponseMode responseMode = ResponseMode.Normal)
        {
            const string MethodPath = "{0}/ConquerableStationList.xml.aspx";
            const string CacheKeyFormat = "ConquerableStationList";

            string cacheKey = CacheKeyFormat.FormatInvariant();

            IDictionary<string, string> apiParams = new Dictionary<string, string>();

            return GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey,
                ApiConstants.SixtyMinuteCache, responseMode, ParseConquerableStationListResponse);
        }

        #endregion

        #region Methods

        /// <summary>The parse conquerable station list response.</summary>
        /// <param name="result">The result.</param>
        /// <returns>The <see cref="IEnumerable" />.</returns>
        private static IEnumerable<ConquerableStation> ParseConquerableStationListResponse(XElement result)
        {
            if (result == null)
            {
                return null; // return null... no data.
            }

            return from rowSet in result.Elements(ApiConstants.Rowset)
                from row in rowSet.Elements(ApiConstants.Row)
                let id = row.Attribute("stationID").Value.ToInt32()
                let name = row.Attribute("stationName").Value
                let type = row.Attribute("stationTypeID").Value.ToInt32()
                let systemId = row.Attribute("solarSystemID").Value.ToInt32()
                let corpId = row.Attribute("corporationID").Value.ToInt32()
                let corpName = row.Attribute("corporationName").Value
                select
                    new ConquerableStation
                    {
                        CorporationId = corpId,
                        CorporationName = corpName,
                        Id = id,
                        Name = name,
                        SolarSystemId = systemId,
                        StationTypeId = type
                    };
        }

        /// <summary>The parse skill tree response.</summary>
        /// <param name="result">The result.</param>
        /// <returns>The <see cref="IEnumerable" />.</returns>
        private static IEnumerable<SkillGroup> ParseSkillTreeResponse(XElement result)
        {
            if (result == null)
            {
                return null; // return null... no data.
            }

            return from groupRowset in result.Elements(ApiConstants.Rowset)
                from groupRow in groupRowset.Elements(ApiConstants.Row)
                let groupId = groupRow.Attribute("groupID").Value.ToInt32()
                let groupName = groupRow.Attribute("groupName").Value
                select new SkillGroup {GroupID = groupId, GroupName = groupName, Skills = GetGroupSkills(groupRow)};
        }

        /// <summary>The get group skills.</summary>
        /// <param name="groupRow">The group row.</param>
        /// <returns>The <see cref="IEnumerable" />.</returns>
        private static IEnumerable<SkillData> GetGroupSkills(XElement groupRow)
        {
            return from rowSet in groupRow.Elements(ApiConstants.Rowset)
                from row in rowSet.Elements(ApiConstants.Row)
                let groupId = row.Attribute("groupID").Value.ToInt32()
                let published = row.Attribute("published").Value.ToBoolean()
                let typeId = row.Attribute("typeID").Value.ToInt32()
                let name = row.Attribute("typeName").Value
                let desc = row.Element("description").Value
                let rank = row.Element("rank").Value.ToInt32()
                let reqAttrib = row.Element("requiredAttributes")
                let priAttrib = reqAttrib.Element("primaryAttribute").Value
                let secAttrib = reqAttrib.Element("secondaryAttribute").Value
                let reqSkills =
                    row.Elements(ApiConstants.Rowset)
                        .Where(e => e.Attribute("name").Value == "requiredSkills")
                        .Elements(ApiConstants.Row)
                        .Select(
                            r =>
                                new ReqSkillData
                                {
                                    Level = r.Attribute("skillLevel").Value.ToInt32(),
                                    TypeId = r.Attribute("typeID").Value.ToInt32()
                                })
                let trialTrainElement =
                    row.Elements(ApiConstants.Rowset)
                        .Where(e => e.Attribute("name").Value == "skillBonusCollection")
                        .Elements(ApiConstants.Row)
                        .FirstOrDefault(r => r.Attribute("bonusType").Value == "canNotBeTrainedOnTrial")
                let cannotBeTrainedOnTrial =
                    trialTrainElement != null && trialTrainElement.Attribute("bonusValue").Value.ToBoolean()
                select
                    new SkillData
                    {
                        CannotBeTrainedOnTrial = cannotBeTrainedOnTrial,
                        Description = desc,
                        GroupId = groupId,
                        Name = name,
                        PrimaryAttribute = priAttrib,
                        Published = published,
                        Rank = rank,
                        RequiredSkills = reqSkills,
                        SecondaryAttribute = secAttrib,
                        TypeId = typeId
                    };
        }

        /// <summary>The parse reference types response.</summary>
        /// <param name="result">The result.</param>
        /// <returns>The <see cref="IEnumerable" />.</returns>
        private static IEnumerable<RefType> ParseReferenceTypesResponse(XElement result)
        {
            if (result == null)
            {
                return null; // return null... no data.
            }

            return from rowset in result.Elements(ApiConstants.Rowset)
                from row in rowset.Elements(ApiConstants.Row)
                let name = row.Attribute("refTypeName").Value
                let id = row.Attribute("refTypeID").Value.ToInt32()
                select new RefType {Id = id, Name = name};
        }

        /// <summary>The parse alliance list response.</summary>
        /// <param name="result">The result.</param>
        /// <returns>The <see cref="IEnumerable" />.</returns>
        private static IEnumerable<AllianceData> ParseAllianceListResponse(XElement result)
        {
            if (result == null)
            {
                return null; // return null... no data.
            }

            return from rowset in result.Elements(ApiConstants.Rowset)
                from row in rowset.Elements(ApiConstants.Row)
                let name = row.Attribute(ApiConstants.Name).Value
                let ticker = row.Attribute("shortName").Value
                let allianceId = row.Attribute("allianceID").Value.ToInt32()
                let execCorp = row.Attribute("executorCorpID").Value.ToInt32()
                let memberCount = row.Attribute("memberCount").Value.ToInt32()
                let startDate = row.Attribute("startDate").Value.ToDateTimeOffset(0)
                let corps = GetCorpData(row.Element(ApiConstants.Rowset))
                select
                    new AllianceData
                    {
                        Id = allianceId,
                        Name = name,
                        ShortName = ticker,
                        ExecutorCorpId = execCorp,
                        MemberCorps = corps,
                        MemberCount = memberCount,
                        StartDate = startDate
                    };
        }

        /// <summary>The get corp data.</summary>
        /// <param name="memberRowSet">The member row set.</param>
        /// <returns>The <see cref="IEnumerable" />.</returns>
        private static IEnumerable<AllianceCorpData> GetCorpData(XElement memberRowSet)
        {
            if (memberRowSet == null)
            {
                return null; // return null... no data.
            }

            return from row in memberRowSet.Elements(ApiConstants.Row)
                let corpId = row.Attribute(ApiConstants.CorporationId).Value.ToInt32()
                let joinDate = row.Attribute("startDate").Value.ToDateTimeOffset(0)
                select new AllianceCorpData {CorporationId = corpId, JoinedDate = joinDate};
        }

        /// <summary>The parse character info result.</summary>
        /// <param name="results">The results.</param>
        /// <returns>The <see cref="CharacterInfo" />.</returns>
        private static CharacterInfo ParseCharacterInfoResult(XElement results)
        {
            if (results == null)
            {
                return null; // return null... no data.
            }

            var info = new CharacterInfo();

            info.AllianceId = results.Element("allianceID").Value.ToInt64();
            info.AllianceInDate = results.Element("alliancenDate").Value.ToDateTimeOffset(0);
            info.AllianceName = results.Element("alliance").Value;
            info.Bloodline = results.Element("bloodline").Value;
            info.CharacterId = results.Element("characterID").Value.ToInt64();
            info.CharacterName = results.Element("characterName").Value;
            info.CorporationId = results.Element("corporationID").Value.ToInt64();
            info.CorporationName = results.Element("corporation").Value;
            info.Race = results.Element("race").Value;
            info.SecurityStatus = results.Element("securityStatus").Value.ToDouble();

            return info;
        }

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
                select new CharacterName {Id = id, Name = name};
        }

        #endregion
    }
}