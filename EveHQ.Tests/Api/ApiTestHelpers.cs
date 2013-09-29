//  ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (ApiTestHelpers.cs), is part of EveHQ.
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
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using EveHQ.Caching;
    using EveHQ.EveApi;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    /// Constants used in api tests
    /// </summary>
    internal static class ApiTestHelpers
    {
        public const string EveServiceApiHost = "https://api.eveonline.com";

        public const string KeyId = "keyId";

        public const string KeyIdValue = "12345678";

        public const string VCode = "vCode";

        public const string VCodeValue = "120398475akdsfjhaslfkjhsdfkjhfasdf=";

        public static void BasicSuccessResultValidations<T>(Task<EveServiceResponse<T>> asyncTask)
        {
            // validate return
            Assert.IsFalse(asyncTask.IsFaulted);
            Assert.IsNull(asyncTask.Exception);
            Assert.IsNotNull(asyncTask.Result);

            EveServiceResponse<T> result = asyncTask.Result;

            Assert.IsTrue(result.WasSucessful);
            Assert.IsFalse(result.WasFaulted);
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
            if (!File.Exists(Path.Combine(System.Environment.CurrentDirectory, fileName)))
            {
                return null;
            }

            using (var fs = new FileStream(Path.Combine(System.Environment.CurrentDirectory, fileName), FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new StreamReader(fs))
            {
                return reader.ReadToEnd();
            }
        }

        public static ICacheProvider GetNullCacheProvider()
        {
           
           Mock<ICacheProvider> provider = new Mock<ICacheProvider>();
           return provider.Object;
        }
    }
}