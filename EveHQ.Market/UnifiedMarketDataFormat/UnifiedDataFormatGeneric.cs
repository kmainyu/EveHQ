// ===========================================================================
// <copyright file="UnifiedDataFormatBase.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  This file (UnifiedDataFormatBase.cs), is part of EveHQ.
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
    using System.Collections.ObjectModel;

    /// <summary>The Unfied data format for a specific kind of row schema.</summary>
    /// <typeparam name="T">Type that will make up the row sets.</typeparam>
    public class UnifiedDataFormat<T> : UnifiedDataFormat
    {
        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="UnifiedDataFormat{T}" /> class.</summary>
        public UnifiedDataFormat()
        {
            Rowsets = new Collection<T>();
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets the columns.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "will refactor to collection.")]
        public virtual string[] Columns { get; set; }

        /// <summary>Gets or sets the rowsets.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rowsets", Justification = "Expected spelling of output format.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "needed for serialization.")]
        public Collection<T> Rowsets { get; set; }

        #endregion
    }
}