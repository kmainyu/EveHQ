// ========================================================================
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2012  EveHQ Development Team
//  
//  This file (Contract.cs), is part of EveHQ.
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

    using Newtonsoft.Json;

    /// <summary>Represents a single contract in EVE.</summary>
    public sealed class Contract
    {
        /// <summary>Gets the contract id.</summary>
        public int ContractId { get; set; }

        /// <summary>Gets the issuer id.</summary>
        public int IssuerId { get; set; }

        /// <summary>Gets the issuer corp ID.</summary>
        public int IssuserCorpId { get; set; }

        /// <summary>Gets the assignee id.</summary>
        public int AssigneeId { get; set; }

        /// <summary>Gets the acceptor id.</summary>
        public int AcceptorId { get; set; }

        /// <summary>Gets the start station id.</summary>
        public int StartStationId { get; set; }

        /// <summary>Gets the end station id.</summary>
        public int EndStationId { get; set; }

        /// <summary>Gets the type.</summary>
        public ContractType Type { get; set; }

        /// <summary>Gets the status.</summary>
        public ContractStatus Status { get; set; }

        /// <summary>Gets the title.</summary>
        public string Title { get; set; }

        /// <summary>Gets a value indicating whether for corp.</summary>
        public bool ForCorp { get; set; }

        /// <summary>Gets the availability.</summary>
        public ContractAvailability Availability { get; set; }

        /// <summary>Gets the date issued.</summary>
        public DateTimeOffset DateIssued { get; set; }

        /// <summary>Gets the date expired.</summary>
        public DateTimeOffset DateExpired { get; set; }

        /// <summary>Gets the date accepted.</summary>
        public DateTimeOffset DateAccepted { get; set; }

        /// <summary>Gets the number of days.</summary>
        public int NumberOfDays { get; set; }

        /// <summary>Gets the date completed.</summary>
        public DateTimeOffset DateCompleted { get; set; }

        /// <summary>Gets the price.</summary>
        public double Price { get; set; }

        /// <summary>Gets the reward.</summary>
        public double Reward { get; set; }

        /// <summary>Gets the collateral.</summary>
        public double Collateral { get; set; }

        /// <summary>Gets the buyout.</summary>
        public double Buyout { get; set; }

        /// <summary>Gets the volume.</summary>
        public double Volume { get; set; }
    }
}