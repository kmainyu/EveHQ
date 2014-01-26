//  ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (ApiKeyInfo.cs), is part of EveHQ.
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

namespace EveHQ.EveApi
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// Describes the details about an eve api key.
    /// </summary>
    public sealed class ApiKeyInfo
    {
        /// <summary>
        /// Gets the access mask associated with this api key.
        /// </summary>
        public int AccessMask { get; set; }

        /// <summary>
        /// Gets the characters associated with this api key
        /// </summary>
        public IEnumerable<AccountCharacter> Characters { get; set; }

        /// <summary>
        /// Gets the date/time of when this api key expires if at all.
        /// </summary>
        public DateTimeOffset Expires { get; set; }

        /// <summary>
        /// Gets the type of api key this object describes.
        /// </summary>
        public ApiKeyType ApiType { get; set; }
    }
}