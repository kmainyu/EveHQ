//  ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (AccountTests.cs), is part of EveHQ.
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

namespace EveHQ.Tests.Api
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using EveHQ.Common;
    using EveHQ.EveApi;

    using NUnit.Framework;

    /// <summary>
    /// Unit tests for the Eve Account API methods
    /// </summary>
    [TestFixture]
    public static class AccountTests
    {
        private const string AccountStatusXml =
            "<?xml version='1.0' encoding='UTF-8'?><eveapi version=\"2\"><currentTime>2011-09-25 03:00:50</currentTime><result><paidUntil>2011-10-20 13:22:57</paidUntil><createDate>2008-02-09 19:51:00</createDate><logonCount>1371</logonCount><logonMinutes>245488</logonMinutes></result><cachedUntil>2011-09-25 03:57:50</cachedUntil></eveapi>";

        private const string ApiKeyInfoXml =
            "<eveapi version=\"2\"><currentTime>2011-10-28 11:14:40</currentTime><result><key accessMask=\"134217727\" type=\"Account\" expires=\"2012-10-13 00:00:00\"><rowset name=\"characters\" key=\"characterID\" columns=\"characterID,characterName,corporationID,corporationName\"><row characterID=\"154416088\" characterName=\"CCP Stillman asdefdsdfdsdfrsdf\" corporationID=\"1000181\" corporationName=\"Federal Defence Union\"/><row characterID=\"154432700\" characterName=\"RTC'3\" corporationID=\"98000179\" corporationName=\"RTC'3 Corp\"/><row characterID=\"154436316\" characterName=\"RTC1337\" corporationID=\"154859952\" corporationName=\"TEST..\"/></rowset></key></result><cachedUntil>2011-10-28 11:19:39</cachedUntil></eveapi>";

        private const string CharactersXml =
            "<?xml version='1.0' encoding='UTF-8'?><eveapi version=\"2\"><currentTime>2007-12-12 11:48:50</currentTime><result><rowset name=\"characters\" key=\"characterID\" columns=\"name,characterID,corporationName,corporationID\"><row name=\"Mary\" characterID=\"150267069\" corporationName=\"Starbase Anchoring Corp\" corporationID=\"150279367\" /><row name=\"Marcus\" characterID=\"150302299\" corporationName=\"Marcus Corp\" corporationID=\"150333466\" /><row name=\"Dieniafire\" characterID=\"150340823\" corporationName=\"center for Advanced Studies\" corporationID=\"1000169\" /></rowset></result><cachedUntil>2007-12-12 12:48:50</cachedUntil></eveapi>";

        /// <summary>
        /// A test for processing the result from the AccountStatus method of the EveAPI
        /// </summary>
        [Test]
        public static void AccountStatusTest()
        {
            // setup mock data and parameters.
            var url = new Uri("https://api.eveonline.com/account/AccountStatus.xml.aspx");
            const int characterId = 123456;
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();
            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, AccountStatusXml);

            // create the client to test
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                // call the method
                Task<EveServiceResponse<Account>> asyncTask = client.Account.AccountStatusAsync(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);

                // wait on the task
                asyncTask.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(asyncTask);
                EveServiceResponse<Account> result = asyncTask.Result;
                Assert.AreEqual(new DateTimeOffset(2011, 9, 25, 03, 57, 50, TimeSpan.Zero), result.CacheUntil);
                Assert.AreEqual(new DateTimeOffset(2011, 10, 20, 13, 22, 57, TimeSpan.Zero), result.ResultData.ExpiryDate);
                Assert.AreEqual(new DateTimeOffset(2008, 02, 09, 19, 51, 00, TimeSpan.Zero), result.ResultData.CreateDate);
                Assert.AreEqual(1371, result.ResultData.LogOnCount);
                Assert.AreEqual(TimeSpan.FromMinutes(245488), result.ResultData.LoggedInTime);
            }
        }

        /// <summary>
        /// Test for processing the xml result of the ApiKeyInfo method.
        /// </summary>
        [Test]
        public static void ApiKeyInfoTest()
        {
            // setup mock data and parameters.
            var url = new Uri("https://api.eveonline.com/account/APIKeyInfo.xml.aspx");
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiKeyInfoXml);

            // create the client to test
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                // call the method
                Task<EveServiceResponse<ApiKeyInfo>> asyncTask = client.Account.ApiKeyInfoAsync(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue);

                // wait on the task
                asyncTask.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(asyncTask);
                EveServiceResponse<ApiKeyInfo> result = asyncTask.Result;
                Assert.AreEqual(new DateTimeOffset(2011, 10, 28, 11, 19, 39, TimeSpan.Zero), result.CacheUntil);
                Assert.AreEqual(new DateTimeOffset(2012, 10, 13, 00, 00, 00, TimeSpan.Zero), result.ResultData.Expires);
                Assert.AreEqual(134217727, result.ResultData.AccessMask);
                Assert.AreEqual(ApiKeyType.Account, result.ResultData.ApiType);
                Assert.AreEqual(3, result.ResultData.Characters.Count());
                Assert.IsNotNull(result.ResultData.Characters.FirstOrDefault(item => item.Name == "RTC'3" && item.CharacterId == 154432700 && item.CorporationName == "RTC'3 Corp" && item.CorporationId == 98000179));
            }
        }

        /// <summary>
        /// Tests the processing of the EveAPI characters method.
        /// </summary>
        [Test]
        public static void CharactersTest()
        {
            // setup mock data and parameters.
            var url = new Uri("https://api.eveonline.com/account/Characters.xml.aspx");
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, CharactersXml);

            // create the client to test
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                // call the method
                Task<EveServiceResponse<IEnumerable<AccountCharacter>>> asyncTask = client.Account.CharactersAsync(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue);

                // wait on the task
                asyncTask.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(asyncTask);

                EveServiceResponse<IEnumerable<AccountCharacter>> result = asyncTask.Result;
                Assert.AreEqual(new DateTimeOffset(2007, 12, 12, 12, 48, 50, TimeSpan.Zero), result.CacheUntil);

                Assert.AreEqual(3, result.ResultData.Count());
                Assert.IsNotNull(result.ResultData.FirstOrDefault(item => item.Name == "Mary" && item.CharacterId == 150267069 && item.CorporationName == "Starbase Anchoring Corp" && item.CorporationId == 150279367));
                Assert.IsNotNull(result.ResultData.FirstOrDefault(item => item.Name == "Marcus" && item.CharacterId == 150302299 && item.CorporationName == "Marcus Corp" && item.CorporationId == 150333466));
                Assert.IsNotNull(
                    result.ResultData.FirstOrDefault(item => item.Name == "Dieniafire" && item.CharacterId == 150340823 && item.CorporationName == "center for Advanced Studies" && item.CorporationId == 1000169));
            }
        }
    }
}