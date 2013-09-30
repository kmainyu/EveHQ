// ===========================================================================
// <copyright file="UnifiedDataRowset.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (UnifiedDataRowset.cs), is part of EveHQ.
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 2 of the License, or
//  (at your option) any later version.
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with EveHQ.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// ============================================================================
namespace EveHQ.Market.UnifiedMarketDataFormat
{
    using System.Collections.Generic;

    /// <summary>Rowset for a specific type of rows.</summary>
    /// <typeparam name="T">Format of the rows.</typeparam>
    public abstract class UnifiedDataRowset<T>
    {
        #region Public Properties

        /// <summary>Gets or sets the generated at.</summary>
        public string GeneratedAt { get; set; }

        /// <summary>Gets or sets the region id.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID", Justification = "output format expects this casing.")]
        public string RegionID { get; set; }

        /// <summary>Gets or sets the rows.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Need AddRange method.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Required for serialization operations.")]
        public List<T> Rows { get; set; }

        /// <summary>Gets or sets the type id.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ID", Justification = "output format expects this casing.")]
        public string TypeID { get; set; }

        #endregion
    }
}