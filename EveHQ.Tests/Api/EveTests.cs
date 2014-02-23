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
using EveHQ.Common;
using EveHQ.EveApi;
using NUnit.Framework;

namespace EveHQ.Tests.Api
{
    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    [TestFixture]
    public static class EveTests
    {
        [Test]
        public static void AllianceListTest()
        {
            var url = new Uri("https://api.eveonline.com/eve/AllianceList.xml.aspx");
            var data = new Dictionary<string, string>();

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data,
                ApiTestHelpers.GetXmlData("TestData\\Api\\AllianceList.xml"));
            using (
                var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(),
                    mockProvider))
            {
                Task<EveServiceResponse<IEnumerable<AllianceData>>> task = client.Eve.AllianceListAsync();
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                List<AllianceData> result = task.Result.ResultData.ToList();

                AllianceData rabbits = result[1];

                Assert.AreEqual("The Dead Rabbits", rabbits.Name);
                Assert.AreEqual("TL.DR", rabbits.ShortName);
                Assert.AreEqual(1, rabbits.MemberCorps.Count());
            }
        }

        [Test]
        public static void BasicErrorParsingTest()
        {
            var url = new Uri("https://api.eveonline.com/eve/CharacterID.xml.aspx");

            var names = new[] {"CCP Garthahk"};
            var data = new Dictionary<string, string>();
            data.Add(ApiConstants.Names, string.Join(",", names));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data,
                ApiTestHelpers.GetXmlData("TestData\\Api\\GenericError.xml"));

            using (
                var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(),
                    mockProvider))
            {
                Task<EveServiceResponse<IEnumerable<CharacterName>>> task = client.Eve.CharacterIdAsync(names);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                EveServiceResponse<IEnumerable<CharacterName>> result = task.Result;

                Assert.AreEqual(222, result.EveErrorCode);
                Assert.AreEqual("Key has expired. Contact key owner for access renewal.", result.EveErrorText);
            }
        }

        [Test]
        public static void CharacterIdTest()
        {
            var url = new Uri("https://api.eveonline.com/eve/CharacterID.xml.aspx");

            var names = new[] {"CCP Garthahk"};
            var data = new Dictionary<string, string>();
            data.Add(ApiConstants.Names, string.Join(",", names));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data,
                ApiTestHelpers.GetXmlData("TestData\\Api\\CharacterId.xml"));

            using (
                var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(),
                    mockProvider))
            {
                Task<EveServiceResponse<IEnumerable<CharacterName>>> task = client.Eve.CharacterIdAsync(names);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                EveServiceResponse<IEnumerable<CharacterName>> result = task.Result;

                Assert.AreEqual(1, result.ResultData.Count());
                Assert.AreEqual("CCP Garthagk", result.ResultData.First().Name);
                Assert.AreEqual(797400947, result.ResultData.First().Id);
            }
        }

        [Test]
        public static void CharacterInfoTest()
        {
            var url = new Uri("https://api.eveonline.com/eve/CharacterInfo.xml.aspx");


            var data = new Dictionary<string, string>();
            data.Add(ApiConstants.CharacterId, "1643072492");
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data,
                ApiTestHelpers.GetXmlData("TestData\\Api\\CharacterInfo.xml"));
            using (
                var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(),
                    mockProvider))
            {
                Task<EveServiceResponse<CharacterInfo>> task = client.Eve.CharacterInfoAsync(1643072492);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                EveServiceResponse<CharacterInfo> result = task.Result;
                Assert.AreEqual("Catari Taga", result.ResultData.CharacterName);

                Assert.AreEqual(1923227030, result.ResultData.AllianceId);

                Assert.AreEqual(0.0, result.ResultData.SecurityStatus);
            }
        }

        [Test]
        public static void CharacterNameTest()
        {
            // setup mock data and parameters.
            var url = new Uri("https://api.eveonline.com/eve/CharacterName.xml.aspx");
            var ids = new[] {797400947L, 1188435724L};
            var data = new Dictionary<string, string>();
            data.Add(ApiConstants.Ids, string.Join(",", ids));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data,
                ApiTestHelpers.GetXmlData("TestData\\Api\\CharacterName.xml"));

            using (
                var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(),
                    mockProvider))
            {
                EveServiceResponse<IEnumerable<CharacterName>> result = client.Eve.CharacterName(ids);

                Assert.IsTrue(result.IsSuccessfulHttpStatus);
                Assert.IsFalse(result.IsFaulted);
                Assert.IsNull(result.ServiceException);
                Assert.IsFalse(result.CachedResponse);

                Assert.AreEqual(2, result.ResultData.Count());
                Assert.AreEqual("CCP Prism X", result.ResultData.Skip(1).First().Name);
            }
        }

        [Test]
        public static void CharacterNameTestAsyncTest()
        {
            // setup mock data and parameters.
            var url = new Uri("https://api.eveonline.com/eve/CharacterName.xml.aspx");
            var ids = new[] {797400947L, 1188435724L};
            var data = new Dictionary<string, string>();
            data.Add(ApiConstants.Ids, string.Join(",", ids));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data,
                ApiTestHelpers.GetXmlData("TestData\\Api\\CharacterName.xml"));

            using (
                var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(),
                    mockProvider))
            {
                Task<EveServiceResponse<IEnumerable<CharacterName>>> task = client.Eve.CharacterNameAsync(ids);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                EveServiceResponse<IEnumerable<CharacterName>> result = task.Result;

                Assert.AreEqual(2, result.ResultData.Count());
                Assert.AreEqual("CCP Prism X", result.ResultData.Skip(1).First().Name);
            }
        }

        [Test]
        public static void ConquerableStationListTest()
        {
            var url = new Uri("https://api.eveonline.com/eve/ConquerableStationList.xml.aspx");
            var data = new Dictionary<string, string>();

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data,
                ApiTestHelpers.GetXmlData("TestData\\Api\\ConquerableStations.xml"));
            using (
                var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(),
                    mockProvider))
            {
                Task<EveServiceResponse<IEnumerable<ConquerableStation>>> task =
                    client.Eve.ConquerableStationListAsync();
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                List<ConquerableStation> result = task.Result.ResultData.ToList();

                Assert.AreEqual(60014862, result[0].Id);
            }
        }

        [Test]
        public static void RefTypeTest()
        {
            var url = new Uri("https://api.eveonline.com/eve/RefTypes.xml.aspx");
            var data = new Dictionary<string, string>();

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data,
                ApiTestHelpers.GetXmlData("TestData\\Api\\RefTypes.xml"));
            using (
                var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(),
                    mockProvider))
            {
                Task<EveServiceResponse<IEnumerable<RefType>>> task = client.Eve.RefTypesAsync();
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                List<RefType> result = task.Result.ResultData.ToList();

                RefType rabbits = result[21];

                Assert.AreEqual("Mission Completion", rabbits.Name);
                Assert.AreEqual(21, rabbits.Id);
            }
        }

        [Test]
        public static void SkillTreeTest()
        {
            var url = new Uri("https://api.eveonline.com/eve/SkillTree.xml.aspx");
            var data = new Dictionary<string, string>();

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data,
                ApiTestHelpers.GetXmlData("TestData\\Api\\SkillTree.xml"));
            using (
                var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(),
                    mockProvider))
            {
                Task<EveServiceResponse<IEnumerable<SkillGroup>>> task = client.Eve.SkillTreeAsync();
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                List<SkillGroup> result = task.Result.ResultData.ToList();

                Assert.AreEqual(3, result[0].Skills.ToList()[0].Rank);
                Assert.AreEqual(true, result[0].Skills.ToList()[0].CannotBeTrainedOnTrial);

                Assert.AreEqual(3, result[0].Skills.ToList()[1].RequiredSkills.ToList()[1].Level);
            }
        }
    }
}