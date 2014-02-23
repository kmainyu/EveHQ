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
using System.IO;
using System.Threading.Tasks;
using EveHQ.Caching;
using EveHQ.EveApi;
using Moq;
using NUnit.Framework;

namespace EveHQ.Tests.Api
{
    /// <summary>
    ///     Constants used in api tests
    /// </summary>
    internal static class ApiTestHelpers
    {
        public const string EveServiceApiHost = "https://api.eveonline.com";

        public const string KeyId = "keyId";

        public const string KeyIdValue = "12345678";

        public const string VCode = "vCode";

        public const string VCodeValue = "120398475akdsfjhaslfkjhsdfkjhfasdf=";

        public static void BasicSuccessResultValidations<T>(Task<EveServiceResponse<T>> asyncTask) where T : class
        {
            // validate return
            Assert.IsFalse(asyncTask.IsFaulted);
            Assert.IsNull(asyncTask.Exception);
            Assert.IsNotNull(asyncTask.Result);

            EveServiceResponse<T> result = asyncTask.Result;

            Assert.IsTrue(result.IsSuccessfulHttpStatus);
            Assert.IsFalse(result.IsFaulted);
            Assert.IsNull(result.ServiceException);
            Assert.IsFalse(result.CachedResponse);
        }

        public static Dictionary<string, string> GetBaseTestParams()
        {
            var paramData = new Dictionary<string, string>();
            paramData.Add(VCode, VCodeValue);
            paramData.Add(KeyId, KeyIdValue);

            return paramData;
        }

        public static string GetXmlData(string fileName)
        {
            if (!File.Exists(Path.Combine(Environment.CurrentDirectory, fileName)))
            {
                return null;
            }

            using (
                var fs = new FileStream(Path.Combine(Environment.CurrentDirectory, fileName), FileMode.Open,
                    FileAccess.Read, FileShare.Read))
            using (var reader = new StreamReader(fs))
            {
                return reader.ReadToEnd();
            }
        }

        public static ICacheProvider GetNullCacheProvider()
        {
            var provider = new Mock<ICacheProvider>();
            return provider.Object;
        }
    }
}