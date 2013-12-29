// -----------------------------------------------------------------------
// <copyright file="EveTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EveHQ.Tests.Api
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using EveHQ.Common;
    using EveHQ.EveApi;

    using NUnit.Framework;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestFixture]
    public static class EveTests
    {
        [Test]
        public static void CharacterNameTest()
        {
            // setup mock data and parameters.
            var url = new Uri("https://api.eveonline.com/eve/CharacterName.xml.aspx");
            var ids = new[] { 797400947L, 1188435724L };
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ApiConstants.Ids, string.Join(",", ids));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\CharacterName.xml"));

            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var result = client.Eve.CharacterName(ids);
                
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
            var ids = new[] { 797400947L, 1188435724L };
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ApiConstants.Ids, string.Join(",", ids));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\CharacterName.xml"));

            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Eve.CharacterNameAsync(ids);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                var result = task.Result;
                
                Assert.AreEqual(2, result.ResultData.Count());
                Assert.AreEqual("CCP Prism X", result.ResultData.Skip(1).First().Name);
            }
        }

        [Test]
        public static void CharacterIdTest()
        {
            var url = new Uri("https://api.eveonline.com/eve/CharacterID.xml.aspx");
         
            var names = new[] {"CCP Garthahk" };
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ApiConstants.Names, string.Join(",", names));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\CharacterId.xml"));

            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Eve.CharacterIdAsync(names);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                var result = task.Result;

                Assert.AreEqual(1, result.ResultData.Count());
                Assert.AreEqual("CCP Garthagk", result.ResultData.First().Name);
                Assert.AreEqual(797400947, result.ResultData.First().Id);
            }
        }


        [Test]
        public static void BasicErrorParsingTest()
        {
            var url = new Uri("https://api.eveonline.com/eve/CharacterID.xml.aspx");

            var names = new[] { "CCP Garthahk" };
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ApiConstants.Names, string.Join(",", names));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\GenericError.xml"));

            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Eve.CharacterIdAsync(names);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                var result = task.Result;

                Assert.AreEqual(222, result.EveErrorCode);
                Assert.AreEqual("Key has expired. Contact key owner for access renewal.", result.EveErrorText);
            }
        }

        [Test]
        public static void CharacterInfoTest()
        {
            var url = new Uri("https://api.eveonline.com/eve/CharacterInfo.xml.aspx");

            
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(ApiConstants.CharacterId, "1643072492");
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\CharacterInfo.xml"));
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Eve.CharacterInfoAsync(1643072492);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                var result = task.Result;
                Assert.AreEqual("Catari Taga", result.ResultData.CharacterName);

                Assert.AreEqual(1923227030, result.ResultData.AllianceId);

                Assert.AreEqual(0.0, result.ResultData.SecurityStatus);

            }
        }

        [Test]
        public static void AllianceListTest()
        {
            var url = new Uri("https://api.eveonline.com/eve/AllianceList.xml.aspx");
            Dictionary<string, string> data = new Dictionary<string, string>();

               IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\AllianceList.xml"));
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Eve.AllianceListAsync();
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                var result = task.Result.ResultData.ToList();

                var rabbits = result[1];

                Assert.AreEqual("The Dead Rabbits", rabbits.Name);
                Assert.AreEqual("TL.DR", rabbits.ShortName);
                Assert.AreEqual(1, rabbits.MemberCorps.Count());
            }
        }

        [Test]
        public static void RefTypeTest()
        {
            var url = new Uri("https://api.eveonline.com/eve/RefTypes.xml.aspx");
            Dictionary<string, string> data = new Dictionary<string, string>();

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\RefTypes.xml"));
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Eve.RefTypesAsync();
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                var result = task.Result.ResultData.ToList();

                var rabbits = result[21];

                Assert.AreEqual("Mission Completion", rabbits.Name);
                Assert.AreEqual(21, rabbits.Id);
            }
        }

        [Test]
        public static void SkillTreeTest()
        {
            var url = new Uri("https://api.eveonline.com/eve/SkillTree.xml.aspx");
            Dictionary<string, string> data = new Dictionary<string, string>();

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\SkillTree.xml"));
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Eve.SkillTreeAsync();
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                var result = task.Result.ResultData.ToList();

                Assert.AreEqual(3, result[0].Skills.ToList()[0].Rank);
                Assert.AreEqual(true, result[0].Skills.ToList()[0].CannotBeTrainedOnTrial);

                Assert.AreEqual(3, result[0].Skills.ToList()[1].RequiredSkills.ToList()[1].Level);
            }
        }


        [Test]
        public static void ConquerableStationListTest()
        {
            var url = new Uri("https://api.eveonline.com/eve/ConquerableStationList.xml.aspx");
            Dictionary<string, string> data = new Dictionary<string, string>();

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\ConquerableStations.xml"));
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Eve.ConquerableStationListAsync();
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                var result = task.Result.ResultData.ToList();

                Assert.AreEqual(60014862, result[0].Id);
            }
        }
    }
}
 