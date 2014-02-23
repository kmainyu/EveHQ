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

namespace EveHQ.EveApi
{
    /// <summary>Represents a single contract in EVE.</summary>
    public sealed class Contract
    {
        /// <summary>Gets the contract id.</summary>
        public long ContractId { get; set; }

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