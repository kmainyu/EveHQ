//  ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (MockRequests.cs), is part of EveHQ.
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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using EveHQ.Common;
    using EveHQ.EveApi;

    using Moq;

    /// <summary>
    /// Extensions for generating mocked instances of the web request provider for unit testing Eve web service client methods.
    /// </summary>
    public static class MockRequests
    {
        /// <summary>
        /// Generates a mocked instance of the request provider
        /// </summary>
        /// <param name="expectedUrl">what the expected service url should be generated as by the client object </param>
        /// <param name="expectedParameters">expected parameter key/value collection that should be sent</param>
        /// <param name="mockResponseContent">The response message that should be returned for processing</param>
        /// <returns>a mocked instance of the WebRequestProvider type.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static IHttpRequestProvider GetMockedProvider(Uri expectedUrl, IDictionary<string, string> expectedParameters, string mockResponseContent)
        {
            var mockProvider = new Mock<IHttpRequestProvider>();

            mockProvider.Setup(
                m => m.PostAsync(
                    // validate that we are called with expected values from the api client
                    It.Is<Uri>(uri => uri == expectedUrl), It.Is<IDictionary<string, string>>(data => data.All(kvp => expectedParameters.ContainsKey(kvp.Key) && expectedParameters[kvp.Key] == kvp.Value))))
                // return the mocked data in a task
                        .Returns(() => Task.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(mockResponseContent) }));

            return mockProvider.Object;
        }
    }
}