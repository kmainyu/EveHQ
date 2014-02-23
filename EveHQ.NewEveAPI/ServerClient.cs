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

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using EveHQ.Caching;
using EveHQ.Common;
using EveHQ.Common.Extensions;

namespace EveHQ.EveApi
{
    public class ServerClient : BaseApiClient
    {
        /// <summary>The request prefix.</summary>
        private const string RequestPrefix = "/server";

        /// <summary>Initializes a new instance of the EveClient class.</summary>
        /// <param name="eveServiceLocation">location of the eve API web service</param>
        /// <param name="cacheProvider">root folder used for caching.</param>
        /// <param name="requestProvider">Request provider to use for this instance.</param>
        internal ServerClient(string eveServiceLocation, ICacheProvider cacheProvider,
            IHttpRequestProvider requestProvider)
            : base(eveServiceLocation, cacheProvider, requestProvider)
        {
        }

        public EveServiceResponse<ServerStatus> ServerStatus(ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(ServerStatusAsync, responseMode);
        }

        public Task<EveServiceResponse<ServerStatus>> ServerStatusAsync(ResponseMode responseMode = ResponseMode.Normal)
        {
            const string MethodPath = "{0}/ServerStatus.xml.aspx";
            const string CacheKeyFormat = "ServerStatus";

            string cacheKey = CacheKeyFormat.FormatInvariant();

            IDictionary<string, string> apiParams = new Dictionary<string, string>();

            return GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey,
                ApiConstants.SixtyMinuteCache, responseMode, ParseServerStatusResponse);
        }


        private static ServerStatus ParseServerStatusResponse(XElement result)
        {
            if (result == null)
            {
                return null; // return null... no data.
            }

            bool online = result.Element("serverOpen").Value.ToBoolean();
            int players = result.Element("onlinePlayers").Value.ToInt32();

            return new ServerStatus {OnlinePlayers = players, IsServerOpen = online};
        }
    }
}