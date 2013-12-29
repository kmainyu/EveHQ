// ===========================================================================
// <copyright file="EveClient.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2013  EveHQ Development Team
//  This file (EveClient.cs), is part of EveHQ.
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
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

            return GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey, ApiConstants.SixtyMinuteCache, ParseCharacterNameResult);
        }

        /// <summary>The character id.</summary>
        /// <param name="names">The names.</param>
        public EveServiceResponse<IEnumerable<CharacterName>> CharacterId(IEnumerable<string> names)
        {
            Task<EveServiceResponse<IEnumerable<CharacterName>>> task = CharacterIdAsync(names);
            task.Wait();
            return task.Result;
        }

        /// <summary>The character id async.</summary>
        /// <param name="names">The names.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="ArgumentException"></exception>
        public Task<EveServiceResponse<IEnumerable<CharacterName>>> CharacterIdAsync(IEnumerable<string> names)
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

            return GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey, ApiConstants.SixtyMinuteCache, ParseCharacterNameResult);
        }

        /// <summary>The character info async.</summary>
        /// <param name="characterId">The character id.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task<EveServiceResponse<CharacterInfo>> CharacterInfoAsync(int characterId)
        {
            Guard.Ensure(characterId > 0);
            const string MethodPath = "{0}/CharacterInfo.xml.aspx";
            const string CacheKeyFormat = "Eve_CharacterInfo{0}";

            string cacheKey = CacheKeyFormat.FormatInvariant(characterId);
            IDictionary<string, string> apiParams = new Dictionary<string, string>();
            apiParams.Add(ApiConstants.CharacterId, characterId.ToInvariantString());

            return GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey, ApiConstants.SixtyMinuteCache, ParseCharacterInfoResult);
        }

        /// <summary>The character info.</summary>
        /// <param name="characterId">The character id.</param>
        public EveServiceResponse<CharacterInfo> CharacterInfo(int characterId)
        {
            Task<EveServiceResponse<CharacterInfo>> task = CharacterInfoAsync(characterId);
            task.Wait();
            return task.Result;
        }


        public EveServiceResponse<IEnumerable<AllianceData>> AllianceList()
        {
            return RunAsyncMethod(AllianceListAsync);
        }


        public Task<EveServiceResponse<IEnumerable<AllianceData>>> AllianceListAsync()
        {

            const string MethodPath = "{0}/AllianceList.xml.aspx";
            const string CacheKeyFormat = "AllianceList";

            string cacheKey = CacheKeyFormat.FormatInvariant();

            IDictionary<string, string> apiParams = new Dictionary<string, string>();

            return GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey, ApiConstants.SixtyMinuteCache, ParseAllianceListResponse);
        }

        public EveServiceResponse<IEnumerable<RefType>> RefTypes()
        {
            return RunAsyncMethod(RefTypesAsync);
        }

        public Task<EveServiceResponse<IEnumerable<RefType>>> RefTypesAsync()
        {

            const string MethodPath = "{0}/RefTypes.xml.aspx";
            const string CacheKeyFormat = "RefTypes";

            string cacheKey = CacheKeyFormat.FormatInvariant();

            IDictionary<string, string> apiParams = new Dictionary<string, string>();

            return GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey, ApiConstants.SixtyMinuteCache, ParseReferenceTypesResponse);
        }

        public EveServiceResponse<IEnumerable<SkillGroup>> SkillTree()
        {
            return RunAsyncMethod(SkillTreeAsync);
        }

        public Task<EveServiceResponse<IEnumerable<SkillGroup>>> SkillTreeAsync()
        {
            const string MethodPath = "{0}/SkillTree.xml.aspx";
            const string CacheKeyFormat = "SkillTree";

            string cacheKey = CacheKeyFormat.FormatInvariant();

            IDictionary<string, string> apiParams = new Dictionary<string, string>();

            return GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey, ApiConstants.SixtyMinuteCache, ParseSkillTreeResponse);
        }

        #endregion

        #region Methods

        private static IEnumerable<SkillGroup> ParseSkillTreeResponse(XElement result)
        {
            if (result == null)
            {
                return null; // return null... no data.
            }

            return (from groupRowset in result.Elements(ApiConstants.Rowset)
                    from groupRow in groupRowset.Elements(ApiConstants.Row)
                    let groupId = groupRow.Attribute("groupID").Value.ToInt32()
                    let groupName = groupRow.Attribute("groupName").Value
                    select new SkillGroup() { GroupID = groupId, GroupName = groupName, Skills = GetGroupSkills(groupRow) });                   
        }


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
                        .Select(r => new ReqSkillData() { Level = r.Attribute("skillLevel").Value.ToInt32(), TypeId = r.Attribute("typeID").Value.ToInt32() })
                    let trialTrainElement =
                        row.Elements(ApiConstants.Rowset)
                        .Where(e => e.Attribute("name").Value == "skillBonusCollection")
                        .Elements(ApiConstants.Row)
                        .FirstOrDefault(r => r.Attribute("bonusType").Value == "canNotBeTrainedOnTrial")
                       let cannotBeTrainedOnTrial = trialTrainElement != null && trialTrainElement.Attribute("bonusValue").Value.ToBoolean()
                    select new SkillData() { CannotBeTrainedOnTrial = cannotBeTrainedOnTrial, Description = desc, GroupId = groupId, Name = name, PrimaryAttribute = priAttrib, Published = published, Rank = rank, RequiredSkills = reqSkills, SecondaryAttribute = secAttrib, TypeId = typeId};

        }


        private static IEnumerable<RefType> ParseReferenceTypesResponse(XElement result)
        {
            if (result == null)
            {
                return null; // return null... no data.
            }


            return
                (from rowset in result.Elements(ApiConstants.Rowset)
                 from row in rowset.Elements(ApiConstants.Row)
                 let name = row.Attribute("refTypeName").Value
                 let id = row.Attribute("refTypeID").Value.ToInt32()
                     select new RefType(){ Id=id, Name = name});
        }

        private static IEnumerable<AllianceData> ParseAllianceListResponse(XElement result)
        {
            if (result == null)
            {
                return null; // return null... no data.
            }

           return (from rowset in result.Elements(ApiConstants.Rowset)
                             from row in rowset.Elements(ApiConstants.Row)
                             let name = row.Attribute(ApiConstants.Name).Value
                             let ticker = row.Attribute("shortName").Value
                             let allianceId = row.Attribute("allianceID").Value.ToInt32()
                             let execCorp = row.Attribute("executorCorpID").Value.ToInt32()
                             let memberCount = row.Attribute("memberCount").Value.ToInt32()
                             let startDate = row.Attribute("startDate").Value.ToDateTimeOffset(0)
                             let corps = GetCorpData(row.Element(ApiConstants.Rowset))
                             select new AllianceData() { Id= allianceId, Name = name, ShortName = ticker, ExecutorCorpId = execCorp, MemberCorps = corps,MemberCount = memberCount, StartDate = startDate});
            
        }

        private static IEnumerable<AllianceCorpData> GetCorpData(XElement memberRowSet)
        {
            if (memberRowSet == null)
            {
                return null; // return null... no data.
            }


            return (from row in memberRowSet.Elements(ApiConstants.Row)
                    let corpId = row.Attribute(ApiConstants.CorporationId).Value.ToInt32()
                    let joinDate = row.Attribute("startDate").Value.ToDateTimeOffset(0)
                    select new AllianceCorpData() { CorporationId = corpId, JoinedDate = joinDate });

        }

        /// <summary>The parse character info result.</summary>
        /// <param name="results">The results.</param>
        /// <returns>The <see cref="CharacterInfo"/>.</returns>
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
                   select new CharacterName { Id = id, Name = name };
        }

        #endregion
    }
}