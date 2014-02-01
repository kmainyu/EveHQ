// ===========================================================================
// <copyright file="CorpCharBaseClient.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2013  EveHQ Development Team
//  This file (CorpCharBaseClient.cs), is part of EveHQ.
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
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using EveHQ.Caching;
    using EveHQ.Common;
    using EveHQ.Common.Extensions;

    /// <summary>The information client.</summary>
    public abstract class CorpCharBaseClient : BaseApiClient
    {
        /// <summary>The _path prefix.</summary>
        private readonly string _pathPrefix;

        /// <summary>Initializes a new instance of the <see cref="CorpCharBaseClient"/> class. Initializes a new instance of the
        ///     CharacterClient class.</summary>
        /// <param name="eveServiceLocation">location of the eve API web service</param>
        /// <param name="cacheProvider">root folder used for caching.</param>
        /// <param name="requestProvider">Request provider to use for this instance.</param>
        /// <param name="pathPrefix">prefix to add to url paths in requests</param>
        protected internal CorpCharBaseClient(string eveServiceLocation, ICacheProvider cacheProvider, IHttpRequestProvider requestProvider, string pathPrefix)
            : base(eveServiceLocation, cacheProvider, requestProvider)
        {
            _pathPrefix = pathPrefix;
        }

        /// <summary>Gets the path prefix.</summary>
        protected string PathPrefix
        {
            get
            {
                return _pathPrefix;
            }
        }

        /// <summary>The account balance.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="responseMode">The response mode.</param>
        /// <returns></returns>
        public EveServiceResponse<IEnumerable<AccountBalance>> AccountBalance(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(AccountBalanceAsync, keyId, vCode, characterId, responseMode);
        }

        /// <summary>Gets the balance of a character.</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>Account balances.</returns>
        public Task<EveServiceResponse<IEnumerable<AccountBalance>>> AccountBalanceAsync(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Against(keyId.IsNullOrWhiteSpace());
            Guard.Against(vCode.IsNullOrWhiteSpace());
            Guard.Against(characterId == 0);

            const string methodPath = "{0}/AccountBalance.xml.aspx";
            const string cacheKeyFormat = "Character_AccountBalance_{0}_{1}";

            string cacheKey = cacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, methodPath.FormatInvariant(PathPrefix), null, cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ParseBalanceResponse);
        }

        /// <summary>The asset list.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="responseMode">The response mode.</param>
        /// <returns></returns>
        public EveServiceResponse<IEnumerable<AssetItem>> AssetList(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(AssetListAsync, keyId, vCode, characterId, responseMode);
        }

        /// <summary>Retrieves the given character's asset list.</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>An enumerable collection of all items in the characters assets.</returns>
        public Task<EveServiceResponse<IEnumerable<AssetItem>>> AssetListAsync(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Against(keyId.IsNullOrWhiteSpace());
            Guard.Against(vCode.IsNullOrWhiteSpace());
            Guard.Against(characterId == 0);

            const string MethodPath = "{0}/AssetList.xml.aspx";
            const string CacheKeyFormat = "Character_AssetList_{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null, cacheKey, ApiConstants.SixHourCache, responseMode, ParseAssetListResponse);
        }

        /// <summary>The contact list.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="responseMode">The response mode.</param>
        /// <returns></returns>
        public EveServiceResponse<IEnumerable<Contact>> ContactList(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(ContactListAsync, keyId, vCode, characterId, responseMode);
        }

        /// <summary>Retrieves the character's contact list</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The contact list for the given character</returns>
        public Task<EveServiceResponse<IEnumerable<Contact>>> ContactListAsync(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Against(keyId.IsNullOrWhiteSpace());
            Guard.Against(vCode.IsNullOrWhiteSpace());
            Guard.Against(characterId == 0);

            const string MethodPath = "{0}/ContactList.xml.aspx";
            const string CacheKeyFormat = "ContactList{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null, cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ParseContactListResponse);
        }

        /// <summary>Retrieves the collection of notifications from contacts.</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The notification list for the given character</returns>
        public Task<EveServiceResponse<IEnumerable<ContactNotification>>> ContactNotifications(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Against(keyId.IsNullOrWhiteSpace());
            Guard.Against(vCode.IsNullOrWhiteSpace());
            Guard.Against(characterId == 0);

            const string MethodPath = "{0}/ContactNotifications.xml.aspx";
            const string CacheKeyFormat = "ContactNotifications{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null, cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ParseContactNotificationResponse);
        }

        /// <summary>The contracts.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="contractId">The contract id.</param>
        /// <param name="responseMode">The response mode.</param>
        /// <returns></returns>
        public EveServiceResponse<IEnumerable<Contract>> Contracts(string keyId, string vCode, int characterId, int contractId = 0, ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(ContractsAsync, keyId, vCode, characterId, contractId, responseMode);
        }

        /// <summary>Retrieves the list of contracts</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="contractId">[OPTIONAL] Only supply a value &gt; 0 if interested in a single contract.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task<EveServiceResponse<IEnumerable<Contract>>> ContractsAsync(string keyId, string vCode, int characterId, int contractId = 0, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Against(keyId.IsNullOrWhiteSpace());
            Guard.Against(vCode.IsNullOrWhiteSpace());
            Guard.Against(characterId == 0);

            const string MethodPath = "{0}/Contracts.xml.aspx";
            const string CacheKeyFormat = "CharacterContracts{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            IDictionary<string, string> apiParams = new Dictionary<string, string>();
            if (contractId > 0)
            {
                const string ContractId = "contractID";
                apiParams[ContractId] = contractId.ToInvariantString();
            }

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), apiParams, cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ParseContractResponse);
        }

        /// <summary>The contract items.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="contractId">The contract id.</param>
        /// <param name="responseMode">The response mode.</param>
        /// <returns></returns>
        public EveServiceResponse<IEnumerable<ContractItem>> ContractItems(string keyId, string vCode, int characterId, long contractId, ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(ContractItemsAsync, keyId, vCode, characterId, contractId, responseMode);
        }

        /// <summary>Retrieves the list of contracts</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="contractId">Contract ID to get items for.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task<EveServiceResponse<IEnumerable<ContractItem>>> ContractItemsAsync(string keyId, string vCode, int characterId, long contractId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Against(keyId.IsNullOrWhiteSpace());
            Guard.Against(vCode.IsNullOrWhiteSpace());
            Guard.Against(characterId == 0);

            const string MethodPath = "{0}/ContractItems.xml.aspx";
            const string CacheKeyFormat = "CharacterContractItems{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            IDictionary<string, string> apiParams = new Dictionary<string, string>();

            const string ContractId = "contractID";
            apiParams[ContractId] = contractId.ToInvariantString();

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), apiParams, cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ParseContractItemResponse);
        }

        /// <summary>Gets the character's factional warfare statistics.</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>Factional warfare data.</returns>
        public Task<EveServiceResponse<IEnumerable<ContractBid>>> ContractBids(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Against(keyId.IsNullOrWhiteSpace());
            Guard.Against(vCode.IsNullOrWhiteSpace());
            Guard.Against(characterId == 0);

            const string MethodPath = "{0}/ContractBids.xml.aspx";
            const string CacheKeyFormat = "CharacterContractBids{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null, cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ProcessContractBidsResponse);
        }

        /// <summary>Gets the character's factional warfare statistics.</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>Factional warfare data.</returns>
        public Task<EveServiceResponse<FactionalWarfareStats>> FactionalWarfareStatistics(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Against(keyId.IsNullOrWhiteSpace());
            Guard.Against(vCode.IsNullOrWhiteSpace());
            Guard.Against(characterId == 0);

            const string MethodPath = "{0}/FacWarStats.xml.aspx";
            const string CacheKeyFormat = "CharacterFacWarStats{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null, cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ParseFacWarfareResponse);
        }

        /// <summary>The industry jobs.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="responseMode">The response mode.</param>
        /// <returns></returns>
        public EveServiceResponse<IEnumerable<IndustryJob>> IndustryJobs(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(IndustryJobsAsync, keyId, vCode, characterId, responseMode);
        }

        /// <summary>Gets the Industry Jobs for the given user.</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>A collection of Industry Job data.</returns>
        public Task<EveServiceResponse<IEnumerable<IndustryJob>>> IndustryJobsAsync(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Against(keyId.IsNullOrWhiteSpace());
            Guard.Against(vCode.IsNullOrWhiteSpace());
            Guard.Against(characterId <= 0);

            const string MethodPath = "{0}/IndustryJobs.xml.aspx";
            const string CacheKeyFormat = "CharacterIndustryJobs{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null, cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ParseIndustryJobsResponse);
        }

        /// <summary>The market orders.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="responseMode">The response mode.</param>
        /// <returns></returns>
        public EveServiceResponse<IEnumerable<MarketOrder>> MarketOrders(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(MarketOrdersAsync, keyId, vCode, characterId, responseMode);
        }

        /// <summary>Retrieves the market orders for the character.</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>a collection of market orders</returns>
        public Task<EveServiceResponse<IEnumerable<MarketOrder>>> MarketOrdersAsync(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            Guard.Ensure(characterId > 0);

            const string MethodPath = "{0}/MarketOrders.xml.aspx";
            const string CacheKeyFormat = "CharacterMarketOrders{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null, cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ProcessMarketOrdersResponse);
        }

        /// <summary>Retrieves a collection of a character's medals</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>a collection of medals.</returns>
        public Task<EveServiceResponse<IEnumerable<Medal>>> Medals(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            Guard.Ensure(characterId > 0);

            const string MethodPath = "{0}/Medals.xml.aspx";
            const string CacheKeyFormat = "CharacterMedals{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null, cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ProcessMedalsResponse);
        }

        /// <summary>The research.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task<EveServiceResponse<IEnumerable<Research>>> Research(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            Guard.Ensure(characterId > 0);

            const string MethodPath = "{0}/Research.xml.aspx";
            const string CacheKeyFormat = "CharacterResearch{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null, cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ProcessResearchResponse);
        }

        /// <summary>The skill in training.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task<EveServiceResponse<SkillInTraining>> SkillInTraining(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            Guard.Ensure(characterId > 0);

            const string MethodPath = "{0}/SkillInTraining.xml.aspx";
            const string CacheKeyFormat = "SkillInTraining{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null, cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ProcessSkillInTrainingResponse);
        }

        /// <summary>The skill queue.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public EveServiceResponse<IEnumerable<QueuedSkill>> SkillQueue(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(SkillQueueAsync, keyId, vCode, characterId, responseMode);
        }

        /// <summary>The skill queue.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task<EveServiceResponse<IEnumerable<QueuedSkill>>> SkillQueueAsync(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            Guard.Ensure(characterId > 0);

            const string MethodPath = "{0}/SkillQueue.xml.aspx";
            const string CacheKeyFormat = "SkillQueue{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null, cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ProcessSkillQueueResponse);
        }

        /// <summary>The npc standings.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="responseMode">The response mode.</param>
        /// <returns></returns>
        public EveServiceResponse<IEnumerable<NpcStanding>> NPCStandings(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(NPCStandingsAsync, keyId, vCode, characterId, responseMode);
        }

        /// <summary>The standings.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task<EveServiceResponse<IEnumerable<NpcStanding>>> NPCStandingsAsync(string keyId, string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            Guard.Ensure(characterId > 0);

            const string MethodPath = "{0}/Standings.xml.aspx";
            const string CacheKeyFormat = "CharacterNpcStandings{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null, cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ProcessStandingsResponse);
        }

        // TODO: Wallet methods require support for wallet divisions

        /// <summary>The wallet journal.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="accountKey">The account key.</param>
        /// <param name="fromId">The from id.</param>
        /// <param name="rowCount">The row count.</param>
        /// <param name="responseMode">The response mode.</param>
        /// <returns></returns>
        public EveServiceResponse<IEnumerable<WalletJournalEntry>> WalletJournal(
            string keyId, 
            string vCode, 
            int characterId, 
            int accountKey, 
            long? fromId = null, 
            int? rowCount = null, 
            ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(WalletJournalAsync, keyId, vCode, characterId, accountKey, fromId, rowCount, responseMode);
        }

        /// <summary>The wallet journal.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="accountKey"></param>
        /// <param name="fromId">The from id.</param>
        /// <param name="rowCount">The row count.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task<EveServiceResponse<IEnumerable<WalletJournalEntry>>> WalletJournalAsync(
            string keyId, 
            string vCode, 
            int characterId, 
            int accountKey, 
            long? fromId = null, 
            int? rowCount = null, 
            ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            Guard.Ensure(characterId > 0);
            Guard.Ensure(rowCount == null || rowCount.Value > 0);

            const string MethodPath = "{0}/WalletJournal.xml.aspx";
            const string CacheKeyFormat = "WalletJournal{0}_{1}";

            const string FromId = "fromID";
            const string RowCount = "rowCount";
            const string AccountKey = "accountKey";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);
            IDictionary<string, string> apiParams = new Dictionary<string, string>();

            apiParams[AccountKey] = accountKey.ToInvariantString();

            if (fromId != null)
            {
                apiParams[FromId] = fromId.Value.ToInvariantString();
            }

            if (rowCount != null)
            {
                apiParams[RowCount] = rowCount.Value.ToInvariantString();
            }

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), apiParams, cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ProcessWalletJournalResponse);
        }

        /// <summary>The wallet transactions.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="accountKey">The account key.</param>
        /// <param name="fromId">The from id.</param>
        /// <param name="rowCount">The row count.</param>
        /// <param name="responseMode">The response mode.</param>
        /// <returns></returns>
        public EveServiceResponse<IEnumerable<WalletTransaction>> WalletTransactions(
            string keyId, 
            string vCode, 
            int characterId, 
            int accountKey, 
            long? fromId = null, 
            int? rowCount = null, 
            ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(WalletTransactionsAsync, keyId, vCode, characterId, accountKey, fromId, rowCount, responseMode);
        }

        /// <summary>The wallet transactions.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="accountKey"></param>
        /// <param name="fromId">The from id.</param>
        /// <param name="rowCount">The row count.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task<EveServiceResponse<IEnumerable<WalletTransaction>>> WalletTransactionsAsync(
            string keyId, 
            string vCode, 
            int characterId, 
            int accountKey, 
            long? fromId = null, 
            int? rowCount = null, 
            ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            Guard.Ensure(characterId > 0);
            Guard.Ensure(rowCount == null || rowCount.Value > 0);

            const string MethodPath = "{0}/WalletTransactions.xml.aspx";
            const string CacheKeyFormat = "WalletTransactions{0}_{1}";

            const string FromId = "fromID";
            const string RowCount = "rowCount";
            const string AccountKey = "accountKey";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);
            IDictionary<string, string> apiParams = new Dictionary<string, string>();

            apiParams[AccountKey] = accountKey.ToInvariantString();

            if (fromId != null)
            {
                apiParams[FromId] = fromId.Value.ToInvariantString();
            }

            if (rowCount != null)
            {
                apiParams[RowCount] = rowCount.Value.ToInvariantString();
            }

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), apiParams, cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ProcessWalletTransctionResponse);
        }

        /// <summary>The process wallet journal response.</summary>
        /// <param name="result">The result.</param>
        /// <returns>The collection of wallet entries.</returns>
        private static IEnumerable<WalletTransaction> ProcessWalletTransctionResponse(XElement result)
        {
            if (result == null)
            {
                return new WalletTransaction[0]; // empty collection
            }

            return from rowset in result.Elements(ApiConstants.Rowset)
                    from row in rowset.Elements(ApiConstants.Row)
                    let date = row.Attribute("transactionDateTime").Value.ToDateTimeOffset(0)
                    let transactionId = row.Attribute("transactionID").Value.ToInt32()
                    let quantity = row.Attribute("quantity").Value.ToInt32()
                    let typeName = row.Attribute("typeName").Value
                    let typeId = row.Attribute("typeID").Value.ToInt32()
                    let price = row.Attribute("price").Value.ToDouble()
                    let clientId = row.Attribute("clientID").Value.ToInt32()
                    let clientName = row.Attribute("clientName").Value
                    let stationId = row.Attribute("stationID").Value.ToInt32()
                    let stationNane = row.Attribute("stationName").Value
                    let transactionType = row.Attribute("transactionType").Value
                    let transactionFor = row.Attribute("transactionFor").Value
                    let journalEntryId = row.Attribute("journalTransactionID") != null ? row.Attribute("journalTransactionID").Value.ToInt64() : 0
                    select
                        new WalletTransaction
                            {
                                ClientId = clientId, 
                                ClientName = clientName, 
                                Price = price, 
                                Quantity = quantity, 
                                StationId = stationId, 
                                StationName = stationNane, 
                                TransactionDateTime = date, 
                                TransactionFor = transactionFor, 
                                TransactionId = transactionId, 
                                TransactionType = transactionType, 
                                TypeId = typeId, 
                                TypeName = typeName, 
                                WalletJournalEntryId = journalEntryId
                            };
        }

        /// <summary>The process wallet journal response.</summary>
        /// <param name="result">The result.</param>
        /// <returns>The collection of wallet entries.</returns>
        private static IEnumerable<WalletJournalEntry> ProcessWalletJournalResponse(XElement result)
        {
            if (result == null)
            {
                return new WalletJournalEntry[0]; // empty collection
            }

            return from rowset in result.Elements(ApiConstants.Rowset)
                    from row in rowset.Elements(ApiConstants.Row)
                    let date = row.Attribute("date").Value.ToDateTimeOffset(0)
                    let refId = row.Attribute("refID").Value.ToInt64()
                    let refType = row.Attribute("refTypeID").Value.ToInt32()
                    let firstName = row.Attribute("ownerName1").Value
                    let firstId = row.Attribute("ownerID1").Value.ToInt32()
                    let secondName = row.Attribute("ownerName2").Value
                    let secondId = row.Attribute("ownerID2").Value.ToInt32()
                    let argName = row.Attribute("argName1").Value
                    let argId = row.Attribute("argID1").Value.ToInt32()
                    let amount = row.Attribute("amount").Value.ToDouble()
                    let balance = row.Attribute("balance").Value.ToDouble()
                    let reason = row.Attribute("reason").Value
                    let taxReceiverId = row.Attribute("taxReceiverID").Value.ToInt32()
                    let taxAmount = row.Attribute("taxAmount").Value.ToDouble()
                    select
                        new WalletJournalEntry
                            {
                                Amount = amount, 
                                ArgumentId = argId, 
                                ArgumentName = argName, 
                                Balance = balance, 
                                Date = date, 
                                FirstPartyId = firstId, 
                                FirstPartyName = firstName, 
                                Reason = reason, 
                                ReferenceType = refType, 
                                RefId = refId, 
                                SecondPartyId = secondId, 
                                SecondPartyName = secondName, 
                                TaxAmount = taxAmount, 
                                TaxReceiverId = taxReceiverId
                            };
        }

        /// <summary>The process standings response.</summary>
        /// <param name="result">The result.</param>
        /// <returns>The collection of NPC standings</returns>
        private static IEnumerable<NpcStanding> ProcessStandingsResponse(XElement result)
        {
            if (result == null)
            {
                return new NpcStanding[0]; // empty collection
            }

            return from standings in result.Elements("characterNPCStandings")
                    from rowset in standings.Elements(ApiConstants.Rowset)
                    from row in rowset.Elements(ApiConstants.Row)
                    let type = rowset.Attribute("name").Value.ToEnum<NpcType>()
                    let fromId = row.Attribute("fromID").Value.ToInt32()
                    let fromName = row.Attribute("fromName").Value
                    let standing = row.Attribute("standing").Value.ToDouble()
                    select new NpcStanding { FromId = fromId, FromName = fromName, Kind = type, Standing = standing };
        }

        /// <summary>The process skill queue response.</summary>
        /// <param name="result">The result.</param>
        /// <returns>The collection of skills in queue</returns>
        public static IEnumerable<QueuedSkill> ProcessSkillQueueResponse(XElement result)
        {
            if (result == null)
            {
                return new QueuedSkill[0]; // empty collection
            }

            return from rowset in result.Elements(ApiConstants.Rowset)
                    from row in rowset.Elements(ApiConstants.Row)
                    let position = row.Attribute("queuePosition").Value.ToInt32()
                    let type = row.Attribute("typeID").Value.ToInt32()
                    let level = row.Attribute("level").Value.ToInt32()
                    let startSP = row.Attribute("startSP").Value.ToInt32()
                    let endSP = row.Attribute("endSP").Value.ToInt32()
                    let startTime = row.Attribute("startTime").Value.ToDateTimeOffset(0)
                    let endTime = row.Attribute("endTime").Value.ToDateTimeOffset(0)
                    select new QueuedSkill { EndSP = endSP, EndTime = endTime, Level = level, QueuePosition = position, StartSP = startSP, StartTime = startTime, TypeId = type };
        }

        /// <summary>The process skill in training response.</summary>
        /// <param name="result">The result.</param>
        /// <returns>The <see cref="EveApi.SkillInTraining"/>.</returns>
        private static SkillInTraining ProcessSkillInTrainingResponse(XElement result)
        {
            if (result == null)
            {
                return null; // empty
            }

            var skill = new SkillInTraining();

            skill.IsTraining = result.Element("skillInTraining").Value.ToInt32() == 1;

            if (skill.IsTraining)
            {
                skill.TrainingEndTime = result.Element("trainingEndTime").Value.ToDateTimeOffset(0);
                skill.TrainingStartTime = result.Element("trainingStartTime").Value.ToDateTimeOffset(0);
                skill.CurrentTQTime = result.Element("currentTQTime").Value.ToDateTimeOffset(0);
                skill.TrainingTypeId = result.Element("trainingTypeID").Value.ToInt32();
                skill.TrainingStartSP = result.Element("trainingStartSP").Value.ToInt32();
                skill.TrainingEndSP = result.Element("trainingDestinationSP").Value.ToInt32();
                skill.TrainingToLevel = result.Element("trainingToLevel").Value.ToInt32();
            }

            return skill;
        }

        /// <summary>The process research response.</summary>
        /// <param name="result">The result.</param>
        /// <returns>The collection of research.</returns>
        private static IEnumerable<Research> ProcessResearchResponse(XElement result)
        {
            if (result == null)
            {
                return new Research[0]; // empty collection
            }

            return from rowset in result.Elements(ApiConstants.Rowset)
                    from row in rowset.Elements(ApiConstants.Row)
                    let agent = row.Attribute("agentID").Value.ToInt32()
                    let skillType = row.Attribute("skillTypeID").Value.ToInt32()
                    let startDate = row.Attribute("researchStartDate").Value.ToDateTimeOffset(0)
                    let pointsPerDay = row.Attribute("pointsPerDay").Value.ToDouble()
                    let pointsRemaining = row.Attribute("remainderPoints").Value.ToDouble()
                    select new Research { AgentId = agent, PointsPerDay = pointsPerDay, RemainingPoints = pointsRemaining, ResearchStartDate = startDate, SkillTypeId = skillType };
        }

        /// <summary>Processes the notifications response.</summary>
        /// <param name="result"></param>
        /// <returns>The collection of notifications.</returns>
        private static IEnumerable<ContractBid> ProcessContractBidsResponse(XElement result)
        {
            if (result == null)
            {
                return new ContractBid[0]; // empty collection
            }

            return from rowset in result.Elements(ApiConstants.Rowset)
                    from row in rowset.Elements(ApiConstants.Row)
                    let bidId = row.Attribute("bidID").Value.ToInt64()
                    let contractId = row.Attribute("contractID").Value.ToInt64()
                    let bidder = row.Attribute("bidderID").Value.ToInt64()
                    let date = row.Attribute("dateBid").Value.ToDateTimeOffset(0)
                    let amount = row.Attribute("amount").Value.ToDouble()
                    select new ContractBid { Amount = amount, BidDateTime = date, BidderId = bidder, BidId = bidId, ContractId = contractId };
        }

        /// <summary>Processes the medals xml</summary>
        /// <param name="result">xml data to process.</param>
        /// <returns>a collection of Medals</returns>
        private static IEnumerable<Medal> ProcessMedalsResponse(XElement result)
        {
            if (result == null)
            {
                return new Medal[0]; // empty collection
            }

            return from rowset in result.Elements(ApiConstants.Rowset)
                    from row in rowset.Elements(ApiConstants.Row)
                    let medalId = row.Attribute("medalID").Value.ToInt32()
                    let reason = row.Attribute("reason").Value
                    let status = row.Attribute("status").Value
                    let issuerId = row.Attribute("issuerID").Value.ToInt32()
                    let issued = row.Attribute("issued").Value.ToDateTimeOffset(0)
                    let corpId = row.Attribute("corporationID") != null ? row.Attribute("corporationID").Value.ToInt32() : 0
                    let title = row.Attribute("title") != null ? row.Attribute("title").Value : string.Empty
                    let description = row.Attribute("description") != null ? row.Attribute("description").Value : string.Empty
                    select new Medal { MedalId = medalId, Reason = reason, Status = status, IssuerId = issuerId, DateIssued = issued, CorporationId = corpId, Title = title, Description = description };
        }

        /// <summary>Processes the market order xml into objects.</summary>
        /// <param name="result">xml to process</param>
        /// <returns>Market orders.</returns>
        private static IEnumerable<MarketOrder> ProcessMarketOrdersResponse(XElement result)
        {
            if (result == null)
            {
                return new MarketOrder[0]; // empty collection
            }

            return from rowset in result.Elements(ApiConstants.Rowset)
                    from row in rowset.Elements(ApiConstants.Row)
                    let orderId = row.Attribute("orderID").Value.ToInt32()
                    let charId = row.Attribute("charID").Value.ToInt32()
                    let stationId = row.Attribute("stationID").Value.ToInt32()
                    let quantityEntered = row.Attribute("volEntered").Value.ToInt32()
                    let quantityRemaining = row.Attribute("volRemaining").Value.ToInt32()
                    let minVolumn = row.Attribute("minVolume").Value.ToInt32()
                    let orderState = (MarketOrderState)Enum.Parse(typeof(MarketOrderState), row.Attribute("orderState").Value)
                    let typeId = row.Attribute("typeID").Value.ToInt32()
                    let range = row.Attribute("range").Value.ToInt32()
                    let accountKey = row.Attribute("accountKey").Value.ToInt32()
                    let duration = TimeSpan.FromDays(row.Attribute("duration").Value.ToInt32())
                    let escrow = row.Attribute("escrow").Value.ToDouble()
                    let price = row.Attribute("price").Value.ToDouble()
                    let isBuyOrder = row.Attribute("bid").Value.ToBoolean()
                    let dateIssued = row.Attribute("issued").Value.ToDateTimeOffset(0)
                    select
                        new MarketOrder
                            {
                                OrderId = orderId, 
                                CharId = charId, 
                                StationId = stationId, 
                                QuantityEntered = quantityEntered, 
                                QuantityRemaining = quantityRemaining, 
                                MinQuantity = minVolumn, 
                                OrderState = orderState, 
                                TypeId = typeId, 
                                Range = range, 
                                AccountKey = accountKey, 
                                Duration = duration, 
                                Escrow = escrow, 
                                Price = price, 
                                IsBuyOrder = isBuyOrder, 
                                DateIssued = dateIssued
                            };
        }

        /// <summary>Processes the xml response from the web service for the industry jobs method.</summary>
        /// <param name="result">xml from the web service.</param>
        /// <returns>a collection of industry jobs</returns>
        private static IEnumerable<IndustryJob> ParseIndustryJobsResponse(XElement result)
        {
            if (result == null)
            {
                return new IndustryJob[0]; // empty collection
            }

            IEnumerable<IndustryJob> jobs = from rowset in result.Elements(ApiConstants.Rowset)
                                             from row in rowset.Elements(ApiConstants.Row)
                                             let jobId = row.Attribute("jobID").Value.ToInt64()
                                             let assemblyLineId = row.Attribute("assemblyLineID").Value.ToInt64()
                                             let containerId = row.Attribute("containerID").Value.ToInt64()
                                             let installedItemId = row.Attribute("installedItemID").Value.ToInt64()
                                             let installedItemLocationId = row.Attribute("installedItemLocationID").Value.ToInt64()
                                             let installedItemQuantity = row.Attribute("installedItemQuantity").Value.ToInt32()
                                             let installedItemProductionLevel = row.Attribute("installedItemProductivityLevel").Value.ToInt32()
                                             let installedItemMaterialLevel = row.Attribute("installedItemMaterialLevel").Value.ToInt32()
                                             let installedItemLicensedProductionRunsRemaining = row.Attribute("installedItemLicensedProductionRunsRemaining").Value.ToInt32()
                                             let outputLocationId = row.Attribute("outputLocationID").Value.ToInt64()
                                             let installerId = row.Attribute("installerID").Value.ToInt64()
                                             let runs = row.Attribute("runs").Value.ToInt32()
                                             let licensedProductionRuns = row.Attribute("licensedProductionRuns").Value.ToInt32()
                                             let installedInSolarSystemId = row.Attribute("installedInSolarSystemID").Value.ToInt32()
                                             let containerLocationId = row.Attribute("containerLocationID").Value.ToInt64()
                                             let materialMultiplier = row.Attribute("materialMultiplier").Value.ToDouble()
                                             let charMaterialMultiplier = row.Attribute("charMaterialMultiplier").Value.ToDouble()
                                             let timeMultiplier = row.Attribute("timeMultiplier").Value.ToDouble()
                                             let charTimeMultiplier = row.Attribute("charTimeMultiplier").Value.ToDouble()
                                             let installedItemTypeId = row.Attribute("installedItemTypeID").Value.ToInt32()
                                             let outputTypeId = row.Attribute("outputTypeID").Value.ToInt32()
                                             let containerTypeId = row.Attribute("containerTypeID").Value.ToInt64()
                                             let installedItemCopy = row.Attribute("installedItemCopy").Value.ToBoolean()
                                             let completed = row.Attribute("completed").Value.ToBoolean()
                                             let completedSuccessfully = row.Attribute("completedSuccessfully").Value.ToBoolean()
                                             let installedItemFlag = row.Attribute("installedItemFlag").Value.ToBoolean()
                                             let outputFlag = row.Attribute("outputFlag").Value.ToInt32()
                                             let activityId = row.Attribute("activityID").Value.ToInt32()
                                             let installTime = row.Attribute("installTime").Value.ToDateTimeOffset(0)
                                             let beginProductionTime = row.Attribute("beginProductionTime").Value.ToDateTimeOffset(0)
                                             let endProductionTime = row.Attribute("endProductionTime").Value.ToDateTimeOffset(0)
                                             let pauseProductionTime = row.Attribute("pauseProductionTime").Value.ToDateTimeOffset(0)
                                             select
                                                 new IndustryJob
                                                     {
                                                         JobId = jobId, 
                                                         AssemblyLineId = assemblyLineId, 
                                                         ContainerId = containerId, 
                                                         InstalledItemId = installedItemId, 
                                                         InstalledItemLocationId = installedItemLocationId, 
                                                         InstalledItemQuantity = installedItemQuantity, 
                                                         InstalledItemProductivityLevel = installedItemProductionLevel, 
                                                         InstalledItemMaterialLevel = installedItemMaterialLevel, 
                                                         InstalledItemLicensedProductionRunsRemaining = installedItemLicensedProductionRunsRemaining, 
                                                         OutputLocationId = outputLocationId, 
                                                         InstallerId = installerId, 
                                                         Runs = runs, 
                                                         LicensedProductionRuns = licensedProductionRuns, 
                                                         InstalledInSolarSystemId = installedInSolarSystemId, 
                                                         ContainerLocationId = containerLocationId, 
                                                         MaterialMultiplier = materialMultiplier, 
                                                         CharMaterialMultiplier = charMaterialMultiplier, 
                                                         TimeMultiplier = timeMultiplier, 
                                                         CharTimeMultiplier = charTimeMultiplier, 
                                                         InstalledItemTypeId = installedItemTypeId, 
                                                         OutputTypeId = outputTypeId, 
                                                         ContainerTypeId = containerTypeId, 
                                                         InstalledItemCopy = installedItemCopy, 
                                                         Completed = completed, 
                                                         CompletedSuccessfully = completedSuccessfully, 
                                                         InstalledItemFlag = installedItemFlag, 
                                                         OutputFlag = outputFlag, 
                                                         ActivityId = activityId, 
                                                         InstallTime = installTime, 
                                                         BeginProductionTime = beginProductionTime, 
                                                         EndProductionTime = endProductionTime, 
                                                         PauseProductionTime = pauseProductionTime
                                                     };

            return jobs;
        }

        /// <summary>Parses the factional warfare data.</summary>
        /// <param name="result">xml data</param>
        /// <returns>FactionalWarfareStats object.</returns>
        private static FactionalWarfareStats ParseFacWarfareResponse(XElement result)
        {
            if (result == null)
            {
                return null;
            }

            // Suppressing Null checks for xml parsing as exceptions should be thrown if xml structure is not correct. Base class will catch these.
            // ReSharper disable PossibleNullReferenceException
            int factionId = result.Element("factionID").Value.ToInt32();
            string factionName = result.Element("factionName").Value;
            DateTimeOffset enlisted = result.Element("enlisted").Value.ToDateTimeOffset(0);
            int currentRank = result.Element("currentRank").Value.ToInt32();
            int highestRank = result.Element("highestRank").Value.ToInt32();
            int killsYesterday = result.Element("killsYesterday").Value.ToInt32();
            int killsLastWeek = result.Element("killsLastWeek").Value.ToInt32();
            int killsTotal = result.Element("killsTotal").Value.ToInt32();
            int victoryPointsYesterday = result.Element("victoryPointsYesterday").Value.ToInt32();
            int victoryPointsLastWeek = result.Element("victoryPointsLastWeek").Value.ToInt32();
            int victoryPointsTotal = result.Element("victoryPointsTotal").Value.ToInt32();

            // ReSharper restore PossibleNullReferenceException
            return new FactionalWarfareStats
                       {
                           FactionId = factionId, 
                           FactionName = factionName, 
                           Enlisted = enlisted, 
                           CurrentRank = currentRank, 
                           HighestRank = highestRank, 
                           KillsYesterday = killsYesterday, 
                           KillsLastWeek = killsLastWeek, 
                           KillsTotal = killsTotal, 
                           VictoryPointsYesterday = victoryPointsYesterday, 
                           VictoryPointsLastWeek = victoryPointsLastWeek, 
                           VictoryPointsTotal = victoryPointsTotal
                       };
        }

        /// <summary>Processes the response for the Contracts method.</summary>
        /// <param name="result">XML data to process.</param>
        /// <returns>A collection of contract objects.</returns>
        private static IEnumerable<ContractItem> ParseContractItemResponse(XElement result)
        {
            if (result == null)
            {
                return new ContractItem[0]; // return empty collection... no data.
            }

            return from rowset in result.Elements(ApiConstants.Rowset)
                    from row in rowset.Elements(ApiConstants.Row)
                    let record = row.Attribute("recordID").Value.ToInt64()
                    let type = row.Attribute("typeID").Value.ToInt32()
                    let quantity = row.Attribute("quantity").Value.ToInt64()
                    let rawQuantity = row.Attribute("rawQuantity") != null ? row.Attribute("rawQuantity").Value.ToInt32() : -1
                    let single = row.Attribute("singleton").Value.ToBoolean()
                    let included = row.Attribute("included").Value.ToBoolean()
                    select new ContractItem { IsIncluded = included, IsSingleton = single, Quantity = quantity, RawQuantity = rawQuantity, RecordId = record, TypeId = type };
        }

        /// <summary>Processes the response for the Contracts method.</summary>
        /// <param name="result">XML data to process.</param>
        /// <returns>A collection of contract objects.</returns>
        private static IEnumerable<Contract> ParseContractResponse(XElement result)
        {
            if (result == null)
            {
                return new Contract[0]; // return empty collection... no data.
            }

            ContractType tempType;
            ContractStatus tempStatus;
            ContractAvailability tempAvail;
            DateTime tempDate;
            IEnumerable<Contract> contracts = from rowset in result.Elements(ApiConstants.Rowset)
                                               from row in rowset.Elements(ApiConstants.Row)
                                               let contractId = int.Parse(row.Attribute("contractID").Value)
                                               let issuerId = int.Parse(row.Attribute("issuerID").Value)
                                               let issuerCorpId = int.Parse(row.Attribute("issuerCorpID").Value)
                                               let assigneeId = int.Parse(row.Attribute("assigneeID").Value)
                                               let acceptorId = int.Parse(row.Attribute("acceptorID").Value)
                                               let startStationId = int.Parse(row.Attribute("startStationID").Value)
                                               let endStationId = int.Parse(row.Attribute("endStationID").Value)
                                               let type = Enum.TryParse(row.Attribute("type").Value, out tempType) ? tempType : ContractType.Unknown
                                               let status = Enum.TryParse(row.Attribute("status").Value, out tempStatus) ? tempStatus : ContractStatus.Unknown
                                               let title = row.Attribute("title").Value
                                               let forCorp = int.Parse(row.Attribute("forCorp").Value) == 1
                                               let availability = Enum.TryParse(row.Attribute("availability").Value, out tempAvail) ? tempAvail : ContractAvailability.Unknown
                                               let dateIssued = DateTime.TryParse(row.Attribute("dateIssued").Value, out tempDate) ? new DateTimeOffset(tempDate, TimeSpan.Zero) : DateTimeOffset.MinValue
                                               let dateExpired = DateTime.TryParse(row.Attribute("dateExpired").Value, out tempDate) ? new DateTimeOffset(tempDate, TimeSpan.Zero) : DateTimeOffset.MinValue
                                               let dateAccepted = DateTime.TryParse(row.Attribute("dateAccepted").Value, out tempDate) ? new DateTimeOffset(tempDate, TimeSpan.Zero) : DateTimeOffset.MinValue
                                               let dateCompleted = DateTime.TryParse(row.Attribute("dateCompleted").Value, out tempDate) ? new DateTimeOffset(tempDate, TimeSpan.Zero) : DateTimeOffset.MinValue
                                               let numDate = int.Parse(row.Attribute("numDays").Value)
                                               let price = double.Parse(row.Attribute("price").Value)
                                               let reward = double.Parse(row.Attribute("reward").Value)
                                               let collateral = double.Parse(row.Attribute("collateral").Value)
                                               let buyout = double.Parse(row.Attribute("buyout").Value)
                                               let volume = double.Parse(row.Attribute("volume").Value)
                                               select
                                                   new Contract
                                                       {
                                                           ContractId = contractId, 
                                                           IssuerId = issuerId, 
                                                           IssuserCorpId = issuerCorpId, 
                                                           AssigneeId = assigneeId, 
                                                           AcceptorId = acceptorId, 
                                                           StartStationId = startStationId, 
                                                           EndStationId = endStationId, 
                                                           Type = type, 
                                                           Status = status, 
                                                           Title = title, 
                                                           ForCorp = forCorp, 
                                                           Availability = availability, 
                                                           DateIssued = dateIssued, 
                                                           DateExpired = dateExpired, 
                                                           DateAccepted = dateAccepted, 
                                                           NumberOfDays = numDate, 
                                                           DateCompleted = dateCompleted, 
                                                           Price = price, 
                                                           Reward = reward, 
                                                           Collateral = collateral, 
                                                           Buyout = buyout, 
                                                           Volume = volume
                                                       };

            return contracts;
        }

        /// <summary>Parses the xml data response from the Eve web service's ContactNotifications method.</summary>
        /// <param name="result">The xml data to process</param>
        /// <returns>the collection of contact notifications.</returns>
        private static IEnumerable<ContactNotification> ParseContactNotificationResponse(XElement result)
        {
            if (result == null)
            {
                return new ContactNotification[0]; // return empty collection... no data.
            }

            IEnumerable<ContactNotification> notifications = from rowset in result.Elements(ApiConstants.Rowset)
                                                              from row in rowset.Elements(ApiConstants.Row)
                                                              let notificationId = int.Parse(row.Attribute("notificationID").Value)
                                                              let senderId = int.Parse(row.Attribute("senderID").Value)
                                                              let senderName = row.Attribute("senderName").Value
                                                              let sentDate = new DateTimeOffset(DateTime.Parse(row.Attribute("sentDate").Value, CultureInfo.InvariantCulture), TimeSpan.Zero)
                                                              let messageData = row.Attribute("messageData").Value
                                                              select
                                                                  new ContactNotification
                                                                      {
                                                                          NotificationId = notificationId, 
                                                                          SenderId = senderId, 
                                                                          SenderName = senderName, 
                                                                          SentDate = sentDate, 
                                                                          MessageData = messageData
                                                                      };
            return notifications;
        }

        /// <summary>Parses the xml data response from the Eve web service's ContactList method.</summary>
        /// <param name="result">The xml data to process</param>
        /// <returns>the collection of contact lists.</returns>
        private static IEnumerable<Contact> ParseContactListResponse(XElement result)
        {
            if (result == null)
            {
                return new Contact[0]; // return empty collection... no data.
            }

            // convert the xml into collections of objects.
            // only the personal list has the "inWatchlist" attribute...others defaulted to false    
            return from rowset in result.Elements(ApiConstants.Rowset)
                   from row in rowset.Elements(ApiConstants.Row)
                   let contactId = int.Parse(row.Attribute("contactID").Value)
                   let contactName = row.Attribute("contactName").Value
                   let isInWatchList = row.Attributes("inWatchlist").Any() && bool.Parse(row.Attribute("inWatchlist").Value)
                   let standing = int.Parse(row.Attribute("standing").Value)
                   let kind = rowset.Attribute("name").Value.ToEnum<ContactType>()
                   select new Contact { ContactId = contactId, ContactType = kind, ContactName = contactName, IsInWatchList = isInWatchList, Standing = standing };
        }

        /// <summary>Parses the asset list response xml into C# objects.</summary>
        /// <param name="results">the xml result element.</param>
        /// <returns>a collection of items.</returns>
        private static IEnumerable<AssetItem> ParseAssetListResponse(XElement results)
        {
            XElement rowset = results.Element(ApiConstants.Rowset);

            if (rowset == null)
            {
                return new AssetItem[0]; // return empty collection
            }

            return rowset.Elements(ApiConstants.Row).Select(row => CreateItemFromRow(row, 0));
        }

        /// <summary>Parses the response from AccountBalance API.</summary>
        /// <param name="results">XML results from the service</param>
        /// <returns>a collection of balances.</returns>
        private static IEnumerable<AccountBalance> ParseBalanceResponse(XElement results)
        {
            XElement rowset = results.Element(ApiConstants.Rowset);

            if (rowset == null)
            {
                return new AccountBalance[0]; // return empty collection
            }

            return rowset.Elements().Select(
                row =>
                    {
                        int accountId = row.Attribute(ApiConstants.AccountId).Value.ToInt32();
                        int accountKey = row.Attribute(ApiConstants.AccountKey).Value.ToInt32();
                        double balance = row.Attribute(ApiConstants.Balance).Value.ToDouble();
                        return new AccountBalance { AccountId = accountId, AccountKey = accountKey, Balance = balance };
                    });
        }

        /// <summary>Creates a single AssetItem from xml.</summary>
        /// <param name="row">row describing the item.</param>
        /// <param name="parentId">parent id if applicable, otherwise 0.</param>
        /// <returns>an AssetItem.</returns>
        private static AssetItem CreateItemFromRow(XElement row, long parentId)
        {
            long itemId,  quantity, rawQuantity;
            int typeId, locationId, flag;
            bool single;

            XAttribute item = row.Attribute(ApiConstants.ItemId);
            XAttribute location = row.Attribute(ApiConstants.LocationId);
            XAttribute type = row.Attribute(ApiConstants.TypeId);
            XAttribute count = row.Attribute(ApiConstants.Quantity);
            XAttribute rawCount = row.Attribute(ApiConstants.RawQuantity);
            XAttribute flagAttrib = row.Attribute(ApiConstants.Flag);
            XAttribute singleton = row.Attribute(ApiConstants.Singleton);

            itemId = item != null ? item.Value.ToInt64() : 0;
            locationId = location != null ? location.Value.ToInt32() : 0;
            typeId = type != null ? type.Value.ToInt32(): 0;
            quantity = count != null ? count.Value.ToInt64() : 0;
            rawQuantity = rawCount != null ? rawCount.Value.ToInt64() : 0;
            flag = flagAttrib != null ?flagAttrib.Value.ToInt32() : 0;
            single = singleton != null && singleton.Value.ToBoolean();

            // check to see if there are children.
            XElement childRowset = row.Element(ApiConstants.Rowset);
            IEnumerable<AssetItem> children = null;
            if (childRowset != null)
            {
                children = childRowset.Elements(ApiConstants.Row).Select(rowItem => CreateItemFromRow(rowItem, itemId));
            }

            return new AssetItem
                       {
                           ItemId = itemId, 
                           LocationId = locationId, 
                           TypeId = typeId, 
                           Quantity = quantity, 
                           RawQuantity = rawQuantity, 
                           Flag = flag, 
                           Singleton = single, 
                           Contents = children, 
                           ParentItemId = parentId
                       };
        }
    }
}