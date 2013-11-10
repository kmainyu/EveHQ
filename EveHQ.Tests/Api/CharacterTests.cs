using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveHQ.Tests.Api
{
    using System.Globalization;
    using System.Threading.Tasks;

    using EveHQ.Common;
    using EveHQ.Common.Extensions;
    using EveHQ.EveApi;

    using NUnit.Framework;

    /// <summary>
    /// Unit tests for the character group of methods on the Eve web api.
    /// </summary>
    [TestFixture]
    public static class CharacterTests
    {
        #region test data from eve api documentation

        private const string AssetListXml =
            "<?xml version='1.0' encoding='UTF-8'?><eveapi version=\"2\"><currentTime>2010-10-05 20:28:55</currentTime><result><rowset name=\"assets\" key=\"itemID\" columns=\"itemID,locationID,typeID,quantity,flag,singleton\"><row itemID=\"1810006706\" locationID=\"60001108\" typeID=\"11289\" quantity=\"20\" flag=\"4\" singleton=\"0\" /><row itemID=\"318532021\" locationID=\"60007948\" typeID=\"602\" quantity=\"1\" flag=\"4\" singleton=\"0\" /><row itemID=\"1957020168\" locationID=\"60007954\" typeID=\"16273\" quantity=\"762\" flag=\"4\" singleton=\"0\" /><row itemID=\"235023463\" locationID=\"60008887\" typeID=\"5051\" quantity=\"1\" flag=\"4\" singleton=\"0\" /><row itemID=\"743501315\" locationID=\"60008887\" typeID=\"2865\" quantity=\"2\" flag=\"4\" singleton=\"0\" /><row itemID=\"1365372093\" locationID=\"60008887\" typeID=\"16273\" quantity=\"1000\" flag=\"4\" singleton=\"0\" /><row itemID=\"1526182165\" locationID=\"60008887\" typeID=\"25590\" quantity=\"3\" flag=\"4\" singleton=\"0\" /><row itemID=\"1526774663\" locationID=\"60008887\" typeID=\"15331\" quantity=\"2\" flag=\"4\" singleton=\"0\" /><row itemID=\"1748819720\" locationID=\"60008887\" typeID=\"2205\" quantity=\"1\" flag=\"4\" singleton=\"0\" /><row itemID=\"1748819725\" locationID=\"60008887\" typeID=\"2205\" quantity=\"1\" flag=\"4\" singleton=\"0\" /><row itemID=\"1748819727\" locationID=\"60008887\" typeID=\"2205\" quantity=\"1\" flag=\"4\" singleton=\"0\" /><row itemID=\"1748819730\" locationID=\"60008887\" typeID=\"5281\" quantity=\"1\" flag=\"4\" singleton=\"0\" /><row itemID=\"1748819736\" locationID=\"60008887\" typeID=\"5281\" quantity=\"1\" flag=\"4\" singleton=\"0\" /><row itemID=\"1748819742\" locationID=\"60008887\" typeID=\"25861\" quantity=\"1\" flag=\"4\" singleton=\"0\" /><row itemID=\"1748819746\" locationID=\"60008887\" typeID=\"21096\" quantity=\"1\" flag=\"4\" singleton=\"0\" /><row itemID=\"1748819747\" locationID=\"60008887\" typeID=\"2032\" quantity=\"1\" flag=\"4\" singleton=\"0\" /><row itemID=\"1133149816\" locationID=\"60009544\" typeID=\"16273\" quantity=\"1249\" flag=\"4\" singleton=\"0\" /><row itemID=\"318035361\" locationID=\"60012238\" typeID=\"1405\" quantity=\"1\" flag=\"4\" singleton=\"0\" /><row itemID=\"304013235\" locationID=\"60012241\" typeID=\"641\" quantity=\"1\" flag=\"4\" singleton=\"1\"><rowset name=\"contents\" key=\"itemID\" columns=\"itemID,typeID,quantity,flag,singleton\"><row itemID=\"1037228230\" typeID=\"12807\" quantity=\"1274\" flag=\"5\" singleton=\"0\" /><row itemID=\"1554990313\" typeID=\"23707\" quantity=\"1\" flag=\"87\" singleton=\"1\" /><row itemID=\"1580733649\" typeID=\"2032\" quantity=\"1\" flag=\"22\" singleton=\"1\" /><row itemID=\"1792171545\" typeID=\"26000\" quantity=\"1\" flag=\"93\" singleton=\"1\" /><row itemID=\"1944177945\" typeID=\"26000\" quantity=\"1\" flag=\"94\" singleton=\"1\" /><row itemID=\"1954986037\" typeID=\"23561\" quantity=\"1\" flag=\"87\" singleton=\"1\" /><row itemID=\"1954986033\" typeID=\"23561\" quantity=\"1\" flag=\"87\" singleton=\"1\" /><row itemID=\"1895713388\" typeID=\"23707\" quantity=\"2\" flag=\"87\" singleton=\"0\" /><row itemID=\"1852719739\" typeID=\"3090\" quantity=\"1\" flag=\"27\" singleton=\"1\" /><row itemID=\"1852719737\" typeID=\"3090\" quantity=\"1\" flag=\"28\" singleton=\"1\" /><row itemID=\"1852719731\" typeID=\"3090\" quantity=\"1\" flag=\"30\" singleton=\"1\" /><row itemID=\"1852719695\" typeID=\"3090\" quantity=\"1\" flag=\"29\" singleton=\"1\" /><row itemID=\"1852719691\" typeID=\"3090\" quantity=\"1\" flag=\"31\" singleton=\"1\" /><row itemID=\"1852719688\" typeID=\"3090\" quantity=\"1\" flag=\"32\" singleton=\"1\" /><row itemID=\"1833226118\" typeID=\"3090\" quantity=\"1\" flag=\"33\" singleton=\"1\" /><row itemID=\"1803545835\" typeID=\"29011\" quantity=\"2\" flag=\"5\" singleton=\"0\" /><row itemID=\"1803545726\" typeID=\"29009\" quantity=\"2\" flag=\"5\" singleton=\"0\" /><row itemID=\"1792164501\" typeID=\"4473\" quantity=\"1\" flag=\"34\" singleton=\"1\" /><row itemID=\"1789870235\" typeID=\"12054\" quantity=\"1\" flag=\"19\" singleton=\"1\" /><row itemID=\"1789868853\" typeID=\"10190\" quantity=\"1\" flag=\"13\" singleton=\"1\" /><row itemID=\"1789868845\" typeID=\"10190\" quantity=\"1\" flag=\"14\" singleton=\"1\" /><row itemID=\"1789868005\" typeID=\"1952\" quantity=\"1\" flag=\"20\" singleton=\"1\" /><row itemID=\"1789867916\" typeID=\"1952\" quantity=\"1\" flag=\"21\" singleton=\"1\" /><row itemID=\"1789867102\" typeID=\"14003\" quantity=\"1\" flag=\"16\" singleton=\"1\" /><row itemID=\"1653391398\" typeID=\"21740\" quantity=\"3408\" flag=\"5\" singleton=\"0\" /><row itemID=\"1107135027\" typeID=\"2048\" quantity=\"1\" flag=\"11\" singleton=\"1\" /><row itemID=\"954751134\" typeID=\"23525\" quantity=\"1\" flag=\"87\" singleton=\"1\" /><row itemID=\"954751118\" typeID=\"23525\" quantity=\"1\" flag=\"87\" singleton=\"1\" /><row itemID=\"855392148\" typeID=\"232\" quantity=\"4732\" flag=\"5\" singleton=\"0\" /><row itemID=\"832232191\" typeID=\"23707\" quantity=\"1\" flag=\"87\" singleton=\"1\" /><row itemID=\"832231982\" typeID=\"23707\" quantity=\"1\" flag=\"87\" singleton=\"1\" /><row itemID=\"605338843\" typeID=\"11269\" quantity=\"1\" flag=\"12\" singleton=\"1\" /><row itemID=\"352545357\" typeID=\"8265\" quantity=\"1\" flag=\"17\" singleton=\"1\" /><row itemID=\"202959235\" typeID=\"11323\" quantity=\"1\" flag=\"15\" singleton=\"1\" /></rowset></row></rowset></result><cachedUntil>2010-10-06 19:28:55</cachedUntil></eveapi>";

        private const string AccountBalanceXml = "<?xml version='1.0' encoding='UTF-8'?><eveapi version=\"2\">  <currentTime>2010-10-05 20:28:55</currentTime>  <result>    <rowset name=\"Accounts\" key=\"accountID\" columns=\"accountID,accountKey,balance\" >      <row accountID=\"4807144\" accountKey=\"1000\" balance=\"209127923.31\" /></rowset>  </result>  <cachedUntil>2010-10-05 21:28:55</cachedUntil></eveapi>";

        private const string AttendeeXml =
            "<?xml version='1.0' encoding='UTF-8'?><eveapi><currentTime>2010-12-12 11:49:41</currentTime><result><rowset name=\"eventAttendees\" key=\"characterID\" columns=\"characterID,characterName,response\"><row characterID=\"123456789\" characterName=\"Jane Doe\" response=\"Accepted\" /><row characterID=\"987654321\" characterName=\"John Doe\" response=\"Tentative\" /><row characterID=\"192837645\" characterName=\"Another Doe\" response=\"Declined\" /><row characterID=\"918273465\" characterName=\"Doe the Third\" response=\"Undecided\" /></rowset></result><cachedUntil>2010-12-12 11:59:41</cachedUntil></eveapi>";


        #endregion

        /// <summary>
        /// Tests the asset list processing
        /// </summary>
        [Test]
        public static void AssetListTest()
        {
            // setup mock data and parameters.
            var url = new Uri("https://api.eveonline.com/char/AssetList.xml.aspx");
            const int characterId = 123456;
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();
            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, AssetListXml);

            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                Task<EveServiceResponse<IEnumerable<AssetItem>>> task = client.Character.AssetListAsync(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);
                EveServiceResponse<IEnumerable<AssetItem>> result = task.Result;
                Assert.AreEqual(new DateTimeOffset(2010, 10, 6, 19, 28, 55, TimeSpan.Zero), result.CacheUntil);
                Assert.AreEqual(19, result.ResultData.Count());

            }
        }

        /// <summary>
        /// Test for the processing of account balance requests.
        /// </summary>
        [Test]
        public static void AccountBalanceTest()
        {
            // setup mock data and parameters.
            var url = new Uri("https://api.eveonline.com/char/AccountBalance.xml.aspx");
            const int characterId = 123456;
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();
            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, AccountBalanceXml);


            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                Task<EveServiceResponse<IEnumerable<AccountBalance>>> task = client.Character.AccountBalanceAsync(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);
                EveServiceResponse<IEnumerable<AccountBalance>> result = task.Result;
                Assert.AreEqual(new DateTimeOffset(2010, 10, 5, 21, 28, 55, TimeSpan.Zero), result.CacheUntil);
                Assert.AreEqual(1, result.ResultData.Count());

            }
        }


        [Test]
        public static void CalendarEventAttendeeTest()
        {
            // setup mock data and parameters.
            var url = new Uri("https://api.eveonline.com/char/CalendarEventAttendees.xml.aspx");
            const int characterId = 123456;
            const int eventId = 7890572;
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();
            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));
            data.Add(ApiConstants.EventId, eventId.ToInvariantString());
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, AttendeeXml);

            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.CalendarEventAttendees(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId, eventId);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);
                var result = task.Result;
                var resultData = result.ResultData;
                Assert.AreEqual(4, resultData.Count());
                Assert.AreEqual(192837645, resultData.Skip(2).Take(1).First().CharacterId);
                Assert.AreEqual(AttendeeResponseType.Undecided, resultData.Skip(3).Take(1).First().Response);
            }
        }

        [Test]
        public static void CharacterSheetTest()
        {
            var url = new Uri("https://api.eveonline.com/char/CharacterSheet.xml.aspx");
            const int characterId = 123456;
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();
            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\CharacterSheet.xml"));

            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.CharacterSheetAsync(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);
                var result = task.Result;
                var resultData = result.ResultData;

                // spot check some details.
                Assert.AreEqual(150337897, resultData.CharacterId);
                Assert.AreEqual("corpslave", resultData.Name);
                Assert.AreEqual(new DateTimeOffset(2006, 01, 01, 0, 0, 0, TimeSpan.Zero), resultData.BirthDate);
                Assert.AreEqual("Minmatar", resultData.Race);
                Assert.AreEqual(190210393.87, resultData.Balance);
                Assert.AreEqual(3, resultData.PerceptionBonus.Value);
                Assert.AreEqual(6, resultData.Intelligence);
                Assert.AreEqual(4, resultData.Memory);
                Assert.AreEqual(7, resultData.Charisma);
                Assert.AreEqual(12, resultData.Perception);
                Assert.AreEqual(10, resultData.Willpower);
                Assert.AreEqual(5, resultData.Skills.Count());
                Assert.AreEqual(536500, resultData.Skills.Select(skill => skill.SkillPoints).Sum());
                Assert.AreEqual(7, resultData.Certificates.Count());
                Assert.AreEqual(239, resultData.Certificates.Skip(3).Take(1).First());
                Assert.AreEqual(1, resultData.CorporationRoles.Count());
                Assert.AreEqual(1, resultData.CorporationRolesAtHq.Count());
                Assert.AreEqual(1, resultData.CorporationRolesAtOthers.Count());
                Assert.AreEqual(1, resultData.CorporationRolesAtBase.Count());
            }
        }

        [Test]
        public static void ContactListTest()
        {
            var url = new Uri("https://api.eveonline.com/char/ContactList.xml.aspx");
            const int characterId = 123456;
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();
            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\ContactList.xml"));
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.ContactList(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);
                var result = task.Result;
                var resultData = result.ResultData.ToList();

                Assert.AreEqual(3, resultData.Count());

                // personal contacts
                Assert.AreEqual("contactList", resultData[0].Name);
                Assert.AreEqual(2, resultData[0].Contacts.Count());
                Assert.AreEqual(3010913, resultData[0].Contacts.Take(1).First().ContactId);
                Assert.AreEqual("Hirento Raikkanen", resultData[0].Contacts.Take(1).First().ContactName);
                Assert.AreEqual(false, resultData[0].Contacts.Take(1).First().IsInWatchList);
                Assert.AreEqual(0, resultData[0].Contacts.Take(1).First().Standing);

                Assert.AreEqual(797400947, resultData[0].Contacts.Skip(1).Take(1).First().ContactId);
                Assert.AreEqual("CCP Garthagk", resultData[0].Contacts.Skip(1).Take(1).First().ContactName);
                Assert.AreEqual(true, resultData[0].Contacts.Skip(1).Take(1).First().IsInWatchList);
                Assert.AreEqual(10, resultData[0].Contacts.Skip(1).Take(1).First().Standing);

                // corp contacts
                Assert.AreEqual("corporateContactList", resultData[1].Name);
                Assert.AreEqual(797400947, resultData[1].Contacts.Take(1).First().ContactId);
                Assert.AreEqual("CCP Garthagk", resultData[1].Contacts.Take(1).First().ContactName);
                Assert.AreEqual(false, resultData[1].Contacts.Take(1).First().IsInWatchList);
                Assert.AreEqual(-10, resultData[1].Contacts.Take(1).First().Standing);
            }
        }

        [Test]
        public static void ContactNotificationTest()
        {
            var url = new Uri("https://api.eveonline.com/char/ContactNotifications.xml.aspx");
            const int characterId = 123456;
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();
            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\ContactNotifications.xml"));
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.ContactNotifications(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);
                var result = task.Result;

                Assert.AreEqual(1, result.ResultData.Count());
                Assert.AreEqual(308734131, result.ResultData.First().NotificationId);
                Assert.AreEqual(797400947, result.ResultData.First().SenderId);
                Assert.AreEqual("CCP Garthagk", result.ResultData.First().SenderName);
                Assert.AreEqual(new DateTimeOffset(2010, 05, 29, 23, 04, 00, TimeSpan.Zero), result.ResultData.First().SentDate);

            }
        }

        [Test]
        public static void ContractsTest()
        {
            var url = new Uri("https://api.eveonline.com/char/Contracts.xml.aspx");
            const int characterId = 123456;
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();
            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\Contracts.xml"));
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.Contracts(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);
                var result = task.Result;

                Assert.AreEqual(38, result.ResultData.Count());
                // pick one to examine
                var sample = result.ResultData.Skip(5).Take(1).First();

                Assert.AreEqual(62102990, sample.ContractId);
                Assert.AreEqual(602995120, sample.IssuerId);
                Assert.AreEqual(821674710, sample.IssuserCorpId);
                Assert.AreEqual(ContractType.Courier, sample.Type);
                Assert.AreEqual(ContractStatus.Completed, sample.Status);
                Assert.AreEqual(new DateTimeOffset(2012, 12, 09, 20, 46, 54, TimeSpan.Zero), sample.DateIssued);
            }
        }

        [Test]
        public static void FacWarfareTest()
        {
            var url = new Uri("https://api.eveonline.com/char/FacWarStats.xml.aspx");
            const int characterId = 123456;
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();
            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\FacWarfareData.xml"));
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.FactionalWarfareStatistics(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);
                var result = task.Result;
                Assert.AreEqual(500001, result.ResultData.FactionId);
                Assert.AreEqual(12, result.ResultData.KillsTotal);

            }
        }

        [Test]
        public static void IndustryJobsTest()
        {
            var url = new Uri("https://api.eveonline.com/char/IndustryJobs.xml.aspx");
            const int characterId = 123456;
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();
            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\IndustryJobs.xml"));
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.IndustryJobs(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);
                var result = task.Result;
                Assert.AreEqual(2, result.ResultData.Count());


            }
        }

        [Test]
        public static void MailingListTest()
        {
            var url = new Uri("https://api.eveonline.com/char/mailinglists.xml.aspx");
            const int characterId = 123456;
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();
            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));
            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\Mailinglists.xml"));
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.MailingListsAsync(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();
                ApiTestHelpers.BasicSuccessResultValidations(task);
                var result = task.Result;
                Assert.AreEqual(3, result.ResultData.Count());
                var sample = result.ResultData.Skip(1).Take(1).First();
                Assert.AreEqual(128783669, sample.ListId);
                Assert.AreEqual("EVEMarkerScanner", sample.DisplayName);
            }
        }

        [Test]
        public static void MailMessageTest()
        {
            var url = new Uri("https://api.eveonline.com/char/MailMessages.xml.aspx");
            const int characterId = 123456;
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();

            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\MailMessages.xml"));
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {

                var task = client.Character.MailMessagesAsync(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();
                ApiTestHelpers.BasicSuccessResultValidations(task);
                var result = task.Result;
                Assert.AreEqual(3, result.ResultData.Count());
            }
        }

        [Test]
        public static void MarketOrdersTest()
        {
            var url = new Uri("https://api.eveonline.com/char/MarketOrders.xml.aspx");
            const int characterId = 123456;
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();

            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\MarketOrders.xml"));
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.MarketOrdersAsync(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();
                ApiTestHelpers.BasicSuccessResultValidations(task);
                var result = task.Result;
                Assert.AreEqual(3, result.ResultData.Count());
                var items = result.ResultData.ToList();
                Assert.AreEqual(24, items[1].QuantityRemaining);

            }
        }

        [Test]
        public static void MedalsTest()
        {
            var url = new Uri("https://api.eveonline.com/char/Medals.xml.aspx");
            const int characterId = 123456;
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();

            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\Medals.xml"));
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.Medals(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();
                ApiTestHelpers.BasicSuccessResultValidations(task);
                var result = task.Result;
                Assert.AreEqual(2, result.ResultData.Count());
            }
        }

        [Test]
        public static void NotificationTest()
        {
            var url = new Uri("https://api.eveonline.com/char/Notifications.xml.aspx");
            const int characterId = 123456;
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();

            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\Notifications.xml"));
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.NotificationsAsync(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();
                ApiTestHelpers.BasicSuccessResultValidations(task);
                var result = task.Result;
                Assert.AreEqual(2, result.ResultData.Count());
            }
        }


        [Test]
        public static void NotificationTextsTest()
        {
            var url = new Uri("https://api.eveonline.com/char/NotificationTexts.xml.aspx");
            const int characterId = 123456;
            long[] ids = new[] { 1234L, 5678L };
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();

            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));
            data.Add(ApiConstants.Ids, string.Join(",", ids.Select(id => id.ToInvariantString())));

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\NotificationTexts.xml"));

            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.NotificationTextsAsync(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId, ids);
                task.Wait();
                ApiTestHelpers.BasicSuccessResultValidations(task);
                var result = task.Result.ResultData.ToList();
                Assert.AreEqual(5, result.Count);
                Assert.IsTrue(!result[2].Text.IsNullOrWhiteSpace());

            }
        }



        [Test]
        public static void ResearchTest()
        {
            var url = new Uri("https://api.eveonline.com/char/Research.xml.aspx");
            const int characterId = 123456;
            
            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();

            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));


            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\Research.xml"));

            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.Research(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();
                ApiTestHelpers.BasicSuccessResultValidations(task);
                var result = task.Result.ResultData.ToList();
                Assert.AreEqual(4, result.Count);
                Assert.IsTrue(result[1].AgentId == 3011154);
                Assert.IsTrue(result[1].SkillTypeId == 11452);
                Assert.IsTrue(double.Equals(result[1].RemainingPoints,33.0962187499972));
                Assert.IsTrue(double.Equals(result[1].PointsPerDay, 65.66));
                Assert.IsTrue(result[1].ResearchStartDate == new DateTimeOffset(2009,09,02,06,49,35,TimeSpan.Zero));
            }
        }

        [Test]
        public static void SkillInTrainingTest()
        {
            var url = new Uri("https://api.eveonline.com/char/SkillInTraining.xml.aspx");

            const int characterId = 123456;

            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();

            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));


            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\SkillInTraining.xml"));

            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.SkillInTraining(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();
                ApiTestHelpers.BasicSuccessResultValidations(task);

                Assert.IsNotNull(task.Result);
                Assert.IsNotNull(task.Result.ResultData);
                var skill = task.Result.ResultData;
                Assert.AreEqual(new DateTimeOffset(2008, 08, 15, 04, 01, 16, TimeSpan.Zero), skill.TrainingStartTime);
                Assert.AreEqual(new DateTimeOffset(2008, 08, 17, 15, 29, 44, TimeSpan.Zero), skill.TrainingEndTime);
                Assert.AreEqual(new DateTimeOffset(2008, 08, 17, 06, 43, 00, TimeSpan.Zero), skill.CurrentTQTime);
                Assert.AreEqual(3305, skill.TrainingTypeId);
                Assert.AreEqual(24000, skill.TrainingStartSP);
                Assert.AreEqual(135765, skill.TrainingEndSP);
                Assert.AreEqual(4, skill.TrainingToLevel);
                Assert.AreEqual(true, skill.IsTraining);
            }
        }

        [Test]
        public static void SkillQueueTest()
        {
            var url = new Uri("https://api.eveonline.com/char/SkillQueue.xml.aspx");

            const int characterId = 123456;

            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();

            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\SkillQueue.xml"));

            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.SkillQueue(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();
                ApiTestHelpers.BasicSuccessResultValidations(task);

                Assert.IsNotNull(task.Result);
                Assert.IsNotEmpty(task.Result.ResultData);
                var skillQueue = task.Result.ResultData.ToList();
                Assert.AreEqual(2, skillQueue.Count);

                var skill = skillQueue[1];

                Assert.AreEqual(2, skill.QueuePosition);
                Assert.AreEqual(20533, skill.TypeId);
                Assert.AreEqual(4, skill.Level);
                Assert.AreEqual(112000, skill.StartSP);
                Assert.AreEqual(633542, skill.EndSP);
                Assert.AreEqual(new DateTimeOffset(2009, 03, 18, 15, 19, 21, TimeSpan.Zero), skill.StartTime);
                Assert.AreEqual(new DateTimeOffset(2009, 03, 30, 03, 16, 14, TimeSpan.Zero), skill.EndTime);
            }
        }

        [Test]
        public static void NPCStandingsTest()
        {
            var url = new Uri("https://api.eveonline.com/char/Standings.xml.aspx");

            const int characterId = 123456;

            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();

            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\NPCStandings.xml"));

            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.NPCStandings(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();
                ApiTestHelpers.BasicSuccessResultValidations(task);

                Assert.IsNotNull(task.Result);
                Assert.IsNotEmpty(task.Result.ResultData);
                var standings = task.Result.ResultData.ToList();
                Assert.AreEqual(7, standings.Count);

                var standing = standings[3];

                Assert.AreEqual(NpcType.NPCCorporations, standing.Kind);
                Assert.AreEqual(1000064, standing.FromId);
                Assert.AreEqual("Carthum Conglomerate", standing.FromName);
                Assert.AreEqual(0.34, standing.Standing);
                
            }
        }

        [Test]
        public static void UpcomingCalendarEvents()
        {
            var url = new Uri("https://api.eveonline.com/char/UpcomingCalendarEvents.xml.aspx");

            const int characterId = 123456;

            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();

            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\UpcomingCalendarEvents.xml"));

            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.UpcomingCalendarEvents(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();
                ApiTestHelpers.BasicSuccessResultValidations(task);

                Assert.IsNotNull(task.Result);
                Assert.IsNotEmpty(task.Result.ResultData);
                var events = task.Result.ResultData.ToList();
                Assert.AreEqual(1, events.Count);

                var newEvent = events[0];

                
                Assert.AreEqual(93264, newEvent.EventId);
                Assert.AreEqual(1, newEvent.OwnerId);
                Assert.AreEqual(new DateTimeOffset(2011,03,26,09,00,00,TimeSpan.Zero), newEvent.EventDate);
                Assert.AreEqual("EVE Online Fanfest 2011", newEvent.EventTitle);
                Assert.AreEqual(TimeSpan.Zero, newEvent.Duration);
                Assert.AreEqual(false, newEvent.IsImportant);
                Assert.AreEqual("Undecided", newEvent.Response);
                Assert.AreEqual("Join us for <a href=\"http://fanfest.eveonline.com/\"> EVE Online's Fanfest 2011</a>!", newEvent.EventText);
            }
        }

        [Test]
        public static void WalletJournalTest()
        {
            var url = new Uri("https://api.eveonline.com/char/WalletJournal.xml.aspx");

            const int characterId = 123456;

            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();

            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\WalletJournal.xml"));

            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.WalletJournal(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();
                ApiTestHelpers.BasicSuccessResultValidations(task);

                Assert.IsNotNull(task.Result);
                Assert.IsNotEmpty(task.Result.ResultData);
                var entries = task.Result.ResultData.ToList();
                Assert.AreEqual(10, entries.Count);

                var entry = entries[8];


                Assert.AreEqual(new DateTimeOffset(2008,08,20,05,19,00,TimeSpan.Zero), entry.Date);
                Assert.AreEqual(1572531630, entry.RefId);
                Assert.AreEqual(33, entry.ReferenceType);
                Assert.AreEqual("anonymous", entry.FirstPartyName);
                Assert.AreEqual(30497503, entry.FirstPartyId);
                Assert.AreEqual("corpslave", entry.SecondPartyName);
                Assert.AreEqual(150337897, entry.SecondPartyId);
                Assert.IsNullOrEmpty(entry.ArgumentName);
                Assert.AreEqual(30497503, entry.ArgumentId);
                Assert.AreEqual(945000.00, entry.Amount);
                Assert.AreEqual(610237267.52, entry.Balance);
            }
        }

        [Test]
        public static void WalletTransactionTest()
        {
            var url = new Uri("https://api.eveonline.com/char/WalletTransactions.xml.aspx");

            const int characterId = 123456;

            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();

            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\WalletTransactions.xml"));

            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.WalletTransactions(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId);
                task.Wait();
                ApiTestHelpers.BasicSuccessResultValidations(task);

                Assert.IsNotNull(task.Result);
                Assert.IsNotEmpty(task.Result.ResultData);
                var entries = task.Result.ResultData.ToList();
                Assert.AreEqual(7, entries.Count);

                var entry = entries[6];


                Assert.AreEqual(new DateTimeOffset(2010, 01, 29, 15, 45, 00, TimeSpan.Zero), entry.TransactionDateTime);
                Assert.AreEqual(1298649939, entry.TransactionId);
                Assert.AreEqual(1, entry.Quantity);
                Assert.AreEqual("Heavy Missile Launcher II", entry.TypeName);
                Assert.AreEqual(2410, entry.TypeId);
                Assert.AreEqual(556001.01, entry.Price);

                Assert.AreEqual(1703231064, entry.ClientId);
                Assert.AreEqual("Der Suchende", entry.ClientName);
                Assert.AreEqual(60004369, entry.StationId);

                Assert.AreEqual("Ohmahailen V - Moon 7 - Corporate Police Force Assembly Plant", entry.StationName);
                Assert.AreEqual("buy", entry.TransactionType);
                Assert.AreEqual("personal", entry.TransactionFor);

            }
        }

        [Test]
        public static void ContractItemsTest()
        {
            var url = new Uri("https://api.eveonline.com/char/ContractItems.xml.aspx");

            const int characterId = 123456;
            const int contractId = 7890;

            Dictionary<string, string> data = ApiTestHelpers.GetBaseTestParams();

            data.Add(ApiConstants.CharacterId, characterId.ToString(CultureInfo.InvariantCulture));
            data.Add("contractID", contractId.ToString(CultureInfo.InvariantCulture));

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\ContractItems.xml"));

            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Character.ContractItems(ApiTestHelpers.KeyIdValue, ApiTestHelpers.VCodeValue, characterId, contractId);
                task.Wait();
                ApiTestHelpers.BasicSuccessResultValidations(task);

                Assert.IsNotNull(task.Result);
                Assert.IsNotEmpty(task.Result.ResultData);
                var items = task.Result.ResultData.ToList();
                Assert.AreEqual(1, items.Count);

                var item = items[0];


                
                Assert.AreEqual(600515136, item.RecordId);

                Assert.AreEqual(12067, item.TypeId);
                Assert.AreEqual(1, item.Quantity);
                Assert.AreEqual(-1, item.RawQuantity);
                Assert.AreEqual(false, item.IsSingleton);
                Assert.AreEqual(true, item.IsIncluded);

                

            }
        }
    }
}
