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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EveHQ.Common;
using Moq;

namespace EveHQ.Tests.Api
{
    /// <summary>
    ///     Extensions for generating mocked instances of the web request provider for unit testing Eve web service client
    ///     methods.
    /// </summary>
    public static class MockRequests
    {
        /// <summary>
        ///     Generates a mocked instance of the request provider
        /// </summary>
        /// <param name="expectedUrl">what the expected service url should be generated as by the client object </param>
        /// <param name="expectedParameters">expected parameter key/value collection that should be sent</param>
        /// <param name="mockResponseContent">The response message that should be returned for processing</param>
        /// <returns>a mocked instance of the WebRequestProvider type.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static IHttpRequestProvider GetMockedProvider(Uri expectedUrl,
            IDictionary<string, string> expectedParameters, string mockResponseContent)
        {
            var mockProvider = new Mock<IHttpRequestProvider>();

            mockProvider.Setup(
                m => m.PostAsync(
                    // validate that we are called with expected values from the api client
                    It.Is<Uri>(uri => uri == expectedUrl),
                    It.Is<IDictionary<string, string>>(
                        data =>
                            data.All(
                                kvp =>
                                    expectedParameters.ContainsKey(kvp.Key) && expectedParameters[kvp.Key] == kvp.Value))))
                // return the mocked data in a task
                .Returns(
                    () =>
                        Task.Factory.StartNew(
                            () =>
                                new HttpResponseMessage(HttpStatusCode.OK)
                                {
                                    Content = new StringContent(mockResponseContent)
                                }));

            return mockProvider.Object;
        }
    }
}