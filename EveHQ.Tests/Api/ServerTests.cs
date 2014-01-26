using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveHQ.Tests.Api
{
    using EveHQ.Common;
    using EveHQ.EveApi;

    using NUnit.Framework;

    [TestFixture]
    public static class ServerTests
    {



        [Test]
        public static void ServerStatusTest()
        {
            var url = new Uri("https://api.eveonline.com/server/ServerStatus.xml.aspx");
            Dictionary<string, string> data = new Dictionary<string, string>();

            IHttpRequestProvider mockProvider = MockRequests.GetMockedProvider(url, data, ApiTestHelpers.GetXmlData("TestData\\Api\\ServerStatus.xml"));
            using (var client = new EveAPI(ApiTestHelpers.EveServiceApiHost, ApiTestHelpers.GetNullCacheProvider(), mockProvider))
            {
                var task = client.Server.ServerStatusAsync();
                task.Wait();

                ApiTestHelpers.BasicSuccessResultValidations(task);

                var result = task.Result.ResultData;

                Assert.IsTrue(result.IsServerOpen);
                Assert.AreEqual(38102, result.OnlinePlayers);
            }
        }
    }
}
