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
    /// <summary>The character access masks.</summary>
    [Flags]
    public enum CharacterAccessMasks : long
    {
        /// <summary>The account balances.</summary>
        AccountBalances = 1 << 0,

        /// <summary>The asset list.</summary>
        AssetList = 1 << 1,

        /// <summary>The calendar event attendees.</summary>
        CalendarEventAttendees = 1 << 2,

        /// <summary>The character sheet.</summary>
        CharacterSheet = 1 << 3,

        /// <summary>The contact list.</summary>
        ContactList = 1 << 4,

        /// <summary>The contact notifications.</summary>
        ContactNotifications = 1 << 5,

        /// <summary>The fac war stats.</summary>
        FacWarStats = 1 << 6,

        /// <summary>The industry jobs.</summary>
        IndustryJobs = 1 << 7,

        /// <summary>The kill log.</summary>
        KillLog = 1 << 8,

        /// <summary>The mail bodies.</summary>
        MailBodies = 1 << 9,

        /// <summary>The mailing lists.</summary>
        MailingLists = 1 << 10,

        /// <summary>The mail messages.</summary>
        MailMessages = 1 << 11,

        /// <summary>The market orders.</summary>
        MarketOrders = 1 << 12,

        /// <summary>The medals.</summary>
        Medals = 1 << 13,

        /// <summary>The notifications.</summary>
        Notifications = 1 << 14,

        /// <summary>The notification text.</summary>
        NotificationText = 1 << 15,

        /// <summary>The research.</summary>
        Research = 1 << 16,

        /// <summary>The skill in training.</summary>
        SkillInTraining = 1 << 17,

        /// <summary>The skill queue.</summary>
        SkillQueue = 1 << 18,

        /// <summary>The standings.</summary>
        Standings = 1 << 19,

        /// <summary>The upcoming calendar events.</summary>
        UpcomingCalendarEvents = 1 << 20,

        /// <summary>The wallet journal.</summary>
        WalletJournal = 1 << 21,

        /// <summary>The wallet transactions.</summary>
        WalletTransactions = 1 << 22,

        /// <summary>The character info private.</summary>
        CharacterInfoPrivate = 1 << 23,

        /// <summary>The character info public.</summary>
        CharacterInfoPublic = 1 << 24,

        /// <summary>The account status.</summary>
        AccountStatus = 1 << 25,

        /// <summary>The contracts.</summary>
        Contracts = 1 << 26,
    }

    /// <summary>The corporate access masks.</summary>
    [Flags]
    public enum CorporateAccessMasks : long
    {
        /// <summary>The account balances.</summary>
        AccountBalances = 1 << 0,

        /// <summary>The asset list.</summary>
        AssetList = 1 << 1,

        /// <summary>The member medals.</summary>
        MemberMedals = 1 << 2,

        /// <summary>The corporation sheet.</summary>
        CorporationSheet = 1 << 3,

        /// <summary>The contact list.</summary>
        ContactList = 1 << 4,

        /// <summary>The container log.</summary>
        ContainerLog = 1 << 5,

        /// <summary>The fac war stats.</summary>
        FacWarStats = 1 << 6,

        /// <summary>The industry jobs.</summary>
        IndustryJobs = 1 << 7,

        /// <summary>The kill log.</summary>
        KillLog = 1 << 8,

        /// <summary>The member security.</summary>
        MemberSecurity = 1 << 9,

        /// <summary>The member security log.</summary>
        MemberSecurityLog = 1 << 10,

        /// <summary>The member tracking.</summary>
        MemberTracking = 1 << 11,

        /// <summary>The market orders.</summary>
        MarketOrders = 1 << 12,

        /// <summary>The medals.</summary>
        Medals = 1 << 13,

        /// <summary>The outpost list.</summary>
        OutpostList = 1 << 14,

        /// <summary>The outpost service list.</summary>
        OutpostServiceList = 1 << 15,

        /// <summary>The shareholders.</summary>
        Shareholders = 1 << 16,

        /// <summary>The starbase detail.</summary>
        StarbaseDetail = 1 << 17,

        /// <summary>The standings.</summary>
        Standings = 1 << 18,

        /// <summary>The starbase list.</summary>
        StarbaseList = 1 << 19,

        /// <summary>The wallet journal.</summary>
        WalletJournal = 1 << 20,

        /// <summary>The wallet transactions.</summary>
        WalletTransactions = 1 << 21,

        /// <summary>The titles.</summary>
        Titles = 1 << 22,

        /// <summary>The contracts.</summary>
        Contracts = 1 << 23
    }

    public static class AccessMasks
    {
        public static bool HasCorpPermissions(long accessMask, CorporateAccessMasks reqPermissions)
        {
            return ((CorporateAccessMasks) accessMask & reqPermissions) == reqPermissions;
        }

        public static bool HasCharacterPermissions(long accessMask, CharacterAccessMasks reqPermissions)
        {
            return ((CharacterAccessMasks) accessMask & reqPermissions) == reqPermissions;
        }
    }
}