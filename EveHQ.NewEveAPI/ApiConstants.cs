// ==============================================================================
// 
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2014  EveHQ Development Team
//   
// This file (ApiConstants.cs), is part of EveHQ.
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

namespace EveHQ.EveApi
{
    /// <summary>
    ///     The api constants.
    /// </summary>
    internal static class ApiConstants
    {
        /// <summary>
        ///     The account id.
        /// </summary>
        public const string AccountId = "accountID";

        /// <summary>
        ///     The account key.
        /// </summary>
        public const string AccountKey = "accountKey";

        /// <summary>
        ///     The balance.
        /// </summary>
        public const string Balance = "balance";

        /// <summary>
        ///     Character ID
        /// </summary>
        public const string CharacterId = "characterID";

        /// <summary>
        ///     The character name.
        /// </summary>
        public const string CharacterName = "characterName";

        /// <summary>
        ///     The corporation id.
        /// </summary>
        public const string CorporationId = "corporationID";

        /// <summary>
        ///     The corporation name.
        /// </summary>
        public const string CorporationName = "corporationName";

        /// <summary>
        ///     The create date.
        /// </summary>
        public const string CreateDate = "createDate";

        /// <summary>
        ///     5 minute cache
        /// </summary>
        public const int FiveMinuteCache = 300;

        /// <summary>
        ///     The key.
        /// </summary>
        public const string Key = "key";

        /// <summary>
        ///     Numerical id of the API key
        /// </summary>
        public const string KeyId = "keyId";

        /// <summary>
        ///     The logon count.
        /// </summary>
        public const string LogonCount = "logonCount";

        /// <summary>
        ///     The logon minutes.
        /// </summary>
        public const string LogonMinutes = "logonMinutes";

        /// <summary>
        ///     The name.
        /// </summary>
        public const string Name = "name";

        /// <summary>
        ///     The paid until.
        /// </summary>
        public const string PaidUntil = "paidUntil";

        /// <summary>
        ///     name of the rowset element.
        /// </summary>
        public const string Rowset = "rowset";

        /// <summary>
        ///     The row.
        /// </summary>
        public const string Row = "row";

        /// <summary>
        ///     1 hour cache
        /// </summary>
        public const int SixtyMinuteCache = 3600;

        /// <summary>
        ///     The user id.
        /// </summary>
        public const string UserId = "userID";

        /// <summary>
        ///     Verification code associated with a keyID.
        /// </summary>
        public const string VCode = "vCode";

        /// <summary>
        ///     The item id.
        /// </summary>
        public const string ItemId = "itemID";

        /// <summary>
        ///     The location id.
        /// </summary>
        public const string LocationId = "locationID";

        /// <summary>
        ///     The type id.
        /// </summary>
        public const string TypeId = "typeID";

        /// <summary>
        ///     The quantity.
        /// </summary>
        public const string Quantity = "quantity";

        public const string RawQuantity = "rawQuantity";

        /// <summary>
        ///     The flag.
        /// </summary>
        public const string Flag = "flag";

        /// <summary>
        ///     The singleton.
        /// </summary>
        public const string Singleton = "singleton";

        /// <summary>
        ///     The six hour cache.
        /// </summary>
        public const int SixHourCache = 21600;

        public const string EventId = "eventIDs";

        public const string Response = "response";


        public const string Ids = "ids";

        public const string Names = "names";
    }
}